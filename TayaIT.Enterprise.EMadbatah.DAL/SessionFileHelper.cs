using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class SessionFileHelper
    {
        public static bool AddNewSessionFile(int sessionID, string name, long durationSec, int order)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile file = new SessionFile
                    {
                        SessionID = sessionID,
                        Name = name,
                        DurationSecs = durationSec,
                        Order = order,
                        
                    };
                    context.SessionFiles.AddObject(file);
                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.AddNewSessionFile(" + sessionID + "," + name + ")");
                return false;
            }
        }

        public static bool AddNewSessionFiles(List<SessionFile> newSessionFiles)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    foreach (SessionFile sf in newSessionFiles)
                    {
                        //context.SessionFiles.AddObject(sf);
                        if (context.SessionFiles.SingleOrDefault<SessionFile>(f => f.Name == sf.Name) == null)
                        {
                            context.SessionFiles.AddObject(new SessionFile()
                            {
                                DurationSecs = sf.DurationSecs,
                                Name = sf.Name,
                                Order = sf.Order,
                                SessionID = sf.SessionID,
                                Status = sf.Status,
                            });
                        }
                    }

                    context.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.AddNewSessionFiles()");
                return false;
            }
        }

        public static int GetSessionFilesCount(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var files = from f in context.SessionFiles
                                where f.SessionID == sessionID
                                select f;

                    
                    return files.Count<SessionFile>();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.AddNewSessionFiles()");
                return -1;
            }
        }

        public static bool LockSessionFile(long sessionFileID, long userID, bool isAdmin)
        {
            try
            {
                //User user = null;
                SessionFile file = null;

                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //user = context.Users.FirstOrDefault(u => u.ID == userID);
                    file = context.SessionFiles.FirstOrDefault(f => f.ID == sessionFileID);

                    //if (user.SessionFiles.Contains(file))
                    //    return true;
                    if (file.UserID != null && file.UserID != userID && !isAdmin)
                        return false;

                    file.UserID = userID;
                    foreach (SessionContentItem item in file.SessionContentItems)
                    {
                        item.UserID = userID;
                    }
                    context.SaveChanges();
                }
                return true;   
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.AssignLockSessionFile(" + sessionFileID + "," + userID + ")");
                return false;
            }           
        }



        public static bool LockSessionFileReviewer(long sessionFileID, long revID, bool isAdmin)
        {
            try
            {
                //User user = null;
                SessionFile file = null;

                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //user = context.Users.FirstOrDefault(u => u.ID == userID);
                    file = context.SessionFiles.FirstOrDefault(f => f.ID == sessionFileID);

                    //if (user.SessionFiles.Contains(file))
                    //    return true;
                    if (file.FileReviewerID != null && file.FileReviewerID != revID && !isAdmin)
                        return false;

                    file.FileReviewerID = revID;
                    foreach (SessionContentItem item in file.SessionContentItems)
                    {
                        item.FileReviewerID = revID;
                    }
                    context.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.LockSessionFileReviewer(" + sessionFileID + "," + revID + ")");
                return false;
            }
        }




        public static bool UnlockSessionFile(long sessionFileID)
        {
            try
            {
                SessionFile file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    file = context.SessionFiles.FirstOrDefault(f => f.ID == sessionFileID);
                    file.UserID = null;
                    foreach (SessionContentItem item in file.SessionContentItems)
                    {
                        item.UserID = null;
                    }
                    context.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UnlockSessionFile(" + sessionFileID + ")");
                return false;
            }
        }


        public static bool IsSessionFileLockedForFileReviewer(long sessionContentItemID)
        {
            try
            {
                SessionFile file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionContentItem scitem = context.SessionContentItems.FirstOrDefault<SessionContentItem>(sci => sci.ID == sessionContentItemID);
                    return (scitem.SessionFile.FileReviewerID != null);

                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.IsSessionFileLockedForFileReviewer(" + sessionContentItemID + ")");
                return false;
            }
        }


        public static bool UnloackSessionFileReviewer(long sessionFileID)
        {
            try
            {
                SessionFile file = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    file = context.SessionFiles.FirstOrDefault(f => f.ID == sessionFileID);
                    file.UserID = null;
                    foreach (SessionContentItem item in file.SessionContentItems)
                    {
                        item.FileReviewerID = null;
                    }
                    context.SaveChanges();
                    return true;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UnloackSessionFileReviewer(" + sessionFileID + ")");
                return false;
            }
        }






        //public bool ReAssignSessionFile(long sessionFileID, long newUserID)
        //{
        //    try
        //    {
        //        return AssignLockSessionFile(sessionFileID, newUserID);
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.ReAssignSessionFile(" + sessionFileID + "," + newUserID +")");
        //        return false;
        //    }
        //}

        

        public static SessionFile GetSessionFileByID(long sessionFileID)
        {
            try
            {
                SessionFile sessionFile = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    sessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    sessionFile.User = sessionFile.User;
                    sessionFile.FileReviewer = sessionFile.FileReviewer;
                }
                return sessionFile;

                //using (EMadbatahEntities context = new EMadbatahEntities())
                //{
                //    var sessionFile = from sf in context.SessionFiles
                //                  where sf.ID == sessionFileID
                //                  select sf;

                //    return sessionFile.FirstOrDefault<SessionFile>();                   
                //}
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.GetSessionFileBySessionFileID(" + sessionFileID + ")");
                return null;
            }
        }

        public static List<SessionFile> GetSessionFilesBySessionID(long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var results = from sf in context.SessionFiles
                                  where sf.SessionID == sessionID
                                  select sf;

                    return results.ToList<SessionFile>();
                    // return context.SessionFiles.Where<SessionFile>(;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.GetSessionFilesBySessionID(" + sessionID + ")");
                return null;
            }
        }
        public static SessionFile GetSessionFileByOrder(int sessionFileOrder)
        {
            try
            {
                SessionFile sessionFile = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    sessionFile = context.SessionFiles.FirstOrDefault(c => c.Order == sessionFileOrder);
                }
                return sessionFile;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.GetSessionFileByOrder(" + sessionFileOrder + ")");
                return null;
            }
        }

        public static bool UpdateSessionFileOrder(long sessionFileID1, int newOrder1, long sessionFileID2, int newOrder2)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile updatedSessionFile1 = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID1);
                    SessionFile updatedSessionFile2 = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID2);

                    if (updatedSessionFile1 != null && updatedSessionFile2 != null)
                    {
                        updatedSessionFile1.Order = newOrder1;
                        updatedSessionFile2.Order = newOrder2;
                        int res = context.SaveChanges();
                        return true;
                    }
                    else
                        return false;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.ChangeSessionFileOrder(" + sessionFileID1 + "," + newOrder1 + "," + sessionFileID2 + "," + newOrder2 + ")");
                return false; ;
            }
        }
        public static int UpdateSessionFileFragOrderAndModifiedDate(long sessionFileID, int FragOrder , DateTime ModifiedDate)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile updatedSessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    if (updatedSessionFile != null)
                    {
                        updatedSessionFile.LastInsertedFragNumInXml = FragOrder;
                        updatedSessionFile.LastModefied = ModifiedDate;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.ChangeSessionFileOrder(" + sessionFileID + "," + FragOrder + ")");
                return -1; ;
            }
        }
        public static int UpdateSessionFileStatus(long sessionFileID, int statusId, long? revID ,string note=null)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile updatedSessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    if (updatedSessionFile != null)
                    {
                        updatedSessionFile.Status = statusId;
                        updatedSessionFile.SessionStartReviewNote = note;
                        if(revID != null)
                            updatedSessionFile.SessionStartReviewerID = revID;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(" + sessionFileID + "," + statusId + ")");
                return -1; ;
            }
        }
        public static int UpdateSessionFileStartText(long sessionFileID, string updatedSessionText, string note = null)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile updatedSessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    if (updatedSessionFile != null)
                    {
                        updatedSessionFile.SessionStartReviewNote = note;
                        updatedSessionFile.SessionStartText = updatedSessionText;
                        updatedSessionFile.LastModefied = DateTime.Now;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStartText(" + sessionFileID + ")");
                return -1; ;
            }
        }


        public static int UpdateSessionFileModifiedDate(long sessionFileID, DateTime ModifiedDate)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionFile updatedSessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    if (updatedSessionFile != null)
                    {
                        updatedSessionFile.LastModefied= ModifiedDate;
                        int res = context.SaveChanges();
                        return res;
                    }
                    else
                        return -1;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(" + sessionFileID + "," + ModifiedDate.ToShortDateString() + ")");
                return -1; ;
            }
        }

        //public void AddSessionFile() { }//from code generator
    }
}
