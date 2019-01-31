using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Word.Api.Interfaces;
using Word.W2004;
using Word.W2004.Elements;
using Word.W2004.Elements.TableElements;
using Word.W2004.Style;
using Font = Word.W2004.Style.Font;
using Image = Word.W2004.Elements.Image;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Drawing;

namespace TayaIT.Enterprise.EMadbatah.Word
{
    public class DocumentSharp2Word
    {
        IDocument myDoc;
        public DocumentSharp2Word()
        {
            myDoc = new Document2004();
            
        }
        public void setDocumentProperties(string author,
                                           string title, string subject)
        {
            Properties prop = new Properties
            {
                //AppName = "Sharp2Word",
                Author = author,
                Title = title,
                Subject = subject,
                LastSaved = DateTime.Now
            };
            myDoc.Head.Properties = prop;
        }
        public void insertBreakLine(int numBreakLines)
        {
            myDoc.AddEle(BreakLine.SetTimes(numBreakLines).Create()); 
        }

        public void insertHeading(string text, HeadingLevel level, BasicFormat format)
        {
            //HeadingStyle style = Heading3.With(text).WithStyle();
            //HeadingStyle style = new HeadingStyle();
            HeadingStyle style = null;
            switch (level)
            {
                case HeadingLevel.Heading1:
                    style = Heading1.With(text).WithStyle();
                    //Heading1 heading1 = new Heading1(text);
                    //heading1.Style = style;

                    //myDoc.AddEle(heading1);
                    //myDoc.AddEle(Heading1.With(style.GetNewContentWithStyle(text)).Create());
                    //myDoc.AddEle(heading1);
                    //myDoc.AddEle(Heading1.With(text).);
                    break;
                case HeadingLevel.Heading2:
                    style = Heading2.With(text).WithStyle();
                    //Heading2 heading2 = new Heading2(text);
                    //heading2.Style = style;
                    //myDoc.AddEle(heading2.Create());
                    break;
                case HeadingLevel.Heading3:
                    style = Heading3.With(text).WithStyle();
                    //Heading3 heading3 = new Heading3(text);
                    //heading3.Style = style;
                    //myDoc.AddEle(heading3.Create());
                    break;

            }


            switch (format.align)
            {
                case Alignment.Center:
                    style = style.Align(Align.CENTER);
                    break;
                case Alignment.Right:
                    style = style.Align(Align.RIGHT);
                    break;
                case Alignment.Left:
                    style = style.Align(Align.LEFT);
                    break;
            }
            switch (format.textStyle)
            {
                case TextStyle.Normal:
                    break;
                case TextStyle.Bold:
                    style = style.Bold();
                    break;
                case TextStyle.Italic:
                    style = style.Italic();
                    break;
                case TextStyle.BoldItalic:
                    style = style.Bold().Italic();
                    break;
                case TextStyle.BoldUnderlined:
                    //style = style.Bold().Un();
                    break;
                case TextStyle.ItalicUnderLined:
                    break;
                //case TextStyle.Underlined:
                //    style = style.un;
                //    break;
            }
            myDoc.AddEle(style.Create());
            
            //myDoc.AddEle(Heading3.With(text).WithStyle().Bold()
            //                .Align(Align.RIGHT).Create());
        }

