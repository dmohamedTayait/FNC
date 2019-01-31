using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class SessionAudioFile
    {
        public SessionAudioFile(long id, long? userID, long sessionID, string name, int lastInsertedFragNumInXml, int order, long durationInSec, SessionFileStatus status, string ownerUserName, DateTime? lastModefied, string ownerEmail, bool isSessionStart, string sessionStartText, long? fileReviewrID, string fileReviewerUserName, string fileReviewerEmail)
        {           
            ID = id;
            UserID = userID;
            OwnerUserName = ownerUserName;
            OwnerEmail = ownerEmail;
            SessionID = sessionID;
            Name = name;
            LastInsertedFragNumInXml = lastInsertedFragNumInXml;
            Order = order;
            DurationInSec = durationInSec;
            Status = status;
            LastModefied = lastModefied;
            IsSessionStart = isSessionStart;
            SessionStartText = sessionStartText;
            FileReviewrID = fileReviewrID;
            FileReviewerUserName = fileReviewerUserName;
            FileReviewerEmail = fileReviewerEmail;
        }

        public SessionAudioFile(long sessionID, string name, int order, long durationInSec, SessionFileStatus status)
        {            
            SessionID = sessionID;
            Name = name;
            Order = order;
            DurationInSec = durationInSec;
            Status = status;
        }

        public string Name { get; set; }
        public long ID { get; set; }
        public long? UserID { get; set; }
        public long? FileReviewrID { get; set; }
        
        public string OwnerUserName { get; set; }
        public string OwnerEmail { get; set; }


        public string FileReviewerUserName { get; set; }
        public string FileReviewerEmail { get; set; }

        public int LastInsertedFragNumInXml { get; set; }
        public DateTime? LastModefied { get; set; }
        public long DurationInSec { get; set; }
        public int Order { get; set; }
        public long SessionID { get; set; }
        public SessionFileStatus Status { get; set; }
        public bool IsSessionStart { get; set; }
        public string SessionStartText { get; set; }

    }
}
