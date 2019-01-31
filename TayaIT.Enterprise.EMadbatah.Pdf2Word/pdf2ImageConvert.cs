using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
 
namespace TayaIT.Enterprise.EMadbatah.Pdf2Word
{
    public class pdf2ImageConvert
    {
        

        /////////////////////////////////
        ////////////////////////////////

        public static void convertPdfFile(String pdfFilePath)
        {
            //String pdfFilePath = "Madbtah.pdf";
            int pagenos = getNumberOfPdfPages(pdfFilePath);
            convert(pdfFilePath, pagenos);
        }

        /////////////////////////////////////
        ///////////////////////////////////

        private static void convert(String pdfFilePath, int pageNo)
        {

            System.IO.FileInfo input = new FileInfo(pdfFilePath);

            string directoryString = input.Directory + "\\"+input.Name.Substring(0, input.Name.Length - 4);
            
            if (Directory.Exists(directoryString))
                ;
            else
                Directory.CreateDirectory(directoryString);

            for(int i=0;i<pageNo;i++)
            {
                convertPage(input, directoryString, i);
            }

        }

        /////////////////////////////////////
        /////////////////////////////////////

        private static void convertPage(FileInfo input, string outputDirectory,int pageno)
        {
            PDFConvert converter = new PDFConvert();
            converter.FirstPageToConvert = pageno;
            converter.LastPageToConvert = pageno;
            converter.FitPage = true;
            converter.JPEGQuality = 10;
            converter.OutputFormat = "pngalpha";


            string output = outputDirectory + "\\" + input.Name.Substring(0, input.Name.Length - 4) + "_" + pageno + ".png";
            
            converter.Convert(input.FullName, output);
            

        }


        ///////////////////////////////////
        //////////////////////////////////

 
public static int GetPDFPageCount(string path)
{                 
      // open the file
      PdfReader pdf_file = new PdfReader(path);
 
      // read it's page count
      int page_count = pdf_file.NumberOfPages;
 
      // close the file.
      pdf_file.Close();
 
      // return the value
      return page_count;
}


        public static int getNumberOfPdfPages(string fileName)
        {
            using (StreamReader sr = new StreamReader(File.OpenRead(fileName)))
            {
                Regex regex = new Regex(@"/Type\s*/Page[^s]");
                MatchCollection matches = regex.Matches(sr.ReadToEnd());

                return matches.Count;
            }
        }


    }
}
