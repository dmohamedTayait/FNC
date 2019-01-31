<%@ Page Title = "المضبطة الإلكترونية - ادارة النظام" Language="C#" AutoEventWireup="true" CodeFile="AdminHome.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AdminHome" MasterPageFile="~/Site.master" %>
<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="grid_22 prefix_1 suffix_1">
        <div class="xxlargerow Ntitle">إختر الصفحة المراد إدارتها:</div>
        <div class="toolsbox suffix_1 prefix_1">
            <div class="adminhomebutton setting_icon"><a href="AdminAPPConfig.aspx">إدارة الإعدادات</a></div>
            <div class="adminhomebutton user_icon"><a href="AdminSecurity.aspx">إدارة المستخدمين</a></div>
            <div class="adminhomebutton session_icon"><a href="AdminSessions.aspx">إدارة الجلسات</a></div>
            <div class="adminhomebutton user_icon"><a href="SpeakersTitles.aspx">تعريف المتحدثين</a></div>
            <div class="clear"></div>
        </div>
    </div>
    <div class="clear">
    </div>
</asp:Content>
