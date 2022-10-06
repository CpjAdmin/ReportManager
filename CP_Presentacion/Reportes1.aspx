<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Reportes1.aspx.cs" Inherits="CP_Presentacion.Reportes" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>



<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

        <asp:TextBox ID="txtNombreReporte" runat="server" BorderColor="White" Font-Bold="True" visible="true" Font-Size="Large" ForeColor="White" BackColor="#325d81" Width="100%"></asp:TextBox>

        <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Tahoma" Font-Size="10pt" WaitMessageFont-Names="Tahoma" WaitMessageFont-Size="10pt"
            WaitMessageFont-Bold="True" ToolBarItemBorderWidth="1" Font-Bold="True" InternalBorderWidth="1" ShowParameterPrompts="True" ShowCredentialPrompts="False" AsyncRendering="True">
        </rsweb:ReportViewer>
    </div>

</asp:Content>