        public void insertParagraphPeice(string text, int fontSize, BasicFormat format, Color fontColor, TextFont font)
        {
            ParagraphPieceStyle style = ParagraphPiece.With(text).WithStyle();
            style = style.FontSize(fontSize);
           
            style = style.TextColor(fontColor);

            //switch (format.align)
            //{
            //    case Align.LEFT:
            //        style = style.Ali
            //        break;
            //}
            switch (format.textStyle)
            {
                case TextStyle.Normal:
                    break;
                case TextStyle.Bold:
                    style = style.Bold;
                    break;
                case TextStyle.Italic:
                    style = style.Italic;
                    break;
                case TextStyle.Underlined:
                    style = style.Underline;
                    break;
                case TextStyle.BoldItalic:
                    style = style.Bold.Italic;
                    break;
                case TextStyle.BoldUnderlined:
                    style = style.Bold.Underline;
                    break;
                case TextStyle.ItalicUnderLined:
                    style = style.Italic.Underline;
                    break;
            }
            switch (font)
            {
                case TextFont.Arial:
                    style = style.Font(Font.ARIAL);
                    break;
                case TextFont.TimesNewRoman:
                    style = style.Font(Font.TIMES_NEW_ROMAN);
                    break;
            }
            //ParagraphPiece paragraph = ParagraphPiece.With(text);
            //paragraph.Style = style;
            //.WithStyle().Bold.Italic.Create();

            //myDoc.AddEle(style.Create());

            
            //working but no alignment
            //myDoc.AddEle(Paragraph.WithPieces(style.Create()));

            //for adding alignment
            Paragraph p = new Paragraph(style.Create());
            switch (format.align)
            {
                case Alignment.Center :
                    p.WithStyle().Align(Align.CENTER);      
                    break;
                case Alignment.Left:
                    p.WithStyle().Align(Align.LEFT);      
                    break;
                case Alignment.Right:
                    p.WithStyle().Align(Align.RIGHT);
                    break;
            }

            
            //p.WithStyle().Align(Align.RIGHT);
            myDoc.AddEle(p.Create());
            //myDoc.AddEle(Paragraph.WithPieces(ParagraphPiece.With("No size").Create(),
            //                                  ParagraphPiece.With("I am size 50.").WithStyle().FontSize(50).TextColor(Color.Cyan).Create()));


        }
        public void saveDocument(string outFilePath)
        {
            //string tmp = myDoc.Content;
            myDoc.Save(outFilePath);
        }
        public void test()
        {
            IDocument myDoc = new Document2004();

            Properties prop = new Properties
            {
                //AppName = "Sharp2Word",
                Author = "Dublicator",
                LastSaved = DateTime.Now
            };
            myDoc.Head.Properties = prop;

            myDoc.AddEle(BreakLine.SetTimes(1).Create()); // this is one breakline

            //Headings
            myDoc.AddEle(Heading2.With("===== Headings ======").Create());
            myDoc.AddEle(
                Paragraph.With(
                    "This doc has been generated by Sharp2Word.")
                    .Create());
            myDoc.AddEle(BreakLine.SetTimes(1).Create());

            myDoc.AddEle(Paragraph
                             .With("I will try to use a little bit of everything in the API Sharp2word. " +
                                   "I realised that is very dificult to keep the doucmentation updated " +
                                   "so this is where I will demostrate how to do some cool things with Sharp2Word!")
                             .Create());


            myDoc.AddEle(Heading1.With("Heading01 without styling").Create());
            myDoc.AddEle(Heading2.With("Heading02 with styling").WithStyle()
                             .Align(Align.CENTER).Italic().Create());
            myDoc.AddEle(Heading3.With("Heading03").WithStyle().Bold()
                             .Align(Align.RIGHT).Create());

            //Paragraph and ParagrapPiece
            myDoc.AddEle(Heading2.With("===== Paragraph and ParagrapPiece ======").Create());
            myDoc.AddEle(Paragraph.With("I am a very simple paragraph.").Create());

            myDoc.AddEle(BreakLine.SetTimes(1).Create());
            ParagraphPiece myParPiece01 =
                ParagraphPiece.With(
                    "If you use the class 'Paragraph', you will have limited style. Maybe only paragraph aligment.");
            ParagraphPiece myParPiece02 =
                ParagraphPiece.With("In order to use more advanced style, you have to use ParagraphPiece");
            ParagraphPiece myParPiece03 =
                ParagraphPiece.With(
                    "One example of this is when you want to make ONLY one word BOLD or ITALIC. the way to to this is create many pieces, format them separetely and put all together in a Paragraph object. Example:");

            myDoc.AddEle(Paragraph.WithPieces(myParPiece01, myParPiece02, myParPiece03).Create());

            ParagraphPiece myParPieceJava = ParagraphPiece.With("I like C# and ").WithStyle().Font(Font.COURIER).Create();
            ParagraphPiece myParPieceRuby = ParagraphPiece.With("Ruby!!! ").WithStyle().Bold.Italic.Create();
            ParagraphPiece myParPieceAgile =
                ParagraphPiece.With("I actually love C#, TDD, patterns... ").WithStyle().
                    TextColor("008000").Create();

            myDoc.AddEle(Paragraph.WithPieces(myParPieceJava, myParPieceRuby, myParPieceAgile).Create());

            //font size
            myDoc.AddEle(Paragraph.WithPieces(ParagraphPiece.With("No size").Create(),
                                              ParagraphPiece.With("I am size 50.").WithStyle().FontSize(50).TextColor(Color.Cyan).Create()));

            //Document Header and Footer
            myDoc.AddEle(BreakLine.SetTimes(2).Create());
            myDoc.AddEle(Heading2.With("===== Document Header and Footer ======").Create());
            myDoc.AddEle(Paragraph.With("By default everything is added to the Body when you do 'myDoc.AddEle(...)'." +
                                        " But you can add elements to the Header and/or Footer. Other cool thing is show page number or not.")
                             .Create());

            myDoc.AddEle(BreakLine.SetTimes(2).Create());
            myDoc.AddEle(
                Paragraph.With(
                    "Page number is displayed by default but you can disable: 'myDoc.getFooter().showPageNumber(false)' ")
                    .Create());

            myDoc.AddEle(BreakLine.SetTimes(2).Create());

            myDoc.Save(@"d:\outtest.doc");
        }
    }
}
