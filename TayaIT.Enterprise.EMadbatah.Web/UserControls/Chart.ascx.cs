using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class UserControls_Chart : BaseUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Chart1.Series["Series 1"].Points.DataBindXY(XVals, YVals);
        }
    }
}