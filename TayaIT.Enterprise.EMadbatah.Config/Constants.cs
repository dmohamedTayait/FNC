using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace TayaIT.Enterprise.EMadbatah.Config
{
    public class Constants
    {
        private static QueryStringKeyNames _qsNames = null;
        public static QueryStringKeyNames QSKeyNames
        {
            get
            {
                if (_qsNames != null)
                    return _qsNames;
                else
                    _qsNames = new QueryStringKeyNames();
                return _qsNames;
            }
        }
        public class QueryStringKeyNames
        {

            public string SEND_EMAIL { get { return "semail"; } }

            public string SESSION_ID { get { return "sid"; } }

            public string SESSION_PREV_ID { get { return "srid"; } }

            public string SESSION_ID_EParliment { get { return "epsid"; } }
            public string NEW_SESSION_AUDIO_FOLDER_PATH { get { return "safp"; } }
            public string NEW_SESSION_XML_FOLDER_PATH { get { return "sxfp"; } }

            public string ATTENDANT_ID { get { return "attid"; } }
            public string SESSION_ATTENDANT_TITLE_ID { get { return "s_att_title_id"; } }
            public string ATTENDANT_ID_EParliment { get { return "epattid"; } }

            public string AGENDA_ITEM_ID { get { return "agid"; } }
            public string AGENDA_ITEM_ID_EParliment { get { return "epagid"; } }

            public string USER_ID { get { return "uid"; } }
            public string SESSION_FILE_ID { get { return "sfid"; } }
            public string SESSION_START_ID { get { return "ssid"; } }
            public string SESSION_CONTENT_ITEM_ID { get { return "scid"; } }
           


            public string AJAX_FUNCTION_NAME { get { return "funcname"; } }

            //for Admin
            public string USER_NAME { get { return "username"; } }
            public string USER_EMAIL { get { return "useremail"; } }
            public string USER_DOMAIN_USER_NAME { get { return "userdomainname"; } }
            public string USER_IS_ACTIVE { get { return "userisactive"; } }
            public string USER_ROLE_ID { get { return "userroleid"; } }
            public string VECSYS_PATH { get { return "vecpath"; } }
            public string AUDIO_PATH { get { return "audiopath"; } }
            public string EPARLIMENT_URL { get { return "epurl"; } }

            //for Paging
            public string PAGE_NO { get { return "pageno"; } }
            public string ITEMS_PER_PAGE { get { return "items"; } }

            //for re-order
            public string NEW_ORDER1 { get { return "order1"; } }
            public string NEW_ORDER2 { get { return "order2"; } }
            public string REORDER_ID1 { get { return "rid1"; } }
            public string REORDER_ID2 { get { return "rid2"; } }

            //for attachment // and downloads
            public string ATTACHMENT_ID { get { return "attachid"; } }
            public string ATTACHMENT_ORDER { get { return "attachorder"; } }
            public string ATTACHMENT_FILE_NAME { get { return "qqfile"; } }
            public string FILE_TYPE { get { return "filetype"; } }
            public string FILE_VERSION { get { return "filever"; } }

            //for reviewer
            public string CONTENT_ITEM_TEXT { get { return "contentitemtext"; } }
            public string REVIEWER_NOTE { get { return "reviewernote"; } }
            public string REVIEWER_USER_ID { get { return "revieweruserid"; } }
            public string REVIEWER_UPDATED { get { return "reviewerupdate"; } }

            //for editor
            public string AGENDA_ID { get { return "agendaid"; } }
            public string AGENDA_ITEM_TEXT { get { return "agendaitemtext"; } }
            public string EDIT_PAGE_MODE { get { return "editmode"; } }
            public string SESSION_CONTENT_ITEM_STATUS { get { return "scistatus"; } }

            public string AgendaItemID { get { return "AgendaItemID"; } }
            public string AgendaSubItemID { get { return "AgendaSubItemID"; } }
            public string SpeakerID { get { return "SpeakerID"; } }
            public string SameAsPrevSpeaker { get { return "SameAsPrevSpeaker"; } }
            public string IsGroupSubAgendaItems { get { return "IsGroupSubAgendaItems"; } }
            public string Ignored { get { return "Ignored"; } }
            public string PrevContentID { get { return "PrevContentID"; } }
            public string SpeakerJob { get { return "SpeakerJob"; } }
            public string Text { get { return "Text"; } }
            public string Comments { get { return "Comments"; } }
            public string Footer { get { return "Footer"; } }
            public string REDIT { get { return "reedit"; } }
            public string FRAGORDER { get { return "FRAGORDER"; } }
            public string XMLPATH { get { return "XMLPATH"; } }
            public string SPLITTEDTEXT { get { return "SPLITTEDTEXT"; } }

            //for error page
            public string ERROR_TYPE { get { return "errtype"; } }

            //for SessionStart
            public string SESSION_START_HTML { get { return "sshtml"; } }

            //signout
            public string SIGN_OUT_TYPE { get { return "signouttype"; } }

            //for statistics page
            public string SPEAKERS_TYPE { get { return "speakerstype"; } }

            //for SessionAgendaIndex page
            public string AGENDA_ITEM_NAME { get { return "itemname"; } }


        }

        public static class PageNames
        {
            public const string DEFAULT_HOME = "Default.aspx";
            public const string EDIT_SESSION_FILE = "EditSessionFile.aspx";
            public const string REVIEW_NOTES = "ReviewNotes.aspx";
            public const string REVIEW = "Review.aspx";
            public const string STATISTICS = "Statistics.aspx";
            public const string EDIT_INDEX_ITEMS = "EditIndexItems.aspx";
            public const string SESSION_START = "SessionStart.aspx";
            public const string FINAL_APPROVED_SESSION = "FinalApprovedSessions.aspx";

            public const string ADMIN_HOME = "AdminHome.aspx";
            public const string ADMIN_SECURITY = "AdminSecurity.aspx";
            public const string ADMIN_SESSIONS = "AdminSessions.aspx";
            public const string ADMIN_APP_CONFIG = "AdminAPPConfig.aspx";

            public const string ERROR_PAGE = "Error.aspx";
            public const string SIGN_OUT = "SignOut.aspx";

        }

        public static class WebSiteFolderNames
        {
            public const string MAIL_TEMPLATES = "MailTemplates";
            public const string HTML_TEMPLATES = "HTMLTemplates";
            public const string APP_DATA = "App_Data"; 
        }

        public static class HTMLTemplateFileNames
        {
            public const string SESSION_HEADER = "SessionsHeader.htm";
            public const string SESSION_DATA = "SessionData.htm";
            public const string ATTACHMENT_LIST_REVIEWER = "AttachmentsListForReviewer.htm";
            public const string ATTACHMENT_LIST_DATAENTRY = "AttachmentsListForDataEntry.htm";
            public const string FILE_LIST_REVIEWER = "FilesListForReviewer.htm";
            public const string FILE_LIST_DATAENTRY = "FilesListForDataEntry.htm";
            public const string Add_New_User_WinRow = "AddNewUserWinRow.htm";
            public const string EMadbatahUserRow = "EMadbatahUserRow.htm";
            public const string ReviewNotesFileInfo = "ReviewNotesFileInfo.htm";
            public const string ReviewNotesFileNotes = "ReviewNotesFileNotes.htm";
            public const string ReviewNotesFileNoteItem = "ReviewNotesFileNoteItem.htm";
            public const string ReviewItem = "ReviewItem.htm";
            public const string ReviewItemAgendaItem = "ReviewItemAgendaItem.htm";
            public const string ReviewItemAgendaSubItem = "ReviewItemAgendaSubItem.htm";
            public const string ReviewItemSpeaker = "ReviewItemSpeaker.htm";
        }

        public static class EmailTemplateFileNames
        {
            public const string GENERAL_EMAIL_HTML = "GeneralEmail.htm";
            public const string APPROVE_SESSION= "ApproveSessionEmailBody.txt";
            public const string CHANGE_USER_ROLE = "ChangeUserRoleEmailBody.txt";
            public const string DEACTIVATE_USER = "DeActivateUserEmailBody.txt";
            public const string DELETE_USER = "DeleteUserEmailBody.txt";
            public const string FINAL_APPROVE_SESSION = "FinalApproveSessionEmailBody.txt";
            public const string NEW_USER = "NewUserEmailBody.txt"; //done
            public const string REASSIGN_SESSION_FILE = "ReAssignSessionFileEmailBody.txt";
            public const string REASSIGN_SESSION_REVIEWER = "ReAssignSessionRevFileEmailBody.txt";
            public const string SESSION_MARKED_COMPLETE = "SessionMarkedAsCompleteEmailBody.txt";
            public const string UNLOCK_SESSION_FILE = "UnlockSessionFileEmailBody.txt";
            public const string UNLOCK_SESSION_REVIEWER = "UnlockSessionRevEmailBody.txt";
            public const string ASSIGN_SESSION_REVIEWER = "AssignSessionRevFileEmailBody.txt";
            public const string NEW_MADBATAH_WORD_CREATED = "NewMadbatahWordFileCreatedEmailBody.txt";
            

        } 

        public static class FileNames
        {
            public const string APP_CONFIG = "AppSettings.xml";
            public const string EPARLIMENT_SAMPLE_DATA = "eMadbatah_Sample_Data_V1.5.xlsx";
        }

        public static class SessionObjectsNames
        {
            public const string USER_ROLES = "UserRoles";
            public const string CURRENT_USER = "CurrentUser";
            public const string CURRENT_DOMAIN = "CurrentDomain";
        }

        public static class CookieNames
        {
            public const string PREFERENCES_COOKIE = "cookie_preferences";
        }

        public static class Security
        {
            public const string MAIN_ADMINS = "MainAdmins";
            public const string EXPLICIT_MAIN_ADMINS = "ExplicitMainAdmins";
            public const string ALLOWED_DOMAIN_NAME = "AllowedDomainName";
        }
        public static class Settings
        {
            public const string ROOT_URL = "RootUrl";            
        }

    }
}
