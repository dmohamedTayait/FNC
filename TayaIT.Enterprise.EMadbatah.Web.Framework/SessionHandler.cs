using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Config;
using System.IO;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Threading;
using System.Collections;
using System.Data.SqlClient;
using System.Data;


namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    public class SessionHandler : BaseHandler
    {

        protected override void HandleRequest()
        {
            string jsonStringOut = null;
            WebFunctions.SessionsFunctions function;

            if ((AjaxFunctionName != null && Enum.TryParse<WebFunctions.SessionsFunctions>(AjaxFunctionName, true, out function)))
            {

                long sessionID;
                long speakersType;
                int attachID;
                long agendaItemID = -1, agendaSubItemID = -1;
                switch (function)
                {
                    case WebFunctions.SessionsFunctions.CreateMadbatahFiles:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID)
                            && FileVersion != null && Enum.IsDefined(typeof(FileVersion), FileVersion) )
                        {
                            SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
                            //EMAIL ApproveSession
                            if (sd.Status == SessionStatus.Approved || sd.Status == SessionStatus.FinalApproved)
                            {
                                //Hashtable emailData = new Hashtable();
                                //string toEmail = CurrentUser.Email;
                                //string toUserName = CurrentUser.Name;
                                //string sessionName = EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial);
                                //string sessionDateStr = sd.Date.Date.ToShortDateString();
                                //emailData.Add("<%SessionName%>", sessionName);
                                //emailData.Add("<%SessionDate%>", sessionDateStr);
                                //emailData.Add("<%UserName%>", sd.ReviewerName);//reviewr name

                                //MailManager.SendMail(new Email(new Emailreceptionist(toEmail, toUserName)), SystemMailType.ApproveSession, emailData);

                                //CREATE MADBATAH PDF
                                //RUN IN A NEW THREAD
                                //Thread t = new Thread(new ThreadStart(CreateMadbatahFiles));
                                FileVersion fileVersion = (FileVersion)Enum.Parse(typeof(FileVersion), FileVersion);
                                Thread t2 = new Thread(new ParameterizedThreadStart(EMadbatahFacade.CreateMadbatahFiles));
                                object[] threadParams = new object[4];
                                threadParams[0] = sd;
                                threadParams[1] = CurrentUser;
                                threadParams[2] = _context;
                                threadParams[3] = fileVersion;
                                t2.Start(threadParams);

                            }
                            
                        }
 
                        break;
                    case WebFunctions.SessionsFunctions.GetAllSessions:
                        break;
                    case WebFunctions.SessionsFunctions.UpdateSessionInfo:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID))
                        {
                            Eparliment ep = new Eparliment();
                            
                                jsonStringOut = SerializeObjectInJSON(ep.UpdateSessionDetails(sessionID));
                            
                   
                        }
                        break;
                    case WebFunctions.SessionsFunctions.CreateNewSession:

                        break;
                    case WebFunctions.SessionsFunctions.AssignSessionStart:
                        break;

                    case WebFunctions.SessionsFunctions.GetSessionFiles:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID))
                            jsonStringOut = SerializeObjectInJSON(SessionFileFacade.GetSessionFilesBySessionID(sessionID));
                        break;

                    case WebFunctions.SessionsFunctions.GetSpeakersStatistics:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID) && SpeakersType != null)
                        {
                            bool isAllSpeakers = !SpeakersType.Equals("filteredSpeakers");//(SpeakersType == "filteredSpeakers") ? true:false;
                            
                            ICollection attendants = null;
                            


                            long.TryParse(AgendaItemID, out agendaItemID);
                            long.TryParse(AgendaSubItemID, out agendaSubItemID);
                                //get data according to ids and speakertype
                                attendants = EMadbatahFacade.GetSpeakersStats(isAllSpeakers,sessionID, agendaItemID,agendaSubItemID);


                                jsonStringOut = SerializeObjectInJSON(attendants);
                            //speakersType
                        }
                        break;

                    case WebFunctions.SessionsFunctions.UpdateSessionAgendaItemValue:
                        if ( AgendaItemName != null &&
                            ((AgendaItemID != null && long.TryParse(AgendaItemID, out agendaItemID) && agendaItemID != -1) ||
                            (AgendaSubItemID != null && long.TryParse(AgendaSubItemID, out agendaSubItemID) && agendaSubItemID != -1)))
                        {
                            int result = -1;
                            if (agendaItemID > -1)
                                result = DAL.AgendaHelper.UpdateAgendaItem(agendaItemID, AgendaItemName);
                            else if (agendaSubItemID > -1)
                                result = DAL.AgendaHelper.UpdateSubAgendaItem(agendaItemID, AgendaItemName);

                            jsonStringOut = SerializeObjectInJSON(result);
                        }
                        break;

                    case WebFunctions.SessionsFunctions.GetSessionAgendaItemsIndex:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID))
                        {
                            List<MadbatahIndexItem> index = new List<MadbatahIndexItem>();
                            List<List<DAL.SessionContentItem>> allItems = DAL.SessionContentItemHelper.GetItemsBySessionIDGrouped(sessionID);

                            List<string> writtenAgendaItems = new List<string>();

                            foreach (List<DAL.SessionContentItem> groupedItems in allItems)
                            {
                                DAL.AgendaItem curAgendaItem = groupedItems[0].AgendaItem;
                                DAL.AgendaSubItem curAgendaSubItem = groupedItems[0].AgendaSubItem;

                                if (writtenAgendaItems.IndexOf(curAgendaItem.Name) == -1)
                                {
                                    string originalName = curAgendaItem.Name;
                                    writtenAgendaItems.Add(curAgendaItem.Name);

                                    index.Add(new MadbatahIndexItem(curAgendaItem.ID, originalName, 0+"", true, "", "", curAgendaItem.IsCustom, curAgendaItem.IsGroupSubAgendaItems) { ID = curAgendaItem.ID });
                                }

                                if (curAgendaSubItem != null)
                                {
                                    index.Add(new MadbatahIndexItem(curAgendaSubItem.ID, curAgendaSubItem.Name, 0+"", false, curAgendaSubItem.QFrom, curAgendaSubItem.QTo, curAgendaSubItem.IsCustom,  curAgendaItem.IsGroupSubAgendaItems) { ID = curAgendaSubItem.ID });
                                }
                            }
                            jsonStringOut = SerializeObjectInJSON(index);
                            //Response.Write(index);
                        }
                        break;

                    case WebFunctions.SessionsFunctions.ReloadSessionFiles:
                        if (SessionID != null && long.TryParse(SessionID, out sessionID))
                        {
                            SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                            List<SessionAudioFile> filesThatAlreadyExist = new List<SessionAudioFile>();
                            string mp3FolderPath = sd.MP3FolderPath;
                            string sessionFilesPath = _context.Server.MapPath("~") + "\\SessionFiles\\" + sd.EparlimentID;

                            if (!Directory.Exists(mp3FolderPath))
                            {
                                var result = new { Sucess = false, Message = "لم يستطع التطبيق الوصول لمجلد ملفات الصوت من سيرفر الصوت" };
                                jsonStringOut = SerializeObjectInJSON(result);
                                break;
                            }

                            List<SessionAudioFile> files2Send = new List<SessionAudioFile>();
                            DirectoryInfo mp3DirInfo = new DirectoryInfo(mp3FolderPath);
                            FileInfo[] mp3Files = mp3DirInfo.GetFiles();

                            files2Send = CopyMP3Files(mp3Files, sessionFilesPath, sd.EparlimentID, sessionID, out filesThatAlreadyExist);

                            if (files2Send.Count > 0 || filesThatAlreadyExist.Count > 0)
                            {
                                EMadbatahFacade.UpdateSessionMP3Path(sessionID, mp3FolderPath);
                                //send to db

                                if (files2Send.Count == 0)
                                    files2Send = filesThatAlreadyExist;
                                bool filesAddedtoDB = SessionFileFacade.AddSessionFiles(files2Send, sessionID);
                                if (filesAddedtoDB)
                                //if (SessionFileFacade.AddSessionFiles(files2Send, sessionID))
                                {
                                    EMadbatahFacade.UpdateSessionStatus(sessionID, SessionStatus.InProgress);
                                    TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.UpdateSessionStartStatus(sd.SessionID, (int)SessionFileStatus.New);
                                    //here insert new session start
                                    //string decodedSessionStartHTML = SessionStartFacade.GetAutomaticSessionStartText(sessionID);

                                    //if (SessionStartFacade.AddNewSessionStart(sessionID, decodedSessionStartHTML, " بداية المضبطة " + sd.Serial))
                                    //{
                                    //    var result = new { Sucess = true, Message = "" };
                                    //    jsonStringOut = SerializeObjectInJSON(result);
                                    //}
                                    //else
                                    //{
                                        var result = new { Sucess = true, Message = "" };
                                        jsonStringOut = SerializeObjectInJSON(result);
                                    //}
                                }
                                else
                                {
                                    var result = new { Sucess = false, Message = "لقد حدث خطأ أثناء إدخال الملفات إلي قاعدة البيانات" };
                                    jsonStringOut = SerializeObjectInJSON(result);
                                }
                            }
                            else
                            {
                                var result = new { Sucess = false, Message = "لا يوجد ملفات جديدة" };
                                jsonStringOut = SerializeObjectInJSON(result);
                            }
                            //}//endelse

                        }
                        break;

                    case WebFunctions.SessionsFunctions.ConfirmNewSession:
                        //get session

                        if (SessionID != null && long.TryParse(SessionID, out sessionID)
                            && NewSessionAudioFolderPath != null && NewSessionXMLFolderPath != null)
                        {

                            SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                            int eparID = sd.EparlimentID;

                            List<SessionAudioFile>  filesThatAlreadyExist = new List<SessionAudioFile>();

                            //if sessionFiles exist we dont do anything
                            //if (SessionFileFacade.GetSessionFilesCount(sessionID) > 0)
                            //{
                            //    return;
                            //}

                            string xmlPath = HttpUtility.UrlDecode(NewSessionXMLFolderPath);
                            string mp3Path = HttpUtility.UrlDecode(NewSessionAudioFolderPath);

                            if (!Directory.Exists(xmlPath))
                            {
                                var result = new { Sucess = false, Message = "لم يستطع التطبيق الوصول لمجلد ملفات النص من سيرفر تحويل الصوت إلى نص" };
                                jsonStringOut = SerializeObjectInJSON(result);
                                break;
                            }

                            if (!Directory.Exists(mp3Path))
                            {
                                var result = new { Sucess = false, Message = "لم يستطع التطبيق الوصول لمجلد ملفات الصوت من سيرفر الصوت" };
                                jsonStringOut = SerializeObjectInJSON(result);
                                break;
                            }

                            //&& Directory.Exists(mp3Path))


                            List<SessionAudioFile> files2Send = null;
                            DirectoryInfo mp3DirInfo = new DirectoryInfo(mp3Path);
                            DirectoryInfo xmlDirInfo = new DirectoryInfo(xmlPath);
                            //int count = 0;
                            string sessionFilesPath = _context.Server.MapPath("~") + "\\SessionFiles\\" + eparID;

                            //check if XML files count == mp3 files count
                            FileInfo[] mp3Files = mp3DirInfo.GetFiles();
                            FileInfo[] xmlFiles = xmlDirInfo.GetFiles();
                            
                            int mp3FilesCount = mp3Files.Length;

                            //now the session can start and run without all MP3 Files completed
                            //int xmlFilesCount = 0;
                            //bool xmlFileMissing = false;
                            //foreach (FileInfo mp3File in mp3Files)
                            //{
                            //    if (mp3File.Name.ToLower().EndsWith("mp3") && 
                            //        !mp3File.Name.ToLower().Contains("full") && 
                            //        !mp3File.Name.ToLower().Contains("opening"))
                            //    {
                            //        //xmlFiles.Contains(new FileInfo(AppConfig.GetInstance().VecSysServerPath + "\\"+ mp3File.Name.Replace(".mp3", ".trans.xml"))))
                            //        if (File.Exists( AppConfig.GetInstance().VecSysServerPath + "\\"+ mp3File.Name.Replace(".mp3", ".trans.xml")))
                            //        {
                            //            xmlFilesCount++;
                            //        }
                            //        else
                            //        {
                            //            xmlFileMissing = true;
                            //            break;
                            //        }      
                            //    }                                                                                                  
                            //}

                           
                            //if (xmlFileMissing)
                            //{
                            //    var result = new { Sucess=false, Message="لا يمكن بدئ جلسة جديدة .. يوجد ملف صوتي ليس له ملف نصي مكافئ"};
                            //    jsonStringOut = SerializeObjectInJSON(result);
                            //    break;
                            //}
                            //else
                            //{
                                

                            // copyfiles From Share
                            files2Send = CopyMP3Files(mp3Files, sessionFilesPath, eparID, sessionID, out filesThatAlreadyExist);

                            if (files2Send.Count > 0 || filesThatAlreadyExist.Count > 0)
                                {
                                    EMadbatahFacade.UpdateSessionMP3Path(sessionID, mp3Path);
                                    //send to db
                                if(files2Send.Count == 0)
                                    files2Send = filesThatAlreadyExist;
                                bool filesAddedtoDB = SessionFileFacade.AddSessionFiles(files2Send, sessionID);
                                    if (filesAddedtoDB)
                                    {
                                        EMadbatahFacade.UpdateSessionStatus(sessionID, SessionStatus.InProgress);

                                      
                                        //here insert new session start
                                        string decodedSessionStartHTML = SessionStartFacade.GetAutomaticSessionStartText(sessionID);
                                        
                                        if (SessionStartFacade.AddNewSessionStart(sessionID, decodedSessionStartHTML, " بداية المضبطة " + sd.Serial))
                                        {
                                            var result = new { Sucess = true, Message = "" };
                                            jsonStringOut = SerializeObjectInJSON(result);
                                        }
                                        else
                                        {
                                            var result = new { Sucess = false, Message = "لقد حدث خطأ أثناء إدخال بداية المضبطة" };
                                            jsonStringOut = SerializeObjectInJSON(result);
                                        }


                                    }
                                    else
                                    {
                                        var result = new { Sucess = false, Message = "لقد حدث خطأ أثناء إدخال الملفات إلي قاعدة البيانات" };
                                        jsonStringOut = SerializeObjectInJSON(result);
                                    }
                                }
                                else
                                {
                                    var result = new { Sucess = false, Message = "لقد حدث خطأ .. لا يوجد ملفات للعمل عليها" };
                                    jsonStringOut = SerializeObjectInJSON(result);
                                }
                            //}//endelse

                            
                            
                            //jsonStringOut = SerializeObjectInJSON();
                        }
                            
                        break;

                    case WebFunctions.SessionsFunctions.ReorderSessionFiles:
                        break;
                    case WebFunctions.SessionsFunctions.AddAttachment:
                        break;
                    case WebFunctions.SessionsFunctions.RemoveAttachment:
                        if (AttachmentID != null && int.TryParse(AttachmentID, out attachID))
                        {
                            jsonStringOut = SerializeObjectInJSON(TayaIT.Enterprise.EMadbatah.DAL.AttachmentHelper.DeleteAttachementByID(attachID));
                            
                        }
                        break;
                    case WebFunctions.SessionsFunctions.ReOrderAttachment:
                        break;
                    case WebFunctions.SessionsFunctions.GetReviewNotesSummaryForDataEntry:
                        break;
                    case WebFunctions.SessionsFunctions.GetReviewNotesSummaryForReviewer:
                        break;
                    case WebFunctions.SessionsFunctions.UpdateAttendantSessionTitle:
                        string x = "x";
                        int status = EMadbatahFacade.update_session_attendant_title(Convert.ToInt64(SessionID), Convert.ToInt64(AttendantID), Convert.ToInt64(SessionAttendantTitleID));
                        jsonStringOut = "true";
                        break;
                    default:
                        break;
                }
            }

            if (jsonStringOut != null)
            {
                _context.Response.AddHeader("Encoding", "UTF-8");
                _context.Response.Write(jsonStringOut);
            }
        }


        public List<SessionAudioFile> CopyMP3Files(FileInfo[] mp3Files, string sessionFilesPath, int eparID, long sessionID, out List<SessionAudioFile> filesThatAlreadyExist)
        {
            List<SessionAudioFile> files2Send = new List<SessionAudioFile>();
            filesThatAlreadyExist = new List<SessionAudioFile>();

            int count = 0;
            foreach (FileInfo file in mp3Files)
            {
                if (file.Name.ToLower().EndsWith("mp3") &&
                !file.Name.ToLower().Contains("full") &&
                !file.Name.ToLower().Contains("opening"))
                {
                    count++;
                    string tmpFileName = file.Name.Replace(".mp3", "");
                    int orderIndex = tmpFileName.LastIndexOf("_") + 1;
                    string orderStr = "";
                    int order = 0;
                    if (orderIndex > 1)
                    {
                        orderStr = tmpFileName.Substring(orderIndex);
                        if (!int.TryParse(orderStr, out order))
                            order = count;
                    }
                    else
                        order = count;

                    if (!Directory.Exists(sessionFilesPath))
                        Directory.CreateDirectory(sessionFilesPath);

                    string newFileName = sessionFilesPath + "\\" + new FileInfo(file.FullName).Name;

                    bool xmlFileExist = false;

                    bool xmlFileExistLocally = false;
                    bool mp3FileExistLocally = false;

                    if (File.Exists(newFileName))
                        mp3FileExistLocally = true;
                    if (File.Exists(newFileName.Replace(".mp3", ".trans.xml")))
                        xmlFileExistLocally = true;
                    

                    if (!File.Exists(newFileName) && File.Exists(AppConfig.GetInstance().VecSysServerPath + "\\" + file.Name.Replace(".mp3", ".trans.xml")))
                        File.Copy(file.FullName, newFileName);

                    if (!File.Exists(newFileName.Replace(".mp3", ".trans.xml")) &&
                        File.Exists(AppConfig.GetInstance().VecSysServerPath + "\\" + file.Name.Replace(".mp3", ".trans.xml")) &&
                        File.Exists(AppConfig.GetInstance().VecSysServerPath + "\\" + file.Name.Replace(".mp3", ".trans.txt")) )
                    {
                        File.Copy(AppConfig.GetInstance().VecSysServerPath + "\\" + file.Name.Replace(".mp3", ".trans.xml"),
                            newFileName.Replace(".mp3", ".trans.xml"));
                        xmlFileExist = true;
                    }
                    string fileNameToSaveInDB = "\\SessionFiles\\" + eparID + "\\" + new FileInfo(file.FullName).Name;

                    long mp3FileDuration = FileHelper.GetMP3FileDurationAsLong(file.FullName); // calculate duration
                    SessionAudioFile saf = new SessionAudioFile(sessionID, fileNameToSaveInDB, order, mp3FileDuration, SessionFileStatus.New);

                    if(xmlFileExist)
                         files2Send.Add(saf);

                    if (mp3FileExistLocally && xmlFileExistLocally)
                        filesThatAlreadyExist.Add(saf);
                }
            }

            return files2Send;
        }



        public string NewSessionAudioFolderPath
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.NEW_SESSION_AUDIO_FOLDER_PATH, _context);                
            }
        }

        public string NewSessionXMLFolderPath
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.NEW_SESSION_XML_FOLDER_PATH, _context);
            }
        }
        public string FileVersion
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.FILE_VERSION, _context);
            }
        }

        public string SpeakersType
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SPEAKERS_TYPE, _context);
            }
        }

        public string AgendaItemName
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.AGENDA_ITEM_NAME, _context);
            }
        }

    }
}
