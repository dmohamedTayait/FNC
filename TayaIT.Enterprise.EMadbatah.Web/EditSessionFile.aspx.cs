using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.Model.VecSys;
using TayaIT.Enterprise.EMadbatah.Vecsys;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Collections;
using TayaIT.Enterprise.EMadbatah.Util.Web;
using TayaIT.Enterprise.EMadbatah.Config;
using System.Text;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class EditSessionFile : BasePage
    {
        

        long sessionId;
        long session_file_id;
        Session current_session;
        public SessionAudioFile file;
        private EditorPageMode currentPageMode = EditorPageMode.Edit;
        protected void Page_Load(object sender, EventArgs e)
        {
            int tmpPageMode = 1;
            if(EditPageMode != null && 
                int.TryParse(EditPageMode, out tmpPageMode))
            {
                currentPageMode = (EditorPageMode)tmpPageMode;
            }

            hdPageMode.Value = ((int)currentPageMode).ToString();
                

            if (SessionFileID != null && long.TryParse(SessionFileID, out session_file_id))
            {
                // load the current session details and file details
                bool flag = false;
                file = SessionFileFacade.GetSessionFilesByID(session_file_id);
                if (file.UserID == null)
                {
                    flag = SessionFileFacade.LockSessionFile(session_file_id, CurrentUser.ID, false);
                }

                current_session = EditorFacade.GetSessionByID(file.SessionID);

                if (CurrentUser.ID == file.UserID || flag
                    || ((CurrentUser.Role != UserRole.DataEntry) && (current_session.ReviewerID == CurrentUser.ID)))
                {
                    sessionId = file.SessionID;
                    sessionID.Value = sessionId.ToString();
                    bool outReedit = false;
                    if (ReEdit != null && bool.TryParse(ReEdit, out outReedit))
                    {
                        if (outReedit)
                        {
                            SessionFileHelper.UpdateSessionFileFragOrderAndModifiedDate(session_file_id, 0, DateTime.Now);
                            file.LastInsertedFragNumInXml = 0;
                        }
                    }
                    // bind data
                    BindData();
                }
                else
                {
                    // redirect to message notifaction page ""
                    editSessionFileForm.Visible = false;
                    ShowWarn("عفواً . غير مسموح لك بالتعديل في هذه الصفحة");
                }

            }
        }
        private string getHTML(Paragraph p)
        {
            string output = "";
            foreach (SpeechSegment segment in p.speechSegmentsList)
            {
                foreach (TayaIT.Enterprise.EMadbatah.Model.VecSys.Word word in segment.words)
                {
                    output += "<span class='segment' data-stime='" + word.stime.ToString() + "'>" + word.value + "</span> ";
                }
            }
            return output;
        }
        private void BindData()
        {
            string html; // editor rendered HTML text
            Hashtable current_session_info = new Hashtable(); // to store required values in user session
            //int FragOrder = Request.QueryString["action"] == "continue" ? (int)file.LastInsertedFragNumInXml : 0;
            int FragOrder = (int)file.LastInsertedFragNumInXml;            

            //ibrahim: need to know how many fargments are there in the file and this will be done once
            // set XML,MP3 file path
            string xmlFilePath = Context.Server.MapPath("~") + file.Name.ToLower().Replace(".mp3", ".trans.xml");
            string mp3FilePath = string.Format("{0}://{1}:{2}/", Request.Url.Scheme, Request.Url.Host, Request.Url.Port) + file.Name.Substring(1).Replace(@"\", "/");
            //AppConfig.GetInstance().RootUrl + file.Name.Substring(1).Replace(@"\", "/");
            XmlFilePath.Value = xmlFilePath;
            MP3FilePath.Value = mp3FilePath;
            // parse XML file
            TransFile tf = new TransFile();
            tf = VecsysParser.ParseTransXml(xmlFilePath);
            List<Paragraph> p = VecsysParser.combineSegments(tf.SpeechSegmentList);

            SessionContentItem lastContentItem = null;
            long tmpSessionContentItemID = 0;
            if ((currentPageMode == EditorPageMode.Correct || currentPageMode == EditorPageMode.Review)
                && SessionContentItemID != null
                && long.TryParse(SessionContentItemID, out tmpSessionContentItemID))
            {
                lastContentItem = SessionContentItemHelper.GetSessionContentItemById(tmpSessionContentItemID);
            }
            else
            {
                lastContentItem = SessionContentItemFacade.GetSessionContentItemByIdAndFragmentOrder(session_file_id, FragOrder);
            }
            //corrector

            if (lastContentItem == null) // load from xml
            {

                //// set XML,MP3 file path
                //string xmlFilePath = file.Name.ToLower().Replace(".mp3", ".trans.xml");
                //string mp3FilePath = file.Name;
                //MP3FilePath.Value = mp3FilePath;
                //// parse XML file
                //TransFile tf = new TransFile();
                //tf = VecsysParser.ParseTransXml(xmlFilePath);
                //List<Paragraph> p = VecsysParser.combineSegments(tf.SpeechSegmentList);
                //// compose HTML spans from speech segments and set editor value
                html = getHTML(p[FragOrder]);
                current_session_info["PargraphStartTime"] = p[FragOrder].speechSegmentsList[0].stime;
                current_session_info["PargraphEndTime"] = p[FragOrder].speechSegmentsList[p[FragOrder].speechSegmentsList.Count - 1].etime;
                startTime.Value = p[FragOrder].speechSegmentsList[0].stime.ToString();
                endTime.Value = p[FragOrder].speechSegmentsList[p[FragOrder].speechSegmentsList.Count - 1].etime.ToString();
            }
            else // load from DB
            {
                

                html = lastContentItem.Text;
                current_session_info["PargraphStartTime"] = lastContentItem.StartTime;
                current_session_info["PargraphEndTime"] = lastContentItem.EndTime;
                hdSessionContentItemID.Value = tmpSessionContentItemID.ToString(); ;
                startTime.Value = lastContentItem.StartTime.ToString();
                endTime.Value = lastContentItem.EndTime.ToString();

            }
            // set editor text
            elm1.Value = html;


            //get prev item from db if exist
            if (FragOrder - 1 >= 0)
            {
                SessionContentItem prevContentItem = SessionContentItemHelper.GetPrevSessionContentItem(session_file_id, FragOrder);
                if (prevContentItem != null)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type=\"text/javascript\">");
                    sb.Append("prevAgendaItemIndex=").Append(prevContentItem.AgendaItemID).Append(";");
                    sb.Append("prevSpeakerIndex=").Append(prevContentItem.AttendantID).Append(";");

                    if (prevContentItem.AgendaSubItemID != null)
                        sb.Append("prevAgendaSubItemIndex=").Append(prevContentItem.AgendaSubItemID).Append(";");
                    if (prevContentItem.Attendant.JobTitle != null && !string.IsNullOrEmpty(prevContentItem.Attendant.JobTitle))
                        sb.Append("prevSpeakerTitle=\"").Append(prevContentItem.Attendant.JobTitle).Append("\";");
                    sb.Append("</script>");
                    ClientScript.RegisterClientScriptBlock(typeof(string), "abbas", sb.ToString());

                    //ClientScript.RegisterClientScriptBlock(typeof(string), "abbas", "<script type=\"text/javascript\">prevAgendaItemIndex=" + prevContentItem.AgendaItemID + ";prevAgendaSubItemIndex=" + prevContentItem.AgendaSubItemID == null ? "''" : prevContentItem.AgendaSubItemID
                    //    + ";prevSpeakerIndex=" + prevContentItem.AttendantID + ";prevSpeakerTitle=" + prevContentItem.Attendant.JobTitle == null ? "''" : prevContentItem.Attendant.JobTitle + ";</script>");
                }
            }

            //bind Speakers Drop Down List
            ListItem li;
            //ddlSpeakers.DataSource = current_session.Attendants;
            //ddlSpeakers.DataValueField = "ID"; 
            //ddlSpeakers.DataTextField = "Name";
            //ddlSpeakers.DataBind();
            foreach (Attendant item in current_session.Attendants)
            {
                ListItem liAttendant = new ListItem(BLL.MabatahCreatorFacade.GetAttendantTitle(item, current_session.ID), item.ID.ToString());
                ddlSpeakers.Items.Add(liAttendant);
            }

            li = new ListItem("-------- اختر المتحدث --------", "0");
            ddlSpeakers.Items.Insert(0, li);

            //bind Agenda Items Drop Down List
            //ddlAgendaItems.DataSource = current_session.AgendaItems;
            //ddlAgendaItems.DataValueField = "ID";
            //ddlAgendaItems.DataTextField = "Name";
            //ddlAgendaItems.DataBind();

            foreach (AgendaItem item in current_session.AgendaItems)
            {
                ListItem liAgenda = new ListItem(item.Name, item.ID.ToString());
                liAgenda.Attributes["IsGroupSubAgendaItems"] = item.IsGroupSubAgendaItems.ToString().ToLower();
                ddlAgendaItems.Items.Add(liAgenda);
            }

            li = new ListItem("-------- اختر البند --------", "0");
            ddlAgendaItems.Items.Insert(0, li);

            if (lastContentItem != null)
            {
                ddlAgendaItems.SelectedValue = lastContentItem.AgendaItemID.ToString();
                
                //check for subitems



                ddlSpeakers.SelectedValue = lastContentItem.AttendantID.ToString();
                txtSpeakerJob.Value = lastContentItem.CommentOnAttendant;//usama for job title
                txtComments.InnerText = lastContentItem.CommentOnText;
                txtFooter.InnerText = lastContentItem.PageFooter;

                if (lastContentItem.AgendaItem.IsGroupSubAgendaItems)
                {
                    chkGroupSubAgendaItems.Attributes.Add("checked", "checked");
                    ddlAgendaSubItems.Attributes.Add("disabled", "disabled");
                }
                else
                    chkGroupSubAgendaItems.Attributes.Remove("checked");

                if(lastContentItem.MergedWithPrevious == null || lastContentItem.MergedWithPrevious == false)
                    sameAsPrevSpeaker.Attributes.Remove("checked");
                else
                    sameAsPrevSpeaker.Attributes.Add("checked", "checked");

                if (lastContentItem.Ignored == null && lastContentItem.Ignored == false)
                    chkIgnoredSegment.Attributes.Remove("checked");
                else
                    chkIgnoredSegment.Attributes.Add("checked", "checked");

                if (lastContentItem.AgendaSubItemID != null)
                {
                    // bind agenda sub items DDL
                    List<SessionAgendaItem> agendaSubItems = EditorFacade.GetAgendaSubItemsByAgendaID(lastContentItem.AgendaItemID);
                    ddlAgendaSubItems.DataSource = agendaSubItems;
                    ddlAgendaSubItems.DataValueField = "ID";
                    ddlAgendaSubItems.DataTextField = "Text";
                    ddlAgendaSubItems.DataBind();
                    ddlAgendaSubItems.SelectedValue = lastContentItem.AgendaSubItemID.ToString();
                }
                else
                    ddlAgendaSubItems.Enabled = false;
            }


            // bind labels
            lblSessionName.Text = current_session.Serial.ToString() + "/" + current_session.Stage.ToString() + "/" + current_session.Season.ToString();
            lblSessionDate.Text = current_session.Date.ToShortDateString();
            lblMP3FileName.Text = System.IO.Path.GetFileName(file.Name);

            // save required info into the user session
            current_session_info["SessionID"] = sessionId;
            current_session_info["SessionFileID"] = session_file_id;
            current_session_info["FragOrderInXml"] = FragOrder;
            Session["current_session_info"] = current_session_info;

            currentOrder.Value = FragOrder.ToString();
            int numInserted = SessionContentItemHelper.GetItemsCountByFileID(session_file_id);

            if (currentPageMode == EditorPageMode.Edit)
            {
                if (FragOrder == 0)//FragOrder == 0 || 
                {
                    btnNext.Attributes.Remove("disabled");
                    btnPrev.Attributes.Add("disabled", "disabled");
                    sameAsPrevSpeaker.Attributes.Add("disabled", "disabled");
                }
                else
                    if (FragOrder > 0 && FragOrder <= p.Count - 1 )
                    {
                        btnNext.Attributes.Remove("disabled");
                        btnPrev.Attributes.Remove("disabled");
                    }
                    else
                        if (FragOrder >= p.Count - 1)
                            {
                                btnNext.Attributes.Add("disabled", "disabled");
                                btnPrev.Attributes.Remove("disabled");

                            }


                if (FragOrder >= p.Count - 1)
                {
                    btnFinish.Attributes.Remove("disabled");
                    btnNext.Attributes.Add("disabled", "disabled");
                    btnFinish.Attributes.CssStyle.Add("display", "inline");
                }
                //else
                //    if(FragOrder > 0 )
                //        btnPrev.Attributes.Remove("disabled");
                //    else

                //        if(((Model.SessionFileStatus)file.Status) == Model.SessionFileStatus.Completed)
            }
            else
            {
                btnNext.Attributes.CssStyle.Add("display", "none");
                btnPrev.Attributes.CssStyle.Add("display", "none");
                btnFinish.Attributes.CssStyle.Add("display", "none");
                btnSplit.Attributes.CssStyle.Add("display", "none");
                
            }
            if (current_session != null && (current_session.SessionStatusID == (int)Model.SessionStatus.Approved ||
                current_session.SessionStatusID == (int)Model.SessionStatus.Completed))
            {
                btnFinish.Attributes.Remove("disabled");
                btnFinish.Attributes.CssStyle.Add("display", "inline");

            }
            bool isReEdit=false;
            if (ReEdit != null && bool.TryParse(ReEdit, out isReEdit))
            {
                if (isReEdit )
                {
                    btnFinish.Attributes.Remove("disabled");
                    btnFinish.Attributes.CssStyle.Add("display", "inline");
                }

            }
            if (numInserted >= p.Count - 1)
            {
                btnFinish.Attributes.Remove("disabled");
                btnFinish.Attributes.CssStyle.Add("display", "inline");
            }

            if (currentPageMode != EditorPageMode.Edit)
                btnFinish.Attributes.CssStyle.Add("display", "none");

        }


        public string EditPageMode
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.EDIT_PAGE_MODE, Context);
            }
        }

        public string SessionContentItemID
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.SESSION_CONTENT_ITEM_ID, Context);
            }
        }
        public string ReEdit
        {
            get
            {
                return WebHelper.GetQSValue(Constants.QSKeyNames.REDIT, Context);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }
}
}
