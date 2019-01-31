using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Vecsys;
using TayaIT.Enterprise.EMadbatah.Model;
using System.IO;
using TayaIT.Enterprise.EMadbatah.BLL;
using System.Globalization;
using System.Text.RegularExpressions;
using TayaIT.Enterprise.EMadbatah.Localization;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Web;
using System.Xml;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.Pdf2Word;
//using TagLib.Riff;
using System.Web.Security;
using TayaIT.Enterprise.EMadbatah.OpenXml.Word;
public partial class Tester : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        /*
        List<SpeakersIndexItem> list=new List<SpeakersIndexItem>();
        list.Add(new SpeakersIndexItem("محمد أحمد محمد المر","6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, 52, 53",null));
        list.Add(new SpeakersIndexItem("محمد سالم عبيد المزروعي","6, 7, 204, 205, 206",null));
        list.Add(new SpeakersIndexItem("محمد سالم عبيد المزروعي","6, 7, 204, 205, 206",null));
        list.Add(new SpeakersIndexItem("محمد سالم عبيد المزروعي","6, 7, 204, 205, 206",null));
        list.Add(new SpeakersIndexItem("محمد سالم عبيد المزروعي","6, 7, 204, 205, 206",null));
         

        TayaIT.Enterprise.EMadbatah.BLL.MabatahCreatorFacade.CreateSpeakersIndex(list, 15, @"E:\", @"E:\temp.docx");
        */
        //HTMLtoDOCX hd = new HTMLtoDOCX();
        //hd.CreateFileFromHTML(System.IO.File.ReadAllText(@"E:\indexSpeakers.docx"), @"E:\" + "tmp_indexSpeakers.docx");
        //hd.CreateFileFromHTML(SessionStartFacade.GetAutomaticSessionStartText(15), @"E:\" + "tmp_sessionStart.docx");

        //string name = EMadbatahFacade.GetSessionName(14, 5, 466);

        //Console.WriteLine(name);

        //TayaIT.Enterprise.EMadbatah.Model.SessionDetails details = SessionStartFacade.GetSessionDetails(98);
        //MabatahCreatorFacade.CreateMadbatahCover(details, @"\\192.168.0.90\c$\Websites\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\cover.docx", Server.MapPath("~"));

        //using (WordprocessingWorker doc = new WordprocessingWorker(@"D:\work\projects\Enterprise\eMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\body.docx",
        //    DocFileOperation.Open))
        //{
        //    doc.AddParagraph("( Hello )", ParagraphStyle.NormalArabic, ParagrapJustification.Center, false, "");
        //    doc.SaveAs(@"D:\work\projects\Enterprise\eMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\\body-mod.docx");
        //}

        //koko is koko
        /*using (WordprocessingWorker doc = new WordprocessingWorker(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\temp.docx",
            WordprocessingWorker.GetDocParts(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\resources\"),
            DocFileOperation.CreateNew))
        {
            doc.AddParagraph("( Hello )", ParagraphStyle.NormalArabic, ParagrapJustification.Center, false, "");
            doc.AddParagraph("اا يا اي اينشنسنشتشنش نشن شن", ParagraphStyle.NormalArabic, ParagrapJustification.Center, false, "");
            doc.AddTextToWordDocument(" اضافه سميكههههههههه",ParagraphStyle.ParagraphTitle, ParagrapJustification.Both);
            doc.Save();
            //doc.SaveAs("E:\\FNC\\EMadbatah\\TayaIT.Enterprise.EMadbatah.Web\\Files\\98\\sessionStartDoc-mod.docx");
        }*/
       
        
        

        /*PdfMaker.ConvertDocxToPdf(@"C:\Websites\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98-2", Server.MapPath("~"),
            @"C:\Websites\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98-2\98.docx",
            @"C:\Websites\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98-2\98.pdf");
        */

        //System.Diagnostics.Process myproc = new System.Diagnostics.Process();
        //foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
        //{
        //    if (thisproc.ProcessName.StartsWith("WINWORD"))
        //    {
        //        thisproc.Kill();
        //    }
        //}

        

        //MabatahCreatorFacade.CreateMadbatahStart(details, Server.MapPath("~") + "\\Files\\98\\" + "sessionStartDoc.docx", Server.MapPath("~"));
        
        //ImageWriter.CreateImageDocument(Server.MapPath("~") + "\\Files\\98\\images.docx",Server.MapPath("~") + "\\resources\\",
        //    Directory.GetFiles(@"E:\FNC\AddingImagesToDocumentsInWord2007ByUsingTheOpenXMLSDK2.0\AddingImagesToDocumentsInWord2007ByUsingTheOpenXMLSDK2.0_ART","*.bmp"));

       //MabatahCreatorFacade.CreateMadbatah(98, Server.MapPath("~") + "\\Files\\98\\", Server.MapPath("~"));
        //WordCom.ConvertDocument(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\98.docx",
        //    @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\98.pdf",TargetFormat.Pdf);
        //string html = SessionStartFacade.GetAutomaticSessionStartText(98);
        //HTMLtoDOCX hd = new HTMLtoDOCX();
        //hd.CreateFileFromHTML(html, Server.MapPath("~") + "\\Files\\98\\" + "sessionStartDoc.docx");
        //int tt = 0;
        //HtmlToOpenXml.SaveHtmlToWord(html, Server.MapPath("~") + "\\Files\\98\\" + "sessionStartDoc.docx", Server.MapPath("~") + "\\resources\\", out tt);
       /*
        string[] files = new string[] { 
            //*@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\coverDoc.docx",
           @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\sessionStartDoc.docx",
            @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\indexDoc.docx",
            @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\body.docx",
            @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\indexSpeakers.docx"
        };

        List<byte[]> docs = new List<byte[]>();
        foreach (string file in files)
            docs.Add(System.IO.File.ReadAllBytes(file));
*/
        //System.IO.File.WriteAllBytes(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\merged.docx", WordprocessingWorker.OpenAndCombine(docs));
        //WordprocessingWorker.MergeWithAltChunk(@"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\coverDoc.docx",
        //   files);
       
        //WordMerger.MergeDocs(files, @"E:\FNC\EMadbatah\TayaIT.Enterprise.EMadbatah.Web\Files\98\merged.docx", false);


        //WinActiveDirectory ad = new WinActiveDirectory(ConfigManager.GetConnectionString("ADConnectionString"), CurrentUser.DomainUserName.Split('\\')[1].ToLower());
        //List<WinActiveDirectory.LDAPUser> users = ad.GetAllUsers();
        //Response.Write(users.Count);

        //WordDocument doc = new WordDocument();
        //WordDocument docStart = new WordDocument();
        //try
        //{
        

        //    doc.CreateNew();
        //    doc.initializeStyle();
        //    BasicFormat format = new BasicFormat();
        //    format.align = Alignment.Right;
        //    format.textStyle = TextStyle.Bold;

        //    format.align = Alignment.Center;


        //    doc.insertBreakLine(1);
        //    doc.insertText("دولة الإمارات العربية المتحدة"
        //        , 14, format, FontColor.Black, TextFont.Arial);
        //    doc.insertBreakLine(1);
        //    doc.insertText("المجلس الوطني الاتحادي"
        //        , 14, format, FontColor.Black, TextFont.Arial);
        //    doc.insertBreakLine(4);

        //    doc.insertText("رقم الجلسة مسلسلا"
        //        , 14, format, FontColor.Black, TextFont.Arial);
        //    doc.insertBreakLine(1);
        //    doc.insertText("منذ بـدء الحيـاة النيابيـة"
        //        , 14, format, FontColor.Black, TextFont.Arial);
        //    doc.insertBreakLine(1);


        //    doc.SaveDocument("G:\\coverTest.doc");
        //    doc.Quit();
        //}
        //catch (Exception ex)
        //{

        //    if (!doc.IsClosed)
        //        doc.Quit();
        //    foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
        //    {
        //        if (thisproc.ProcessName.StartsWith("WINWORD"))
        //        {
        //            thisproc.Kill();
        //        }
        //    }

        //    LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.BLL.MabatahCreatorFacade.CreateMadbatahIndexWithAttachment");

            
        //}

        /*string inputText = "<doc><span class=\"segment\" stime=\"157.2\">ككل</span><span class=\"segment\" stime=\"157.66\">وليس</span><span class=\"segment\" stime=\"158.9\">بسياسة</span></doc>";

        XmlDocument doc = new XmlDocument();
        doc.Load(@"\\192.168.0.91\Share\emadbatah\SimulateVecsysServerFolder\session_11_01_2011_10.trans.xml");

        StringBuilder sb = new StringBuilder();
        sb.Append("<SpeechSegment ch=\"1\" sconf=\"1.00\" stime=\"4.89\" etime=\"13.61\" spkid=\"TTTT\" lang=\"ara-fnc\" lconf=\"1.00\" trs=\"1\">");
        */
        //<span stime="170.17" class="segment">وهذا</span>
        /*MatchCollection subCollections = Regex.Matches(inputText, "<span.*?</span>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        foreach (Match submatch in subCollections)
            sb.Append("<Word stime=" + submatch.Value + " dur=\"0.25\" conf=\"0.886\">" + "" + "</Word>");
        */
        
        
        //TayaIT.Enterprise.EMadbatah.DAL.Attachement att  =  TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.GetAttachementByID(1);
        //System.IO.File.WriteAllBytes("E:\\myfile.pdf", att.FileContent);

       //VecSysSessionFile sessionFile =
       //    Parser.ParseTransXml(@"f:\session_22-06-2010_2.trans.xml");

       //List<VecSysParagraph> paragraphs = Parser.combineSegments(sessionFile.SpeechSegmentList);
        //DocumentSharp2Word doc = new DocumentSharp2Word();
        //DateTime curTime = DateTime.Now;
        //TayaIT.Enterprise.EMadbatah.Util.Dates dates = new TayaIT.Enterprise.EMadbatah.Util.Dates();
        //string date = dates.FormatGreg(curTime.ToString("yyyy/MM/dd"), "yyyy/MM/dd");

        //string res = setFormatAtrrInSpan(File.ReadAllText(@"E:\FNC\sessionContentItem_sample.txt"));
        
        int sessionID = 116;
        string folderPath = Server.MapPath("~") + @"\Files\" + sessionID + @"\";
        Exception ex = new Exception();
        //string folderPath = "C:\\TayaIT\\Websites\\EMadbatahWeb\\Files\\30\\";
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);
        LogHelper.LogException( ex,"Tester: Folder Created");
        TayaIT.Enterprise.EMadbatah.Model.SessionDetails details = SessionStartFacade.GetSessionDetails(sessionID);
        if (details.Status == TayaIT.Enterprise.EMadbatah.Model.SessionStatus.Approved)
        {
            LogHelper.LogException(ex, "Tester: Status approved");
            TayaIT.Enterprise.EMadbatah.DAL.SessionFile start = SessionStartFacade.GetSessionStartBySessionID(sessionID);
            if (start != null)
            {
                LogHelper.LogException(ex, "Tester: Start not equal null");
               TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)TayaIT.Enterprise.EMadbatah.Model.MadbatahFilesStatus.InProgress);
               LogHelper.LogException(ex, "Tester: Madbata Creating");
                if (MabatahCreatorFacade.CreateMadbatah(sessionID, folderPath,Server.MapPath("~")))
                {
                    LogHelper.LogException(ex, "Tester: Madbata Created");
                    lblText.Text = " Madbatah Created";
                    string wordFilePath = folderPath + sessionID + ".docx";
                    byte[] wordDoc = File.ReadAllBytes(wordFilePath);
                    //WordConverter.ConvertDocument(wordFilePath, wordFilePath.Replace(".doc", ".pdf"), TargetFormat.Pdf);
                    bool pdfSuccess = WordCom.ConvertDocument(wordFilePath, wordFilePath.Replace(".docx", ".pdf"), TargetFormat.Pdf);
                    byte[] pdfDoc = File.ReadAllBytes(wordFilePath.Replace(".docx", ".pdf"));
                    Hashtable emailData = new Hashtable();
                    if (SessionHelper.UpdateSessionWordAndPdfFiles(sessionID, wordDoc, pdfDoc) > 0)
                    {

                        string sessionName = EMadbatahFacade.GetSessionName(details.Season, details.Stage, details.Serial);

                        emailData.Add("<%SessionName%>", sessionName);
                        emailData.Add("<%SessionDate%>", details.Date.ToShortDateString());
                        emailData.Add("<%RevName%>", CurrentUser.Name);
                        MailManager.SendMail(new Email(new Emailreceptionist(CurrentUser.Email, CurrentUser.Name)), SystemMailType.MadbatahFileCreated, emailData);
                    }
                    else
                    {
                        // show error happend while adding madbatah word and pdf to database
                    }
                }
                else
                {
                    //show error something went wrong when creating the Madbatah Word/pdf
                }
            }
            else
            {
                //show warning session start not completed yet
            }
        }
        else
        {
            //show warninig session start not approved yet
        }
        
       
        /*
        try{
        int sessionStartSize = 0;
        long sessionID = 116;
        String ServerMapPath = Server.MapPath("~");
        String SessionWorkingDir = Server.MapPath("~") + @"\Files\" + sessionID + @"\";
                SessionFile start = SessionStartFacade.GetSessionStartBySessionID(sessionID);
                //HtmlToOpenXml.SaveHtmlToWord(start.SessionStartText, SessionWorkingDir + "sessionStartDoc.docx", ServerMapPath+ "\\resources\\", out sessionStartSize);
                HTMLtoDOCX hd = new HTMLtoDOCX();
                hd.CreateFileFromHTML(start.SessionStartText, SessionWorkingDir + "sessionStartDoc.docx");


                using (WordprocessingWorker doc = new WordprocessingWorker(SessionWorkingDir + "sessionStartDoc.docx", WordprocessingWorker.GetDocParts(ServerMapPath+"\\resources\\"), DocFileOperation.Open))
                {
                    WordprocessingWorker doctmp = doc;
                    sessionStartSize = doc.CountPagesUsingOpenXML(doc, SessionWorkingDir + "sessionStartDoc.docx", WordprocessingWorker.GetDocParts(ServerMapPath + "\\resources\\"), ServerMapPath, out doctmp);//CountPagesUsingOpenXML(DocumentType.DOCX, folderPath + att.Name);
                    doctmp.Dispose();
                }


                int coverSize = 1;
                List<MadbatahIndexItem> index = new List<MadbatahIndexItem>();
                List<SpeakersIndexItem> speakersIndex = new List<SpeakersIndexItem>();
                TayaIT.Enterprise.EMadbatah.Model.SessionDetails details = SessionStartFacade.GetSessionDetails(sessionID);
                int bodySize = MabatahCreatorFacade.CreateMadbatahBody(sessionID, SessionWorkingDir + "body.docx", ServerMapPath , details, out index, out speakersIndex);
                //, out sessionName, out sessionNameForFooter);
                if (bodySize == -1)
                    throw new Exception("Madbatah Body Creation Failed."); 

                //create madbatahCover
                MabatahCreatorFacade.CreateMadbatahCover(details, SessionWorkingDir + "coverDoc.docx",ServerMapPath);
                
                
                //attachment Cover
                
                //AttachCover.CreateNew();
                //AttachCover.initializeStyle();
                //format = new BasicFormat();
                //format.textStyle = TextStyle.Bold;
                //format.align = Alignment.Center;

                //AttachCover.insertBreakLine(8);
                //AttachCover.insertText("الملاحـق", 72, format, FontColor.Black, TextFont.Arial);
                //AttachCover.insertBreakLine(2);
                //AttachCover.ReplaceWithPageBreak("mcenoneditable");
                //AttachCover.SaveDocument(workingDir + "attachCover.doc");
                //AttachCover.Quit();

                List<string> mergeList = new List<string>();
                //mergeList.Add(SessionWorkingDir + "coverDoc.docx");//done
                mergeList.Add(SessionWorkingDir + "indexDoc.docx");
                mergeList.Add(SessionWorkingDir + "sessionStartDoc.docx");//done
                mergeList.Add(SessionWorkingDir + "body.docx");
                mergeList.Add(ServerMapPath + "\\docs\\AttachmentsCover.docx");//ready


                
                

                ////attachments
                List<Attachement> attachments = AttachmentHelper.GetSessionAttachments(sessionID);
                foreach (Attachement att in attachments)
                {
                    System.IO.File.WriteAllBytes(SessionWorkingDir + att.Name, att.FileContent);
                    FileInfo fInfo = new FileInfo(SessionWorkingDir + att.Name);

                    if (!fInfo.Extension.ToLower().Equals(".pdf"))
                        mergeList.Add(SessionWorkingDir + att.Name);
                    else
                    {
                        String pdfFilePath = SessionWorkingDir + att.Name;
                        pdf2ImageConvert.convertPdfFile(pdfFilePath);
                        string wordAttFilePath = SessionWorkingDir + att.Name.ToLower().Replace(".pdf", ".pdf.docx");
                        string[] files = Directory.GetFiles(SessionWorkingDir + fInfo.Name.Replace(fInfo.Extension, ""));
                        //WordDocument pdfAttDoc = new WordDocument();
                        //pdfAttDoc.CreateNew();
                        
                       // WordprocessingWorker.InsertAPicture(wordAttFilePath, files);

                        ImageWriter.CreateImageDocument(wordAttFilePath, ServerMapPath + "\\resources\\", files);

                        
                        //foreach (string file in files)
                        //    pdfAttDoc.InsertImage(file);
                        ////int pagenos =  getNumberOfPdfPages(pdfFilePath);
                        ////convert(pdfFilePath, pagenos);

                        //pdfAttDoc.SaveDocument(SessionWorkingDir + att.Name.ToLower().Replace(".pdf", ".pdf.doc"));
                        //pdfAttDoc.Quit();
                        mergeList.Add(SessionWorkingDir + att.Name.ToLower().Replace(".pdf", ".pdf.docx"));
                        att.Name = att.Name.ToLower().Replace(".pdf", ".pdf.docx");
                        //use waleed function
                    }
                }


                //int indexSize = MabatahCreatorFacade.CreateMadbatahIndex(index, folderPath + "coverDoc1.doc", folderPath + "indexDoc1.doc", details);//, sessionName );
                int allAttachSizes = 0;
                int indexSize = 0;
                indexSize = MabatahCreatorFacade.CreateMadbatahIndexWithAttachment(
                        index, attachments, SessionWorkingDir, indexSize, sessionStartSize, bodySize,
                        SessionWorkingDir + "indexDoc.docx", ServerMapPath,
                        details, false, out allAttachSizes);//, sessionName );

                if (indexSize == -1)
                    throw new Exception("Index with Attachment Creation Failed.");
                for (int i = 0; i < index.Count; i++)
                {
                    string[] parts = index[i].PageNum.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                    string newPageNum = "";
                    List<int> pages = new List<int>();
                    foreach (string part in parts)
                    {
                        int p = int.Parse(part) + coverSize + sessionStartSize + indexSize - 1;//1 cover
                        if (pages.IndexOf(p) == -1)
                        {
                            pages.Add(p);
                            newPageNum += p + ",";
                        }
                    }
                    newPageNum = newPageNum.Remove(newPageNum.Length - 1);
                    index[i].PageNum = newPageNum;
                    //index[i].PageNum += coverSize + sessionStartSize + indexSize - 1;//1 cover
                }
                int attachCover = 1;

                int speakerSize = MabatahCreatorFacade.CreateSpeakersIndex(speakersIndex,
                    coverSize + sessionStartSize+indexSize - 1, //2 for att cover + speakers cover
                    ServerMapPath, SessionWorkingDir + "indexSpeakers.docx");

                //mergeList.Add(SessionWorkingDir + "indexSpeakers.doc");
                if (speakerSize == -1)
                    throw new Exception("Speakers Index Creation Failed.");


                indexSize = MabatahCreatorFacade.CreateMadbatahIndexWithAttachment(
                    index, attachments, SessionWorkingDir,
                    indexSize, sessionStartSize, bodySize,
                    SessionWorkingDir + "indexDoc.docx", ServerMapPath,
                    details, true, out allAttachSizes);//, sessionName );
                if (indexSize == -1)
                    throw new Exception("Index with Attachment Creation Failed.");

                //mergeList.Add(SessionWorkingDir + "indexDoc3.doc");//done

                mergeList.Add(ServerMapPath + "\\docs\\SpeakersCover.docx");//ready
                mergeList.Add(SessionWorkingDir + "indexSpeakers.docx");//done

                //List<byte[]> docs = new List<byte[]>();
                //foreach (string file in mergeList)
                //    docs.Add(File.ReadAllBytes(file));

                //File.WriteAllBytes(SessionWorkingDir + "merged.docx", WordprocessingWorker.OpenAndCombine(docs));
                //WordMerger.MergeDocs(mergeList.ToArray(), SessionWorkingDir + sessionID + ".docx", true);
                File.Copy(SessionWorkingDir + "coverDoc.docx", SessionWorkingDir + sessionID + ".docx", true);
                WordprocessingWorker.MergeWithAltChunk(SessionWorkingDir + sessionID + ".docx",mergeList.ToArray());

                //PdfMaker.ConvertDocxToPdf(SessionWorkingDir, ServerMapPath, SessionWorkingDir + sessionID + ".docx", SessionWorkingDir + sessionID + ".pdf");
                //doc = new WordDocument();
                //doc.Open(SessionWorkingDir + sessionID + ".doc");

                //string sessionSerial = EMadbatahFacade.GetSessionNameForWord(13, details.Stage, details.Season);
                //doc.InsertPageNumInFooterWithSessionNum(sessionSerial);//"5/5/14");
                //Console.WriteLine(index.Count.ToString());
                //doc.SaveDocument();
                //doc.Quit();

                //return true;
            }
            catch (Exception ex)
            {
                //if (!docStart.IsClosed)
                //    docStart.Quit();
                //if (!AttachCover.IsClosed)
                //    AttachCover.Quit();

                //if (!doc.IsClosed)
                //    doc.Quit();

                //foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
                //{
                //    if (thisproc.ProcessName.StartsWith("WINWORD"))
                //    {
                //        thisproc.Kill();
                //    }
                //}

                //LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.BLL.MabatahCreatorFacade.CreateMadbatah(" + SessionID + "," + SessionWorkingDir + ")");
                //return false;
                
            }
               
        */
       

        /*
        BasicFormat format = new BasicFormat();
        WordDocument doc = new WordDocument();
        //doc.Open(@"E:\FNC\wordTOhtml.html");

        //doc.ReplaceWithPageBreak("mcenoneditable");
        //doc.InsertPageNumInFooterWithSessionNum("4/13/4");
        //doc.SaveDocument(@"E:\wordTOhtml.doc");
        //doc.Quit();
        
        doc.CreateNew();
        
        format.align = Alignment.Center;
        format.textStyle = TextStyle.Bold;
        doc.insertText("العنوان", 16,format,FontColor.Blue,TextFont.Arial );
        doc.insertBreakLine(1);

        doc.InsertHeading("عنوان رئيسي", HeadingLevel.Heading2);
        doc.insertBreakLine(1);


        format.align = Alignment.Right;
        format.textStyle = TextStyle.Italic;
        doc.insertText("كان القائد العسكري المنتصر المغوار خالد بن الوليد", 14, format, FontColor.Black, TextFont.Arial);
        doc.insertBreakLine(1);

        format.align = Alignment.Right;
        format.textStyle = TextStyle.Normal;
        doc.insertText("فرحيل مبارك لم يعن بالنسبة للجماهير الثائرة تحقيق مطلب الثورة الرئيسي وهو الشعب يريد اسقاط النظام", 14, format, FontColor.Green, TextFont.TimesNewRoman);
        doc.insertBreakLine(1);

        doc.insertHeader("تذييل للصفحه من فوق");
        //doc.inserFooter("تذييل للصفحه");


        //doc.insertPageNumInFooter();

        doc.InsertPageNumInFooterWithSessionNum("20/4/14");
        doc.AddFooteNote( "هامش جانبي");

        doc.AddBulletList(new List<string> { "عنصر 1", "عنصر 2", "عنصر 3" }, TextFont.Arial,  format);

        doc.InsertHyperLink("http://www.msn.com");
        doc.insertBreakLine(1);

        format.align = Alignment.Right;
        format.textStyle = TextStyle.Italic;
        doc.insertText("فرحيل مبارك لم يعن بالنسبةis it possible to convert this document to pdf using iTextSharp. الرئيسي وهو الشعب يريد اسقاط النظام", 18, format, FontColor.Green, TextFont.Arial);
        doc.insertBreakLine(1);

        doc.AddNumberList(new List<string> { "عنصر 1", "عنصر 2", "عنصر 3" }, TextFont.Arial, format);
        int num = doc.GetCurrentPageNumber();

        doc.insertBreakLine(10);

        num = doc.GetCurrentPageNumber();
        doc.SaveDocument(@"f:\out.doc");
        doc.Quit();
        */
        //WordConverter.ConvertDocument(@"f:\out.doc", @"f:\out.pdf", TargetFormat.Pdf);
        
        //WordMerger.Merge(new string[] { @"f:\TayaSearchSolution-Plan.doc", @"f:\out.doc" }, @"f:\merged.doc", true);
        
        ////doc.test();
        //doc.setDocumentProperties("e-madbatah", "test doc", "subject");
        //string text = "";
       
        /*
        
        format.align = Alignment.Center;
        format.textStyle = TextStyle.Italic;
        doc.insertHeading("Heading 1", HeadingLevel.Heading1, format);
        doc.insertBreakLine(1);
 
        format.align = Alignment.Left;
        format.textStyle = TextStyle.Italic;

        font = TextFont.TimesNewRoman;
        doc.insertParagraphPeice("paragraph 1 text", 20, format, System.Drawing.Color.Blue, font);
        doc.insertBreakLine(1);

        format.align = Alignment.Left;
        format.textStyle = TextStyle.Italic;
        font = TextFont.Arial;
        text = "The conference will be sponsored by the Institute for Systems and Technologies of Information, Control and Communication (INSTICC) and held in cooperation with the Portuguese Association for Artificial Intelligence (APPIA)";
        doc.insertParagraphPeice(text, 14, format, System.Drawing.Color.Black, font);
        doc.insertBreakLine(1);
        */
       // format.align = Alignment.Right;
       // format.textStyle = TextStyle.Normal;
       // font = TextFont.Arial;
       // //text = ReverseWords("كان القائد العسكري المنتصر المغوار خالد بن الوليد");
        
       // //text = File.ReadAllText(@"F:\SampleText.txt");
       // text = "{RTL}كان القائد العسكري المنتصر المغوار خالد بن الوليد";
       // doc.insertParagraphPeice(text, 14, format, System.Drawing.Color.Black, font);
       // doc.insertBreakLine(1);

       // /*
       // format.align = Alignment.Left;
       // format.textStyle = TextStyle.Bold;
       // doc.insertHeading("Heading 2:", HeadingLevel.Heading2, format);
       // doc.insertBreakLine(1);

       //*/
       // doc.insertBreakLine(1);
       // doc.saveDocument(@"f:\out.doc");

    }

    protected void koko_Click(object sender, EventArgs e)
    {
        Response.StatusCode = 401;
        Response.Status = "401 Unauthorized";
        Response.AddHeader("WWW-Authenticate", "BASIC Realm=emadbatah");

        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        Response.Cache.SetExpires(DateTime.MinValue);
        Response.Cache.SetNoStore();

        HttpContext.Current.Session.Clear();
        HttpContext.Current.Session.Abandon();
        FormsAuthentication.SignOut();

        //Response.Redirect("/");
    }

    public string Reverse(string s)
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }
    /// <summary>
    /// Receive string of words and return them in the reversed order.
    /// </summary>
    public string ReverseWords(string sentence)
    {
        string[] words = sentence.Split(' ');
        Array.Reverse(words);
        return string.Join(" ", words);
    }
}