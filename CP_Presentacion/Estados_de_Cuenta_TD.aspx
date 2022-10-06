<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.Master" AutoEventWireup="true" CodeBehind="Estados_de_Cuenta_TD.aspx.cs" Inherits="CP_Presentacion.Estados_de_Cuenta_TD" ClientIDMode="Static" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">

    <!-- icheck -->
    <link href="bower_components/icheck/skins/futurico/futurico.css" rel="stylesheet">
    <!-- Botones DataTable -->
    <link rel="stylesheet" href="bower_components/datatables-buttons/v0005/buttons.dataTables.min.css">
    <!-- DatePicker -->
    <link rel="stylesheet" href="bower_components/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <section class="content-header">
        <h1 style="text-align: center; width: 500px">ESTADOS DE CUENTA - AHORRO VISTA</h1>
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
        <div class="row" style="margin-left: 1px; margin-right: 1px;">
            <%--Busqueda por Zona Geográfica<--%>
            <div class="col-md-6">
                <div class="box box-solid box-primary" style="height: 190px;">
                    <div class="box-header with-border center">
                        <p style="font-size: 12px; font-weight: bold;" class="box-title">BUSQUEDA POR UBICACIÓN Y LUGARES DE TRABAJO</p>
                    </div>
                    <div class="container">
                    </div>
                    <div class="box-body">
                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>PROVINCIA</label>
                                        <asp:DropDownList ID="ddlProvincia" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                            onchange="CargarCantones();">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>CANTÓN</label>
                                        <asp:DropDownList ID="ddlCanton" runat="server" CssClass="form-control"
                                            onchange="CargarDistritos();">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>DISTRITO</label>
                                        <asp:DropDownList ID="ddlDistrito" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>

                        <div class="col-md-12">
                            <div class="row">
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>CENTROS DE TRABAJO</label>
                                        <asp:DropDownList ID="ddlCentro" runat="server" CssClass="form-control" AppendDataBoundItems="true"
                                            onchange="CargarInstituciones();">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>INSTITUCIONES</label>
                                        <asp:DropDownList ID="ddlInstitucion" runat="server" CssClass="form-control"
                                            onchange="CargarLugaresTrabajo();">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                                <div class="col-xs-4">
                                    <div class="form-group">
                                        <label>LUGARES</label>
                                        <asp:DropDownList ID="ddlLugarTrabajo" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Seleccione un valor" Value="0"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 center">
                            <button type="button" id="btnAgregar2" class="btn btn-primary btn-sm center" style="width: 170px;"><span class="fa fa-2x fa-plus-circle"></span>&nbsp;AGREGAR ASOCIADOS</button>
                        </div>
                    </div>
                </div>
            </div>
            <%--Busqueda por Asociado<--%>
            <div class="col-md-3">
                <div class="box box-solid box-primary" style="height: 190px;">
                    <div class="box-header with-border center">
                        <p style="font-size: 12px; font-weight: bold;" class="box-title">BUSQUEDA POR ASOCIADO</p>
                    </div>
                    <div class="container">
                    </div>

                    <div class="box-body">
                        <div class="form-group">
                            <label class="text-black">TIPO DE BUSQUEDA</label>
                            <div class="input-group">
                                <div class="input-group-btn" id="btnBuscar">
                                    <button class="btn btn-primary dropdown-toggle" aria-expanded="false" type="button" data-toggle="dropdown">
                                        Buscar por&nbsp;&nbsp;<span class="fa fa-caret-down"></span>
                                    </button>
                                    <ul class="dropdown-menu">
                                        <li><a class="text-black">IDENTIFICACIÓN</a></li>
                                        <li class="divider"></li>
                                        <li><a class="text-black">CÓDIGO</a></li>
                                    </ul>
                                </div>
                                <!-- /btn-group -->
                                <input runat="server" id="txtBuscar" disabled class="form-control text-bold text-center" type="text">
                            </div>
                        </div>
                        <div class="form-group">
                            <label class="control-label"><i class="fa fa-search fa-3x">&nbsp;</i>BUSCAR ASOCIADO</label>
                            <div class="input-group">
                                <input id="txtBuscarAsociado" class="form-control" type="number" value="">
                                <span class="input-group-btn">
                                    <button id="btnAgregar1" class="btn btn-primary btn-sm center" type="button"><span class="fa fa-2x fa-plus-circle"></span>&nbsp;AGREGAR</button>
                                </span>
                            </div>
                            <span class="help-block" style="color: darkred; font-weight: bold; font-size: 12px;">Presione ENTER o AGREGAR para Buscar!</span>
                        </div>
                    </div>
                </div>
            </div>
            <%--Opciones de Busqueda<--%>
            <div class="col-md-3">
                <div class="box box-solid box-primary" style="height: 190px;">
                    <div class="box-header with-border center">
                        <p style="font-size: 12px; font-weight: bold;" class="box-title">OPCIONES DEL MÓDULO</p>
                    </div>
                    <div class="container">
                    </div>
                    <div class="box-body">
                        <div class="form-group">
                            <input type="checkbox" checked name="fancy-checkbox-success" id="conEmail" />
                            <div class="btn-group">
                                <label for="conEmail" class="btn btn-warning" style="padding: 3px 6px;">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span></span>
                                </label>
                                <label for="conEmail" class="btn btn-default active" style="padding: 3px 6px;">
                                    Buscar solo asociados con Email&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp<i class="fa fa-envelope"></i>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <input type="checkbox" checked name="fancy-checkbox-success" id="conTarjeta" />
                            <div class="btn-group">
                                <label for="conTarjeta" class="btn btn-primary" style="padding: 3px 6px;">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span></span>
                                </label>
                                <label for="conTarjeta" class="btn btn-default active" style="padding: 3px 6px;">
                                    Buscar solo asociados con Tarjeta&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-address-card"></i>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <input type="checkbox" name="chkfiltroContrato" id="conContrato" />
                            <div class="btn-group">
                                <label for="conContrato" class="btn btn-warning" style="padding: 3px 6px;">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span></span>
                                </label>
                                <label for="conContrato" class="btn btn-default active" style="padding: 3px 6px;">
                                    Buscar solo asociados con Contrato&nbsp;&nbsp;&nbsp;<i class="fa fa-book"></i>
                                </label>
                            </div>
                        </div>
                        <div class="form-group">
                            <input type="checkbox" name="chkBorrarDirectorio" id="borrarDirectorio" />
                            <div class="btn-group">
                                <label for="borrarDirectorio" class="btn btn-danger" style="padding: 3px 6px;">
                                    <span class="glyphicon glyphicon-ok"></span>
                                    <span></span>
                                </label>
                                <label for="borrarDirectorio" class="btn btn-default active" style="padding: 3px 6px;">
                                    Eliminar los archivos del Directorio&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<i class="fa fa-trash"></i>
                                </label>
                            </div>
                        </div>
                        <div class="form-group has-success">
                            <div class="input-group">
                                <span class="input-group-addon" id="AddonUltimoCodCliente">
                                    <input type="checkbox" id="chkUltimoCodCliente">
                                </span>
                                <input type="number" class="form-control" id="txtUltimoCodCliente" style="width: 85%" placeholder="BUSCAR DESDE EL CÓDIGO CLIENTE #" disabled>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <%--Botones del Formulario--%>
        <div class="center">
            <table>
                <tr>
                    <td>
                        <button type="button" id="btnLimpiarTabla" class="btn btn-default btn-sm active" style="width: 120px;"><span class="glyphicon glyphicon-trash"></span>&nbsp;Limpiar Tabla</button>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <button type="button" id="btnAbrirModal" class="btn btn-primary btn-sm" style="width: 130px;" data-toggle="modal" data-target="#modalUbicacion">
                            <span class="glyphicon glyphicon-ok"></span>&nbsp;Generar Estados</button>
                    </td>
                    <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td>
                        <a href="Inicio.aspx" id="btnCancelar" class="btn btn-danger btn-sm" style="width: 120px;">Cancelar</a>
                    </td>
                </tr>
            </table>
        </div>
        <%-- ************************************** DATATABLE - Lista de Asociados--%>
        <div class="row">
            <div class="col-sm-12">
                <div class="box box-primary">
                    <%--<div class="box-header">
                        <h3 class="box-title">Lista de Asociados</h3>
                    </div>--%>
                    <span id="txtDuracion" class="help-block " style="color: darkred; font-weight: bold; font-size: 12px;"></span>
                    <div class="box-body table-responsive">
                        <table id="tbl_asociados" class="table table-bordered table-hover">
                            <thead>
                                <tr>
                                    <th>Código</th>
                                    <th>Identificación</th>
                                    <th>Nombre</th>
                                    <th>Centro</th>
                                    <th>Institución</th>
                                    <th>Lugar de Trabajo</th>
                                    <th>Contrato</th>
                                    <th>Tarjeta</th>
                                    <th>Cuenta IBAN</th>
                                    <th>Email</th>
                                    <th>Acción</th>
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

        <!-- Modal -->
        <div class="modal fade" id="modalUbicacion" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
            <div class="modal-dialog" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h4 class="modal-title" id="exampleModalLabel"><b>Generación de Estados de Cuenta</b>
                        </h4>
                    </div>
                    <div class="modal-body">
                        <p style="color: darkred; font-weight: bold; font-size: 12px;">
                            Seleccione la unidad de disco y el directorio para guardar los Estados de Cuenta. <b>Ejem:</b> <a class="tooltip-test" title="Esta es la predeterminada!"><strong>C:\Estados</strong></a>
                        </p>
                        <div class="form-inline">
                            <div class="form-group">
                                <label for="disco">Disco:</label>
                                <asp:DropDownList ID="ddlDisco" runat="server" CssClass="form-control">
                                </asp:DropDownList>
                            </div>
                            <div class="form-group" style="margin-left: 30px;">
                                <label for="directorio">&nbsp;Directorio:</label>
                                <input runat="server" disabled type="text" class="form-control" id="txtDirectorio">
                            </div>
                            &nbsp;&nbsp;&nbsp;&nbsp;
                            <div class="[ form-group ]">
                                <input type="checkbox" name="fancy-checkbox-warning" id="ckUbicacion" />
                                <div class="[ btn-group ]">
                                    <label for="ckUbicacion" class="[ btn btn-warning ]">
                                        <span class="[ glyphicon glyphicon-ok ]"></span>
                                        <span></span>
                                    </label>
                                    <label for="ckUbicacion" class="[ btn btn-default active ]">
                                        Cambiar <i class="fa fa-pencil-alt fa-fw"></i>
                                    </label>
                                </div>
                            </div>
                        </div>
                        <br />
                        <h6>
                            <span class="glyphicon glyphicon-hdd gi-5x" style="font-size: 30px;"></span><b style="color: darkred;">&nbsp;&nbsp;Ruta Actual: </b>
                            <label id="lbRuta" runat="server" style="font-weight: bold;"></label>
                        </h6>
                        <br />
                        <div class="input-group col-xs-6">
                            <div class="input-group-btn open" id="btnFormato">
                                <button class="btn btn-warning dropdown-toggle" aria-expanded="true" type="button" data-toggle="dropdown">
                                    Formato&nbsp;<span class="fa fa-caret-down"></span>
                                </button>
                                <ul class="dropdown-menu">
                                    <li><a>PDF</a></li>
                                    <li class="divider"></li>
                                    <li><a>EXCEL</a></li>
                                </ul>
                            </div>
                            <!-- /btn-group -->

                            <input runat="server" id="txtFormato" disabled class="form-control text-bold text-center" type="text">
                        </div>

                    </div>
                    <div class="modal-footer">
                        <%--<asp:Button ID="btnModalGenerar"  runat="server" Text="Generar" Style="width: 85px;" CssClass="btn btn-primary" OnClick="btnRegistrar_Click" />--%>
                        <button type="button" id="btnModalGenerar" class="btn btn-primary" style="width: 85px;">Aceptar</button>
                        <button type="button" id="btnModalCancelar" class="btn btn-default" style="width: 85px;" data-dismiss="modal">Cancelar</button>
                    </div>
                </div>
            </div>
        </div>
    </section>
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
    <!-- DatePicker -->
    <script src="bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js"></script>
    <!-- JS Estado de Cuenta TD -->
    <script src="v0005/js/estado_cuenta_td.min.js" type="text/javascript"></script>
    <!-- JS Barra Progreso -->
    <script src="v0005/js/barra_progreso.min.js" type="text/javascript"></script>

</asp:Content>
