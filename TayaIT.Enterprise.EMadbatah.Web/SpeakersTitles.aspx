<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpeakersTitles.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.SpeakersTitles"
    MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - ادارة المستخدمين" %>

<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Model" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.BLL" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Util" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <link rel="stylesheet" type="text/css" href="styles/jquery.fancybox-1.3.4.css" />
    <script type="text/javascript" src="scripts/jquery.fancybox-1.3.4.pack.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <asp:Literal Text="" runat="server" ID="litStartupScript" />
    <script type="text/javascript">
    function strStartsWith(str, prefix) {
    return str.indexOf(prefix) === 0;
}

        $(document).ready(function () {
            // on hover over the rows
            $('#maintable1 tr.tbrow').hover(function () {
                $(this).addClass('hover');
            }, function () {
                $(this).removeClass('hover');
            })

            $('.smalltable tr').hover(function () {
            var att_name= $(this).find('.att_name').text();
            var sessionStatus =  $.trim($(this).closest("table.columns").find('.sessionStatus').text());
             console.log(sessionStatus);
             if (strStartsWith($.trim(att_name),"سعادة") || strStartsWith($.trim(att_name),"سعاده") ||
                 strStartsWith($.trim(att_name),"معالي")|| strStartsWith($.trim(att_name),"معالى"))
              {
              }
             else
              {
                  if(sessionStatus == "New" || sessionStatus == "InProgress"){
                    console.log(att_name);
                    $(this).find('.editbuttons').css("visibility", "visible");
                   }
              }
            }, function () {
                $(this).find('.editbuttons').css("visibility", "hidden");
            });

            $('.smalltable tr .editbuttons a').click(function () {
                $(".editingTools").show().css({
                    "top": $(this).offset().top - 10,
                    "left": $(this).offset().left + $(this).width() + 42,
                }).data({
                    "valueField": $(this).closest("tr").find(".type_text"),
                    "attID": $(this).closest("tr").find(".attID").text(),
                    "sessionID": $(this).closest("table.columns").find(".sessionID").text(),
                });

                 $('.editingTools select[name="title"]').val($.trim($(this).closest("tr").find(".type_text").text()));

                 return false;
            });
            $('.editingTools .cancel').click(function () {
                $(this).parents(".editingTools").hide();
                 return false;
            });

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
                            المتحدثين
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
                            string sessionID = session.SessionID.ToString();
                            string sessionDate = session.Date.Date.ToShortDateString();
                            string sessionDateHijri = session.DateHijri.ToString();
                            string sessionName = EMadbatahFacade.GetSessionName(session.Season, session.Stage, session.Serial);//"[ د /" + session.Stage + "ف /" + session.Season + " " + session.Serial + " ]";
                            string sessionStatus = GetLocalizedString("strSessionStatus" + session.Status.ToString());
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
                                            <%=sessionName%></strong> <span class="sessionID" style="display: none">
                                                <%=sessionID%></span> <span class="sessionStatus" style="display: none">
                                                    <%=session.Status.ToString()%></span>
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
                                                        المتحدث
                                                    </th>
                                                    <th style="width: 130px;">
                                                        اللقب
                                                    </th>
                                                    <th>
                                                    </th>
                                                </tr>
                                            </thead>
                                            <tbody class="displayOnOpen">
                                                <%
                                                    List<TayaIT.Enterprise.EMadbatah.DAL.Attendant> attendants = TayaIT.Enterprise.EMadbatah.DAL.AttendantHelper.GetAttendantsBySessionId(session.SessionID);
                                                    foreach (TayaIT.Enterprise.EMadbatah.DAL.Attendant att in attendants)
                                                    {
                                                        string name = att.Name;
                                                        int attendant_title_id = EMadbatahFacade.get_session_attendant_title_id(Convert.ToInt64(session.SessionID), Convert.ToInt64(att.ID));
                                                                    
                                                %>
                                                <tr>
                                                    <td>
                                                        <span class="att_name">
                                                            <%= name%></span>
                                                    </td>
                                                    <td>
                                                        <span class="type_text" style="display: none">
                                                            <%= attendant_title_id%></span> <span class="attID" style="display: none">
                                                                <%=att.ID%></span>
                                                        <%
                                                            string title = "";
                                                            int? type = attendant_title_id;
                                                            TayaIT.Enterprise.EMadbatah.Model.AttendantTitle attType = (TayaIT.Enterprise.EMadbatah.Model.AttendantTitle)type.Value;
                                                            switch (attType)
                                                            {
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.M3alyPresident:
                                                                    title = "معالى الرئيس";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.S3adetSessionPresident:
                                                                    title = "سعادة رئيس الجلسة";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.S3adet:
                                                                    title = "سعادة";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.M3aly:
                                                                    title = "معالى";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.AlOstaz:
                                                                    title = "الأستاذ";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.Almostashaar:
                                                                    title = "المستشار";
                                                                    break;
                                                                case TayaIT.Enterprise.EMadbatah.Model.AttendantTitle.NoTitle:
                                                                    title = "بدون لقب";
                                                                    break;
                                                            }
                                                        %>
                                                        <span class="title_text">
                                                            <%= title%></span>
                                                    </td>
                                                    <td>
                                                        <div class="editbuttons" style="display: block; visibility: hidden;">
                                                            <a href="#">Edit</a>
                                                        </div>
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
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <!-- new row end -->
                    <%
                        
                        }//end of foreach
                
                    %>
                </tbody>
            </table>
            <div class="prefix_21">
                <span id="spnMsg"></span>
            </div>
            <div class="editingTools" style="margin-left: 0; z-index: 999; width: 140px; height: 34px;">
                <form method="post" action="dummy.php" id="adminform">
                <select name="title" style="padding: 6px 5px; width: 142px; height: 32px; line-height: 32px;">
                    <option value="1">معالى الرئيس</option>
                    <option value="2">سعادة رئيس الجلسة</option>
                    <option value="3">سعادة</option>
                    <option value="4">معالى</option>
                    <option value="5">الأستاذ</option>
                    <option value="6">المستشار</option>
                    <option value="7">بدون لقب</option>
                </select>
                <div class="tools" style="width: 100px; line-height: 34px; left: -103px; text-align: center;">
                    <a class="save" href="#" style="display: inline-block; margin: 0 5px;">[حفظ]</a>
                    <a class="cancel" href="#" style="display: inline-block; margin: 0 5px;">[الغاء]</a>
                </div>
                </form>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        $('.save').click(function (e) {
            //confirm first
            var answer = confirm("هل أنت متأكد أنك تريد إعادة تخصيص الملف إلى  ؟")

            if (answer) {
                console.log($('.editingTools select[name="title"]').val());
                //create ajax admin handler
                var tools = $(this).closest(".editingTools"),
                    attID = tools.data("attID"),
                    sID = tools.data("sessionID");
                $('.absLoad.loading').show();
                jQuery.ajax({
                    cache: false,
                    type: 'post',
                    url: 'SessionHandler.ashx',
                    data: {
                        funcname: 'UpdateAttendantSessionTitle',
                        sID: sID,
                        attid: attID,
                        s_att_title_id: $('.editingTools select[name="title"]').val()
                    },
                    success: function (response) {
                        var id = $('.editingTools select[name="title"]').val();
                        tools.hide().data("valueField").html(id).siblings(".title_text").html($('.editingTools select option[value="' + id + '"]').text());
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
