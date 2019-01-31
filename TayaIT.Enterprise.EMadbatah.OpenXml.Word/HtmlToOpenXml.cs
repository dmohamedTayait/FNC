using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
//using NotesFor.HtmlToOpenXml;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class HtmlToOpenXml
    {
        //public static void SaveHtmlToWord(string html, string filename, string resfolderpath, out int numPages)
        //{

        //    if (File.Exists(filename)) File.Delete(filename);

        //    using (MemoryStream generatedDocument = new MemoryStream())
        //    {
        //        using (WordprocessingDocument package = WordprocessingDocument.Create(generatedDocument, WordprocessingDocumentType.Document))
        //        //using (WordprocessingWorker doc = new WordprocessingWorker(filename, WordprocessingWorker.GetDocParts(resfolderpath), DocFileOperation.CreateNew))

        //        {
        //            MainDocumentPart mainPart = package.MainDocumentPart;//doc.DocMainPart;
        //            if (mainPart == null)
        //            {
        //                mainPart = package.AddMainDocumentPart();
        //                new Document(new Body()).Save(mainPart);
        //            }

        //            HtmlConverter converter = new HtmlConverter(mainPart);
        //            //converter.HtmlStyles.DefaultStyle = converter.HtmlStyles.GetStyle("Normal");
        //            Body body = mainPart.Document.Body;

        //            var paragraphs = converter.Parse(html);
                   
        //            for (int i = 0; i < paragraphs.Count; i++)
        //            {
        //                //for making sure it is right to left
        //                /*ParagraphProperties paragraphProp = new ParagraphProperties();
        //                Justification justification = new Justification();
        //                justification.Val = JustificationValues.Right;
        //                paragraphProp.Append(justification);
        //                paragraphProp.ParagraphStyleId = new ParagraphStyleId() { Val = ParagraphStyle.NormalArabic.ToString() }; // we set the style
        //                paragraphs[i].Append(paragraphProp);
        //                */
        //                body.Append(paragraphs[i]);
        //            }
                    
        //            mainPart.Document.Save();
        //            //doc.Save();
        //        }
        //        numPages = 0;
        //        File.WriteAllBytes(filename, generatedDocument.ToArray());
        //    }

        //}
    }
}
