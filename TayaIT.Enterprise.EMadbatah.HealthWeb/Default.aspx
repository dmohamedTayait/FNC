<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AdminAPPConfig"
    MasterPageFile="~/Site.master" Title="المضبطة الإلكترونية - فحص حالة النظام" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="Ntitle clearfix largerow">
        <center>
            <span class="fr">حالة النظام:</span>
            <input type="button" id="btnReload" value="اعادة الفحص" /></center>
    </div>
    <div class="largerow">
        <div class="largerow clearfix">
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل خادم الصوت يمكن الوصول اليه:</div>
                <div class="grid_1">
                    <input name="imgIsAudioServerAccessible" id="imgIsAudioServerAccessible" type="image"
                        runat="server" class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل خادم تحويل الصوت يمكن الوصول اليه:</div>
                <div class="grid_1">
                    <input name="imgIsVecSysServerAccessible" id="imgIsVecSysServerAccessible" type="image"
                        runat="server" class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل برنامج نقل الملفات يعمل:</div>
                <div class="grid_1">
                    <input name="imgIsWatcherRunning" id="imgIsWatcherRunning" type="image" runat="server"
                        class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل خدمةالتعامل مع نظام البرلمان الالكتروني يمكن الوصول اليها:</div>
                <div class="grid_1">
                    <input name="imgIsServiceAccessible" id="imgIsServiceAccessible" type="image" runat="server"
                        class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل موقع المضبطة الالكترونية يعمل :</div>
                <div class="grid_1">
                    <input name="imgIsMadbatahAccessible" id="imgIsMadbatahAccessible" type="image" runat="server"
                        class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل قاعدة بيانات المجلس تعمل:</div>
                <div class="grid_1">
                    <input name="imgIsFNCDBAcessible" id="imgIsFNCDBAcessible" type="image" runat="server"
                        class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
            <div class="grid_8">
                <div class="grid_6 h2">
                    هل قاعدة بيانات المضبطة الالكترونيه تعمل:</div>
                <div class="grid_1">
                    <input name="imgIsEMadbtahDBAcessible" id="imgIsEMadbtahDBAcessible" type="image"
                        runat="server" class="textfield tex_en bgw" size="45" /></div>
                <div class="clear">
                </div>
            </div>
        </div>
    <div class="clear">
    </div>
    <div class="largerow">
        <div class="grid_8">
            <div class="row h3">
                <center>
                    Watcher Log File</center>
            </div>
            <div>
                <textarea name="txtWatcherLogFilePath" id="txtWatcherLogFilePath" rows="20" class="fitAll dirrtl"
                    runat="server"></textarea>
            </div>
        </div>
        <div class="grid_8">
            <div class="row h3">
                <center>
                    Service Log File</center>
            </div>
            <div>
                <textarea name="txtServiceLogFilePath" id="txtServiceLogFilePath" rows="20" class="fitAll dirrtl"
                    runat="server"></textarea>
            </div>
        </div>
        <div class="grid_8">
            <div class="row h3">
                <center>
                    Madbatah Log File</center>
            </div>
            <div>
                <textarea name="txtMadbatahLogFilePath" id="txtMadbatahLogFilePath" rows="20" class="fitAll dirrtl"
                    runat="server"></textarea>
            </div>
        </div>
        <div class="clear">
        </div>
    </div>
    </div>
    <div class="clear">
        <link rel="stylesheet" type="text/css" href="styles/jquery.fancybox-1.3.4.css" />
        <script type="text/javascript" src="scripts/jquery.fancybox-1.3.4.pack.js"></script>
        <script type="text/javascript">
            $('#btnReload').click(function (e) {
                window.setTimeout('location.reload()', 10);
            })
    
        </script>
    </div>
</asp:Content>
