using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class AdminAPPConfig : BasePage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(CurrentUser.Role != Model.UserRole.Admin)
                Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            AppConfig _app = AppConfig.GetInstance();
            vecsysPath.Value = _app.VecSysServerPath;
            EparliamantUrl.Value = _app.EPrlimentServerURL;
            audioPath.Value = _app.AudioServerPath;
        }
    }
}
