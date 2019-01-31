using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.DAL;
using TayaIT.Enterprise.EMadbatah.Web;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class AddNewMadbatah : BasePage
    {
        public static bool checher = false;
        protected void Page_Load(object sender, EventArgs e)
        {


        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            ContentPlaceHolder mpContentPlaceHolder;
            TextBox mpTextBox;
            mpContentPlaceHolder =
              (ContentPlaceHolder)Master.FindControl("MainContent");
            if (mpContentPlaceHolder != null)
            {
                mpTextBox =
                    (TextBox)mpContentPlaceHolder.FindControl("txtMadbatahTitle");
                Session sessionObj = new Session();
                sessionObj.Date = DateTime.Now;

                sessionObj.President = "Dina";
                sessionObj.Subject = mpTextBox.Text;
                sessionObj.Stage = 1;
                sessionObj.StageType = "123/l";
                sessionObj.SessionStatusID = (int)Model.SessionStatus.New;
                long id = SessionHelper.AddNewSession(sessionObj, "test");
            }
        }
    }
}