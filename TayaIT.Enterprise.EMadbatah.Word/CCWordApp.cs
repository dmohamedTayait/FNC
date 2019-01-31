using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Model;
using Microsoft.Office.Interop.Word;

namespace TayaIT.Enterprise.EMadbatah.Word
{
    public class CCWordApp
    {
        private Microsoft.Office.Interop.Word.Application oWordApplic;	// a reference to Word application
        private Microsoft.Office.Interop.Word.Document oDoc;					// a reference to the document


        public CCWordApp()
        {
            // activate the interface with the COM object of Microsoft Word
            oWordApplic = new Microsoft.Office.Interop.Word.Application();
        }

        // Open a file (the file must exists) and activate it
        public void Open(string strFileName, bool isreadOnly)
        {
            object fileName = strFileName;
            object readOnly = isreadOnly;
            object isVisible = true;
            object missing = System.Reflection.Missing.Value;

            oDoc = oWordApplic.Documents.Open(ref fileName, ref missing, ref readOnly,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing);
            oWordApplic.DisplayAlerts = WdAlertLevel.wdAlertsNone;//.SetWarnings = False
            oDoc.Activate();
            //Microsoft.Office.Interop.Word.Range range = oWordApplic.ActiveDocument.Content;
            //range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            //range.PageSetup.SectionDirection = WdSectionDirection.wdSectionDirectionRtl;
        }
        public void OpenForHtml(string strFileName)
        {
            object fileName = strFileName;
            object readOnly = false;
            object isVisible = true;
            object missing = System.Reflection.Missing.Value;

            oDoc = oWordApplic.Documents.Open(ref fileName, ref missing, ref readOnly,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref isVisible, ref missing, ref missing, ref missing);
            oWordApplic.DisplayAlerts = WdAlertLevel.wdAlertsNone;//.SetWarnings = False
            oDoc.Activate();
            oWordApplic.ActiveWindow.ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;
            //Microsoft.Office.Interop.Word.Range range = oWordApplic.ActiveDocument.Content;
            //range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            Microsoft.Office.Interop.Word.Range range = SelectAll().Range;
            
            // Change the formatting.
            
            range.ParagraphFormat.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
            range.PageSetup.SectionDirection = WdSectionDirection.wdSectionDirectionRtl;
            range.Font.Name = TextFont.Arial.ToString();
            range.Font.Size = 14;
            range.FormattedText.Font.Name = TextFont.Arial.ToString();
            range.FormattedText.Font.Size = 14;

            GoToTheEnd();
            InsertLineBreak(1);
        }
        public void New()
        {
            object readOnly = false;
            object isVisible = true;
            object missing = System.Reflection.Missing.Value;

            oDoc = oWordApplic.Documents.Add(ref missing, ref missing, ref missing, ref missing);

            oDoc.Activate();
            //Microsoft.Office.Interop.Word.Range range = oWordApplic.ActiveDocument.Content;
            //range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            //range.PageSetup.SectionDirection = WdSectionDirection.wdSectionDirectionRtl;

        }


        // Open a new document
        public void Open()
        {
            object missing = System.Reflection.Missing.Value;
            oDoc = oWordApplic.Documents.Add(ref missing, ref missing, ref missing, ref missing);
            oWordApplic.ActiveWindow.ActivePane.View.Type = Microsoft.Office.Interop.Word.WdViewType.wdPrintView;
            oWordApplic.DisplayAlerts = WdAlertLevel.wdAlertsNone;//.SetWarnings = False
            oDoc.Activate();

        }


        public Selection SelectAll()
        {
            // C#
            // Position the insertion point at the beginning 
            // of the document.
            Object unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            Object extend = Microsoft.Office.Interop.Word.WdMovementType.wdMove;
            oWordApplic.Selection.HomeKey(ref unit, ref extend);

            // Select from the insertion point to the end of the document.
            unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            extend = Microsoft.Office.Interop.Word.WdMovementType.wdExtend;
            oWordApplic.Selection.EndKey(ref unit, ref extend);

            return oWordApplic.Selection;
        }

