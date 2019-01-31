<%@ Page Language="C#" AutoEventWireup="true" CodeFile="EditSessionFile.aspx.cs"
    Inherits="TayaIT.Enterprise.EMadbatah.Web.EditSessionFile" MasterPageFile="~/Site.master"
    Title="المضبطة الإلكترونية - تعديل الملف الصوتي" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <script type="text/javascript" src="scripts/jquery.hotkeys.js"></script>
    <script type="text/javascript" src="scripts/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript" src="scripts/jPlayer/jquery.jplayer.min.js"></script>
    <script type="text/javascript" src="scripts/EditSessionScript.js"></script>
    <link href="styles/jplayer.blue.monday.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
// <![CDATA[

        function Button1_onclick() {
         $.ajax({ url: "FileError.aspx?sid=" + <%=SessionFileID %>});
                                    if(document.getElementById("Button1").value == "خطأ" )
                                        document.getElementById("Button1").value = "خطأ هنا";
                                    else
                                        document.getElementById("Button1").value = "خطأ";
                                    
                                    // you can add some processing to the AJAX call
                                    return false;
        }

// ]]>
    </script>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <% 
        if (CurrentUser == null || file == null)
        {
            Context.Response.Redirect("~/Error.aspx");
        }
    //if (CurrentUser.ID == file.UserID || file.UserID == null
    //            || ((CurrentUser.Role != UserRole.DataEntry) && (current_session.ReviewerID == CurrentUser.ID)))
            
    %>
    <form id="editSessionFileForm" runat="server">
    <input id="MP3FilePath" class="MP3FilePath" type="hidden" runat="server" value="" />
    <input id="sessionID" type="hidden" value="" runat="server" class="sessionID" />
    <input id="XmlFilePath" type="hidden" value="" runat="server" class="hdxmlFilePath" />
    <input type="hidden" id="hdPageMode" runat="server" value="1" class="hdPageMode" />
    <input type="hidden" id="hdSessionContentItemID" runat="server" value="1" class="hdSessionContentItemID" />
    <input type="hidden" id="startTime" runat="server" value="" class="hdstartTime" />
    <input type="hidden" id="endTime" runat="server" value="" class="hdendTime" />
    <input type="hidden" id="currentOrder" runat="server" value="" class="hdcurrentOrder" />
    <div id="editSessionFile" class="grid_22 prefix_1 suffix_1">
        <div class="borderBD row">
            <div class="fr">
                الملف: <strong>
                    <asp:Label ID="lblMP3FileName" runat="server"></asp:Label></strong>
            </div>
            <div class="fl spaceR">
                بتاريخ: <strong>
                    <asp:Label ID="lblSessionDate" runat="server"></asp:Label></strong>
            </div>
            <div class="fl">
                الجلسة: <strong>[<asp:Label ID="lblSessionName" runat="server"></asp:Label>]</strong>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="row">
            <div class="row">
                <div class="grid_3">
                    <span class="red">*</span> البند:
                </div>
                <div class="grid_7">
                    <div class="marginBS">
                        <asp:DropDownList ID="ddlAgendaItems" AutoPostBack="false" runat="server">
                        </asp:DropDownList>
                    </div>



                    <div>
                        <input type="checkbox" class="chkGroupSubAgendaItems" id="chkGroupSubAgendaItems" runat="server" />
                        <label>دمج البنود الفرعية</label>
                    </div>





                </div>
                <div class="grid_3 prefix_1">
                    البند الفرعى:</div>
                <div class="grid_7">
                    <div class="marginBS">
                        <asp:DropDownList ID="ddlAgendaSubItems" AutoPostBack="false" runat="server">
                        </asp:DropDownList>
                    </div>
                                        <div>
                        <input name="" type="checkbox" id="specialBranch" value="" />
                        <label for="specialBranch">
                            بند استثنائى</label>
                        <div id="smallwindow" class="smallwindow">
                            <div class="smallwindowCont">
                                <div class="smallwindowArrow">
                                </div>
                                <div class="marginBS title">
                                    ادخل اسم البند الأستثنائى</div>
                                <div class="inputs">
                                    <input name="txtAgendaItem" type="text" id="txtAgendaItem" class="textfield">
                                    <input name="" type="button" id="btnAddCustomAgendaItem" class="smallbtn" value="إضافة">
                                    <div class="errorCont">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="row">
                <div class="grid_3">
                    <span class="red">*</span> المتحدث:
                </div>
                <div class="grid_7">
                    <div class="marginBS">
                        <asp:DropDownList ID="ddlSpeakers" AutoPostBack="false" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div>
                        <input name="" id="sameAsPrevSpeaker" runat="server" class="sameAsPrevSpeaker" type="checkbox"
                            value="" />
                        <label>
                            نفس المتحدث السابق</label>
                    </div>
                </div>
                <div class="grid_3 prefix_1">
                    وظيفة المتحدث:</div>
                <div class="grid_7">
                    <div id="newjobtitle" class="edit">
                        <div class="editmode">
                            <%--<input id="addnewjobbutton" name="" type="button" class="smallbtn" value="إضافة">--%>
                            <input name="addnewjobtext" id="txtSpeakerJob" runat="server" type="text" class="textfield"
                                size="26">
                        </div>
                        <div class="donemode">
                            <strong></strong>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<a id="editnewjobbutton" href="#">[تعديل]</a>
                        </div>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <div id="jquery_jplayer_1" class="jp-jplayer">
        </div>
        <div id="jp_container_1" class="jp-audio">
            <div class="jp-type-single">
                <div id="jp_interface_1" class="jp-interface">
                    <ul class="jp-controls">
                        <li><a href="#" class="jp-play" tabindex="1" title="play"></a></li>
                        <li><a href="#" class="jp-pause" tabindex="1" title="pause"></a></li>
                    </ul>
                    <div class="jp-progress">
                        <div class="jp-seek-bar">
                            <div class="jp-play-bar">
                            </div>
                        </div>
                    </div>
                    <div class="jp-current-time">
                    </div>
                    <div class="jp-duration">
                    </div>
                    <div class="next-jp-xseconds" title="تقديم 5 ثوانى">
                    </div>
                    <div class="prev-jp-xseconds" title="تاخير 5 ثوانى">
                    </div>
                </div>
            </div>
        </div>
        <div class="graybg">
            <div class="row">
                <textarea id="elm1" runat="server" name="elm1" rows="3" style="width: 100%" class="tinymce"></textarea>
            </div>
            <div class="row">
                <div class="grid_11 alpha">
                    <div class="row">
                        تعليق على الفقرة</div>
                    <div>
                        <textarea rows="3" id="txtComments" runat="server" class="textfield fitAll"></textarea>
                    </div>
                </div>
                <div class="grid_10 prefix_1 omega">
                    <div class="row">
                        تذيل الصفحة</div>
                    <div>
                        <textarea rows="3" id="txtFooter" runat="server" class="textfield fitAll"></textarea>
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="nav row">
                <div class="fr">
                    <!--disabled="disabled" -->
                    <input name="" runat="server" disabled="disabled" id="btnNext" type="button" class="btn next"
                        value="التالى" />
                    &nbsp;&nbsp;
                    <input name="" runat="server" disabled="disabled" id="btnPrev" type="button" class="btn prev"
                        value="السابق" />
                        &nbsp;&nbsp;
                       
