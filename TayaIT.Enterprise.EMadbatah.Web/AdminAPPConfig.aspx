<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdminAPPConfig.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AdminAPPConfig" MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - إدارة الإعدادات" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_22 prefix_2 xxlargerow">
            <div class="Ntitle">إعدادت التطبيق:</div>
          </div>
          <div class="clear"></div>
          <div class="grid_19 prefix_5">
          	<div class="largerow">
            	<div class="grid_5 h3">مسار خادم تحويل الصوت لنص:</div>
                <div class="grid_8">
                <input name="vecsysPath" id="vecsysPath" type="text" runat="server" class="textfield tex_en" size="45"/></div>
                <div class="clear"></div>
            </div>
            <div class="largerow">
            	<div class="grid_5 h3">مسار خادم ملفات الصوت:</div>
                <div class="grid_8">
                <input name="audioPath" id="audioPath" type="text" runat="server" class="textfield tex_en" size="45"/></div>
                <div class="clear"></div>
            </div>
            <div class="largerow">
            	<div class="grid_5 h3">البرلمان الالكترونى:</div>
                <div class="grid_8"><input name="EparliamantUrl" id="EparliamantUrl" type="text" runat="server" class="textfield tex_en" size="45"/></div>
                <div class="clear"></div>
            </div>
            <div class="prefix_5 addnewusercont">
            	<a id="btnSave" href="#" class="actionbtnsmenu cnfsave">حفظ</a>
                <span id="spnMsg"></span>
            </div>
          </div>
          
          <div class="clear">

          <link rel="stylesheet" type="text/css" href="styles/jquery.fancybox-1.3.4.css" />
    <script type="text/javascript" src="scripts/jquery.fancybox-1.3.4.pack.js"></script>
    <script type="text/javascript">
        $('#btnSave').click(function (e) {
            $('.absLoad.loading').show();
            jQuery.ajax({
                cache: false,
                type: 'post',
                url: 'AdminHandler.ashx',
                data: {
                    funcname: 'UpdateServersPaths',
                    vecpath: $('#MainContent_vecsysPath').val(),
                    epurl: $('#MainContent_EparliamantUrl').val(),
                    audiopath: $('#MainContent_audioPath').val()

                },
                success: function (response) 
                {
                    if (response == true) {//| response != 'true')
                        $('#spnMsg').val('تم الحفظ');
                    }
                    else {
                        $('#spnMsg').val('لقد حدث خطأ');
                    }

                    $('.absLoad.loading').hide();
                }
               
            });
        })
    
    </script>

          </div>
          
</asp:Content>
