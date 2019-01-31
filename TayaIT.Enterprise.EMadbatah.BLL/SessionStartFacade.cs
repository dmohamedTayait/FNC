using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Localization;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Globalization;
using System.Data.Objects.DataClasses;
namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public class SessionStartFacade
    {
        public static string madbatahHeader =
           "<div style=\"text-align:center;direction:rtl;font-family:arial;font-face:arial;font-size:14pt;clear:both;\"><strong>جدول أعمال %sessionNum% <br>المعقودة يوم : %HijriDate% <br>الموافـــق :  %GeorgianDate%"
           + "<br>ــــــــــــــــــــــــــــــــــ"
           + "<br>(  الساعة %sessionTime%   )</strong></div>";

        public static string madbatahIntro = "<p style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\"><strong>عقد المجلس الوطني الاتحادي </strong> %sessionNum%في دور انعقاده ال%sessionType% %sessionStage% من الفصل التشريعي %sessionSeason% ، وذلك في تمام الساعة %sessionTime% يوم %HijriDate% الموافق %GeorgianDate% برئاسة معالي / %sessionPresident% – رئيس المجلس .</p>";


        public static SessionDetails GetSessionDetails(long sessionID)
        {
            return EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
        }
        public static List<AgendaSubItem> GetAgendaSubItemsbyAgendaID(long agendaItemID)
        {
            return AgendaHelper.GetAgendaSubItemsByAgendaID(agendaItemID);
        }
        //public static Hashtable GetSessionAgendaItems(long sessionID)
        //{
        //    return EMadbatahFacade.GetSessionBySessionID(sessionID).AgendaItems;
        //    //((SessionAgendaItem)EMadbatahFacade.GetSessionBySessionID(sessionID).AgendaItems[0]).SubAgendaItems;
        //}

        //public static int UpdateSessionSetSessionStartID(long sessionID, long sessionStartID)
        //{
        //    return SessionHelper.UpdateSessionSetSessionStartID(sessionID, sessionStartID);
        //}
        //public static SessionStart AddSessionStart(string text, long userID)
        //{
        //    return SessionStartHelper.AddSessionStart(text, userID,false);
        //}
        //public static int UpdateSessionStart(long sessionStartID, string text)
        //{
        //    return SessionStartHelper.UpdateSessionStartText(sessionStartID, text);
        //}
        public static string GetAutomaticSessionStartText(long sessionID)
        {


            //new usama
            // the session start naming of الإول الثاني الثالث will be based on the ItemOrder from AgendaTable


            NumberingFormatter fomratterFemale = new NumberingFormatter(false);
            NumberingFormatter fomratterMale = new NumberingFormatter(true);
            Model.SessionDetails details = SessionStartFacade.GetSessionDetails(sessionID);

            //calculate hijri date
            //details.Date = details.Date.Subtract(new TimeSpan(1, 0, 0));
            DateTimeFormatInfo dateFormat= Util.DateUtils.ConvertDateCalendar(details.Date, Util.CalendarTypes.Hijri, "en-us");
            //DateTime hijDate = details.Date.ToString("f", dateFormat);


            string dayNameAr = details.Date.ToString("dddd", dateFormat); // LocalHelper.GetLocalizedString("strDay" + hijDate.DayOfWeek);
            string monthNameAr = LocalHelper.GetLocalizedString("strMonth" + details.Date.Month);
            string monthNameHijAr = details.Date.ToString("MMMM", dateFormat); //LocalHelper.GetLocalizedString("strHijMonth"+hijDate.Month);
            string dayOfMonthNumHij = details.Date.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("dd", dateFormat);//hijDate.Day;
            string yearHij = details.Date.ToString("yyyy", dateFormat);  //hijDate.Year;

            /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
            //  dateFormat.ShortDatePattern = "dd/MM/yyyy";
            //   return dateConv;//(dateConv.Date.ToString("f", dateFormat));

            //for header
            string sessionNum = details.Subject; //"الخامسة عشره";
            string hijriDate = dayNameAr + " " + dayOfMonthNumHij + " " + monthNameHijAr + " سنة " + yearHij + " هـ";//" 10 رجب سنة 1431 ه";//"الثلاثاء 10 رجب سنة 1431 ه";
            string gDate = details.Date.Day + " " + monthNameAr + " سنة " + details.Date.Year + " م "; //"22 يونيو سنة 2010 م";
            string timeInHour = LocalHelper.GetLocalizedString("strHour" + details.StartTime.Hour) + " " + LocalHelper.GetLocalizedString("strTime" + details.Date.ToString("tt"));//"التاسعة صباحا";
            string seasonType = details.StageType;// "العادي";
            long seasonStage = details.Stage;// "الخامس";
            string sessionSeason = details.Season + "";// "الرابع عشر";
            string sessionHeader = madbatahHeader.Replace("%sessionNum%", sessionNum);
            sessionHeader = sessionHeader.Replace("%HijriDate%", hijriDate);
            sessionHeader = sessionHeader.Replace("%GeorgianDate%", gDate);
            sessionHeader = sessionHeader.Replace("%sessionTime%", timeInHour);

            //if (details.AgendaItems == null)
            //    return "";
            //for gadwal el a3mal
            /*
            Hashtable agendaItems = details.AgendaItems;
            string gadawalA3mal = "";
            int counter = 0;
            foreach (DictionaryEntry Item in agendaItems)
            {

                SessionAgendaItem agendaItem = (SessionAgendaItem)Item.Value;
                string eparliamentID = Item.Key.ToString();
                //List<AgendaSubItem> subItems = SessionStartFacade.GetAgendaSubItemsbyAgendaID(agendaItem.ID);
                List<SessionAgendaItem> subItems = agendaItem.SubAgendaItems;
                string itemNum = "البند " + LocalHelper.GetLocalizedString("str" + (counter + 1)) + " :";
                gadawalA3mal += "<strong>" + itemNum + agendaItem.Text + "</strong><br>";
                if (subItems.Count > 0)
                {
                    for (int i = 0; i < subItems.Count; i++)
                    {
                        gadawalA3mal += (i + 1) + ".  " + subItems[i].Text + "<br>";
                    }
                }
                counter++;
            }
            */
            //for ordered agenda as existing from editor
            List<AgendaItem> agendaItems = SessionContentItemHelper.GetOrderedAgendaItemsBySessionID(sessionID);

            string gadawalA3mal = "<p style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
            int counter = 1;
            foreach (AgendaItem Item in agendaItems)
            {


                EntityCollection<AgendaSubItem> subItems = Item.AgendaSubItems;


                //string itemNum = "البند " + LocalHelper.GetLocalizedString("str" + (counter + 1)) + " :";
                
                //string itemNum = "البند " + LocalHelper.GetLocalizedString("str" + Item.Order) + " :"; ////usama new ordering
                string itemNum = "البند " + LocalHelper.GetLocalizedString("str" + counter) + " :"; ////usama new ordering


                //if (Item.Name.Contains("افتتاحية الجلسة ، وكلمة معالي رئيس المجلس"))
                if (Item.IsCustom == true)//for handling eftta7eyet el magles
                {
                    gadawalA3mal += "<strong>" + Item.Name + "</strong><br>";
                    counter--;
                }
                else
                    gadawalA3mal += "<strong>" + itemNum + Item.Name + "</strong><br>";

                if (subItems.Count > 0)
                {
                    int i = 0;
                    foreach (AgendaSubItem sub in subItems)
                    {
                        if (!string.IsNullOrEmpty(sub.QFrom) && !string.IsNullOrEmpty(sub.QTo))
                            sub.Name = new StringBuilder().Append("سؤال موجه إلى معالي / ") + sub.QTo + "من سعادةالعضو /" + sub.QFrom + "حول \"" + sub.Name + "\".";
                        gadawalA3mal += (i + 1) + ".  " + sub.Name + "<br>";
                        i++;
                    }
                    //for (int i = 0; i < subItems.Count; i++)
                    //{
                    //    gadawalA3mal += (i + 1) + ".  " + subItems.[i].Name + "<br>";
                    //}
                }
                counter++;
            }
            gadawalA3mal += "</p>";
            //FOR session introduction
            string sessionIntro = madbatahIntro.Replace("%sessionNum%", sessionNum);
            sessionIntro = sessionIntro.Replace("%HijriDate%", hijriDate);
            sessionIntro = sessionIntro.Replace("%GeorgianDate%", gDate);
            sessionIntro = sessionIntro.Replace("%sessionTime%", timeInHour);
            sessionIntro = sessionIntro.Replace("%sessionType%", seasonType);
            sessionIntro = sessionIntro.Replace("%sessionSeason%", fomratterMale.getResultEnhanced(int.Parse(sessionSeason.Trim())));
            sessionIntro = sessionIntro.Replace("%sessionStage%", fomratterMale.getResultEnhanced((int)seasonStage));

            sessionIntro = sessionIntro.Replace("%sessionPresident%", details.Presidnt);

            //for attendance
            List<SessionAttendant> attendance = details.Attendance;
            string president = details.Presidnt;
            DateTime date = details.Date;

            List<SessionAttendant> apologies = new List<SessionAttendant>();
            List<SessionAttendant> absents = new List<SessionAttendant>();
            List<SessionAttendant> inMission = new List<SessionAttendant>();
            List<SessionAttendant> secertairs = new List<SessionAttendant>();
            List<SessionAttendant> attendingFromMajles = new List<SessionAttendant>();
            List<SessionAttendant> attendingOutOfMajles = new List<SessionAttendant>();
            List<SessionAttendant> attendingFromGovernment = new List<SessionAttendant>();
            List<SessionAttendant> othersAttending = new List<SessionAttendant>();

            foreach (SessionAttendant attendant in attendance)
            {
                switch (attendant.State)
                {
                    case Model.AttendantState.Apology:
                        apologies.Add(attendant);
                        break;
                    case Model.AttendantState.Absent:
                        absents.Add(attendant);
                        break;
                    case Model.AttendantState.Attended:
                        switch (attendant.Type)
                        {
                            case Model.AttendantType.FromOutsideTheCouncil:
                                attendingOutOfMajles.Add(attendant);
                                break;
                            case Model.AttendantType.FromTheCouncilMembers:
                                attendingFromMajles.Add(attendant);
                                break;
                            case Model.AttendantType.GovernmentRepresentative:
                                attendingFromGovernment.Add(attendant);
                                break;
                            case Model.AttendantType.Secretariat:
                                secertairs.Add(attendant);
                                break;
                            case Model.AttendantType.NA:
                                othersAttending.Add(attendant);
                                break;
                        }
                        break;

                    case Model.AttendantState.InMission:
                        inMission.Add(attendant);
                        break;

                }
            }

            string introBody = "";
            if (inMission.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>وقد اعتذر</strong> عن عدم حضور هذه الجلسة في مهمه رسميه كل من :";
                introBody += "</div><br/>";
                introBody += "<table align=\"center\"  width=\"100%\" style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
                for (int i = 0; i < inMission.Count; i++)
                {
                    string tdTag = "<td style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i == 0)
                        tdTag = "<td width=\"50%\" style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i % 2 == 0)
                    {
                        introBody += "<tr>";
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(inMission[i], sessionID) + "</td>";
                    }
                    else
                    {
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(inMission[i], sessionID) + "</td>";
                        introBody += "</tr>";
                    }
                }
                if (inMission.Count % 2 != 0)
                {
                    introBody += "</tr>";
                }
                introBody += "</table>";
            }
            //introBody += "<br>";
            if (apologies.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>وقد اعتذر</strong> عن عدم حضور هذه الجلسة كل من :";
                introBody += "</div><br/>";
                introBody += "<table align=\"center\" width=\"100%\" style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
                for (int i = 0; i < apologies.Count; i++)
                {
                    string tdTag = "<td style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i == 0)
                        tdTag = "<td width=\"50%\" style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i % 2 == 0)
                    {
                        introBody += "<tr>";
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(apologies[i],sessionID)  + "</td>";
                    }
                    else
                    {
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(apologies[i], sessionID) + "</td>";
                        introBody += "</tr>";

                    }
                }
                if (apologies.Count % 2 != 0)
                {
                    introBody += "</tr>";
                }
                introBody += "</table>";
            }
            //introBody += "<br>";
            if (absents.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>وقد تغيب</strong> عن حضور هذه الجلسة :";
                introBody += "</div><br/>";
                introBody += "<table align=\"center\" width=\"100%\" style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
                for (int i = 0; i < absents.Count; i++)
                {
                    string tdTag = "<td style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i == 0)
                        tdTag = "<td width=\"50%\" style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i % 2 == 0)
                    {
                        introBody += "<tr>";
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(absents[i], sessionID) + "</td>";
                    }
                    else
                    {
                        introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(absents[i], sessionID) + "</td>";
                        introBody += "</tr>";

                    }
                }
                if (absents.Count % 2 != 0)
                {
                    introBody += "</tr>";
                }
                introBody += "</table>";
            }
            //introBody += "<br>";
            if (attendingFromGovernment.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>وحضر</strong> هذه الجلسة كل من :";
                introBody += "</div><br/>";
                introBody += "<table align=\"right\"  width=\"100%\" style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
                for (int i = 0; i < attendingFromGovernment.Count; i++)
                {
                    introBody += "<tr>";
                    string tdTag = "<td style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    if (i == 0)
                        tdTag = "<td width=\"50%\" style=\"text-align:right;direction:rtl;unicode-bidi:embed\" dir=\"RTL\" >";
                    introBody += tdTag + (i + 1) + ". " + MabatahCreatorFacade.GetAttendantTitle(attendingFromGovernment[i], sessionID) + "</td>";
                    introBody += tdTag + attendingFromGovernment[i].JobTitle + "</td>";
                    introBody += "</tr>";
                }
                introBody += "</table>";
            }
            //introBody += "<br>";
            if (attendingOutOfMajles.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>كما حضر</strong> الجلسة كل من :";

                for (int i = 0; i < attendingOutOfMajles.Count; i++)
                {
                    introBody += MabatahCreatorFacade.GetAttendantTitle(attendingFromMajles[i], sessionID) + "." + attendingOutOfMajles[i].JobTitle + "،";
                }
                introBody += "</div>";
            }
            //introBody += "<br>";

            //if (secertairs.Count > 0)
            //{
            //    introBody += "<p style=\"text-align:right; direction: rtl; font-family: arial; font-size: 14pt;\">";
            //    introBody += "<strong>كما حضر</strong> الجلسة كل من :";

            //    for (int i = 0; i < secertairs.Count; i++)
            //    {
            //        introBody += secertairs[i].Name + "–" + secertairs[i].JobTitle + "،";
            //    }
            //    introBody += "</p>";
            //}

            //introBody += "<br>";
            if (secertairs.Count > 0)
            {
                introBody += "<div style=\"text-align:right;direction:rtl;font-family:arial;font-size:14pt;clear:both;\">";
                introBody += "<strong>وتولى</strong> الأمانة العامة :";

                for (int i = 0; i < secertairs.Count; i++)
                {
                    introBody += MabatahCreatorFacade.GetAttendantTitle(secertairs[i], sessionID) + " - " + secertairs[i].JobTitle + "،";
                    //introBody += "</tr>";
                }
                introBody += "</div>";
            }
            //introBody += "<br>";
            string madbatahStart = "<html style=\"direction: rtl; font-family: arial; font-size: 14pt;\">";
            madbatahStart +="<body dir=\"rtl\"><font size=\"14\" face=\"arial\" >";
            madbatahStart += /*"<p style=\"direction: rtl; font-family: arial; font-size: 14pt;\">" +*/ sessionHeader /*+"</p>"*/;
            madbatahStart += gadawalA3mal;// +"<br/>";
            madbatahStart += "<hr class=\"mceNonEditable\"/>";
            madbatahStart += sessionIntro;
            madbatahStart += introBody + "</font></body></html>";
            return madbatahStart;
        }
        public static bool AddUpdateSessionStart(long sessionId, string sessionStartText, long userID, string startName)
        {

            //Session session = SessionHelper.GetSessionByID(sessionId);
            SessionFile sessionStart = SessionStartHelper.GetSessionStartBySessionId(sessionId);
            if (sessionStart == null)
            {
                DAL.SessionFile start = SessionStartHelper.AddSessionStart(sessionStartText, userID, sessionId, startName);
                if (start != null)
                    return true;
                else
                    return false;
                //SessionStartFacade.UpdateSessionSetSessionStartID(sessionId, start.ID);
            }
            else
            {
                if (SessionStartHelper.UpdateSessionStartText(sessionStart.ID, sessionStartText, userID) > 0)
                    return true;
                else
                    return false;
            }


        }

        public static bool AddNewSessionStart(long sessionId, string sessionStartText, string startName)
        {


            DAL.SessionFile start = SessionStartHelper.AddSessionStart(sessionStartText, sessionId, startName);
            if (start != null)
                return true;
            else
                return false;
            //SessionStartFacade.UpdateSessionSetSessionStartID(sessionId, start.ID);


        }



        public static SessionFile GetSessionStartBySessionID(long sessionID)
        {
            return SessionStartHelper.GetSessionStartBySessionId(sessionID);
        }
    }
}
