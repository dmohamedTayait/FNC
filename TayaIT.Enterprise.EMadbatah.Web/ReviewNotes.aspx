<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReviewNotes.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.ReviewNotes" MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - صفحة المراجعه" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
        <div class="grid_22 prefix_1" id="mainNotes" runat="server">
          <div class="notes-top-header">
            <div class="grid_3 alpha">
              <h4>الملاحظات:</h4>
            </div>
            <div class="grid_9 prefix_10 alpha omega"> 
                <span class="info_cont"> 
                    <span class="title">الجلسة:</span> 
                    <span class="Bold_title" runat= "server" id="sessionTitle">[464/14ف]</span> 
                </span> 
                <span class="info_cont prefix_1"> 
                    <span class="title">بتاريخ:</span>
                    <span class="Bold_title" runat= "server" id="sessionDate">23-6-2010</span> 
                </span> 
            </div>
            <div class="clear"></div>
          </div>

          <div id="reviewNotes" runat="server"></div>
          
          </div>
        <script>
            $(document).ready(function () {
                $('.expandicondown').click(function () {
                    var id = jQuery(this).attr("id");
                    if ($('.notes-items' + id).length > 0) {
                        $(this).toggleClass('expandicondown expandiconup');
                        $('.notes-items' + id).slideToggle();
                    }
                });

                $('#MainContent_reviewNotes .graybar').hover(function () {
                  var current = $(this);
                  $('#MainContent_reviewNotes .graybar').not(current).find(".Bold_title").removeClass("grayhover");
                  current.find(".Bold_title").toggleClass("grayhover");
              });

              //$('#MainContent_reviewNotes .graybar').click(function () {
                  //var currentid = $(this).find('.expandicondown').attr('id');
                  //if ($('.notes-items' + currentid).length > 0) {
                  //    $('.notes-items' + currentid).slideToggle();
                 // }
              //});

            });
        </script>
</asp:Content>