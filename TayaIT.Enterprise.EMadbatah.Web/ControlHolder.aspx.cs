using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Util.Web;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class ControlHolder : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
    {
        //string chartControlPath = Server.MapPath("~\\UserControls\\Chart.asax");
        //if (!Page.IsPostBack && AjaxFunctionName != null)
        //{
        //    BaseUserControl uc = (BaseUserControl)LoadControl(chartControlPath);

        //    string [] arrNames =  { "Peter", "Andrew", "Julie", "Mary", "Dave"};
        //    int [] arrVals =  { 2, 6, 4, 5, 3};

        //    uc.XVals = arrNames;
        //    uc.YVals = arrVals;
        //    this.Controls.Add(uc);


        }

        public string AjaxFunctionName
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.AJAX_FUNCTION_NAME, HttpContext.Current);
            }
        }
    }
}
