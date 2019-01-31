using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Office.Interop.Word;
using TayaIT.Enterprise.EMadbatah.Model;

namespace TayaIT.Enterprise.EMadbatah.Word.OpenXML
{
    public class WordConverter
    {
        //public static void ConvertDocument(string sourceDocPath, string targetFilePath, TargetFormat format)
        //{
        //    WdExportFormat targetFormat = WdExportFormat.wdExportFormatPDF;
        //    switch (format)
        //    {
        //        case TargetFormat.Pdf:
        //            targetFormat = WdExportFormat.wdExportFormatPDF;
        //            break;

        //        case TargetFormat.Xps:
        //            targetFormat = WdExportFormat.wdExportFormatXPS;
        //            break;
        //    }

        //    // Make sure the source document exists.
        //    if (!System.IO.File.Exists(sourceDocPath))
        //        throw new Exception("The specified source document does not exist.");

        //    // Create an instance of the Word ApplicationClass object.          
        //    //ApplicationClass wordApplication = new ApplicationClass();
        //    Application wordApplication = new Application();
        //    Document wordDocument = null;

        //    // Declare variables for the Documents.Open and ApplicationClass.Quit method parameters. 
        //    object paramSourceDocPath = sourceDocPath; //@"D:\Dev\Work\Akona\How To's\HowTos\Word\ConvertingDocToPDFXPS\test.docx"; // 
        //    object paramMissing = Type.Missing;

        //    // Declare variables for the Document.ExportAsFixedFormat method parameters.
        //    string paramExportFilePath = targetFilePath;// @"D:\Dev\Work\Akona\How To's\HowTos\Word\ConvertingDocToPDFXPS\test.xps";
        //    WdExportFormat paramExportFormat = targetFormat;// WdExportFormat.wdExportFormatXPS;
        //    bool paramOpenAfterExport = false;
        //    WdExportOptimizeFor paramExportOptimizeFor = WdExportOptimizeFor.wdExportOptimizeForOnScreen;
        //    WdExportRange paramExportRange = WdExportRange.wdExportAllDocument;
        //    int paramStartPage = 0;
        //    int paramEndPage = 0;
        //    WdExportItem paramExportItem = WdExportItem.wdExportDocumentContent;
        //    bool paramIncludeDocProps = true;
        //    bool paramKeepIRM = true;
        //    WdExportCreateBookmarks paramCreateBookmarks =
        //        WdExportCreateBookmarks.wdExportCreateWordBookmarks;
        //    bool paramDocStructureTags = true;
        //    bool paramBitmapMissingFonts = true;
        //    bool paramUseISO19005_1 = false;

        //    try
        //    {
        //        // Open the source document.
        //        wordDocument = wordApplication.Documents.Open(ref paramSourceDocPath, ref paramMissing, ref paramMissing, ref paramMissing,
        //            ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing,
        //            ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing, ref paramMissing);

        //        // Export it in the specified format.
        //        if (wordDocument != null)
        //            wordDocument.ExportAsFixedFormat(paramExportFilePath, paramExportFormat, paramOpenAfterExport, paramExportOptimizeFor,
        //                paramExportRange, paramStartPage, paramEndPage, paramExportItem, paramIncludeDocProps, paramKeepIRM, paramCreateBookmarks, paramDocStructureTags,
        //                paramBitmapMissingFonts, paramUseISO19005_1, ref paramMissing);
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //    finally
        //    {
        //        // Close and release the Document object.
        //        if (wordDocument != null)
        //        {
        //            wordDocument.Close(ref paramMissing, ref paramMissing, ref paramMissing);
        //            wordDocument = null;
        //        }

        //        // Quit Word and release the ApplicationClass object.
        //        if (wordApplication != null)
        //        {
        //            wordApplication.Quit(ref paramMissing, ref paramMissing, ref paramMissing);
        //            wordApplication = null;
        //        }

        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //    }
        //}
    }
}
