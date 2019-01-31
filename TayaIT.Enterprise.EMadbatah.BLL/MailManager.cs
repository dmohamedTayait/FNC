#region Header

// Copyright (c) Omar AL Zabir. All rights reserved.
// For continued development and updates, visit http://msmvps.com/omar

#endregion Header

/// <summary>
/// Summary description for MailManager
/// </summary>
using System;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Localization;
using System.Linq;

namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public static class MailManager
    {

        public static bool SendMail(Email email, SystemMailType mailType, Hashtable emailDate)
        {
            if(email.Receptionists != null && email.Receptionists.Count > 0 && string.IsNullOrEmpty(email.FromEmail ))
            {
                var recs = from rec in email.Receptionists
                           where !string.IsNullOrEmpty(rec.ToEmail)
                           select rec;
                if (recs.Count<Emailreceptionist>() == 0)
                {
                    return false;
                }
            }
            
            if(AppConfig.GetInstance().EnableEmailNotifications)
            {
                MailUtility util = new MailUtility(AppConfig.GetInstance().EmailServerSettings);

                string templateEmailMessage = HttpContext.Current.Application[Constants.EmailTemplateFileNames.GENERAL_EMAIL_HTML].ToString();
                string emailBody = HttpContext.Current.Application[mailType.ToString() + "EmailBody.txt"].ToString();
                string subject = LocalHelper.GetLocalizedString("strEmailSubj"+mailType.ToString());
                string fromName = LocalHelper.GetLocalizedString("strAppName");

                email.Subject = subject;
                email.FromEmail = AppConfig.GetInstance().EmailServerSettings.UserName;
                email.FromName = fromName;
                foreach (string key in emailDate.Keys)
	            {
                    emailBody = emailBody.Replace(key, (emailDate[key] == null) ? "" : emailDate[key].ToString());
	            }
                
                email.Body = templateEmailMessage.Replace("<%MainContent%>", emailBody);
                return util.SendMail(email);
            }

            return false;
                

        }

        public static bool SendMail(Email email, SystemMailType mailType, Hashtable emailDate, HttpContext context)
        {

            if (AppConfig.GetInstance().EnableEmailNotifications)
            {
                MailUtility util = new MailUtility(AppConfig.GetInstance().EmailServerSettings);

                string templateEmailMessage = context.Application[Constants.EmailTemplateFileNames.GENERAL_EMAIL_HTML].ToString();
                string emailBody = context.Application[mailType.ToString() + "EmailBody.txt"].ToString();
                string subject = LocalHelper.GetLocalizedString("strEmailSubj" + mailType.ToString());
                string fromName = LocalHelper.GetLocalizedString("strAppName");

                email.Subject = subject;
                email.FromEmail = AppConfig.GetInstance().EmailServerSettings.UserName;
                email.FromName = fromName;
                foreach (string key in emailDate.Keys)
                {
                    emailBody = emailBody.Replace(key, emailDate[key].ToString());
                }

                email.Body = templateEmailMessage.Replace("<%MainContent%>", emailBody);
                return util.SendMail(email);
            }

            return false;


        }
    }
}

