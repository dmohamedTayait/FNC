using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.Model;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class Error : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ErrorType type = Model.ErrorType.Generic; 
            if (ErrorType != null 
                && Enum.TryParse<ErrorType>(ErrorType, out type))
            {
                divMessage.InnerHtml = GetLocalizedString("str" + type.ToString());
            }
        }


        public string ErrorType
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.ERROR_TYPE, Context);

            }
        }
    } 


}
