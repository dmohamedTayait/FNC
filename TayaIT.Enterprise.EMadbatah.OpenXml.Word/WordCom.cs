using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Word;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class WordCom
    {
        public static int GetDocumentNumPages(string docPath)
        {
            WordDocument doc = new WordDocument();
            doc.Open(docPath, false);
            int numPages = doc.GetNumPages();
            doc.Quit();
            
            return numPages;
        }
        public static bool ConvertDocument(string sourceDocPath, string targetFilePath, EMadbatah.Model.TargetFormat format)
        {
            return WordConverter.ConvertDocument(sourceDocPath, targetFilePath, format);
        }
    }
}
