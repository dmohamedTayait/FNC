using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Collections;
namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public class SessionContentItemFacade
    {

        public static long AddNewSessionContentItem(long sessionFileID, long sessionID, string item_text, int attendantID, int agendaItemID, int? agendaSubItemID, long userID, int statusID, string commentOnText, string commentOnAttendant, string footer, bool mergedWithPrev, int fragmentOrderInXML, float startTime, float endTime, float duration, bool ignored)
        {
            long id = SessionContentItemHelper.AddNewSessionContentItem(sessionFileID, sessionID, item_text, attendantID, agendaItemID, agendaSubItemID, userID, statusID, commentOnText, commentOnAttendant, footer, mergedWithPrev, fragmentOrderInXML, startTime, endTime, duration, ignored);
            return id;
        }
        public static SessionContentItem GetSessionContentItemByIdAndFragmentOrder(long sessionFileID, int fragmentOrderInXML)
        {
            return SessionContentItemHelper.GetSessionContentItemByIdAndFragmentOrder(sessionFileID, fragmentOrderInXML);
        }
        public static int UpdateSessionContentItem(long sessionContentItemID, string text, long attendantID, long agendaItemID, long? agendaSubItemID, string commentsOnAttendant, string commentsOnText, string FooterText, Model.SessionContentItemStatus status, bool mergedWithPrev, bool ignored)
        {
            return SessionContentItemHelper.UpdateSessionContentItem(sessionContentItemID, text, attendantID, agendaItemID, agendaSubItemID, commentsOnAttendant, commentsOnText, FooterText, (int)status, mergedWithPrev, ignored);
        }
        

    }
}
