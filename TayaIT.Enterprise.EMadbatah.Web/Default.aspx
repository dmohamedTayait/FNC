<%@ Page Title="المضبطة الإلكترونية - الصفحه الرئيسيه" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web._Default" %>

<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Model" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.BLL" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Util" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Web" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="scripts/SessionScript.js" type="text/javascript"></script>
    <script src="scripts/fileuploader.js" type="text/javascript"></script>
    <script src="scripts/DefaultScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Literal Text="" runat="server" ID="litStartupScript" />
    <link href="styles/uniform.aristo.css" rel="stylesheet" type="text/css" />
    <link href="styles/fileuploader.css" rel="stylesheet" type="text/css" />
    <input type="hidden" runat="server" id="UserRuleHidden" />
     <input type="hidden" runat="server" id="test" />
    <div id="mainContent" runat="server">
        <div id="maintable1"  class="MainhomeWrapper">
            <table class="table deftable" border="0" cellspacing="0" cellpadding="0">
                <thead>
                    <tr>
                        <th class="options">
                        </th>
                        <th class="column column1">
                            رقم الجلسة
                        </th>
                        <th class="column column2">
                            التاريخ
                        </th>
                        <th class="column column3">
                            الحالة
                        </th>
                        <th class="column column4">
                            الملفات
                        </th>
                        <th class="column column5">
                            المرفقات
                        </th>
                        <th class="column column6">
                            حالة المراجعة
                        </th>
                        <th class="options">
                        </th>
                    </tr>
                </thead>
                <tbody>
                    <%
                        //for paging
                        int sessionsCount = EMadbatahFacade.GetSessionsCount();
                        int currentPageNo = 1;
                        int itemsPerPage = 10;

                        if (CurrentPageNo != null)
                            int.TryParse(CurrentPageNo, out currentPageNo);

                        if (ItemsPerPage != null)
                            int.TryParse(ItemsPerPage, out itemsPerPage);

                        List<SessionDetails> sessionsDetails = EMadbatahFacade.GetSessions(currentPageNo, itemsPerPage);
                        //build html of sessions
                        int currentSessionNum = 0;
                        foreach (SessionDetails session in sessionsDetails)
                        {
                            currentSessionNum++;
                            string sessionDate = session.Date.Date.ToShortDateString();
                            string sessionDateHijri = session.DateHijri.ToString();
                            string sessionName = EMadbatahFacade.GetSessionName(session.Season, session.Stage, session.Serial);//"[ د /" + session.Stage + "ف /" + session.Season + " " + session.Serial + " ]";
                            string sessionStatus = GetLocalizedString("strSessionStatus" + session.Status.ToString());

                            int nRejected = 0;
                            int nModefiedAfterApprove = 0;
                            int nFixed = 0;

                            Hashtable tblStats = EMadbatahFacade.GetSessionStatistics(CurrentUser, session.SessionID);
                            if (tblStats != null)
                            {
                                nRejected = int.Parse(tblStats[(int)SessionContentItemStatus.Rejected].ToString());
                                nModefiedAfterApprove = int.Parse(tblStats[(int)SessionContentItemStatus.ModefiedAfterApprove].ToString());
                                nFixed = int.Parse(tblStats[(int)SessionContentItemStatus.Fixed].ToString());
                            }
                        
                    %>
                    <!-- new row -->
                    <tr class="tbrow tbrow<%=session.Status.ToString()%> tbrowID<%=currentSessionNum%>">
                        <td class="options">
                            <% if (session.Status != SessionStatus.New)
                               {%>
                            <div class="hoverArrow down">
                            </div>
                            <%} %>
                        </td>
                        <td colspan="6">
                            <table class="columns" width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr class="color1">
                                    <td class="column column1">
                                        <strong>
                                            <%=sessionName%></strong>
                                    </td>
                                    <td class="column column2">
                                        <%=sessionDate%>
                                    </td>
                                    <td class="column column3">
                                        <span class="status"><span class="icon"></span> <%=sessionStatus%></span>
                                    </td>
                                    <% switch (session.Status)
                                       {
                                           case SessionStatus.New:                                                           
                                    %>
                                    <td class="column column4">
                                        <span class="grayed">لم يتم البدء بعد</span>
                                        <%InitializeVecsysNewSession(session); %>
                                        <%if (CurrentUser.Role != UserRole.Reviewer)
                                          {%>
                                        <select id="selectVecsysFolders" runat="server" visible="false" class="selectVecsysFolders">
                                        </select>
                                        <input type="button" id="btnConfirm" runat="server" visible="false" value="تأكيد"
                                            class="btnConfirm" />
                                        <%} %>
                                        <span id="spnWarn" name="spnWarn" runat="server"></span>
                                    </td>
                                    <td class="column column5">
                                        <span class="grayed">- - - - -</span>
                                    </td>
                                    <td class="column column6">
                                        <span class="grayed">- - - - -</span>
                                    </td>
                                    <%
                                        break;
                                               case SessionStatus.InProgress:
                                               case SessionStatus.Completed:
                                               case SessionStatus.Approved:
                                               case SessionStatus.FinalApproved:
                                    %>
                                    <td class="column column4">
                                        <table class="smalltable" width="100%" border="0" cellspacing="0" cellpadding="0">
                                            <thead>
                                                <tr>
                                                    <th>
                                                        الملفات
                                                    </th>
                                                    <th>
                                                        الحالة
                                                    </th>
                                                    <th>
                                                        المصحح
                                                    </th>

                                                    <th>
                                                        المراجع
                                                    </th>


                                                    <th>
                                                        آخر تعديل
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody class="displayOnOpen">
                                            <tr><td>
                                            <%if (session.Status != SessionStatus.Approved 
                                                  && session.Status != SessionStatus.FinalApproved 
                                                  && CurrentUser.Role != UserRole.Reviewer 
                                                  && CurrentUser.Role != UserRole.FileReviewer)
                                              {%>
                                            <input type="button"  value="تحميل الملفات" class="btnReloadSessionFiles" data-sid="<%=session.SessionID%>" />
                                            <%} %>
                                            </td> 

                                            <td></td><td></td><td></td> <td></td> 
                                            </tr>

                                            <%if (session.Status == SessionStatus.Completed)
                                              { %>
                                            <tr><td><a href="EditIndexItems.aspx?sid=<%=session.SessionID%>">تعديل فهرس المضبطة</a> </td> <td></td><td></td><td></td><td></td>  </tr>
                                            <%} %>
                                            
                                                <%
                                                    //session files
                                                    foreach (SessionAudioFile saf in session.SessionFiles)
                                                    {
                                                        string sessionFileCompleteName = saf.Name;
                                                        string sessionFileName = System.IO.Path.GetFileName(saf.Name);
                                                        string sessionFileStatus = GetLocalizedString("strSessionFileStatus" + saf.Status.ToString());
                                                        string sessionFileOrderStr = saf.Order.ToString();
                                                        string sessionFileOwnerName = saf.OwnerUserName;
                                                        string sessionFileRevName = saf.FileReviewerUserName;
                                                        string lastModefied = saf.LastModefied.ToString();
                                                        int sessionFileOrder = saf.Order;
                                                                    
                                                                    
                                                %>
                                                <tr data-order="<%=sessionFileOrder%>" data-id="<%=saf.ID%>" data-sessionid="<%=session.SessionID%>">
                                                    <td title="<%=sessionFileName%>">
                                                        <span>
                                                            <%=sessionFileName%></span> <%--//TextHelper.Truncate(sessionFileName, 18, "...")--%>
                                                    </td>
                                                    <td class="<%=((saf.UserID != CurrentUser.ID) && (saf.UserID != null))?"lock":""%>">
                                                        <%=sessionFileStatus%>
                                                        <%
                                                            if (CurrentUser.Role != UserRole.Reviewer && CurrentUser.Role != UserRole.FileReviewer && !(session.Status == SessionStatus.FinalApproved))
                                                            {
                                                                switch (saf.Status)
                                                                {
                                                                    case SessionFileStatus.New:
                                                                        if ((saf.UserID == CurrentUser.ID || saf.UserID == null))
                                                                        {
                                                                            if (!saf.IsSessionStart)
                                                                            {%><br /><a href="EditSessionFile.aspx?sfid=<%=saf.ID%>&sid=<%=saf.SessionID%>">[تعديل]</a><%}
                                                                            else
                                                                            {

                                                                                var notCompletedSessionFiles = from sf in session.SessionFiles
                                                                                                               where (sf.Status != SessionFileStatus.Completed && !sf.IsSessionStart)
                                                                                                               select sf;

                                                                                if (notCompletedSessionFiles.ToList<SessionAudioFile>().Count == 0)
                                                                                {
                                                                                        
                                                                                %><br /><a href="SessionStart.aspx?sid=<%=saf.SessionID%>">[تعديل]</a><%}
                                                                            }
                                                                            }
                                                                            break;

                                                                        case SessionFileStatus.InProgress:
                                                                            if (saf.UserID == CurrentUser.ID)
                                                                            {
                                                                                if (!saf.IsSessionStart)
                                                                                {%><br /><a href="EditSessionFile.aspx?sfid=<%=saf.ID%>&sid=<%=saf.SessionID%>">[استكمال التعديل]</a><%}
                                                                                else
                                                                                {%><br /><a href="SessionStart.aspx?sid=<%=saf.SessionID%>">[استكمال التعديل]</a><%}
                                                                            }
                                                                            break;
                                                                        case SessionFileStatus.Completed:
                                                                        case SessionFileStatus.SessionStartApproved:
                                                                        case SessionFileStatus.SessionStartFixed:
                                                                        case SessionFileStatus.SessionStartRejected:
                                                                        case SessionFileStatus.SessionStartModifiedAfterApprove:
                                                                            if (saf.UserID == CurrentUser.ID)
                                                                            {
                                                                                if (!saf.IsSessionStart)
                                                                                {%><br /><a href="EditSessionFile.aspx?sfid=<%=saf.ID%>&reedit=true&sid=<%=saf.SessionID%>">[اعادة التعديل]</a><%}
                                                                                                    else
                                                                                                    {%><br /><a href="SessionStart.aspx?sid=<%=saf.SessionID%>">[اعادة التعديل]</a><%}
                                                                                                }
                                                                                                break;
                                                                                            default:
                                                                                                break;
                                                                    }
                                                                }
                                                                                                  %>
                                                    </td>
                                                    <td title="<%=sessionFileOwnerName%>">
                                                        <span>
                                                            <%= TextHelper.Truncate(sessionFileOwnerName, 15, "...")%></span>
                                                    </td>


                                                    <td title="<%=sessionFileRevName%>">
                                                        <span>
                                                            <%= TextHelper.Truncate(sessionFileRevName, 15, "...")%></span>
                                                    </td>

                                                    <td>
                                                        <!-- usama here add reorder js ajax calls -->
                                                        <%=lastModefied%>
                                                        <% if (CurrentUser.Role != UserRole.Reviewer && !(session.Status == SessionStatus.FinalApproved))
                                                           { %>
                                                        <div class="upordown">
                                                            <div class="up">
                                                            </div>
                                                            <div class="down">
                                                            </div>
                                                        </div>
                                                        <%} %>
                                                    </td>
                                                </tr>
                                                
                                                <%
                                                                    }//end foreach
                                                %>
                                                
                                            </tbody>
                                        </table>
                                    </td>
                                    <td class="column column5">
                                        <div class="margR5">
                                            <div class="row">
                                                <div class="grayed fr">
                                                    ملفات مرفقة</div>
                                                <div class="number fl">
                                                    (<span><%=session.Attachments.Count%></span>)</div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div class="row tex_align_ar displayOnOpen">
                                                <ul class="ulist circle" id="separate-list<%=currentSessionNum%>">
                                                    <%
                                
                                                        //load attachments
                                                        foreach (SessionAttachment attachment in session.Attachments)
                                                        {
                                                            string attachmentFileName = TextHelper.Truncate(attachment.Name, 17, "...");
                                                            string attachmentFileOrderStr = attachment.Order.ToString();
                                                            int attachmentFileOrder = attachment.Order;
                                                    %>
                                                    <li class="word_wrap flnone" title="<%=attachment.Name%>" data-id="<%=attachment.ID.ToString()%>"
                                                        data-order="<%=attachmentFileOrderStr%>" data-sessionid="<%=session.SessionID%>">
                                                        <%=attachmentFileName%>
                                                        <% if (CurrentUser.Role != UserRole.Reviewer && !(session.Status == SessionStatus.FinalApproved))
                                                           { %>
                                                        <a href="#" class="Dellink">حذف</a>
                                                        <div class="upordown flnone">
                                                            <div class="up">
                                                            </div>
                                                            <div class="down">
                                                            </div>
                                                        </div>
                                                        <%} %>
                                                    </li>
                                                    <%
                                                                    
                                                        }
                                                    %>
                                                </ul>
                                                <div class="clear">
                                                </div>
                                                <% if(! (session.Status == SessionStatus.FinalApproved)                         
                                                  && CurrentUser.Role != UserRole.Reviewer 
                                                  && CurrentUser.Role != UserRole.FileReviewer)
                                                   {%>
                                                <div id="file-uploader<%=currentSessionNum%>" data-sid="<%=session.SessionID%>">
                                                </div>
                                                <script language="javascript" type="text/javascript">
                                                                            fileUploader(<%=currentSessionNum%>)
                                                </script>
                                                <div class="clear">
                                                </div>
                                                <% }%>
                                            </div>
                                        </div>
                                    </td>
                                    <td class="column column6">
                                        <div class="margR5">
                                            <div>
                                                <div class="grayed fr">
                                                    مرفوض</div>
                                                <div class="number fl">
                                                    (<%=nRejected%>)</div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div>
                                                <div class="grayed fr">
                                                    يحتاج مراجعه</div>
                                                <div class="number fl">
                                                    (<%=nFixed%>)</div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div>
                                                <div class="grayed fr">
                                                    تعديل بعد الموافقة</div>
                                                <div class="number fl">
                                                    (<%=nModefiedAfterApprove%>)</div>
                                                <div class="clear">
                                                </div>
                                            </div>
                                            <div class="tex_align_ar">
                                                <%
                                                    if (session.ReviewerID != null)
                                                    {
                                                %>
                                                <div>
                                                    المراجع:
                                                    <%=session.ReviewerName %></div>
                                                <%  
                                                                        }
                                                                        if (CurrentUser.Role != UserRole.DataEntry &&
                                                                            (CurrentUser.Role == UserRole.Admin || session.ReviewerID == CurrentUser.ID || session.ReviewerID == null || CurrentUser.Role == UserRole.FileReviewer || CurrentUser.Role == UserRole.ReviewrDataEntry)
                                                                            && !(session.Status == SessionStatus.FinalApproved))
                                                                        {
                                                %>
                                                                <div><a href="Review.aspx?sid=<%=session.SessionID%>">[صفحة المراجعة]</a></div>
                                                <%   
                                                                        }
                                                                        if (nRejected > 0)
                                                                        {
                                                                       if(CurrentUser.Role == UserRole.DataEntry ||
                                                                           //now the dataentry-reviewr role is using filereviewer privilages instead od session reviewer
                                                                          // CurrentUser.Role == UserRole.ReviewrDataEntry ||                                                                          
                                                                           CurrentUser.Role == UserRole.Admin)
                                                                       {
                                                %>
                                                                <div> <a href="ReviewNotes.aspx?sid=<%=session.SessionID%>">[جميع الملاحظات]</a></div>
                                                <%} %>
                                                <%
                                                    }

                                                                        if (CurrentUser.Role != UserRole.DataEntry)
                                                                        {
                                                                            %> 
                                                                            <div>
                                                                            <a href="statistics.aspx?sid=<%=session.SessionID%>">[إحصائيات]</a></div>
                                                                            <%
                                                                        } 
                                                %>
                                                
                                            </div>
                                        </div>
                                    </td>
                                    <%
                                        break;

                                               default:
                                        break;
                                           } %>
                                </tr>
                            </table>
                        </td>
                        <td class="options">
                            <% 
                        
TayaIT.Enterprise.EMadbatah.DAL.SessionFile start = SessionStartFacade.GetSessionStartBySessionID(session.SessionID);
if ((session.Status == SessionStatus.Approved || session.Status == SessionStatus.FinalApproved) && start != null)
{
    string fileVersion = (session.Status == SessionStatus.FinalApproved) ? "final" : "draft";


    //string hrefpdf = hrefWord.Replace("filetype=doc","filetype=pdf");

    //bool isFilesCreated = 
    //TayaIT.Enterprise.EMadbatah.DAL.SessionHelper.IsSessionWordFileAndPDFCreated(session.SessionID)

    //here if session approved word wasnt created we put enforcer creation link
                                                
                            %>
                            <div class="filesIcons displayOnHover">
                                <div class="word marginBS">
                                    <%-- here if session approved word wasnt created we put enforcer creation link--%>
                                    <a href="javascript:downloadFile('<%=session.SessionID.ToString()%>','doc','<%=fileVersion%>');"
                                        class="fitAll"></a>
                                </div>
                                <div class="pdf">
                                    <a href="javascript:downloadFile('<%=session.SessionID.ToString()%>','pdf','<%=fileVersion%>');"
                                        class="fitAll"></a>
                                </div>
                            </div>
                            <%} 
                            %>
                        </td>
                    </tr>
                    <!-- new row end -->
                    <%
                        
                        }//end of foreach
                
                    %>
                </tbody>
            </table>
            <div id="divPaging">
                <%
                    if (sessionsCount > itemsPerPage)
                    {
                        IEnumerable<PagingItem> pagingItemLinks = PagingProvider.CreatePages(itemsPerPage, sessionsCount, currentPageNo);
                        foreach (PagingItem item in pagingItemLinks)
                        {
                            //    item.

                            if (!item.CurrentPage)
                            {
                %><span><a href="<%=item.URL%>">
                    <%=item.Title%></a></span><%
}
                                else
                                {
                    %><span><%=item.Title%></span><%
                                                      }
                            }
                        } 
                    %>
            </div>
        </div>
    </div>
</asp:Content>
