<%@ Page Title="المضبطة الإلكترونية - بداية المضبطه" Language="C#" MasterPageFile="~/Site.master"  ValidateRequest="false" CodeFile="SessionStart.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.SessionStart" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Model" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.BLL" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Util" %>
<%@ Import Namespace="TayaIT.Enterprise.EMadbatah.Web" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript" src="scripts/tiny_mce/jquery.tinymce.js"></script>
    <script type="text/javascript">

        function htmlEncode(value) {
            return $('<div/>').text(value).html();
        }
        function htmlDecode(value) {
            return $('<div/>').html(value).text();
        }
       

$(document).ready(function () {
    // for the popup window
    $("#various1").fancybox({
        'titlePosition': 'inside',
        'transitionIn': 'none',
        'transitionOut': 'none',
        autoDimensions: false,
        padding: 20,
        width: 600,
        height: 120
    });


    $('#btnSave').click(function () {
        var ed = $('#MainContent_elm1').tinymce()
        var sessionFileID = $('#MainContent_SessionFileIDHidden').val();

        // Do you ajax call here, window.setTimeout fakes ajax call
        ed.setProgressState(1); // Show progress
        jQuery.ajax({
            cache: false,
            type: 'post',
            url: 'SessionStartHandler.ashx',
            data: {
                funcname: 'SaveSessionStart',
                sid: $('#MainContent_SessionIDHidden').val(),
                sfid: sessionFileID,
                contentitemtext: htmlEncode($('#MainContent_elm1').val())


            },
            success: function (response) {
                if (response == '1') {
                    ed.setProgressState(0); // Hide progress
                    var qs = getParameterByName('editmode');
                    if (qs && qs == '2')
                        document.location = "ReviewNotes.aspx?sid=" + sessionFileID;
                    else
                        document.location = "Default.aspx";
                }
                else {
                    alert('لقد حدث خطأ');
                }
            },
            error: function () {

            }
        });
        // close popup
        $.fancybox.close()
    })


    // popup buttons actions
    $('#yes').click(function () {
        var ed = $('#MainContent_elm1').tinymce()
        // Do you ajax call here, window.setTimeout fakes ajax call
        ed.setProgressState(1); // Show progress
        jQuery.ajax({
            cache: false,
            type: 'get',
            url: 'SessionStartHandler.ashx',
            data: {
                funcname: 'ReloadAutomaticSessionStart',
                sid: $('#MainContent_SessionIDHidden').val()
            },
            success: function (html) {
                if (html != '') {
                    ed.setProgressState(0); // Hide progress
                    ed.setContent(html);
                }
            },
            error: function () {

            }
        });
        // close popup
        $.fancybox.close()
    })
    $('#no').click(function () {
        $.fancybox.close()
    })
    // to init the editor
    $('textarea.tinymce').tinymce({
        // Location of TinyMCE script
        script_url: 'scripts/tiny_mce/tiny_mce.js',
        // General options
        theme: "advanced",
        plugins: "pagebreak,directionality,noneditable",
        language: "ar",
        // direction
        directionality: "rtl",
        // clean up
        cleanup: true,
        cleanup_on_startup: true,
        width: '100%',
        height: 380,
        theme_advanced_source_editor_wrap: true,
        // Theme options
        theme_advanced_buttons1: "bold,italic",
        theme_advanced_buttons2: "",
        theme_advanced_buttons3: "",
        theme_advanced_buttons4: "",
        theme_advanced_path: false,
        theme_advanced_toolbar_location: "top",
        theme_advanced_toolbar_align: "right",
        theme_advanced_resizing: false,
        // Example content CSS (should be your site CSS)
        content_css: "styles/sessionstart_tinymce_content.css",
        // invalid elements
        invalid_elements: "applet,body,button,caption,fieldset ,font,form,frame,frameset,,head,,html,iframe,img,input,link,meta,object,option,param,script,select,textarea,xmp",
        // valid elements
        //valid_elements: "@[class],span[*],strong,em,br,table",
        force_br_newlines: true,
        force_p_newlines: false,
        forced_root_block: ''
    });
});
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <input id="SessionIDHidden" type="hidden" runat="server" />
    <input id="SessionFileIDHidden" type="hidden" runat="server" />
    <div class="grid_22 prefix_1 suffix_1">
        <h5 class="largerow">
            بداية المضبطة:</h5>
        <div class="graybg">
            <div class="info_cont largerow">
                <div class="fr">
                                                                <%
                                                                    string sessionName = "";
                                                                    string sessionDate = null;
                                                                    if (currentSessionId != -1)
                                                                  {
                                                                      SessionDetails sd = EMadbatahFacade.GetSessionBySessionID(currentSessionId);    
                                                                      sessionName = EMadbatahFacade.GetSessionName(sd.Season, sd.Stage, sd.Serial);
                                                                      sessionDate = sd.Date.Date.ToShortDateString();
                       
                                                                  }%>
                    <span class="title">الجلسة:</span><%=sessionName %> <strong>
                                                                   
                                                                   </strong>
                </div>
                <div class="fr spaceR">
                    <span class="title">بتاريخ:</span> <strong><%=sessionDate%></strong>
                </div>
                <div class="clear"></div>
            </div>
            <div class="Editor_container largerow">
                <textarea id="elm1" name="elm1" rows="15" cols="100" style="width: 100%" runat="server" class="tinymce" enableviewstate="true">
                
                </textarea>
            </div>
            <div class="actions_container tex_align_en">
                <!--<input type="submit" id="btnSave" class="btn" value="حفظ و خروج" runat="server" />-->
                <form runat="server">
                <%--<asp:Button  ID = "saveBtn" runat="server" CssClass="btn" Text ="حفظ و خروج" OnClick ="saveBtn_Click"/>--%>
               <div class="addnewusercont">
                <input id="btnSave" type="button" class="actionbtnsmenu" value="حفظ و خروج" />
                <a id="various1" href="#inline1">تحميل النسخة المعدة مسبقا من النص</a>
                </div>
                </form>
                
            </div>
        </div>
    </div>
    <div class="clear">
    </div>
    <!--Begin popup container div-->
    <div class="main_popup_container displaynone">
        <div id="inline1" class="popup_container_content tex_align_center">
            <h5 class="largerow">
                هل أنت موافق على العودة للنص الأصلي؟</h5>
            <div class="largerow">
                سوف يتم إستبدال كل التعديلات التي قمت بها بالنص الأصلي الموجود، يرجي العلم بأنه
                في حال موافقتك علي إستبدال التعديلات بالنص الأصلي، فإنه لا يمكن العودة لتعديلاتك
                مرة أخري.</div>
            <div class="actions_btns addnewusercont">
                <input type="button" class="actionbtnsmenu" id="yes" value="نعم" />
                <input type="button" class="actionbtnsmenu" id="no" value="لا" />
            </div>
        </div>
    </div>
</asp:Content>
