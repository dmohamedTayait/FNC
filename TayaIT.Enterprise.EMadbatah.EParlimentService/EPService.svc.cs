using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Web;
using System.Web.Hosting;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Configuration;
using System.Data.SqlClient;
using System.Collections;

namespace TayaIT.Enterprise.EMadbatah.EParlimentService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class EPService : IEPService
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        //Now we work on Excel data from App_Data folder of the webservice
        //we need to connect on SQL Server using Stored Procedure instead of Excel
        //return dataset if Sucess, return null if fail.
        //Will be Implemented by Ahmed AbdelMotteleb FNC
        public DataSet GetEParlimentSessionDetails(long sessionID)
        {
            try
            {
                return EPDatabaseHelper.GetSessionDetailsFromDB(sessionID);
                //return EPDatabaseHelper.GetSessionDetailsFromExcel(sessionID);
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "EPService.scv.cs");
                return null;

            }                   
        }

        public DataSet UpdateEParlimentSessionDetails(long sessionID)
        {
            try
            {
                DataSet tmp = EPDatabaseHelper.UpdateSessionDetailsFromDB(sessionID);             
                
                //DataRow rowAttendant = tmp.Tables[Constants.SESSION_ATTENDANT_TBL].NewRow();// .Rows.Add(new DataRow(
                //rowAttendant["AttendanceID"] = 5000;
                //rowAttendant[Constants.AttendantFields.ATTENDANT_STATE] = "Attended";
                //rowAttendant[Constants.AttendantFields.ATTENDANT_TYPE] = "FromTheCouncilMembers";
                //rowAttendant[Constants.AttendantFields.JOB_TITLE] = "معلم";
                //rowAttendant[Constants.AttendantFields.NAME] = "عباس الضو بيقول لأ";
                //rowAttendant["eparlimentID"] = 1018;
                //tmp.Tables[Constants.SESSION_ATTENDANT_TBL].Rows.Add(rowAttendant);


                
                //tmp.Tables[Constants.SESSION_AGENDA_ITEMS_TBL].Rows.RemoveAt(0); //ele3tzarat

                //DataRow rowAgendaItem = tmp.Tables[Constants.SESSION_AGENDA_ITEMS_TBL].NewRow();// .Rows.Add(new DataRow(
                //rowAgendaItem[Constants.AgendaItemFields.AGENDA_ITEM_ID] = 5000;
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_ID] = 5000;
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_ORDER] = 5;
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_STATUS_DESCRIPTION] = "نوقش جزئيا";
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_STATUS_ID] = 0;
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_TITLE] = "تعديل نظام الكفيل 22222";
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_TYPE_DESCRIPTION] = "" ;
                //rowAgendaItem[Constants.AgendaItemFields.ITEM_TYPE_ID] = 0;
                //rowAgendaItem[Constants.AgendaItemFields.PARENT_ITEM_ID] = 31;
                //rowAgendaItem[Constants.AgendaItemFields.PARENT_ITEM_TITLE] = "الأسئلة";
                //rowAgendaItem[Constants.AgendaItemFields.SESSION_ID] = 1018;

                //tmp.Tables[Constants.SESSION_AGENDA_ITEMS_TBL].Rows.Add(rowAgendaItem);

                //tmp.Tables[Constants.SESSION_AGENDA_ITEMS_TBL].Rows.RemoveAt(1);

                //rowAttendant[Constants.AttendantFields.ATTENDANT_STATE] = "Attended";
                //rowAttendant[Constants.AttendantFields.ATTENDANT_TYPE] = "FromTheCouncilMembers";
                //rowAttendant[Constants.AttendantFields.JOB_TITLE] = "معلم";
                //rowAttendant[Constants.AttendantFields.NAME] = "عباس الضو بيقول لأ";
                //rowAttendant["eparlimentID"] = 1018;
                //tmp.Tables[Constants.SESSION_ATTENDANT_TBL].Rows.Add(rowAttendant);
                
                //return tmp;

                return EPDatabaseHelper.UpdateSessionDetailsFromDB(sessionID);
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "EPService.scv.cs");
                return null;

            }                   
        }

        public long GetPrevSessionFilesTiming(Session session, int currentFileOrder)
        {
            long totalTime = 0;

                var sessionFiles = from sf in session.SessionFiles
                                   where (sf.IsSessionStart == false) && (sf.Order < currentFileOrder)
                                   orderby sf.Order
                                   select sf;

                if (sessionFiles != null && sessionFiles.Count<SessionFile>() > 0)
                {
                    foreach (SessionFile sf in sessionFiles)
                    {
                        totalTime += sf.DurationSecs;
                    }
 
                }

                return totalTime;


        }


        //return true on sucess, false on fail
        //Insert All EP needed content (all text segments of all speakers, Final PDF)
        public bool IngestContentsForFinalApprove(long sessionID)
        {
            try
            {
                Session session = SessionHelper.GetSessionDetailsByID(sessionID);

                

                List<List<SessionContentItem>> allItems = SessionContentItemHelper.GetItemsBySessionIDGrouped(sessionID);                               
                List<EparliamentSegment> allSegments = new List<EparliamentSegment>();
                int ctr = 1;

                //used when last item in a file is merged with first item in next file, meshmesh
                SessionContentItem lastIteminPrevList = null;
DateTime sessionStartTime = new DateTime(session.Date.Year, session.Date.Month, session.Date.Day, 
                                               session.StartTime.Value.Hour, session.StartTime.Value.Minute, session.StartTime.Value.Second);
//DateTime segEndTime = segStartTime;



                //starttime

                foreach (List<SessionContentItem> groupedItems in allItems)
                {

                    foreach (SessionContentItem item in groupedItems)
                    {

                        DateTime segStartTime = new DateTime();
                        DateTime segEndTime = new DateTime();


                        long prevTiming = GetPrevSessionFilesTiming(session, item.SessionFile.Order);
                        if (allSegments.Count == 0)
                        {
                            segStartTime = sessionStartTime.AddSeconds(prevTiming).AddSeconds((double)item.StartTime);
                            segEndTime = sessionStartTime.AddSeconds(prevTiming).AddSeconds((double)(item.EndTime));
                        }
                        else
                        {
                            segStartTime = allSegments[allSegments.Count - 1].EndTime;//sessionStartTime.AddSeconds(prevTiming).AddSeconds((double)item.StartTime);
                            segEndTime = segStartTime.AddSeconds((double)(item.EndTime - item.StartTime));
                        }

                        if (lastIteminPrevList != null && lastIteminPrevList.AttendantID == item.AttendantID &&
                            lastIteminPrevList.AgendaItemID == item.AgendaItemID &&
                            lastIteminPrevList.AgendaSubItemID == item.AgendaSubItemID
                            && allSegments.Count > 0)
                            item.MergedWithPrevious = true;
                        else
                            item.MergedWithPrevious = false;

                        lastIteminPrevList = item;

                        if (item.MergedWithPrevious == true && allSegments.Count > 0)
                        {
                            allSegments[allSegments.Count - 1].Text += " " + TextHelper.StripHTML(item.Text.ToLower()).Replace("&nbsp;", " ");
                            allSegments[allSegments.Count - 1].EndTime = segEndTime;

                            //segEndTime = segEndTime.AddSeconds((double)item.EndTime);
                        }
                        else
                        {
                            EparliamentSegment seg = new EparliamentSegment();
                            seg.AgendaItemId = item.AgendaItem.EParliamentID;
                            seg.AgendaSubItemID = item.AgendaSubItem == null ? null : item.AgendaSubItem.EParliamentID;
                            seg.AttendantID = item.Attendant.EparlimentID;
                            seg.CreatorBy = 2;// EPDatabaseHelper.GetUserEparliamentIDByEmail(item.User.Email);
                            seg.CreatedDate = item.CreatedDate;
                            seg.CreatorID = 2;// EPDatabaseHelper.GetUserEparliamentIDByEmail(item.User.Email);
                            seg.DiaryDate = session.Date;
                            seg.DiaryNumber = ctr; //check
                            seg.EndTime = segEndTime;
                            seg.SessionID = session.EParliamentID;
                            seg.StartTime = segStartTime;
                            seg.Subject = item.CommentOnText; //will be left as is for now
                            seg.Text = TextHelper.StripHTML(item.Text.ToLower()).Replace("&nbsp;", " ");
                            if (item.UpdatedByReviewer)
                            {
                                seg.UpdatedBy = EPDatabaseHelper.GetUserEparliamentIDByEmail(item.Reviewer.Email);
                                seg.UpdatedDate = item.UpdateDate;
                            }
                            else
                            {
                                seg.UpdatedBy = EPDatabaseHelper.GetUserEparliamentIDByEmail(item.User.Email);
                                seg.UpdatedDate = item.CreatedDate;

                            }
                            ctr++;
                            allSegments.Add(seg);//new EparliamentSegment(session.EParliamentID, item.User.FName, session.Date, item.StartTime, item.EndTime, item.AgendaItem.EParliamentID, item.AgendaSubItem.EParliamentID, item.Attendant.EparlimentID, item.Text));
                        }
                    }
                }

                foreach (EparliamentSegment seg in allSegments)
                {
                    bool sucess = EPDatabaseHelper.InsertIntoEparliament(seg);
                   // LogHelper.LogMessage(seg.StartTime.TimeOfDay.ToString() + " ---> " + seg.EndTime.TimeOfDay.ToString(), "service", System.Diagnostics.TraceEventType.Information);
                    if (!sucess)
                        return false;
                }
               

                InsertAttachmentIntoEParliment(session.EParliamentID, "مضبطة الجلسه " + session.Subject + " بعد التصديق", session.SessionMadbatahWordFinal, "مضبطه");//"application/ms-word");
                InsertAttachmentIntoEParliment(session.EParliamentID, "مضبطة الجلسه " + session.Subject + " بعد التصديق", session.SessionMadbatahPDFFinal, "مضبطه");//application/pdf");

                return true;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.EParlimentService.EPService.IngestContentsForFinalApprove(" + sessionID + ")");
                return false;
            }
        }



        

        //for inserting final pdf/word
        //Will be Implemented by Ahmed AbdelMotteleb FNC
        public bool InsertAttachmentIntoEParliment(long sessionID, string attachmentFilename, byte[] attachmentData, string fileType)
        {
            //return true on sucess, false on fail
            return true;
        }
        //helper class
        

        public int CheckHealth(int id)
        {
            return (1);
        }
    }
}
