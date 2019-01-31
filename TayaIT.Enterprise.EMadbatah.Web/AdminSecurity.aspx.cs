using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Config;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class AdminSecurity : BasePage 
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Role != Model.UserRole.Admin)
                Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            UserId.Value = CurrentUser.ID.ToString();
            if (!Page.IsPostBack)
            {
                List<EMadbatahUser> users = EMadbatahFacade.GetUsersFromDB(false);
                StringBuilder sb = new StringBuilder();
                foreach (EMadbatahUser user in users)
                {

                    string emadbatahUserRow = Application[Constants.HTMLTemplateFileNames.EMadbatahUserRow].ToString();

                    
                    emadbatahUserRow = emadbatahUserRow.Replace("<%UserID%>", user.ID+"");
                    emadbatahUserRow = emadbatahUserRow.Replace("<%Name%>", user.Name);
                    emadbatahUserRow = emadbatahUserRow.Replace("<%DomainUserName%>", user.DomainUserName);
                    emadbatahUserRow = emadbatahUserRow.Replace("<%Email%>", user.Email);
                    emadbatahUserRow = emadbatahUserRow.Replace("<%RoleStr%>", GetLocalizedString("strRole"+ (int)user.Role));
                    emadbatahUserRow = emadbatahUserRow.Replace("<%Role%>", ((int)user.Role).ToString());
                    emadbatahUserRow = emadbatahUserRow.Replace("<%DomainUserName%>", user.DomainUserName);
                    if (user.IsActive)
                        emadbatahUserRow = emadbatahUserRow.Replace("<%IsActive%>", "").Replace("<%ActiveStr%>", "نشط");
                    else
                        emadbatahUserRow = emadbatahUserRow.Replace("<%IsActive%>", "class=\"inactive\"").Replace("<%ActiveStr%>", "غير نشط");

                    if (user.DomainUserName == AppConfig.GetInstance().MainAdmin.DomainUserName)
                        emadbatahUserRow = emadbatahUserRow.Replace("<%Show%>", "");
                    else
                        emadbatahUserRow = emadbatahUserRow.Replace("<%Show%>", "<div class=\"editbuttons\"><a href=\"#\" class=\"edit\">[تعديل]</a> <a class=\"remove\" href=\"#inline2\">[حذف]</a></div>");

                    sb.Append(emadbatahUserRow);

                    /*sb.Append("<tr id=\"row"+user.ID + "\">");
                    sb.Append("<td>").Append(user.Name).Append("</td>");
                    sb.Append("<td>").Append(user.DomainUserName).Append("</td>");
                    sb.Append("<td>").Append(user.Email).Append("</td>");
                    sb.Append("<td id=\"role" + (int)user.Role + "\">").Append(user.Role).Append("</td>");
                    if (user.IsActive)
                        sb.Append("<td>").Append("Active").Append("</td>");
                    else
                         sb.Append("<td class=\"inactive\" >").Append("Active").Append("</td>");
                        

                    sb.Append("<td class=\"editbuttons\"><a href=\"#\" class=\"edit\">[تعديل]</a> <a href=\"#\" class=\"remove\">[حذف]</a></td>");

                    sb.Append("</tr>");*/
                }

                db_users.InnerHtml = sb.ToString();
            }
        }
    }
}