        public void Quit()
        {
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Paragraphs.Format.ReadingOrder  = WdReadingOrder.wdReadingOrderRtl;
            //object missing = System.Reflection.Missing.Value;
            
            //oWordApplic.Application.Quit(ref missing, ref missing, ref missing);

            Object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
            Object originalFormat = Microsoft.Office.Interop.Word.WdOriginalFormat.wdWordDocument;// Type.Missing;
            Object routeDocument = false;// Type.Missing;
            //2-4-2012
            saveChanges = WdSaveOptions.wdSaveChanges;
            object oMissing = System.Reflection.Missing.Value;
            oDoc.Close(ref saveChanges, ref oMissing, ref oMissing);
            oDoc = null;



            oWordApplic.Quit(ref saveChanges,
                ref originalFormat, ref routeDocument);

            /*
            // Automatically save changes.
            Object saveChanges = Word.WdSaveOptions.wdSaveChanges;
            Object originalFormat = Type.Missing;
            Object routeDocument = Type.Missing;
            ThisApplication.Quit(ref saveChanges,
                ref originalFormat, ref routeDocument);

            // Prompt to save changes.
            saveChanges = Word.WdSaveOptions.wdPromptToSaveChanges;
            originalFormat = Type.Missing;
            routeDocument = Type.Missing;
            ThisApplication.Quit(ref saveChanges,
                ref originalFormat, ref routeDocument);

            // Quit without saving changes.
            saveChanges = Word.WdSaveOptions.wdDoNotSaveChanges;
            originalFormat = Type.Missing;
            routeDocument = Type.Missing;
            ThisApplication.Quit(ref saveChanges,
                ref originalFormat, ref routeDocument);
            */
        }

        public void Save()
        {
            oDoc.Save();
        }

