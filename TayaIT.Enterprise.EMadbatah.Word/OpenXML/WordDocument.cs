using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Drawing;

namespace TayaIT.Enterprise.EMadbatah.Word.OpenXML
{
    public class WordDocument
    {
        private CCWordApp wordApp;
        private bool _isClosed = false;
        public bool IsClosed
        {
            get
            {
                return _isClosed;
            }

            set
            {
                _isClosed = value;
            }
        }
        public WordDocument()
        {
            wordApp = new CCWordApp();
        }
        public void CreateNew()
        {
            wordApp.New();
        }
        public void Open(string filePath, bool isreadOnly)
        {
            wordApp.Open(filePath, false);
        }
        public void OpenForHtml(string filePath)
        {
            wordApp.OpenForHtml(filePath);
        }
        public void SaveDocument()
        {
            wordApp.Save();
        }
        public void initializeStyle()
        {
            wordApp.SetFont();
        }
        public int GetNumPages()
        {
            return wordApp.GetNumPages();
        }
        public void insertText(string text, int fontSize, BasicFormat format, FontColor fontColor, TextFont font)
        {
            switch (font)
            {
                case TextFont.Arial:
                    wordApp.SetFontName("Arial");
                    break;
                case TextFont.TimesNewRoman:
                    wordApp.SetFontName("Times New Roman");
                    break;
            }
            bool isBold = false;
            bool isItalic = false;
            switch (format.textStyle)
            {
                case TextStyle.Normal:
                    wordApp.SetFont("Normal");
                    wordApp.SetFont();
                    break;
                case TextStyle.Bold:
                    wordApp.SetFont("Bold");
                    isBold = true;
                    break;
                case TextStyle.Italic:
                    wordApp.SetFont("Italic");
                    isItalic = true;
                    break;
                case TextStyle.Underlined:
                    wordApp.SetFont("Underlined");
                    break;
                case TextStyle.BoldItalic:
                    wordApp.SetFont("Bold");
                    wordApp.SetFont("Italic");
                    isBold = true;
                    isItalic = true;
                    break;
                case TextStyle.BoldUnderlined:
                    wordApp.SetFont("Bold");
                    wordApp.SetFont("Underlined");
                    isBold = true;
                    break;
                case TextStyle.ItalicUnderLined:
                    wordApp.SetFont("Italic");
                    wordApp.SetFont("Underlined");
                    isItalic = true;
                    break;
            }
            wordApp.SetFontSize(fontSize);

            switch (format.align)
            {
                case Alignment.Center:
                    wordApp.SetAlignment("Center");
                    break;
                case Alignment.Right:
                    wordApp.SetAlignment("Right");
                    break;
                case Alignment.Left:
                    wordApp.SetAlignment("Left");
                    break;
                case Alignment.Justify:
                    wordApp.SetAlignment("Justify");
                    break;
            }

            wordApp.InsertText(text, isBold, isItalic) ;
        }
        public void SaveDocument(string outFilePath)
        {
            //string tmp = myDoc.Content;
            wordApp.SaveAs(outFilePath);
        }
        public void insertBreakLine(int numBreakLines)
        {
            wordApp.InsertLineBreak(numBreakLines);
        }
        public void Quit()
        {
            try
            {
                wordApp.Quit();
            }
            catch { }
            IsClosed = true;
        }
        public void InsertImage(string imageFile)
        {
            wordApp.InsertImage(imageFile);
        }
        public void typeParagraph()
        {
            wordApp.TypeParagraph();
        }
        public void typeBackspace()
        {
            wordApp.TypeBackspace();
        }

        public void inserFooter(string text)
        {
            //wordApp.insertFooter(text);
            wordApp.InsertPageFooter(text);
        }
        public void insertHeader(string text)
        {
            wordApp.InsertPageHeader(text);
        }
        public void insertPageNumInFooter()
        {
            wordApp.InsertPageNumInFooter();
        }
        public void InsertHeading(string text, HeadingLevel level)
        {
            wordApp.InsertHeading(text, level);
        }
        public void AddFooteNote( object footeNote)
        {
            wordApp.InsertFooteNote(footeNote);
        }
        public void AddBulletList(List<string> list, TextFont font, BasicFormat fomrat)
        {
            wordApp.InsertBulletList(list, font, fomrat);
        }
        public void AddNumberList(List<string> list, TextFont font, BasicFormat fomrat)
        {
            wordApp.InsertNumberList(list, font, fomrat);
        }
        public void InsertHyperLink(string url)
        {
            wordApp.InsertHyperLink(url);
        }
        public int GetCurrentPageNumber()
        {
            return wordApp.GetCurrentPageNumber();
        }
        public void InsertPageNumInFooterWithSessionNum(string serial)
        {
            wordApp.InsertPageNumInFooterWithSessionNum(serial);
        }
        public void FindReplace(string src, string dest)
        {
            wordApp.FindReplace(src, dest);
        }
        public void ReplaceWithPageBreak(string text)
        {
            wordApp.ReplaceWithPageBreak(text);
        }
        public void GoToTheBegining()
        {
            wordApp.GoToTheBeginning();
        }
    }
}
