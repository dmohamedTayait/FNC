using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Hosting;
using System.Data.OleDb;
using System.Configuration;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.EParlimentService
{
    public class EPDatabaseHelper
    {
        //Will be Implemented by Ahmed AbdelMotteleb FNC 
        public static bool InsertIntoEparliament(EparliamentSegment seg)
        {
            using(FNCDBEntities context = new FNCDBEntities())
            {
                SessionDiary diary = new SessionDiary();
                diary.CreatedBy = seg.CreatorBy;
                diary.CreatedDate = seg.CreatedDate;
                diary.CreatorID = seg.CreatorBy;
                diary.DiaryDate = seg.DiaryDate;
                diary.DiaryNumber = seg.DiaryNumber;
                diary.EndTime = seg.EndTime;
                //diary.EntityKey = seg.
                //diary.EntityState = seg.
                diary.InterventionOwnerID = seg.AttendantID;
                diary.MainItemID = seg.AgendaItemId;
                diary.SessionID = seg.SessionID;
                diary.StartTime = seg.StartTime;
                diary.SubItemID = seg.AgendaSubItemID;
                diary.Subject = seg.Subject;
                diary.Text = seg.Text;
                diary.UpdatedBy = seg.UpdatedBy;
                diary.UpdatedDate = seg.UpdatedDate;

                context.SessionDiaries.AddObject(diary);
                context.SaveChanges();
            }
            
            //return true on sucess, false on fail
            return true;
        }

        //Will be Implemented by Ahmed AbdelMotteleb FNC 
        private static Hashtable _epusers;
        public static int? GetUserEparliamentIDByEmail(string email)
        {
                Hashtable tblUsers = GetEparliamentUsers();
                if (tblUsers != null && tblUsers[email] != null)
                    return int.Parse(GetEparliamentUsers()[email].ToString()); //should select from Eparliament table by email and return the eparliament id 
                else
                    return null;
            
        }
        private static Hashtable GetEparliamentUsers()
        {
            //key email value epid
            if (_epusers == null)
            {
                try
                {
                    //fill hashtable
                    string connectionString = ConfigurationManager.ConnectionStrings["FNCDBConn"].ConnectionString;
                    DataSet ds = new DataSet();

                    string sessiondetailssqlstr = "";
                    sessiondetailssqlstr = "SELECT ID, Email from Users";

                    DataTable tblEparliamentUsers = new DataTable("EparliamentUsers");
                    tblEparliamentUsers.Columns.Add("ID", typeof(int));
                    tblEparliamentUsers.Columns.Add("Email", typeof(string));

                    ds.Tables.Add(tblEparliamentUsers);

                    var adapter = new SqlDataAdapter(sessiondetailssqlstr, connectionString);
                    adapter.Fill(ds, "EparliamentUsers");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string key = row["Email"].ToString();
                        if (!string.IsNullOrEmpty(key) && !key.Contains("@") && !_epusers.ContainsKey(key))
                            _epusers.Add(key, row["ID"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    LogHelper.LogException(ex, "Failed to fill Ep users hashtable");
                }
            }
            else
                return _epusers;
            return null; 
        }
        public static DataSet GetSessionDetailsFromDB(long sessionID)
        {           
            string connectionString = ConfigurationManager.ConnectionStrings["FNCDBConn"].ConnectionString;
            DataSet ds = PrepareResultDataSet();

            string sessiondetailssqlstr = "";
            sessiondetailssqlstr = "SELECT     SessionID AS eparlimentID, SessionSequentialNumber AS serial, Subject AS SessionSubject, SessionDate AS date, StartTime, EndTime, SessionStatusDescription AS type, "
                  + " PresidentName AS president, EmirateDescription AS place, TermDescription AS season, RoundDescription AS stage, "
                  + " CASE WHEN IsNormalRound = 1 THEN 'عادي' ELSE 'غير عادي' END AS StageType "
                  + " FROM     _WS_SessionsVW where SessionID =";
            var adapter = new SqlDataAdapter(sessiondetailssqlstr + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_DETAILS_TBL);

            sessiondetailssqlstr = "SELECT      SessionID AS eparlimentID, AttendeeDescription AS name, FirstName AS FirstName , SecondName AS SecondName, TribeName AS TribeName, AttendeeID AS AttendanceID, "
                   + " CASE WHEN AttendeeStatusID = '1' THEN 'Attended' WHEN AttendeeStatusID = '2' THEN 'Apology' WHEN AttendeeStatusID = '3' THEN 'Absent' WHEN AttendeeStatusID "
                   + " = '5' THEN 'InMission' ELSE 'NA' END AS attendantState, "
                   + " CASE WHEN RelationTypeID = '1' THEN 'President' WHEN RelationTypeID = '2' THEN 'FromTheCouncilMembers' WHEN RelationTypeID = '4'"
                   + " THEN 'FromOutsideTheCouncil' WHEN RelationTypeID = '3' THEN 'Secretariat' WHEN RelationTypeID = '19' THEN 'GovernmentRepresentative' ELSE 'NA' END AS attendantType"
                   + " FROM       _WS_SessionsEnitiesVW where SessionID =";
            adapter = new SqlDataAdapter(sessiondetailssqlstr + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_ATTENDANT_TBL);

            sessiondetailssqlstr = "SELECT     SessionID, ParentItemID, ParentItemTitle, ItemID, ItemTitle, ItemTypeID, ItemTypeDescription, ItemStatusID, ItemStatusDescription, AgendaItemID, "
                  + " IndexOrder AS ItemOrder, dbo.GetQuestionSubmittedFromByQuestionID(TopicID) AS QTo,  dbo.GetQuestionSubmittedToByQuestionID(TopicID) "
                  + " AS QTo FROM         dbo._WS_SessionsAgendaItemsVW WHERE    ( (NOT (ParentItemID IS NULL)) OR (ItemTypeID = 1))"
                  + " and SessionID = " + sessionID
                  + " ORDER BY ItemOrder";

            adapter = new SqlDataAdapter(sessiondetailssqlstr, connectionString);
            adapter.Fill(ds, Constants.SESSION_AGENDA_ITEMS_TBL);

            return ds;
        }


        public static DataSet UpdateSessionDetailsFromDB(long sessionID)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["FNCDBConn"].ConnectionString;
            DataSet ds = PrepareResultDataSet();
            string sessiondetailssqlstr = "";

            sessiondetailssqlstr = "SELECT      SessionID AS eparlimentID, AttendeeDescription AS name, FirstName, SecondName, TribeName, AttendeeID AS AttendanceID, "
                   + " CASE WHEN AttendeeStatusID = '1' THEN 'Attended' WHEN AttendeeStatusID = '2' THEN 'Apology' WHEN AttendeeStatusID = '3' THEN 'Absent' WHEN AttendeeStatusID "
                   + " = '5' THEN 'InMission' ELSE 'NA' END AS attendantState, "
                   + " CASE WHEN RelationTypeID = '1' THEN 'President' WHEN RelationTypeID = '2' THEN 'FromTheCouncilMembers' WHEN RelationTypeID = '4'"
                   + " THEN 'FromOutsideTheCouncil' WHEN RelationTypeID = '3' THEN 'Secretariat' WHEN RelationTypeID = '19' THEN 'GovernmentRepresentative' ELSE 'NA' END AS attendantType"
                   + " FROM       _WS_SessionsEnitiesVW where SessionID =";
            var adapter = new SqlDataAdapter(sessiondetailssqlstr + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_ATTENDANT_TBL);

            sessiondetailssqlstr = "SELECT     SessionID, ParentItemID, ParentItemTitle, ItemID, ItemTitle, ItemTypeID, ItemTypeDescription, ItemStatusID, ItemStatusDescription, AgendaItemID, "
                  + " IndexOrder AS ItemOrder, dbo.GetQuestionSubmittedFromByQuestionID(TopicID) AS QTo,  dbo.GetQuestionSubmittedToByQuestionID(TopicID) "
                  + " AS QTo FROM         dbo._WS_SessionsAgendaItemsVW WHERE    ( (NOT (ParentItemID IS NULL)) OR (ItemTypeID = 1))"
                  + " and SessionID = " + sessionID
                  + " ORDER BY ItemOrder";

            adapter = new SqlDataAdapter(sessiondetailssqlstr, connectionString);
            adapter.Fill(ds, Constants.SESSION_AGENDA_ITEMS_TBL);

            return ds;
        }


        public static DataSet GetSessionDetailsFromExcel(long sessionID)
        {
            var fileName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, "App_Data", "eMadbatah_Sample_Data_V1.7.xls");//string.Format("{0}\\App_Data", Directory.GetCurrentDirectory());
            var connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0; data source={0}; Extended Properties=Excel 8.0;", fileName);

            DataSet ds = PrepareResultDataSet();            

            var adapter = new OleDbDataAdapter("SELECT * FROM [eMadbatah_SessionDetails$] where SessionID = " + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_DETAILS_TBL);

            adapter = new OleDbDataAdapter("SELECT * FROM [eMadbatah_SessionAttendant$] where SessionID = " + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_ATTENDANT_TBL);

            adapter = new OleDbDataAdapter("SELECT * FROM [eMadbatah_SessionAgendaItem$] where SessionID = " + sessionID, connectionString);
            adapter.Fill(ds, Constants.SESSION_AGENDA_ITEMS_TBL);


            //foreach (DataRow row in ds.Tables[Constants.SESSION_DETAILS_TBL].Rows)
            //{
            //    if ((row[Constants.SessionDetailsFields.SERIAL] != null)
            //        && !string.IsNullOrEmpty(row[Constants.SessionDetailsFields.SERIAL].ToString()))
            //    {

            //        long tmpLong = -1;
            //        string tmpRes = "";
            //        string tmpVal = row[Constants.SessionDetailsFields.SERIAL].ToString();
            //        if (tmpVal.Contains("/"))
            //        {
            //            if (tmpVal.Split("/".ToCharArray()).Length > 0)
            //            {
            //                tmpRes = tmpVal.Split("/".ToCharArray())[0];
            //                if (!string.IsNullOrEmpty(tmpRes) && long.TryParse(tmpRes, out tmpLong))
            //                {
            //                    ds.Tables[Constants.SESSION_DETAILS_TBL].Rows[0][Constants.SessionDetailsFields.SERIAL] = tmpRes;
            //                }
            //            }
            //        }
            //    }

            //}

            return ds;
        }

        private static DataSet PrepareResultDataSet()
        {
            var ds = new DataSet();
            DataTable tblSessionDetails = new DataTable(Constants.SESSION_DETAILS_TBL);
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.DATE, typeof(DateTime));
            //tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.DATE_HIJRI, typeof(DateTime));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.END_TIME, typeof(DateTime));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.START_TIME, typeof(DateTime));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.PLACE, typeof(string));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.PRESIDENT, typeof(string));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.SEASON, typeof(long));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.SERIAL, typeof(string));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.SESSION_ID, typeof(long));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.STAGE, typeof(long));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.STAGE_TYPE, typeof(string));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.SESSION_TYPE, typeof(string));
            tblSessionDetails.Columns.Add(Constants.SessionDetailsFields.SUBJECT, typeof(string));

            DataTable tblSessionAttendant = new DataTable(Constants.SESSION_ATTENDANT_TBL);
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.SESSION_ID, typeof(long));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.NAME, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.ATTENDANT_STATE, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.ATTENDANT_TYPE, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.JOB_TITLE, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.ATTENDANT_ID, typeof(long));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.FIRST_NAME, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.SECOND_NAME, typeof(string));
            tblSessionAttendant.Columns.Add(Constants.AttendantFields.TRIBE_NAME, typeof(string));

            DataTable tblSessionAgendaItem = new DataTable(Constants.SESSION_AGENDA_ITEMS_TBL);
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.AGENDA_ITEM_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_STATUS_DESCRIPTION, typeof(string));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_STATUS_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_TITLE, typeof(string));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_TYPE_DESCRIPTION, typeof(string));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_TYPE_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.PARENT_ITEM_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.PARENT_ITEM_TITLE, typeof(string));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.SESSION_ID, typeof(long));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.ITEM_ORDER, typeof(int));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.QUESTION_FROM, typeof(string));
            tblSessionAgendaItem.Columns.Add(Constants.AgendaItemFields.QUESTION_TO, typeof(string));

            ds.Tables.Add(tblSessionAgendaItem);
            ds.Tables.Add(tblSessionAttendant);
            ds.Tables.Add(tblSessionDetails);

            return ds;

        }




    }



    static class Constants
    {
        public static class SessionDetailsFields
        {
            public const string SESSION_ID = "SessionID";
            public const string SERIAL = "serial";
            public const string DATE = "date";
            public const string START_TIME = "StartTime";
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
            public const string ATTENDANT_ID = "AttendantID";//USAMA MISSING FROM  ep ???
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