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
    public partial class _Default : BasePage
    {
        JavaScriptSerializer serializer = new JavaScriptSerializer();

        protected void Page_Load(object sender, EventArgs e)
        {
          


             //           MasterPage master = this.Master;
           // master.RemoveHTMLTags(


           // litStartupScript.Text = "<script type=\"text/javascript\"> var constants =  eval(" + serializer.Serialize(Constants.QSKeyNames) + "); alert(constants.AJAX_FUNCTION_NAME); </script>";
            if (!Page.IsPostBack)
            {

                UserRuleHidden.Value = CurrentUser.Role.ToString();

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
                                //InitializeVecsysNewSession(sd);
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
                else
                {
                    int sessionsCount = EMadbatahFacade.GetSessionsCount();
                    if (sessionsCount == 0)
                    {
                        mainContent.Visible = false;
                        ShowInfo(GetLocalizedString("strNoSessionsExistWarn"));
                    }
                }
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

        //prepare send session files mp3
        public void InitializeVecsysNewSession(SessionDetails sd)
        {
            selectVecsysFolders.Items.Clear();
            //if it is new from database we show selection and confirm from vecsys path
            //string sessionPath = Path.Combine(AppConfig.GetInstance().VecSysServerPath, sd.Season.ToString(), sd.Stage.ToString());
            string mp3AudioServerPath = AppConfig.GetInstance().AudioServerPath;
            string xmlPath = AppConfig.GetInstance().VecSysServerPath;
            
            //if folder of vecsys exist the voice to text done and everything is fine
            if (Directory.Exists(mp3AudioServerPath) && Directory.Exists(xmlPath))
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
                //btnConfirm.Attributes.Add("onclick", "confirmNewSession(" + sd.SessionID + ");");
                btnConfirm.Attributes.Add("data-sid", sd.SessionID.ToString());
            }
            else
            {
                //warn user that the session dirctory is not found on voice to text server and 
                //the user can not confirm to start new session
                spnWarn.Visible = true;
                spnWarn.InnerText = GetLocalizedString("strMsgWarnNoVecsysFolder");
            } 
        }


        #region paging
        //const int MAX_DISPLAYED_PAGES = 10;
        //private int _totalResults;
        //public string BuildPagingLinks(StringBuilder sb)
        //{
        //    int currentPageNo = 0;
        //    int resPerPage = 10;

        //    if (ItemsPerPage != null && int.TryParse(ItemsPerPage, out resPerPage)
        //        && CurrentPageNo != null && int.TryParse(CurrentPageNo, out currentPageNo))
        //    {
        //        string arabicNumbers = "٠١٢٣٤٥٦٧٨٩", englishNumbers = "0123456789";

        //        bool blnAppendPrev, blnAppendNext, appendedhLinksContDiv;
        //        blnAppendNext = true;
        //        appendedhLinksContDiv = true;

        //        StringBuilder bld = new StringBuilder();
        //        bld.Append("<div class=\"paging_links\">\n");//container
        //        int currentPage = tempCommand.StartIndexArg / (resPerPage - 1);

        //        if (currentPage >= 10 && currentPage < 20)
        //            currentPage = currentPage - 1;
        //        if (currentPage >= 20 && currentPage < 30)
        //            currentPage = currentPage - 2;
        //        if (currentPage >= 30 && currentPage < 40)
        //            currentPage = currentPage - 3;
        //        if (currentPage >= 40 && currentPage < 50)
        //            currentPage = currentPage - 4;
        //        if (currentPage >= 50 && currentPage < 60)
        //            currentPage = currentPage - 5;
        //        if (currentPage >= 60 && currentPage < 70)
        //            currentPage = currentPage - 6;
        //        if (currentPage >= 70 && currentPage < 80)
        //            currentPage = currentPage - 7;

        //        int startIndex;
        //        int endIndex;

        //        if (currentPage < MAX_DISPLAYED_PAGES / 2)
        //        {
        //            startIndex = 0;
        //            endIndex = MAX_DISPLAYED_PAGES;
        //            //set the pervious HTML to be not vaisible litPrevious.Visible = false;
        //            blnAppendPrev = false;
        //        }
        //        else
        //        {
        //            startIndex = Math.Abs(currentPage - MAX_DISPLAYED_PAGES / 2);
        //            endIndex = currentPage + MAX_DISPLAYED_PAGES / 2 + 1;
        //            //set the pervious HTML to be vaisible  //litPrevious.Visible = true;
        //            blnAppendPrev = true;

        //        }
        //        //for (int i = startIndex; i < endIndex; i++)
        //        for (int i = endIndex - 1; i >= startIndex; i--)
        //        {
        //            int nextStartIndex = tempCommand.ResultsPerPageArg * i;
        //            if (nextStartIndex < TotalResults)
        //            {
        //                //next line is used in case of complex Search mode
        //                //tempCommand.HitsParameters.Start = nextStartIndex; 

        //                //next line is used in case of Arg Based Search mode
        //                tempCommand.StartIndexArg = nextStartIndex;

        //                int pageIndex = (nextStartIndex % tempCommand.ResultsPerPageArg) + i;
        //                string pageIndexToShow = (pageIndex + 1).ToString();
        //                if (pageIndex != currentPage)
        //                {
        //                    if (blnAppendPrev)
        //                    {
        //                        //tempCommand.StartIndex -= tempCommand.ResultsPerPage;
        //                        SearchQuery cmd = (SearchQuery)tempCommand.Clone();
        //                        cmd.StartIndexArg = (currentPage - 1) * cmd.ResultsPerPageArg + 1;
        //                        bld.AppendFormat("<a class=\"previousPage\" href='{0}' >{1}</a>", SearchHelper.GetSearchURL(cmd), GetLocalizedString("strPrevious"));
        //                        blnAppendPrev = false;
        //                    }
        //                    if (appendedhLinksContDiv)
        //                    {
        //                        bld.Append("");
        //                        appendedhLinksContDiv = false;
        //                    }

        //                    if (Preferences.UiLang.ToString().Equals("ar"))
        //                    {
        //                        string arabicNum = "";
        //                        foreach (char ch in pageIndexToShow)
        //                        {
        //                            arabicNum += arabicNumbers[int.Parse(ch.ToString())].ToString();
        //                        }
        //                        //arabicNum = arabicNumbers[pageIndex + 1].ToString();
        //                        pageIndexToShow = arabicNum;
        //                    }

        //                    //bld.AppendFormat("<a href='{0}'>{1}</a>", SearchHelper.GetSearchURL(tempCommand), pageIndex + 1);
        //                    bld.AppendFormat("<a href='{0}'>{1}</a>&nbsp;", SearchHelper.GetSearchURL(tempCommand), pageIndexToShow);
        //                }
        //                else
        //                {
        //                    if (Preferences.UiLang.ToString().Equals("ar"))
        //                    {
        //                        string arabicNum = "";
        //                        foreach (char ch in pageIndexToShow)
        //                        {
        //                            arabicNum += arabicNumbers[int.Parse(ch.ToString())].ToString();
        //                        }
        //                        //arabicNum = arabicNumbers[pageIndex + 1].ToString();
        //                        pageIndexToShow = arabicNum;
        //                    }

        //                    if (appendedhLinksContDiv)
        //                    {
        //                        bld.Append("\n");
        //                        appendedhLinksContDiv = false;
        //                    }
        //                    bld.AppendFormat("<span class=\"current \">{0}</span>&nbsp;", pageIndexToShow);
        //                }
        //            }
        //            else
        //            {
        //                //we will not append any of the paging HTML
        //                //litNext.Visible = false;
        //                blnAppendNext = false;
        //            }
        //        }

        //        bld.Append("");

        //        if (blnAppendNext)
        //        {
        //            //currentPage
        //            SearchQuery cmd = (SearchQuery)tempCommand.Clone();
        //            cmd.StartIndexArg = ((currentPage + 1) * cmd.ResultsPerPageArg);
        //            SearchQuery lastPageCmd = (SearchQuery)tempCommand.Clone();
        //            int lastPageIndex = ((MAX_DISPLAYED_PAGES * 10) - 1) * lastPageCmd.ResultsPerPageArg;
        //            lastPageCmd.StartIndexArg = lastPageIndex;
        //            bld.AppendFormat("<a href='{0}' class=\"nextPage\" >{1}</a>&nbsp;", SearchHelper.GetSearchURL(cmd), GetLocalizedString("strNext"));
        //            bld.AppendFormat("<a href='{0}' class=\"nextPage\" >{1}</a>&nbsp;", SearchHelper.GetSearchURL(lastPageCmd), GetLocalizedString("strLast"));
        //        }

        //        //bld.Append("</div>\n");
        //        bld.Append("</div>\n");
        //        return bld.ToString();
        //    }

        //}


        #endregion paging








    }
}

