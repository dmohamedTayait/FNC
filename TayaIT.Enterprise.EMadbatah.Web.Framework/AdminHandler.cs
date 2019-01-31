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
using TayaIT.Enterprise.EMadbatah.Util.Web;
using System.IO;
using System.Security.Principal;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Localization;
using System.Web.Security;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    public class AdminHandler : BaseHandler
    {

        protected override void HandleRequest()
        {

            //if (_context.Request.ContentType.Contains("json")) // these are ajax json posts 
            //{
            
            
                    
                WebFunctions.AdminFunctions function;

                if (AjaxFunctionName != null && Enum.TryParse<WebFunctions.AdminFunctions>(AjaxFunctionName, true, out function))
                {
                    string jsonStringOut = null;
                    long userId;
                    long sessionFileID, sessionID, revID;
                    bool isActiveUser = false;
                    int userRoleId = -1;
                    Hashtable emailData = new Hashtable();
                    
                    switch (function)
                    {
                        case WebFunctions.AdminFunctions.AddNewUser:
                            if (UserName != null && UserDomainUserName != null //&& UserEmail != null 
                                && UserRoleID != null && int.TryParse(UserRoleID, out userRoleId) && Enum.IsDefined(typeof(UserRole), userRoleId )
                                && UserIsActive != null && bool.TryParse(UserIsActive, out isActiveUser))
                            {
                                //long toRet=-1;
                                //EMadbatahUser user = EMadbatahFacade.GetUserByDomainUserName(UserDomainUserName);
                                DAL.User user = DAL.UserHelper.GetUserByDomainName(UserDomainUserName);
                                Hashtable result = new Hashtable();
                                
                                if (user != null && user.Deleted)
                                {
                                    int res = TayaIT.Enterprise.EMadbatah.DAL.UserHelper.UnDeleteUser(user.ID, userRoleId, UserName, UserDomainUserName, UserEmail, isActiveUser);
                                    if (res >= 0)
                                    {
                                        result["Message"] = "success";
                                        result["ID"] = user.ID;
                                        result["IsDuplicate"] = "false";
                                    }
                                    else
                                        result["Message"] = "fail";
                                   
                                }
                                else
                                    if (user == null)
                                    {
                                        //
                                        long res = EMadbatahFacade.AddNewUser(new EMadbatahUser((UserRole)userRoleId, UserName, UserDomainUserName, UserEmail, isActiveUser));
                                        if (res >= 0)
                                        {
                                            result["Message"] = "success";
                                            result["ID"] = res;
                                            result["IsDuplicate"] = "false";
                                        }
                                        else
                                            result["Message"] = "fail";
                                    }
                                    else
                                        if (user != null && !user.Deleted)
                                        {
                                            result["Message"] = "fail";
                                            result["IsDuplicate"] = "true";
                                            
                                        }
                                //EMAIL NewUser
                                if ( result["Message"] == "success")
                                {                                   
                                    emailData.Add("<%RoleName%>", LocalHelper.GetLocalizedString("strRole" + userRoleId.ToString()));
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);
                                    emailData.Add("<%RootUrl%>", AppConfig.GetInstance().RootUrl);
                                    MailManager.SendMail(new Email(new Emailreceptionist(UserEmail, UserName)), SystemMailType.NewUser, emailData);
                                }
                                jsonStringOut = SerializeObjectInJSON(result);

                            }
                            break;
                        case WebFunctions.AdminFunctions.GetAllUsersFromDB:
                            List<EMadbatahUser> usersdb = EMadbatahFacade.GetUsersFromDB(false);
                            jsonStringOut = SerializeObjectInJSON(usersdb);
                            break;

                        case WebFunctions.AdminFunctions.SignOut:
                            _context.Response.StatusCode = 401;
                            _context.Response.Status = "401 Unauthorized";
                            _context.Response.AddHeader("WWW-Authenticate", "BASIC Realm=my application name");

                            _context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            _context.Response.Cache.SetExpires(DateTime.MinValue);
                            _context.Response.Cache.SetNoStore();

                             HttpContext.Current.Session.Clear();
                             HttpContext.Current.Session.Abandon();
                            FormsAuthentication.SignOut();
                            //_context.Response.Redirect("/");



                            jsonStringOut = SerializeObjectInJSON("ok");
                            break;

                        case WebFunctions.AdminFunctions.GetRevUsersFromDB:
                            List<EMadbatahUser> rev = EMadbatahFacade.GetRevUsersFromDB();
                            jsonStringOut = SerializeObjectInJSON(rev);
                            break;


                        case WebFunctions.AdminFunctions.GetAllUsersFromAD:
                            List<EMadbatahUser> usersad = EMadbatahFacade.GetUsersFromAD(CurrentUser);
                            StringBuilder sb = new StringBuilder();

                            foreach(EMadbatahUser user in usersad)
                            {
                                

                                string newUserRow = _context.Application[Constants.HTMLTemplateFileNames.Add_New_User_WinRow].ToString();
                                

                                newUserRow = newUserRow.Replace("<%UserName%>", user.Name);
                                newUserRow = newUserRow.Replace("<%DomainUserName%>", user.DomainUserName);
                                newUserRow = newUserRow.Replace("<%Email%>", user.Email);

                                sb.Append(newUserRow);
                                /*
                                sb.Append("<tr><td><input name=\"\" type=\"checkbox\" value=\"\" />");
                                sb.Append(user.Name+"</td>");
                                sb.Append("<td>"+user.DomainUserName+"</td>");
                                sb.Append("<td>"+user.Email+"</td>");
                                sb.Append("<td>");
                                sb.Append("<select class=\"fitAll\" name=\"task\">");
                                sb.Append("<option value=\"1\">مصحح</option>");
                                sb.Append("<option value=\"2\">ادمين</option>");
                                sb.Append("</select>");
                                sb.Append("</td> </tr>");*/
                            }
                            jsonStringOut = sb.ToString();
                            //jsonStringOut = SerializeObjectInJSON(usersad);
                            break;

                        case WebFunctions.AdminFunctions.ActivateUser:
                            if (UserID != null && long.TryParse(UserID, out userId))
                                jsonStringOut = SerializeObjectInJSON(EMadbatahFacade.ActivateUser(userId));
                            break;

                        case WebFunctions.AdminFunctions.DeActivateUser:
                            if (UserID != null && long.TryParse(UserID, out userId))
                            {
                                int res = EMadbatahFacade.DeActivateUser(userId);
                                if (res > 0)
                                {
                                    //EMAIL DeActivateUser
                                    EMadbatahUser user = EMadbatahFacade.GetUserByUserID(userId);
                                    if (user != null && user.IsActive)
                                    {                                       
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);                                       
                                        MailManager.SendMail(new Email(new Emailreceptionist(user.Email,user.Name)), SystemMailType.DeActivateUser, emailData);
                                    }
                                }
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;
                            
                        case WebFunctions.AdminFunctions.RemoveUser:
                            if (UserID != null && long.TryParse(UserID, out userId))
                            {

                                int res = EMadbatahFacade.RemoveUser(userId);                                                               
                                if (res > 0)
                                {
                                    //EMAIL DeleteUser            
                                    EMadbatahUser user = EMadbatahFacade.GetUserByUserID(userId);
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);

                                    MailManager.SendMail(new Email(new Emailreceptionist(user.Email, user.Name)), SystemMailType.DeleteUser, emailData);
                                }
                                jsonStringOut = SerializeObjectInJSON(res);

                            }
                            break;

                        case WebFunctions.AdminFunctions.UpdateUser:

                            if (UserID != null && long.TryParse(UserID, out userId)
                                && UserDomainUserName != null
                               // && UserEmail != null
                                && UserIsActive != null && bool.TryParse(UserIsActive, out isActiveUser)
                                && UserRoleID != null && int.TryParse(UserRoleID, out userRoleId) && Enum.IsDefined(typeof(UserRole), userRoleId)
                                && UserName != null)
                            {
                                //EMAIL ChangeUserRole
                                EMadbatahUser user = EMadbatahFacade.GetUserByUserID(userId);
                                if (user != null && user.Role != (UserRole)userRoleId)
                                {
                                    emailData.Add("<%OldRoleName%>", LocalHelper.GetLocalizedString("strRole" + (int)user.Role));
                                    emailData.Add("<%NewRoleName%>", LocalHelper.GetLocalizedString("strRole" + userRoleId));
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);
                                    emailData.Add("<%RootUrl%>", AppConfig.GetInstance().RootUrl);
                                    MailManager.SendMail(new Email(new Emailreceptionist(UserEmail, UserName)), SystemMailType.ChangeUserRole, emailData);   
                                }

                                //EMAIL DeActivateUser
                                if (user != null && user.IsActive && isActiveUser == false)
                                {                                   
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);
                                    MailManager.SendMail(new Email(new Emailreceptionist(UserEmail, UserName)), SystemMailType.DeActivateUser, emailData);   
                                }

                                jsonStringOut = SerializeObjectInJSON(EMadbatahFacade.UpdateUser(new EMadbatahUser(userId, (UserRole)userRoleId, UserName, UserDomainUserName, UserEmail, isActiveUser)));                               
                            }                                
                            break;

                        case WebFunctions.AdminFunctions.UnlockSessionFile:

                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID))
                            {
                                SessionAudioFile sf = SessionFileFacade.GetSessionFilesByID(sessionFileID);
                                bool res = EMadbatahFacade.UnloackSessionFile(sessionFileID);
                                if (res)
                                {                                    
                                    emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);
                                    List<Emailreceptionist> rec = new List<Emailreceptionist>();
                                    rec.Add(new Emailreceptionist(sf.OwnerEmail, sf.OwnerUserName));
                                    rec.Add(new Emailreceptionist(CurrentUser.Email, CurrentUser.Name));
                                    MailManager.SendMail(new Email(rec), SystemMailType.UnlockSessionFile, emailData);   
                                }
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;





                        case WebFunctions.AdminFunctions.UnlockSessionFileReviewer:

                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID))
                            {
                                SessionAudioFile sf = SessionFileFacade.GetSessionFilesByID(sessionFileID);
                                bool res = EMadbatahFacade.UnloackSessionFileReviewer(sessionFileID);
                                if (res)
                                {                                    
                                    emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                    emailData.Add("<%AdminName%>", CurrentUser.Name);
                                    List<Emailreceptionist> rec = new List<Emailreceptionist>()
                                    {
                                        new Emailreceptionist(sf.FileReviewerEmail, sf.FileReviewerUserName),
                                        new Emailreceptionist(CurrentUser.Email, CurrentUser.Name)
                                    };
                                    MailManager.SendMail(new Email(rec), SystemMailType.UnlockSessionFileReviewer, emailData);   
                                }
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;

















                        case WebFunctions.AdminFunctions.AssignSessionFile:
                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID) &&
                                UserID != null && long.TryParse(UserID, out userId))
                            {
                                SessionAudioFile sf = SessionFileFacade.GetSessionFilesByID(sessionFileID);
                                
                                bool res = EMadbatahFacade.AssignSessionFileToUser(sessionFileID, userId, true);
                                if (res)
                                {
                                    EMadbatahUser user = EMadbatahFacade.GetUserByUserID(userId);
                                    if (sf.UserID == null)
                                    {
                                        //EMAIL AssignSessionFile
                                        emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);

                                        string sessionFileURL = "";
                                        if(sf.IsSessionStart)
                                            sessionFileURL = AppConfig.GetInstance().RootUrl + "/" + Constants.PageNames.EDIT_SESSION_FILE + "?" + Constants.QSKeyNames.SESSION_FILE_ID + "=" + sessionFileID;
                                        else
                                            sessionFileURL = AppConfig.GetInstance().RootUrl + "/" + Constants.PageNames.SESSION_START + "?" + Constants.QSKeyNames.SESSION_ID + "=" + sf.SessionID;

                                        emailData.Add("<%SessionFileUrl%>", sessionFileURL);
                                        MailManager.SendMail(new Email(new Emailreceptionist(user.Email, user.Name)), SystemMailType.AssignSessionFile, emailData);
                                    }
                                    else
                                    {
                                        
                                        //EMAIL ReAssignSessionFile
                                       // EMadbatahUser newUser = EMadbatahFacade.GetUserByUserID(userId);

                                        emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                        emailData.Add("<%OldUserName%>", sf.OwnerUserName);
                                        emailData.Add("<%NewUserName%>", user.Name);
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);

                                        string sessionFileURL = "";
                                        if (sf.IsSessionStart)
                                            sessionFileURL = AppConfig.GetInstance().RootUrl + "/" + Constants.PageNames.EDIT_SESSION_FILE + "?" + Constants.QSKeyNames.SESSION_FILE_ID + "=" + sessionFileID;
                                        else
                                            sessionFileURL = AppConfig.GetInstance().RootUrl + "/" + Constants.PageNames.SESSION_START + "?" + Constants.QSKeyNames.SESSION_ID + "=" + sf.SessionID;

                                        emailData.Add("<%SessionFileUrl%>", sessionFileURL);

                                        MailManager.SendMail(new Email(new Emailreceptionist(user.Email, user.Name)), SystemMailType.ReAssignSessionFile, emailData);
                                    }


                                }
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;



                        //AssignSessionFileReviewer
                        case WebFunctions.AdminFunctions.AssignSessionFileReviewer:
                            if (SessionFileID != null && long.TryParse(SessionFileID, out sessionFileID) &&
                                UserID != null && long.TryParse(UserID, out userId))
                            {
                                SessionAudioFile sf = SessionFileFacade.GetSessionFilesByID(sessionFileID);

                                bool res = EMadbatahFacade.AssignSessionFileToFileReviewer(sessionFileID, userId, true);

                                bool sendEmail = true;
                                if(SendEmail != null)
                                    bool.TryParse(SendEmail, out sendEmail);
                                if (res && sendEmail)
                                {
                                    EMadbatahUser user = EMadbatahFacade.GetUserByUserID(userId);
                                    if (sf.FileReviewrID == null)
                                    {
                                        //EMAIL AssignSessionFile
                                        emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);
                                        //emailData.Add("<%SessionFileUrl%>", AppConfig.GetInstance().RootUrl + "/" + Constants.PageNames.EDIT_SESSION_FILE + "?" + Constants.QSKeyNames.SESSION_FILE_ID + "=" + sessionFileID);
                                        MailManager.SendMail(new Email(new Emailreceptionist(user.Email, user.Name)), SystemMailType.AssignSessionFileReviewer, emailData);
                                    }
                                    else
                                    {

                                        //EMAIL ReAssignSessionFile
                                       

                                        emailData.Add("<%FileName%>", Path.GetFileName(sf.Name));
                                        emailData.Add("<%OldUserName%>", sf.OwnerUserName);
                                        emailData.Add("<%NewUserName%>", user.Name);
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);

                                        MailManager.SendMail(new Email(new Emailreceptionist(user.Email, user.Name)), SystemMailType.ReAssignSessionFileReviewer, emailData);
                                    }


                                }
                                jsonStringOut = SerializeObjectInJSON(res);
                            }
                            break;





                        case WebFunctions.AdminFunctions.UpdateServersPaths:
                            if (EParlimentServiveURL != null && VecsysServerNewPath != null && AudioServerNewPath !=null)
                            {
                                try
                                {
                                    AppConfig conf = new AppConfig();
                                    conf.EPrlimentServerURL = EParlimentServiveURL;
                                    conf.VecSysServerPath = VecsysServerNewPath;
                                    conf.AudioServerPath = AudioServerNewPath;
                                    conf.SaveChanges();
                                    AppConfig.GetInstance(true);
                                    jsonStringOut = SerializeObjectInJSON(true);
                                }
                                catch {
                                    jsonStringOut = SerializeObjectInJSON(false);
                                }
                            }
                            break;


                        case WebFunctions.AdminFunctions.ChangeSessionReviewer:
                            if (SessionID != null && long.TryParse(SessionID, out sessionID) &&
                                SessionReviewerID != null && long.TryParse(SessionReviewerID, out revID))
                            {
                                try
                                {
                                    //oldsessionreviewrname
                                    SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                                    int res = TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UpdateSessionReviewer(sessionID, revID);
                                    if (res > 0)
                                    {
                                        jsonStringOut = SerializeObjectInJSON(true);

                                        //EMAIL AssignSessionRev
                                        EMadbatahUser newRev = EMadbatahFacade.GetUserByUserID(revID);
                                        if (sd.ReviewerID == null)
                                        {
                                            emailData.Add("<%SessionName%>", EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial));
                                            emailData.Add("<%SessionDate%>", sd.Date.Date.ToShortDateString());
                                            emailData.Add("<%AdminName%>", CurrentUser.Name);
                                             
                                            MailManager.SendMail(new Email(new Emailreceptionist(newRev.Email,newRev.Name)), SystemMailType.AssignSessionRev, emailData);
                                        }
                                        else
                                        {
                                            //EMAIL ReAssignSessionRev
                                            EMadbatahUser oldRev = EMadbatahFacade.GetUserByUserID((long)sd.ReviewerID);

                                            emailData.Add("<%SessionName%>", EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial));
                                            emailData.Add("<%SessionDate%>", sd.Date.Date.ToShortDateString());
                                            emailData.Add("<%AdminName%>", CurrentUser.Name);
                                            emailData.Add("<%OldUserName%>", sd.ReviewerName);
                                            emailData.Add("<%NewUserName%>", newRev.Name);

                                            List<Emailreceptionist> receptionists = new List<Emailreceptionist>();
                                            receptionists.Add(new Emailreceptionist(oldRev.Email, oldRev.Name));
                                            receptionists.Add(new Emailreceptionist(newRev.Email, newRev.Name));
                                            Email email = new Email(receptionists);

                                            //email
                                            MailManager.SendMail(email, SystemMailType.ReAssignSessionRev, emailData);
                                        }
                                        
                                    }
                                    else
                                        jsonStringOut = SerializeObjectInJSON(false);
                                }
                                catch
                                {
                                    jsonStringOut = SerializeObjectInJSON(false);
                                }
                            }
                            break;


                        case WebFunctions.AdminFunctions.UnlockSessionReviewer:
                            if (SessionID != null && long.TryParse(SessionID, out sessionID))
                            {
                                try
                                {
                                    bool res = (TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.UnlockSessionReviewer(sessionID) > 0);
                                    if (res)
                                    {
                                        SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(sessionID);
                                        emailData.Add("<%SessionName%>", EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial));
                                        emailData.Add("<%SessionDate%>", sd.Date.Date.ToShortDateString());
                                        emailData.Add("<%AdminName%>", CurrentUser.Name);
                                        

                                        List<Emailreceptionist> receptionists = new List<Emailreceptionist>();
                                        receptionists.Add(new Emailreceptionist(CurrentUser.Email, CurrentUser.Email));             
                                        Email email = new Email(receptionists);
                                        //email
                                        MailManager.SendMail(email, SystemMailType.ReAssignSessionRev, emailData);


                                        jsonStringOut = SerializeObjectInJSON(true);
                                    }
                                    else
                                        jsonStringOut = SerializeObjectInJSON(false);
                                }
                                catch
                                {
                                    jsonStringOut = SerializeObjectInJSON(false);
                                }
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
        }//end ProcessRequest


        public string UserDomainUserName
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.USER_DOMAIN_USER_NAME, _context);                
            }
        }

        public string UserEmail
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.USER_EMAIL, _context);                 
            }
        }

        public string UserIsActive
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.USER_IS_ACTIVE, _context);                 
            }
        }

        public string UserName
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.USER_NAME, _context);                  
            }
        }

        public string UserRoleID
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.USER_ROLE_ID, _context);                 
            }
        }


        public string VecsysServerNewPath
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.VECSYS_PATH, _context);               
            }
        }
        public string AudioServerNewPath
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.AUDIO_PATH, _context);
            }
        }
        public string EParlimentServiveURL
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.EPARLIMENT_URL, _context);                 
            }
        }

        public string SessionReviewerID
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_PREV_ID, _context);
            }
        }
    }//end class
}//end namespace