        public void SaveAs(string strFileName)
        {
            object missing = System.Reflection.Missing.Value;
            object fileName = strFileName;

            oDoc.SaveAs(ref fileName, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        }

        // Save the document in HTML format
        public void SaveAsHtml(string strFileName)
        {
            object missing = System.Reflection.Missing.Value;
            object fileName = strFileName;
            object Format = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatHTML;
            oDoc.SaveAs(ref fileName, ref Format, ref missing, ref missing, ref missing, ref missing, ref missing,
                ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);
        }


        public void InsertImage(string imageFile)
        {
            Object oMissed = System.Reflection.Missing.Value; //the position you want to insert
            Object oLinkToFile = false;  //default
            Object oSaveWithDocument = true;//default
            //GoToTheEnd();
            oDoc.InlineShapes.AddPicture(imageFile, ref  oLinkToFile, ref  oSaveWithDocument, ref  oMissed);
            //InsertLineBreak(1);
        }
        public void InsertText(string strText, bool isBold, bool isItalic)
        {
            GoToTheEnd();
            if (isBold)
            {
                oWordApplic.Selection.Font.Bold = 1;
                oWordApplic.Selection.Font.BoldBi = 1;
                
            }
            else
                if (isItalic)
                {
                    oWordApplic.Selection.Font.Italic = 1;
                    oWordApplic.Selection.Font.ItalicBi = 1;

                }
                else
                {
                    SetFont();
                }

            oWordApplic.Selection.TypeText(strText);
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.PageSetup.SectionDirection = WdSectionDirection.wdSectionDirectionRtl;
            oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Paragraphs.Format.ReadingOrder  = WdReadingOrder.wdReadingOrderRtl;
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Font.Bold = 1;
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Font.BoldB = 1;


            //12-10-2011
            /*if (isBold)
            {
                //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.pa.Font.Bold = 1;
                //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.Font.BoldBi = 1;
                oDoc.Words[oDoc.Words.Count].Bold= 1;
                oDoc.Words[oDoc.Words.Count].BoldBi = 1;
            }
            if (isItalic)
            {
                oDoc.Words[oDoc.Words.Count].Italic = 1;
                oDoc.Words[oDoc.Words.Count].ItalicBi = 1;
            }*/

        }
        public void TypeParagraph()
        {
            GoToTheEnd();
            oWordApplic.Selection.TypeParagraph();
        }
        public void TypeBackspace()
        {
            GoToTheEnd();
            oWordApplic.Selection.TypeBackspace();
        }
        public void InsertHeading(string text, HeadingLevel level)
        {
            object headingType = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading2;
            switch (level)
            {
                case HeadingLevel.Heading1:
                    headingType = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading1;
                    break;
                case HeadingLevel.Heading2:
                    headingType = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading2;
                    break;
                case HeadingLevel.Heading3:
                    headingType = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading3;
                    break;
            }
            //object headingType = Microsoft.Office.Interop.Word.WdBuiltinStyle.wdStyleHeading2;

            //oWordApplic.ActiveWindow.Selection.Range.Text = text;//.PasteAndFormat(WdRecoveryType.wdPasteDefault);
            //oWordApplic.ActiveWindow.Selection.Range.set_Style(ref headingType);

            InsertText(text,false,false);
            
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.bo
            oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.set_Style(ref headingType);
            oDoc.Paragraphs[oDoc.Paragraphs.Count].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
        }
        //public void InsertTextWithFootnote(string strText, string footnote)
        //{
        //    oWordApplic.Selection.TypeText(strText);

        //}

        public void InsertLineBreak()
        {
            oWordApplic.Selection.TypeParagraph();
        }
        public void InsertLineBreak(int nline)
        {
            GoToTheEnd();
            for (int i = 0; i < nline; i++)
                oWordApplic.Selection.TypeParagraph();
        }

        public void InsertLineBreakAllignRight(int nline)
        {
            SetAlignment("Right");
            GoToTheEnd();
            for (int i = 0; i < nline; i++)
            {
                SetAlignment("Right");
                oWordApplic.Selection.TypeParagraph();
            }
        }

        // Change the paragraph alignement
        public void SetAlignment(string strType)
        {
            switch (strType)
            {
                case "Center":
                    oWordApplic.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;
                    break;
                case "Left":
                    oWordApplic.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphLeft;
                    break;
                case "Right":
                    oWordApplic.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
                    break;
                case "Justify":
                    oWordApplic.Selection.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphJustify;
                    break;
            }

        }


        // if you use thif function to change the font you should call it again with 
        // no parameter in order to set the font without a particular format
        public void SetFont(string strType)
        {
            switch (strType)
            {
                case "Bold":
                    oWordApplic.Selection.Font.Bold = 1;
                    oWordApplic.Selection.Font.BoldBi = 1;
                    break;
                case "Italic":
                    oWordApplic.Selection.Font.Italic = 1;
                    oWordApplic.Selection.Font.ItalicBi = 1;
                    break;
                case "Underlined":
                    oWordApplic.Selection.Font.Subscript = 0;
                    break;
                case "Normal":
                    oWordApplic.Selection.Font.Bold = 0;
                    oWordApplic.Selection.Font.BoldBi = 0;
                    oWordApplic.Selection.Font.Italic = 0;
                    oWordApplic.Selection.Font.ItalicBi = 0;
                    oWordApplic.Selection.Font.Subscript = 0;
                    break;
            }

        }

        public void SetFontColor(FontColor color)
        {
            switch (color)
            {
                case FontColor.Black:
                    oWordApplic.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlack;
                    break;
                case FontColor.Blue:
                    oWordApplic.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorBlue;
                    break;
                case FontColor.Green:
                    oWordApplic.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorGreen;
                    break;
                case FontColor.Red:
                    oWordApplic.Selection.Font.Color = Microsoft.Office.Interop.Word.WdColor.wdColorRed;
                    break;

            }

        }

        // disable all the style 
        public void SetFont()
        {
            oWordApplic.Selection.Font.Bold = 0;
            oWordApplic.Selection.Font.BoldBi = 0;
            oWordApplic.Selection.Font.Italic = 0;
            oWordApplic.Selection.Font.ItalicBi = 0;
            oWordApplic.Selection.Font.Subscript = 0;

        }

        public void SetFontName(string strType)
        {
            oWordApplic.Selection.Font.Name = strType;

        }

        public void SetFontSize(int nSize)
        {
            oWordApplic.Selection.Font.Size = nSize;
            oWordApplic.Selection.Font.SizeBi = nSize;
        }

        public void InsertPagebreak()
        {
            // VB : Selection.InsertBreak Type:=wdPageBreak
            object pBreak = (int)Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            oWordApplic.Selection.InsertBreak(ref pBreak);
        }

        // Go to a predefined bookmark, if the bookmark doesn't exists the application will raise an error

        public void GotoBookMark(string strBookMarkName)
        {
            // VB :  Selection.GoTo What:=wdGoToBookmark, Name:="nome"
            object missing = System.Reflection.Missing.Value;

            object Bookmark = (int)Microsoft.Office.Interop.Word.WdGoToItem.wdGoToBookmark;
            object NameBookMark = strBookMarkName;
            oWordApplic.Selection.GoTo(ref Bookmark, ref missing, ref missing, ref NameBookMark);
        }

        public void GoToTheEnd()
        {
            // VB :  Selection.EndKey Unit:=wdStory
            object missing = System.Reflection.Missing.Value;
            object unit;
            unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            oWordApplic.Selection.EndKey(ref unit, ref missing);

        }
        public void GoToTheBeginning()
        {
            // VB : Selection.HomeKey Unit:=wdStory
            object missing = System.Reflection.Missing.Value;
            object unit;
            unit = Microsoft.Office.Interop.Word.WdUnits.wdStory;
            oWordApplic.Selection.HomeKey(ref unit, ref missing);

        }

        public void GoToTheTable(int ntable)
        {
            //	Selection.GoTo What:=wdGoToTable, Which:=wdGoToFirst, Count:=1, Name:=""
            //    Selection.Find.ClearFormatting
            //    With Selection.Find
            //        .Text = ""
            //        .Replacement.Text = ""
            //        .Forward = True
            //        .Wrap = wdFindContinue
            //        .Format = False
            //        .MatchCase = False
            //        .MatchWholeWord = False
            //        .MatchWildcards = False
            //        .MatchSoundsLike = False
            //        .MatchAllWordForms = False
            //    End With

            object missing = System.Reflection.Missing.Value;
            object what;
            what = Microsoft.Office.Interop.Word.WdUnits.wdTable;
            object which;
            which = Microsoft.Office.Interop.Word.WdGoToDirection.wdGoToFirst;
            object count;
            count = 1;
            oWordApplic.Selection.GoTo(ref what, ref which, ref count, ref missing);
            oWordApplic.Selection.Find.ClearFormatting();

            oWordApplic.Selection.Text = "";


        }

        public void GoToRightCell()
        {
            // Selection.MoveRight Unit:=wdCell

            object missing = System.Reflection.Missing.Value;
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdCell;
            oWordApplic.Selection.MoveRight(ref direction, ref missing, ref missing);
        }

        public void GoToLeftCell()
        {
            // Selection.MoveRight Unit:=wdCell

            object missing = System.Reflection.Missing.Value;
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdCell;
            oWordApplic.Selection.MoveLeft(ref direction, ref missing, ref missing);
        }

        public void GoToDownCell()
        {
            // Selection.MoveRight Unit:=wdCell

            object missing = System.Reflection.Missing.Value;
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            oWordApplic.Selection.MoveDown(ref direction, ref missing, ref missing);
        }

        public void GoToUpCell()
        {
            // Selection.MoveRight Unit:=wdCell

            object missing = System.Reflection.Missing.Value;
            object direction;
            direction = Microsoft.Office.Interop.Word.WdUnits.wdLine;
            oWordApplic.Selection.MoveUp(ref direction, ref missing, ref missing);
        }


        // this function doesn't work
        public void InsertPageNumber(string strType, bool bHeader)
        {
            object missing = System.Reflection.Missing.Value;
            object alignment;
            object bFirstPage = false;
            object bF = true;
            //if (bHeader == true)
            //WordApplic.Selection.HeaderFooter.PageNumbers.ShowFirstPageNumber = bF;
            switch (strType)
            {
                case "Center":
                    alignment = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberCenter;
                    //WordApplic.Selection.HeaderFooter.PageNumbers.Add(ref alignment,ref bFirstPage);
                    //Microsoft.Office.Interop.Word.Selection objSelection = WordApplic.pSelection;

                    //oWordApplic.Selection.HeaderFooter.PageNumbers.Item(1).Alignment = Word.WdPageNumberAlignment.wdAlignPageNumberCenter;
                    break;
                case "Right":
                    alignment = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberRight;
                    //oWordApplic.Selection.HeaderFooter.PageNumbers.Item(1).Alignment = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberRight;
                    break;
                case "Left":
                    alignment = Microsoft.Office.Interop.Word.WdPageNumberAlignment.wdAlignPageNumberLeft;
                    oWordApplic.Selection.HeaderFooter.PageNumbers.Add(ref alignment, ref bFirstPage);
                    break;
            }

        }
        public void insertFooter(string text)
        {
            foreach (Microsoft.Office.Interop.Word.Section wordSection in oDoc.Sections)
            {
                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.ColorIndex = Microsoft.Office.Interop.Word.WdColorIndex.wdDarkRed;

                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Font.Size = 20;
                wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
                    .Range.Text = text;
            }
        }


        public void InsertPageHeader(string text)
        {

            try
            {

                /**/////Method 1 to Add Header

                //WordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;

                //WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;

                //WordApp.ActiveWindow.ActivePane.Selection.InsertAfter( "**" );//Header Content

                /**/////Method 2 to Add Header
                if (oWordApplic.ActiveWindow.ActivePane.View.Type == WdViewType.wdNormalView ||
                    oWordApplic.ActiveWindow.ActivePane.View.Type == WdViewType.wdOutlineView)        

                {

                    oWordApplic.ActiveWindow.ActivePane.View.Type = WdViewType.wdPrintView;

                }

                oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageHeader;

                oWordApplic.Selection.HeaderFooter.LinkToPrevious = false;

                oWordApplic.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                oWordApplic.Selection.HeaderFooter.Range.Text = text;

                //oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageFooter;

                //oWordApplic.Selection.HeaderFooter.LinkToPrevious = false;

                //oWordApplic.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                //oWordApplic.ActiveWindow.ActivePane.Selection.InsertAfter("Footer Content");

                //Quit Header and Footer Setting

                oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;

            }

            catch (Exception e)

           {

                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);

               

            }
        }

