using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using System.Diagnostics;

namespace TayaIT.Enterprise.EMadbatah.Util
{
    public static class LogHelper
    {
        public static void LogException(Exception ex, string classFullQualifiedName)
        {
            if (ex != null)
            {
                LogEntry log = new LogEntry();
                log.EventId = 1;
                log.TimeStamp = DateTime.Now;
                log.Title = ex.Message;
                log.Categories.Add(classFullQualifiedName);
                log.Message = PrepareExceptionString(ex);
                log.Severity = TraceEventType.Error;
                Logger.Write(log);
            }
        }
        public static void LogException(Exception ex, string classFullQualifiedName, string customMessage)
        {
            if (ex != null)
            {
                LogEntry log = new LogEntry();
                log.EventId = 1;
                log.TimeStamp = DateTime.Now;
                log.Title = ex.Message;
                log.Categories.Add(classFullQualifiedName);
                log.Message = customMessage + "\r\n+Exception Message:\r\n" + ex.Message + "\r\n+Exception Stack Trace:\r\n" + ex.StackTrace;
                log.Severity = TraceEventType.Error;
                Logger.Write(log);
            }
        }
        private static string PrepareExceptionString(Exception ex)
        {
            StringBuilder sbException = null;

            sbException = new StringBuilder();


            sbException.AppendFormat
                ("<SOURCE>{0}</SOURCE><STACKTRACE>{1}</STACKTRACE>\r\n",
                ex.Source,
                ex.StackTrace
                );

            if (ex.InnerException != null)
            {
                sbException.AppendFormat("<INNEREXCEPTION><INNERMESSAGE>{0}</INNERMESSAGE><SOURCE>{1}</SOURCE><STACKTRACE>{2}</STACKTRACE></INNEREXCEPTION>\r\n",
                    ex.InnerException.Message,
                    ex.InnerException.Source,
                    ex.InnerException.StackTrace);
            }

            return sbException.ToString();
        }

        public static void LogMessage(string message, string classFullQualifiedName, TraceEventType eventType)
        {
            LogEntry log = new LogEntry();
            log.EventId = 2;
            log.Message = message;
            log.Title = message;
            log.TimeStamp = DateTime.Now;
            log.Categories.Add(classFullQualifiedName);
            log.Severity = eventType;
            Logger.Write(log);
        }

        public static void LogCodeExecTrace(string className, SiteSecion siteSection, string xmlQuery, string xmlResponse, string exaleadError, string catchedError)
        {
            StringBuilder message = new StringBuilder();
            if (ConfigManager.Get<bool>("CodeExecTrace"))
            {
                string fileName = HttpContext.Current.Server.MapPath(@"~\" + "_logs") + "\\codeEcexPathtrace_" + siteSection + DateTime.Now.ToString("yy-MM-dd") + ".txt";
                StreamWriter psw = new StreamWriter(fileName, true);
                psw.WriteLine("date/time : " + DateTime.Now.ToString("HH:mm:ss | dd-MM-yy"));
                psw.WriteLine("ClassName   : " + className);
                psw.WriteLine("xml query req : " + xmlQuery);
                psw.WriteLine("xml response  : " + xmlResponse);
                psw.WriteLine("exalead error : " + exaleadError);
                psw.WriteLine("catched error : " + catchedError);
                psw.WriteLine(" ------------------------------------------- ");
                psw.WriteLine(" ");
                psw.Close();
            }
        }

    }
}
