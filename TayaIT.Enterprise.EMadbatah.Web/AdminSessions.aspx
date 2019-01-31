<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminSessions.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AdminSessions"
    MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - ادارة الجلسات" %>

<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Model" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.BLL" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Util" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script src="scripts/SessionScript.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Literal Text="" runat="server" ID="litStartupScript" />
    <!-- 
    NEXT FROM AHMED SABER
     -->
    <script type="text/javascript">
        $(document).ready(function () {
            // on hover over the rows
            $('#maintable1 tr.tbrow').hover(function () {
                $(this).addClass('hover')
            }, function () {
                $(this).removeClass('hover')
            })
            // on click on the arrows
            $('#maintable1 .options .hoverArrow').click(function () {
                var curent = $(this);
                $('#maintable1 .options .hoverArrow').not(curent).removeClass('up').addClass('down');
                $(this).toggleClass('up down')
                var parentRow = $(this).parents('.tbrow');
                // remove opend class from other
                $('#maintable1 tr.tbrow').not(parentRow).removeClass('opend')
                parentRow.toggleClass('opend')
            })
            // for the up and down
            function move(wh, ty) {
                var row = $(wh).parents(ty + ":first");
                // before move current index
                var beforeMoveIndex = row.index()
                var row2;
                if ($(wh).is(".up")) {
                    row2 = row.prev()
                    row.insertBefore(row2);
                } else {
                    row2 = row.next()
                    row.insertAfter(row2);
                }
                // send current index
                var afterMoveIndex = row.index()
                // return
                return { before: beforeMoveIndex, after: afterMoveIndex, tr1: row, tr2: row2 }
            }
            $('#maintable1 .ulist li,#maintable1 .smalltable tr').hover(function () {
                $('.upordown', this).addClass('show')
            }, function () {
                $('.upordown', this).removeClass('show')
            })
            $(".ulist li .upordown .up,.ulist li .upordown .down").click(function () {
                var pos = move(this, 'li')
                reorder($(pos.tr1).attr('data-id'), pos.after, $(pos.tr2).attr('data-id'), pos.before); //attachement
            });
            $(".smalltable .upordown .up,.smalltable .upordown .down").click(function () {
                var pos = move(this, 'tr')
                reorder($(pos.tr1).attr('data-id'), pos.after, $(pos.tr2).attr('data-id'), pos.before);
            });
            function reorder(id1, neworder1, id2, neworder2) {
                $.ajax({
                    cache: false,
                    type: 'post',
                    url: 'ReorderHandler.ashx',
                    data: {
                        funcname: 'ReorderSessionFiles',
                        rid1: id1,
                        order1: neworder1,
                        rid2: id2,
                        order2: neworder2
                    },
                    success: function (response) {
                        if (response != '') {

                        }
                    },
                    error: function () {

                    }
                });
            }
        })
    </script>
      <div id="mainContent" runat="server">
    <div id="maintable1">
        <table class="table mytable" border="0" cellspacing="0" cellpadding="0">
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
                        المراجع
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

                    foreach (SessionDetails session in sessionsDetails)
                    {
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
                <tr class="tbrow tbrow<%=session.Status.ToString()%>">
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
                                    <span class="status"><span class="icon"></span>
                                        <%=sessionStatus%></span>
                                </td>
                                <% switch (session.Status)
                                   {
                                       case SessionStatus.New:
                                %>
                                <td class="column column4">
                                    <span class="grayed">لم يتم البدء بعد</span>
                                    <select id="selectVecsysFolders" runat="server" visible="false" class="selectVecsysFolders">
                                    </select>
                                    <input type="button" id="btnConfirm" runat="server" visible="false" value="confirm" />
                                    <span id="spnWarn" name="spnWarn" runat="server"></span>
                                </td>
                                <!--td class="column column5">
                                    <span class="grayed">- - - - -</span>
                                </td>
                                <td class="column column6">
                                    <span class="grayed">- - - - -</span>
                                </td-->
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
                                                    المصحح
                                                </th>
                                                <th>
                                                    مراجع الملف
                                                </th>
                                                <th>
                                                    الحالة
                                                </th>
                                                <th>
                                                    آخر تعديل
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody class="displayOnOpen">
                                            <%
                                                //session files
                                                foreach (SessionAudioFile saf in session.SessionFiles)
                                                {
                                                    string sessionFileCompleteName = saf.Name;
                                                    string sessionFileName = System.IO.Path.GetFileName(saf.Name);
                                                    string sessionFileStatus = GetLocalizedString("strSessionFileStatus" + saf.Status.ToString());
                                                    string sessionFileOrderStr = saf.Order.ToString();
                                                    string sessionFileOwnerName = saf.OwnerUserName;
                                                    string lastModefied = saf.LastModefied.ToString();
                                                    int sessionFileOrder = saf.Order;

                                                    int currentUserID = saf.UserID == null ? -1 : (int)saf.UserID;
                                                                    
                                            %>
                                            <tr data-order="<%=sessionFileOrder%>" data-id="<%=saf.ID%>">
                                                <td title="<%=sessionFileName%>">
                                                    <span>
                                                        <%= TextHelper.Truncate(sessionFileName, 18, "...") %></span>
                                                </td>
                                                <td data-currentuserid="<%=currentUserID %>" data-sessionfileid="<%=saf.ID%>">
                                                    <select id="selectUsers" class="selectUsers" data-currentuserid="<%=currentUserID %>">
                                                        <% 
                                                    
                                                    List<EMadbatahUser> peopleWithDEPower = (from user in usersdb/*revusersdb*/
                                                                                            where user.Role != UserRole.Reviewer
                                                                                            select user).ToList<EMadbatahUser>();
                                                            int counter = 0;
                                                            foreach (EMadbatahUser user in peopleWithDEPower)
                                                            {
                                                                if (counter == 0 && saf.UserID == null)
                                                        %>
                                                        <option value="-1" selected="selected">-- إسناد إلى مصحح -- </option>
                                                        <%
                                                        %><option value="<%=user.ID%>" <%=saf.UserID == user.ID ? "selected=\"selected\"" : ""%>>
                                                            <%=user.Name%></option>
                                                        <%   
counter++;
                                                                               } 
                                                        %>
                                                    </select>
                                                    <span class="lock" style="<%=(saf.UserID == null)?"display:none": "display:inline"%>;
                                                        cursor: pointer;" title="إزالة المصحح من على الملف"></span>
                                                </td>
                                                <td data-currentuserid="<%=currentUserID %>" data-sessionfileid="<%=saf.ID%>">
                                                
                                                <%--FILEREVIEWER--%>
                                                <select id="selectFileReviewer" class="selectFileReviewer" data-currentuserid="<%=currentUserID %>">
                                                        <% 
                                                    
                                                    List<EMadbatahUser> peopleWithFRPower = (from user in usersdb/*revusersdb*/
                                                                                             //now the dataentry-reviewr role is using filereviewer privilages instead od session reviewer
                                                                                             where user.Role == UserRole.FileReviewer || user.Role == UserRole.ReviewrDataEntry
                                                                                            select user).ToList<EMadbatahUser>();
                                                            int counterrfv = 0;
                                                            foreach (EMadbatahUser user in peopleWithFRPower)
                                                            {
                                                                if (counterrfv == 0 && saf.FileReviewrID == null)
                                                                {
                                                        %>
                                                        <option value="-1" selected="selected">-- إسناد إلى مراجع ملف -- </option>
                                                        <%}
                                                                
                                                        %><option value="<%=user.ID%>" <%=saf.FileReviewrID == user.ID ? "selected=\"selected\"" : ""%>>
                                                            <%=user.Name%></option>
                                                        <%   
                                                            counterrfv++;    
                                                              } 
                                                        %>
                                                    </select>
                                                    <span class="revFilelock" style="<%=(saf.FileReviewrID == null)?"display:none": "display:inline"%>; cursor: pointer;" title="إزالة مراجع الملف من على الملف"></span>



























                                                </td>
                                                <td>
                                                    <span>
                                                        <%=sessionFileStatus%></span>
                                                </td>
                                                <td>
                                                    <!-- usama here add reorder js ajax calls -->
                                                    <%=lastModefied%>
                                                </td>
                                            </tr>
                                            <%
                                                                }//end foreach
                                            %>
                                        </tbody>
                                    </table>
                                </td>
                                <%
                                    break;

                                                       default:
                                    break;
                                                   } %>
                                <%
string currentReviewerID = session.ReviewerID.ToString();
List<EMadbatahUser> reviewrs = (from user in usersdb/*revusersdb*/
                                //now the dataentry-reviewr role is using filereviewer privilages instead od session reviewer
                                where user.Role != UserRole.DataEntry && user.Role != UserRole.FileReviewer
                                
                                select user).ToList<EMadbatahUser>();
                                        
                       
                        
                                %>
                                <td class="column column5" data-currentrevid="<%=currentReviewerID %>" data-sessionid="<%=session.SessionID %>">
                                 <div class="padd">
                                     <select id="selectReviewers" class="selectReviewers" data-currentrevid="<%=currentReviewerID %>"
                        data-sessionid="<%=session.SessionID %>">
                        <% 
int counterRev = 0;
foreach (EMadbatahUser user in reviewrs)
{
    if (counterRev == 0 && session.ReviewerID == null)
                        %>
                        <option value="-1" selected="selected">-- إسناد إلى مراجع -- </option>
                        <%
                        %><option value="<%=user.ID%>" <%=session.ReviewerID == user.ID ? "selected=\"selected\"" : ""%>>
                            <%=user.Name%></option>
                        <%   
counterRev++;
                                                                               } 
                        %>
                    </select>
                    <span class="revlock" style="<%=(session.ReviewerID == null)?"display:none": "display:inline"%>;
                        cursor: pointer;" title="إزالة المراجع من على الجلسة"></span>
                       </div>
                        </td>
                            </tr>
                        </table>
                    </td>
               
                    <td class="options lftoptns">
                    </td>
                </tr>
                <!-- new row end -->
                <%
                        
                    }//end of foreach
                
                %>
            </tbody>
        </table>
        <div class="prefix_21">
            <!--input type="button" value="حفظ" class="btn" name="" /-->
            <span id="spnMsg"></span>
        </div>
    </div>
    </div>
    <script type="text/javascript">
        $('.selectUsers').change(function (e) {

            var selectUsers = $(this);
            var oldUserID = $(this).parent('td').attr('data-currentUserID');
            var span = $(this).parent('td').children('span');
            var parentTD = $(this).parent('td');
            var str = "";
            var newUserIDval = "";
            $(this).children("option:selected").each(function () {
                str = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد إعادة تخصيص الملف إلى " + str + " ؟")

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'AssignSessionFile',
                        sfid: $(this).parents('td').attr("data-sessionFileID"),
                        uid: newUserIDval
                    },
                    success: function (response) 
                    {

                        if (response != 'true') {//| response != 'true')
                            alert('لقد حدث خطأ');
                            $(this).val(oldUserID);
                        }
                        else {
                            span.css("display", "inline");
                            //$(this).parent.children('span').addClass('lock');
                            parentTD.attr('data-currentUserID', newUserIDval);
                            selectUsers.children("option[value='-1']").each(function () {
                                $(this).remove();

                            });
                            // $("#selectBox option[value='option1']").remove();

                        }

                        $('.absLoad.loading').hide();
                    }
                });


            }
            else {
                $(this).val(oldUserID);
            }


        })



        $('.selectReviewers').change(function (e) {

            var selectUsers = $(this);
            var oldUserID = $(this).parent('div').parent('td').attr('data-currentRevID');
            var span = $(this).parent('div').children('span');
            var parentTD = $(this).parent('div').parent('td');
            var str = "";
            var newUserIDval = "";
            $(this).children("option:selected").each(function () {
                str = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد إعادة تخصيص مراجعة الجلسة إلى " + str + " ؟")

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'ChangeSessionReviewer',
                        sid: $(this).attr("data-sessionID"),
                        srid: newUserIDval
                    },
                    success: function (response) {

                        if (response != 'true') 
                        {//| response != 'true')
                            alert('لقد حدث خطأ');
                            $(this).val(oldUserID);
                        }
                        else 
                        {
                            span.css("display", "inline");
                            //$(this).parent.children('span').addClass('lock');
                            parentTD.attr('data-currentRevID', newUserIDval);
                            selectUsers.children("option[value='-1']").each(function () {
                                $(this).remove();

                            });
                            // $("#selectBox option[value='option1']").remove();

                        }
                        $('.absLoad.loading').hide();
                    }
                });


            }
            else {
                $(this).val(oldUserID);
            }


        })


        $('.lock').click(function (e) {
            var lockSpan = $(this);
            var usersSelect = $(this).parent('td').children('select');
            // var userIDtoUnlock = usersSelect.attr('data-currentUserID'); //$(this).attr("data-currentUserID")
            var userNametoUnlock;

            usersSelect.children("option:selected").each(function () {
                userNametoUnlock = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد تحرير الملف من المستخدم الحالي " + userNametoUnlock + " ؟");

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'UnlockSessionFile',
                        sfid: $(this).parents('td').attr("data-sessionFileID")
                    },
                    success: function (response) {

                        if (response != 'true') 
                        {//| response != 'true')
                            alert('لقد حدث خطأ');

                        }
                        else 
                        {
                            //$(this).removeClass('lock');
                            lockSpan.css("display", "none");
                            usersSelect.prepend($('<option></option>').val('-1').html(" -- إسناد إلى مصحح -- "));
                            usersSelect.val('-1');

                        }
                        $('.absLoad.loading').hide();
                    }
                });
            }
        })


        $('.revlock').click(function (e) {
            var lockSpan = $(this);
            var usersSelect = $(this).parent('div').children('select'); //$(this).parent('td').children('select');
            var sessionID = usersSelect.attr("data-sessionID");
            // var userIDtoUnlock = usersSelect.attr('data-currentUserID'); //$(this).attr("data-currentUserID")
            var userNametoUnlock;

            usersSelect.children("option:selected").each(function () {
                userNametoUnlock = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد تحرير الجلسة من المراجع الحالي " + userNametoUnlock + " ؟");

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'UnlockSessionReviewer',
                        sid: sessionID
                    },
                    success: function (response) 
                    {

                        if (response != 'true') {//| response != 'true')
                            alert('لقد حدث خطأ');

                        }
                        else {
                            //$(this).removeClass('lock');
                            lockSpan.css("display", "none");
                            usersSelect.prepend($('<option></option>').val('-1').html(" -- إسناد إلى مراجع -- "));
                            usersSelect.val('-1');

                        }
                        $('.absLoad.loading').hide();
                    }

                });

            }


        })



        /*
        //usama new for the new role sessionfile_reviewer
        UnlockSessionFileReviewer
AssignSessionFileReviewer
AssignSessionFileReviewer
مراجع ملف

//<NewRoleName>

revFilelock*/
       $('.revFilelock').click(function (e) {
            var lockSpan = $(this);
            var usersSelect = $(this).parent('td').children('select');
            // var userIDtoUnlock = usersSelect.attr('data-currentUserID'); //$(this).attr("data-currentUserID")
            var userNametoUnlock;

            usersSelect.children("option:selected").each(function () {
                userNametoUnlock = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد تحرير الملف من مراجع الملف الحالي " + userNametoUnlock + " ؟");

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'UnlockSessionFileReviewer',
                        sfid: $(this).parents('td').attr("data-sessionFileID")
                    },
                    success: function (response) {

                        if (response != 'true') 
                        {//| response != 'true')
                            alert('لقد حدث خطأ');

                        }
                        else 
                        {
                            //$(this).removeClass('lock');
                            lockSpan.css("display", "none");
                            usersSelect.prepend($('<option></option>').val('-1').html(" -- إسناد إلى مراجع ملف -- "));
                            usersSelect.val('-1');

                        }
                        $('.absLoad.loading').hide();
                    }
                });
            }
        })