        public void InsertPageFooter(string text)
        {

            try
            {

                /**/
                ////Method 1 to Add Header

                //WordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;

                //WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;

                //WordApp.ActiveWindow.ActivePane.Selection.InsertAfter( "**" );//Header Content

                /**/
                ////Method 2 to Add Header
                if (oWordApplic.ActiveWindow.ActivePane.View.Type == WdViewType.wdNormalView ||
                    oWordApplic.ActiveWindow.ActivePane.View.Type == WdViewType.wdOutlineView)
                {

                    oWordApplic.ActiveWindow.ActivePane.View.Type = WdViewType.wdPrintView;

                }

                //oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageHeader;

                //oWordApplic.Selection.HeaderFooter.LinkToPrevious = false;

                //oWordApplic.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                //oWordApplic.Selection.HeaderFooter.Range.Text = "Header Content";

                oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageFooter;

                oWordApplic.Selection.HeaderFooter.LinkToPrevious = false;

                oWordApplic.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;

                oWordApplic.ActiveWindow.ActivePane.Selection.InsertAfter(text);

                //Quit Header and Footer Setting

                oWordApplic.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;

            }

            catch (Exception e)
            {

                Console.WriteLine(e.Message);

                Console.WriteLine(e.StackTrace);



            }
        }
        public void InsertPageNumInFooter()
        {
            //foreach (Microsoft.Office.Interop.Word.Section section in oDoc.Sections)
            //{
            //    object fieldEmpty = Microsoft.Office.Interop.Word.WdFieldType.wdFieldEmpty;
            //    object autoText = "صفحه  \"Page X of Y\" ";
            //    object preserveFormatting = true;

            //    section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range.Fields.Add(
            //        section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range,
            //        ref fieldEmpty, ref autoText, ref preserveFormatting);

            //    section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary]
            //    .Range.ParagraphFormat.Alignment = Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            //}

            object currentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            object totalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            object missing = System.Reflection.Missing.Value;
            // Go to the Footer view
            oWordApplic.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            // Right-align the current selection
            oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.Alignment =
                Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphCenter;

            oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;

            // Type the page number in 'Page X of Y' format
            oWordApplic.ActiveWindow.Selection.TypeText("صفحه ");
            oWordApplic.ActiveWindow.Selection.Fields.Add(
                oWordApplic.ActiveWindow.Selection.Range, ref currentPage, ref missing, ref missing);
            oWordApplic.ActiveWindow.Selection.TypeText(" من ");
            oWordApplic.ActiveWindow.Selection.Fields.Add(
                oWordApplic.ActiveWindow.Selection.Range, ref totalPages, ref missing, ref missing);

            // Go back to the Main Document view
            oWordApplic.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;

        }
        public void InsertPageNumInFooterWithSessionNum(string serial)
        {
            object currentPage = Microsoft.Office.Interop.Word.WdFieldType.wdFieldPage;
            object totalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            object missing = System.Reflection.Missing.Value;
            // Go to the Footer view
            oWordApplic.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekCurrentPageFooter;
            // Right-align the current selection
            oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.Alignment =
                Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;

               
            oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;

            // Type the page number in 'Page X of Y' format
            oWordApplic.ActiveWindow.Selection.TypeText(serial+ "	صفحه");
            oWordApplic.ActiveWindow.Selection.Fields.Add(
                oWordApplic.ActiveWindow.Selection.Range, ref currentPage, ref missing, ref missing);
            oWordApplic.ActiveWindow.Selection.TypeText(" من ");
            oWordApplic.ActiveWindow.Selection.Fields.Add(
                oWordApplic.ActiveWindow.Selection.Range, ref totalPages, ref missing, ref missing);

           
            oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.Format.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            //oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.set_Style = WdParagraphAlignment.wdAlignParagraphRight;
            //oWordApplic.ActiveWindow.ActivePane.Selection.Paragraphs.Alignment =
              //  Microsoft.Office.Interop.Word.WdParagraphAlignment.wdAlignParagraphRight;
            
            //oWordApplic.ActiveWindow.Selection.TypeText(serial);
            
            // Go back to the Main Document view
            oWordApplic.ActiveWindow.ActivePane.View.SeekView = Microsoft.Office.Interop.Word.WdSeekView.wdSeekMainDocument;

        }
        public  void InsertFooteNote(object footeNote)
        {
                object missing = System.Reflection.Missing.Value;
                //object text = "Sample footnote text.";
                //InsertText((string)paragraphText);

                //oDoc.Paragraphs[1].Range.InsertParagraphAfter();
                //oDoc.Paragraphs[1].Range.Text = (string)paragraphText;
                oDoc.Footnotes.Location = Microsoft.Office.Interop.Word.WdFootnoteLocation.wdBottomOfPage;
                oDoc.Footnotes.NumberStyle = Microsoft.Office.Interop.Word.WdNoteNumberStyle.wdNoteNumberStyleHindiArabic;
              
                //oDoc.Footnotes.Add(oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range.Words[oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range.Words.Count-1]
                  //  .Characters[oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range.Words[oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range.Words.Count - 1].Characters.Count -1 ], ref missing, ref footeNote);
                oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;    
                oDoc.Footnotes.Add(oDoc.Paragraphs[oDoc.Paragraphs.Count - 1].Range, ref missing, ref footeNote);
                oDoc.Footnotes[oDoc.Footnotes.Count].Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                oDoc.Footnotes[oDoc.Footnotes.Count].Range.ParagraphFormat.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
                oDoc.Footnotes.Separator.LanguageID = WdLanguageID.wdArabic;
                
            //oDoc.Paragraphs[1].Range.InsertParagraphBefore();
            //Microsoft.Office.Tools.Word.Bookmark bookmark1 =
            //    oDoc.Controls.AddBookmark(oDoc.Paragraphs[1].Range,
            //    "bookmark1");
            //bookmark1.Text = "This is sample bookmark text.";
            //bookmark1.FootnoteOptions.NumberStyle =
            //    Microsoft.Office.Tools.Word.WdNoteNumberStyle.wdNoteNumberStyleArabic;
            //bookmark1.FootnoteOptions.StartingNumber = 1;
            //bookmark1.Footnotes.Location =
            //    Microsoft.Office.Tools.Word.WdFootnoteLocation.wdBeneathText;
            //bookmark1.Footnotes.Add(bookmark1.Range,
            //    ref missing, ref Text);
        }

