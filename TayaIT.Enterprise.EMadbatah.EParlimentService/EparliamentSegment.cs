using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TayaIT.Enterprise.EMadbatah.EParlimentService
{
    public class EparliamentSegment
    {
        /*public EparliamentSegment(long _sid, long _cID, DateTime _sdate, double? _stime, double? _etime,
            long? _agendaid, long? _subagendaId, long _attID, string _text)
        {
            sessionID  = _sid;
            creatorID= _cID;
            diaryDate = _sdate;
            startTime = _stime;
            endTime= _etime;
            agendaItemId = _agendaid;
            agendaSubItemID = _subagendaId;
            attendantID = _attID;
            text=_text;
        }*/

        public int? SessionID { set; get; } //for SessionID  field
        public int? CreatorID { set; get; } //CreatorID  = the employee name who insert the diaries
        public int? CreatorBy { set; get; } // same as CreatorID, CreatedBy        = the employee name who insert the diaries
        public DateTime CreatedDate { set; get; }//CreatedDate    = the date for inserting        
        public int? UpdatedBy { set; get; } // UpdatedBy  = the employee name who update the diaries      = the employee name who insert the diaries
        public DateTime UpdatedDate { set; get; }//UpdatedDate  = the date for updating       
        public DateTime DiaryDate { set; get; }//DiaryDate: same as session date          
        public DateTime StartTime { set; get; }//start time for this diary
        public DateTime EndTime { set; get; }//end time for this diary
        public int? AgendaItemId { set; get; }//MainItemID
        public int? AgendaSubItemID { set; get; } //SubItemID
        public int? AttendantID { set; get; }//InterventionOwnerID = the member ,government or employee 
        public string Text { set; get; } //Text
        public string Subject { set; get; } //Subject
        public int DiaryNumber { set; get; }//DiaryNumber = serial for every diary
    }
}