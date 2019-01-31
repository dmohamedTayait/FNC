using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Config;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class EditIndexItems : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //if (CurrentUser.Role == Model.UserRole.DataEntry)
            //    Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            long sessionID = -1;
            //if (!Page.IsPostBack && SessionID != null && long.TryParse(SessionID, out sessionID))
            //{
            //    List<MadbatahIndexItem> index = new List<MadbatahIndexItem>();
            //    List<List<SessionContentItem>> allItems = SessionContentItemHelper.GetItemsBySessionIDGrouped(sessionID);

            //    List<string> writtenAgendaItems = new List<string>();

            //    foreach (List<SessionContentItem> groupedItems in allItems)
            //    {
            //         AgendaItem curAgendaItem = groupedItems[0].AgendaItem;
            //        AgendaSubItem curAgendaSubItem = groupedItems[0].AgendaSubItem;

            //        if (writtenAgendaItems.IndexOf(curAgendaItem.Name) == -1)
            //        {
            //            string originalName = curAgendaItem.Name;
            //            writtenAgendaItems.Add(curAgendaItem.Name);

            //            index.Add(new MadbatahIndexItem(originalName, 0, true, "", "") { ID = curAgendaItem.ID });
            //        }

            //        if (curAgendaSubItem != null)
            //        {
            //            index.Add(new MadbatahIndexItem(curAgendaSubItem.Name, 0, false, curAgendaSubItem.QFrom, curAgendaSubItem.QTo) { ID = curAgendaSubItem.ID });
            //        }
            //    }

            //    Response.Write(index);
            //}

        }
    }
}
