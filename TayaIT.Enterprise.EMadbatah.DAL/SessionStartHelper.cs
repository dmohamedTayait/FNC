using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class SessionStartHelper
    {
        //public static bool AddSessionStart(string sessionStartText,long user_id)
        //{
        //    try
        //    {
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            SessionStart session_start_item = new SessionStart
        //            {
        //                UserID = user_id,
        //                Text = sessionStartText
        //            };
        //            context.SessionStarts.AddObject(session_start_item);
        //            int result = context.SaveChanges();
        //            //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, session_start_item);
        //            return true;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.AddNewSessionStart(" + user_id + ")");
        //        return false;
        //    }
        //}

        public static SessionFile AddSessionStart(string sessionStartText, long user_id, long sessionID, string startName)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile session_start_item = new SessionFile
                    {
                        UserID = user_id,
                        SessionStartText = sessionStartText,
                        IsSessionStart = true,
                        SessionID =sessionID,
                        LastModefied = DateTime.Now,
                        Name = startName,
                        Status = 3
                    };
                    context.SessionFiles.AddObject(session_start_item);
                    int result = context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, session_start_item);
                    return session_start_item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.AddNewSessionStart(" + user_id + ")");
                return null;
            }
        }

        public static SessionFile AddSessionStart(string sessionStartText, long sessionID, string startName)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile session_start_item = new SessionFile
                    {
                        UserID = null,
                        SessionStartText = sessionStartText,
                        IsSessionStart = true,
                        SessionID = sessionID,
                        Name = startName,
                        Status = 1
                    };
                    context.SessionFiles.AddObject(session_start_item);
                    int result = context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, session_start_item);
                    return session_start_item;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.AddSessionStart()");
                return null;
            }
        }

        public static int DeleteSessionStartById(long session_start_id)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile session_start = context.SessionFiles.FirstOrDefault(c => c.ID == session_start_id && c.IsSessionStart == true);
                    context.DeleteObject(session_start);
                    int result = context.SaveChanges();
                    return result;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.DeleteSessionStartById(" + session_start_id + ")");
                return -1;
            }
        }

        public static int UpdateSessionStartText(long session_start_id, string session_start_text, long userID)
        {
            try
            {
                SessionFile updated_session_start = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_start = context.SessionFiles.FirstOrDefault(c => c.ID == session_start_id && c.IsSessionStart == true);
                    if (updated_session_start != null)
                    {
                        //update the SessionStart attributes
                        updated_session_start.SessionStartText = session_start_text;
                        updated_session_start.UserID = userID;
                        updated_session_start.LastModefied = DateTime.Now;
                        updated_session_start.Name = "بداية المضبطة";
                        updated_session_start.Status = 3;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.UpdateSessionStartText(" + session_start_id + ")");
                return -1;
            }
        }


        public static int UpdateSessionStartStatus(long sessionid, int sessionStartStatusID)
        {
            try
            {
                SessionFile updated_session_start = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_start = context.SessionFiles.FirstOrDefault(c => c.SessionID == sessionid && c.IsSessionStart == true);
                    if (updated_session_start != null)
                    {
                        updated_session_start.Status = sessionStartStatusID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.UpdateSessionStartStatus(" + sessionid + ")");
                return -1;
            }
        }

        public static SessionFile GetSessionStartById(long session_start_id)
        {
            try
            {
                SessionFile session_start = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    session_start = context.SessionFiles.FirstOrDefault(c => c.ID == session_start_id && c.IsSessionStart == true);
                }
                return session_start;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.GetSessionStartById(" + session_start_id + ")");
                return null;
            }
        }
        public static SessionFile GetSessionStartBySessionId(long session_id)
        {
            try
            {
                SessionFile session_start = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                   session_start = context.SessionFiles.FirstOrDefault(c => c.SessionID == session_id && c.IsSessionStart == true);
                   if (session_start == null)
                       return null;
                   session_start.User = session_start.User;
                   session_start.FileReviewer = session_start.FileReviewer;
                   session_start.Reviewer = session_start.Reviewer;
                }
                return session_start;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.GetSessionStartBySessionId(" + session_id + ")");
                return null;
            }
        }
        public static List<SessionFile> GetSessionStartByUserId(long user_id)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var session_starts = from ss in context.SessionFiles
                                         where ss.UserID == user_id
                                         && ss.IsSessionStart == true
                                         select ss;
                    return session_starts.ToList<SessionFile>();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.GetSessionStartByUserId(" + user_id + ")");
                return null;
            }
        }

    }
}
