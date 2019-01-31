using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class EmailServer
    {
        public EmailServer() { }
        public EmailServer(string host, int port, string userName, string password, bool useSSL, SmtpDeliveryMethod smtpDeliveryMethod)
        {
            EnableSSL = useSSL;
            DeliveryMethod = smtpDeliveryMethod;
            Port = port;
            Host = host;
            UserName = userName;
            Password = password;
        }
        public bool EnableSSL { get; set; }
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public int Port { get; set; }
        public string Host { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
