using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Data.Objects.DataClasses;
//using TayaIT.Enterprise.EMadbatah.Model;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class SessionHelper
    {
        #region Session
        //session info come from E-Parliment webservice
        public static bool AddNewSession(
            DateTime date,
            DateTime startTime,
            DateTime endTime,
            string type,
            string president,
            string place,
            long agendaId,
            int sessionStatusId,
            int ePalimentID,
            long season,
            long stage,
            string stageType,
            long serial
            )
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session session_item = new Session
                    {
                        Date = date,
                        StartTime = startTime,
                        EndTime = endTime,
                        Type = type,
                        President = president,
                        Place = place,
                        SessionStatusID = sessionStatusId,
                        EParliamentID = ePalimentID,
                        Season = season,
                        Stage = stage,
                        StageType = stageType,
                        Serial = serial,
                    };
                    context.Sessions.AddObject(session_item);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.AddNewSession(" + serial + ")");
                return false;
            }
        }

        public static long AddNewSession(Session session, string customAgendaItemName)
        {
            try
            {

                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    context.Sessions.AddObject(session);
                    int result = context.SaveChanges();

                    context.AgendaItems.AddObject(new AgendaItem()
                    {
                        EParliamentID = null,
                        EParliamentParentID = null,
                        Name = customAgendaItemName,
                        SessionID = result,
                        Order = 0
                    });

                    return session.ID;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.AddNewSession(Session)(" + session.Serial+ ")");
                return -1;
            }
        }

        //public static int 

        //???
        public static List<Session> GetSessions(int pageNo, int count)
        {
            try
            {
                pageNo--;

                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    List<Session> allSessions = new List<Session>();
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        allSessions = context.Sessions
                                        .OrderByDescending(c => c.Date)
                                        .Skip(count * pageNo)
                                        .Take(count).ToList<Session>();

                       

                        foreach (Session session in allSessions)
                        {
                            //session.AgendaItems.Load();
                            //foreach (AgendaItem item in session.AgendaItems)
                            //{
                            //    item.AgendaSubItems.Load();
                            //}

                            session.Attachements.Load();
                            //session.Attendants.Load();
                            session.SessionFiles.Load();

                            // EntityCollection<SessionFile> tmpSessionFiles = new EntityCollection<SessionFile>();

                            foreach (SessionFile sf in session.SessionFiles)
                            {
                                sf.User = sf.User;
                                sf.FileReviewer = sf.FileReviewer;
                                session.SessionFiles.Add(sf);
                            }

                            //session.SessionFiles = tmpSessionFiles;
                        }
                    }
                    return allSessions;



                    //List<Session> ss=  context.Session.Take(5).ToList();
                    //var session = (from obj in context.Session
                    //               where obj.ID == 1
                    //               select obj);
                    //session.
                }
                //return allSessions;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessions(" + pageNo + "," + count + ")");
                return null;
            }



        }

        public static List<Session> GetSessions(int pageNo, int count, int statusCode, bool checkEquality)
        {
            try
            {
                pageNo--;
                
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    List<Session> allSessions = new List<Session>();
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        //allSessions = context.Sessions
                        //                .OrderByDescending(c => c.Date)
                        //                .Skip(count * pageNo)
                        //                .Take(count).ToList<Session>();

                        if (checkEquality)
                            allSessions = context.Sessions
                                        .Where(c => c.SessionStatusID == statusCode)
                                        .OrderByDescending(c => c.Date)
                                        .Skip(count * pageNo)
                                        .Take(count).ToList<Session>();
                        else
                            allSessions = context.Sessions
                                        .Where(c => c.SessionStatusID != statusCode)
                                        .OrderByDescending(c => c.Date)
                                        .Skip(count * pageNo)
                                        .Take(count).ToList<Session>();

                        foreach (Session session in allSessions)
                        {
                            //session.AgendaItems.Load();
                            //foreach (AgendaItem item in session.AgendaItems)
                            //{
                            //    item.AgendaSubItems.Load();
                            //}

                            session.Attachements.Load();
                            //session.Attendants.Load();
                            session.SessionFiles.Load();

                           // EntityCollection<SessionFile> tmpSessionFiles = new EntityCollection<SessionFile>();

                            foreach (SessionFile sf in session.SessionFiles)
                            {
                                sf.User = sf.User;
                                sf.FileReviewer = sf.FileReviewer;
                                session.SessionFiles.Add(sf);
                            }

                            //session.SessionFiles = tmpSessionFiles;
                        }
                    }
                    return allSessions;



                    //List<Session> ss=  context.Session.Take(5).ToList();
                    //var session = (from obj in context.Session
                    //               where obj.ID == 1
                    //               select obj);
                    //session.
                }
                //return allSessions;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessions(" + pageNo + "," + count + ")");
                return null;
            }



        }
        //public static List<Session> GetFinalApprovedSessions(int pageNo, int count, int statusCode)
        //{
        //    try
        //    {
        //        pageNo--;

        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            List<Session> allSessions = new List<Session>();
        //            if (context.Sessions.Count<Session>() > 0)
        //            {
        //                allSessions = context.Sessions
        //                                .Where(c => c.SessionStatusID == statusCode)
        //                                .OrderByDescending(c => c.Date)
        //                                .Skip(count * pageNo)
        //                                .Take(count).ToList<Session>();

        //                foreach (Session session in allSessions)
        //                {
        //                    //session.AgendaItems.Load();
        //                    //foreach (AgendaItem item in session.AgendaItems)
        //                    //{
        //                    //    item.AgendaSubItems.Load();
        //                    //}

        //                    session.Attachements.Load();
        //                    //session.Attendants.Load();
        //                    session.SessionFiles.Load();

        //                    // EntityCollection<SessionFile> tmpSessionFiles = new EntityCollection<SessionFile>();

        //                    foreach (SessionFile sf in session.SessionFiles)
        //                    {
        //                        sf.User = sf.User;
        //                        sf.FileReviewer = sf.FileReviewer;
        //                        session.SessionFiles.Add(sf);
        //                    }

        //                    //session.SessionFiles = tmpSessionFiles;
        //                }
        //            }
        //            return allSessions;



        //            //List<Session> ss=  context.Session.Take(5).ToList();
        //            //var session = (from obj in context.Session
        //            //               where obj.ID == 1
        //            //               select obj);
        //            //session.
        //        }
        //        //return allSessions;
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessions(" + pageNo + "," + count + ")");
        //        return null;
        //    }



        //}

        public static Session GetSessionByID(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session toRet = null;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        toRet = context.Sessions.FirstOrDefault(s => s.ID == sessionID);
                      //  foreach (Attendant attendant in toRet.Attendants.ToList<Attendant>())
                      //  {
                      //      attendant.SessionAttendant.Where(x => x. == sessionID);//("session_id=" + sessionID.ToString(),);
                       //     toRet.Attendants.Add(attendant);
                        // }
                        //toRet.SessionAttendtant.Where(
                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionByID(" + sessionID + ")");
                return null;
            }
        }


        public static Session GetSessionDetailsByID(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session toRet = null;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        toRet = context.Sessions.FirstOrDefault(s => s.ID == sessionID);
                        if (toRet != null)
                        {
                            toRet.AgendaItems.Load();


                            foreach (AgendaItem item in toRet.AgendaItems)
                            {
                                item.AgendaSubItems.Load();
                                toRet.AgendaItems.Add(item);
                            }


                            toRet.Attachements.Load();
                            toRet.Attendants.Load();
                            toRet.SessionFiles.Load();

                            foreach (SessionFile item in toRet.SessionFiles)
                            {
                                item.User = item.User;
                                item.FileReviewer = item.FileReviewer;
                                toRet.SessionFiles.Add(item);
                            }

                            

                        }
                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionByID(" + sessionID + ")");
                return null;
            }
        }









        public static Session GetSessionDetailsByEParlimentID(long epid)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session toRet = null;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        toRet = context.Sessions.FirstOrDefault(s => s.EParliamentID == epid);
                        if (toRet != null)
                        {
                            toRet.AgendaItems.Load();


                            foreach (AgendaItem item in toRet.AgendaItems)
                            {
                                item.AgendaSubItems.Load();
                                toRet.AgendaItems.Add(item);
                            }


                           
                            toRet.Attendants.Load();
                           


                        }
                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionDetailsByEParlimentID(" + epid + ")");
                return null;
            }
        }






        public static Session GetSessionByEParlimentID(long sessionEParlimentID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session toRet = null;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        toRet = context.Sessions.FirstOrDefault(s => s.EParliamentID == sessionEParlimentID);                         
                         return toRet;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionByEParlimentID(" + sessionEParlimentID + ")");
                return null;
            }
        }


        public static bool IsSessionWordFileAndPDFCreated(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null && session_item.SessionMadbatahWord != null && session_item.SessionMadbatahPDF != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.IsSessionWordFileAndPDFCreated(" + sessionID + ")");
                return false;
            }
        }

        public static bool IsSessionFinalWordFileAndPDFCreated(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null && session_item.SessionMadbatahWordFinal != null && session_item.SessionMadbatahPDFFinal != null)
                        {
                            return true;
                        }
                        else
                            return false;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.IsSessionWordFileAndPDFCreated(" + sessionID + ")");
                return false;
            }
        }

        public static int GetSessionMadabathFilesStatus(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        return (int)session_item.MadbatahFilesStatusID;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionMadabathFilesStatus(" + sessionID + ")");
                return -1;
            }
        }


        public static int UpdateSessionMadabathFilesStatus(long sessionID, int newStatus)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        session_item.MadbatahFilesStatusID = newStatus;

                       return  context.SaveChanges();
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionMadabathFilesStatus(" + sessionID + ")");
                return -1;
            }
        }
                

        public static byte[] GetSessionWordFile(long sessionID)
        {
            try
            {
                byte[] word_file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null)
                        {
                            word_file = session_item.SessionMadbatahWord;
                        }
                    }
                }
                return word_file;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionWordFile(" + sessionID + ")");
                return null;
            }

        }

        public static byte[] GetSessionPDFFile(long sessionID)
        {
            try
            {
                byte[] pdf_file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null)
                        {
                            pdf_file = session_item.SessionMadbatahPDF;
                        }
                    }
                }
                return pdf_file;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionPDFFile(" + sessionID + ")");
                return null;
            }
        }

        public static byte[] GetSessionWordFinalFile(long sessionID)
        {
            try
            {
                byte[] word_file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null)
                        {
                            word_file = session_item.SessionMadbatahWordFinal;
                        }
                    }
                }
                return word_file;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionWordFinalFile(" + sessionID + ")");
                return null;
            }

        }
        public static byte[] GetSessionPDFFinalFile(long sessionID)
        {
            try
            {
                byte[] pdf_file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (session_item != null)
                        {
                            pdf_file = session_item.SessionMadbatahPDFFinal;
                        }
                    }
                }
                return pdf_file;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionPDFFinalFile(" + sessionID + ")");
                return null;
            }
        }
        public static int UpdateSessionStatus(long sessionID, int statusID)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            updated_session.SessionStatusID = statusID;
                        }
                        res = context.SaveChanges();
                        
                    }
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionStatus(" + sessionID + "," + statusID + ")");
                return -1;
            }
        }


        public static int UpdateSessionMP3FilePath(long sessionID, string mp3path)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            updated_session.MP3FolderPath = mp3path;
                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionMP3FilePath(" + sessionID + "," + mp3path + ")");
                return -1;
            }
        }

        public static int GetSessionChecker(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        Session session_item = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        return (int)session_item.Checker;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionChecker(" + sessionID + ")");
                return -1;
            }
        }
        public static int UpdateSessionChecker(long sessionID, int checker)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            updated_session.Checker = checker;
                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionChecker(" + sessionID  + ")");
                return -1;
            }
        }


        // added by ihab
        public static int GetSessionFileError(long sessionFileID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    if (context.SessionFiles.Count<SessionFile>() > 0)
                    {
                        SessionFile session_item = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                        return (int)session_item.FileError;
                    }
                    return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionContentItemFileError(" + sessionFileID + ")");
                return -1;
            }
        }
        public static int UpdateSessionFileError(long sessionFileID, int fileError)
        {
            try
            {
                SessionFile updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    int res = 0;
                    if (context.SessionFiles.Count<SessionFile>() > 0)
                    {
                        updated_session = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                        if (updated_session != null)
                        {
                            updated_session.FileError = fileError;
                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionContentItemFileError(" + sessionFileID + ")");
                return -1;
            }
        }
        // added by ihab
        public static int UpdateSessionReviewer(long sessionID, long revID)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    List<SessionContentItem> scis = (from sci in context.SessionContentItems
                                                     where sci.SessionID == sessionID
                                                     select sci).ToList<SessionContentItem>();

                    foreach (SessionContentItem sci in scis)
                    {
                        if (sci.ReviewerUserID != null)
                            sci.ReviewerUserID = revID;
                    }

                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            updated_session.ReviewerID = revID;
                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionReviewer(" + sessionID + "," + revID + ")");
                return -1;
            }
        }

        public static int UnlockSessionReviewer(long sessionID)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    
                    List<SessionContentItem> scis = (from sci in context.SessionContentItems  
                                                   where sci.SessionID == sessionID
                                                   select sci).ToList<SessionContentItem>();
                    
                    //leave other reviewer work, meshmesh
                    //foreach (SessionContentItem sci in scis)
                    //{
                    //    sci.ReviewerUserID = null;
                    //}

                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            updated_session.ReviewerID = null;
                            
                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UnlockSessionReviewer(" + sessionID + ")");
                return -1;
            }
        }


        public static int UpdateSessionWordAndPdfFiles(long sessionID, byte[] wordDoc, byte[] pdfDoc, bool final = false)
        {
            try
            {
                Session updated_session = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    int res = 0;
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                        if (updated_session != null)
                        {
                            if (!final)
                            {
                                updated_session.SessionMadbatahWord = wordDoc;
                                updated_session.SessionMadbatahPDF = pdfDoc;
                            }
                            else
                            {
                                updated_session.SessionMadbatahWordFinal= wordDoc;
                                updated_session.SessionMadbatahPDFFinal = pdfDoc;
                            }

                        }
                        res = context.SaveChanges();

                    }
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionWordAndPdfFiles(" + sessionID + ")");
                return -1;
            }
        }

        //public static int UpdateSessionSetSessionStartID(long sessionID, long sessionStartID)
        //{
        //    try
        //    {
        //        Session updated_session = null;
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            updated_session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
        //            if (updated_session != null)
        //            {
        //                updated_session.SessionStartID = sessionStartID;
        //            }
        //            int res = context.SaveChanges();
        //            return res;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionStatus(" + sessionID + "," + sessionStartID + ")");
        //        return -1;
        //    }
        //}


        public static int GetSessionsCount(int statusCode, bool checkEquality)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //return context.Sessions.Count();
                    List<Session> scis = null;
                    if (checkEquality)
                        scis = (from sci in context.Sessions
                                where sci.SessionStatusID == statusCode
                                select sci).ToList<Session>();
                    else
                        scis = (from sci in context.Sessions
                                where sci.SessionStatusID != statusCode
                                select sci).ToList<Session>();
                     if (scis != null)
                         return scis.Count;
                     else
                         return 0;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionsCount()");
                return -1;
            }
        }
        //public static int GetFinalApprovedSessionsCount(int statusCode)
        //{
        //    try
        //    {
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            //return context.Sessions.Count();
        //            List<Session> scis = (from sci in context.Sessions
        //                                  where sci.SessionStatusID == statusCode
        //                                  select sci).ToList<Session>();
        //            if (scis != null)
        //                return scis.Count;
        //            else
        //                return 0;

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionsCount()");
        //        return -1;
        //    }
        //}
        public static Hashtable GetSessionStatisticsForReviewer(long sessionID)
        {
            //SessionStart session_start = null;
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Hashtable hash = new Hashtable();
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        int approvedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 1 select item).Count();
                        int rejectedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 2 select item).Count();
                        int fixedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 3 select item).Count();
                        int modifiedAfterApproveCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 4 select item).Count();

                        int approvedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 4 select item).Count();
                        int rejectedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 5 select item).Count();
                        int fixedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 6 select item).Count();
                        int modifiedAfterApproveSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 7 select item).Count();
                     
                        hash[1] = approvedCount + approvedSessionStartCount;
                        hash[2] = rejectedCount + rejectedSessionStartCount;
                        hash[3] = fixedCount + fixedSessionStartCount;
                        hash[4] = modifiedAfterApproveCount + modifiedAfterApproveSessionStartCount;
                        
                    }
                    return hash;
                    //var sessionFile = from sf in context.SessionContentItems
                    //                  where sf.SessionID == sessionID
                    //                  select sf;

                    //from context.SessionContentItems

                    //context.SessionContentItems.All
                    //    (c => c.ID == sessionID && c.SessionContentItemStatu.Name == SessionContentItemStatus);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionStatisticsForReviewer(" + sessionID + ")");
                return null;

            }
        }

        public static Hashtable GetSessionStatisticsForDataEntry(long sessionID, long userID)
        {
            //SessionStart session_start = null;
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Hashtable hash = new Hashtable();
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        int approvedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 1  && item.UserID == userID select item).Count();
                        int rejectedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 2 && item.UserID == userID select item).Count();
                        int fixedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 3 && item.UserID == userID select item).Count();
                        int modifiedAfterApproveCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 4 && item.UserID == userID select item).Count();


                        int approvedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 4 && item.UserID == userID select item).Count();
                        int rejectedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 5 && item.UserID == userID select item).Count();
                        int fixedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 6 && item.UserID == userID select item).Count();
                        int modifiedAfterApproveSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 7 && item.UserID == userID select item).Count();

                        hash[1] = approvedCount + approvedSessionStartCount;
                        hash[2] = rejectedCount + rejectedSessionStartCount;
                        hash[3] = fixedCount + fixedSessionStartCount;
                        hash[4] = modifiedAfterApproveCount + modifiedAfterApproveSessionStartCount;
                    }
                    return hash;
                    //var sessionFile = from sf in context.SessionContentItems
                    //                  where sf.SessionID == sessionID
                    //                  select sf;

                    //from context.SessionContentItems

                    //context.SessionContentItems.All
                    //    (c => c.ID == sessionID && c.SessionContentItemStatu.Name == SessionContentItemStatus);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionStatisticsForDataEntry(" + sessionID + ", " + userID + ")");
                return null;

            }
        }



        #endregion
        
        //#region SessionStatus
        //    public static SessionStatus AddNewSessionStatus(string session_status_name)
        //    {
        //        Random rand = new Random(1);
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            SessionStatus session_status_item = new SessionStatus
        //            {
        //                ID = 2,
        //                Name = session_status_name
        //            };
        //            context.AddObject("SessionStatus", session_status_item);
        //            int result = context.SaveChanges();
        //            //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, session_status_item);
        //            return session_status_item;
        //        }
        //    }

        //    public static void RemoveSessionStatus(long session_status_id)
        //    {
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            SessionStatus session_status = context.SessionStatus.FirstOrDefault(c => c.ID == session_status_id);
        //            context.DeleteObject(session_status);
        //            int result = context.SaveChanges();
        //        }
        //    }
        //    public static void RemoveSessionStatus(string session_status_name)
        //    {
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            SessionStatus session_status = context.SessionStatus.FirstOrDefault(c => c.Name == session_status_name);
        //            context.DeleteObject(session_status);
        //            int result = context.SaveChanges();
        //        }
        //    }

        //    public static SessionStatus UpdateSessionStatus(long session_status_id, string session_status_name)
        //    {
        //        SessionStatus updated_session_status = null;
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            updated_session_status = context.SessionStatus.FirstOrDefault(c => c.ID == session_status_id);
        //            if (updated_session_status != null)
        //            {
        //                //update the Role attributes
        //                updated_session_status.Name = session_status_name;
        //            }
        //            context.SaveChanges();
        //        }
        //        return updated_session_status;
        //    }

        //    public static SessionStatus GetSessionStatusById(long session_status_id)
        //    {
        //        SessionStatus session_status = null;
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            session_status = context.SessionStatus.FirstOrDefault(c => c.ID == session_status_id);
        //        }
        //        return session_status;
        //    }

        //    public static SessionStatus GetSessionStatusByName(string session_status_name)
        //    {
        //        SessionStatus session_status = null;
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            session_status = context.SessionStatus.FirstOrDefault(c => c.Name == session_status_name);
        //        }
        //        return session_status;
        //    }
        //    public static SessionStatus GetSessionStatusBySessionID(long sessionID)
        //    {
        //        SessionStatus result = null;
        //        try
        //        {
        //            using (EMadbatahEntities context = new EMadbatahEntities())
        //            {
        //                long session_status_id = (from obj in context.Sessions
        //                                          where obj.ID == sessionID
        //                                          select obj.SessionStatusID).FirstOrDefault();
        //                result = (from obj in context.SessionStatus
        //                          where obj.ID == session_status_id
        //                          select obj).FirstOrDefault();
        //                return result;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            return result;
        //        }
        //    }
        //    #endregion
        //#region SessionContentItemStatus
        //#endregion
        //public static void NewSessionAgendaFromEP(long sessionID, List<SessionAgendaItem> agendaItems)
        //{ }


        //public static void NewSessionSetAttendanceFromEP(long sessionID, List<SessionAttendant> attendances) 
        //{
        //    using (EMadbatahEntities context = new EMadbatahEntities())
        //    {
        //        foreach (SessionAttendant attendant in attendances)
        //        {
        //        }
        //    }
        //}

        //public static void AddCustomAgendaItem(long sessionContentID, long sessionFileID, int fragmentOrder)
        //{
 
        //}



        public static Hashtable GetSessionStatisticsForFileReviewer(long sessionID, long fileRevID)
        {
            //SessionStart session_start = null;
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Hashtable hash = new Hashtable();
                    if (context.Sessions.Count<Session>() > 0)
                    {
                        int approvedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 1 && item.FileReviewerID == fileRevID select item).Count();
                        int rejectedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 2 && item.FileReviewerID == fileRevID select item).Count();
                        int fixedCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 3 && item.FileReviewerID == fileRevID select item).Count();
                        int modifiedAfterApproveCount = (from item in context.SessionContentItems where item.SessionID == sessionID && item.SessionContentItemStatus.ID == 4 && item.FileReviewerID == fileRevID select item).Count();


                        int approvedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 4 && item.FileReviewerID == fileRevID select item).Count();
                        int rejectedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 5 && item.FileReviewerID == fileRevID select item).Count();
                        int fixedSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 6 && item.FileReviewerID == fileRevID select item).Count();
                        int modifiedAfterApproveSessionStartCount = (from item in context.SessionFiles where item.SessionID == sessionID && item.IsSessionStart == true && item.Status == 7 && item.FileReviewerID == fileRevID select item).Count();

                        hash[1] = approvedCount + approvedSessionStartCount;
                        hash[2] = rejectedCount + rejectedSessionStartCount;
                        hash[3] = fixedCount + fixedSessionStartCount;
                        hash[4] = modifiedAfterApproveCount + modifiedAfterApproveSessionStartCount;
                    }
                    return hash;
                    //var sessionFile = from sf in context.SessionContentItems
                    //                  where sf.SessionID == sessionID
                    //                  select sf;

                    //from context.SessionContentItems

                    //context.SessionContentItems.All
                    //    (c => c.ID == sessionID && c.SessionContentItemStatu.Name == SessionContentItemStatus);
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.GetSessionStatisticsForDataEntry(" + sessionID + ", " + fileRevID + ")");
                return null;

            }
        }
    }
}
