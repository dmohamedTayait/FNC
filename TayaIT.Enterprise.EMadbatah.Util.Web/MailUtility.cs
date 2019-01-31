using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Net;

namespace TayaIT.Enterprise.EMadbatah.Util.Web
{
    public class MailUtility
    {
        private SmtpClient _smtpClient = null;
        private MailMessage _message = null;
        
        public MailUtility(EmailServer server)
        {
            _smtpClient = new SmtpClient();
            _smtpClient.Host = server.Host;

            NetworkCredential credentials = new System.Net.NetworkCredential(server.UserName, server.Password);
            _smtpClient.UseDefaultCredentials = false;
            _smtpClient.Credentials = credentials;
            _smtpClient.Port = server.Port;
            _smtpClient.EnableSsl = server.EnableSSL;
            _smtpClient.DeliveryMethod = server.DeliveryMethod;

           
        }


        public bool SendMail(Email email)
        {
            try
            {
                Succeed = false;
                _message = new MailMessage();
                _message.Sender = new MailAddress(email.FromEmail, email.FromName);

                foreach (Emailreceptionist rec in email.Receptionists)
                {
                    MailAddress address = new MailAddress(rec.ToEmail, rec.ToName);
                    if (!_message.To.Contains(address))
                        _message.To.Add(address);                    
                }

                _message.Subject = email.Subject;
                _message.From = new MailAddress(email.FromEmail, email.FromName);
                _message.Body = email.Body;
                _message.BodyEncoding = Encoding.UTF8;
                _message.IsBodyHtml = true;
                _message.Priority = MailPriority.High;
                _smtpClient.Send(_message);

                Succeed = true;

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.Util.Web.MailUtility.SendMail()");
                ErrorMsg = ex.Message;
                Succeed = false;
            }

            return Succeed;
        }

        public bool Succeed { get; set; }
        public string ErrorMsg { get; set; }
    }
}
