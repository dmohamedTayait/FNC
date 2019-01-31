using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Model;

namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public class ReviewNotesFacade
    {
        public static List<SessionContentItem> GetSessionContentItems(long sessionFileID, TayaIT.Enterprise.EMadbatah.Model.SessionContentItemStatus status)
        {
            return SessionContentItemHelper.GetItemsByFileIDAndStatusID(sessionFileID, (int)status);
        }

        public static List<SessionContentItem> GetSessionContentItems(long sessionFileID, TayaIT.Enterprise.EMadbatah.Model.SessionContentItemStatus status, long userID)
        {
            return SessionContentItemHelper.GetSessionContentItems(sessionFileID, (int)status, userID);
        }
    }
}
