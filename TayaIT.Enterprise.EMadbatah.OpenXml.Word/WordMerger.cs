using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenXmlPowerTools;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class WordMerger
    {
        private string _path1, _path2, _path3, _ouputDoc;

        public WordMerger(string path1, string path2, string path3, string outputDoc)
        {
            _path1 = path1;
            _path2 = path2;
            _path3 = path3;
            _ouputDoc = outputDoc;
        }

        public void MergeDocs()
        {

            List<Source> sources = null;


            //usama
            sources = new List<Source>()
            {
                 new Source(new WmlDocument(_path3), 4, false),
                new Source(new WmlDocument(_path1),true),
            };
            //DocumentBuilder.BuildDocument(sources, "Out1.docx");
            WmlDocument outdoc = DocumentBuilder.BuildDocument(sources);
            outdoc.SaveAs(_ouputDoc);  
            



            // Create new document from 10 paragraphs starting at paragraph 5 of Source1.docx
            sources = new List<Source>()
            {
                new Source(new WmlDocument(_path1), 5, 10, true),
            };
            DocumentBuilder.BuildDocument(sources, "Out1.docx");

            // Create new document from paragraph 1, and paragraphs 5 through end of Source3.docx.
            // This effectively 'deletes' paragraphs 2-4
            sources = new List<Source>()
            {
                new Source(new WmlDocument(_path3), 0, 1, false),
                new Source(new WmlDocument(_path3), 4, false),
            };
            DocumentBuilder.BuildDocument(sources, "Out2.docx");

            // Create a new document that consists of the entirety of Source1.docx and Source2.docx.  Use
            // the section information (headings and footers) from source1.
            sources = new List<Source>()
            {
                new Source(new WmlDocument(_path1), true),
                new Source(new WmlDocument(_path2), false),
            };
            DocumentBuilder.BuildDocument(sources, "Out3.docx");

            // Create a new document that consists of the entirety of Source1.docx and Source2.docx.  Use
            // the section information (headings and footers) from source2.
            sources = new List<Source>()
            {
                new Source(new WmlDocument(_path1), false),
                new Source(new WmlDocument(_path2), true),
            };
            DocumentBuilder.BuildDocument(sources, "Out4.docx");
            WmlDocument out4 = DocumentBuilder.BuildDocument(sources);
            out4.SaveAs(_ouputDoc);

            // Create a new document that consists of the first 5 paragraphs of Source1.docx and the first
            // five paragraphs of Source2.docx.  This example returns a new WmlDocument, when you then can
            // serialize to a SharePoint document library, or use in some other interesting scenario.
            sources = new List<Source>()
            {
                new Source(new WmlDocument(_path1), 0, 5, false),
                new Source(new WmlDocument(_path2), 0, 5, true),
            };
            WmlDocument out5 = DocumentBuilder.BuildDocument(sources);
            out5.SaveAs(_ouputDoc);  // save it to the file system, but we could just as easily done something
            // else with it.
        }


        public static void MergeDocs(string[] files, string outFilePath, bool keepSections)
        {
            List<Source> sources = null;
            //usama
           /* sources = new List<Source>()
            {
                 new Source(new WmlDocument(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\docs\Source1.docx"), 4, false),
                new Source(new WmlDocument(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\docs\Source2.docx"),true),
            };
            //DocumentBuilder.BuildDocument(sources, "Out1.docx");
            WmlDocument outdoc = DocumentBuilder.BuildDocument(sources);
            outdoc.SaveAs(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\docs\merged.txt");

            return;*/

            //usama
            sources = new List<Source>(){};
            foreach(string file in files)
               sources.Add(new Source(new WmlDocument(file), keepSections));
            
            //DocumentBuilder.BuildDocument(sources, "Out1.docx");
            //WmlDocument outdoc = DocumentBuilder.BuildDocument(sources);

            /*List<Source> sources2 = new List<Source>() 
                { 
                    new Source(new WmlDocument(files[0]), keepSections),
                    new Source(new WmlDocument(files[1]), keepSections)
                };
            */
            

            DocumentBuilder.BuildDocument(sources, outFilePath);
            //outdoc.SaveAs(outFilePath);  
            
        }
    }
}
