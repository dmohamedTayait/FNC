﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;

namespace NotesFor.HtmlToOpenXml
{
	using a = DocumentFormat.OpenXml.Drawing;
	using pic = DocumentFormat.OpenXml.Drawing.Pictures;
	using wp = DocumentFormat.OpenXml.Drawing.Wordprocessing;


	/// <summary>
	/// Helper class to convert some Html text to OpenXml elements.
	/// </summary>
	public partial class HtmlConverter
	{
		/// <summary>
		/// Occurs when an image tag was detected and you want to manage yourself the download of the data.
		/// </summary>
		public event EventHandler<ProvisionImageEventArgs> ProvisionImage;

		sealed class CachedImagePart
		{
			public ImagePart Part;
			public Int32 Width;
			public Int32 Height;
		}

        private MainDocumentPart mainPart;

		/// <summary>The list of paragraphs that will be returned.</summary>
		private IList<OpenXmlCompositeElement> paragraphs;
		/// <summary>Holds the elements to append to the current paragraph.</summary>
		private List<OpenXmlElement> elements;
		private Paragraph currentParagraph;
		private Int32 footnotesRef=1, endnotesRef=1, figCaptionRef=-1;
		private Dictionary<String, Action<HtmlEnumerator>> knownTags;
		private Dictionary<Uri, CachedImagePart> knownImageParts;
		private List<String> bookmarks;
		private TableContext tables;
		private HtmlDocumentStyle htmlStyles;
		private uint drawingObjId, imageObjId = UInt32.MinValue;
		private Uri baseImageUri;



		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="mainPart">The mainDocumentPart of a document where to write the conversion to.</param>
		/// <remarks>We preload some configuration from inside the document such as style, bookmarks,...</remarks>
		public HtmlConverter(MainDocumentPart mainPart)
		{
			this.mainPart = mainPart;
			this.RenderPreAsTable = true;
			this.ImageProcessing = ImageProcessing.AutomaticDownload;
			knownTags = InitKnownTags();
			htmlStyles = new HtmlDocumentStyle(mainPart);
			knownImageParts = new Dictionary<Uri, CachedImagePart>();
            this.WebProxy = new WebProxy();
		}

		/// <summary>
		/// Start the parse processing.
		/// </summary>
		/// <returns>Returns a list of parsed paragraph.</returns>
		public IList<OpenXmlCompositeElement> Parse(String html)
		{
			if (String.IsNullOrEmpty(html))
				return new Paragraph[0];

			// Reset:
			elements = new List<OpenXmlElement>();
			paragraphs = new List<OpenXmlCompositeElement>();
			tables = new TableContext();
			htmlStyles.Runs.Reset();
			currentParagraph = null;

            //ibrahim
            RunProperties runProperties1 = new RunProperties();
            RunFonts runFonts1 = new RunFonts() { Ascii = "Arial", ComplexScript = "Arial" };
            FontSize fontSize1 = new FontSize() { Val = "28" };
            FontSizeComplexScript fontSizeComplexScript1 = new FontSizeComplexScript() { Val = "28" };
            Highlight highlight1 = new Highlight() { Val = HighlightColorValues.White };
            RightToLeftText rightToLeftText1 = new RightToLeftText();

            runProperties1.Append(runFonts1);
            runProperties1.Append(fontSize1);
            runProperties1.Append(fontSizeComplexScript1);
            runProperties1.Append(highlight1);
            runProperties1.Append(rightToLeftText1);

			// Start a new processing
			paragraphs.Add(currentParagraph = htmlStyles.Paragraph.NewParagraph());
			if (htmlStyles.DefaultParagraphStyle != null)
			{
                //currentParagraph.Append(new ParagraphProperties(
                //    new ParagraphStyleId { Val = htmlStyles.DefaultParagraphStyle }
                //));
                //ibrahim
                currentParagraph.Append(runProperties1);
			}
           


			HtmlEnumerator en = new HtmlEnumerator(html);
			ProcessHtmlChunks(en, null);

			if (elements.Count > 0)
				this.currentParagraph.Append(elements);

			RemoveEmptyParagraphs();

			return paragraphs;
		}

		#region RemoveEmptyParagraphs

