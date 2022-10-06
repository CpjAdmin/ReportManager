<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ErrorOtro.aspx.cs" Inherits="CP_Presentacion.Error" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    
    <div class="error-page">
     <header class="error-page__header">
        <img class="error-page__header-image" src="img/error_computadora.gif" alt="Computadora GIF"><h1 class="error-page__title nolinks">OOPS! Página no encontrada</h1>
    </header>
    <p class="error-page__message">La página que estás buscando no se pudo encontrar.</p>
    </div>

      <!-- CSS -->
     <link rel="stylesheet" href="css/error.css">

</asp:Content>
