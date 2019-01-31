using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Collections;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class Statistics : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Role == Model.UserRole.DataEntry)
                Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            long sessionID = -1;
            if (!Page.IsPostBack && SessionID != null && long.TryParse(SessionID, out sessionID))
            {
                //bool isDataToAgendaItem = true;
                //long agendaItemID = 487;


                SessionDetails sd = BLL.EMadbatahFacade.GetSessionDetailsBySessionID(sessionID);
                List<SessionAttendant> attendants = new List<SessionAttendant>();
                Hashtable allSpeakersTiming = new Hashtable();
                Hashtable allSpeakers = new Hashtable();
                Hashtable agendaItemSpeakers = new Hashtable();

                //bind Agenda Items Drop Down List


                ListItem li;
                ddlAgendaItems.DataSource = sd.AgendaItems.Values;
                ddlAgendaItems.DataValueField = "ID";
                ddlAgendaItems.DataTextField = "Text";
                ddlAgendaItems.DataBind();
                li = new ListItem("-------- اختر البند --------", "0");
                ddlAgendaItems.Items.Insert(0, li);

                //bind Agenda Items Drop Down List
               // ddlAgendaSubItems.Enabled = false;
                ddlAgendaItems.Enabled = false;


            }


        }
    }
}