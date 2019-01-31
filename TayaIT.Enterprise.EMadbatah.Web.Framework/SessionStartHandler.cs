using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.DAL;
namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    class SessionStartHandler : BaseHandler
    {
        protected override void HandleRequest()
        {

            //if (_context.Request.ContentType.Contains("json")) // these are ajax json posts 
            //{
                WebFunctions.SessionStartFunctions function;

                if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.SessionStartFunctions>(AjaxFunctionName, true, out function))
                {
                    string jsonStringOut = null;
                    long sessionId;
                    long sessionFileID;
                    switch (function)
                    {
                        case WebFunctions.SessionStartFunctions.ReloadAutomaticSessionStart:
                            if (SessionID != null && long.TryParse(SessionID, out sessionId))
                            {
                                string res = SessionStartFacade.GetAutomaticSessionStartText(sessionId);
                                jsonStringOut = res;
                            }
                            break;

                        case WebFunctions.SessionStartFunctions.SaveSessionStart:
                            if (SessionContentItemText != null
                                && SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID)

                                )
                            {
                                string decodedSessionStartHTML = System.Web.HttpUtility.HtmlDecode(SessionContentItemText);
                                //jsonStringOut = SerializeObjectInJSON(SessionStartFacade.AddUpdateSessionStart(sessionId, decodedSessionStartHTML, CurrentUser.ID));
                                jsonStringOut = SerializeObjectInJSON(TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStartText(sessionFileID, decodedSessionStartHTML, ReviewerNote)
                                    );
                                if (SessionStartHelper.GetSessionStartById(sessionFileID).Status == (int)Model.SessionFileStatus.SessionStartRejected)
                                {
                                    SessionFileHelper.UpdateSessionFileStatus(sessionFileID, (int)Model.SessionFileStatus.SessionStartFixed, CurrentUser.ID);
                                }
                                else
                                {
                                    SessionFileHelper.UpdateSessionFileStatus(sessionFileID, (int)Model.SessionFileStatus.Completed, CurrentUser.ID);
                                }
                                // TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(start.ID, (int)Model.SessionFileStatus.InProgress);



                                long sessionID = -1;

                                if (SessionID != null && long.TryParse(SessionID, out sessionID))
                                {
                                    Model.SessionDetails sd = EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
                                    var notCompletedSessionFiles = from sf in sd.SessionFiles
                                                                   where ((sf.ID != sessionFileID) &&
                                                                          (sf.Status != Model.SessionFileStatus.Completed))
                                                                   select sf;

                                    if (notCompletedSessionFiles.ToList<Model.SessionAudioFile>().Count == 0)
                                    {
                                        EMadbatahFacade.UpdateSessionStatus(sessionID, Model.SessionStatus.Completed);
                                    }




                                }
                            }
                            break;
                        case WebFunctions.SessionStartFunctions.ApproveSessionStart:
                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID))
                            {
                                int res = SessionFileHelper.UpdateSessionFileStatus(sessionFileID, (int)Model.SessionFileStatus.SessionStartApproved, CurrentUser.ID, ReviewerNote);
                                jsonStringOut = res.ToString();
                                // TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(start.ID, (int)Model.SessionFileStatus.InProgress);
                            }
                            break;
                        case WebFunctions.SessionStartFunctions.RejectSessionStart:
                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID))
                            {
                                int res = SessionFileHelper.UpdateSessionFileStatus(sessionFileID, (int)Model.SessionFileStatus.SessionStartRejected, CurrentUser.ID, ReviewerNote);
                                jsonStringOut = res.ToString();
                                // TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(start.ID, (int)Model.SessionFileStatus.InProgress);
                            }
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
        }

       /* public string SessionStartHTML
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_START_HTML, _context);
            }
        }*/
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
