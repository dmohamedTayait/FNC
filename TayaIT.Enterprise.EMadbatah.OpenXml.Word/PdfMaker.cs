using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.ComponentModel;
using System.Configuration;
using Bullzip.PdfWriter;
using System.IO;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Drawing.Printing;
using System.Drawing;

namespace TayaIT.Enterprise.EMadbatah.OpenXml.Word
{
    public class PdfMaker
    {
        //https://sites.google.com/site/bullzip/pdf-printer/examples/printing-microsoft-word-documents-to-pdf-using-c
        //http://www.codeproject.com/Tips/145780/A-SOA-approach-to-dynamic-DOCX-PDF-report-generati

        //http://notesforhtml2openxml.codeplex.com/

        public static bool ConvertDocxToPdf(string tempFolder, string serverMapPath, string docxFileName, string pdfFileName)
        {
            try
            {
                PrintToPdf(tempFolder, serverMapPath, docxFileName, pdfFileName);
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.OpenXml.Word.PdfMaker.ConvertDocxToPdf(" + docxFileName + "," + tempFolder + ")");
                return false;
            }
        }

        internal static void PrintBatchPdf(string appFolder)
        {
            try
            {
                DirectoryInfo dInfo = new DirectoryInfo(@"C:\Progetti\DocxReportGenerator");
                FileInfo[] docxFiles = dInfo.GetFiles("*.docx");
                IEnumerable<string> fileNames =
                    from docxFile in docxFiles
                    orderby docxFile.FullName
                    select docxFile.FullName;

                FileList files = new FileList();
                files.AddRange(fileNames);

                PdfSettings pdfSettings = new PdfSettings();
                pdfSettings.PrinterName = ConfigurationManager.AppSettings["PdfPrinter"];

                string settingsFile = pdfSettings.GetSettingsFilePath(PdfSettingsFileType.Settings);
                pdfSettings.LoadSettings(appFolder + @"\App_Data\printerSettings.ini");

                foreach (string fileToPrint in files)
                {
                    PdfUtil.PrintFile(fileToPrint, pdfSettings.PrinterName);
                }
            }
            catch (Exception ex)
            {
                //throw new FaultException("WCF error!\r\n" + ex.Message);
            }
        }
        protected void PrintPage(object sender, PrintPageEventArgs ev)
        {
            //int yPos = 100;
            //int leftMargin = ev.MarginBounds.Left;
            //int topMargin = ev.MarginBounds.Top;
            //Font printFont = new Font("Arial", 14);

            //// Insert text
            //ev.Graphics.DrawString(TextBox1.Text, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());

            //// Insert graphics
            //Bitmap bmp = new Bitmap(Server.MapPath("~/logo.png"));
            //Point p = new Point(100, 200);
            //ev.Graphics.DrawImage(bmp, p);
        }
        internal static byte[] PrintToPdf(string tempFolder, string serverMapPath, string docxFileName, string pdfFileName)
        {
            try
            {
                //string tempFolder = appFolder + @"\temp";
                //string tempDocxFilePath = tempFolder + @"\" + tempDocxFileName;

                //PdfSettings pdfSettings = new PdfSettings();
                //pdfSettings.PrinterName = ConfigurationManager.AppSettings["PdfPrinter"];

                //string settingsFile = pdfSettings.GetSettingsFilePath(PdfSettingsFileType.Settings);
                //pdfSettings.LoadSettings(serverMapPath + @"\App_Data\printerSettings.ini");
                //pdfSettings.SetValue("Output", pdfFileName);
                //pdfSettings.WriteSettings(settingsFile);

                const string printerName = @"Bullzip PDF Printer";
                //const string tempFolder = "~/App_Data/Temp/";
                //string sessionGuid = System.Guid.NewGuid().ToString();

                //
                // Print to file
                //

                // Define printer postscript output file
                //string postscriptFileName = tempFolder + System.Guid.NewGuid().ToString() + ".ps";

                //// Set up more file names
                ////string pdfFileName = tempFolder + sessionGuid + ".pdf";
                //string statusFileName = tempFolder + sessionGuid + "_status.ini";
                //string settingsFileName = tempFolder + sessionGuid + "_settings.ini";

                /*
                PdfSettings settings = new PdfSettings();
                settings.PrinterName = printerName;
                settings.SetValue("Output", pdfFileName);
                settings.SetValue("ShowSettings", "never");
                settings.SetValue("ShowSaveAS", "never");
                settings.SetValue("ShowProgress", "no");
                settings.SetValue("ShowProgressFinished", "no");
                settings.SetValue("ShowPDF", "no");
                settings.SetValue("ConfirmOverwrite", "no");
                settings.SetValue("StatusFile", statusFileName);
                settings.SetValue("StatusFileEncoding", "Unicode");
                settings.SetValue("RememberLastFileName", "no");
                settings.SetValue("RememberLastFolderName", "no");
                settings.SetValue("SuppressErrors", "yes");
                settings.SetValue("Linearize", "yes");
                settings.SetValue("OwnerPassword","erion");
                settings.SetValue("Permissions","61632");
                settings.WriteSettings(settingsFileName);

                settings.LoadSettings(settingsFileName);
                */
                Bullzip.PdfWriter.FileList fl = new FileList();
                fl.Add(docxFileName);
                PdfUtil.PrintFile(docxFileName, printerName);//pdfSettings.PrinterName);
                string outFile = @"C:\PDF Writer\Output\Microsoft Word - " + new FileInfo(pdfFileName).Name;//.Replace(".pdf", ".docx.pdf");

                //PrintDocument pd = new PrintDocument();
                //pd.PrinterSettings.PrinterName = printerName;
                //pd.PrinterSettings.PrintFileName = postscriptFileName;
                //pd.PrinterSettings.PrintToFile = true;
                //pd.Print();


                // Get the location of the gui.exe so that we can launch it
                //string gui = PdfUtil.GetPrinterAppFolder(printerName) + @"\gui.exe";
                // Launch gui.exe
                //ProcessStartInfo processStartInfo = new ProcessStartInfo();
                //processStartInfo.FileName = gui;
                //processStartInfo.Arguments = string.Format("printer=\"{0}\" ps=\"{1}\"",
                //    printerName, @"C:\PDF Writer\Temp\BullZip\PDF Printer\dd23e4fd-4121-4855-9df1-b430170885e6_printer.ps");
                //Process p = Process.Start(processStartInfo);

                //// Wait for the gui.exe process to exit
                //DateTime starttime = DateTime.Now;
                //while (!p.HasExited && DateTime.Compare(DateTime.Now, starttime.AddSeconds(30)) < 0)
                //{
                //    System.Threading.Thread.Sleep(1000);//200
                //}

                //// Kill the process if it is still running
                //if (!p.HasExited)
                //{
                //    p.Kill();
                //    throw new Exception("Error creatin PDF file. The gui.exe process did not finish within the time limit.");
                //}

                //string tempPdfFilePath =


                bool fileCreated = false;
                while (!fileCreated)
                {
                    fileCreated = PdfUtil.WaitForFile(outFile, 200);
                }
                File.Copy(outFile, pdfFileName, true);
                byte[] pdfBytes = File.ReadAllBytes(pdfFileName);

                //File.Delete(tempDocxFilePath);
                //File.Delete(tempPdfFilePath);

                return pdfBytes;
            }
            catch (Exception ex)
            {
                //throw new FaultException("WCF error!\r\n" + ex.Message);
                return null;
            }
        }
    }
}
