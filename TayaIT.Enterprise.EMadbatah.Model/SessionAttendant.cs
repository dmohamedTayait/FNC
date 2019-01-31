using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Model
{
    public class SessionAttendant
    {

        public SessionAttendant()
        { }
        public SessionAttendant(int eparlimentID, string name, string jobTitle, AttendantState attendantState, AttendantType attendantType, string fName, string sName, string tName, long attendtantTitleID)
        {
            EparlimentID = eparlimentID;
            Name = name;
            JobTitle = jobTitle;
            State = attendantState;
            Type = attendantType;
            FirstName = fName;
            SecondName = sName;
            TribeName = tName;
            AttendantTitleID = attendtantTitleID;
        }

        public SessionAttendant(int eparlimentID, long eMadbatahID, string name, string jobTitle, AttendantState attendantState, AttendantType attendantType, string fName, string sName, string tName, long attendtantTitleID)
        {
            EparlimentID = eparlimentID;
            ID = eMadbatahID;
            Name = name;
            JobTitle = jobTitle;
            State = attendantState;
            Type = attendantType;
            FirstName = fName;
            SecondName = sName;
            TribeName = tName;
            AttendantTitleID = attendtantTitleID;
        }

        public int EparlimentID { get; set; }
        public long ID { get; set; }
        public string Name { get; set; }
        public string JobTitle { get; set; }
        public AttendantState State { get; set; }
        public AttendantType Type { get; set; }
        public double TotalSpeakTime { get; set; }


        public string FirstName { get; set; }

        public string TribeName { get; set; }

        public string SecondName { get; set; }
        public long AttendantTitleID { get; set; }
    }
}
