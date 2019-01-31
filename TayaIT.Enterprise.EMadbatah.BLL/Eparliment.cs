using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Model;
using System.ServiceModel;
using TayaIT.Enterprise.EMadbatah.Config;
using System.Data;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Util;
using System.ServiceModel.Security;

namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public class Eparliment
    {
        

        EPServiceClient.EPServiceClient _client = null;

        public Eparliment()
        {
            dynamic binding = new BasicHttpBinding();
	        binding.Name = "BasicHttpBinding_IEPService";
	        binding.CloseTimeout = TimeSpan.FromMinutes(2);
	        binding.OpenTimeout = TimeSpan.FromMinutes(2);
	        binding.ReceiveTimeout = TimeSpan.FromMinutes(10);
	        binding.SendTimeout = TimeSpan.FromMinutes(10);
	        binding.AllowCookies = false;
	        binding.BypassProxyOnLocal = false;
	        binding.HostNameComparisonMode = HostNameComparisonMode.StrongWildcard;
            binding.MaxReceivedMessageSize = 200000000;
            binding.MaxBufferSize = 200000000;
            binding.MaxBufferPoolSize = 200000000;
	        binding.MessageEncoding = WSMessageEncoding.Text;
	        binding.TextEncoding = System.Text.Encoding.UTF8;
	        binding.TransferMode = TransferMode.Buffered;
	        binding.UseDefaultWebProxy = true;

	        binding.ReaderQuotas.MaxDepth = 32;
            binding.ReaderQuotas.MaxStringContentLength = 2097152;
            binding.ReaderQuotas.MaxArrayLength = 2097152;
	        binding.ReaderQuotas.MaxBytesPerRead = 4096;
	        binding.ReaderQuotas.MaxNameTableCharCount = 16384;

	        binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
	        binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Windows;
	        binding.Security.Transport.ProxyCredentialType = HttpProxyCredentialType.None;
	        binding.Security.Transport.Realm = "";
	        binding.Security.Message.ClientCredentialType = BasicHttpMessageCredentialType.UserName;
            binding.Security.Message.AlgorithmSuite = SecurityAlgorithmSuite.Default;
            //binding.Security.
//            binding.
//            <security mode="TransportCredentialOnly">
//  <transport clientCredentialType="Windows" proxyCredentialType="None" realm="" />
//  <message clientCredentialType="UserName" algorithmSuite="Default" />
//</security>


	       // binding.Security.Message.AlgorithmSuite = System.Security.SecurityAlgorithmSuite.Default;

	        //Define the endpoint address'
	        dynamic endpointStr = AppConfig.GetInstance().EPrlimentServerURL;
	        dynamic endpoint = new EndpointAddress(endpointStr);
	        //Instantiate the SOAP client using the binding and endpoint'
	        //that were defined above'
	        _client = new EPServiceClient.EPServiceClient(binding, endpoint);
        }

        public SessionDetails GetSessionDetails(int epSessionID)
        {
            DataSet dsSessionDetails =  _client.GetEParlimentSessionDetails(epSessionID);
            if (dsSessionDetails != null)
            {
                return SessionDetailsFromDataSet(epSessionID, dsSessionDetails);
            }
            else
                return null;

            //consume webservice
        }

        public bool UpdateSessionDetails(long sid)
        {
            DAL.Session session = DAL.SessionHelper.GetSessionByID(sid);
            if (session != null)
            {
                DataSet dsSessionDetails = _client.UpdateEParlimentSessionDetails(session.EParliamentID);
                if (dsSessionDetails != null)
                {
                    SessionDetails sd = SessionDetailsFromDataSet(session.EParliamentID, dsSessionDetails);
                    EMadbatahFacade.UpdateSessionDetailsToDB(sd);
                    return true;
                }
                else
                    return false;
            }
            else
                return false;

            //consume webservice
        }



        public bool IngestContentsForFinalApprove(long sessionID)
        {
            try
            {
                return _client.IngestContentsForFinalApprove(sessionID);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.BLL.Eparliment.IngestContentsForFinalApprove(" + sessionID  + ")");
                return false;
            }
            
            
        }

        private SessionDetails SessionDetailsFromDataSet(int sessionEparlimentID, DataSet epData)
        {
            DataTable tblSessionDetails = epData.Tables[Constants.SESSION_DETAILS_TBL];
            DataTable tblSessionAttendant = epData.Tables[Constants.SESSION_ATTENDANT_TBL];
            DataTable tblSessionAgendaItems = epData.Tables[Constants.SESSION_AGENDA_ITEMS_TBL];

            Hashtable agendaItems = new Hashtable();
            List<SessionAgendaItem> agendaItems2 = new List<SessionAgendaItem>();
            int i = 0;
 
            foreach (DataRow row in tblSessionAgendaItems.Rows)
            {

                int? id = int.Parse(row[Constants.AgendaItemFields.ITEM_ID].ToString());
                i = i - 1;
                int? idParent = row[Constants.AgendaItemFields.PARENT_ITEM_ID] != System.DBNull.Value ? int.Parse(row[Constants.AgendaItemFields.PARENT_ITEM_ID].ToString()) : i;
                string title = row[Constants.AgendaItemFields.ITEM_TITLE].ToString();
                string titleParent = row[Constants.AgendaItemFields.PARENT_ITEM_TITLE].ToString();
                
                int order = int.Parse(row[Constants.AgendaItemFields.ITEM_ORDER].ToString());
                string qFrom = row[Constants.AgendaItemFields.QUESTION_FROM].ToString();
                string qTo = row[Constants.AgendaItemFields.QUESTION_TO].ToString();

                if (idParent == i || !agendaItems.ContainsKey(idParent))
                {
                    //usama - ibrahim, 23/12/2012
                    //agendaItems.Add(idParent, new SessionAgendaItem(id, idParent, idParent < 0 ? title : titleParent, order, qFrom, qTo));
                    //agendaItems.Add(idParent, new SessionAgendaItem(idParent, null, idParent < 0 ? title : titleParent, order, qFrom, qTo));
                    agendaItems.Add(idParent, new SessionAgendaItem(idParent < 0 ? id : idParent, null, idParent < 0 ? title : titleParent, order, qFrom, qTo));
                }
                if (idParent > 0)
                {
                    SessionAgendaItem sai = (SessionAgendaItem)agendaItems[idParent];
                    sai.SubAgendaItems.Add(new SessionAgendaItem(id, idParent, title, order, qFrom, qTo));
                    agendaItems[idParent] = sai;
                }
            }

            List<SessionAttendant> attendants = new List<SessionAttendant>();
            foreach (DataRow row in tblSessionAttendant.Rows)
            {
                int id = int.Parse(row[Constants.AttendantFields.ATTENDANT_ID].ToString());
                string name = row[Constants.AttendantFields.NAME].ToString();

                string state = row[Constants.AttendantFields.ATTENDANT_STATE].ToString();
                AttendantState stateEnum = AttendantState.Attended;
                if(Enum.IsDefined(typeof(AttendantState), state))
                    stateEnum = (AttendantState)Enum.Parse(typeof(AttendantState),state);

                string type = row[Constants.AttendantFields.ATTENDANT_TYPE].ToString().Replace("/","");
                AttendantType typeEnum = AttendantType.FromTheCouncilMembers;
                if(Enum.IsDefined(typeof(AttendantType), type))
                    typeEnum = (AttendantType)Enum.Parse(typeof(AttendantType),type);

                string jobTitle = row[Constants.AttendantFields.JOB_TITLE].ToString();
                string firstName = row[Constants.AttendantFields.FIRST_NAME].ToString();
                string secondName = row[Constants.AttendantFields.SECOND_NAME].ToString();
                string tribeName = row[Constants.AttendantFields.TRIBE_NAME].ToString();
                attendants.Add(new SessionAttendant(id,name,jobTitle, stateEnum, typeEnum,firstName, secondName,tribeName,1985));               
            }
            //if (tblSessionDetails.Rows.Count == 0)
            //    return null;

            string sessiontype = "";
            string president = "";
            string place = "";
            int season = 0;
            int stage = 0;
            string stageType = "";
            string subject = "";
            long serial = 0;
            DateTime date = DateTime.Now;
            DateTime dateHijri = DateTime.Now;
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;
            if (tblSessionDetails.Rows.Count > 0)
            {
                DataRow sessionDetailsRow = tblSessionDetails.Rows[0];
                serial = long.Parse(sessionDetailsRow[Constants.SessionDetailsFields.SERIAL].ToString());
                 date = DateTime.Parse(sessionDetailsRow[Constants.SessionDetailsFields.DATE].ToString());
                 dateHijri = DateTime.Parse(sessionDetailsRow[Constants.SessionDetailsFields.DATE].ToString());
                 try
                 {
                     startTime = DateTime.Parse(sessionDetailsRow[Constants.SessionDetailsFields.START_TIME].ToString());
                     startTime = new DateTime(DateTime.Now.Year, startTime.Month, startTime.Day, startTime.Hour, startTime.Minute, startTime.Second);
                 }
                 catch {
                     startTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                 }
                 try
                 {
                     DateTime tmpEndTime = DateTime.Now;

                     DateTime.TryParse(sessionDetailsRow[Constants.SessionDetailsFields.END_TIME].ToString(), out tmpEndTime);

                     endTime = tmpEndTime;
                     endTime = new DateTime(DateTime.Now.Year, endTime.Month, endTime.Day, endTime.Hour, endTime.Minute, endTime.Second);
                 }
                 catch {
                     endTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                 }
                // new DateTime(2014, 1, 21, 9, 0, 0);
                 sessiontype = sessionDetailsRow[Constants.SessionDetailsFields.SESSION_TYPE].ToString();
                 president = sessionDetailsRow[Constants.SessionDetailsFields.PRESIDENT].ToString();
                 place = sessionDetailsRow[Constants.SessionDetailsFields.PLACE].ToString();
                 season = int.Parse(sessionDetailsRow[Constants.SessionDetailsFields.SEASON].ToString());
                 stage = int.Parse(sessionDetailsRow[Constants.SessionDetailsFields.STAGE].ToString());
                 stageType = sessionDetailsRow[Constants.SessionDetailsFields.STAGE_TYPE].ToString();
                 subject = sessionDetailsRow[Constants.SessionDetailsFields.SUBJECT].ToString();
            }
            SessionDetails sd = new SessionDetails(
                sessionEparlimentID,
                serial,
                date,
                dateHijri,
                startTime,
                endTime,
                sessiontype,
                president,
                place,
                season,
                stage,
                stageType,
                attendants,
                agendaItems,
                SessionStatus.New, 
                subject);

            return sd;

            //consume webservice
        }


        //public SessionDetails FinalApproveSession(SessionDetails, )
        //{
        //    //consume webservice
        //}

        private static class Constants
        {
            public static class SessionDetailsFields
            {
                public const string SESSION_ID = "SessionID";
                public const string SERIAL = "serial";
                public const string DATE = "date";                
                public const string START_TIME= "StartTime";
                public const string END_TIME = "EndTime";
                public const string SESSION_TYPE = "Sessiontype";
                public const string PRESIDENT = "president";
                public const string PLACE = "place";
                public const string SEASON = "season";
                public const string STAGE = "stage";
                public const string STAGE_TYPE = "StageType";

                //new from excelfile v1.7
                public const string SUBJECT = "SessionSubject";
            }
            
            public static class AttendantFields
            {
                public const string ATTENDANT_ID = "AttendanceID";
                public const string SESSION_ID = "SessionID";
                public const string NAME = "name";
                public const string ATTENDANT_STATE = "attendantState";
                public const string ATTENDANT_TYPE = "attendantType";
                public const string JOB_TITLE = "JobTitle";
                public const string FIRST_NAME = "FirstName";
                public const string SECOND_NAME = "SecondName";
                public const string TRIBE_NAME = "TribeName";
            }

            public static class AgendaItemFields
            {
                public const string PARENT_ITEM_TITLE = "ParentItemTitle";
                public const string PARENT_ITEM_ID = "ParentItemID";
                public const string SESSION_ID = "SessionID";
                public const string ITEM_ID = "ItemID";
                public const string ITEM_TITLE = "ItemTitle";
                public const string ITEM_TYPE_ID = "ItemTypeID";
                public const string ITEM_TYPE_DESCRIPTION = "ItemTypeDescription";
                public const string ITEM_STATUS_ID = "ItemStatusID";
                public const string ITEM_STATUS_DESCRIPTION = "ItemStatusDescription";
                public const string AGENDA_ITEM_ID = "AgendaItemID";

                //new from excelfile v1.7
                public const string ITEM_ORDER = "ItemOrder";
                public const string QUESTION_FROM = "QFrom";
                public const string QUESTION_TO = "QTo";
            }



            public const string SESSION_DETAILS_TBL = "eMadbatah_SessionDetails";
            public const string SESSION_ATTENDANT_TBL = "eMadbatah_SessionAttendant";
            public const string SESSION_AGENDA_ITEMS_TBL = "eMadbatah_SessionAgendaItem";

        }

    }
}
