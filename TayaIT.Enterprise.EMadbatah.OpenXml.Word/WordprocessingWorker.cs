using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Validation;
using System.IO;
using System.Xml;
using DocumentFormat.OpenXml.ExtendedProperties;
using System.Xml.Linq;
using A = DocumentFormat.OpenXml.Drawing;
using DW = DocumentFormat.OpenXml.Drawing.Wordprocessing;
using PIC = DocumentFormat.OpenXml.Drawing.Pictures;
using System.Text.RegularExpressions;
using System.Threading;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class WordprocessingWorker : IDisposable
    {
        //http://stackoverflow.com/questions/3308299/replace-bookmark-text-in-word-file-using-open-xml-sdk
        private WordprocessingDocument _currentDoc;
        private MainDocumentPart _docMainPart;
        
        private StyleDefinitionsPart _docStylePart;
        private DocumentSettingsPart _docSettingsPart;
        private FooterPart _docFooterPart;
        private ThemePart _themePart;
        private WebSettingsPart _webSettingsPart;
        private ExtendedFilePropertiesPart _extendedFilePropertiesPart;
        private NumberingDefinitionsPart _numberingDefinitionsPart;
        private EndnotesPart _endnotesPart;
        private FontTablePart _fontTablePart;
        private FootnotesPart _footnotesPart;
        private Footnotes _footNotes;
        private CoreFilePropertiesPart _coreFilePropertiesPart;
        private DocFileOperation _docOperation;

        public MainDocumentPart DocMainPart
        {
            get
            {
                return _docMainPart;
            }
        }

        //private Body _body;
        private MemoryStream _inMemoryStream;
        private string _outputPath;
        

        public String DocPath { get; set; }
        public bool IsOpen
        {
            get { return (_currentDoc != null); }
        }

        private DocXmlParts _docXmlParts;

        public WordprocessingWorker(string docPath,
            DocXmlParts docXmlParts,
            DocFileOperation fileOperation) 
        {
            DocPath = docPath;
            _docXmlParts = docXmlParts;
            _docOperation = fileOperation;
            _footNotes = new Footnotes();
            if (fileOperation == DocFileOperation.CreateNew)
            {
                _currentDoc = WordprocessingDocument.Create(DocPath, WordprocessingDocumentType.Document);                
                _docMainPart = _currentDoc.AddMainDocumentPart();
                _docMainPart.Document = MakeEmpyDocument();

                InitializeDocumentPartsFromXml();
                InitializeDocumentStyles();
            }
            else
            {

                Open(docPath);
            }

                     
           // 
            //_docMainPart.Document = new Document();
            //_body = new Body();

            //InitializeDocumentPartsFromXml();
            //InitializeDocumentStyles();


            
        }


        public WordprocessingWorker(string docPath,
    DocFileOperation fileOperation)
        {
            DocPath = docPath;

            _docOperation = fileOperation;

            _footNotes = new Footnotes();
            if (fileOperation == DocFileOperation.CreateNew)
            {
                _currentDoc = WordprocessingDocument.Create(DocPath, WordprocessingDocumentType.Document);
                // _currentDoc.FileOpenAccess = FileAccess.ReadWrite;
                _docMainPart = _currentDoc.AddMainDocumentPart();
                _docMainPart.Document = MakeEmpyDocument();
            }
            else
            {
                Open(docPath);
            }



        }


        public static DocXmlParts GetDocParts(string resfolderpath)
        {
           
            DocXmlParts xmlFilesPaths = new DocXmlParts();
            xmlFilesPaths.CoreFilePropertiesPart = resfolderpath + "core.xml";
            xmlFilesPaths.EndNotes = resfolderpath + "endnotes.xml";
            xmlFilesPaths.FilePropPartPath = resfolderpath + "app.xml";
            xmlFilesPaths.FontTablePart = resfolderpath + "fontTable.xml";
            xmlFilesPaths.FooterPartPath = resfolderpath + "footer1.xml";
            xmlFilesPaths.FootnotesPart = resfolderpath + "footnotes.xml";
            xmlFilesPaths.NumberingDefinitionsPartPath = resfolderpath + "numbering.xml";
            xmlFilesPaths.SettingsPartPath = resfolderpath + "settings.xml";
            xmlFilesPaths.StylePartPath = resfolderpath + "styles.xml";
            xmlFilesPaths.ThemePartPath = resfolderpath + "theme1.xml";
            xmlFilesPaths.WebSettingsPartPath = resfolderpath + "webSettings.xml";
            return xmlFilesPaths;
        }


        private void InitializeDocumentPartsFromXml()
        {

            //app.xml
            //XmlDocument filePropPartXml = new XmlDocument();
            //filePropPartXml.Load(_docXmlParts.FilePropPartPath);
            //_extendedFilePropertiesPart = _docMainPart.AddNewPart<ExtendedFilePropertiesPart>();
            //using (Stream outputStream = _extendedFilePropertiesPart.GetStream())
            //{
            //    using (StreamWriter ts = new StreamWriter(outputStream))
            //    {
            //        ts.Write(filePropPartXml.InnerXml);
            //    }
            //}


            //numbering.xml
            XmlDocument numberingPartXML = new XmlDocument();
            numberingPartXML.Load(_docXmlParts.NumberingDefinitionsPartPath);
            _numberingDefinitionsPart = _docMainPart.AddNewPart<NumberingDefinitionsPart>(); //("rId9");
            using (Stream outputStream = _numberingDefinitionsPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(numberingPartXML.InnerXml);
                }
            }

            
            //endnotes.xml
            XmlDocument endNotesPartXML = new XmlDocument();
            endNotesPartXML.Load(_docXmlParts.EndNotes);
            _endnotesPart = _docMainPart.AddNewPart<EndnotesPart>("rId6");
            using (Stream outputStream = _endnotesPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(endNotesPartXML.InnerXml);
                }
            }


            //fontTable.xml
            XmlDocument fontTablePartXML = new XmlDocument();
            fontTablePartXML.Load(_docXmlParts.FontTablePart);
            _fontTablePart = _docMainPart.AddNewPart<FontTablePart>("rId11");
            using (Stream outputStream = _fontTablePart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(fontTablePartXML.InnerXml);
                }
            }


            //footnotes.xml
            XmlDocument footnotePartXML = new XmlDocument();
            footnotePartXML.Load(_docXmlParts.FootnotesPart);
            _footnotesPart = _docMainPart.AddNewPart<FootnotesPart>("rId5");
            using (Stream outputStream = _footnotesPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(footnotePartXML.InnerXml);
                }
            }


            //_coreFilePropertiesPart
            //XmlDocument coreFilePropertiesPartXml = new XmlDocument();
            //coreFilePropertiesPartXml.Load(_docXmlParts.CoreFilePropertiesPart);
            //_coreFilePropertiesPart = _docMainPart.AddNewPart<CoreFilePropertiesPart>();
            //using (Stream outputStream = _coreFilePropertiesPart.GetStream())
            //{
            //    using (StreamWriter ts = new StreamWriter(outputStream))
            //    {
            //        ts.Write(coreFilePropertiesPartXml.InnerXml);
            //    }
            //}
            

            XmlDocument settingsPartXml = new XmlDocument();
            settingsPartXml.Load(_docXmlParts.SettingsPartPath);
            _docSettingsPart = _docMainPart.AddNewPart<DocumentSettingsPart>("rId3");
            using (Stream outputStream = _docSettingsPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(settingsPartXml.InnerXml);
                }
            }

            XmlDocument themePartXml = new XmlDocument();
            themePartXml.Load(_docXmlParts.ThemePartPath);
            _themePart = _docMainPart.AddNewPart<ThemePart>("rId12");
            using (Stream outputStream = _themePart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(themePartXml.InnerXml);
                }
            }

            XmlDocument footerPartXml = new XmlDocument();
            footerPartXml.Load(_docXmlParts.FooterPartPath);
            _docFooterPart = _docMainPart.AddNewPart<FooterPart>("rId8");
            using (Stream outputStream = _docFooterPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(footerPartXml.InnerXml);
                }
            }

            XmlDocument webSettingsPartXml = new XmlDocument();
            webSettingsPartXml.Load(_docXmlParts.WebSettingsPartPath);
            _webSettingsPart = _docMainPart.AddNewPart<WebSettingsPart>("rId4");
            using (Stream outputStream = _webSettingsPart.GetStream())
            {
                using (StreamWriter ts = new StreamWriter(outputStream))
                {
                    ts.Write(webSettingsPartXml.InnerXml);
                }
            }

        }

        public WordprocessingWorker()
        {
            _footNotes = new Footnotes();
            //_docMainPart = _currentDoc.AddMainDocumentPart();
            //_docMainPart.Document = new Document();
            //_body = new Body();

            //InitializeDocumentStyles();
            //InitializeFootNotesPart();
        }


        /// <summary>
        /// Opens the specified Document file.
        /// This loads it in memory for later modification.
        /// </summary>
        /// <param name="docPath"></param>
        public void Open(string docPath)
        {
            //if the document is already Open well close it first to free up resources.
            if (IsOpen)
            {
                Close();
            }

            DocPath = docPath;

            //populate a MemoryStream with byte array of the Document
            byte[] sourceBytes = File.ReadAllBytes(DocPath);
            _inMemoryStream = new MemoryStream();
            _inMemoryStream.Write(sourceBytes, 0, (int)sourceBytes.Length);

            //Create the in-memory Document from the stream for modification
            _currentDoc = WordprocessingDocument.Open(docPath, true);//_inMemoryStream, true);
            _docMainPart = _currentDoc.MainDocumentPart;

            _themePart = _docMainPart.ThemePart;
            _docStylePart = _docMainPart.StyleDefinitionsPart;
            _docSettingsPart = _docMainPart.DocumentSettingsPart;
            _docFooterPart = _docMainPart.FooterParts.FirstOrDefault<FooterPart>();
            _webSettingsPart = _docMainPart.WebSettingsPart;

            _numberingDefinitionsPart = _docMainPart.NumberingDefinitionsPart;
            _endnotesPart = _docMainPart.EndnotesPart;
            _fontTablePart = _docMainPart.FontTablePart;
            _footnotesPart = _docMainPart.FootnotesPart;
            _coreFilePropertiesPart = _currentDoc.CoreFilePropertiesPart;
            _extendedFilePropertiesPart = _currentDoc.ExtendedFilePropertiesPart;
            //_numberingDefinitionsPart = from p in _currentDoc.Parts
            //                            where typeof(p) is NumberingDefinitionsPart
            //                            select p;


        }


        public void LoadDocumentFromHTML(string htmlpath)
        {
            //http://notesforhtml2openxml.codeplex.com/documentation?referringTitle=Home
        }

        public void SaveDocumentAsHTML(string htmlpath)
        {
            //http://msdn.microsoft.com/en-us/library/ff628051%28v=office.14%29.aspx
        }

        //public static int CountPagesUsingOpenXML(WordprocessingWorker doc, string docPath, DocXmlParts xmlFilesPaths, string serverMapPath)
        //{

        //    /*doc.Save();
        //    doc.Close();
        //    doc.Dispose();
            
        //    int count = 0;
        //    string tmpFileName = docPath.Replace(".docx", "_temp.docx");
        //    File.Copy(docPath, tmpFileName, true);
        //    FileInfo fi=new FileInfo(docPath);
            
        //    PdfMaker.PrintToPdf(fi.DirectoryName, serverMapPath, tmpFileName, tmpFileName.Replace(".docx", ".pdf"));
        //    count = TayaIT.Enterprise.EMadbatah.Pdf2Word.pdf2ImageConvert.getNumberOfPdfPages(tmpFileName.Replace(".docx", ".pdf"));
        //    File.Delete(tmpFileName);
        //    File.Delete(tmpFileName.Replace(".docx", ".pdf"));
        //    doc = new WordprocessingWorker(docPath, xmlFilesPaths, DocFileOperation.Open);
        //    return count;*/
        //    return 0;
        //}

        public int CountPagesUsingOpenXML(WordprocessingWorker doc, string docPath, DocXmlParts xmlFilesPaths, string serverMapPath, out WordprocessingWorker worker)
        {
            //worker = doc;
            //return WordCom.GetDocumentNumPages(docPath);
            //WordprocessingWorker tmpDoc = ObjectCopier.Clone<WordprocessingWorker>(doc);//openxml wordprocessing object is not serializable           
            //usama 
            try
            {
                int count = 0;
                doc.Save();
                doc.Dispose();

                string tmpFileName = docPath.Replace(".docx", "_temp.docx");
                using (WordprocessingWorker tmpDoc = new WordprocessingWorker(docPath, DocFileOperation.Open))
                {
                    tmpDoc.SaveAs(tmpFileName);
                }

                //File.Copy(docPath, tmpFileName, true);


                //FileInfo fi=new FileInfo(docPath);

                //PdfMaker.PrintToPdf(fi.DirectoryName, serverMapPath, tmpFileName, tmpFileName.Replace(".docx", ".docx.pdf"));

                //File.Copy(@"C:\PDF Writer\Output\Microsoft Word - body_temp.docx.pdf", tmpFileName.Replace(".docx", ".pdf"), true);
                //File.Copy(@"C:\PDF Writer\Output\Microsoft Word - " + new FileInfo(tmpFileName).Name + ".pdf", tmpFileName.Replace(".docx", ".docx.pdf"), true);
                //File.Delete(@"C:\PDF Writer\Output\Microsoft Word - " + new FileInfo(tmpFileName).Name + ".pdf");
                //count = Pdf2Word.pdf2ImageConvert.GetPDFPageCount(tmpFileName.Replace(".docx", ".docx.pdf"));
                count = WordCom.GetDocumentNumPages(tmpFileName);
                //Thread.Sleep(1000);
                while (true)
                {
                    bool WordExists = false;
                    Thread.Sleep(200);
                    foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
                    {
                        if (thisproc.ProcessName.StartsWith("WINWORD"))
                        {
                            WordExists = true;
                            break;
                        }
                    }
                    if (!WordExists)
                        break;
                }

                File.Delete(tmpFileName);
                //File.Delete(tmpFileName.Replace(".docx", ".docx.pdf"));

                worker = new WordprocessingWorker(docPath, DocFileOperation.Open);
                //worker.SaveAs(docPath);
                return count;
            }
            catch (Exception ex)
            {
                worker = doc;
                return 0;
            }
        }
        
        public int GetCurrentPageNumber()
        {
            //int pageCount = (int)document.ExtendedFilePropertiesPart.Properties.Pages.Text;
            //return _docMainPart.Document.Descendants<SectionProperties>().Count();
            return _docMainPart.Document.Descendants<SectionProperties>().Count();

            /*FileInfo fi = new FileInfo(docPath);
            PdfMaker.PrintToPdf(fi.DirectoryName, serverMapPath, docPath, docPath.Replace(".docx", ".pdf"));
            int count = TayaIT.Enterprise.EMadbatah.Pdf2Word.pdf2ImageConvert.getNumberOfPdfPages(docPath.Replace(".docx", ".pdf"));
            File.Delete(docPath.Replace(".docx", ".pdf"));

            return 0;*/
        }
        public void AddBreak()
        {

            if (_docMainPart == null)
                _docMainPart = _currentDoc.AddMainDocumentPart();
            if (_docMainPart.Document == null)
                _docMainPart.Document = MakeEmpyDocument();

            Paragraph paragraph = new Paragraph();
            Run run = new Run(new Break() {});


            paragraph.Append(run);

            _docMainPart.Document.Body.Append(paragraph);

            Save();
        }
        public void AddCarriageReturn()
        {

            if (_docMainPart == null)
                _docMainPart = _currentDoc.AddMainDocumentPart();
            if (_docMainPart.Document == null)
                _docMainPart.Document = MakeEmpyDocument();

            Paragraph paragraph = new Paragraph();
            Run run = new Run(new CarriageReturn() {});


            paragraph.Append(run);

            _docMainPart.Document.Body.Append(paragraph);

            Save();
        }

        public void AddTextToWordDocument(string paragraphText, ParagraphStyle paragraphType, ParagrapJustification textDirection)
        {
            if (_docMainPart == null)
                _docMainPart = _currentDoc.AddMainDocumentPart();
            if (_docMainPart.Document == null)
                _docMainPart.Document = MakeEmpyDocument();
            //Run run = new Run(new Text(paragraphText) { Space = SpaceProcessingModeValues.Preserve });
            Run run = new Run();// { RsidRunProperties = "006B5D2A" };

            // Assign a reference to the existing document body.
            //Body body = _currentDoc.MainDocumentPart.Document.Body;
            //var sdtContentRunElements =
            //    from sdtContentRun in _currentDoc.MainDocumentPart.RootElement.Descendants<SdtContentRun>()
            //    select sdtContentRun;

            Paragraph currentParagraph = _currentDoc.MainDocumentPart.Document.Body.Elements<Paragraph>().Last();
            //Paragraph currentParagraph = (Paragraph)_currentDoc.MainDocumentPart.RootElement.Descendants<Paragraph>().Last();
            //string modifiedString = Regex.Replace(pp.InnerText, currentString, reusableContentString);
            //currentParagraph.RemoveAllChildren<Run>();
           
            switch (paragraphType)
            {
                case ParagraphStyle.ParagraphTitle:
                    RunProperties runParagraphTitleTextProp = new RunProperties();
                    RunFonts runParagraphTitleTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                    Bold bold = new Bold();
                    BoldComplexScript boldComplexScript = new BoldComplexScript();
                    FontSize fontParagraphTitleTextSize = new FontSize() { Val = "28" };
                    FontSizeComplexScript fontParagraphTitleComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                    RightToLeftText rtlTextParagraphTitle = new RightToLeftText();
                    runParagraphTitleTextProp.Append(runParagraphTitleTextFonts);
                    runParagraphTitleTextProp.Append(bold);
                    runParagraphTitleTextProp.Append(boldComplexScript);
                    runParagraphTitleTextProp.Append(fontParagraphTitleTextSize);
                    runParagraphTitleTextProp.Append(fontParagraphTitleComplexScriptSize);
                    runParagraphTitleTextProp.Append(rtlTextParagraphTitle);
                    run.Append(runParagraphTitleTextProp);

                    /*RunProperties runProperties1 = new RunProperties();
                    Bold bold1 = new Bold();
                    BoldComplexScript boldComplexScript1 = new BoldComplexScript();
                    runProperties1.Append(bold1);
                    runProperties1.Append(boldComplexScript1);
                    Text text3 = new Text();
                    text3.Text = "شسيستيا تي ستاس";

                    run3.Append(runProperties1);
                    run3.Append(text3);
                    */

                    break;
                case ParagraphStyle.NormalArabic:
                    RunProperties runNormalTextProp = new RunProperties();
                    RunFonts runNormalTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial", AsciiTheme = ThemeFontValues.MinorBidi, HighAnsiTheme = ThemeFontValues.MinorBidi };
                    FontSize fontNormalTextSize = new FontSize() { Val = "28" };
                    FontSizeComplexScript fontComplexNormalTextSize = new FontSizeComplexScript() { Val = "28" };
                    RightToLeftText rightToLeftText = new RightToLeftText();
                    //BiDi biDi1 = new BiDi();
                    //Languages languages = new Languages() { EastAsia = "ar-SA", Bidi = "ar-SA" };

                    runNormalTextProp.Append(runNormalTextFonts);
                    runNormalTextProp.Append(fontNormalTextSize);
                    runNormalTextProp.Append(fontComplexNormalTextSize);
                    runNormalTextProp.Append(rightToLeftText);
                    run.Append(runNormalTextProp);
                    break;
                case ParagraphStyle.ParagraphItalic:
                    RunProperties runParagraphItalicTextProp = new RunProperties();
                    RunFonts runParagraphItalicTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                    Italic italic = new Italic();
                    BoldComplexScript boldComplexScriptItalic = new BoldComplexScript();
                    ItalicComplexScript italicComplexScript = new ItalicComplexScript();
                    FontSize fontParagraphItalicTextSize = new FontSize() { Val = "28" };
                    FontSizeComplexScript fontParagraphItalicComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                    RightToLeftText rtlTextParagraphItalic = new RightToLeftText();
                    runParagraphItalicTextProp.Append(runParagraphItalicTextFonts);
                    runParagraphItalicTextProp.Append(italic);
                    runParagraphItalicTextProp.Append(italicComplexScript);
                    runParagraphItalicTextProp.Append(fontParagraphItalicTextSize);
                    runParagraphItalicTextProp.Append(fontParagraphItalicComplexScriptSize);
                    runParagraphItalicTextProp.Append(rtlTextParagraphItalic);
                    run.Append(runParagraphItalicTextProp);
                    break;
                case ParagraphStyle.ParagraphBoldItalic:
                    RunProperties runParagraphBoldItalicTextProp = new RunProperties();
                    RunFonts runParagraphBoldItalicTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                    Italic italicBoldItalic = new Italic();
                    Bold boldBoldItalic = new Bold();
                    BoldComplexScript boldComplexScriptBoldItalic = new BoldComplexScript();
                    ItalicComplexScript italicComplexScriptBoldItalic = new ItalicComplexScript();
                    FontSize fontParagraphBoldItalicTextSize = new FontSize() { Val = "28" };
                    FontSizeComplexScript fontParagraphBoldItalicComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                    RightToLeftText rtlTextParagraphBoldItalic = new RightToLeftText();
                    runParagraphBoldItalicTextProp.Append(runParagraphBoldItalicTextFonts);
                    runParagraphBoldItalicTextProp.Append(italicBoldItalic);
                    runParagraphBoldItalicTextProp.Append(italicComplexScriptBoldItalic);
                    runParagraphBoldItalicTextProp.Append(boldBoldItalic);
                    runParagraphBoldItalicTextProp.Append(boldComplexScriptBoldItalic);
                    runParagraphBoldItalicTextProp.Append(fontParagraphBoldItalicTextSize);
                    runParagraphBoldItalicTextProp.Append(fontParagraphBoldItalicComplexScriptSize);
                    runParagraphBoldItalicTextProp.Append(rtlTextParagraphBoldItalic);
                    run.Append(runParagraphBoldItalicTextProp);
                    break;
                case ParagraphStyle.FootnoteText:
                    // footnote = new Footnote() { Type = FootnoteEndnoteValues.Separator, Id = -1 };
                    break;
                case ParagraphStyle.FootnoteReference:
                    //     FootnoteReference footnoteRef = new FootnoteReference() { CustomMarkFollows = true, Id = 1 };
                    //  run.Append(footnoteRef);
                    break;
                case ParagraphStyle.SeparatorMark:
                    SeparatorMark separatorMark = new SeparatorMark();
                    run.Append(separatorMark);
                    break;
                default:
                    break;
            }
            run.Append(new Text(paragraphText) { Space = SpaceProcessingModeValues.Preserve });
            currentParagraph.AppendChild<Run>(run);
            

        }

        public void AddParagraph(string paragraphText, ParagraphStyle paragraphType, ParagrapJustification textDirection, bool appendFootNote, string footNoteText)
        {


            paragraphText = System.Web.HttpUtility.HtmlDecode(paragraphText).Replace(";psbn&", " ").Replace("&nbsp;", " ");
 

            if (_docMainPart == null)
                _docMainPart = _currentDoc.AddMainDocumentPart();
            if (_docMainPart.Document == null)
                _docMainPart.Document = MakeEmpyDocument();
            
            Paragraph paragraph = new Paragraph();
            ParagraphProperties paragraphProp = new ParagraphProperties();
            BiDi biDi1 = new BiDi();
            Justification justification = new Justification();

            switch (textDirection)
            {
                case ParagrapJustification.RTL:
                    justification.Val = JustificationValues.Right;
                    break;
                case ParagrapJustification.LTR:
                    justification.Val = JustificationValues.Left;
                    break;
                case ParagrapJustification.Center:
                    justification.Val = JustificationValues.Center;
                    break;
                case ParagrapJustification.Both:
                    justification.Val = JustificationValues.Both;
                    paragraphProp.Append(biDi1);
                    break;
                default:
                    break;
            }
            
            
                paragraphProp.Append(justification);
                paragraphProp.ParagraphStyleId = new ParagraphStyleId() { Val = paragraphType.ToString() }; // we set the style
                paragraph.Append(paragraphProp);    //HeadingArabic

                Run run = new Run(new Text(paragraphText) { Space = SpaceProcessingModeValues.Preserve });

                

                switch (paragraphType)
                {
                    case ParagraphStyle.ParagraphTitle:
                        break;
                    case ParagraphStyle.NormalArabic:
                        break;
                    case ParagraphStyle.FootnoteText:
                       // footnote = new Footnote() { Type = FootnoteEndnoteValues.Separator, Id = -1 };
                        break;
                    case ParagraphStyle.FootnoteReference:
                   //     FootnoteReference footnoteRef = new FootnoteReference() { CustomMarkFollows = true, Id = 1 };
                  //  run.Append(footnoteRef);
                        break;
                    case ParagraphStyle.SeparatorMark:
                        SeparatorMark separatorMark = new SeparatorMark();                        
                        run.Append(separatorMark);
                        break;
                    default:
                        break;
                }


                paragraph.Append(run);

            if(appendFootNote)//if (paragraphType == ParagraphStyle.FootnoteText)
            {
                //SeparatorMark separatorMark2 = new SeparatorMark();
                //paragraph.Append(new Run(separatorMark2));

                //Footnote footnote = null;
                //footnote = new Footnote() { Type = FootnoteEndnoteValues.Normal, Id = -1 };

                //footnote.Append(paragraph);
                ////_docMainPart.Document.Body.Append(footnote);
                //_footNotes.Append(footnote);
                //_footnotesPart.Footnotes = _footNotes;
               
                //NewWordWorker n = new NewWordWorker();
                //n.GenerateEndnotesPart1Content(_endnotesPart);
                //n.GenerateFootnotesPart1Content(_footnotesPart, "عباس الهنداوي");


               
            }
            else
                _docMainPart.Document.Body.Append(paragraph);
            
            Save();
        }

        
        //public void GenerateAllFootNoteWork()
        //{
        //            GeneratedCode.GeneratedClass gc = new GeneratedCode.GeneratedClass();
        //            gc.CreateMainDocumentPart(_docMainPart);
        //            Save();
        //}

        
        public void Save() 
        {
            _docMainPart.Document.Save();// Save changes to the main document part.             
        }



        private void InitializeDocumentStyles()
        {


            
                // Create a style part and add it to the document
                XmlDocument stylesXml = new XmlDocument();
                stylesXml.Load(_docXmlParts.StylePartPath);

                _docStylePart = _docMainPart.AddNewPart<StyleDefinitionsPart>();
                //  Copy the style.xml content into the new part...
                using (Stream outputStream = _docStylePart.GetStream())
                {
                    using (StreamWriter ts = new StreamWriter(outputStream))
                    {
                        ts.Write(stylesXml.InnerXml);
                    }
                }

        
                 if(_docMainPart.StyleDefinitionsPart == null)
                    _docStylePart = _docMainPart.AddNewPart<StyleDefinitionsPart>();
                  else
                   _docStylePart = _docMainPart.StyleDefinitionsPart;


                //Run runNormalText = new Run() { RsidRunProperties = "007A6E3F" };
                RunProperties runNormalTextProp = new RunProperties();
                RunFonts runNormalTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial", AsciiTheme = ThemeFontValues.MinorBidi, HighAnsiTheme = ThemeFontValues.MinorBidi };
                FontSize fontNormalTextSize = new FontSize() { Val = "28" };
                FontSizeComplexScript fontComplexNormalTextSize = new FontSizeComplexScript() { Val = "28" };
                RightToLeftText rightToLeftText = new RightToLeftText();
                //BiDi biDi1 = new BiDi();
                //Languages languages = new Languages() { EastAsia = "ar-SA", Bidi = "ar-SA" };

                runNormalTextProp.Append(runNormalTextFonts);
                runNormalTextProp.Append(fontNormalTextSize);
                runNormalTextProp.Append(fontComplexNormalTextSize);
                runNormalTextProp.Append(rightToLeftText);
                //runNormalTextProp.Append(biDi1);
                //runNormalTextProp.Append(languages);
                
                Style styleNormalText = new Style();
                styleNormalText.StyleId = "NormalArabic"; //this is the ID of the style
                styleNormalText.Append(new Name() { Val = "Taya Madbatah Normal Text" }); //this is name                
                styleNormalText.Append(new BasedOn() { Val = "Normal" });// our style based on Normal style                
                styleNormalText.Append(new NextParagraphStyle() { Val = "Normal" });// the next paragraph is Normal type
                styleNormalText.Append(runNormalTextProp);//we are adding properties previously defined


                // Run runParagraphTitleText = new Run() { RsidRunProperties = "00CD365A" };
                RunProperties runParagraphTitleTextProp = new RunProperties();
                RunFonts runParagraphTitleTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                Bold bold = new Bold();
                BoldComplexScript boldComplexScript = new BoldComplexScript();
                FontSize fontParagraphTitleTextSize = new FontSize() { Val = "28" };
                FontSizeComplexScript fontParagraphTitleComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                RightToLeftText rtlTextParagraphTitle = new RightToLeftText();
                runParagraphTitleTextProp.Append(runParagraphTitleTextFonts);
                runParagraphTitleTextProp.Append(bold);
                runParagraphTitleTextProp.Append(boldComplexScript);
                runParagraphTitleTextProp.Append(fontParagraphTitleTextSize);
                runParagraphTitleTextProp.Append(fontParagraphTitleComplexScriptSize);
                runParagraphTitleTextProp.Append(rtlTextParagraphTitle);
                //runParagraphTitleTextProp.Append(languages);
                //runParagraphTitleText.Append(runParagraphTitleTextProp);
                Style styleParagraphTitle = new Style();
                styleParagraphTitle.StyleId = "ParagraphTitle";
                styleParagraphTitle.Append(new Name() { Val = "Taya Madbatah Paragraph Title Text" });
                styleParagraphTitle.Append(new BasedOn() { Val = "Normal" });
                styleParagraphTitle.Append(new NextParagraphStyle() { Val = "Normal" });
                styleParagraphTitle.Append(runParagraphTitleTextProp);



                // Run runParagraphTitleText = new Run() { RsidRunProperties = "00CD365A" };
                RunProperties runParagraphItalicTextProp = new RunProperties();
                RunFonts runParagraphItalicTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                Italic italic = new Italic();
                BoldComplexScript boldComplexScriptItalic = new BoldComplexScript();
                FontSize fontParagraphItalicTextSize = new FontSize() { Val = "28" };
                FontSizeComplexScript fontParagraphItalicComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                RightToLeftText rtlTextParagraphItalic = new RightToLeftText();
                runParagraphItalicTextProp.Append(runParagraphItalicTextFonts);
                runParagraphItalicTextProp.Append(italic);
                runParagraphItalicTextProp.Append(fontParagraphItalicTextSize);
                runParagraphItalicTextProp.Append(fontParagraphItalicComplexScriptSize);
                runParagraphItalicTextProp.Append(rtlTextParagraphItalic);
                //runParagraphTitleTextProp.Append(languages);
                //runParagraphTitleText.Append(runParagraphTitleTextProp);
                Style styleParagraphTitleItalic = new Style();
                styleParagraphTitleItalic.StyleId = "ParagraphItalic";
                styleParagraphTitleItalic.Append(new Name() { Val = "Taya Madbatah Paragraph Title Text" });
                styleParagraphTitleItalic.Append(new BasedOn() { Val = "Normal" });
                styleParagraphTitleItalic.Append(new NextParagraphStyle() { Val = "Normal" });
                styleParagraphTitleItalic.Append(runParagraphItalicTextProp);


                // Run runParagraphTitleText = new Run() { RsidRunProperties = "00CD365A" };
                RunProperties runParagraphBoldItalicTextProp = new RunProperties();
                RunFonts runParagraphBoldItalicTextFonts = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                Italic italicBoldItalic = new Italic();
                Bold boldBoldItalic = new Bold();
                BoldComplexScript boldComplexScriptBoldItalic = new BoldComplexScript();
                FontSize fontParagraphBoldItalicTextSize = new FontSize() { Val = "28" };
                FontSizeComplexScript fontParagraphBoldItalicComplexScriptSize = new FontSizeComplexScript() { Val = "28" };
                RightToLeftText rtlTextParagraphBoldItalic = new RightToLeftText();
                runParagraphBoldItalicTextProp.Append(runParagraphBoldItalicTextFonts);
                runParagraphBoldItalicTextProp.Append(italicBoldItalic);
                runParagraphBoldItalicTextProp.Append(boldBoldItalic);
                runParagraphBoldItalicTextProp.Append(boldComplexScriptBoldItalic);
                runParagraphBoldItalicTextProp.Append(fontParagraphBoldItalicTextSize);
                runParagraphBoldItalicTextProp.Append(fontParagraphBoldItalicComplexScriptSize);
                runParagraphBoldItalicTextProp.Append(rtlTextParagraphBoldItalic);
                //runParagraphTitleTextProp.Append(languages);
                //runParagraphTitleText.Append(runParagraphTitleTextProp);
                Style styleParagraphTitleBoldItalic = new Style();
                styleParagraphTitleBoldItalic.StyleId = "ParagraphBoldItalic";
                styleParagraphTitleBoldItalic.Append(new Name() { Val = "Taya Madbatah Paragraph Title Text" });
                styleParagraphTitleBoldItalic.Append(new BasedOn() { Val = "Normal" });
                styleParagraphTitleBoldItalic.Append(new NextParagraphStyle() { Val = "Normal" });
                styleParagraphTitleBoldItalic.Append(runParagraphBoldItalicTextProp);

                //(now from style)
                //RunProperties runFootNoteProp = new RunProperties();
                //RunFonts runFontsFootNote = new RunFonts() { ComplexScript = "Simplified Arabic" };
                //NoProof noProof = new NoProof();
                //FontSize fontSizeFootNote = new FontSize() { Val = "20" };
                //FontSizeComplexScript fontSizeComplexScriptFootNote = new FontSizeComplexScript() { Val = "20" };
                //RightToLeftText rtlFootNote = new RightToLeftText();
                //Languages languages = new Languages() { EastAsia = "ar-SA", Bidi = "ar-SA" };
                //runFootNoteProp.Append(runFontsFootNote);
                //runFootNoteProp.Append(noProof);
                //runFootNoteProp.Append(fontSizeFootNote);
                //runFootNoteProp.Append(fontSizeComplexScriptFootNote);
                //runFootNoteProp.Append(rtlFootNote);
                //runFootNoteProp.Append(languages);
                //Style styleFootNote = new Style() { Type = StyleValues.Paragraph, StyleId = "FootnoteText" };
                //SemiHidden semiHidden = new SemiHidden();
                //Rsid rsidFootNote = new Rsid() { Val = "00DB4CCA" };
                //styleFootNote.Append(new Name() { Val = "Taya Madbatah Foot Note Text" });
                //styleFootNote.Append(new BasedOn() { Val = "Normal" });
                //styleFootNote.Append(new NextParagraphStyle() { Val = "Normal" });
                //styleFootNote.Append(runFootNoteProp);


                //RunProperties runFootnoteRefProp = new RunProperties();
                //Rsid rsiFdootnoteRef = new Rsid() { Val = "008A7103" };
                //// RunStyle runStyleFootnoteRef = new RunStyle() { Val = "FootnoteReference" };
                //RunFonts runFontsFootnoteRef = new RunFonts() { Ascii = "Arial", HighAnsi = "Arial", ComplexScript = "Arial" };
                //FontSize fontSizeFootnoteRef = new FontSize() { Val = "28" };
                //FontSizeComplexScript fontSizeComplexScriptFootnoteRef = new FontSizeComplexScript() { Val = "28" };
                //RightToLeftText rtlFootnoteRef = new RightToLeftText();

                ////runFootnoteRefProp.Append(runStyleFootnoteRef);
                //runFootnoteRefProp.Append(runFontsFootnoteRef);
                //runFootnoteRefProp.Append(fontSizeFootnoteRef);
                //runFootnoteRefProp.Append(fontSizeComplexScriptFootnoteRef);
                //runFootnoteRefProp.Append(rtlFootnoteRef);
                ////Rsid rsidFootNoteRef = new Rsid() { Val = "00DB4CCA" };
                //Style styleFootNoteRef = new Style() { StyleId = "FootnoteReference" };
                //styleFootNoteRef.Append(new Name() { Val = "Taya Madbatah Foot Note Ref Text" });
                //styleFootNoteRef.Append(new BasedOn() { Val = "Normal" });
                //styleFootNoteRef.Append(new NextParagraphStyle() { Val = "Normal" });
                //styleFootNoteRef.Append(runFootnoteRefProp);


                _docStylePart.Styles = new Styles();
                _docStylePart.Styles.Append(styleNormalText);
                _docStylePart.Styles.Append(styleParagraphTitle);
                _docStylePart.Styles.Append(styleParagraphTitleItalic);
                _docStylePart.Styles.Append(styleParagraphTitleBoldItalic);
                //_docStylePart.Styles.Append(styleFootNoteRef);
                //_docStylePart.Styles.Append(styleFootNote);
                _docStylePart.Styles.Save();
            
               //// we have to set the properties
               // RunProperties runProp = new RunProperties();
               // runProp.Append(new Color() { Val = "FF0000" });// the color is red
               // runProp.Append(new RunFonts() { ComplexScript = "Arial" });
               // runProp.Append(new Bold()); 
               // runProp.Append(new FontSize() { Val = "28" }); //font size (in 1/72 of an inch)
               // Style styleHeading1 = new Style();
               // styleHeading1.StyleId = "Heading1"; //this is the ID of the style
               // styleHeading1.Append(new Name() { Val = "Taya Heading 1" }); //this is name                
               // styleHeading1.Append(new BasedOn() { Val = "Normal" });// our style based on Normal style                
               // styleHeading1.Append(new NextParagraphStyle() { Val = "Normal" });// the next paragraph is Normal type
               // styleHeading1.Append(runProp);//we are adding properties previously defined

               // RunProperties runArabicProp = new RunProperties();
               // runArabicProp.Append(new Color() { Val = "#00569f" });// the color is Blue
               // runArabicProp.Append(new RunFonts() { ComplexScript = "Arial" });
               // runArabicProp.Append(new Bold());
               // runArabicProp.Append(new FontSize() { Val = "14" }); //font size (in 1/72 of an inch)
                

                // we have to add style that we have created to the StylePart
 // we save the style part
        }

        public void Validate()
        {
            if (!IsOpen)
            {
                throw new DocumentNotOpenException("The object must be Open before calling Validate()");
            }

            if (_currentDoc != null)
            {
                OpenXmlValidator validator = new OpenXmlValidator();
                validator.Validate(_currentDoc);
                
            }
        }

        public void SaveAs(string fileName)
        {
            if (!IsOpen)
            {
                throw new DocumentNotOpenException("The object must be Open before calling SaveAs()");
            }

            _outputPath = fileName;
            if (!string.IsNullOrEmpty(_outputPath))
            {
                //Close the in-memory document to ensure the memory stream is ready for saving
                CloseInMemoryDocument();
                try
                {

                    //Now save the stream to file
                    using (FileStream fileStream = new FileStream(_outputPath, System.IO.FileMode.Create))
                    {
                        _inMemoryStream.WriteTo(fileStream);
                    }

                }
                finally
                {
                    //Now close the memory stream. 
                    //The in-memory document has already been closed above 
                    //and there's no point having one without the other!
                    CloseInMemoryStream();
                }
            }
        }
        //private Footer MakeFooterPart(string FooterText)
        //{
        //    var element =
        //      new Footer(
        //        new Paragraph(
        //          new ParagraphProperties(
        //            new ParagraphStyleId() { Val = "Footer" }),
        //          new Run(
        //            new Text(FooterText))
        //        )
        //      );

        //    return element;
        //}


        //public void AppendFooter(string sFtText)
        //{
        //    //using (WordprocessingDocument wdDoc = WordprocessingDocument.Open(sFile, true))
        //    //{
        //    FooterPart newFtPart = _docMainPart.AddNewPart<FooterPart>();
        //    string ft_ID = _docMainPart.GetIdOfPart(newFtPart);

        //        MakeFooterPart(sFtText).Save(newFtPart);
        //        foreach (SectionProperties sectProperties in
        //                _docMainPart.Document.Descendants<SectionProperties>())
        //        {
        //            FooterReference newFtReference =
        //             new FooterReference() { Id = ft_ID, Type = HeaderFooterValues.Default };
        //            sectProperties.Append(newFtReference);
        //        }
        //        //  Save the changes to the main document part.
        //        _docMainPart.Document.Save();
        //    //}
        //}


        /// <summary>
        /// 
        /// </summary>
        public void ApplyHeaderAndFooter()
        {
            if (!IsOpen)
            {
                throw new DocumentNotOpenException("The object must be Open before calling ApplyHeaderAndFooter()");
            }


            if (_docMainPart == null)
            {
                _docMainPart = _currentDoc.AddMainDocumentPart();
                _docMainPart.Document = MakeEmpyDocument();
            }
            else
            {
                // Delete the existing header and Footer parts.
                _docMainPart.DeleteParts(_docMainPart.HeaderParts);
                _docMainPart.DeleteParts(_docMainPart.FooterParts);
            }

            if (_docMainPart.Document == null)
                _docMainPart.Document = MakeEmpyDocument();

            //Multi-sectioned documents seem to have their SectionProperties hidden away inside paragraphs.
            //Go through all these first, and set a flag if we find any.
            bool paragraphSectionPropertiesFound = false;
            IEnumerable<Paragraph> paragraphElements = _docMainPart.Document.Body.Elements<Paragraph>().AsEnumerable();
            foreach (Paragraph paragraph in paragraphElements)
            {
                ParagraphProperties paragraphProperties = paragraph.Elements<ParagraphProperties>().LastOrDefault();
                if (paragraphProperties != null)
                {
                    SectionProperties sectionProps = paragraphProperties.Elements<SectionProperties>().LastOrDefault();
                    if (sectionProps != null)
                    {
                        ApplyHeaderToSectionProperties(sectionProps);
                        ApplyFooterToSectionProperties(sectionProps);
                        paragraphSectionPropertiesFound = true;
                    }
                }
            }

            //documents can also have SectionProperties under the body. Particularly single section Documents.
            SectionProperties bodySectionProperties = _docMainPart.Document.Body.Elements<SectionProperties>().LastOrDefault();

            //if no SectionProperties were found in paragraphs, we will populate the SectionProperties found in the Body instead
            if (paragraphSectionPropertiesFound)
            {
                //SectionProperties were set in the paragraph(s) which should be sufficient, 
                //We'll just remove any header/footer references we find in the Body SectionProperties
                if (bodySectionProperties != null)
                {
                    bodySectionProperties.RemoveAllChildren<HeaderReference>();
                    bodySectionProperties.RemoveAllChildren<FooterReference>();
                }
            }
            else
            {
                if (bodySectionProperties == null)
                {
                    //not there? create it
                    bodySectionProperties = new SectionProperties();
                    ApplyHeaderToSectionProperties(bodySectionProperties);
                    ApplyFooterToSectionProperties(bodySectionProperties);
                    _docMainPart.Document.Body.Append(bodySectionProperties);
                }
                else
                {
                    ApplyHeaderToSectionProperties(bodySectionProperties);
                    ApplyFooterToSectionProperties(bodySectionProperties);
                }
            }

            Save();
        }

        private void ApplyHeaderToSectionProperties(SectionProperties sectionProps)
        {
            // Create a new header part and grab its id for the header reference.
            HeaderPart headerPart = _docMainPart.AddNewPart<HeaderPart>();
            string rId = _docMainPart.GetIdOfPart(headerPart);

            //create our Header reference
            HeaderReference headerRef = new HeaderReference();
            headerRef.Id = rId;

            sectionProps.RemoveAllChildren<HeaderReference>();
            sectionProps.Append(headerRef);

            //Now populate the header contents
            headerPart.Header = MakeHeader();
            headerPart.Header.Save();
        }

        private void ApplyFooterToSectionProperties(SectionProperties sectionProps)
        {
            // Create a new Footer part and grab its id for the Footer reference.
            FooterPart footerPart = _docMainPart.AddNewPart<FooterPart>();
            string rId = _docMainPart.GetIdOfPart(footerPart);

            //create our Footer reference
            FooterReference footerRef = new FooterReference();
            footerRef.Id = rId;

            sectionProps.RemoveAllChildren<FooterReference>();
            sectionProps.Append(footerRef);

            //Now populate the Footer contents
            footerPart.Footer = MakeFooter();
            footerPart.Footer.Save();

        }




        public static Document MakeEmpyDocument()
        {
            Document doc = new Document();

            Body body = new Body();
            //Paragraph paragraph = new Paragraph();
            //Run run = new Run();
            //Text text = new Text("");
            //run.Append(text);
            //paragraph.Append(run);
            //body.Append(paragraph);
            doc.Append(body);

            return doc;
        }

        public Header MakeHeader()
        {
            Header header = new Header();
            Paragraph paragraph = new Paragraph();
            Run run = new Run();
            Text text = new Text();
            text.Text = "عباس الضو هيدر";
            run.Append(text);
            paragraph.Append(run);
            header.Append(paragraph);

            return header;
        }

        public Footer MakeFooter()
        {
            Footer footer = new Footer();

            ParagraphProperties paragraphProperties = new ParagraphProperties(
                            new ParagraphStyleId() { Val = "Footer" },
                            new Tabs(
                                new TabStop() { Val = TabStopValues.Clear, Position = 4320 },
                                new TabStop() { Val = TabStopValues.Clear, Position = 8640 },
                                new TabStop() { Val = TabStopValues.Center, Position = 4820 },
                                new TabStop() { Val = TabStopValues.Right, Position = 9639 }));

            Paragraph paragraph = new Paragraph(

                        paragraphProperties,
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.Begin }),
                        new Run(
                            new FieldCode(" TITLE   \\* MERGEFORMAT ") { Space = SpaceProcessingModeValues.Preserve }
                        ),
                        new Run(
                            new FieldChar() { FieldCharType = FieldCharValues.End }),

                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Center, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        ),
                        new Run(
                            new PositionalTab() { Alignment = AbsolutePositionTabAlignmentValues.Right, RelativeTo = AbsolutePositionTabPositioningBaseValues.Margin, Leader = AbsolutePositionTabLeaderCharValues.None }
                        ),
                        new Run(
                            new Text("صفحة ") { Space = SpaceProcessingModeValues.Preserve }
                        ),
                        new SimpleField(
                            new Run(
                                new RunProperties(
                                    new NoProof()),
                                new Text("1")
                            )
                        ) { Instruction = " PAGE   \\* MERGEFORMAT " }
                    );

            footer.Append(paragraph);

            return footer;
        }

        /// <summary>
        /// Closes and disposes the in-memory document
        /// </summary>
        
        private void CloseInMemoryDocument()
        {
            if (_currentDoc != null)
            {

                _currentDoc.Close();
                _currentDoc.Dispose();
                _currentDoc = null;
            }



        }
        /// <summary>
        /// Closes and disposes the memory stream
        /// </summary>
        private void CloseInMemoryStream()
        {
            if (_inMemoryStream != null)
            {
                _inMemoryStream.Close();
                _inMemoryStream.Dispose();
                _inMemoryStream = null;
            }
        }

        public void Close()
        {
            CloseInMemoryDocument();
            CloseInMemoryStream();
        }

        public void Dispose()
        {
            Close();
           
            
        }


      
       public static byte[] OpenAndCombine(IList<byte[]> documents)
            {
                MemoryStream mainStream = new MemoryStream();

                mainStream.Write(documents[0], 0, documents[0].Length);
                mainStream.Position = 0;

                int pointer = 1;
                byte[] ret;
                try
                {
                    using (WordprocessingDocument mainDocument = WordprocessingDocument.Open(mainStream, true))
                    {

                        XElement newBody = XElement.Parse(mainDocument.MainDocumentPart.Document.Body.OuterXml);

                        for (pointer = 1; pointer < documents.Count; pointer++)
                        {
                            WordprocessingDocument tempDocument = WordprocessingDocument.Open(new MemoryStream(documents[pointer]), true);
                            XElement tempBody = XElement.Parse(tempDocument.MainDocumentPart.Document.Body.OuterXml);

                            newBody.Add(tempBody);
                            mainDocument.MainDocumentPart.Document.Body = new Body(newBody.ToString());
                            mainDocument.MainDocumentPart.Document.Save();
                            mainDocument.Package.Flush();
                        }
                    }
                }
                catch (OpenXmlPackageException oxmle)
                {
                    //throw new OfficeMergeControlException(string.Format(CultureInfo.CurrentCulture, "Error while merging files. Document index {0}", pointer), oxmle);
                }
                catch (Exception e)
                {
                    //throw new OfficeMergeControlException(string.Format(CultureInfo.CurrentCulture, "Error while merging files. Document index {0}", pointer), e);
                }
                finally
                {
                    ret = mainStream.ToArray();
                    mainStream.Close();
                    mainStream.Dispose();
                }
                return (ret);
            }
       static void AppendPageBreak(WordprocessingDocument myDoc)
       {
           MainDocumentPart mainPart = myDoc.MainDocumentPart;
           OpenXmlElement last = myDoc.MainDocumentPart.Document
               .Body
               .Elements()
               .LastOrDefault(e => e is Paragraph || e is AltChunk);
           last.InsertAfterSelf(new Paragraph(
               new Run(
                   new Break() { Type = BreakValues.Page })));
       }
       public static void  MergeWithAltChunk(string outPath, string[] files)
       {
           //using (WordprocessingWorker myDoc = new WordprocessingWorker(outPath,GetDocParts(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\resources\"), DocFileOperation.Open))
           using (WordprocessingDocument myDoc = WordprocessingDocument.Open(outPath, true))
           {
               MainDocumentPart mainPart = myDoc.MainDocumentPart;
               /*if (mainPart == null)
                   mainPart = myDoc.AddMainDocumentPart();
               if (mainPart.Document == null)
                   mainPart.Document = MakeEmpyDocument();
               */
               for (int i = 0; i < files.Length; i++)
               {
                   string altChunkId = "AltChunkId"+i;
                   AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                   AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                   using (FileStream fileStream = File.Open(files[i], FileMode.Open))
                       chunk.FeedData(fileStream);
                   AltChunk altChunk = new AltChunk();
                   altChunk.Id = altChunkId;
                   //Ibrahim
                   //AltChunkProperties acp = new AltChunkProperties() { MatchSource = new MatchSource() { Val = OnOffValue.FromBoolean(true) } };
                   //altChunk.Append(acp);
                   //Paragraph  para = new Paragraph(new Run(new Break()));
                   //altChunk.Append(para);

                   mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                   if (i != files.Length - 1)
                       AppendPageBreak(myDoc);
                   //mainPart.Document.Append(para);
                   //mainPart.Document.Body.InsertAfter(para, mainPart.Document.Body.Elements<AltChunk>().Last());
                   
               }
               mainPart.Document.Save();
           }

           /*
           using (WordprocessingDocument myDoc = WordprocessingDocument.Open(doc1, true))
            {
                string altChunkId = "AltChunkId2";
                MainDocumentPart mainPart = myDoc.MainDocumentPart;
                AlternativeFormatImportPart chunk = mainPart.AddAlternativeFormatImportPart(
                AlternativeFormatImportPartType.WordprocessingML, altChunkId);
                using (FileStream fileStream = File.Open(doc2, FileMode.Open))
                chunk.FeedData(fileStream);
                AltChunk altChunk = new AltChunk();
                altChunk.Id = altChunkId;
                mainPart.Document.Body.InsertAfter(altChunk, mainPart.Document.Body.Elements<Paragraph>().Last());
                mainPart.Document.Save();
            }
          */
       }
       public static void InsertAPicture(string document, string fileName)
       {
           using (WordprocessingDocument wordprocessingDocument =
               WordprocessingDocument.Open(document, true))
           {

               MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
               //if (mainPart == null)
               //    mainPart = wordprocessingDocument.AddMainDocumentPart();
               //if (mainPart.Document == null)
               //    mainPart.Document = MakeEmpyDocument();

               ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);

               using (FileStream stream = new FileStream(fileName, FileMode.Open))
               {
                   imagePart.FeedData(stream);
               }

               AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
           }
       }

       public static void InsertAPicture(string document, string[] fileNames)
       {
           using (WordprocessingDocument wordprocessingDocument =
               WordprocessingDocument.Open(document, true))
           {
               MainDocumentPart mainPart = wordprocessingDocument.MainDocumentPart;
               if (mainPart == null)
                   mainPart = wordprocessingDocument.AddMainDocumentPart();
               if (mainPart.Document == null)
                   mainPart.Document = MakeEmpyDocument();
               ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg);
               foreach(string fileName in fileNames)
                   using (FileStream stream = new FileStream(fileName, FileMode.Open))
                   {
                       imagePart.FeedData(stream);
                   }

               AddImageToBody(wordprocessingDocument, mainPart.GetIdOfPart(imagePart));
           }
       }
       private static void AddImageToBody(WordprocessingDocument wordDoc, string relationshipId)
       {
           
           // Define the reference of the image.
           var element =
                new Drawing(
                    new DW.Inline(
                        new DW.Extent() { Cx = 990000L, Cy = 792000L },
                        new DW.EffectExtent()
                        {
                            LeftEdge = 0L,
                            TopEdge = 0L,
                            RightEdge = 0L,
                            BottomEdge = 0L
                        },
                        new DW.DocProperties()
                        {
                            Id = (UInt32Value)1U,
                            Name = "Picture 1"
                        },
                        new DW.NonVisualGraphicFrameDrawingProperties(
                            new A.GraphicFrameLocks() { NoChangeAspect = true }),
                        new A.Graphic(
                            new A.GraphicData(
                                new PIC.Picture(
                                    new PIC.NonVisualPictureProperties(
                                        new PIC.NonVisualDrawingProperties()
                                        {
                                            Id = (UInt32Value)0U,
                                            Name = "New Bitmap Image.jpg"
                                        },
                                        new PIC.NonVisualPictureDrawingProperties()),
                                    new PIC.BlipFill(
                                        new A.Blip(
                                            new A.BlipExtensionList(
                                                new A.BlipExtension()
                                                {
                                                    Uri =
                                                      "{28A0092B-C50C-407E-A947-70E740481C1C}"
                                                })
                                        )
                                        {
                                            Embed = relationshipId,
                                            CompressionState =
                                            A.BlipCompressionValues.Print
                                        },
                                        new A.Stretch(
                                            new A.FillRectangle())),
                                    new PIC.ShapeProperties(
                                        new A.Transform2D(
                                            new A.Offset() { X = 0L, Y = 0L },
                                            new A.Extents() { Cx = 990000L, Cy = 792000L }),
                                        new A.PresetGeometry(
                                            new A.AdjustValueList()
                                        ) { Preset = A.ShapeTypeValues.Rectangle }))
                            ) { Uri = "http://schemas.openxmlformats.org/drawingml/2006/picture" })
                    )
                    {
                        DistanceFromTop = (UInt32Value)0U,
                        DistanceFromBottom = (UInt32Value)0U,
                        DistanceFromLeft = (UInt32Value)0U,
                        DistanceFromRight = (UInt32Value)0U,
                        EditId = "50D07946"
                    });

           // Append the reference to body, the element should be in a Run.
           wordDoc.MainDocumentPart.Document.Body.AppendChild(new Paragraph(new Run(element)));
       }
       public void AddFootnote(string text)
       {
           FootnoteEndnoteReferenceType reference;
           reference = new FootnoteReference() { Id = AddFootnoteReference(text) };
           Run run;
           List<OpenXmlElement> elements = new List<OpenXmlElement>();
           elements.Add(
           run = new Run(
               new RunProperties(
                   new RunStyle() { Val = "Normal" }),
               reference)
           );
           Paragraph paragraph = new Paragraph();


           paragraph.Append(elements);

           _docMainPart.Document.Body.Append(paragraph);


           //_docMainPart.Document.Body.Append(run);

       }
       private Int32 footnotesRef = 1, endnotesRef = 1, figCaptionRef = -1;
       private int AddFootnoteReference(string description)
       {
           FootnotesPart fpart =  _docMainPart.FootnotesPart;
           if (fpart == null)
               fpart = _docMainPart.AddNewPart<FootnotesPart>();

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
                           new ParagraphStyleId() { Val = "Normal"}),//htmlStyles.GetStyle("footnote text", false) }),
                       markerRun = new Run(
                           new RunProperties(
                               new RunStyle() { Val = "Normal"}),//htmlStyles.GetStyle("footnote reference", true) }),
                           new FootnoteReferenceMark()),
                       new Run(
               // Word insert automatically a space before the definition to separate the reference number
               // with its description
                           new Text(" " + description) { Space = SpaceProcessingModeValues.Preserve })
                   )
               ) { Id = footnotesRef });

           /*if (!htmlStyles.DoesStyleExists("footnote reference"))
           {
               // Force the superscript style because if the footnote text style does not exists,
               // the rendering will be awful.
               markerRun.InsertInProperties(new VerticalTextAlignment() { Val = VerticalPositionValues.Superscript });
           }
           fpart.Footnotes.Save();
           */
           return footnotesRef;
       }

        

    }
}
