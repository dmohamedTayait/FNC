using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Collections;
namespace TayaIT.Enterprise.EMadbatah.BLL
{
    public class EditorFacade
    {
        public static Session GetSessionByID(long sessionID)
        {
            return SessionHelper.GetSessionDetailsByID(sessionID);
        }
        public static List<SessionAgendaItem> GetAgendaSubItemsByAgendaID(long agendaID)
        {
            List<AgendaSubItem> items = AgendaHelper.GetAgendaSubItemsByAgendaID(agendaID);
            List<SessionAgendaItem> subItems = new List<SessionAgendaItem>();
            foreach (AgendaSubItem subItem in items)
                // subItems.Add(new SessionAgendaItem(subItem.ID, subItem.EParliamentID.Value, subItem.EParliamentParentID.Value, subItem.Name, subItem.Order , ));
                subItems.Add(new SessionAgendaItem(subItem.ID, subItem.EParliamentID, subItem.EParliamentParentID, subItem.Name, subItem.Order, subItem.QFrom, subItem.QTo));
            return subItems;
        }
        public static long AddNewAgendaItem(string itemText,long sessionID, long parentItemID)
        {
            AgendaItem item = AgendaHelper.GetAgendaItemByName(itemText);
            long itemID;
            if (item == null)
            {
                itemID = AgendaHelper.AddCustomSubAgendaItem(itemText, parentItemID);
                TayaIT.Enterprise.EMadbatah.DAL.SessionStartHelper.UpdateSessionStartStatus(sessionID, (int)Model.SessionFileStatus.New);
            }
            else
            {
                itemID = 0;
            }
            return itemID;
        }
        public static string UpdateAgendaItem(long id, string itemText)
        {
            AgendaHelper.UpdateAgendaItem(id, itemText);
            return "success";
        }
        public static void AddNewSessionItem(string itemText, long sessionID)
        {
            //long itemID = SessionIte.AddCustomAgendaItem(itemText, sessionID);
            //return itemID;
        }
    }
}
