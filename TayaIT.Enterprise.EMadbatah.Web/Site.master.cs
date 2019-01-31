using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.BLL;
using System.Security.Principal;
using System.Web.Security;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class SiteMaster : System.Web.UI.MasterPage
    {
        public Preferences _preferences = null;

        protected void Page_Load(object sender, EventArgs e)
        {
            //string user_name =  (TayaIT.Enterprise.EMadbatah.Model.EMadbatahUser)_context.Session[Constants.SessionObjectsNames.CURRENT_USER].Name;
            //InitializeCulture();
        }

        public EMadbatahUser CurrentUser
        {
            get
            {
                return (EMadbatahUser)Session[Constants.SessionObjectsNames.CURRENT_USER];
            }          
        }
        public string CurrentDomain
        {
            get
            {
                return (string)Session[Constants.SessionObjectsNames.CURRENT_DOMAIN];
            }           
        }


}
}