        public void InsertBulletList(List<string> list, TextFont font, BasicFormat format)
        {
            //oDoc.Paragraphs.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
            InsertText("",false,false);
            object oMissing = System.Reflection.Missing.Value;
            oDoc.Paragraphs[oDoc.Paragraphs.Count].ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
            Range r = oDoc.Paragraphs[oDoc.Paragraphs.Count].Range;

            r.Text = "";
            r.LanguageID = WdLanguageID.wdArabic;
           

            switch (font)
            {
                case TextFont.Arial:
                    r.Font.Name = TextFont.Arial.ToString();
                    break;
                case TextFont.TimesNewRoman:
                    r.Font.Name = TextFont.TimesNewRoman.ToString();
                    break;
            }

            switch (format.textStyle)
            {
                case TextStyle.Bold:
                    r.Font.Bold = 1;
                    break;
                case TextStyle.BoldItalic:
                    r.Font.Bold = 1;
                    r.Font.Italic = 1;
                    break;
                case TextStyle.BoldUnderlined:
                    r.Font.Bold = 1;
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
                case TextStyle.Italic:
                    r.Font.Italic = 1;
                    break;
                case TextStyle.ItalicUnderLined:
                    r.Font.Italic = 1;
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
                case TextStyle.Normal:
                    break;
                case TextStyle.Underlined:
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
            }
            switch (format.align)
            {
                case Alignment.Right:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                    break;
                case Alignment.Left:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    break;
                case Alignment.Center:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    break;
                case Alignment.Justify:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    break;
            }

            
            r.InsertParagraphAfter();
            r.Start = r.End;
            r.Collapse(oMissing);
            string desc = "";
            
            foreach (string str in list)
            {
                desc += str;
                desc += Environment.NewLine;
            }

            r.Text = desc;
            r.ListFormat.ApplyListTemplate(oWordApplic.ListGalleries[Microsoft.Office.Interop.Word.WdListGalleryType.wdBulletGallery].ListTemplates[1], false, Microsoft.Office.Interop.Word.WdListApplyTo.wdListApplyToSelection, oMissing);
            r.ListFormat.ListIndent();
            

            r.InsertParagraphAfter();
            r.Start = r.End;
            r.ListFormat.RemoveNumbers(Microsoft.Office.Interop.Word.WdNumberType.wdNumberParagraph);
            r.Collapse(oMissing);

           
            
            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            // NEXT THREE LINES ARE DUMMY VARIABLES FOR TESTING ONLY
            //int numberOfSections = 3;
            //string[] bodyParts = { "neck", "back", "left arm" };
            //int[] numberOfNotes = { 2, 4, 3 };
            //for (int i = 0; i < numberOfSections; i++)
            //{
            //    r.Text = "Examination of " + bodyParts[i];
            //    r.Font.Bold = 1;
            //    r.InsertParagraphAfter();
            //    r.Start = r.End;
            //    r.Collapse(oMissing);
            //    string desc = "";
            //    for (int j = 0; j < numberOfNotes[i]; j++)
            //    {
            //        desc += "This is comment number " + j.ToString();
            //        if (j < numberOfNotes[i] - 1)
            //        {
            //            desc += Environment.NewLine;
            //        }
            //    }
            //    r.Text = desc;
            //    r.ListFormat.ApplyListTemplate(oWordApplic.ListGalleries[Microsoft.Office.Interop.Word.WdListGalleryType.wdBulletGallery].ListTemplates[1], false, Microsoft.Office.Interop.Word.WdListApplyTo.wdListApplyToSelection, oMissing);
            //    r.ListFormat.ListIndent();
            //    r.InsertParagraphAfter();
            //    r.Start = r.End;
            //    r.ListFormat.RemoveNumbers(Microsoft.Office.Interop.Word.WdNumberType.wdNumberParagraph);
            //    r.Collapse(oMissing);
            //}
        }

