using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Web;

public partial class Check : BasePage
{
    public static bool checher = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        String SessionID = "";
        string val = Context.Request.QueryString[TayaIT.Enterprise.EMadbatah.Config.Constants.QSKeyNames.SESSION_ID];
        if (!string.IsNullOrEmpty(val) && val.Trim() != string.Empty)
        {
            SessionID =  val;
        }
        else
            SessionID = null;
        if (SessionHelper.GetSessionChecker(long.Parse(SessionID)) == 0)
        {
            SessionHelper.UpdateSessionChecker(long.Parse(SessionID), 1);
            checher = true;
        }
        else
        {
            SessionHelper.UpdateSessionChecker(long.Parse(SessionID), 0);
            checher = false;
        }
           
    }
}