		/// <summary>
		/// Remove empty paragraph unless 2 tables are side by side.
		/// These paragraph could be empty due to misformed html or spaces in the html source.
		/// </summary>
		private void RemoveEmptyParagraphs()
		{
			bool hasRuns;

			for (int i = 0; i < paragraphs.Count; i++)
			{
				OpenXmlCompositeElement p = paragraphs[i];

				// If the paragraph is between 2 tables, we don't remove it (it provides some
				// separation or Word will merge the two tables)
				if (i > 0 && i + 1 < paragraphs.Count - 1
					&& paragraphs[i - 1].LocalName == "tbl"
					&& paragraphs[i + 1].LocalName == "tbl") continue;

				if (p.HasChildren)
				{
					if (!(p is Paragraph)) continue;

					// Has this paragraph some other elements than ParagraphProperties?
					// This code ensure no default style or attribute on empty div will stay
					hasRuns = false;
					for (int j = p.ChildElements.Count - 1; j >= 0; j--)
					{
						if (!(p.ChildElements[j] is ParagraphProperties))
						{
							hasRuns = true;
							break;
						}
					}

					if (hasRuns) continue;
				}

				paragraphs.RemoveAt(i);
				i--;
			}
		}

		#endregion

		#region ProcessHtmlChunks

		private void ProcessHtmlChunks(HtmlEnumerator en, String endTag)
		{
			while (en.MoveUntilMatch(endTag))
			{
				if (en.IsCurrentHtmlTag)
				{
					Action<HtmlEnumerator> action;
					if (knownTags.TryGetValue(en.CurrentTag, out action))
					{
						action(en);
					}

					// else unknow or not yet implemented - we ignore
				}
				else
				{
					// apply the previously discovered style
					Run run = new Run(
						new Text(HttpUtility.HtmlDecode(en.Current)) { Space = SpaceProcessingModeValues.Preserve }
					);
					htmlStyles.Runs.ApplyTags(run);
					elements.Add(run);
				}
			}
		}

		#endregion

		#region AlternateProcessHtmlChunks

		/// <summary>
		/// Save the actual list and restart with a new one.
		/// Continue to process until we found endTag.
		/// </summary>
		private void AlternateProcessHtmlChunks(HtmlEnumerator en, string endTag)
		{
			if(elements.Count > 0) CompleteCurrentParagraph();
			ProcessHtmlChunks(en, endTag);
		}

		#endregion

		#region AddParagraph

		/// <summary>
		/// Add a new paragraph, table, ... to the list of processed paragrahs. This method takes care of 
		/// adding the new element to the current table if it exists.
		/// </summary>
		private void AddParagraph(OpenXmlCompositeElement element)
		{
			if (tables.HasContext)
				tables.CurrentTable.GetLastChild<TableRow>().GetLastChild<TableCell>().Append(element);
			else
				this.paragraphs.Add(element);
		}

		#endregion

		#region AddFootnoteReference

		/// <summary>
		/// Add a note to the FootNotes part and ensure it exists.
		/// </summary>
		/// <param name="description">The description of an acronym, abbreviation, some book references, ...</param>
		/// <returns>Returns the id of the footnote reference.</returns>
		private int AddFootnoteReference(string description)
		{
			FootnotesPart fpart = mainPart.FootnotesPart;
			if (fpart == null)
				fpart = mainPart.AddNewPart<FootnotesPart>();

			if (fpart.Footnotes == null)
			{
				// Insert a new Footnotes reference
				new Footnotes(
					new Footnote(
						new Paragraph(
							new ParagraphProperties(
								new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto }),
							new Run(
								new SeparatorMark())
						)
					) { Type = FootnoteEndnoteValues.Separator, Id = -1 },
					new Footnote(
						new Paragraph(
							new ParagraphProperties(
								new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto }),
							new Run(
								new ContinuationSeparatorMark())
						)
					) { Type = FootnoteEndnoteValues.ContinuationSeparator, Id = 0 }).Save(fpart);
				footnotesRef = 1;
			}
			else
			{
				// The footnotesRef Id is a required field and should be unique. You can assign yourself some hard-coded
				// value but that's absolutely not safe. We will loop through the existing Footnote
				// to retrieve the highest Id.
				foreach (var p in fpart.Footnotes.Elements<Footnote>())
				{
					if (p.Id.HasValue && p.Id > footnotesRef) footnotesRef = (int)p.Id.Value;
				}
				footnotesRef++;
			}

