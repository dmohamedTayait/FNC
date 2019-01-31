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
    public static bool fileErrorChecker = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        String SessionFileID = "";
        string val = Context.Request.QueryString[TayaIT.Enterprise.EMadbatah.Config.Constants.QSKeyNames.SESSION_ID];
        if (!string.IsNullOrEmpty(val) && val.Trim() != string.Empty)
        {
            SessionFileID =  val;
        }
        else
            SessionFileID = null;
        if (SessionHelper.GetSessionFileError(long.Parse(SessionFileID)) == 0)
        {
            SessionHelper.UpdateSessionFileError(long.Parse(SessionFileID), 1);
            fileErrorChecker = true;
        }
        else
        {
            SessionHelper.UpdateSessionFileError(long.Parse(SessionFileID), 0);
            fileErrorChecker = false;
        }
           
    }
}