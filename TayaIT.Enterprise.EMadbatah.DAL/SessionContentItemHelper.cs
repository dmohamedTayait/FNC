using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Util;

namespace TayaIT.Enterprise.EMadbatah.DAL
{
    public class SessionContentItemHelper
    {
        #region SessionContentItem
        public static long AddNewSessionContentItem(
            long sessionFileID,
            long sessionID,
            string item_text,                          
            int attendantID,
            int agendaItemID,
            int? agendaSubItemID,
            long userID,
            int statusID,
            string commentOnText,
            string commentOnAttendant,
            string footer,              
            bool? mergedWithPrev,
            int fragmentOrderInXML,
            float startTime,
            float endTime,
            float duration,
            bool ignored)
        {


            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    SessionContentItem session_content_item = new SessionContentItem
                    {
                        
                        SessionFileID = sessionFileID,
                        SessionID = sessionID,
                        Text = item_text,
                        AttendantID = attendantID,
                        AgendaItemID = agendaItemID,
                        AgendaSubItemID = agendaSubItemID,
                        UserID = userID,
                        StatusID = statusID,
                        CommentOnAttendant = commentOnAttendant,
                        CommentOnText = commentOnText,
                        PageFooter = footer,
                        MergedWithPrevious = mergedWithPrev,
                        FragOrderInXml = fragmentOrderInXML,
                        StartTime = startTime,
                        EndTime = endTime,
                        Duration = duration,
                        CreatedDate = DateTime.Now,
                        UpdateDate = DateTime.Now,
                        Ignored = ignored
                        
                    };
                    context.SessionContentItems.AddObject(session_content_item);
                    if (!string.IsNullOrEmpty(commentOnAttendant))
                    {
                        var att = context.Attendants.FirstOrDefault<Attendant>(a => a.ID == attendantID);
                        if (att != null)
                        {
                            att.JobTitle = commentOnAttendant;
                        }
                    }
                    context.SaveChanges();
                    return session_content_item.ID;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.AddNewSessionContentItem(" + userID + "," + sessionID + "," + sessionFileID + ")");
                return 0;
            }
        }
        //for next and previous in editor
        public static SessionContentItem GetSessionContentItemByIdAndFragmentOrder(long sessionFileID, int fragmentOrderInXML)
        {
            try
            {
                SessionContentItem session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    session_content_item = context.SessionContentItems.FirstOrDefault(c => c.SessionFileID == sessionFileID
                                                                                && c.FragOrderInXml == fragmentOrderInXML);
                    if (session_content_item != null)
                        session_content_item.AgendaItem = session_content_item.AgendaItem;
                }

                return session_content_item;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemByIdAndFragmentOrder(" + sessionFileID + "," + fragmentOrderInXML + ")");
                return null;
            }
        }

        public static int AprroveFileSessionContentItems(long sessionFileID)
        {
            try
            {
                SessionFile sessionFile = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    sessionFile = context.SessionFiles.FirstOrDefault(c => c.ID == sessionFileID);
                    var sessionContentItems =
                               from sci in sessionFile.SessionContentItems
                               where sci.StatusID != 1
                                select sci;
                        List<SessionContentItem> allItems = sessionContentItems.ToList<SessionContentItem>();
                        for (int i = 0; i < allItems.Count;i++)// sessionContentItems.Count<SessionContentItem>(); i++)
                        {
                            allItems[i].StatusID = 1;
                        }
                    context.SaveChanges();
                }

                return 1;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.AprroveFileSessionContentItems(" + sessionFileID + ")");
                return -1;
            }
        }

        //for getting prev sci on page load
        public static SessionContentItem GetPrevSessionContentItem(long sessionFileID, int fragmentOrderInXML)
        {
            try
            {
                SessionContentItem session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var file =
                        from sf in context.SessionFiles
                        where sf.ID == sessionFileID
                        select sf;
                    //List<SessionFile> files =  file.ToList<SessionFile>();
                    SessionFile curFile = file.First<SessionFile>();
                    if (curFile != null)
                    {
                        var sessionContentItems =
                               from sci in curFile.SessionContentItems
                               where sci.FragOrderInXml < fragmentOrderInXML
                               orderby sci.FragOrderInXml descending
                               select sci;
                        List<SessionContentItem> allItems = sessionContentItems.ToList<SessionContentItem>();
                        if (allItems.Count > 0)
                        {
                            session_content_item = sessionContentItems.ToList<SessionContentItem>()[0];
                            session_content_item.Attendant = session_content_item.Attendant;
                        }
                        //else
                          //  return null;
                    }
                    //else
                    //    return null;
                    //session_content_item = context.SessionContentItems.SingleOrDefault(c => c.SessionFileID == sessionFileID
                                                                                //&& c.FragOrderInXml == fragmentOrderInXML);
                    //session_content_item.AgendaItem = session_content_item.AgendaItem;
                }

                return session_content_item;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetPrevSessionContentItem(" + sessionFileID + "," + fragmentOrderInXML + ")");
                return null;
            }
        }

        //for editor
        public static SessionContentItem GetSessionContentItemById(long sessionContentItemID)
        {
            try
            {
                SessionContentItem session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (session_content_item != null)
                        session_content_item.AgendaItem = session_content_item.AgendaItem;
                }
                return session_content_item;
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemById(" + sessionContentItemID + ")");
                return null;
            }
        }

        //for review page
        public static List<SessionContentItem> GetSessionContentItemsBySessionID(long sessionID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;

                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {
                            contentItem.Attendant = contentItem.Attendant;
                            contentItem.User = file.User;//contentItem.User;
                            contentItem.SessionFile = contentItem.SessionFile;
                            contentItem.AgendaItem = contentItem.AgendaItem;
                            contentItem.AgendaSubItem = contentItem.AgendaSubItem;
                            toRet.Add(contentItem);
                        }

                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }

        //for SessionStart
        public static List<AgendaItem> GetOrderedAgendaItemsBySessionID(long sessionID)
        {
            try
            {
                List<AgendaItem> toRet = new List<AgendaItem>();
                
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;

                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {
                            contentItem.AgendaItem = contentItem.AgendaItem;
                            contentItem.AgendaItem.AgendaSubItems.Load();
                            //contentItem.AgendaItem.AgendaSubItems.OrderBy(
                            if(toRet.IndexOf(contentItem.AgendaItem) == -1)
                                toRet.Add(contentItem.AgendaItem);
                        }

                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetOrderedAgendaItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }
        public static List<SessionContentItem> GetItemsByAgendaItemID(long agendaItemID, long sessionID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var items =
                        from sf in context.SessionContentItems
                        where sf.SessionID == sessionID && sf.AgendaItemID == agendaItemID
                        select sf;

                    toRet.AddRange(items.ToList<SessionContentItem>());

                    //List<AgendaSubItem> subItems = AgendaHelper.GetAgendaSubItemsByAgendaID(agendaItemID);

                    //foreach (AgendaSubItem item in subItems)
                    //{
                    //    var sessionContentItemsForSubItem =
                    //        from sf in context.SessionContentItems
                    //        where sf.SessionID == sessionID && sf.AgendaItemID== agendaItemID
                    //        select sf;

                    //    foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                    //    {

                    //        if (contentItem.StatusID == statusID && contentItem.UserID == userID)
                    //            toRet.Add(contentItem);
                    //    }

                    //}
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }

        public static List<SessionContentItem> GetItemsBySubAgendaItemID(long agendaItemID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var items =
                        from sf in context.SessionContentItems
                        where sf.AgendaSubItemID == agendaItemID
                        select sf;

                    toRet.AddRange(items.ToList<SessionContentItem>());

                    
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + agendaItemID + ")");
                return null;
            }

        }

        public static List<List<SessionContentItem>> filterOnAdjacentSubAgendaItems(List<SessionContentItem> list)
        {
            if(list.Count == 0)
                return new List<List<SessionContentItem>>();
            List<List<SessionContentItem>> newList = new List<List<SessionContentItem>>();
            
            List<SessionContentItem> curList = new List<SessionContentItem>(){list[0]};
            for (int i = 1; i < list.Count; i++)
            {
                SessionContentItem cur = list[i];
                if (cur.AgendaSubItemID == curList[curList.Count - 1].AgendaSubItemID)
                    curList.Add(cur);
                else
                {
                    newList.Add(curList);
                    curList = new List<SessionContentItem>();
                    curList.Add(cur);
                }
            }
            if (curList.Count != 0)
                newList.Add(curList); ;
            return newList;
        }
        //for madbatahCreator
        public static List<List<SessionContentItem>> GetItemsBySessionIDGrouped(long sessionID)
        {
            try
            {
                List<List<SessionContentItem>> toRet = new List<List<SessionContentItem>>();
                List<SessionContentItem> tempRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;
                            //group sci by new { sci.AgendaItemID, sci.AgendaSubItemID } into g
                            //select g;//.ToList<SessionContentItem>();
                        
                        /*foreach (List<SessionContentItem> contentItems in sessionContentItems.ToList<List<SessionContentItem>>())
                        {*/
                        foreach (SessionContentItem it in sessionContentItems)
                            {
                                it.AgendaItem = it.AgendaItem;
                                it.AgendaSubItem = it.AgendaSubItem;
                                it.Attendant = it.Attendant;
                                it.Reviewer = it.Reviewer;
                                //added for review page
                                it.User = it.User;
                                it.SessionFile.FileReviewer = it.SessionFile.FileReviewer;
                                if (it.User == null)
                                {
                                    ;
                                }
                                it.SessionFile = it.SessionFile;
                            
                            if(!it.Ignored.Value)
                                tempRet.Add(it);
                            }
                            
                        /*}*/

                    }
                    //commented //12-04-2012
                    //var returned =
                    //    from ret in tempRet
                    //    group ret by new { ret.AgendaItemID, ret.AgendaSubItemID } into g
                    //    select g.ToList<SessionContentItem>();


                    //var returned =
                    //    from t in tempRet.WithAdjacentGrouping()
                    //    group t by new { t.AgendaItemID, t.AgendaSubItemID } into g
                    //    select g;

                    //var groups =
                    //   from t in tempRet.WithAdjacentGrouping()
                    //   group t by t.AgendaItemID into g
                    //   select new {
                    //        Val = g.ToList<SessionContentItem>()
                    //   };
                    //12-04-2012
                    var groups =
                       from t in tempRet.WithAdjacentGrouping()
                       group t by t.AgendaItemID into g
                       select new
                       {
                           Val = g.ToList<SessionContentItem>()
                       };


                    //foreach (List<SessionContentItem> gTemp in groups)
                    //{
                     foreach (var g2 in groups)
                     {
                    ////var tt = groups.ToList()[0];
                    //List<SessionContentItem> tmpList = new List<SessionContentItem>();

                     
                    //foreach (SessionContentItem sci in groups.ToList<int, List<SessionContentItem>>())
                    //    tmpList.Add(sci);

                        //var groups2 =
                        //   from t in g2.Val.WithAdjacentGrouping()
                        //   group t by t.AgendaSubItemID.Value into g3
                        //   select new
                        //   {
                        //       Val = g3.ToList<SessionContentItem>()
                        //   };
                        //foreach (var g3 in groups2)
                        // {
                         foreach (var g3 in filterOnAdjacentSubAgendaItems(g2.Val))
                                toRet.Add(g3);
                        //}
                     }
                    //}
                    //var returned = tempRet.GroupAdjacent(i => i.AgendaSubItemID );

                    //var returned = tempRet.Where((w, i) => i == 0 || (w.AgendaItemID != tempRet.ElementAt(i - 1).AgendaItemID && w.AgendaSubItemID != tempRet.ElementAt(i - 1).AgendaSubItemID));

                    //foreach (List<SessionContentItem> contentItems in returned.ToList<List<SessionContentItem>>())
                    //{
                    //    toRet.Add(contentItems);
                    //}
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }

       

        //for reviewNotes page
        public static List<SessionContentItem> GetItemsByFileIDAndStatusID(long sessionFileID, int statusID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //var sessionFiles =
                    //    from sf in context.SessionFiles
                    //    orderby sf.Order
                    //    where sf.SessionID == sessionID && sf.ID == sessionFileID
                    //    select sf;

                    SessionFile file  = context.SessionFiles.Single(c => c.ID == sessionFileID
                                                                       );// && c.Status == statusID);                    
                    var sessionContentItems =
                        from sci in file.SessionContentItems
                        orderby sci.FragOrderInXml
                        where sci.StatusID == statusID
                        select sci;

                    foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                    {
                        contentItem.AgendaItem = contentItem.AgendaItem;
                        contentItem.Reviewer = contentItem.Reviewer;
                        contentItem.FileReviewer = contentItem.FileReviewer;
                        contentItem.User = file.User;
                        contentItem.SessionFile = contentItem.SessionFile;
                        toRet.Add(contentItem);
                    }

                    
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetRejectedItemsByFileIDAndStatusID(" + sessionFileID + ")");
                return null;
            }

        }

        //for edit session file page
        public static int GetItemsCountByFileID(long sessionFileID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //var sessionFiles =
                    //    from sf in context.SessionFiles
                    //    orderby sf.Order
                    //    where sf.SessionID == sessionID && sf.ID == sessionFileID
                    //    select sf;

                    SessionFile file = context.SessionFiles.Single(c => c.ID == sessionFileID
                                                                       );// && c.Status == statusID);                    
                    var sessionContentItems =
                        from sci in file.SessionContentItems
                        orderby sci.FragOrderInXml
                        select sci;

                    //foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                    //{
                    //    contentItem.AgendaItem = contentItem.AgendaItem;
                    //    contentItem.Reviewer = contentItem.Reviewer;
                    //    toRet.Add(contentItem);
                    //}

                    return sessionContentItems.ToList<SessionContentItem>().Count;
                    //return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetItemsCountByFileID(" + sessionFileID + ")");
                return 0;
            }

        }


        public static List<SessionContentItem> GetSessionContentItemsBySessionIDAndStatusID(long sessionID, int statusID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;

                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {

                            if (contentItem.StatusID == statusID)
                                toRet.Add(contentItem);
                        }

                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }
        public static List<SessionContentItem> GetSessionContentItemsBySessionIDAndNotStatusID(long sessionID, int statusID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;

                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {

                            if (contentItem.StatusID != statusID)
                                toRet.Add(contentItem);
                        }

                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }
        public static bool DoesSessionContainsUndefinedAttendants(long sessionID)
        {
            try
            {
                
                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    Attendant unknownAtt = context.Sessions.FirstOrDefault(c => c.ID == sessionID).Attendants.FirstOrDefault(c => c.Name == "غير معرف");
                    var sessionContentItems =
                        from sci in context.SessionContentItems
                        //where sci.SessionID == sessionID && sci.Attendant.Type == 7 && sci.AttendantID != unknownAtt.ID
                        where ((sci.SessionID == sessionID) && (sci.AttendantID == unknownAtt.ID))
                        orderby sci.FragOrderInXml
                        select sci;

                    if (sessionContentItems == null)
                        return false; // or throw an exception
                    try
                    {
                        return sessionContentItems.Any();
                    }
                    catch {
                        return false;
                    }

                    //if (sessionContentItems != null && sessionContentItems.ToList<SessionContentItem>() != null && sessionContentItems.ToList<SessionContentItem>().Count > 0)
                    //    return true;
                    //else
                    //    return false;
                   
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return false;
            }

        }

        public static List<SessionContentItem> GetSessionContentItemsOfUndefinedAttendants(long sessionID)
        {
            try
            {

                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    Attendant unknownAtt = context.Sessions.FirstOrDefault(c => c.ID == sessionID).Attendants.FirstOrDefault(c => c.Name == "غير معرف");
                    var sessionContentItems =
                        from sci in context.SessionContentItems
                        //where sci.SessionID == sessionID && sci.Attendant.Type == 7 && sci.AttendantID != unknownAtt.ID
                        where ((sci.SessionID == sessionID) && (sci.AttendantID == unknownAtt.ID))
                        orderby sci.FragOrderInXml
                        select sci;
                    List<SessionContentItem> toRet = new List<SessionContentItem>();
                    if (sessionContentItems != null)
                    {
                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {
                            contentItem.SessionFile = contentItem.SessionFile;
                            contentItem.User = contentItem.User;
                            //contentItem.FileReviewer = contentItem.FileReviewer ;
                            toRet.Add(contentItem);
                        }
                    }
                    return toRet;

                    //if (sessionContentItems != null && sessionContentItems.ToList<SessionContentItem>() != null && sessionContentItems.ToList<SessionContentItem>().Count > 0)
                    //    return true;
                    //else
                    //    return false;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsOfUndefinedAttendants(" + sessionID + ")");
                return null;
            }

        }
        public static List<SessionContentItem> GetSessionContentItemsOfUndefinedAgendaItem(long sessionID)
        {
            try
            {

                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    AgendaItem unknownAgendaItem = context.Sessions.FirstOrDefault(c => c.ID == sessionID).AgendaItems.FirstOrDefault(c => c.Name == "غير معرف");
                    var sessionContentItems =
                        from sci in context.SessionContentItems
                        where ((sci.SessionID == sessionID) && (sci.AgendaItemID == unknownAgendaItem.ID))
                        select sci;

                    List<SessionContentItem> toRet = new List<SessionContentItem>();
                    if (sessionContentItems != null)
                    {
                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {
                            contentItem.SessionFile = contentItem.SessionFile;
                            contentItem.User = contentItem.User;
                            //contentItem.FileReviewer = contentItem.FileReviewer;
                            toRet.Add(contentItem);
                        }
                    }
                    return toRet;

                    //if (sessionContentItems != null && sessionContentItems.ToList<SessionContentItem>() != null && sessionContentItems.ToList<SessionContentItem>().Count > 0)
                    //    return true;
                    //else
                    //    return false;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }

        public static bool DoesSessionContainsUndefinedAgendaItem(long sessionID)
        {
            try
            {

                using (EMadbatahEntities context = new EMadbatahEntities())
                {

                    AgendaItem unknownAgendaItem = context.Sessions.FirstOrDefault(c => c.ID == sessionID).AgendaItems.FirstOrDefault(c => c.Name == "غير معرف");
                    var sessionContentItems =
                        from sci in context.SessionContentItems
                        where ((sci.SessionID == sessionID) && (sci.AgendaItemID == unknownAgendaItem.ID))
                        select sci;

                    if (sessionContentItems == null)
                        return false; // or throw an exception
                    try
                    {
                        return sessionContentItems.Any();
                    }
                    catch
                    {
                        return false;
                    }

                    //if (sessionContentItems != null && sessionContentItems.ToList<SessionContentItem>() != null && sessionContentItems.ToList<SessionContentItem>().Count > 0)
                    //    return true;
                    //else
                    //    return false;

                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return false;
            }

        }

        public static List<SessionContentItem> GetSessionContentItemsBySessionIDAndStatusAndUserID(long sessionID, int statusID, long userID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    var sessionFiles =
                        from sf in context.SessionFiles
                        orderby sf.Order
                        where sf.SessionID == sessionID
                        select sf;

                    foreach (SessionFile file in sessionFiles.ToList<SessionFile>())
                    {
                        var sessionContentItems =
                            from sci in file.SessionContentItems
                            orderby sci.FragOrderInXml
                            select sci;

                        foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                        {

                            if (contentItem.StatusID == statusID && contentItem.UserID == userID)
                                toRet.Add(contentItem);
                        }

                    }
                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetSessionContentItemsBySessionID(" + sessionID + ")");
                return null;
            }

        }

        public static int UpdateSessionContentItem(long sessionContentItemID,string text, long attendantID, long agendaItemID, long? agendaSubItemID, string commentsOnAttendant,string commentsOnText, string FooterText, int sessioContentItemStatusID, bool mergedWithPrev, bool ignored
            )//, float startTime, float endTime, float duration)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        //update the Role attributes
                        updated_session_content_item.Text = text;
                        updated_session_content_item.AttendantID = attendantID;
                        updated_session_content_item.AgendaItemID = agendaItemID;
                        updated_session_content_item.AgendaSubItemID = agendaSubItemID;
                        updated_session_content_item.CommentOnAttendant = commentsOnAttendant;
                        updated_session_content_item.CommentOnText = commentsOnText;
                        updated_session_content_item.PageFooter = FooterText;
                        updated_session_content_item.StatusID = sessioContentItemStatusID;
                        updated_session_content_item.MergedWithPrevious = mergedWithPrev;
                        updated_session_content_item.CreatedDate = DateTime.Now;
                        updated_session_content_item.Ignored = ignored;
                        //updated_session_content_item.StartTime = startTime;
                        //updated_session_content_item.EndTime = endTime;
                        //updated_session_content_item.Duration = duration;
                    }

                    if (!string.IsNullOrEmpty(commentsOnAttendant))
                    {
                        var att = context.Attendants.FirstOrDefault<Attendant>(a => a.ID == attendantID);
                        if (att != null)
                        {
                            att.JobTitle = commentsOnAttendant;
                        }
                    }

                    int res = context.SaveChanges();
                    return res;
                }
               
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItem('')");
                return -1;
            }
        }

        public static int UpdateSessionContentItem(long sessionContentItemID, string text, long attendantID, long agendaItemID,
            long? agendaSubItemID, string commentsOnAttendant, string commentsOnText, string FooterText, int sessioContentItemStatusID, bool updatedByRev , long reviewerID, bool mergedWithPrev, bool ignored
            )//,float startTime,float endTime,float duration)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        //update the Role attributes
                        updated_session_content_item.Text = text;
                        updated_session_content_item.AttendantID = attendantID;
                        updated_session_content_item.AgendaItemID = agendaItemID;
                        updated_session_content_item.AgendaSubItemID = agendaSubItemID;
                        updated_session_content_item.CommentOnAttendant = commentsOnAttendant;
                        updated_session_content_item.CommentOnText = commentsOnText;
                        updated_session_content_item.PageFooter = FooterText;
                        updated_session_content_item.StatusID = sessioContentItemStatusID;
                        updated_session_content_item.UpdatedByReviewer = updatedByRev;
                        updated_session_content_item.ReviewerUserID = reviewerID;
                        updated_session_content_item.MergedWithPrevious = mergedWithPrev;
                        updated_session_content_item.Ignored = ignored;

                        if (updatedByRev)
                            updated_session_content_item.UpdateDate = DateTime.Now;
                        else
                            updated_session_content_item.CreatedDate = DateTime.Now;

                        if (!string.IsNullOrEmpty(commentsOnAttendant))
                        {
                            var att = context.Attendants.FirstOrDefault<Attendant>(a => a.ID == attendantID);
                            if (att != null)
                            {
                               // att.JobTitle = commentsOnAttendant;
                            }
                        }

                        //updated_session_content_item.StartTime = startTime;
                        //updated_session_content_item.EndTime = endTime;
                        //updated_session_content_item.Duration = duration;
                    }
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItem('')");
                return -1;
            }
        }

        public static int UpdateSessionContentItemOrders(long sessionFileID, long currentOrder)//,float startTime,float endTime,float duration)
        {
            try
            {
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    
                    var sessionContentItems =
                           from sci in context.SessionContentItems
                           where sci.SessionFileID == sessionFileID &&
                                    sci.FragOrderInXml > currentOrder
                           select sci;

                    foreach (SessionContentItem contentItem in sessionContentItems)
                    {


                        contentItem.FragOrderInXml = contentItem.FragOrderInXml + 1;
                    }
                    int res = context.SaveChanges();
                    return res;
                }

            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItem('')");
                return -1;
            }
        }

        public static int UpdateSessionContentItemStatus(long sessionContentItemID, int statusID, string note, long revID, bool isUserFileRev)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        updated_session_content_item.StatusID = statusID == null? updated_session_content_item.SessionContentItemStatus.ID : statusID ;
                        updated_session_content_item.ReviewerNote = note;
                        if(isUserFileRev)
                            updated_session_content_item.FileReviewerID = revID;
                        else
                            updated_session_content_item.ReviewerUserID = revID;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemStatus(" + sessionContentItemID + "," + statusID + ")");
                return -1;
            }
        }
        public static int UpdateSessionContentItemForReviewer(long sessionContentItemID, string text , long reviewerUserID, string reviewerNote)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                bool updatedByReviewer = false;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        if (!updated_session_content_item.Text.Equals(text))
                            updatedByReviewer = true;

                        updated_session_content_item.Text = text == null ? updated_session_content_item.Text : text;
                        updated_session_content_item.ReviewerUserID = reviewerUserID == null ? updated_session_content_item.ReviewerUserID : reviewerUserID;
                        updated_session_content_item.ReviewerNote = reviewerNote == null ? updated_session_content_item.ReviewerNote : reviewerNote;
                        updated_session_content_item.UpdatedByReviewer = updatedByReviewer;

                        if (updatedByReviewer)
                            updated_session_content_item.UpdateDate = DateTime.Now;
                        else
                            updated_session_content_item.CreatedDate = DateTime.Now;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemForReviewer(" + sessionContentItemID + "," + reviewerUserID + "," + reviewerNote + ")");
                return -1;
            }
        }
        //public static int UpdateSessionContentItemReviewerNote(long sessionContentItemID, string reviewerNote)
        //{
        //    try
        //    {
        //        SessionContentItem updated_session_content_item = null;
        //        using (EMadbatahEntities context = new EMadbatahEntities())
        //        {
        //            updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
        //            if (updated_session_content_item != null)
        //            {
        //                updated_session_content_item.ReviewerNote = reviewerNote == null ? updated_session_content_item.ReviewerNote : reviewerNote;
        //            }
        //            int res = context.SaveChanges();
        //            return res;
        //        }
                
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemReviewerNote(" + sessionContentItemID + "," + reviewerNote + ")");
        //        return -1;
        //    }
        //}
        public static int UpdateSessionContentItemUpdatedByReviewer(long sessionContentItemID, bool updatedByReviewer)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        updated_session_content_item.UpdatedByReviewer = updatedByReviewer == null? updated_session_content_item.UpdatedByReviewer : updatedByReviewer ;
                        if (updatedByReviewer)
                            updated_session_content_item.UpdateDate = DateTime.Now;
                        else
                            updated_session_content_item.CreatedDate = DateTime.Now;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemUpdatedByReviewer(" + sessionContentItemID + "," + updatedByReviewer + ")");
                return -1;
            }
        }
        public static int UpdateSessionContentItemText(long sessionContentItemID, string text)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        updated_session_content_item.Text = text== null ? updated_session_content_item.Text: text;
                        updated_session_content_item.CreatedDate = DateTime.Now;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemText(" + sessionContentItemID + ")");
                return -1;
            }
        }

        public static int UpdateSessionContentItemAgendaItemID(long sessionContentItemID, long agendaItemID, int statusID)
        {
            try
            {
                SessionContentItem updated_session_content_item = null;
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    updated_session_content_item = context.SessionContentItems.FirstOrDefault(c => c.ID == sessionContentItemID);
                    if (updated_session_content_item != null)
                    {
                        updated_session_content_item.AgendaItemID = agendaItemID;
                        updated_session_content_item.StatusID = statusID;
                        if (updated_session_content_item.AgendaSubItemID != null)
                            updated_session_content_item.AgendaSubItemID = null;
                    }
                    int res = context.SaveChanges();
                    return res;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.UpdateSessionContentItemText(" + sessionContentItemID + ")");
                return -1;
            }
        }

        #endregion

        public static List<SessionContentItem> GetSessionContentItems(long sessionFileID, int statusID, long userID)
        {
            try
            {
                List<SessionContentItem> toRet = new List<SessionContentItem>();
                using (EMadbatahEntities context = new EMadbatahEntities())
                {
                    //var sessionFiles =
                    //    from sf in context.SessionFiles
                    //    orderby sf.Order
                    //    where sf.SessionID == sessionID && sf.ID == sessionFileID
                    //    select sf;

                    SessionFile file = context.SessionFiles.Single(c => c.ID == sessionFileID
                                                                       );// && c.Status == statusID);                    
                    var sessionContentItems =
                        from sci in file.SessionContentItems
                        orderby sci.FragOrderInXml
                        where (sci.StatusID == statusID) && (sci.UserID == userID)
                        select sci;

                    foreach (SessionContentItem contentItem in sessionContentItems.ToList<SessionContentItem>())
                    {
                        contentItem.AgendaItem = contentItem.AgendaItem;
                        contentItem.Reviewer = contentItem.Reviewer;
                        contentItem.FileReviewer = contentItem.FileReviewer;
                        toRet.Add(contentItem);
                    }


                    return toRet;
                }
            }
            catch (Exception ex)
            {
                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.DAL.SessionContentItemHelper.GetRejectedItemsByFileIDAndStatusID(" + sessionFileID + ")");
                return null;
            }

        }
    }
}
