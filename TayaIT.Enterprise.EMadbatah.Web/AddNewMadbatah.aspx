<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddNewMadbatah.aspx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.AddNewMadbatah" MasterPageFile="~/Site.master" %>

<asp:Content  ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
<form runat='server' id="form1">
    <div>
        <asp:TextBox ID="txtMadbatahTitle" runat="server" ></asp:TextBox>
        <asp:TextBox ID="txtSessionNo" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>

        <asp:Button ID="Button1" runat="server" Text="Button" onclick="Button1_Click" />
    
    </div>
</form>
</asp:Content>