//selectFileReviewer
$('.selectFileReviewer').change(function (e) {

            var selectFileReviewer = $(this);
            var oldUserID = $(this).parent('td').attr('data-currentUserID');
            var span = $(this).parent('td').children('span');
            var parentTD = $(this).parent('td');
            var str = "";
            var newUserIDval = "";
            $(this).children("option:selected").each(function () {
                str = $(this).text();
                newUserIDval = $(this).val();
            });


            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد إعادة تخصيص الملف إلى مراجع ملف " + str + " ؟")

            if (answer) {
                //create ajax admin handler
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'AdminHandler.ashx',
                    data: {
                        funcname: 'AssignSessionFileReviewer',
                        sfid: $(this).parents('td').attr("data-sessionFileID"),
                        uid: newUserIDval
                    },
                    success: function (response) 
                    {

                        if (response != 'true') {//| response != 'true')
                            alert('لقد حدث خطأ');
                            $(this).val(oldUserID);
                        }
                        else {
                            span.css("display", "inline");
                            //$(this).parent.children('span').addClass('lock');
                            parentTD.attr('data-currentUserID', newUserIDval);
                            selectFileReviewer.children("option[value='-1']").each(function () {
                                $(this).remove();

                            });
                            // $("#selectBox option[value='option1']").remove();

                        }

                        $('.absLoad.loading').hide();
                    }
                });


            }
            else {
                $(this).val(oldUserID);
            }


        })

        
        


    
    </script>
</asp:Content>
