using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public static class AgendaHelper
    {
        #region methods
        public static bool AddAgendaItem(string text, long sessionID, int eParlimentID, int eParlimentParentID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaItem item = new AgendaItem
                    {
                        Name = text,
                        EParliamentID = eParlimentID,
                        EParliamentParentID = eParlimentParentID,
                        SessionID = sessionID,
                        IsCustom = false
                    };
                    context.AgendaItems.AddObject(item);
                    context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.AddAgendaItem(" + sessionID + "," + eParlimentID + ")");
                return false;
            }

        }


        public static bool AddAgendaItem(AgendaItem agendaItem, long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    Session session = context.Sessions.FirstOrDefault(c => c.ID == sessionID);
                    session.AgendaItems.Add(agendaItem);
                    context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.AddAgendaItem()");
                return false;
            }

        }



        public static bool AddAgendaSubItem(string text, long agendaItemID,  int? eParlimentID, int? eParlimentParentID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaSubItem item = new AgendaSubItem
                    {
                        Name = text,
                        EParliamentID = eParlimentID,
                        EParliamentParentID = eParlimentParentID,
                        AgendaItemID = agendaItemID,

                    };
                    context.AgendaSubItems.AddObject(item);
                    context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.AddAgendaSubItem(" + agendaItemID + " , " + eParlimentID + " , " + eParlimentParentID  + ")");
                return false;
            }

        }

        //public static long AddCustomAgendaItem(string text, long sessionID)
        //{
        //    try
        //    {
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            AgendaItem item = new AgendaItem
        //            {
        //                Name = text,
        //                SessionID = sessionID,
        //                IsCustom = true
        //            };
        //            context.AgendaItems.AddObject(item);
        //            context.SaveChanges();
        //            //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
        //            return item.ID;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.AddAgendaItem(" + sessionID + ")");
        //        return 0;
        //    }

        //}


        public static long AddCustomSubAgendaItem(string text, long parentAgendaItemID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaSubItem item = new AgendaSubItem
                    {
                        Name = text,
                        //Order = 
                        AgendaItemID = parentAgendaItemID,
                        IsCustom = true
                    };
                    context.AgendaSubItems.AddObject(item);
                    context.SaveChanges();
                    //context.Refresh(System.Data.Objects.RefreshMode.StoreWins, item);
                    return item.ID;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.AddCustomSubAgendaItem(" + text + ")");
                return 0;
            }

        }

        public static int DeleteAgendaItemByID(long agendaItemID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaItem agenda = context.AgendaItems.FirstOrDefault(c => c.ID == agendaItemID);
                    if (agenda.AgendaSubItems.Count > 0)
                    {
                        foreach (AgendaSubItem subItem in agenda.AgendaSubItems)
                        {
                            context.AgendaSubItems.DeleteObject(subItem);
                        }
                        
                    }
                    context.AgendaItems.DeleteObject(agenda);
                    int res = context.SaveChanges();
                    return res;
                }
                
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.DeleteAgendaItem(" + agendaItemID + ")");
                return -1;
            }
        }

        public static int DeleteAgendaItemByEparliamentID(long agendaItemEparlID, long sessionID)
        {
            try
            {
                AgendaItem sessionUnknownItem = AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف", sessionID);
                if (sessionUnknownItem == null)
                {
                    sessionUnknownItem = new AgendaItem();
                    sessionUnknownItem.IsCustom = true;
                    sessionUnknownItem.Name = "غير معرف";
                    sessionUnknownItem.EParliamentID = null;
                    sessionUnknownItem.Order = null;
                    AgendaHelper.AddAgendaItem(sessionUnknownItem, sessionID);

                }

                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaItem agenda = context.AgendaItems.FirstOrDefault(c => c.EParliamentID == agendaItemEparlID && c.SessionID == sessionID);
                    List<SessionContentItem> allItems = SessionContentItemHelper.GetItemsByAgendaItemID(agenda.ID, (long)agenda.SessionID);
                    long unknownAgendaItemID = AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف",(long)agenda.SessionID).ID;
                    foreach (SessionContentItem item in allItems)
                    {
                        SessionContentItemHelper.UpdateSessionContentItemAgendaItemID(item.ID, unknownAgendaItemID, 2);
                    }

                    if (agenda.AgendaSubItems.Count > 0)
                    {
                        foreach (AgendaSubItem subItem in agenda.AgendaSubItems.ToList())
                        {
                            context.AgendaSubItems.DeleteObject(subItem);
                        }

                    }
                    context.AgendaItems.DeleteObject(agenda);
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.DeleteAgendaItemByEparliamentID(" + agendaItemEparlID + ")");
                return -1;
            }
        }

        public static int UpdateAgendaItem(long agendaItemID, string itemName)
        {
            try
            {
                AgendaItem updated_agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_agenda = context.AgendaItems.FirstOrDefault(c => c.ID == agendaItemID);
                    if (updated_agenda != null)
                    {
                        //update the agenda attributes
                        updated_agenda.Name = itemName == null ? updated_agenda.Name : itemName;
                        //updated_agenda.ParentID = parent_id == null ? updated_agenda.ParentID : parent_id;
                        //updated_agenda.EParliamentID = eparliamentID == null ? updated_agenda.EParliamentID : eparliamentID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                //return updated_agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.UpdateAgendaItem(" + agendaItemID + "," + itemName + ")");
                return -1;
            }
        }



        public static int MarkSubAgendaItemsAsGrouped(long agendaItemID, bool isGrouped)
        {
            try
            {
                AgendaItem updated_agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_agenda = context.AgendaItems.FirstOrDefault(c => c.ID == agendaItemID);
                    if (updated_agenda != null)
                    {
                        //update the agenda attributes
                        updated_agenda.IsGroupSubAgendaItems = isGrouped;
                        //updated_agenda.ParentID = parent_id == null ? updated_agenda.ParentID : parent_id;
                        //updated_agenda.EParliamentID = eparliamentID == null ? updated_agenda.EParliamentID : eparliamentID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                //return updated_agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.MarkSubAgendaItemsAsGrouped(" + agendaItemID + ")");
                return -1;
            }
        }

        public static int UpdateSubAgendaItem(long subAgendaItemID, string itemName)
        {
            try
            {
                AgendaSubItem updated_agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_agenda = context.AgendaSubItems.FirstOrDefault(c => c.ID == subAgendaItemID);
                    if (updated_agenda != null)
                    {
                        //update the agenda attributes
                        updated_agenda.Name = itemName == null ? updated_agenda.Name : itemName;
                        //updated_agenda.ParentID = parent_id == null ? updated_agenda.ParentID : parent_id;
                        //updated_agenda.EParliamentID = eparliamentID == null ? updated_agenda.EParliamentID : eparliamentID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
                //return updated_agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.UpdateSubAgendaItem(" + subAgendaItemID + "," + itemName + ")");
                return -1;
            }
        }


        public static AgendaItem GetAgendaItemById(long itemID)
        {
            try
            {
                AgendaItem agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    agenda = context.AgendaItems.FirstOrDefault(c => c.ID == itemID);
                }
                return agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.GetAgendaItemByAgendaId(" + itemID + ")");
                return null;
            }
        }

        public static AgendaItem GetAgendaItemByName(string itemName)
        {
            try
            {
                AgendaItem agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    agenda = context.AgendaItems.FirstOrDefault(c => c.Name == itemName);
                }
                return agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.GetAgendaByAgendaName(" + itemName + ")");
                return null;
            }
        }
        public static AgendaItem GetAgendaItemByNameAndSessionID(string itemName, long sessionID)
        {
            try
            {
                AgendaItem agenda = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    agenda = context.AgendaItems.FirstOrDefault(c => c.Name == itemName && c.SessionID == sessionID);
                }
                return agenda;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.GetAgendaByAgendaName(" + itemName + ")");
                return null;
            }
        }
        public static List<AgendaSubItem> GetAgendaSubItemsByAgendaID(long agendaID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    return context.AgendaItems.FirstOrDefault(c => c.ID == agendaID).AgendaSubItems.ToList<AgendaSubItem>();
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.GetAgendaItemsByAgendaID(" + agendaID + ")");
                return null;
            }
        }

        
        #endregion



        public static int DeleteSubAgendaItemByEparliamentID(long subAgendaItemEparlID, long subAgendaItemParentEparlID, long sessionID)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    AgendaSubItem agenda = context.AgendaSubItems.FirstOrDefault(c => c.EParliamentID == subAgendaItemEparlID && c.EParliamentParentID == subAgendaItemParentEparlID);
                    List<SessionContentItem> allItems = SessionContentItemHelper.GetItemsBySubAgendaItemID(agenda.ID);
                    long unknownAgendaItemID = AgendaHelper.GetAgendaItemByNameAndSessionID("غير معرف", sessionID).ID;
                    foreach (SessionContentItem item in allItems)
                    {
                        SessionContentItemHelper.UpdateSessionContentItemAgendaItemID(item.ID, unknownAgendaItemID, 2);
                    }

                    
                    context.AgendaSubItems.DeleteObject(agenda);
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.AgendaHelper.DeleteAgendaItemByEparliamentID(" + subAgendaItemEparlID + ")");
                return -1;
            }
        }

    }
}
