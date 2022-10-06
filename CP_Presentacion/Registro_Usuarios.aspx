<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Registro_Usuarios.aspx.cs" Inherits="CP_Presentacion.Registro_Usuarios" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <!-- Botones DataTable -->
    <link rel="stylesheet" href="bower_components/datatables-buttons/v0005/buttons.dataTables.min.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- Encabezado de contenido (encabezado de página) -->
    <section class="content-header">
        <h1 style="text-align: center; width: 400px">MÓDULO DE SEGURIDAD</h1>
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx"><i class="fa fa-dashboard active"></i>Inicio</a></li>
            <li><a href="Login.aspx"><i class="fa fa-close"></i>Salir</a></li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <%--Busqueda por Zona Geográfica<--%>
            <div class="col-md-8">
                <div class="box box-primary" style="height: 135px;">
                    <div class="box-body">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>NOMBRE</label>
                                        <asp:TextBox ID="txtNombre" required="true" runat="server" Text="" CssClass="form-control" oninput="this.value = this.value.toUpperCase()"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>USUARIO</label>
                                        <asp:TextBox ID="txtUsuario" required="true" runat="server" Text="" CssClass="form-control" oninput="this.value = this.value.toUpperCase()"></asp:TextBox>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>CLAVE</label>
                                        <asp:TextBox TextMode="Password" required="true" ID="txtClave" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>SUCURSAL</label>
                                        <asp:DropDownList ID="ddlSucursal" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>DEPARTAMENTO</label>
                                        <asp:DropDownList ID="ddlDepartamento" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>ROL DE USUARIO</label>
                                        <asp:DropDownList ID="ddlRol" runat="server" CssClass="form-control">
                                        </asp:DropDownList>

                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="col-md-4">
                <div class="box box-primary" style="height: 135px;">
                    <div class="box-body">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-xs-8">
                                    <div class="form-group">
                                        <label>FOTO DE PERFÍL</label>
                                        <asp:FileUpload ID="FUploadUsuario" runat="server" CssClass="form-control fileupload" accept="image/*" onchange="vistaPreviaImagen(this);" />
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <img id="imagenUsuario" src="img/usuario.png" alt="Foto" width="120"/>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="center">
            <table>
                <tr>
                    <td>
                        <asp:Button ID="btn_Usuarios_Agregar" runat="server" CssClass="btn btn-primary" Width="150px" Text="Registrar" OnClick="btnRegistrar_Click" />
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <asp:Button ID="btnCancelar" runat="server" CssClass="btn btn-danger" Width="150px" Text="Cancelar" OnClick="btnCancelar_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField ID="FnEditar" runat="server" />
        <%-- ************************************** DATATABLE - Lista de Usuarios--%>
        <!-- /.box-header -->
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-primary">
                    <%--<div class="box-header">
                        <h3 class="box-title">Lista de Usuarios</h3>
                    </div>--%>
                    <%--<span id="txtDuracion" class="help-block " style="color: darkred; font-weight: bold; font-size: 12px;"></span>--%>
                    <div class="box-body table-responsive">
                        <table id="tbl_usuarios" class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Nombre</th>
                                    <th>Sucursal</th>
                                    <th>Departamento</th>
                                    <th>Rol</th>
                                    <th>Login</th>
                                    <th>Estado</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody id="tbl_body_table">
                                <%-- Datos por AJAX--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <!-- /.box-body -->
    </section>

    <%--Modal modal_actualizar--%>
    <div class="modal fade" id="modal_actualizar" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title negrita" id="myModalLabel">Actualizar información de Usuario</h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>NOMBRE</label>
                        <asp:TextBox ID="ModalTxtNombre" runat="server" Text="" CssClass="form-control" oninput="this.value = this.value.toUpperCase()"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>SUCURSAL</label>
                        <asp:DropDownList ID="ModalDdlSucursal" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>DEPARTAMENTO</label>
                        <asp:DropDownList ID="ModalDdlDepartamento" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>ROL DE USUARIO</label>
                        <asp:DropDownList ID="ModalDdlRol" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>CLAVE</label>
                        <asp:TextBox TextMode="Password" requited="true" ID="ModalTxtClave" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>ESTADO</label>
                        <asp:DropDownList ID="ModalDdlEstado" runat="server" CssClass="form-control">
                            <asp:ListItem Text="Activo" Value="A" />
                            <asp:ListItem Text="Inactivo" Value="I" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="center">
                        <table>
                            <tr>
                                <td>
                                    <button runat="server" type="button" class="btn btn-primary" id="btn_Usuarios_Editar">Actualizar</button>
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td>
                                    <button type="button" class="btn btn-danger" id="btnModalCancelar">Cancelar</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </div>
    </div>

</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="footer" runat="server">

    <!-- Botones DataTable -->
    <script src="bower_components/datatables-buttons/v0005/dataTables.buttons.min.js"></script>
    <script src="bower_components/datatables-buttons/v0005/buttons.flash.min.js"></script>
    <script src="bower_components/datatables-buttons/v0005/jszip.min.js"></script>
    <script src="bower_components/datatables-buttons/v0005/vfs_fonts.js"></script>
    <script src="bower_components/datatables-buttons/v0005/buttons.html5_1.0.3.min.js"></script>

    <!-- JS Usuarios -->
    <script src="v0005/js/usuarios.min.js" type="text/javascript"></script>

</asp:Content>
