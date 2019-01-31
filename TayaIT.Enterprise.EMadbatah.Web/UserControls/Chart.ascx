<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Chart.ascx.cs" Inherits="TayaIT.Enterprise.EMadbatah.Web.UserControls_Chart" %>
<%@ Register Assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI.DataVisualization.Charting" TagPrefix="asp" %>
<asp:chart id="Chart1" runat="server" ImageType="Png" ImageLocation="~/TempImages/ChartPic_#SEQ(300,3)"
    BackColor="#D3DFF0" Width="412px" Height="296px" BorderColor="26, 59, 105" Palette="BrightPastel"
    BorderlineDashStyle="Solid" BackGradientStyle="TopBottom" BorderWidth="2">
    
    <titles>
        <asp:title ShadowColor="32, 0, 0, 0" Font="Trebuchet MS, 14.25pt, style=Bold" ShadowOffset="3" Text="Parking Violations" Alignment="MiddleLeft" ForeColor="26, 59, 105"></asp:title>
    </titles>
    
    <legends>
        <asp:legend Enabled="False" IsTextAutoFit="False" Name="Default" BackColor="Transparent" Font="Trebuchet MS, 8.25pt, style=Bold"></asp:legend>
    </legends>

    <borderskin skinstyle="Emboss"></borderskin>

    <series>
        <asp:series XValueType="Double" Name="Series 1" ChartType="Pie" BorderColor="180, 26, 59, 105" Color="220, 65, 140, 240" YValueType="Double" font="Trebuchet MS, 8.25pt, style=Bold"></asp:series>
    </series>

    <chartareas>
	    <asp:chartarea Name="ChartArea1" BorderColor="64, 64, 64, 64" BackSecondaryColor="Transparent" BackColor="64, 165, 191, 228" ShadowColor="Transparent" BackGradientStyle="TopBottom">
		    <area3dstyle Rotation="-21" perspective="10" enable3d="True" Inclination="48" IsRightAngleAxes="False" wallwidth="0" IsClustered="False"></area3dstyle>
		    <axisy linecolor="64, 64, 64, 64">
			    <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" />
			    <majorgrid linecolor="64, 64, 64, 64" />
		    </axisy>
		    <axisx linecolor="64, 64, 64, 64">
			    <labelstyle font="Trebuchet MS, 8.25pt, style=Bold" />
			    <majorgrid linecolor="64, 64, 64, 64" />
		    </axisx>
	    </asp:chartarea>
    </chartareas>
</asp:chart>

