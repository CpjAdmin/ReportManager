<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Reporte.aspx.cs" Inherits="CP_Presentacion.Reporte" UICulture="es" Culture="es-CR" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=14.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />

    <title>Report Manager</title>
    <%--Favicon--%>
    <link rel="shorcut icon" type="image/x-icon" href="img/favicon/favicon.ico" />

    <%--CSS--%>
    <link rel="stylesheet" href="dist/v0005/css/AdminLTE.min.css" />
    <link rel="stylesheet" href="v0005/css/reporte.min.css" type="text/css" />
    <!-- Bootstrap 3.3.7 -->
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css" />
    <!-- Alerts -->
    <link rel="stylesheet" href="plugins/jquery-confirm/jquery-confirm-master/dist/jquery-confirm.min.css" />

    <!-- jQuery 3 -->
    <script src="bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>
    <!-- Alerts-Confirm -->
    <script src="plugins/jquery-confirm/jquery-confirm-master/dist/jquery-confirm.min.js"></script>
    <!-- JS Publico -->
    <script src="v0005/js/publico.min.js" type="text/javascript"></script>

</head>
<!--BODY-->
<body class="hold-transition skin-blue sidebar-mini">
    <!--**************************************************************************************************************** INICIO DEL HEADER-->
    <header class="main-header2">
        <!-- LOGO -->
        <a class="logo_reporte">
            <span class="logo_reporte">
                <img src="img/iconos/reporte_logo.jpg" alt="REPORT MANAGER" /></span>
        </a>
        <!-- Barra de navegación del encabezado: el estilo se puede encontrar en header.less -->
        <nav class="navbar navbar-static-top">
            <!-- Menú de la barra de navegación derecha -->
            <div class="navbar-custom-menu">
                <ul class="nav navbar-nav">
                    <!-- CUENTA DE USUARIO: el estilo se puede encontrar en dropdown.less-->
                    <li class="dropdown user user-menu">
                        <a id="user_name">
                            <img src="img/usuario_rm.png" class="user-image" alt="User Image" />
                            <span class="hidden-xs"><%= Session["NombreUsuario"] %></span>
                        </a>
                    </li>
                    <!-- Control Sidebar Toggle Button -->
                    <li>
                        <button id="btnCerrarReporte" onclick="javascript:window.close();opener.window.focus();" class="btn-salir btn-danger btn">CERRAR</button>
                    </li>
                </ul>
            </div>
        </nav>
    </header>
    <form id="form1" runat="server">
        <div class="content2">
            <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="900" EnablePartialRendering="false"></asp:ScriptManager>

            <asp:UpdatePanel runat="server" ID="UpdatePanel">
                <ContentTemplate>
                    <asp:TextBox ID="txtNombreReporte" runat="server"></asp:TextBox>

                    <rsweb:ReportViewer ID="ReportViewer1" runat="server" Font-Names="Tahoma" Font-Size="10pt" waitmessagefont-names="Tahoma" waitmessagefont-size="10pt"
                        waitmessagefont-bold="True" ToolBarItemBorderWidth="1" Font-Bold="True" InternalBorderWidth="1" ShowParameterPrompts="True" ShowCredentialPrompts="False" AsyncRendering="True" SizeToReportContent="True">
                    </rsweb:ReportViewer>

                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>

    <!-- Reportes -->
    <script src="v0005/js/reportes.min.js" type="text/javascript"></script>

</body>
</html>