        public void InsertNumberList(List<string> list, TextFont font, BasicFormat format)
        {
            //oDoc.Paragraphs.ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
            InsertText("",false,false);
            object oMissing = System.Reflection.Missing.Value;
            oDoc.Paragraphs[oDoc.Paragraphs.Count].ReadingOrder = WdReadingOrder.wdReadingOrderRtl;
            Range r = oDoc.Paragraphs[oDoc.Paragraphs.Count].Range;

            r.Text = "";
            r.LanguageID = WdLanguageID.wdArabic;


            switch (font)
            {
                case TextFont.Arial:
                    r.Font.Name = TextFont.Arial.ToString();
                    break;
                case TextFont.TimesNewRoman:
                    r.Font.Name = TextFont.TimesNewRoman.ToString();
                    break;
            }

            switch (format.textStyle)
            {
                case TextStyle.Bold:
                    r.Font.Bold = 1;
                    break;
                case TextStyle.BoldItalic:
                    r.Font.Bold = 1;
                    r.Font.Italic = 1;
                    break;
                case TextStyle.BoldUnderlined:
                    r.Font.Bold = 1;
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
                case TextStyle.Italic:
                    r.Font.Italic = 1;
                    break;
                case TextStyle.ItalicUnderLined:
                    r.Font.Italic = 1;
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
                case TextStyle.Normal:
                    break;
                case TextStyle.Underlined:
                    r.Font.Underline = WdUnderline.wdUnderlineDash;
                    break;
            }
            switch (format.align)
            {
                case Alignment.Right:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphRight;
                    break;
                case Alignment.Left:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphLeft;
                    break;
                case Alignment.Center:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                    break;
                case Alignment.Justify:
                    r.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphJustify;
                    break;
            }


            r.InsertParagraphAfter();
            r.Start = r.End;
            r.Collapse(oMissing);
            string desc = "";

            foreach (string str in list)
            {
                desc += str;
                desc += Environment.NewLine;
            }

            r.Text = desc;
            r.ListFormat.ApplyListTemplate(oWordApplic.ListGalleries[Microsoft.Office.Interop.Word.WdListGalleryType.wdNumberGallery].ListTemplates[1], false, Microsoft.Office.Interop.Word.WdListApplyTo.wdListApplyToSelection, oMissing);
            r.ListFormat.ListIndent();


            r.InsertParagraphAfter();
            r.Start = r.End;
            r.ListFormat.RemoveNumbers(Microsoft.Office.Interop.Word.WdNumberType.wdNumberParagraph);
            r.Collapse(oMissing);



            //oDoc.Paragraphs[oDoc.Paragraphs.Count].Alignment = WdParagraphAlignment.wdAlignParagraphRight;
            // NEXT THREE LINES ARE DUMMY VARIABLES FOR TESTING ONLY
            //int numberOfSections = 3;
            //string[] bodyParts = { "neck", "back", "left arm" };
            //int[] numberOfNotes = { 2, 4, 3 };
            //for (int i = 0; i < numberOfSections; i++)
            //{
            //    r.Text = "Examination of " + bodyParts[i];
            //    r.Font.Bold = 1;
            //    r.InsertParagraphAfter();
            //    r.Start = r.End;
            //    r.Collapse(oMissing);
            //    string desc = "";
            //    for (int j = 0; j < numberOfNotes[i]; j++)
            //    {
            //        desc += "This is comment number " + j.ToString();
            //        if (j < numberOfNotes[i] - 1)
            //        {
            //            desc += Environment.NewLine;
            //        }
            //    }
            //    r.Text = desc;
            //    r.ListFormat.ApplyListTemplate(oWordApplic.ListGalleries[Microsoft.Office.Interop.Word.WdListGalleryType.wdBulletGallery].ListTemplates[1], false, Microsoft.Office.Interop.Word.WdListApplyTo.wdListApplyToSelection, oMissing);
            //    r.ListFormat.ListIndent();
            //    r.InsertParagraphAfter();
            //    r.Start = r.End;
            //    r.ListFormat.RemoveNumbers(Microsoft.Office.Interop.Word.WdNumberType.wdNumberParagraph);
            //    r.Collapse(oMissing);
            //}
        }

