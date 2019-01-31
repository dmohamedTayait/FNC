using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Web.Security;
using System.IO;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model.VecSys;
using System.Collections;
using System.Data.Objects.DataClasses;
using System.Security.Principal;
using System.Web;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Threading;
using TayaIT.Enterprise.EMadbatah.OpenXml.Word;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public static class EMadbatahFacade
    {
        public static bool LockSessionFile(long sessionFileID)
        {
            //SessionFileHelper
            return true;
        }

        public static string GetSessionName(long season, long stage, long serial)
        {
            //return new StringBuilder("[ ف ").Append(season).Append("/").Append("د ").Append(stage).Append("/").Append(serial).Append(" ]").ToString();
            StringBuilder sb=  new StringBuilder("");
            String stageAlfa="";
            switch (stage)
            {
                case 1:
                    stageAlfa = "أ";
                    break;
                case 2:
                    stageAlfa = "ب";
                    break;
                case 3:
                    stageAlfa = "ج";
                    break;
                case 4:
                    stageAlfa = "د";
                    break;
                case 5:
                    stageAlfa = "هـ";
                    break;
                default:
                    stageAlfa = "هـ";
                    break;
            }
            sb.Append(serial).Append(" / ").Append(" ف").Append(season).Append(" / ").Append(stageAlfa);
            return "[ " + sb.ToString() + " ]";
        }


        public static Model.MadbatahFilesStatus GetSessionMadbatahFilesStatus(long sessionID)
        {
            int statusID = DAL.SessionHelper.GetSessionMadabathFilesStatus(sessionID);
            if (statusID > 0)
            {

                Model.MadbatahFilesStatus status = (Model.MadbatahFilesStatus)statusID;
                if (status == Model.MadbatahFilesStatus.DraftCreated && !SessionHelper.IsSessionWordFileAndPDFCreated(sessionID))
                {
                    SessionHelper.UpdateSessionMadabathFilesStatus(sessionID, (int)Model.MadbatahFilesStatus.DraftFail);
                    return Model.MadbatahFilesStatus.DraftFail;
                }
                else if (status == Model.MadbatahFilesStatus.FinalCreated && !SessionHelper.IsSessionFinalWordFileAndPDFCreated(sessionID))
                {
                    SessionHelper.UpdateSessionMadabathFilesStatus(sessionID, (int)Model.MadbatahFilesStatus.FinalFail);
                    return Model.MadbatahFilesStatus.FinalFail;
                }
                else
                    return status;

            }
            else
            {
                if (SessionHelper.IsSessionFinalWordFileAndPDFCreated(sessionID))
                    return Model.MadbatahFilesStatus.FinalCreated;
                else if (SessionHelper.IsSessionWordFileAndPDFCreated(sessionID))
                    return Model.MadbatahFilesStatus.DraftCreated;
                else
                    return Model.MadbatahFilesStatus.NotCreated;

            }
        }

        public static SessionDetails GetSessionBySessionID(long sessionID)
        {
            Session session = SessionHelper.GetSessionByID(sessionID);
            if (session != null)
            {
                SessionDetails sd = GetSessionDetailsFromSessionObj(session);
                return sd;
            }
            else
                return null;
           // return 
             
        }

        public static int GetUsersCount()
        {
            return UserHelper.GetAllUsersCount();
        }

        public static int UpdateSessionMP3Path(long sessionID, string mp3Path)
        {
            return SessionHelper.UpdateSessionMP3FilePath(sessionID, mp3Path);
        }

        public static int UpdateSessionStatus(long sessionID, Model.SessionStatus newStatus)
        {
            return SessionHelper.UpdateSessionStatus(sessionID, (int)newStatus);
        }

        public static SessionDetails GetSessionDetailsBySessionID(long sessionID)
        {
            Session session = SessionHelper.GetSessionDetailsByID(sessionID);
            if (session != null)
            {
                SessionDetails sd = GetSessionDetailsFromSessionObj(session);
                return sd;
            }
            else
                return null;
            // return 

        }
        public static SessionDetails GetSessionByEParlimentID(long eParlimentID)
        {
            Session session = SessionHelper.GetSessionByEParlimentID(eParlimentID);
            if (session != null)
            {
                SessionDetails sd = GetSessionDetailsFromSessionObj(session);
                return sd;
            }
            else
                return null;
            // return 

        }

        /*
        public static void ReorderSessionFiles(int firstOrder, int secondOrder)
        {
            SessionFile firstSessionFile = SessionFileHelper.GetSessionFileByOrder(firstOrder);
            SessionFile secondSessionFile = SessionFileHelper.GetSessionFileByOrder(secondOrder);
            //swap orders
            if (firstSessionFile != null)
            {
                SessionFileHelper.UpdateSessionFileOrder(firstSessionFile.ID, secondOrder);
            }
            if (secondSessionFile != null)
            {
                SessionFileHelper.UpdateSessionFileOrder(secondSessionFile.ID, firstOrder);
            }
        }
        */
        public static EMadbatahUser GetUserByDomainUserName(string userDomainUserName)
        {
            User user = UserHelper.GetUserByDomainName(userDomainUserName);
            return DALUserEMadbatahUser(user);
        }

        public static EMadbatahUser GetUserByUserID(long userId)
        {
            User user = UserHelper.GetUserByID(userId);
            return DALUserEMadbatahUser(user);
        }

        public static List<EMadbatahUser> GetUsersFromDB(bool activeUsersOnly)
        {
            List<EMadbatahUser> usersToReturn = new List<EMadbatahUser>();
            List<User> users = null;
            if (activeUsersOnly)
                users = UserHelper.GetACtiveUsers();
            else
                users = UserHelper.GetAllUsers();

            foreach (User user in users)
            {
                usersToReturn.Add(DALUserEMadbatahUser(user));
            }

            return usersToReturn;
        }

        public static List<EMadbatahUser> GetRevUsersFromDB()
        {
            List<EMadbatahUser> usersToReturn = new List<EMadbatahUser>();
            List<User> users = UserHelper.GetAllUsers();

            foreach (User user in users)
            {
                if(user.RoleID != (int)UserRole.DataEntry)
                    usersToReturn.Add(DALUserEMadbatahUser(user));
            }

            return usersToReturn;
        }

        public static List<EMadbatahUser> GetUsersFromAD(EMadbatahUser currentUser)
        {
            List<EMadbatahUser> usersToReturn = new List<EMadbatahUser>();

            //MembershipUserCollection users = null;
            //users = Membership.GetAllUsers();

            WindowsIdentity id = (WindowsIdentity)HttpContext.Current.User.Identity;


            WinActiveDirectory ad = new WinActiveDirectory(ConfigManager.GetConnectionString("ADConnectionString"), currentUser.DomainUserName.Split('\\')[1].ToLower());
            List<WinActiveDirectory.LDAPUser> users = ad.GetAllUsers();



            if (id != null && users != null && users.Count > 0)
            {
                string domainName = id.Name.Split('\\')[0].ToLower();
                foreach (WinActiveDirectory.LDAPUser user in users)
                {
                    string name = "";

                    if(!string.IsNullOrEmpty(user.Description))
                        name = user.Description;
                    else if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(user.DisplayName))
                        name = user.DisplayName;
                    else if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(user.UserName))
                        name = user.UserName;
                    else if (string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(user.AccountName))
                        name = user.AccountName;

                    usersToReturn.Add(new EMadbatahUser(0, UserRole.DataEntry,name, domainName + "\\" + user.AccountName, user.Email, true));
                }
            }


            return usersToReturn;
        }

        public static int ActivateUser(long userId)
        {
            return UserHelper.UpdateUserStatus(userId, true);
        }

        public static int DeActivateUser(long userId)
        {
            return UserHelper.UpdateUserStatus(userId, true);
        }




        private static EMadbatahUser DALUserEMadbatahUser(User user)
        {
            //Enum.IsDefined(typeof(Foo), i)
            if (user == null)
                return null;
            else
                return new EMadbatahUser(user.ID, (UserRole)user.RoleID, user.FName , user.DomainUserName, user.Email, user.Status);

        }




        public static int RemoveUser(long userId)
        {
            return UserHelper.DeleteUserById(userId);
        }

        public static bool UpdateUser(EMadbatahUser userDetails)
        {
            long userRoleID = (long)userDetails.Role;
            if (UserHelper.UpdateUser(userDetails.ID, userDetails.Name, userDetails.DomainUserName, userDetails.Email, userDetails.IsActive, userRoleID) != -1)
                return true;
            else
                return false;
        }

        public static long AddNewUser(EMadbatahUser userDetails)
        {

            return UserHelper.AddNewUser(userDetails.Name, (long)userDetails.Role, userDetails.DomainUserName, userDetails.Email, userDetails.IsActive);
            
        }

        public static void ReorderAttachements(int firstOrder, int secondOrder)
        {
            Attachement firstAttachement = AttachmentHelper.GetAttachementByOrder(firstOrder);
            Attachement secondAttachement = AttachmentHelper.GetAttachementByOrder(secondOrder);
            //swap orders
            if (firstAttachement != null)
            {
                AttachmentHelper.UpdateAttachmentOrder(firstAttachement.ID, secondOrder);
            }
            if (secondAttachement != null)
            {
                AttachmentHelper.UpdateAttachmentOrder(secondAttachement.ID, firstOrder);
            }
        }

        public static List<SessionDetails> GetSessionDetailsFromDB(int page_no, int count)
        {

            List<SessionDetails> sessionDetailsList = new List<SessionDetails>();                        
            List<string> reviewerNotes = new List<string>();//???
            
            List<Session> allSessions = SessionHelper.GetSessions(page_no, count);
            
            //fill the sessionDetalisList
            foreach (Session session in allSessions)
            {
                List<Attachement> attachments =  session.Attachements.ToList<Attachement>();
                List<SessionAttachment> sessionAttachments =  new List<SessionAttachment>();

                List<SessionFile> sessionFiles = session.SessionFiles.ToList<SessionFile>();
                List<SessionAudioFile> sessionAudioFiles = new List<SessionAudioFile>();

                foreach (Attachement att in attachments)
	            {
		             sessionAttachments.Add(new SessionAttachment(att.ID, att.Name, (FileExtensionType)Enum.Parse(typeof(FileExtensionType), att.FileType),att.Order, att.SessionID, att.FileContent));
	            }

                foreach (SessionFile sf in sessionFiles)
	            {
                    sessionAudioFiles.Add(new SessionAudioFile(sf.ID, (long)sf.UserID, sf.SessionID, sf.Name, sf.LastInsertedFragNumInXml, sf.Order, sf.DurationSecs, (Model.SessionFileStatus)sf.Status, sf.User.FName, (DateTime)sf.LastModefied, sf.User == null ? null : sf.User.Email, sf.IsSessionStart, sf.SessionStartText, sf.FileReviewerID, (sf.FileReviewer != null) ? sf.FileReviewer.FName : null, (sf.FileReviewer != null) ? sf.FileReviewer.Email : null));
	            }

                SessionDetails detail = GetSessionDetailsFromSessionObj(session);

                //add new SessionDetails instance
                sessionDetailsList.Add(detail);

            }
            return sessionDetailsList;
        }

    //    public static SendSessionAudioFilesToDB(List<SessionAudioFile> files)
    //    {
    //        List<SessionFile> sessionFiles = new List<SessionFile>();
    //        foreach (SessionAudioFile file in files)
    //{
    //     sessionFiles.Add(new SessionFile()
    //     {
    //         DurationSecs = file.DurationInSec,
    //            Name = file.Name,
    //            Order = file.Order,
    //            SessionID = file.SessionID,
    //           Status = (int)file.Status                
    //     }

    //     if(files
    //}
    //    }

        public static List<SessionDetails> GetSessions(int pageNo, int itemsPerPage)
        {
            List<SessionDetails> sessionsToRet = new List<SessionDetails>();
            List<Session> sessions = SessionHelper.GetSessions(pageNo, itemsPerPage, (int)Model.SessionStatus.FinalApproved, false);
            if (sessions != null)
            {
                foreach (Session session in sessions)
                {
                    sessionsToRet.Add(GetSessionDetailsFromSessionObj(session));
                }
            }

            return sessionsToRet;
        }
        public static List<SessionDetails> GetFinalApprovedSessions(int pageNo, int itemsPerPage)
        {
            List<SessionDetails> sessionsToRet = new List<SessionDetails>();
            List<Session> sessions = SessionHelper.GetSessions(pageNo, itemsPerPage, (int)Model.SessionStatus.FinalApproved, true);
            if (sessions != null)
            {
                foreach (Session session in sessions)
                {
                    sessionsToRet.Add(GetSessionDetailsFromSessionObj(session));
                }
            }

            return sessionsToRet;
        }

        public static int GetSessionsCount()
        {
            return SessionHelper.GetSessionsCount((int)Model.SessionStatus.FinalApproved, false);
        }
        public static int GetFinalApprovedSessionsCount()
        {
            return SessionHelper.GetSessionsCount((int)Model.SessionStatus.FinalApproved, true);
        }

        public static SessionDetails AddNewSessionDetailsToDB(SessionDetails sd)
        {
            Session session = new Session();

            var attendants = new EntityCollection<Attendant>();
            foreach (var att in sd.Attendance)
            {
                Attendant newAttendant = new Attendant();
                newAttendant.State = (int)att.State;
                newAttendant.Type = (int)att.Type;
                newAttendant.EparlimentID = att.EparlimentID;
                newAttendant.JobTitle = att.JobTitle;
                newAttendant.Name = att.Name;
                newAttendant.FirstName = att.FirstName;
                newAttendant.SecondName = att.SecondName;
                newAttendant.TribeName = att.TribeName;
                attendants.Add(newAttendant);
            }

                Attendant unknownAttendant = new Attendant();
                unknownAttendant.State = (int)Model.AttendantState.Attended;
                unknownAttendant.Type = (int)Model.AttendantType.UnKnown;
                unknownAttendant.EparlimentID = -1;
                unknownAttendant.JobTitle = "";
                unknownAttendant.Name = "غير معرف";
                attendants.Add(unknownAttendant);
            
            AgendaItem sessionIntroItem = new AgendaItem();
            sessionIntroItem.IsCustom = true;
            sessionIntroItem.Name = "افتتاحية الجلسة ، وكلمة معالي رئيس المجلس ";
            sessionIntroItem.EParliamentID = null;
            sessionIntroItem.Order = 0;
            session.AgendaItems.Add(sessionIntroItem);



            //var agendaItems = new EntityCollection<AgendaItem>();
            foreach (SessionAgendaItem item in sd.AgendaItems.Values)
            {
                AgendaItem item2Send = new AgendaItem();
                item2Send.IsCustom = false;
                item2Send.Name = item.Text;
                item2Send.EParliamentID = item.EparlimentID;
                if (item.EparlimentParentID < 0)
                    item2Send.EParliamentParentID = null;
                else
                    item2Send.EParliamentParentID = item.EparlimentParentID;
                item2Send.Order = item.Order;

                if (item.SubAgendaItems.Count > 0)
                {
                    foreach (SessionAgendaItem subItem in item.SubAgendaItems)
                    {
                        AgendaSubItem subitem2Send = new AgendaSubItem();
                        subitem2Send.Name = subItem.Text;
                        subitem2Send.EParliamentID = subItem.EparlimentID;
                        subitem2Send.EParliamentParentID = subItem.EparlimentParentID;
                        subitem2Send.Order = subItem.Order;
                        subitem2Send.QFrom = subItem.QuestionFrom;
                        subitem2Send.QTo = subItem.QuestionTo;
                        item2Send.AgendaSubItems.Add(subitem2Send);
                    }                    
                }

                session.AgendaItems.Add(item2Send);
            }

            AgendaItem sessionUnknownItem = new AgendaItem();
            sessionUnknownItem.IsCustom = true;
            sessionUnknownItem.Name = "غير معرف";
            sessionUnknownItem.EParliamentID = null;
            sessionUnknownItem.Order = null;
            session.AgendaItems.Add(sessionUnknownItem);

            
            
            //var agendaItems = new EntityCollection<AgendaItem>();





            ////koko
            //foreach (long key in sd.AgendaItems.Keys)
            //{
            //    var childs = new EntityCollection<AgendaItem>();
            //    SessionAgendaItem sai = (SessionAgendaItem)sd.AgendaItems[key];
            //    AgendaItem agendaItem = new AgendaItem();
            //    agendaItem.Name = sai.Text;
            //    agendaItem.IsCustom = false;
            //    agendaItem.EParliamentID = sai.EparlimentID;

            //    //agendaItem.EParliamentParentID = sai.
            //    if (sai.SubAgendaItems.Count > 0)
            //    {
            //        foreach (SessionAgendaItem item in sai.SubAgendaItems)
            //        {
            //            AgendaItem agendaItemSub = new AgendaItem();
            //            agendaItemSub.Name = sai.Text;
            //            agendaItemSub.IsCustom = false;
            //            agendaItemSub.EParliamentID = sai.EparlimentID;
            //            agendaItemSub.EParliamentParentID = agendaItem.EParliamentID;
            //            childs.Add(agendaItemSub);
            //        }
            //        agendaItem.AgendaSubItems = childs;
            //    }
            //}

            //session.AgendaItems = 
            session.Attendants = attendants;
            session.EParliamentID = sd.EparlimentID;




            session.Place = sd.Place;
            session.President = sd.Presidnt;
            session.Season = sd.Season;
            session.Serial = sd.Serial;
            session.SessionStatusID = (int)Model.SessionStatus.New;
            session.Stage = sd.Stage;
            session.StageType = sd.StageType;
            session.StartTime = sd.StartTime;
            session.EndTime = sd.EndTime;
            session.Date = sd.Date;
            session.Type = sd.Type;
            session.Subject = sd.Subject;
            
            //session.DateHijri = sd.DateHijri;

            //session.State = sd.SessionStatus
            //session.Type = sd.se
            
            //session.Attachements = EntityCollection sd.Attachments;
            
            //add Default AgendaItem MadbatahIntro

            long newSessionID = SessionHelper.AddNewSession(session, Localization.LocalHelper.GetLocalizedString("strMadbatahStartAgendaItem"));
            if (newSessionID != -1)
            {
                sd.SessionID = newSessionID;
            }

            return sd;

        }


        //new to update session data from EParliment (Attendance, and Agenda Items)
        public static void UpdateSessionDetailsToDB(SessionDetails sd)
        {
            Session session = SessionHelper.GetSessionDetailsByEParlimentID(sd.EparlimentID);

            

            var attendants = new EntityCollection<Attendant>();
            foreach (var att in sd.Attendance)
            {
                var existingAtt = session.Attendants.FirstOrDefault(s => s.EparlimentID == att.EparlimentID);
                //if existingAtt == null this is a new attendant
                //if existingAtt has value then this val should be updated

                if (existingAtt == null)
                {
                    //Attendant newAttendant = new Attendant();
                    //newAttendant.State = (int)att.State;
                    //newAttendant.Type = (int)att.Type;
                    //newAttendant.EparlimentID = att.EparlimentID;
                    //newAttendant.JobTitle = att.JobTitle;
                    //newAttendant.Name = att.Name;
                    //attendants.Add(newAttendant);
                    AttendantHelper.AddNewAttendant(att.Name, att.JobTitle, att.EparlimentID, (int)att.Type, (int)att.State, session.ID, att.FirstName, att.SecondName , att.TribeName);
                }
                else
                {
                    //existingAtt.JobTitle = att.JobTitle;
                    //AttendantHelper.UpdateAttendant(existingAtt.ID, att.Name, existingAtt.JobTitle, (int)att.Type, (int)att.State);                    
                }
               

            }

            //delete not required
            //foreach (var att in session.Attendants)
            //{
            //    var existingAtt = sd.Attendance.FirstOrDefault(s => s.EparlimentID == att.EparlimentID);
            //    //if null delete
            //    if (existingAtt == null)
            //    {
            //        //ignore
            //        AttendantHelper.DeleteAttendantByEPID(existingAtt.EparlimentID);
            //    }
            //}

            


            foreach (SessionAgendaItem item in sd.AgendaItems.Values)
            {
                var existingItem = session.AgendaItems.FirstOrDefault(s => s.EParliamentID == item.EparlimentID);
                //if existingAtt == null this is a new item
                //if existingAtt has value then this val should be updated

                if (existingItem == null)
                {
                    AgendaItem item2Send = new AgendaItem();
                    item2Send.IsCustom = false;
                    item2Send.Name = item.Text;
                    item2Send.EParliamentID = item.EparlimentID;
                    if (item.EparlimentParentID < 0)
                        item2Send.EParliamentParentID = null;
                    else
                        item2Send.EParliamentParentID = item.EparlimentParentID;
                    item2Send.Order = item.Order;

                    if (item.SubAgendaItems.Count > 0)
                    {
                        foreach (SessionAgendaItem subItem in item.SubAgendaItems)
                        {
                            AgendaSubItem subitem2Send = new AgendaSubItem();
                            subitem2Send.Name = subItem.Text;
                            subitem2Send.EParliamentID = subItem.EparlimentID;
                            subitem2Send.EParliamentParentID = subItem.EparlimentParentID;
                            subitem2Send.Order = subItem.Order;
                            subitem2Send.QFrom = subItem.QuestionFrom;
                            subitem2Send.QTo = subItem.QuestionTo;
                            item2Send.AgendaSubItems.Add(subitem2Send);
                        }
                    }
                    
                    //session.AgendaItems.Add(item2Send);
                    AgendaHelper.AddAgendaItem(item2Send, session.ID);
                }
                else
                {
                    //this item already exist (needs update ??)

                    if(item.SubAgendaItems != null && item.SubAgendaItems.Count > 0)
                    {
                         foreach (SessionAgendaItem subItem in item.SubAgendaItems)
                         {
                            var existingsubItem = existingItem.AgendaSubItems.FirstOrDefault(s => s.EParliamentID == subItem.EparlimentID);
                            if(existingsubItem == null)
                            {
                                //this item is not in our database, and should be added
                                AgendaHelper.AddAgendaSubItem(subItem.Text, existingItem.ID, subItem.EparlimentID, subItem.EparlimentParentID);
                            }
                         }
                        
                    }
                }
                
            }





            foreach (var item in session.AgendaItems)
            {
                if(item.EParliamentID == null || item.Name == "غير معرف")
                    continue;
                bool isFound = false;
                SessionAgendaItem foundAgendaItem = null;
                foreach (Model.SessionAgendaItem agendaItem in sd.AgendaItems.Values)
                {

                     if(agendaItem.EparlimentID == item.EParliamentID)
                    {
                        //this is item
                        isFound = true;
                        foundAgendaItem = agendaItem;
                        break;
                    }
                     
                }


                //var res = from s in sd.AgendaItems.Values where s.
                //var existingItem = from select [item.EParliamentID];
                //if null delete
                if (!isFound)// existingItem == null)
                {
                    AgendaHelper.DeleteAgendaItemByEparliamentID((long)item.EParliamentID, session.ID);
                }
                //for checking the subitems
                else
                {
                    foreach (AgendaSubItem subitem in item.AgendaSubItems)
                    {
                        bool isSubFound = false;

                        foreach (SessionAgendaItem agendaSubItem in foundAgendaItem.SubAgendaItems)
                        {

                            if (agendaSubItem.EparlimentID == subitem.EParliamentID)
                            {
                                //this is item
                                isSubFound = true;
                                break;
                            }
                        }
                        if (!isSubFound)// existingItem == null)
                        {
                            AgendaHelper.DeleteSubAgendaItemByEparliamentID((long)subitem.EParliamentID, (long)subitem.EParliamentParentID, session.ID);
                        }
                    }
                }

            }


            


            



           

        }


        public static Hashtable GetSessionStatistics(EMadbatahUser currentUser, long sessionID)
        {
            Hashtable tblRevStats = null;
            switch (currentUser.Role)
            {
                case UserRole.Admin:
                case UserRole.Reviewer:               
                    //now the dataentry-reviewr role is using filereviewer privilages instead od session reviewer
                    tblRevStats = SessionHelper.GetSessionStatisticsForReviewer(sessionID);
                    //tblRevStats = SessionHelper.GetSessionStatisticsForFileReviewer(sessionID, currentUser.ID);
                    break;
                case UserRole.ReviewrDataEntry:
                case UserRole.FileReviewer:
                    tblRevStats = SessionHelper.GetSessionStatisticsForFileReviewer(sessionID, currentUser.ID);
                    break;
                case UserRole.DataEntry:
                    tblRevStats = SessionHelper.GetSessionStatisticsForDataEntry(sessionID, currentUser.ID);
                    break;
            }

            return tblRevStats;
        }

        public static SessionDetails GetSessionDetailsFromSessionObj(Session session)
        {
            DateTime hijriDate = session.Date;
            List<SessionAttendant> attendants = new List<SessionAttendant>();
            if (session.Attendants.IsLoaded)
            {
                foreach (Attendant attendant in session.Attendants.ToList<Attendant>())
                {                    
                    Model.AttendantState stateEnum = Model.AttendantState.Attended;
                    stateEnum = (Model.AttendantState)attendant.State;

                    //if (Enum.IsDefined(typeof(Model.AttendantState), attendant.AttendantState.Name))
                    //    stateEnum = (Model.AttendantState)Enum.Parse(typeof(Model.AttendantState), attendant.AttendantState.Name);

                    Model.AttendantType typeEnum = Model.AttendantType.FromTheCouncilMembers;
                    typeEnum = (Model.AttendantType)attendant.Type;
                    //if (Enum.IsDefined(typeof(Model.AttendantType), attendant.AttendantType.Name))
                    //    typeEnum = (Model.AttendantType)Enum.Parse(typeof(Model.AttendantType), attendant.AttendantType.Name);
                    int attendant_title_id = get_session_attendant_title_id(Convert.ToInt64(session.ID),Convert.ToInt64(attendant.ID));
                   
                    attendants.Add(new SessionAttendant(attendant.EparlimentID, attendant.ID, attendant.Name, attendant.JobTitle, stateEnum, typeEnum, attendant.FirstName,attendant.SecondName, attendant.TribeName,attendant_title_id));
                }
            }


            Hashtable agendaItems = new Hashtable();

            if (session.AgendaItems.IsLoaded)
            {
                foreach (AgendaItem agendaItem in session.AgendaItems.ToList<AgendaItem>())
                {
                    SessionAgendaItem newSessionAgendaItem = new SessionAgendaItem(agendaItem.ID, agendaItem.EParliamentID, agendaItem.EParliamentParentID, agendaItem.Name, agendaItem.Order, agendaItem.IsCustom);
                    
                    if (!(bool)agendaItem.IsCustom && agendaItem.AgendaSubItems.Count > 0)
                    {
                        foreach (AgendaSubItem agendaSubItem in agendaItem.AgendaSubItems)
                        {
                            newSessionAgendaItem.SubAgendaItems.Add(new SessionAgendaItem(agendaSubItem.ID, agendaSubItem.EParliamentID, agendaSubItem.EParliamentParentID, agendaSubItem.Name, agendaSubItem.Order, agendaSubItem.QFrom, agendaSubItem.QTo));
                            //agendaItems.Add(agendaItem.ID, new SessionAgendaItem(agendaSubItem.ID, (long)agendaSubItem.EParliamentID, (long)agendaSubItem.EParliamentID, agendaSubItem.Name));
                        }
                    }
                    agendaItems.Add(agendaItem.ID, newSessionAgendaItem);
                }
            }

            List<SessionAudioFile> sessionFiles = new List<SessionAudioFile>();
            if (session.SessionFiles.IsLoaded)
            {
                foreach (SessionFile sf in session.SessionFiles.OrderBy(x => x.Order))
                {
                    sessionFiles.Add(new SessionAudioFile(sf.ID,
                        sf.UserID,
                        sf.SessionID,
                        sf.Name,
                        sf.LastInsertedFragNumInXml,
                        sf.Order,
                        sf.DurationSecs,
                        (Model.SessionFileStatus)sf.Status,
                        (sf.User != null) ? sf.User.FName : null,
                        sf.LastModefied,
                        (sf.User != null) ? sf.User.Email : null, 
                        sf.IsSessionStart, 
                        sf.SessionStartText,
                        sf.FileReviewerID, 
                        (sf.FileReviewer != null) ? sf.FileReviewer.FName : null,
                        (sf.FileReviewer != null) ? sf.FileReviewer.Email : null));
                }
            }

            List<SessionAttachment> sessionAttachments = new List<SessionAttachment>();
            if (session.Attachements.IsLoaded)
            {
                foreach (Attachement attachment in session.Attachements.OrderBy(x=> x.Order))
                {
                    sessionAttachments.Add(new SessionAttachment(attachment.ID,
                            attachment.Name,
                            (FileExtensionType)Enum.Parse(typeof(FileExtensionType), attachment.FileType, true),
                            attachment.Order,
                            session.ID,
                            null));
                }

            }
            
            //if (!session.AgendaItems.Childs.IsLoaded)
            //   {
            //      session.AgendaItems.Childs.Load();
            //   }
            //foreach (AgendaItem item in session.AgendaItems.Childs.ToList<AgendaItem>())
            //{
            //    long id = item.ID;
            //    long idParent = (long)item.ParentID;
            //    long idParentEP = (long)item.EParliamentID;
            //    string title = item.Name;

            //    if (agendaItems.ContainsKey(idParent))
            //    {
            //        SessionAgendaItem tmp = (SessionAgendaItem)agendaItems[idParent];
            //        tmp.SubAgendaItems.Add(new SessionAgendaItem(id, title));
            //        agendaItems[idParent] = tmp;
            //    }
            //    else
            //    {
            //        agendaItems.Add(id, new SessionAgendaItem(id, title));
            //    }
                
            //    if(!item.Childs.IsLoaded)
            //    {
            //        item.Childs.Load();
            //    }



            //    foreach (AgendaItem itemlevel2 in session.AgendaItem.Childs.ToList<AgendaItem>())
            //    {
            //        long id2 = itemlevel2.ID;
            //        long idParent2 = (long)itemlevel2.ParentID;
            //        long idParentEP2 = (long)itemlevel2.EParliamentID;
            //        string title2 = item.Name;

            //        if (agendaItems.ContainsKey(idParent2))
            //        {
            //            SessionAgendaItem tmp = (SessionAgendaItem)agendaItems[idParent2];
            //            tmp.SubAgendaItems.Add(new SessionAgendaItem(id2, title2));
            //            agendaItems[idParent2] = tmp;
            //        }
            //        else
            //        {
            //            agendaItems.Add(id2, new SessionAgendaItem(id2, title2));
            //        }
                
            //    }

            //}
            string reviwerName = "";
            if(session.ReviewerID!=null)
                reviwerName= UserHelper.GetUserByID((long)session.ReviewerID).FName;
            
            SessionDetails sd = new SessionDetails(
                session.EParliamentID,
                session.ID,                
                session.Serial,
                session.Date,
                hijriDate, // from
                (DateTime)session.StartTime,
                (DateTime)session.EndTime,
                session.Type,
                session.President,
                session.Place,
                session.Season,
                session.Stage,
                session.StageType,
                sessionFiles,
                attendants,
                sessionAttachments,
                agendaItems,
                (Model.SessionStatus)session.SessionStatusID,
                session.Subject,
                session.ReviewerID,
                reviwerName,
                session.MP3FolderPath
                );

            return sd;
        }

        public static int get_session_attendant_title_id(long session_id,long attendant_id)
        {
            int role = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMadbatahConn"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("SELECT AttendantTitleID FROM SessionAttendant where SessionID=" + session_id.ToString() + " and AttendantID=" + attendant_id.ToString(), cn);
                cn.Open();
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                rdr.Read();
                role = Convert.ToInt32(rdr[0]);
                // Response.Write(rdr[0].ToString()); //read a value
            }
            return role;
        }

        public static int update_session_attendant_title(long session_id, long attendant_id,long session_attendant_title_id)
        {
            int status = 0;
            using (SqlConnection cn = new SqlConnection(ConfigurationManager.ConnectionStrings["EMadbatahConn"].ToString()))
            {
                SqlCommand cmd = new SqlCommand("update SessionAttendant set AttendantTitleID=" + session_attendant_title_id.ToString() + " where SessionID=" + session_id.ToString() + " and AttendantID=" + attendant_id.ToString(), cn);
                cn.Open();
                status = cmd.ExecuteNonQuery();
                cn.Close();
            }
            return status;
        }

        public static bool UnloackSessionFile(long sessionFileID)
        {
            return SessionFileHelper.UnlockSessionFile(sessionFileID);
        }

        public static bool UnloackSessionFileReviewer(long sessionFileID)
        {
            return SessionFileHelper.UnloackSessionFileReviewer(sessionFileID);
        }


        public static bool AssignSessionFileToUser(long sessionFileID, long userID, bool isAdmin)
        {
            return SessionFileHelper.LockSessionFile(sessionFileID, userID, isAdmin);
        }

        public static bool AssignSessionFileToFileReviewer(long sessionFileID, long userID, bool isAdmin)
        {
            return SessionFileHelper.LockSessionFileReviewer(sessionFileID, userID, isAdmin);
        }

        public static void CreateMadbatahFiles(object threadParams)
        {

            foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.StartsWith("WINWORD"))
                {
                    thisproc.Kill();
                }
            }

            object[] threadObjs = (object[])threadParams;//sessionDetailsObj;
            SessionDetails details = (SessionDetails)threadObjs[0];//sessionDetailsObj;
            EMadbatahUser threadUser = (EMadbatahUser)threadObjs[1];
            HttpContext threadContext = (HttpContext)threadObjs[2];
            FileVersion fileVersion = (FileVersion)threadObjs[3];
            long sessionID = details.SessionID;
            string folderPath = threadContext.Server.MapPath("~") + @"\Files\" + sessionID + @"\";
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);

            //TayaIT.Enterprise.EMadbatah.Model.SessionDetails details = SessionStartFacade.GetSessionDetails(sessionID);
            if (details.Status == Model.SessionStatus.Approved ||
                details.Status == Model.SessionStatus.FinalApproved
                )
            {
                DAL.SessionFile start = SessionStartFacade.GetSessionStartBySessionID(sessionID);
                try
                {
                    if (start != null)
                    {
                        DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)Model.MadbatahFilesStatus.InProgress);

                        if (MabatahCreatorFacade.CreateMadbatah(sessionID, folderPath, threadContext.Server.MapPath("~")))
                        {
                            Thread.Sleep(5000);
                            string wordFilePath = folderPath + sessionID + ".docx";
                            byte[] wordDoc = File.ReadAllBytes(wordFilePath);
                            bool pdfSuccess = WordCom.ConvertDocument(wordFilePath, wordFilePath.Replace(".docx", ".pdf"), TargetFormat.Pdf);
                            //bool pdfSuccess = PdfMaker.ConvertDocxToPdf(folderPath,threadContext.Server.MapPath("~"),  wordFilePath, wordFilePath.Replace(".docx", ".docx.pdf"));
                            //Correct: PdfMaker.ConvertDocxToPdf(SessionWorkingDir, ServerMapPath, SessionWorkingDir + sessionID + ".docx", SessionWorkingDir + sessionID + ".pdf");
                            if (!pdfSuccess)
                            {
                                LogHelper.LogMessage("PDF creation failed", "ReviewerHandler", System.Diagnostics.TraceEventType.Error);
                                HandleMadbatahCreationError(details, fileVersion, threadUser, threadContext);

                            }
                            byte[] pdfDoc = File.ReadAllBytes(wordFilePath.Replace(".docx", ".pdf"));
                            Hashtable emailData = new Hashtable();
                            int ret = -1;
                            switch (fileVersion)
                            {
                                case FileVersion.draft:
                                    ret = SessionHelper.UpdateSessionWordAndPdfFiles(sessionID, wordDoc, pdfDoc);
                                    break;
                                case FileVersion.final:
                                    ret = DAL.SessionHelper.UpdateSessionWordAndPdfFiles(sessionID, wordDoc, pdfDoc, true);
                                    break;
                            }

                            if (ret > 0)
                            {
                                if (Directory.Exists(folderPath))
                                    Directory.Delete(folderPath, true);
                                string sessionName = EMadbatahFacade.GetSessionName(details.Season, details.Stage, details.Serial);

                                emailData.Add("<%SessionName%>", sessionName);
                                emailData.Add("<%SessionDate%>", details.Date.ToShortDateString());
                                emailData.Add("<%RevName%>", threadUser.Name);
                                switch (fileVersion)
                                {
                                    case FileVersion.draft:
                                        DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)Model.MadbatahFilesStatus.DraftCreated);
                                        MailManager.SendMail(new Email(new Emailreceptionist(threadUser.Email, threadUser.Name)), SystemMailType.MadbatahFileCreated, emailData, threadContext);
                                        break;
                                    case FileVersion.final:
                                        DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)Model.MadbatahFilesStatus.FinalCreated);
                                        Eparliment ep = new Eparliment();
                                        bool result = ep.IngestContentsForFinalApprove(sessionID);
                                        if (result)
                                            MailManager.SendMail(new Email(new Emailreceptionist(threadUser.Email, threadUser.Name)), SystemMailType.FinalApproveSession, emailData, threadContext);
                                        else
                                        {
                                            SessionHelper.UpdateSessionStatus(sessionID, (int)Model.SessionStatus.Approved);
                                            MailManager.SendMail(new Email(new Emailreceptionist(threadUser.Email, threadUser.Name)), SystemMailType.FinalApproveSessionFail, emailData, threadContext);
                                        }
                                        break;
                                }

                            }
                            else
                            {
                                // show error happend while adding madbatah word and pdf to database
                                //log error
                                LogHelper.LogMessage("error happend while adding madbatah word and pdf to database", "ReviewerHandler", System.Diagnostics.TraceEventType.Error);
                                HandleMadbatahCreationError(details, fileVersion, threadUser, threadContext);
                                //if (Directory.Exists(folderPath))
                                //    Directory.Delete(folderPath, true);
                            }
                        }
                        else
                        {
                            //show error something went wrong when creating the Madbatah Word/pdf
                            //log error
                            LogHelper.LogMessage("show error something went wrong when creating the Madbatah Word/pdf", "ReviewerHandler", System.Diagnostics.TraceEventType.Error);
                            HandleMadbatahCreationError(details, fileVersion, threadUser, threadContext);
                            if (Directory.Exists(folderPath))
                                Directory.Delete(folderPath, true);
                        }

                    }
                    else
                    {
                        //show warning session start not completed yet
                        //log error
                        LogHelper.LogMessage("Session Start not completed yet", "ReviewerHandler", System.Diagnostics.TraceEventType.Error);
                        if (Directory.Exists(folderPath))
                            Directory.Delete(folderPath, true);
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, "SessionID: ");

                }
            }
            else
            {
                //show warninig session start not approved yet
                //log error
                LogHelper.LogMessage("session start not approved yet", "ReviewerHandler", System.Diagnostics.TraceEventType.Information);
                if (Directory.Exists(folderPath))
                    Directory.Delete(folderPath, true);
            }
        }
        public static void HandleMadbatahCreationError(SessionDetails details, FileVersion fileVersion, EMadbatahUser threadUser, HttpContext threadContext)
        {
            System.Diagnostics.Process myproc = new System.Diagnostics.Process();
            foreach (System.Diagnostics.Process thisproc in System.Diagnostics.Process.GetProcesses())
            {
                if (thisproc.ProcessName.StartsWith("WINWORD"))
                {
                    thisproc.Kill();
                }
            }

            string sessionName = EMadbatahFacade.GetSessionName(details.Season, details.Stage, details.Serial);
            Hashtable failEmailData = new Hashtable();
            failEmailData.Add("<%SessionName%>", sessionName);
            failEmailData.Add("<%SessionDate%>", details.Date.ToShortDateString());

            switch (fileVersion)
            {
                case FileVersion.draft:
                    DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)Model.MadbatahFilesStatus.DraftFail);
                    SessionHelper.UpdateSessionStatus(details.SessionID, (int)Model.SessionStatus.Completed);
                    failEmailData.Add("<%SessionStatus%>", Localization.LocalHelper.GetLocalizedString("strSessionStatusCompleted"));
                    break;
                case FileVersion.final:
                    DAL.SessionHelper.UpdateSessionMadabathFilesStatus(details.SessionID, (int)Model.MadbatahFilesStatus.FinalFail);
                    SessionHelper.UpdateSessionStatus(details.SessionID, (int)Model.SessionStatus.Approved);
                    failEmailData.Add("<%SessionStatus%>", Localization.LocalHelper.GetLocalizedString("strSessionStatusApproved"));
                    break;
            }
            MailManager.SendMail(new Email(new Emailreceptionist(threadUser.Email, threadUser.Name)), SystemMailType.MadbatahFilesCreationFailed, failEmailData, threadContext);
        }

       
        public static ICollection GetSpeakersStats(bool isAllSpeakers, long sessionID, long agendaItemID, long agendaSubItemID)
        {

            long unkonwnAttID = AttendantHelper.GetUnknownAttendantId(sessionID);
            SessionDetails sd = BLL.EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
            List<SessionAttendant> attendants = new List<SessionAttendant>();
            //Hashtable allSpeakersTiming = new Hashtable();
            Hashtable allSpeakers = new Hashtable();
            Hashtable agendaItemSpeakers = new Hashtable();

            //bind Agenda Items Drop Down List

            if ((agendaItemID >0 || agendaSubItemID >0))
            {
                List<Attendant> tmpagendaItemsSpeakrsList = null;                
                    tmpagendaItemsSpeakrsList = AttendantHelper.GetAttendants(sessionID, agendaItemID, agendaSubItemID, isAllSpeakers);



                    if (tmpagendaItemsSpeakrsList == null)
                    {
                        return null;
                    }
                foreach (Attendant attendant in tmpagendaItemsSpeakrsList)
                {


                    Model.AttendantState stateEnum = Model.AttendantState.Attended;
                    stateEnum = (Model.AttendantState)attendant.State;

                    Model.AttendantType typeEnum = Model.AttendantType.FromTheCouncilMembers;
                    typeEnum = (Model.AttendantType)attendant.Type;
                    int attendant_title_id = get_session_attendant_title_id(Convert.ToInt64(sessionID), Convert.ToInt64(attendant.ID));
                    attendants.Add(new SessionAttendant(attendant.EparlimentID, attendant.ID, attendant.Name, attendant.JobTitle, stateEnum, typeEnum, attendant.FirstName, attendant.SecondName, attendant.TribeName, attendant_title_id));
                }
            }
            else if (!isAllSpeakers)
            {
                attendants = (from att in sd.Attendance
                             where att.State == Model.AttendantState.Attended
                             select att).ToList<SessionAttendant>();
            }
            else
            {
                attendants = sd.Attendance;//AttendantHelper.GetAttendantsBySessionId(sessionID);
            }



            foreach (SessionAttendant att in attendants)
            {
                //allSpeakersTiming.Add(att.ID, 0);
                allSpeakers.Add(att.ID, att);
            }

            List<List<SessionContentItem>> allItems = SessionContentItemHelper.GetItemsBySessionIDGrouped(sessionID);
            foreach (List<SessionContentItem> groupedItems in allItems)
            {

                AgendaItem curAgendaItem = groupedItems[0].AgendaItem;
                AgendaSubItem curAgendaSubItem = groupedItems[0].AgendaSubItem;

                foreach (SessionContentItem sessionItem in groupedItems)
                {

                        //double speakerTime = double.Parse(allSpeakersTiming[sessionItem.AttendantID].ToString());
                        //double currentSegmentTime = sessionItem.Duration == null ? 0 : sessionItem.Duration.Value;
                        //allSpeakersTiming[sessionItem.AttendantID] = speakerTime + currentSegmentTime;
                        SessionAttendant currentSessionAttendant = (SessionAttendant)allSpeakers[sessionItem.AttendantID];
                        if (currentSessionAttendant != null)
                        {
                            double currentSegmentTime = sessionItem.Duration == null ? 0 : sessionItem.Duration.Value;
                            currentSessionAttendant.TotalSpeakTime += currentSegmentTime;
                            allSpeakers[sessionItem.AttendantID] = currentSessionAttendant;
                        }
                }

                //foreach (long key in allSpeakersTiming.Keys)
                //foreach (long key in allSpeakers.Keys)
                //{
                //    SessionAttendant att = (SessionAttendant)allSpeakers[key];
                //    att.TotalSpeakTime = double.Parse(allSpeakers[key].ToString());
                //    allSpeakers[key] = att;
                //    // Response.Write("NAME: " + att.Name + " -JOBTITLE: "  + att.JobTitle + " -STATE: " + att.State.ToString() + " -TYPE: " + att.Type.ToString() + " -TIME: " +  allSpeakersTiming[key].ToString() + "<br/>");
                //}

            }
            if (unkonwnAttID != -1)
                allSpeakers.Remove(unkonwnAttID);
            return allSpeakers.Values;
        }
    }
}
