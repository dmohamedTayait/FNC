using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Word = Microsoft.Office.Interop.Word;
namespace TayaIT.Enterprise.EMadbatah.Word.OpenXML
{
    
    public class WordMerger
    {
        /// <summary>
        /// This is the default Word Document Template file
        /// </summary>
        private const string defaultWordDocumentTemplate = @"Normal.dot";

        /// <summary>
        /// A function that merges Microsoft Word Documents that uses the default template
        /// </summary>
        /// <param name="filesToMerge">An array of files that we want to merge</param>
        /// <param name="outputFilename">The filename of the merged document</param>
        /// <param name="insertPageBreaks">Set to true if you want to have page breaks inserted after each document</param>
        public static void Merge(string[] filesToMerge, string outputFilename, bool insertPageBreaks)
        {
            Merge(filesToMerge, outputFilename, insertPageBreaks, defaultWordDocumentTemplate);
        }

        /// <summary>
        /// A function that merges Microsoft Word Documents that uses a template specified by the user
        /// </summary>
        /// <param name="filesToMerge">An array of files that we want to merge</param>
        /// <param name="outputFilename">The filename of the merged document</param>
        /// <param name="insertPageBreaks">Set to true if you want to have page breaks inserted after each document</param>
        /// <param name="documentTemplate">The word document you want to use to serve as the template</param>
        public static void Merge(string[] filesToMerge, string outputFilename, bool insertPageBreaks, string documentTemplate)
        {
            object defaultTemplate = documentTemplate;
            object missing = System.Type.Missing;
            object pageBreak = Microsoft.Office.Interop.Word.WdBreakType.wdPageBreak;
            object outputFile = outputFilename;
            object readOnly = true;
            // Create  a new Word application
            Microsoft.Office.Interop.Word._Application wordApplication = new Microsoft.Office.Interop.Word.Application();
            wordApplication.DisplayAlerts = Microsoft.Office.Interop.Word.WdAlertLevel.wdAlertsNone;
            try
            {
                // Create a new file based on our template
                Microsoft.Office.Interop.Word._Document wordDocument = wordApplication.Documents.Add(
                                              ref missing/*defaultTemplate*/
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                // Make a Word selection object.
                Microsoft.Office.Interop.Word.Selection selection = wordApplication.Selection;

                // Loop thru each of the Word documents
                foreach (string file in filesToMerge)
                {
                    // Insert the files to our template
                    selection.InsertFile(
                                                file
                                            , ref missing
                                            , ref missing
                                            , ref missing
                                            , ref missing);

                    //Do we want page breaks added after each documents?
                    if (insertPageBreaks)
                    {
                        selection.InsertBreak(ref pageBreak);
                    }
                }

                // Save the document to it's output file.
                wordDocument.SaveAs(
                                ref outputFile
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing
                            , ref missing);
                
                // Clean up!
                wordDocument = null;
            }
            catch (Exception ex)
            {
                //I didn't include a default error handler so i'm just throwing the error
                throw ex;
            }
            finally
            {
                // Finally, Close our Word application
                //wordApplication.
                Object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
                wordApplication.Quit(ref saveChanges, ref missing, ref missing);
                //Object saveChanges = Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
                //Object originalFormat = Type.Missing;
                //Object routeDocument = Type.Missing;
                //wordApplication.Quit(ref saveChanges,
                //    ref originalFormat, ref routeDocument);
            }
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.StartsWith("WINWORD"))
                {
                    thisproc.Kill();
                }
            }

        }
    }
}
