using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TayaIT.Enterprise.EMadbatah.BLL;
using TayaIT.Enterprise.EMadbatah.Model;
using TayaIT.Enterprise.EMadbatah.DAL;
using System.Text;
using TayaIT.Enterprise.EMadbatah.Config;
using System.IO;

namespace TayaIT.Enterprise.EMadbatah.Web
{
    public partial class ReviewNotes : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //if (CurrentUser.Role == Model.UserRole.DataEntry)
            //    Response.Redirect(Constants.PageNames.ERROR_PAGE + "?" + Constants.QSKeyNames.ERROR_TYPE + "=" + (int)ErrorType.Unauthorized);

            if (!Page.IsPostBack)
            {
                long sessionId;
                if (SessionID != null && long.TryParse(SessionID, out sessionId))
                {

                    bool anyNotesExist = false;
                    Model.SessionDetails details = SessionStartFacade.GetSessionDetails(sessionId);
                    SessionFile sessionStart = SessionStartHelper.GetSessionStartBySessionId(sessionId);
                    StringBuilder notes = new StringBuilder(); ;

                    if (sessionStart != null && (Model.SessionFileStatus)sessionStart.Status == Model.SessionFileStatus.SessionStartRejected)
                    {
                        anyNotesExist = true;
                        
                        string fileInfo = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileInfo].ToString();

                        fileInfo = fileInfo.Replace("<%FileName%>", sessionStart.Name);
                        fileInfo = fileInfo.Replace("<%ID%>", sessionStart.ID + "");

                        string ItemInfo = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileNoteItem].ToString();
                        ItemInfo = ItemInfo.Replace("<%AgendaItem%>", "مقدمة الجلسة" + "");
                        
                        
                        ItemInfo = ItemInfo.Replace("<%ReviewerFirstName%>", (sessionStart.Reviewer != null) ? sessionStart.Reviewer.FName : "");
                        ItemInfo = ItemInfo.Replace("<%FileReviewerName%>", (sessionStart.FileReviewer != null) ? sessionStart.FileReviewer.FName : "");


                        ItemInfo = ItemInfo.Replace("<%ReviewerNote%>", sessionStart.SessionStartReviewNote)
                            .Replace("<%SessionContentID%>", "")
                            .Replace("<%SessionFileID%>", sessionStart.ID.ToString())
                            .Replace("<%LINK%>", (sessionStart.UserID == CurrentUser.ID) ? "<a href=\"SessionStart.aspx?sid=" + sessionId + "&editmode=2\" class=\"editerror\">[تصحيح الخطأ]</a>" : "<span class=\"smalltxt\">[" + sessionStart.User.FName + "]</span>");

                        string fileNotes = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileNotes].ToString();
                        fileNotes = fileNotes.Replace("<%FileNotes%>", ItemInfo.ToString());
                        fileNotes = fileNotes.Replace("<%Counter%>",  "1");
                        
                        notes.Append(fileNotes);

                    }

                    if (details == null)
                        return;
                    sessionDate.InnerText = details.Date.ToString();
                    sessionTitle.InnerText = details.Serial + "/" + details.Season + "/" + details.StageType;
                    
                    int currentCounter = 1;




                    foreach (SessionAudioFile file in details.SessionFiles)
                    {
                        List<SessionContentItem> items = null;
                        //if admin
                        if (CurrentUser.Role == UserRole.Admin)
                        {
                            items = ReviewNotesFacade.GetSessionContentItems(file.ID, Model.SessionContentItemStatus.Rejected);                            
                           // ShowWarn(GetLocalizedString("strWrongReviewer"));
                        }
                        else
                            items = ReviewNotesFacade.GetSessionContentItems(file.ID, Model.SessionContentItemStatus.Rejected, CurrentUser.ID);

                        if (items == null || items.Count <= 0)
                        {
                            continue;
                        }
                        else
                            anyNotesExist = true;

                        string fileInfo = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileInfo].ToString();

                        fileInfo = fileInfo.Replace("<%FileName%>", new FileInfo(file.Name).Name + "");
                        fileInfo = fileInfo.Replace("<%ID%>", currentCounter + "");

                        notes.Append(fileInfo);
                        //notes.Append("<div class=\"graybar\"><div class=\"info_cont fr\"> <span class=\"title\">")
                        //    .Append("الملف:</span> <span class=\"Bold_title\">" + file.Name + "</span> </div>")
                        //    .Append("<div class=\"expandicondown fl\"  id=\"1\"></div>")
                        //    .Append("<div class=\"clear\"></div></div>");

                        //notes.Append("<div class=\"notes-items notes-items1\">");

                        StringBuilder sbItems = new StringBuilder();
                        foreach (SessionContentItem item in items)
                        {

                            string ItemInfo = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileNoteItem].ToString();
                            ItemInfo = ItemInfo.Replace("<%AgendaItem%>", item.AgendaItem.Name + "");
                            
                            ItemInfo = ItemInfo.Replace("<%ReviewerFirstName%>", (item.Reviewer != null)? item.Reviewer.FName : "" );
                            ItemInfo = ItemInfo.Replace("<%FileReviewerName%>", (item.FileReviewer != null) ? item.FileReviewer.FName : "");

                            ItemInfo = ItemInfo.Replace("<%ReviewerNote%>", item.ReviewerNote + "")
                                .Replace("<%SessionContentID%>", item.ID.ToString())
                                .Replace("<%SessionFileID%>", file.ID.ToString())
                                .Replace("<%LINK%>", (item.UserID == CurrentUser.ID)? "<a href=\"EditSessionFile.aspx?scid=" + item.ID.ToString() + "&editmode=2&sfid=" + file.ID.ToString() + "&sid=" + SessionID + "\" class=\"editerror\">[تصحيح الخطأ]</a>" : "<span class=\"smalltxt\">["+item.User.FName+"]</span>");
                            
                            sbItems.Append(ItemInfo);

                            //             notes.Append(" <li class="note-itm"><span class="smalltxt">تم رفض النص الموجود فى البند <strong class="darkcolor"> الرسائل الواردة الى المجلس </strong> بواسطة المراجع <strong class="darkcolor">محمد أحمد عبد اللطيف</strong> و كانت ملاحظاته للرفض هى : </span>
                            //<div class="notetext smalltxt rednote">الرجاء التأكد من عدم وجود أخطاء إملائية فى النص</div>
                            //<a href="#" class="editerror">[تصحيح الخطأ]</a> </li>

                            //            notes += "تم رفض النص الموجود في البند " + item.AgendaItem.Name;
                            //            notes += "بواسطة المراجع " + item.Reviewer.FName;
                            //            notes += "وكانت ملاحظاته للرفض كالتالي" + "\r\n";
                            //            notes += item.ReviewerNote;
                        }

                        string fileNotes = Application[Constants.HTMLTemplateFileNames.ReviewNotesFileNotes].ToString();
                        fileNotes = fileNotes.Replace("<%FileNotes%>", sbItems.ToString());
                        fileNotes = fileNotes.Replace("<%Counter%>", currentCounter + "");
                        currentCounter++;

                        notes.Append(fileNotes);
                    }

                    if(!anyNotesExist)
                        ShowInfo("لا يوجد أي ملاحظات بعد");

                    reviewNotes.InnerHtml = notes.ToString();
                }
                else
                {
                    ShowMainError(GetLocalizedString("strNoQueryStr"));
                }
            }
        }
    }
}
