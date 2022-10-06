<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Perfiles.aspx.cs" Inherits="CP_Presentacion.Perfiles" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- Botones DataTable -->
    <link rel="stylesheet" href="bower_components/datatables-buttons/v0005/buttons.dataTables.min.css">
    <!-- icheck -->
    <link rel="stylesheet" href="bower_components/icheck/skins/futurico/futurico.css">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Encabezado de contenido (encabezado de página) -->
    <section class="content-header">
        <h1 style="text-align: center; width: 290px">MÓDULO DE PERFILES</h1>
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx"><i class="fa fa-dashboard active"></i>Inicio</a></li>
            <li><a href="Login.aspx"><i class="fa fa-close"></i>Salir</a></li>
        </ol>
    </section>

    <section class="content">
        <%--Mensaje de Alerta--%>
        <div class="center">
            <div id="mensajeAlerta"></div>
        </div>

        <%--Formulario de Perfiles--%>
        <div class="row">
            <div class="col-md-3">
                <div class="box box-primary" style="height: 285px;">
                    <div class="box-body">
                        <div class="form-group">
                            <table>
                                <tr>
                                    <td>
                                        <label>ID PERFIL</label>
                                        <input id="txtIDPerfil" disabled style="width: 60px" class="textoCentrado form-control" />
                                    </td>
                                    <td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                    <td id="tdEstado">
                                        <label>ESTADO</label>
                                        <select disabled runat="server" id="ddlEstado" class="form-control">
                                            <option value="A">Activo</option>
                                            <option value="I">Inactivo</option>
                                            <option value="P">Pendiente</option>
                                        </select>
                                    </td>
                                </tr>
                            </table>
                        </div>

                        <div class="form-group">
                            <label>NOMBRE</label>
                            <input runat="server" id="txtNombrePerfil" maxlength="50" class="form-control" oninput="this.value = this.value.toUpperCase()" />
                        </div>
                        <div class="form-group">
                            <label>DESCRIPCIÓN</label>
                            <textarea id="txtDescripcionPerfil" style="resize: none" maxlength="83" class="form-control" rows="2" placeholder="Descripción del Perfil ..."></textarea>
                        </div>
                        <div class="form-group">
                            <div class="center">
                                <table>
                                    <tr>
                                        <td>
                                            <button runat="server" type="button" class="btn btn-primary Perfil" id="btn_Perfiles_Agregar" style="width: 80px;">Guardar</button>
                                        </td>
                                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                        <td>
                                            <button runat="server" type="button" class="btn btn-danger" id="btnCancelar" style="width: 80px;">Limpiar</button>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="FnAgregar" runat="server" />
            <asp:HiddenField ID="FnEditar" runat="server" />
            <%-- ************************************** DATATABLE--%>

            <%--INICIO - Tabla tbl_perfiles--%>
            <div class="col-sm-9">
                <div class="box box-primary">
                    <div class="box-body">
                        <div class="form-group">
                            <%-- <label>LISTA DE PERFILES</label>--%>
                            <div class="box-body table-responsive">
                                <table id="tbl_perfiles" class="table table-bordered table-hover">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>NOMBRE</th>
                                            <th>DESCRIPCION</th>
                                            <th>ESTADO</th>
                                            <th>ACCIONES</th>
                                        </tr>
                                    </thead>
                                    <tbody id="tbl_perfil_body">
                                        <%-- Datos por AJAX--%>
                                    </tbody>
                                </table>
                            </div>
                            <%-- FIN - Tabla tbl_perfiles--%>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <%--INICIO - Tablas--%>
            <div class="col-sm-12">
                <div class="box box-primary">
                    <!-- Custom Tabs -->
                    <div class="nav-tabs-custom">
                        <ul class="nav nav-tabs">
                            <li id="tabUsuarios" class="active" data-toggle="tooltip" data-placement="bottom" title="Asignar Usuarios"><a href="#tab_2_usuarios" data-toggle="tab">
                                <i class="fa fa-user-plus fa-2x"></i>
                                <label>USUARIOS ASIGNADOS</label></a></li>
                            <li data-toggle="tooltip" data-placement="bottom" title="Asignar Reportes"><a href="#tab_1_reportes" data-toggle="tab">
                                <i class="fa fa-bar-chart fa-2x"></i>
                                <label>REPORTES ASIGNADOS</label></a></li>
                        </ul>
                        <div style="padding: 0px" class="tab-content">
                            <div class="tab-pane active" id="tab_2_usuarios">
                                <%-- ************************************** DATATABLE - Usuarios--%>
                                <div class="col-sm-12">
                                    <div class="box">
                                        <%--<span id="txtDuracion" class="help-block " style="color: darkred; font-weight: bold; font-size: 12px;"></span>--%>
                                        <div class="box-body table-responsive">
                                            <table id="tbl_usuarios" class="table table-bordered table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            <input type="checkbox" id="iCheckUsuarios"></th>
                                                        <th>ID</th>
                                                        <th>NOMBRE</th>
                                                        <th>SUCURSAL</th>
                                                        <th>DEPARTAMENTO</th>
                                                        <th>ROL</th>
                                                        <th>LOGIN</th>
                                                        <th>ESTADO</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbl_body_usuarios">
                                                    <%-- Datos por AJAX--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <!-- /.box-body -->
                            </div>
                            <!-- /.tab-pane -->
                            <div class="tab-pane" id="tab_1_reportes">
                                <%-- ************************************** DATATABLE - Reportes--%>
                                <div class="col-sm-12">
                                    <div class="box">
                                        <div class="box-body table-responsive">
                                            <table id="tbl_reportes" class="table table-bordered table-hover">
                                                <thead>
                                                    <tr>
                                                        <th>
                                                            <input type="checkbox" id="iCheckReportes"></th>
                                                        <th>ID</th>
                                                        <th>SISTEMA</th>
                                                        <th>NOMBRE</th>
                                                        <th>CODIGO</th>
                                                        <th>UBICACION</th>
                                                        <th>PROPIETARIO</th>
                                                        <th>ESTADO</th>
                                                    </tr>
                                                </thead>
                                                <tbody id="tbl_body_table">
                                                    <%-- Datos por AJAX--%>
                                                </tbody>
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <%-- FIN - Tabla tbl_reportes--%>
                            </div>
                            <!-- /.tab-pane -->
                        </div>
                        <!-- /.tab-content -->
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
                    <h4 class="modal-title negrita" id="ModalTitulo"></h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <label>PERFIL :</label>
                        <input id="ModalTxtPerfilDetalle" class="form-control textoCentrado" readonly />
                    </div>
                    <div class="form-group">
                        <label>DESCRIPCIÓN :</label>
                        <input id="ModalTxtPerfilDescripcion" class="form-control text-bold" readonly />
                    </div>
                    <div class="form-group">
                        <label>REPORTES :</label>
                        <textarea id="ModalTxtPerfilReportes" style="resize: none" class="form-control text-bold" rows="3" readonly></textarea>
                    </div>
                    <div class="form-group">
                        <label>USUARIOS :</label>
                        <textarea id="ModalTxtPerfilUsuarios" style="resize: none" class="form-control text-bold" rows="7" readonly></textarea>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="center">
                        <table>
                            <tr>
                                <td>
                                    <button runat="server" type="button" class="btn btn-primary Perfil" id="btn_Perfiles_Editar" data-dismiss="modal">Actualizar</button>
                                </td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td>
                                    <button type="button" class="btn btn-danger" id="btnModalCancelar" data-dismiss="modal">Cancelar</button>
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

    <!-- JS icheck -->
    <script src="bower_components/icheck/icheck.js"></script>
    <!-- JS PERFILES -->
    <script src="v0005/js/perfiles_v2.1.min.js" type="text/javascript"></script>

</asp:Content>
