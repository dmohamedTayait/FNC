using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class Email
    {
        public Email(string fromEmail, string fromName, List<Emailreceptionist> receptionists, string subject, string body)
        {
            FromEmail = fromEmail;
            FromName = fromName;
            Receptionists = receptionists;
            Subject = subject;
            Body = body;

        }

        public Email(List<Emailreceptionist> receptionists)
        {
            Receptionists = receptionists;
        }

        public Email(Emailreceptionist receptionist)
        {
            Receptionists = new List<Emailreceptionist>();
            Receptionists.Add(receptionist);
        }

        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public List<Emailreceptionist> Receptionists { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }


    }

    public class Emailreceptionist 
    {
        public Emailreceptionist(string toEmail, string toName)
        {
            ToEmail = toEmail;
            ToName = toName;
        }
        public string ToEmail { get; set; }
        public string ToName { get; set; }
    }
}
