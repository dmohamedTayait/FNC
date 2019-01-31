using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.BLL;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Config;
//using TayaIT.Enterprise.EMadbatah.DAL;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class SessionStart : BasePage
    {
        public long currentSessionId = -1;
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (CurrentUser.Role == Model.UserRole.Reviewer)
            //    Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            if (!Page.IsPostBack)
            {

                if (SessionID != null && long.TryParse(SessionID, out currentSessionId))
                {
                    SessionIDHidden.Value = currentSessionId.ToString();
                    DAL.SessionFile start = SessionStartFacade.GetSessionStartBySessionID(currentSessionId);

                    if (start != null)
                    {
                        if (start.Status == (int)Model.SessionFileStatus.New)
                        {
                            elm1.InnerHtml = SessionStartFacade.GetAutomaticSessionStartText(currentSessionId);
                            TayaIT.Enterprise.EMadbatah.DAL.SessionFileHelper.UpdateSessionFileStatus(start.ID, (int)Model.SessionFileStatus.InProgress, null);
                        }
                        else
                            elm1.InnerHtml = start.SessionStartText;
                        
                        SessionFileIDHidden.Value = start.ID.ToString();
                        SessionFileFacade.LockSessionFile(start.ID, CurrentUser.ID, false);
                        
                    }
                    else
                    {
                        string madbatahStart = SessionStartFacade.GetAutomaticSessionStartText(currentSessionId);
                        elm1.InnerHtml = madbatahStart;
                    }

                    
                }
                else
                {
                    ShowMainError(GetLocalizedString("strNoQueryStr"));                    
                }
            }

        }
        /*protected void saveBtn_Click(Object sender, EventArgs e)
        {
            // When the button is clicked,
            // change the button text, and disable it.
            long sessionId;
            if (SessionID != null && long.TryParse(SessionID, out sessionId))
            {
                SessionStartFacade.AddUpdateSessionStart(sessionId, elm1.InnerHtml, CurrentUser.ID);
            }
        }*/
    }
}