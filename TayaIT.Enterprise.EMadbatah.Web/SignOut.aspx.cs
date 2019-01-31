using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.Util;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using System.Web.Security;


namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class SignOut : BasePage
    {
        private int _authenticationAttempts = 0;
        public int AuthenticationAttempts
        {
            get
            {
                if (!string.IsNullOrEmpty(string.Format("{0}", Session["AuthenticationAttempts"])))
                {
                    int.TryParse(Session["AuthenticationAttempts"].ToString(), out _authenticationAttempts);
                }

                return _authenticationAttempts;
            }
            set
            {
                _authenticationAttempts = value;
                Session["AuthenticationAttempts"] = _authenticationAttempts;
            }
        }
        

        /// <summary>
        /// This event fires on every page load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            // Make sure the browser does not cache this page
            this.DisablePageCaching();


            switch (SignOutTypeVal)
            {
                case SignOutType.SignInAsDifferentUser:
                    ShowWarn(GetLocalizedString("strSignInAsDifferentUserWarning"));
                    // Increase authentication attempts
                    this.AuthenticationAttempts = this.AuthenticationAttempts + 1;

                    if (this.AuthenticationAttempts == 1)
                    {
                        // Change previous user to current user
                        PreviousUser = CurrentUser;

                        // Send the first 401 response
                        this.Send401();
                    }
                    else
                    {
                        // When a browser is set to "automaticaly sign in with current credentials", we have to send two 401 responses to let the browser re-authenticate itself.
                        // I don't know how to determine if a browser is set to "automaticaly sign in with current credentials", so two 401 responses are always send when the user
                        // does not switch accounts. In Micrososft Office sharepoint the user has to supply the credentials 3 times, when the user does not switch accounts,
                        // so it think this is not a problem.

                        //usama
                        if (CurrentUser == null && PreviousPage == null)
                        {
                            Response.Redirect("Default.aspx");
                        }

                        if (AuthenticationAttempts == 2 && CurrentUser.Equals(PreviousUser))
                        {
                            // Send the second 401 response
                            this.Send401();
                        }
                        else
                        {
                            // Clear the session of the current user. This will clear all sessions objects including the "AuthenticationAttempts"
                            Session.Abandon();
                            Session.Clear();

                            // Redirect back to the main page
                            Response.Redirect("Default.aspx");
                        }
                    }
                    break;
                case SignOutType.SignOut:
                    ClientScript.RegisterClientScriptBlock(typeof(string), "closewindow", "window.close();");
                    ShowWarn(GetLocalizedString("strSignOutWarning"));
                     HttpContext.Current.Session.Clear();
            HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
                    break;
                default:
                    break;
            }

           
        }

        /// <summary>
        /// Make sure the browser does not cache this page
        /// </summary>
        public void DisablePageCaching()
        {
            Response.Expires = 0;
            Response.Cache.SetNoStore();
            Response.AppendHeader("Pragma", "no-cache");


            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.MinValue);
            

           
        }
        /// <summary>
        /// Send a 401 response
        /// </summary>
        public void Send401()
        {
            // Create a 401 response, the browser will show the log-in dialogbox, asking the user to supply new credentials,             // if browser is not set to "automaticaly sign in with current credentials"
            Response.Buffer = true;
            Response.StatusCode = 401;
            Response.StatusDescription = "Unauthorized";

            // A authentication header must be supplied. This header can be changed to Negotiate when using keberos authentication
            Response.AddHeader("WWW-Authenticate", "NTLM");

            // Send the 401 response
            Response.End();
        }

        public SignOutType SignOutTypeVal { 
            get 
        {
            string signoutTypeVal = WebHelper.GetQSValue(Constants.QSKeyNames.SIGN_OUT_TYPE, this.Context);
                SignOutType type2Return = SignOutType.SignOut;
                if (!string.IsNullOrEmpty(signoutTypeVal))
                {
                    Enum.TryParse<SignOutType>(signoutTypeVal, out type2Return); 
                }
                return type2Return;

        } 
        }
    }
}

