using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Web.Script.Serialization;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Util.Web;

namespace TayaIT.Enterprise.EMadbatah.Web.Framework
{
    public abstract class BaseHandler : IHttpHandler, IRequiresSessionState 
    {
        public HttpContext _context = null;
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        protected abstract void HandleRequest();

        public void Init() 
        {

        }
        
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            _context = context;
            HandleRequest();

        }

        public string SerializeObjectInJSON(Object obj)
        {
            return serializer.Serialize(obj);
        }

        public Object DeSerializeObjectInJSON(String objJSONString)
        {
            return serializer.Deserialize<Object>(objJSONString);
        }


         public EMadbatahUser CurrentUser
         {
             get
             {

                 return (EMadbatahUser)_context.Session[Constants.SessionObjectsNames.CURRENT_USER];
             }
         }
         public string CurrentDomain
         {
             get
             {
                 return (string)_context.Session[Constants.SessionObjectsNames.CURRENT_DOMAIN];
             }
         }

         public string SessionID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_ID, _context);                 
             }
         }

         public string SessionIDEParliment
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_ID_EParliment, _context); 
             }
         }

         public string AttendantID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.ATTENDANT_ID, _context); 
             }
         }

         public string SessionAttendantTitleID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_ATTENDANT_TITLE_ID, _context);
             }
         }

         public string AttendantIDEParliment
         {
             get
             {

                 return WebHelper.GetQSValue(Constants.QSKeyNames.ATTENDANT_ID_EParliment, _context);
             }
         }

         public string AgendaItemID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.AGENDA_ITEM_ID, _context);                 
             }
         }

         public string AgendaSubItemID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.AgendaSubItemID, _context);
             }
         }

         public string AgendaItemIDEparliment
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.AGENDA_ITEM_ID_EParliment, _context);                  
             }
         }

         public string UserID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.USER_ID, _context); 
                 
             }
         }

         public string SessionFileID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_FILE_ID, _context);                 
             }
         }        


         public string SessionStartID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_START_ID, _context);                  
             }
         }

         public string SessionContentItemID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_CONTENT_ITEM_ID, _context);                  
             }
         }

         public string AttachmentID
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.ATTACHMENT_ID, _context);                  
             }
         }

         public string AjaxFunctionName
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.AJAX_FUNCTION_NAME, _context);
             }
         }

         public string SendEmail
         {
             get
             {
                 return WebHelper.GetQSValue(Constants.QSKeyNames.SEND_EMAIL, _context);
             }
         }

         public EmailServer EmailServerSettings 
         {
             get { return AppConfig.GetInstance().EmailServerSettings; }
         }

    }
}