        public void InsertHyperLink( string url)
        {
            // Insert a hyperlink to the Web page.
            //InsertText(text);
            object oMissing = System.Reflection.Missing.Value;
            Range r = oDoc.Paragraphs[oDoc.Paragraphs.Count].Range;
            r.LanguageID = WdLanguageID.wdArabic;
            
            Object oAddress = url;
            
            oDoc.Hyperlinks.Add(r, ref oAddress, ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }

        public int GetCurrentPageNumber()
        {
            object oMissing = System.Reflection.Missing.Value;
            Microsoft.Office.Interop.Word.WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
            int num = oDoc.ComputeStatistics(stat, ref oMissing);
            return num;

        }
        private int DocumentComputeStatistics()
        {
            object IncludeFootnotesAndEndnotes = false;

            int wordCount = oDoc.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages, ref IncludeFootnotesAndEndnotes);
            return wordCount;
            //MessageBox.Show("There are " + wordCount.ToString() + " words in this document.");
        }

        public int GetNumPages()
        {
            GoToTheEnd();
            //return GetCurrentPageNumber();
            object oMissing = System.Reflection.Missing.Value;
            object totalPages = Microsoft.Office.Interop.Word.WdFieldType.wdFieldNumPages;
            Microsoft.Office.Interop.Word.WdStatistic stat = Microsoft.Office.Interop.Word.WdStatistic.wdStatisticPages;
            int num = oDoc.ComputeStatistics(stat, ref oMissing);
            num = DocumentComputeStatistics();
            return num;
        }