			Run markerRun;
			fpart.Footnotes.Append(
				new Footnote(
					new Paragraph(
						new ParagraphProperties(
							new ParagraphStyleId() { Val = htmlStyles.GetStyle("footnote text", false) }),
						markerRun = new Run(
							new RunProperties(
								new RunStyle() { Val = htmlStyles.GetStyle("footnote reference", true) }),
							new FootnoteReferenceMark()),
						new Run(
							// Word insert automatically a space before the definition to separate the reference number
							// with its description
                            new Text(" " + description) { Space = SpaceProcessingModeValues.Preserve })
					)
				) { Id = footnotesRef });

			if (!htmlStyles.DoesStyleExists("footnote reference"))
			{
				// Force the superscript style because if the footnote text style does not exists,
				// the rendering will be awful.
				markerRun.InsertInProperties(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript });
			}
			fpart.Footnotes.Save();

			return footnotesRef;
		}

		#endregion

		#region AddEndnoteReference

		/// <summary>
		/// Add a note to the Endnotes part and ensure it exists.
		/// </summary>
		/// <param name="description">The description of an acronym, abbreviation, some book references, ...</param>
		/// <returns>Returns the id of the endnote reference.</returns>
		private int AddEndnoteReference(string description)
		{
			EndnotesPart fpart = mainPart.EndnotesPart;
			if (fpart == null)
				fpart = mainPart.AddNewPart<EndnotesPart>();

			if (fpart.Endnotes == null)
			{
				// Insert a new Footnotes reference
				new Endnotes(
					new Endnote(
						new Paragraph(
							new ParagraphProperties(
								new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto }),
							new Run(
								new SeparatorMark())
						)
					) { Type = FootnoteEndnoteValues.ContinuationSeparator, Id = -1 },
					new Endnote(
						new Paragraph(
							new ParagraphProperties(
								new SpacingBetweenLines() { After = "0", Line = "240", LineRule = LineSpacingRuleValues.Auto }),
							new Run(
								new ContinuationSeparatorMark())
						)
					) { Id = 0 }).Save(fpart);
				endnotesRef = 1;
			}
			else
			{
				// The footnotesRef Id is a required field and should be unique. You can assign yourself some hard-coded
				// value but that's absolutely not safe. We will loop through the existing Footnote
				// to retrieve the highest Id.
				foreach (var p in fpart.Endnotes.Elements<Endnote>())
				{
					if (p.Id.HasValue && p.Id > footnotesRef) endnotesRef = (int)p.Id.Value;
				}
				endnotesRef++;
			}

			Run markerRun;
			fpart.Endnotes.Append(
				new Endnote(
					new Paragraph(
						new ParagraphProperties(
							new ParagraphStyleId() { Val = htmlStyles.GetStyle("endnote text", false) }),
						markerRun = new Run(
							new RunProperties(
								new RunStyle() { Val = htmlStyles.GetStyle("endnote reference", true) }),
							new FootnoteReferenceMark()),
						new Run(
							// Word insert automatically a space before the definition to separate the reference number
							// with its description
                            new Text(" " + description) { Space = SpaceProcessingModeValues.Preserve })
					)
				) { Id = endnotesRef });

			if (!htmlStyles.DoesStyleExists("endnote reference"))
			{
				// Force the superscript style because if the footnote text style does not exists,
				// the rendering will be awful.
				markerRun.InsertInProperties(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript });
			}
	
			fpart.Endnotes.Save();

			return endnotesRef;
		}

		#endregion

        #region AddFigureCaption

        /// <summary>
        /// Add a new figure caption to the document.
        /// </summary>
        /// <returns>Returns the id of the new figure caption.</returns>
        private int AddFigureCaption()
        {
            if(figCaptionRef == -1)
            {
                figCaptionRef = 0;
                foreach (var p in mainPart.Document.Descendants<SimpleField>())
                {
                    if (p.Instruction == " SEQ Figure \\* ARABIC ")
                        figCaptionRef++;
                }
            }
            figCaptionRef++;
            return figCaptionRef;
        }

        #endregion

        #region AddImagePart

        private Drawing AddImagePart(Uri imageUrl, String imageSource, String alt, Size preferredSize)
		{
			if (imageObjId == UInt32.MinValue)
			{
				// In order to add images in the document, we need to asisgn an unique id
				// to each Drawing object. So we'll loop through all of the existing <wp:docPr> elements
				// to find the largest Id, then increment it for each new image.

				drawingObjId = 1; // 1 is the minimum ID set by MS Office.
				imageObjId = 1;
				foreach (var d in mainPart.Document.Body.Descendants<Drawing>())
				{
					if (d.Inline.DocProperties.Id > drawingObjId) drawingObjId = d.Inline.DocProperties.Id;

					var nvPr = d.Inline.Graphic.GraphicData.GetFirstChild<pic.NonVisualPictureProperties>();
					if(nvPr != null && nvPr.NonVisualDrawingProperties.Id > imageObjId)
						imageObjId = nvPr.NonVisualDrawingProperties.Id;
				}
				if (drawingObjId > 1) drawingObjId++;
				if (imageObjId > 1) imageObjId++;
			}


			// Cache all the ImagePart processed to avoid downloading the same image.
			CachedImagePart imagePart;
			if(!knownImageParts.TryGetValue(imageUrl, out imagePart))
			{
				ProvisionImageEventArgs e = new ProvisionImageEventArgs(imageUrl);
				e.ImageSize = preferredSize;
				if (this.ImageProcessing == ImageProcessing.AutomaticDownload && imageUrl.IsAbsoluteUri)
				{
					e.Data = ConverterUtility.DownloadData(imageUrl, this.WebProxy);
				}
				else
				{
					RaiseProvisionImage(e);
				}

				if(e.Data == null) return null;

				if (!e.ImageExtension.HasValue)
				{
					e.ImageExtension = ConverterUtility.GetImagePartTypeForImageUrl(imageUrl);
					if (!e.ImageExtension.HasValue) return null;
				}

				ImagePart ipart = mainPart.AddImagePart(e.ImageExtension.Value);
				imagePart = new CachedImagePart() { Part = ipart };

				using (Stream outputStream = ipart.GetStream(FileMode.Create))
				{
					outputStream.Write(e.Data, 0, e.Data.Length);
					outputStream.Seek(0L, SeekOrigin.Begin);

					if (e.ImageSize.Width == 0 || e.ImageSize.Height == 0)
					{
						e.ImageSize = ConverterUtility.GetImageSize(outputStream);
					}
					imagePart.Width = e.ImageSize.Width;
					imagePart.Height = e.ImageSize.Height;
				}

				knownImageParts.Add(imageUrl, imagePart);
			}

			if (preferredSize.IsEmpty)
			{
				preferredSize.Width = imagePart.Width;
				preferredSize.Height = imagePart.Height;
			}
			else if (preferredSize.Width <= 0 || preferredSize.Height <= 0)
			{
				Size actualSize = new Size(imagePart.Width, imagePart.Height);
				preferredSize = ImageHeader.KeepAspectRatio(actualSize, preferredSize);
			}

			String imagePartId = mainPart.GetIdOfPart(imagePart.Part);
			long widthInEmus = new Unit("px", preferredSize.Width).ValueInEmus;
			long heightInEmus = new Unit("px", preferredSize.Height).ValueInEmus;

			++drawingObjId;
			++imageObjId;

			var img = new Drawing(
				new wp.Inline(
					new wp.Extent() { Cx = widthInEmus, Cy = heightInEmus },
					new wp.EffectExtent() { LeftEdge = 19050L, TopEdge = 0L, RightEdge = 0L, BottomEdge = 0L },
					new wp.DocProperties() { Id = drawingObjId, Name = imageSource, Description = String.Empty },
					new wp.NonVisualGraphicFrameDrawingProperties(
						new a.GraphicFrameLocks() { NoChangeAspect = true }),
					new a.Graphic(
						new a.GraphicData(
							new pic.Picture(
								new pic.NonVisualPictureProperties(
									new pic.NonVisualDrawingProperties() { Id = imageObjId, Name = imageSource, Description = alt },
									new pic.NonVisualPictureDrawingProperties(
										new a.PictureLocks() { NoChangeAspect = true, NoChangeArrowheads = true })),
								new pic.BlipFill(
									new a.Blip() { Embed = imagePartId },
									new a.SourceRectangle(),
									new a.Stretch(
										new a.FillRectangle())),
								new pic.ShapeProperties(
									new a.Transform2D(
										new a.Offset() { X = 0L, Y = 0L },
										new a.Extents() { Cx = widthInEmus, Cy = heightInEmus }),
									new a.PresetGeometry(
										new a.AdjustValueList()
									) { Preset = a.ShapeTypeValues.Rectangle }
								) { BlackWhiteMode = a.BlackWhiteModeValues.Auto })
						) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
				) { DistanceFromTop = (UInt32Value)0U, DistanceFromBottom = (UInt32Value)0U, DistanceFromLeft = (UInt32Value)0U, DistanceFromRight = (UInt32Value)0U }
			);

			return img;
		}

		#endregion

		#region InitKnownTags

		private Dictionary<String, Action<HtmlEnumerator>> InitKnownTags()
		{
			// A complete list of HTML tags can be found here: http://www.w3schools.com/tags/default.asp

			var knownTags = new Dictionary<String, Action<HtmlEnumerator>>(StringComparer.InvariantCultureIgnoreCase) {
				{ "<a>", ProcessLink },
				{ "<abbr>" , ProcessAcronym },
				{ "<acronym>" , ProcessAcronym },
				{ "<b>", ProcessBold },
                { "<blockquote>", ProcessBlockQuote },
				{ "<body>", ProcessBody },
				{ "<br>", ProcessBr },
				{ "<caption>", ProcessTableCaption },
				{ "<cite>", ProcessCite },
				{ "<del>", ProcessStrike },
				{ "<div>", ProcessDiv },
				{ "<dd>", ProcessDefinitionListItem },
				{ "<dt>", ProcessDefinitionList },
				{ "<em>", ProcessItalic },
				{ "<font>", ProcessFont },
				{ "<h1>", ProcessHeading },
				{ "<h2>", ProcessHeading },
				{ "<h3>", ProcessHeading },
				{ "<h4>", ProcessHeading },
				{ "<h5>", ProcessHeading },
				{ "<h6>", ProcessHeading },
				{ "<hr>", ProcessHorizontalLine },
                { "<figcaption>", ProcessFigureCaption },
				{ "<i>", ProcessItalic },
				{ "<img>", ProcessImage },
				{ "<ins>", ProcessUnderline },
				{ "<li>", ProcessLi },
				{ "<ol>", ProcessNumberingList },
				{ "<p>", ProcessParagraph },
				{ "<pre>", ProcessPre },
                { "<q>", ProcessQuote },
				{ "<span>", ProcessSpan },
				{ "<s>", ProcessStrike },
				{ "<strike>", ProcessStrike },
				{ "<strong>", ProcessBold },
				{ "<sub>", ProcessSubscript },
				{ "<sup>", ProcessSuperscript },
				{ "<table>", ProcessTable },
				{ "<tbody>", ProcessTablePart },
				{ "<td>", ProcessTableColumn },
				{ "<tfoot>", ProcessTablePart },
				{ "<th>", ProcessTableColumn },
				{ "<thead>", ProcessTablePart },
				{ "<tr>", ProcessTableRow },
				{ "<u>", ProcessUnderline },
				{ "<ul>", ProcessNumberingList },
				{ "<xml>", ProcessXmlDataIsland },

				// closing tag
				{ "</b>", ProcessClosingTag },
                { "</blockquote>", ProcessClosingBlockQuote },
				{ "</body>", ProcessClosingTag },
				{ "</cite>", ProcessClosingTag },
				{ "</del>", ProcessClosingTag },
				{ "</div>", ProcessClosingDiv },
				{ "</em>", ProcessClosingTag },
				{ "</font>", ProcessClosingTag },
				{ "</i>", ProcessClosingTag },
				{ "</ins>", ProcessClosingTag },
				{ "</ol>", ProcessClosingNumberingList },
                { "</p>", ProcessClosingParagraph },
                { "</q>", ProcessClosingQuote },
				{ "</span>", ProcessClosingTag },
				{ "</s>", ProcessClosingTag },
				{ "</strike>", ProcessClosingTag },
				{ "</strong>", ProcessClosingTag },
				{ "</sub>", ProcessClosingTag },
				{ "</sup>", ProcessClosingTag },
				{ "</table>", ProcessClosingTable },
				{ "</tbody>", ProcessClosingTablePart },
				{ "</tfoot>", ProcessClosingTablePart },
				{ "</thead>", ProcessClosingTablePart },
				{ "</td>", ProcessClosingTableColumn },
				{ "</th>", ProcessClosingTableColumn },
				{ "</tr>", ProcessClosingTableRow },
				{ "</u>", ProcessClosingTag },
				{ "</ul>", ProcessClosingNumberingList },
			};

			return knownTags;
		}

		#endregion

		#region Bookmarks

		private List<String> Bookmarks
		{
			get
			{
				if (bookmarks == null)
				{
					bookmarks = new List<String>();
					var en = mainPart.Document.Body.Descendants<BookmarkStart>().GetEnumerator();
					while (en.MoveNext())
						bookmarks.Add(en.Current.Name.Value);
					bookmarks.Sort(StringComparer.Ordinal);
				}
				return bookmarks;
			}
		}

		#endregion

		#region CompleteCurrentParagraph

		/// <summary>
		/// Push the elements members to the current paragraph and reset the elements collection.
		/// </summary>
		private void CompleteCurrentParagraph()
		{
			this.currentParagraph.Append(elements);
			htmlStyles.Paragraph.ApplyTags(currentParagraph);
			elements.Clear();
		}

		#endregion

		#region RefreshStyle

		/// <summary>
		/// Refresh the cache of styles presents in the document.
		/// </summary>
		public void RefreshStyles()
		{
			htmlStyles.PrepareStyles(mainPart);
		}

		#endregion

        #region EnsureCaptionStyle

        /// <summary>
        /// Ensure the 'caption' style exists in the document.
        /// </summary>
        private void EnsureCaptionStyle()
        {
            String normalStyleName = htmlStyles.GetStyle("Normal", false);
            Style style = new Style(
                new StyleName { Val = "caption" },
                new BasedOn { Val = normalStyleName },
                new NextParagraphStyle { Val = normalStyleName },
                new UnhideWhenUsed(),
                new PrimaryStyle(),
                new StyleParagraphProperties(
                    new SpacingBetweenLines { Line = "240", LineRule = LineSpacingRuleValues.Auto }
                ),
                new StyleRunProperties(
                    new Bold(),
                    new BoldComplexScript(),
                    new DocumentFormat.OpenXml.Wordprocessing.Color() { Val = "4F81BD", ThemeColor = ThemeColorValues.Accent1 },
                    new FontSize { Val = "18" },
                    new FontSizeComplexScript { Val = "18" }
                )
            ) { Type = StyleValues.Paragraph, StyleId = "Caption" };

            htmlStyles.AddStyle("caption", style);
        }

        #endregion

		#region ProcessContainerAttributes

		/// <summary>
		/// There is a few attributes shared by a large number of tags. This method will check them for a limited
		/// number of tags (&lt;p&gt;, &lt;pre&gt;, &lt;div&gt;, &lt;span&gt; and &lt;body&gt;).
		/// </summary>
		private bool ProcessContainerAttributes(HtmlEnumerator en, IList<OpenXmlElement> styleAttributes)
		{
			if (en.Attributes.Count == 0) return false;

			bool paragraphBreakAfter = false;
			List<OpenXmlElement> containerStyleAttributes = new List<OpenXmlElement>();

			string attrValue = en.Attributes["lang"];
			if (attrValue != null && attrValue.Length > 0)
			{
				containerStyleAttributes.Add(
					new ParagraphMarkRunProperties(
						new Languages() { Val = attrValue }));
			}


			// Not applicable to a table : page break
			if (!tables.HasContext || en.CurrentTag == "<pre>")
			{
				attrValue = en.StyleAttributes["page-break-after"];
				if (attrValue == "always")
				{
					paragraphs.Add(new Paragraph(
						new Run(
							new Break() { Type = BreakValues.Page })));
					paragraphBreakAfter = true;
				}

				attrValue = en.StyleAttributes["page-break-before"];
				if (attrValue == "always")
				{
					paragraphs.Insert(paragraphs.Count - 1, new Paragraph(
						new Run(
							new Break() { Type = BreakValues.Page })));
				}
			}

			attrValue = en.StyleAttributes["text-align"];
			if (attrValue != null && en.CurrentTag != "<font>")
			{
				JustificationValues? align = ConverterUtility.FormatParagraphAlign(attrValue);
				if (align.HasValue)
				{
					containerStyleAttributes.Add(new Justification { Val = align });
				}
			}

			htmlStyles.Paragraph.BeginTag(en.CurrentTag, containerStyleAttributes);

			// Process general run styles
			htmlStyles.Runs.ProcessCommonRunAttributes(en, styleAttributes);

			return paragraphBreakAfter;
		}

		#endregion

		// Events

		#region RaiseProvisionImage

		/// <summary>
		/// Raises the ProvisionImage event.
		/// </summary>
		protected virtual void RaiseProvisionImage(ProvisionImageEventArgs e)
		{
			if (ProvisionImage != null) ProvisionImage(this, e);
		}

		#endregion

		//____________________________________________________________________
		//
		// Configuration

		/// <summary>
		/// Gets or sets where to render the acronym or abbreviation tag.
		/// </summary>
		public AcronymPosition AcronymPosition { get; set; }

		/// <summary>
		/// Gets or sets whether the &lt;div&gt; tag should be processed as &lt;p&gt;. It depends whether you consider &lt;div&gt;
		/// as part of the layout or as part of a text field.
		/// </summary>
		public bool ConsiderDivAsParagraph { get; set; }

		/// <summary>
		/// Gets or sets whether anchor links are included or not in the conversion.
		/// </summary>
		/// <remarks>An anchor is a term used to define a hyperlink destination inside a document.
		/// <see href="http://www.w3schools.com/HTML/html_links.asp"/>.
		/// <br/>
		/// It exists some predefined anchors used by Word such as _top to refer to the top of the document.
		/// The anchor <i>#_top</i> is always accepted regardless this property value.
		/// For others anchors like refering to your own bookmark or a title, add a 
		/// <see cref="DocumentFormat.OpenXml.Wordprocessing.BookmarkStart"/> and 
		/// <see cref="DocumentFormat.OpenXml.Wordprocessing.BookmarkEnd"/> elements
		/// and set the value of href to <i>#&lt;name of your bookmark&gt;</i>.
		/// </remarks>
		public bool ExcludeLinkAnchor { get; set; }

		/// <summary>
		/// Gets the Html styles manager mapping to OpenXml style properties.
		/// </summary>
		public HtmlDocumentStyle HtmlStyles
		{
			get { return htmlStyles; }
		}

		/// <summary>
		/// Gets or sets how the &lt;img&gt; tag should be handled.
		/// </summary>
		public ImageProcessing ImageProcessing { get; set; }

		/// <summary>
		/// Gets or sets the base Uri used to automaticaly resolve relative images 
		/// if used with ImageProcessing = AutomaticDownload.
		/// </summary>
		public Uri BaseImageUrl
		{
			get { return this.baseImageUri; }
			set
			{
				if (value != null && !value.IsAbsoluteUri)
					throw new ArgumentException("BaseImageUrl should be an absolute Uri");
				this.baseImageUri = value;
			}
		}

        /// <summary>
        /// Gets or sets the proxy used to download images.
        /// </summary>
        public WebProxy WebProxy { get; set; }

		/// <summary>
		/// Gets or sets where the Legend tag (&lt;caption&gt;) should be rendered (above or below the table).
		/// </summary>
		public CaptionPositionValues TableCaptionPosition { get; set; }

		/// <summary>
		/// Gets or sets whether the &lt;pre&gt; tag should be rendered as a table.
		/// </summary>
		/// <remarks>The table will contains only one cell.</remarks>
		public bool RenderPreAsTable { get; set; }
	}
}