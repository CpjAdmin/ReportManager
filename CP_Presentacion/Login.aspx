<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CP_Presentacion.Login" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <%--Favicon--%>
    <link rel="shorcut icon" type="image/x-icon" href="img/favicon/favicon.ico" />
    <!-- Hacer al navegador Responsive -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1, user-scalable=no" />

    <title>Report Manager - COOPECAJA RL</title>
    <link rel="stylesheet" href="v0005/css/fonts/font-awesome-4.7.0/css/font-awesome.css" />
    <link rel="stylesheet" href="v0005/css/login_v1.min.css" />

    <!-- Bootstrap v3.3.7 -->
    <link rel="stylesheet" href="bower_components/bootstrap/dist/css/bootstrap.min.css" type="text/css">
    <!-- Alerts -->
    <link rel="stylesheet" href="plugins/jquery-confirm/jquery-confirm-master/dist/jquery-confirm.min.css">
    <!-- jQuery 3 -->
    <script src="bower_components/jquery/dist/jquery.min.js" type="text/javascript"></script>
    <!-- Bootstrap 3.3.7 -->
    <script src="bower_components/bootstrap/dist/js/bootstrap.min.js" type="text/javascript"></script>
    <!-- Alerts-Confirm -->
    <script src="plugins/jquery-confirm/jquery-confirm-master/dist/jquery-confirm.min.js"></script>


    <!-- JS Publico -->
    <script src="v0005/js/publico.min.js" type="text/javascript"></script>
    <!-- JS Login -->
    <script src="v0005/js/login.min.js" type="text/javascript"></script>


</head>
<body>

    <div class="main">
        <div class="containerLogin">
            <div class="center">
                <%--Mensaje de Alerta--%>
                <div style='position: fixed; top: -100px; left: 50%;'>
                    <div id="mensajeAlerta"></div>
                </div>
                <div id="login">
                    <form id="formLogin" runat="server">
                        <fieldset class="clearfix">
                            <!-- Textbox Usuario y Contraseña -->
                            <p>
                                <span class="fa fa-user"></span>
                                <input id="usuario" class="InputUser form-control #input-block-level" type="text" runat="server" placeholder="Usuario" required="required" oninput="this.value = this.value.toUpperCase()" autocomplete="off"/>
                            </p>
                            <p>
                                <span class="fa fa-lock"></span>
                                      <input type="password" id="password" name="con" runat="server" class="form-control input-block-level" 
                                          data-toggle="tooltip" data-placement="right" data-trigger="manual" title="La tecla Bloq Mayús está activada." 
                                          placeholder="Contrase&ntilde;a" onkeydown="BlodMayus(event,id)" autocomplete="off" required>
                                </p>
                            <div>
                                <!-- Botón -->
                                <asp:Button ID="btnIngresar" runat="server" Text="Ingresar" OnClick="btnIngresar_Click" />
                            </div>
                        </fieldset>
                        <div class="clearfix"></div>
                    </form>
                    <div class="clearfix"></div>
                </div>
                <div class="logo">
                    <img id="logo" src="img/logo_rm_login.png" alt="REPORT MANAGER" />
                    <div class="clearfix"></div>
                </div>
            </div>
        </div>
    </div>
</body>
</html>
