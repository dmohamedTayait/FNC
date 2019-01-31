using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using System.IO;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Localization;
using System.Threading;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Web;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    class ReviewerHandler : BaseHandler
    {

        

        protected override void HandleRequest()
        {

            //if (_context.Request.ContentType.Contains("json")) // these are ajax json posts 
            //{
                WebFunctions.ReviewFunctions function;

                if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.ReviewFunctions>(AjaxFunctionName, true, out function))
                {
                    string jsonStringOut = null;
                    long sessionId;
                    long sessionContentItemId;
                    long reviewerUserId;
                    bool updatedbyReviewer;
                    Hashtable emailData = new Hashtable();
                    switch (function)
                    {
                        case WebFunctions.ReviewFunctions.GetSessionStatus:
                            if (SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                SessionDetails session = EMadbatahFacade.GetSessionBySessionID(sessionId);
                                if (session != null)
                                {
                                    jsonStringOut = SerializeObjectInJSON((int)session.Status);
                                }
                                else
                                {
                                    jsonStringOut = SerializeObjectInJSON(-1);
                                }

                            }
                            break;

                        case WebFunctions.ReviewFunctions.IsSessionFileLockedByFileRev:
                            if (SessionContentItemID != null && long.TryParse(SessionContentItemID, out sessionContentItemId))
                            {
                                jsonStringOut = SerializeObjectInJSON(DAL.SessionFileHelper.IsSessionFileLockedForFileReviewer(sessionContentItemId));
                            }
                            break;

                        case WebFunctions.ReviewFunctions.ApproveSession:
                            if (SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionId);
                                bool res = false;
                                if (sd.Status == SessionStatus.Completed)
                                {
                                    res = ReviewerFacade.ApproveSession(sessionId);
                                    sd.Status = res == true ? SessionStatus.Approved : sd.Status;
                                    //EMAIL ApproveSession
                                    if (res)
                                    {                                        
                                        string toEmail = CurrentUser.Email;
                                        string toUserName = CurrentUser.Name;
                                        string sessionName = EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial);
                                        string sessionDateStr = sd.Date.Date.ToShortDateString();
                                        emailData.Add("<%SessionName%>", sessionName);
                                        emailData.Add("<%SessionDate%>", sessionDateStr);
                                        emailData.Add("<%UserName%>", sd.ReviewerName);//reviewr name

                                        MailManager.SendMail(new Email(new Emailreceptionist(toEmail, toUserName)), SystemMailType.ApproveSession, emailData);

                                        //CREATE MADBATAH PDF
                                        //RUN IN A NEW THREAD
                                        //Thread t = new Thread(new ThreadStart(CreateMadbatahFiles));
                                        
                                        Thread t2 = new Thread(new ParameterizedThreadStart(EMadbatahFacade.CreateMadbatahFiles));
                                        
                                        object[] threadParams = new object[4];
                                        threadParams[0] = sd;
                                        threadParams[1] = CurrentUser;
                                        threadParams[2] = _context;
                                        threadParams[3] = FileVersion.draft;
                                        t2.Start(threadParams);

                                    }
                                }
                                
                                jsonStringOut = SerializeObjectInJSON(res);


                            }
                            break;

                        case WebFunctions.ReviewFunctions.ApproveSessionContentItem:
                            if (SessionContentItemID != null && long.TryParse(SessionContentItemID, out sessionContentItemId) &&
                                 SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                DAL.SessionContentItem curContItem = DAL.SessionContentItemHelper.GetSessionContentItemById(sessionContentItemId);
                                long unknownAgendaItemID = DAL.AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف", sessionId).ID;

                                if (curContItem.AttendantID == (int)Model.AttendantType.UnKnown || curContItem.AgendaItemID == unknownAgendaItemID)
                                {
                                    jsonStringOut = SerializeObjectInJSON(-2);
                                    break;
                                }
                                int res = ReviewerFacade.ApproveSessionContentItem(sessionContentItemId, ReviewerNote, CurrentUser);

                                //if all sessionfiles completed and session start also
                                //if 0 rejected and 0 modefied after approve
                                //mark session as completed

                                SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionId);

                                //session statistics
                                int nRejected = -1;
                                int nModefiedAfterApprove = -1;
                                int nFixed = -1;
                                Hashtable tblStats = EMadbatahFacade.GetSessionStatistics(CurrentUser, sessionId);
                                                        if (tblStats != null)
                        {
                             nRejected = int.Parse(tblStats[(int)SessionContentItemStatus.Rejected].ToString());
                             nModefiedAfterApprove = int.Parse(tblStats[(int)SessionContentItemStatus.ModefiedAfterApprove].ToString());
                             nFixed = int.Parse(tblStats[(int)SessionContentItemStatus.Fixed].ToString());
                        }

                                var notCompletedSessionFiles = from sf in sd.SessionFiles
                                                               where (sf.Status != Model.SessionFileStatus.Completed)
                                                               select sf;

                                if (notCompletedSessionFiles.ToList<SessionAudioFile>().Count == 0 
                                    && tblStats != null
                                    && nRejected == 0
                                    && nModefiedAfterApprove == 0
                                    && nFixed == 0)
                                    //&& !DAL.SessionContentItemHelper.DoesSessionContainsUndefinedAttendants(sessionId))
                                {
                                    EMadbatahFacade.UpdateSessionStatus(sessionId, Model.SessionStatus.Completed);
                                }



                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                        case WebFunctions.ReviewFunctions.FinalApproveSession:
                            if (SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                bool res = false;
                                SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionId);
                                if (sd.Status == SessionStatus.Approved)
                                {
                                    //code to insert session content items in eparl, when successed complete the below code "koko"
                                    //do we need to create the files again
                                    res = ReviewerFacade.FinalApproveSession(sessionId);
                                    if (res)
                                    {
                                        string sessionFilesPath = _context.Server.MapPath("~") + "\\SessionFiles\\" + sd.EparlimentID;
                                        if (!Directory.Exists(sessionFilesPath))
                                            Directory.Delete(sessionFilesPath);//we have to delete temp word and html files also

                                        //EMAIL final approve
                                        //string toEmail = CurrentUser.Email;
                                        //string toUserName = CurrentUser.Name;
                                        //string sessionName = EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial);
                                        //string sessionDateStr = sd.Date.Date.ToShortDateString();
                                        //emailData.Add("<%SessionName%>", sessionName);
                                        //emailData.Add("<%SessionDate%>", sessionDateStr);
                                        //emailData.Add("<%UserName%>", sd.ReviewerName);//reviewr name
                                        //MailManager.SendMail(new Email(new Emailreceptionist(toEmail, toUserName)), SystemMailType.FinalApproveSession, emailData);

                                        
                                        Thread t2 = new Thread(new ParameterizedThreadStart(EMadbatahFacade.CreateMadbatahFiles));

                                        object[] threadParams = new object[4];
                                        threadParams[0] = sd;
                                        threadParams[1] = CurrentUser;
                                        threadParams[2] = _context;
                                        threadParams[3] = FileVersion.final;
                                        t2.Start(threadParams);

                                    }
                                }                                
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                        case WebFunctions.ReviewFunctions.RejectSessionContentItem:
                            if (SessionContentItemID != null && long.TryParse(SessionContentItemID, out sessionContentItemId) &&
                                SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                int res = ReviewerFacade.RejectSessionContentItem(sessionContentItemId, ReviewerNote, CurrentUser);
                                EMadbatahFacade.UpdateSessionStatus(sessionId, SessionStatus.InProgress);
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                        case WebFunctions.ReviewFunctions.UpdateSessionContentItemText:
                            if (SessionContentItemID != null && long.TryParse(SessionContentItemID, out sessionContentItemId) && SessionContentItemText !=null)
                            {
                                int res = ReviewerFacade.UpdateSessionContentItemText(sessionContentItemId, System.Web.HttpUtility.HtmlDecode(SessionContentItemText), CurrentUser.ID, ReviewerNote);
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                        case WebFunctions.ReviewFunctions.ApproveRejectedItemsInFile:
                            long sessionFileID = -1;
                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID))
                            {
                                int res = DAL.SessionContentItemHelper.AprroveFileSessionContentItems(sessionFileID);
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                        default:
                            break;
                    }

                    if (jsonStringOut != null)
                    {
                        _context.Response.AddHeader("Encoding", "UTF-8");
                        _context.Response.Write(jsonStringOut);
                    }
                }
                else
                    return;


            //}
        }//end ProcessRequest

        public string SessionContentItemText
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.CONTENT_ITEM_TEXT, _context);                
            }
        }
       
        public string ReviewerNote
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.REVIEWER_NOTE, _context);                
            }
        }
       
    }
}
