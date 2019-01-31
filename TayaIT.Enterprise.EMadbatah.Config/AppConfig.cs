using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Net.Mail;
using System.Web;

namespace TayaIT.Enterprise.EMadbatah.Config
{
    public class AppConfig
    {
        private static string confFilePath = Path.Combine(HttpContext.Current.Server.MapPath(@"~\" + Constants.WebSiteFolderNames.APP_DATA), Constants.FileNames.APP_CONFIG);
        private XmlDocument _confDoc = null;

        public AppConfig()
        {
            _confDoc = new XmlDocument();
            _confDoc.Load(confFilePath);

            EmailServer emailServer = new EmailServer();
            MainAdmin = new EMadbatahUser();

            XmlNode defAdminDomainUserNameNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/DefaultAdmin/DomainUserName");
            if (defAdminDomainUserNameNode != null && !string.IsNullOrEmpty(defAdminDomainUserNameNode.InnerText))
                MainAdmin.DomainUserName = defAdminDomainUserNameNode.InnerText;

            XmlNode defAdminNameNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/DefaultAdmin/Name");
            if (defAdminNameNode != null && !string.IsNullOrEmpty(defAdminNameNode.InnerText))
                MainAdmin.Name = defAdminNameNode.InnerText;

            XmlNode defAdminEmailNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/DefaultAdmin/Email");
            if (defAdminEmailNode != null && !string.IsNullOrEmpty(defAdminEmailNode.InnerText))
                MainAdmin.Email = defAdminEmailNode.InnerText;


            XmlNode hostNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Host");
            if (hostNode != null && !string.IsNullOrEmpty(hostNode.InnerText))
                emailServer.Host = hostNode.InnerText;

            XmlNode portNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Port");
            if (portNode != null && !string.IsNullOrEmpty(portNode.InnerText.Trim()))
                emailServer.Port = int.Parse(portNode.InnerText.Trim());

            XmlNode emailUserNameNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/UserName");
            if (emailUserNameNode != null && !string.IsNullOrEmpty(emailUserNameNode.InnerText.Trim()))
                emailServer.UserName = emailUserNameNode.InnerText.Trim();

            XmlNode emailPasswordNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Password");
            if (emailPasswordNode != null && !string.IsNullOrEmpty(emailPasswordNode.InnerText.Trim()))
                emailServer.Password = emailPasswordNode.InnerText.Trim();

            bool enableSSL;
            XmlNode enableSSLNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/EnableSSL");
            if (enableSSLNode != null && !string.IsNullOrEmpty(enableSSLNode.InnerText.Trim()) && bool.TryParse(enableSSLNode.InnerText.Trim(), out enableSSL))
                emailServer.EnableSSL = enableSSL;

            SmtpDeliveryMethod deliveryMode = SmtpDeliveryMethod.Network;
            XmlNode deliveryMethodNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/DeliveryMethod");
            if (deliveryMethodNode != null && !string.IsNullOrEmpty(deliveryMethodNode.InnerText.Trim()) && Enum.TryParse(enableSSLNode.InnerText.Trim(), out deliveryMode))
                emailServer.DeliveryMethod = deliveryMode;

            EmailServerSettings = emailServer;

            XmlNode vecSysServerPathNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/VecSysServerPath");
            if (vecSysServerPathNode != null && !string.IsNullOrEmpty(vecSysServerPathNode.InnerText.Trim()))
                VecSysServerPath = vecSysServerPathNode.InnerText.Trim();

            XmlNode audioServerPathNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/AudioServerPath");
            if (audioServerPathNode != null && !string.IsNullOrEmpty(audioServerPathNode.InnerText.Trim()))
                AudioServerPath = audioServerPathNode.InnerText.Trim();

            XmlNode eParlimrntWebServiceURLNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EParlimrntWebServiceURL");
            if (eParlimrntWebServiceURLNode != null && !string.IsNullOrEmpty(eParlimrntWebServiceURLNode.InnerText.Trim()))
                EPrlimentServerURL = eParlimrntWebServiceURLNode.InnerText.Trim();

            bool enableEmailNotifications = true;
            XmlNode enableEmailNotificationsNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/EnableEmailNotifications");
            if (enableEmailNotificationsNode != null && !string.IsNullOrEmpty(enableEmailNotificationsNode.InnerText.Trim()) && bool.TryParse(enableEmailNotificationsNode.InnerText.Trim(), out enableEmailNotifications))
                EnableEmailNotifications = enableEmailNotifications;

            XmlNode prefCookieNameNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/Cookies/PrefCookieName");
            if (prefCookieNameNode != null && !string.IsNullOrEmpty(prefCookieNameNode.InnerText.Trim()))
                PrefCookieName = prefCookieNameNode.InnerText.Trim();

            bool isOfflineDevMode = false;
            XmlNode offlineDevModeNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/OfflineDevMode");
            if (offlineDevModeNode != null && !string.IsNullOrEmpty(offlineDevModeNode.InnerText.Trim()) && bool.TryParse(offlineDevModeNode.InnerText.Trim(), out isOfflineDevMode))
                IsOfflineDevMode = isOfflineDevMode;

            XmlNode allowedDomainNameNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/AllowedDomainName");
            if (allowedDomainNameNode != null && !string.IsNullOrEmpty(allowedDomainNameNode.InnerText.Trim()))
                AllowedDomainName = allowedDomainNameNode.InnerText.Trim();


            bool isCodeExecTraceEnabled = false;
            XmlNode isCodeExecTraceEnabledNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/Logging/CodeExecTraceEnabled");
            if (isCodeExecTraceEnabledNode != null && !string.IsNullOrEmpty(isCodeExecTraceEnabledNode.InnerText.Trim()) && bool.TryParse(isCodeExecTraceEnabledNode.InnerText.Trim(), out isCodeExecTraceEnabled))
                IsCodeExecTraceEnabled = isCodeExecTraceEnabled;

            bool isErrorLoggingEnabled = false;
            XmlNode isErrorLoggingEnabledNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/Logging/ErrorLoggingEnabled");
            if (isErrorLoggingEnabledNode != null && !string.IsNullOrEmpty(isErrorLoggingEnabledNode.InnerText.Trim()) && bool.TryParse(isErrorLoggingEnabledNode.InnerText.Trim(), out isErrorLoggingEnabled))
                IsErrorLoggingEnabled = isErrorLoggingEnabled;           
 

        }

        public string AllowedDomainName { get; set; }
        public string VecSysServerPath { get; set; }
        public string AudioServerPath { get; set; }
        public string EPrlimentServerURL { get; set; }
        public bool EnableEmailNotifications { get; set; }
        public string PrefCookieName { get; set; }
        public bool IsCodeExecTraceEnabled { get; set; }
        public bool IsOfflineDevMode { get; set; }
        public bool IsErrorLoggingEnabled { get; set; }
        public EMadbatahUser MainAdmin { get; set; }

        public EmailServer EmailServerSettings { get; set; }

        //private List<string> _mainAdmins;
        //public List<string> MainAdmins
        //{
        //    get 
        //    {
        //        if (_mainAdmins != null)
        //            return _mainAdmins;
        //        else
        //        {
        //            string mainAdminsStr = ConfigManager.Get<string>(Constants.Security.MAIN_ADMINS).ToLower();
        //            _mainAdmins = mainAdminsStr.Split(",".ToCharArray()).ToList<string>();
        //            return _mainAdmins;
        //        }
        //    }
        //}

        //private List<string> _explicitMainAdmins;
        //public List<string> ExplicitMainAdmins
        //{
        //    get 
        //    {
        //        if (_explicitMainAdmins != null)
        //            return _explicitMainAdmins;
        //        else
        //        {
        //            string explicitMainAdminsStr = ConfigManager.Get<string>(Constants.Security.EXPLICIT_MAIN_ADMINS).ToLower();
        //            _explicitMainAdmins = explicitMainAdminsStr.Split(",".ToCharArray()).ToList<string>();
        //            return _explicitMainAdmins;
        //        }
        //    }
        //}

        //private string _allowedDomainName;
        //public string AllowedDomainName
        //{
        //    get
        //    {
        //        if (_allowedDomainName != null)
        //            return _allowedDomainName;
        //        else
        //        {
        //            _allowedDomainName = ConfigManager.Get<string>(Constants.Security.ALLOWED_DOMAIN_NAME).ToLower();
        //            return _allowedDomainName;
        //        }
        //    }
        //}

        private string _rootUrl;
        public string RootUrl
        {
            get
            {
                if (_rootUrl != null)
                    return _rootUrl;
                else
                {
                    _rootUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);//ConfigManager.Get<string>(Constants.Settings.ROOT_URL).ToLower();
                    return _rootUrl;
                }
            }
        }

        public bool SaveChanges() 
        {
            try
            {
                if (!string.IsNullOrEmpty(VecSysServerPath))
                {
                    //XmlNode vecSysServerPathNode = _confDoc.SelectSingleNode("/Emadbatah/Conf/VecSysServerPath");
                    //XmlNode newnode = vecSysServerPathNode.CloneNode(true);
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/VecSysServerPath").InnerText = VecSysServerPath;
                }

                if(!string.IsNullOrEmpty(AudioServerPath))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/AudioServerPath").InnerText = AudioServerPath;

                if (!string.IsNullOrEmpty(EPrlimentServerURL))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/EParlimrntWebServiceURL").InnerText = EPrlimentServerURL;


                _confDoc.SelectSingleNode("/Emadbatah/Conf/EnableEmailNotifications").InnerText = EnableEmailNotifications.ToString();

                if (!string.IsNullOrEmpty(PrefCookieName))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/Cookies/PrefCookieName").InnerText = PrefCookieName;

                _confDoc.SelectSingleNode("/Emadbatah/Conf/Logging/CodeExecTraceEnabled").InnerText = IsCodeExecTraceEnabled.ToString();
                _confDoc.SelectSingleNode("/Emadbatah/Conf/OfflineDevMode").InnerText = IsOfflineDevMode.ToString();
                _confDoc.SelectSingleNode("/Emadbatah/Conf/Logging/ErrorLoggingEnabled").InnerText = IsErrorLoggingEnabled.ToString();
                
                if(!string.IsNullOrEmpty(AllowedDomainName))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/AllowedDomainName").InnerText = AllowedDomainName;

                if (!string.IsNullOrEmpty(EmailServerSettings.Host))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Host").InnerText = EmailServerSettings.Host;
                if (EmailServerSettings.Port > 0)
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Port").InnerText = EmailServerSettings.Port.ToString();
                if (!string.IsNullOrEmpty(EmailServerSettings.UserName))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/UserName").InnerText = EmailServerSettings.UserName;
                if (!string.IsNullOrEmpty(EmailServerSettings.Password))
                    _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/Password").InnerText = EmailServerSettings.Password;

                _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/EnableSSL").InnerText = EmailServerSettings.EnableSSL.ToString();
                _confDoc.SelectSingleNode("/Emadbatah/Conf/EmailServer/DeliveryMethod").InnerText = EmailServerSettings.DeliveryMethod.ToString();


                _confDoc.Save(confFilePath);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.Config.EMadbatahAppConfig.SaveChanges()");
            }

            return true;
        }

        private static AppConfig _appConfInstance = null;
        public static AppConfig GetInstance()
        {
            if (_appConfInstance != null)
                return _appConfInstance;
            else
            {
                _appConfInstance = new AppConfig();
                return _appConfInstance;
            }
        }

        public static AppConfig GetInstance(bool reinstantiate)
        {
            _appConfInstance = null;
            return GetInstance();
        }


    }
}