        public void FindReplace(string src, string destination)
        {
            oWordApplic.Visible = true;
            Microsoft.Office.Interop.Word.Range range = oWordApplic.ActiveDocument.Content;

            object findtext = src;
            object findreplacement = destination;
            object findforward = true;
            object findformat = false;
            object findwrap = Microsoft.Office.Interop.Word.WdFindWrap.wdFindContinue;
            object findmatchcase = false;
            object findmatchwholeword = false;
            object findmatchwildcards = false;
            object findmatchsoundslike = false;
            object findmatchallwordforms = false;
            object findreplace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            object nevim = false;


            range.Find.Execute(ref findtext, ref findmatchcase, ref findmatchwholeword, ref findmatchwildcards,
            ref findmatchsoundslike, ref findmatchallwordforms, ref findforward, ref findwrap,
            ref findformat, ref findreplacement, ref findreplace, ref nevim, ref nevim, ref nevim, ref nevim);

            
            

        }
        public void ReplaceWithPageBreak(string text)
        {
            object pBreak = (int)Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            foreach (Microsoft.Office.Interop.Word.Range docRange in oDoc.Words)
            {
                if (docRange.Text.Trim().Equals(text))
                {
                    docRange.InsertBreak(pBreak);
                    //docRange.HighlightColorIndex =
                    //  Microsoft.Office.Interop.Word.WdColorIndex.wdDarkYellow;
                    //docRange.Font.ColorIndex =
                    //  Microsoft.Office.Interop.Word.WdColorIndex.wdWhite;
                }
            }

        }
    }



    //object units = WdUnits.wdCharacter;
    //object last=doc.Characters.Count;
    //doc.Range(ref first, ref last).Delete(ref units, ref last)
}
