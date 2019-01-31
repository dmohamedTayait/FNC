using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.BLL;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class fnc_sessions : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           // DateTime tt = new DateTime();
           // tt.AddSeconds(100);

          //  DateTime segStartTime = new DateTime();
     //
            //        segStartTime = sessionStartTime.AddSeconds(prevTiming).AddSeconds((double)item.StartTime);
               


           Eparliment ep = new Eparliment();
            bool result = ep.IngestContentsForFinalApprove(long.Parse(SessionID));
        }
    }
}