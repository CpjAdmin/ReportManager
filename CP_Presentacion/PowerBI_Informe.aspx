<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="PowerBI_Informe.aspx.cs" Inherits="CP_Presentacion.PowerBI_Informe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content-header">
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx"><i class="fa fa-dashboard active"></i>Inicio</a></li>
        </ol>
    </section>

    <iframe id="FramePBI" runat="server" src="link"></iframe>

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">

</asp:Content>
