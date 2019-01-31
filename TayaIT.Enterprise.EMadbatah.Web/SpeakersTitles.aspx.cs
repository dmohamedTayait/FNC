using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Model;
using System.Text;
using System.IO;
using TayaIT.Enterprise.EMadbatah.Config;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Model.VecSys;
using TayaIT.Enterprise.EMadbatah.Util;
using System.Web.Script.Serialization;
namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class SpeakersTitles : BasePage
    {

        public List<EMadbatahUser> usersdb = null;
        public List<EMadbatahUser> revusersdb = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (CurrentUser.Role != Model.UserRole.Admin)
                Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            //           MasterPage master = this.Master;
            // master.RemoveHTMLTags(
            int sessionsCount = EMadbatahFacade.GetSessionsCount();
            if (sessionsCount == 0)
            {
                mainContent.Visible = false;
                ShowInfo(GetLocalizedString("strNoSessionsExistWarn"));
            }

            // litStartupScript.Text = "<script type=\"text/javascript\"> var constants =  eval(" + serializer.Serialize(Constants.QSKeyNames) + "); alert(constants.AJAX_FUNCTION_NAME); </script>";
            if (!Page.IsPostBack)
            {
                usersdb = EMadbatahFacade.GetUsersFromDB(false);
                revusersdb = EMadbatahFacade.GetRevUsersFromDB();

                if (SessionIDEParliment != null)
                {
                    int sessionIDEparlimet = -1;
                    if (int.TryParse(SessionIDEParliment, out sessionIDEparlimet))
                    {
                        //we have new session
                        //check if session is in DB 

                        SessionDetails sd = EMadbatahFacade.GetSessionByEParlimentID(sessionIDEparlimet);
                        if (sd != null)
                        {
                            //exist in db 
                            //check for its status
                            if (sd.Status == SessionStatus.New)
                            {
                                //initialize new session from vecsys folder            
                                InitializeVecsysNewSession(sd);
                            }

                        }
                        else
                        {
                            //very new we need to get its data from web service then we insert in database

                            //try catch web service dead // display error

                            SessionDetails newSessionDetails = null;

                            try
                            {
                                Eparliment ep = new Eparliment();
                                newSessionDetails = ep.GetSessionDetails(sessionIDEparlimet);
                                if (newSessionDetails == null)
                                    spnWarn.InnerText = GetLocalizedString("strMsgErrorEPService"); // sssion doesn't exist in e-parliament
                            }
                            catch (Exception ex)
                            {
                                LogHelper.LogException(ex, "TayaIT.Enterprise.EMadbatah.Web._Default.Page_Load()");
                                spnWarn.InnerText = GetLocalizedString("strMsgErrorEPService");
                            }

                            if (newSessionDetails != null)
                            {
                                //send to DB
                                sd = EMadbatahFacade.AddNewSessionDetailsToDB(newSessionDetails);
                                if (sd.SessionID != -1)
                                {
                                    //initialize new session from vecsys folder            
                                    InitializeVecsysNewSession(sd);
                                }
                            }
                        }
                    }
                }



            }
        }

        //prepare send session files mp3
        //prepare send session files mp3
        public void InitializeVecsysNewSession(SessionDetails sd)
        {
            //if it is new from database we show selection and confirm from vecsys path
            //string sessionPath = Path.Combine(AppConfig.GetInstance().VecSysServerPath, sd.Season.ToString(), sd.Stage.ToString());
            string mp3AudioServerPath = AppConfig.GetInstance().AudioServerPath;
            string xmlPath = AppConfig.GetInstance().AudioServerPath;

            //if folder of vecsys exist the voice to text done and everything is fine
            if (Directory.Exists(mp3AudioServerPath) && Directory.Exists(mp3AudioServerPath))
            {
                //show dropdown containing selected folder that user need to confirm
                DirectoryInfo mp3Dirinfo = new DirectoryInfo(mp3AudioServerPath);
                DirectoryInfo xmlDirinfo = new DirectoryInfo(xmlPath);



                List<DirectoryInfo> dirs = mp3Dirinfo.GetDirectories().ToList<DirectoryInfo>();
                //chose dir to default select in dropdown
                string dirToChose = GetDirNameToSelect(dirs, sd.Serial.ToString(), sd.Date);
                //TransFile transFile = Vecsys.VecsysParser.ParseTransXml(sessionPath);

                selectVecsysFolders.Visible = true;
                foreach (DirectoryInfo dir in dirs)
                {
                    ListItem li = new ListItem(dir.Name, HttpUtility.UrlEncode(dir.FullName));
                    li.Attributes.Add("data-xmlServerPath", HttpUtility.UrlEncode(xmlDirinfo.FullName));
                    selectVecsysFolders.Items.Add(li);
                }

                if (!string.IsNullOrEmpty(dirToChose))
                {
                    selectVecsysFolders.SelectedIndex = selectVecsysFolders.Items.IndexOf(selectVecsysFolders.Items.FindByText(dirToChose));
                }
                else
                {
                    selectVecsysFolders.Items.Insert(0, new ListItem(GetLocalizedString("strDDLSelect"), ""));
                    selectVecsysFolders.SelectedIndex = 0;
                }

                //display confirm button
                btnConfirm.Visible = true;
                btnConfirm.Attributes.Add("onclick", "confirmNewSession(" + sd.SessionID + ");");
            }
            else
            {
                //warn user that the session dirctory is not found on voice to text server and 
                //the user can not confirm to start new session
                spnWarn.Visible = true;
                spnWarn.InnerText = GetLocalizedString("strMsgWarnNoVecsysFolder");
            }
        }


        public string GetDirNameToSelect(List<DirectoryInfo> dirs, string sessionSerial, DateTime sessionDate)
        {
            string dirToChose = string.Empty;
            foreach (DirectoryInfo dir in dirs)
            {
                if (dir.Name.Contains(sessionSerial.ToString())
                    && dir.Name.Contains(sessionDate.Day.ToString("00") + "-" + sessionDate.Month.ToString("00") + "-" + sessionDate.Year.ToString()))
                {
                    dirToChose = dir.Name;
                    break;
                }
            }
            return dirToChose;
        }
    }
}