<%--                        <input name="" runat="server" disabled="disabled" id="Button1" type="button" class="btn"
                        value="تجاهل" />--%>
                                               <input name="chkIgnoredSegment" id="chkIgnoredSegment" runat="server" class="chkIgnoredSegment" type="checkbox"
                            value="" />
                        <label for="chkIgnoredSegment">تجاهل هذا المقطع</label>


                </div>
                <div class="fr prefix_3">
                    <input name="" runat="server" disabled="disabled" id="btnFinish" type="button" class="btn finish"
                        value="انهاء" />
                </div>
                <div class="fl">
                    
                    <input name="" id="btnSplit" runat="server" type="button" class="btn split"
                        value="اقطع" />
                        &nbsp;&nbsp;
                    <input name="" type="button" id="btnSaveAndExit" class="btn" value="حفظ و خروج" />
                    &nbsp;&nbsp;
                    <input name="" type="button" id="various1" data-div="#inline1" class="btn" value="عودة للنص الأصلى" />
                    &nbsp;&nbsp;
                    <input id="Button1" class ="btn " type="button"  value="خطأ" onclick="return Button1_onclick()" />
                </div>
                <div class="clear">
                </div>
            </div>
        </div>
        <!--Begin popup container div-->
        <div class="main_popup_container displaynone">
            <div id="inline1" class="popup_container_content tex_align_center">
                <h5 class="row">
                    هل أنت موافق على العودة للنص الأصلي؟</h5>
                <div class="row">
                    سوف يتم إستبدال كل التعديلات التي قمت بها بالنص الأصلي الموجود، يرجي العلم بأنه
                    في حال موافقتك علي إستبدال التعديلات بالنص الأصلي، فإنه لا يمكن العودة لتعديلاتك
                    مرة أخري.</div>
                <div class="actions_btns">
                    <input type="button" class="smallbtn" id="yes" value="نعم">
                    <input type="button" class="smallbtn" id="no" value="لا">
                </div>
            </div>
        </div>
        <div class="popupoverlay">
                    </div>
        <div class="reviewpopup_cont graybg">
            <div class="close_btn">
            </div>
            <div class="clear">
            </div>
            <div class="datacontainer inputcont">
                 <textarea id="Textarea1" runat="server" name="elm1" rows="3" style="width: 100%" class="splittinymce"></textarea>
            </div>
            <div class="poppbtnscont fl">
                <div class="fl">
                    <div class="fl">
                        <input type="button" id="approve" class="approve" value="موافق" />
                    </div>
                    <div class="clear">
                    </div>
                </div>
                <div class="clear">
                </div>
            </div>
            <div class="clear">
            </div>
        </div>
        <div class="clear">
        </div>
    </form>
</asp:Content>
