<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="ListaReportes.aspx.cs" Inherits="CP_Presentacion.ListadoReportes" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <link href="bower_components/icheck/skins/futurico/futurico.css" rel="stylesheet">
    <!-- Botones DataTable -->
    <link rel="stylesheet" href="bower_components/datatables-buttons/v0005/buttons.dataTables.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content-header">
        <h1 style="text-align: center; width: 400px">MÓDULO DE REPORTES</h1>
        <ol class="breadcrumb">
            <li><a href="Inicio.aspx"><i class="fa fa-dashboard active"></i>Inicio</a></li>
            <li><a href="Login.aspx"><i class="fa fa-close"></i>Salir</a></li>
        </ol>
    </section>

    <section class="content">
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-primary">
                    <%-- ************************************** DATATABLE - Reportes--%>
                    <%--<span id="txtDuracion" class="help-block " style="color: darkred; font-weight: bold; font-size: 12px;"></span>--%>
                    <div class="box-body table-responsive">
                        <table id="tbl_reportes" class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>SISTEMA</th>
                                    <th>NOMBRE</th>
                                    <th>CODIGO</th>
                                    <th>PROPIETARIO</th>
                                    <th>UBICACION</th>
                                    <th>DIRECTORIO</th>
                                    <th>SUBDIRECTORIO</th>
                                    <th>ESTADO</th>
                                    <th><span title="Generar Reportes" class="btn btn-xs btn-warning btn_datatable"><i class="glyphicon glyphicon-ok" aria-hidden="true"></i></span></th>
                                    <th>ACCIONES</th>
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

    <asp:HiddenField ID="FnAgregar" runat="server" />
    <asp:HiddenField ID="FnEditar" runat="server" />

    <%--Modal Reportes--%>
    <div class="modal fade" id="modal_reportes" tabindex="-1" role="dialog" aria-labelledby="myModalLabel">
        <div class="modal-dialog" role="document" style="width: 480px;">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title negrita" id="modalTitulo"></h4>
                </div>
                <div class="modal-body">

                    <%-- Grupo inline #1--%>
                    <div class="form-inline">
                        <div class="form-group">
                            <label class="text-danger">ID</label>
                            <asp:TextBox ID="idReporte" Style="width: 50px;" disabled="true" runat="server" Text="" CssClass="form-control red"></asp:TextBox>
                        </div>
                        <div class="form-group" style="padding-left: 10px;">
                            <label class="text-danger">CODIGO</label>
                            <asp:TextBox ID="codigoReporte" required="true" Style="width: 160px;" runat="server" Text="" CssClass="form-control" oninput="this.value = this.value.toUpperCase()"></asp:TextBox>
                        </div>
                        <div class="form-group" style="padding-left: 10px;">
                            <label class="text-danger">ESTADO</label>
                            <asp:DropDownList Style="width: 90px;" ID="ddlEstado" runat="server" CssClass="form-control">
                                <asp:ListItem Text="Activo" Value="A" />
                                <asp:ListItem Text="Inactivo" Value="I" />
                            </asp:DropDownList>
                        </div>
                    </div>
                    <br>
                    <div class="form-group">
                        <label>NOMBRE</label>
                        <asp:TextBox ID="nombreReporte" required="true" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>NOMBRE DEL ARCHIVO RPT</label>
                        <asp:TextBox ID="archivoRpt" runat="server" required="true" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>SISTEMA</label>
                        <asp:DropDownList ID="ddlSistema" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>DIRECTORIO</label>
                        <asp:DropDownList ID="ddlDirectorio" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <div class="form-group">
                        <label>SUBDIRECTORIO</label>
                        <asp:DropDownList ID="ddlSubdirectorio" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>

                    <div class="form-group">
                        <label>UBICACION</label>
                        <asp:TextBox ID="ubicacionReporte" disabled="true" runat="server" Text="" CssClass="form-control"></asp:TextBox>
                    </div>
                    <div class="form-group">
                        <label>PROPIETARIO</label>
                        <asp:DropDownList ID="ddlPropietario" runat="server" CssClass="form-control">
                        </asp:DropDownList>
                    </div>
                    <br />
                    <div class="modal-footer">
                        <div class="center">
                            <table>
                                <tr>
                                    <td>
                                        <button type="button" class="btn btn-primary" id="btnModalAccion">Aceptar</button>
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

    <!-- JS Barra Progreso -->
    <script src="v0005/js/barra_progreso.min.js" type="text/javascript"></script>

    <!-- JS Lista de Reportes -->
    <script src="v0005/js/lista_reportes_v2.1.min.js" type="text/javascript"></script>

</asp:Content>
