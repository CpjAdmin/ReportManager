//*** WebAPI
//Local:  http://localhost:8087
//Server-Omega:  http://172.28.42.100:8087

//*** Parametros Globales
var paginaUrl = 'Estados_de_Cuenta_TD.aspx';
var ApiUrl = 'http://OMEGA:8087' + '/api/EstadoCuentaTD';

var tabla, data;
var objTabla = $('#tbl_asociados');
var inicio;
var ruta;
var rutaEnvios;
var animacion;

var MsjTerminarEstados;
var tipoMsj;
var conEmail;
var conTarjeta;
var conContrato;
var borrarDirectorio;
var nombreDirectorio;

var nombreReporte = 'Estados de Cuenta T.D - Lista de Asociados';
var gl_fec_corte = "";
var emailEntrada;
var mensajeAlerta = "";

var tipo;
var clic = false;

var cantidadRegistrosTD = 0;
var totalRegistrosTDProcesados = 0;
var totalLotesTDProcesados = 0;
var cantidadLotesTD = 0;

//*** Archivo 2
var nombreDirectorio;

//*** 1  - Inicio
$(document).ready(function () {

    //*** 0 - Activar PreCarga
    inicializarPreCarga();

    //*** 1 - Fecha de Corte
    getFechaCorte();

    //*** 1.1 - Variables
    nombreDirectorio = formatDate(new Date()) + "_TD";
    login = $('#Login').val().trim();
    rutaEnvios = $('#ddlDisco').val() + "Estados\\" + login + "\\" + formatDate(new Date()) + "_ENVIOS_TD";

    //*** 1.2 - Iniciar DataTable
    iniciarDataTable();

    //*** 1.3 - Eventos Click de Botones
    eventosBotonClic();

});

//*** 2 (1.2) - Iniciar Tabla
function iniciarDataTable() {

    tabla = objTabla.DataTable({
        "pageLength": 500,
        "lengthMenu": [[500, 1000, 2000, -1], ['500 Registros', '1000 Registros', '2000 Registros', 'Todos']],
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'pageLength'
            },
            {
                extend: 'csvHtml5',
                text: "CSV",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                title: function () {
                    return nombreReporte;
                }
            },
            {
                extend: 'excelHtml5',
                text: "EXCEL",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
                },
                title: function () {
                    return nombreReporte;
                },
                sheetName: 'Estados de Cuenta'
            }]
        ,
        language: {
            "url": "bower_components/datatables-buttons/json/spanish.json",
            buttons: {
                pageLength: {
                    _: "MOSTRAR %d REGISTROS",
                    '-1': "TODOS"
                }
            }
        }
        , "fnDrawCallback": function () {
            var tbl = objTabla.DataTable();
            if (tbl.data().length === 0)
                tbl.buttons('.dt-button').disable();
            else
                tbl.buttons('.dt-button').enable();
        },
        "ordering": false,    //*** FALSE = Ordena según el script origen en SQL
        'bDestroy': true,
        "bDeferRender": true,
        "paging": true,
        "scrollY": $(document).height() - 550 + "px",
        "scrollCollapse": true,
        "scroller": true,

        "bSort": false,
        "autoWidth": false,
        "responsive": true,

        "aoColumns": [
            { "sWidth": "50px", "sClass": "textoCentrado" },  //*** Código 
            { "sWidth": "100px", "sClass": "textoCentrado" }, //*** Identificación
            null,                                             //*** Nombre
            null,                                             //*** Centro
            null,                                             //*** Institución
            null,                                             //*** Lugar de Trabajo
            { "sClass": "textoCentrado" },                    //*** Contrato    +
            { "sClass": "textoCentrado" },                    //*** Tarjeta     +
            { "sClass": "textoCentrado" },                    //*** Cuenta IBAN +
            null,                                             //*** Email
            { "bSortable": false, "sWidth": "40px" }          //*** Acciones
        ]
    });
}

//*** 3 (1.3) - Eventos Botones y Busqueda
function eventosBotonClic() {

    // ***  3.1 - Busqueda por Asociado - Agregar
    $("#btnAgregar1").on("click", function (e) {

        //*** Evitar el PostBack
        e.preventDefault();

        //*** Valor a Buscar
        var valorBuscado = $('#txtBuscarAsociado').val();

        if (valorBuscado !== '') {

            //*** Buscar el Valor
            var existe = 0;
            var listaArray;

            if ($("#txtBuscar").val() === "IDENTIFICACIÓN") {
                listaArray = tabla.rows().column(1).data().toArray();
            } else {
                listaArray = tabla.rows().column(0).data().toArray();
            }

            //*** Verificar si Existe
            existe = jQuery.inArray(valorBuscado, listaArray);

            if (existe === -1) {

                //*** 3.1.1 - Buscar Asociado
                EjecutarBusquedaAsociado();

            } else {
                //*** Verificar si hay registros en la Tabla
                if (tabla.data().any()) {

                    Alerta('mensajeAlerta', 'El Asociado ya existe en la Tabla', 'warning', 2000);
                }
            }
        }
    });

    // ***  3.2 - Busqueda por Asociado - Agregar ( ENTER )
    $('#txtBuscarAsociado').on('keydown', function (e) {

        //*** Valor a Buscar
        var valorBuscado = $('#txtBuscarAsociado').val();

        if (valorBuscado !== '') {

            var key = e.which;

            if (key === 13) {

                //*** Buscar el Valor
                var existe = 0;
                var listaArray;

                if ($("#txtBuscar").val() === "IDENTIFICACIÓN") {
                    listaArray = tabla.rows().column(1).data().toArray();
                } else {
                    listaArray = tabla.rows().column(0).data().toArray();
                }

                // Verificar si Existe
                existe = jQuery.inArray(valorBuscado, listaArray);

                if (existe === -1) {

                    //*** 3.2.1 - Buscar Asociado
                    EjecutarBusquedaAsociado();
                } else {
                    //Verificar si hay registros en la Tabla
                    if (tabla.data().any()) {
                        Alerta('mensajeAlerta', 'El Asociado ya existe en la Tabla!', 'warning', 2000);
                    }
                }
            }
        }
    });

    // ***  3.3 - Buqueda por Ubicación y Lugares de Trabajo - Agregar
    $('#btnAgregar2').on('click', function (e) {

        //*** 3.3.1 - Evitar el PostBack
        e.preventDefault();

        //Tomar el tiempo de Ejecución
        //inicio = new Date().getTime();

        //*** Limitar la generación
        /*
        if ($('#ddlCentro').val() === '0') {
            $('#ddlCentro').focus();

        } else if ($('#ddlInstitucion').val() === '0') {
            $('#ddlInstitucion').focus();

        } else if ($('#ddlLugarTrabajo').val() === '0') {
            $('#ddlLugarTrabajo').focus();
        }
        else { }
        */

        //*** 3.3.2 - Iniciar Dialogo de Progreso
        dialogoProgreso.show('Cargando Asociados ' + '<span class="glyphicon glyphicon-hourglass"></span>');

        //*** 3.3.3 - Animación -  cambio de mensaje cada 2 segundos + inicio después del retraso de 1 segundo
        animacion = dialogoProgreso.animate();

        //*** 3.3.4 - Limpiar Tabla
        objTabla.dataTable().fnClearTable();

        //*** 3.3.5 - Ejecutar Busqueda Masiva
        EjecutarBusquedaMasiva();

    });

    // ***  3.4 - Botón Limpiar Tabla - fnClearTable()
    $("#btnLimpiarTabla").on("click", function (e) {

        //*** Evitar el PostBack
        e.preventDefault();
        //*** Limpiar Tabla
        objTabla.dataTable().fnClearTable();

    });

    // ***  3.5 - Botón Eliminar Registro de Tabla - Eliminar ( tabla.row(fila).remove().draw() )
    $(document).on('click', '.btn-delete', function (e) {

        //*** Evitar el PostBack
        e.preventDefault();

        //*** Tabla de Asociados
        var tabla = $('#tbl_asociados').DataTable();

        //*** Primer método: eliminar la fila del datatable ( Obtener el TR : <tr role="row" class="odd"> )
        var fila = $(this).parent().parent()[0];

        //*** Eliminar de la tabla
        tabla.row(fila).remove().draw();

    });

    // ***  3.6 - Botón Enviar por Email de Tabla - Enviar Mensaje por Email
    $(document).on('click', '.btn-enviar', function (e) {

        //*** 3.6.1 - Evitar el PostBack
        e.preventDefault();

        //*** Obtener datos del asociado
        var fila = $(this).parent().parent()[0];

        var datos = objTabla.fnGetData($(fila));

        var envCodigo = parseInt(datos[0]);
        var envIdentificacion = datos[1];
        var envNombre = datos[2];
        var envNumContrato = datos[6];
        var envEmail = datos[9];
        var envInstitucion = datos[4];

        //*** Verificar Contrato
        if (envNumContrato === 0) {

            //*** Asociado no tiene un Número de Contrato Activo.
            var mensaje = 'El asociado <b>' + envIdentificacion + ' - ' + envNombre + '</b> no tiene un Número de Contrato válido. <b>Contrato = ' + envNumContrato.toString() + '</b>';
            var titulo = 'CONTRATO NO ES VÁLIDO';
            mensajeConfirm(mensaje, titulo, 'material', 'red', 'fa-warning');

        } else {

            //*** 3.6.2 - Enviar Email
            mensaje = ' <form action="" class="formName">' +
                ' <div class="form-group">' +
                ' <p> Se enviará el estado de cuenta del asociado seleccionado al correo que aparece registrado en <b> PSBANKER </b>, si lo requiere puede cambiar la dirección de correo.</br>' +
                ' <b class="text-danger">( El cambio de correo NO afecta los datos de PSBANKER )</b>.</p></br>' +
                ' <div style="padding-left: 170px; text-align: left;">' +
                ' <b> Contrato: </b><b class="text-danger">' + envNumContrato + ' </b></br> ' +
                ' <b> Cédula: </b>' + envIdentificacion + '<br>' +
                ' <b> Nombre: </b>' + envNombre + '<br>' +
                ' <b> Institución: </b>' + envInstitucion + '<br>' +
                ' <b> Correo: </b> <input style="width:300px;" type="email" id="emailAsociado" value="' + envEmail + '"/></div></br>' +
                ' </div>' +
                ' </form>';

            titulo = 'Envío de Estado de Cuenta';
            tema = 'material';
            tipo = 'blue';
            icono = 'fa-envelope';
            tituloConfirmacion = 'Enviar';
            mensajeConfirma = 'Está seguro que desea enviar el estado de cuenta del asociado <b>' + envNombre + '</b> al correo ';
            tituloCancel = 'Envío Cancelado';
            mensajeCancel = 'El envío há sido <b> Cancelado</b >.';

            //*** 3.6.3 - Alistar Mensaje por Email #1
            mensajeEnvio(mensaje, titulo, tema, tipo, icono, tituloConfirmacion, mensajeConfirma, tituloCancel, mensajeCancel, envCodigo, envIdentificacion, envNombre, envNumContrato, gl_fec_corte);

        }
    });

    // ***  3.7 - Botón Generación de Estados de Cuenta - Aceptar
    $('#btnModalGenerar').on('click', function (e) {

        //*** Obtiene la tabla completa
        var tabla = $('#tbl_asociados').DataTable();

        //*** Dados de las columnas Cod_cliente, Identificación y Contrato
        var data = tabla.columns([0, 1, 6]).data();

        //*** Inicio de Filtro de Asociados
        var listaContratos = [];

        for (var i = 0; i < data[0].length; i++) {

            //*** Filtra los Contratos en ZERO
            if (data[2][i] !== 0) {

                //*** 3.7.1 - Datos de Cod_cliente, Identificación y Contratos del Array
                listaContratos.push({ "cod_cliente": data[0][i], "identificacion": data[1][i], "num_contrato": data[2][i] });
            }
        }

        //*** Cantidad de Contratos
        cantidadRegistrosTD = listaContratos.length;

        //*** Esconder Modal
        $("#modalUbicacion").modal('hide');

        //*** 3.7.2 - Valida cantidad de contratos validos
        if (cantidadRegistrosTD > 0) {

            //Se asigna el valor de ruta, por si no se realiza un cambio
            ruta = $('#ddlDisco').val() + "Estados\\" + login + "\\" + $('#txtDirectorio').val();

            //*** 3.7.3 - Generar los Estados de Cuenta
            GenerarEstados(listaContratos);

        } else {
            mensajeAlerta = 'No hay registros de Estados de Cuenta con contratos validos para procesar!';
            tipoMsj = 'blue';
            //*** Mensaje Toast ( icon =  warning, error, success, info, question )
            mensajeConfirm(mensajeAlerta, 'ESTADOS DE CUENTA', 'material', tipoMsj, 'fa-envelope');
        }
    });

    // ***  3.8 - Nombre de Directorio - Validación que No permite cambiar o eliminar la "fecha" del nombre del directorio
    $('#txtDirectorio').on('input', function () {
        //*** Directorio
        if (!$('#txtDirectorio').val().includes(nombreDirectorio)) {
            $('#txtDirectorio').val(nombreDirectorio);
        } else {
            ruta = $('#ddlDisco').val() + "Estados\\" + login + "\\" + $('#txtDirectorio').val();
        }
    });

    // ***  3.9 - Último Cod Cliente Generado
    $('#chkUltimoCodCliente').on('ifChanged', function (event) {

        if ($('#chkUltimoCodCliente').is(':checked')) {
            $('#txtUltimoCodCliente').prop('disabled', false);
        } else {
            $('#txtUltimoCodCliente').prop('disabled', true);
        }
    });


}

//*** 4 - (3.1.1 - 3.2.1 ) Buscar Asociado - EjecutarBusquedaAsociado()
function EjecutarBusquedaAsociado() {

    var textoBusqueda = $("#txtBuscarAsociado").val();
    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    //*** Solo Asociados con Email
    if ($('#conEmail').is(':checked')) {
        conEmail = 'S';
    } else {
        conEmail = 'N';
    }

    //*** Solo Asociados con Tarjeta
    if ($('#conTarjeta').is(':checked')) {
        conTarjeta = 'S';
    } else {
        conTarjeta = 'N';
    }

    //*** Solo Asociados con Contrato
    if ($('#conContrato').is(':checked')) {
        conContrato = 'S';
    } else {
        conContrato = 'N';
    }

    //*** Tipo de Busqueda
    if ($("#txtBuscar").val() === "IDENTIFICACIÓN") {
        parametros.identificacion = textoBusqueda;
        parametros.codigo = '0';
    } else {
        parametros.codigo = textoBusqueda;
        parametros.identificacion = '0';
    }

    //*** Parametros Default
    parametros.cod_provincia = '0';
    parametros.cod_canton = '0';
    parametros.cod_distrito = '0';
    parametros.cod_centro = '0';
    parametros.cod_institucion = '0';
    parametros.cod_lugar = '0';
    parametros.con_email = conEmail;

    //*** 4.1 - Llamar a CargarAsociados AJAX
    buscarAsociados(parametros, 1);
}

//*** 5 - (4.1) Buscar Asociados - AJAX CargarAsociados
function buscarAsociados(parametros, tipoConsulta) {

    //*** Tipo de Consulta
    tipo = tipoConsulta;

    //*** Si no se ha precionado Clic en los botones de Busqueda de Asociado
    if (!clic) {

        //*** Inactivar Clic en btnAgregar1
        clic = true;

        var codigo;

        if (parametros.identificacion === '0') {
            codigo = parametros.codigo;
        } else {
            codigo = parametros.identificacion;
        }

        //*** Último código de cliente generado
        if ($('#chkUltimoCodCliente').is(':checked') && $('#txtUltimoCodCliente').val() !== '') {
            parametros.ult_cod_cliente_gen = parseInt($('#txtUltimoCodCliente').val());
        } else {
            parametros.ult_cod_cliente_gen = 0;
        }

        //*** Tipo de Consulta
        parametros.tipo_consulta = 'AHORRO-VISTA';

        //*** 5.1 - Convierte a cadena JSON
        var DatosCA = JSON.stringify(parametros);

        //*** 5.2 - Agregar Filas a la Tabla
        $.ajax({
            type: "POST",
            url: ApiUrl + '/CargarAsociados',
            data: DatosCA,
            contentType: 'application/json; charset=utf-8',
            error: function (xhr, ajaxOptions, thrownError) {
                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
                //*** Detener dialogoProgreso ( Solo existe para llamada masiva de Clientes)
                if (animacion !== undefined) {
                    setTimeout(function () {
                        dialogoProgreso.hide();
                        dialogoProgreso.stopAnimate(animacion);
                    }, 500);
                }
            },
            success: function (data) {

                //*** Error en la ejecución de la consulta
                if (data.Exito === 0) {
                    //*** Alerta
                    Alerta('mensajeAlerta', data.Mensaje, 'danger', 3000);
                }
                else {

                    if (data.Data.length === 0) {

                        if (tipo === 1) {

                            var conEmailMarcado = '';
                            var conFiltroContrato = '';
                            var conFiltroTarjeta = '';

                            //*** Solo Asociados con Email
                            if ($('#conEmail').is(':checked')) {
                                conEmailMarcado = ' [Con Email]';
                            }
                            //*** Solo Asociados con Tarjeta
                            if ($('#conTarjeta').is(':checked')) {
                                conFiltroTarjeta = ' [Con Tarjeta]';
                            }
                            //*** Solo Asociados con Contrato
                            if ($('#conContrato').is(':checked')) {
                                conFiltroContrato = ' [Con Contrato]';
                            }

                            //*** Mensaje Toast 
                            var tipoToast = 'info';
                            var mensajeToast = 'No se encontró el asociado: ' + codigo + ', filtros activos: ' + conEmailMarcado + conFiltroTarjeta + conFiltroContrato;
                            mensajeToast2(tipoToast, '<h4>' + mensajeToast + '</h4>', 'top-end', 5000);

                        } else {

                            //*** Mensaje Toast 
                            var tipoToast = 'info';
                            var mensajeToast = 'No se encontraron asociados que concuerden con los filtros ingresados';
                            mensajeToast2(tipoToast, '<h4>' + mensajeToast + '</h4>', 'top-end', 5000);
                        }

                    } else {
                        //*** 5.3 - Agregar Filas a la Tabla
                        agregarFilasDTAsociados(data.Data);
                    }
                }
            }
        }).done(function () {

            //*** Detener dialogoProgreso ( Solo existe para llamada masiva de Clientes)
            if (animacion !== undefined) {
                setTimeout(function () {
                    dialogoProgreso.hide();
                    dialogoProgreso.stopAnimate(animacion);
                }, 500);
            }

            //*** Activar Clic en btnAgregar1
            clic = false;
        });
    }
}

//*** 6 - (5.3) - Agregar Filas a la Tabla
function agregarFilasDTAsociados(data) {

    //*** Limpiar Tabla despues de cualquier cambio a los Usuarios 
    //objTabla.dataTable().fnClearTable();

    var filas = [];
    var dataFiltroTarjeta = [];
    var dataFiltroContrato = [];
    var dataFiltrada = [];
 

    //*** Filtrar Asociados con Tarjeta
    if (conTarjeta === 'S') {

        dataFiltroTarjeta = jQuery.grep(data, function (n, i) {
            return (n.tarjeta !== 'NO TIENE');
        }, false);

    } else {
        dataFiltroTarjeta = data;
    }

    //*** Filtrar Asociados con Contrato
    if (conContrato === 'S') {

        dataFiltroContrato = jQuery.grep(dataFiltroTarjeta, function (n, i) {
            return (n.num_contrato !== 0);
        }, false);

    } else {
        dataFiltroContrato = dataFiltroTarjeta;
    }

    //*** Datos Filtrados
    dataFiltrada = dataFiltroContrato;

    if (dataFiltrada.length === 0) {

        //*** Mensaje Toast 
        var tipoToast = 'info';
        var mensajeToast = 'No se encontraron asociados que concuerden con los filtros ingresados';
        mensajeToast2(tipoToast, '<h4>' + mensajeToast + '</h4>', 'top-end', 5000);
    } else {

        //*** Asignar valores de dataFiltrada
        for (var i = 0; i < dataFiltrada.length; i++) {

            filas.push([dataFiltrada[i].codigo
                , dataFiltrada[i].identificacion
                , dataFiltrada[i].nombre
                , dataFiltrada[i].centro
                , dataFiltrada[i].institucion
                , dataFiltrada[i].lugar_trabajo
                , dataFiltrada[i].num_contrato
                , dataFiltrada[i].tarjeta
                , dataFiltrada[i].cuenta_iban
                , dataFiltrada[i].email
                , '<button type="button" id="' + dataFiltrada[i].identificacion + '-' + dataFiltrada[i].num_contrato + '" value="' + dataFiltrada[i].email + '" style="margin: 2px;" title="Enviar Email" class="btn btn-primary btn-enviar btn_datatable btn-xs"><i class="fa fa-envelope-o" aria-hidden="true"></i></button>' +
            '<button type="button" value="Eliminar" style="margin: 2px;" title="Eliminar" class="btn btn-danger btn-delete btn_datatable btn-xs"><i class="fa fa-minus" aria-hidden="true"></i></button>']);
        }

        objTabla.dataTable().fnAddData(filas);

        //*** Dibujar la Tabla
        //tabla.draw();

        //*** Título de la Tabla - Fecha de Corte
        //if (!$('#infoTitulo').length) {
        //    $("<button id='infoTitulo' class='dataTables_info dt-button buttons-html5' style='border: darkred solid 1px !important;' type='button'>FECHA DE CORTE: <b style='font-size: large;'>" + gl_fec_corte + "</h4></button>").appendTo("#tbl_asociados_wrapper .dt-buttons");
        //}

        //*** Tiempo Total de Ejecución
        //$("#txtDuracion").text("Duración : " + (new Date().getTime() - inicio) / 1000 + " Segundos");
    }
}

//*** 7 - (3.6.3) - Alistar Mensaje por Email #1
function mensajeEnvio(mensaje, titulo, theme, type, icon, btnConfirmacion, msjConfirmacion, tituloCancel, mensajeCancel, envCodigo, envIdentificacion, envNombre, envNumContrato, envFechaCorte) {

    var columnClass;

    if (mensaje.length > 500) {
        columnClass = 'large';
    } else {
        columnClass = 'medium';
    }

    btnConfirmacion = btnConfirmacion === '' ? 'Aceptar' : btnConfirmacion;
    msjConfirmacion = msjConfirmacion === '' ? 'Está seguro de realizar esta acción ?' : msjConfirmacion;

    var tipo = type === '' ? 'green' : type;
    var icono = icon === '' ? 'fa-check-circle' : icon;
    var tema = theme === '' ? 'material' : theme;

    //*** 7.1 - Mensaje Tipo Confirm
    $.confirm({
        theme: tema,
        title: titulo,
        content: mensaje,
        columnClass: columnClass,
        icon: 'fa ' + icono,
        type: tipo, //*** red,green,blue
        draggable: true,
        dragWindowGap: 0,
        animation: 'scale',
        closeAnimation: 'zoom',
        animationBounce: 2.5,
        escapeKey: 'cerrar',
        backgroundDismiss: false,

        //*** 7.2 - Botones del Form principal
        buttons: {
            confirm: {
                text: btnConfirmacion,
                btnClass: 'btn-primary',
                action: function () {

                    emailEntrada = $('#emailAsociado').val();

                    if (!emailEntrada) {
                        $('#emailAsociado').focus();
                        return false;
                    } else {
                        if (!validarEmail(emailEntrada)) {

                            mensajeAlerta = 'Ingrese una dirección de correo valida!';
                            titulo = 'Email Incorrecto';

                            mensajeConfirm(mensajeAlerta, titulo, 'material', 'red', 'fa-warning');
                            return false;

                        } else {

                            $.confirm({
                                theme: tema,
                                columnClass: 'medium',
                                type: tipo,
                                title: 'Confirmación de Envío',
                                content: msjConfirmacion + '<b class="text-danger">' + emailEntrada + '</b>.',
                                icon: 'fa fa-question-circle',
                                animation: 'scale',
                                closeAnimation: 'zoom',
                                buttons: {
                                    confirm: {
                                        text: 'Confirmar',
                                        btnClass: 'btn-primary',
                                        action: function () {

                                            //*** 7.3 - Enviar Estado de Cuenta
                                            EnviarEstadoCuenta(envCodigo, envIdentificacion, envNombre, envNumContrato, emailEntrada, envFechaCorte, rutaEnvios);
                                        }
                                    },
                                    cancel: {
                                        text: 'CANCELAR',
                                        btnClass: 'btn-red',
                                        action: function () {
                                            mensajeConfirm(mensajeCancel, tituloCancel, tema, 'blue', 'fa-warning');
                                        }
                                    }
                                }
                            });
                        }
                    }
                }
            },
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        }
    });
}

//*** 8 - (7.3) - Enviar Mensaje por Email #2 - AJAX EnviarEstadoCuenta
function EnviarEstadoCuenta(cod_cliente, identificacion, nombre, num_contrato, email, fec_corte, directorio) {


    //*** Variables 
    var mensajeExito = 'Estado de cuenta <b>Enviado</b> al correo: ';
    var mensajeTitulo = 'ENVIO DE ESTADO DE CUENTA';

    var parametros = new Object();

    parametros.tipo_consulta = 'AHORRO-VISTA';
    parametros.cod_cliente = cod_cliente;
    parametros.identificacion = identificacion;
    parametros.nombre = nombre;
    parametros.num_contrato = num_contrato;
    parametros.email = email;
    parametros.fec_corte = fec_corte;
    parametros.ruta_genera = directorio;
    parametros.id_usuario = cod_usuario;

    data = JSON.stringify(parametros);

    //*** 8.1 - Ejecución AJAX
    $.ajax({
        type: 'POST',
        url: ApiUrl + '/EnviarEstadoCuentaTD',
        data: data,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = "Error: " + xhr.status + "<br>" + xhr.responseJSON.Message, "\n" + thrownError;

            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);
        },
        success: function (data) {

            if (data.Exito === 1) {

                mensajeAlerta = mensajeExito + '<b class="text-danger">' + data.Data.email + '</b>.</br>' +
                    '</br><div style="padding-left: 20px; text-align:left;">' +
                    '<b>Asociado: </b>' + data.Data.identificacion + ' - ' + data.Data.nombre +
                    '</br><b>Contrato:  </b><b class="text-danger">' + data.Data.num_contrato + '</b>' +
                    '</br><b>Ruta:  </b>' + data.Data.ruta_genera +
                    '</br><b>Archivo:  </b>' + data.Data.archivo + '</div>';

                mensajeConfirm(mensajeAlerta, mensajeTitulo, 'material', 'green', 'fa-envelope');
            }
            else {

                mensajeConfirm(data.Mensaje, mensajeTitulo, 'material', 'red', 'fa-warning');
            }
        }
    });
}

// *** 9 - (3.3.5) - Ejecutar Busqueda MasivaBusqueda
function EjecutarBusquedaMasiva() {

    //*** 9.1 - Guardar parámetros
    var parametros = new Object();

    //*** 9.2 - Solo Asociados con Email
    if ($('#conEmail').is(':checked')) {
        conEmail = 'S';
    } else {
        conEmail = 'N';
    }

    //*** 9.2 - Solo Asociados con Tarjeta
    if ($('#conTarjeta').is(':checked')) {
        conTarjeta = 'S';
    } else {
        conTarjeta = 'N';
    }

    //*** 9.2 - Solo Asociados con Contrato
    if ($('#conContrato').is(':checked')) {
        conContrato = 'S';
    } else {
        conContrato = 'N';
    }

    //*** 9.3 - Parametros Default
    parametros.cod_provincia = $("#ddlProvincia").val();
    parametros.cod_canton = $("#ddlCanton").val();
    parametros.cod_distrito = $("#ddlDistrito").val();
    parametros.cod_centro = $("#ddlCentro").val();
    parametros.cod_institucion = $("#ddlInstitucion").val();
    parametros.cod_lugar = $("#ddlLugarTrabajo").val();
    parametros.con_email = conEmail;

    //*** 9.4 - Parametros Default
    parametros.codigo = '0';
    parametros.identificacion = '0';

    //*** 9.5 - Llamar a CargarAsociados MASIVO AJAX
    buscarAsociados(parametros, 2);
}

// *** 10  - (3.7.3) - Inicio - Generación de Estados #1
function GenerarEstados(listaContratos) {

    //*** 10.1 - Formato
    var formato = $("#txtFormato").val();

    //*** 10.2 - Borrar Archivos del Directorio
    if ($('#borrarDirectorio').is(':checked')) {
        borrarDirectorio = 'S';
    } else {
        borrarDirectorio = 'N';
    }

    //*** 10.3 - Guardar parámetros 
    var parametrosGenerar = new Object();

    parametrosGenerar.fec_corte = gl_fec_corte;
    parametrosGenerar.ruta = ruta;
    parametrosGenerar.formato = formato;
    parametrosGenerar.borrar_directorio = borrarDirectorio;
    parametrosGenerar.id_usuario = cod_usuario;

    //*** Parametros adicionales
    var navegador = get_browser_info();
    parametrosGenerar.navegador = navegador.name + " - " + navegador.version;;

    //*** 10.4 - Si la cantidad de contratos es mayor o igual a 100, se separa en grupos
    if (cantidadRegistrosTD >= 100) {

        var size;

        //*** Explorador está limitado a 6 o N conexiones simultaneas, se permiten 1 o 2 grupos de 6 lotes
        if (cantidadRegistrosTD >= 3000) {
            size = 11;
        } else {
            size = 5;
        }

        //*** Tamaño de los grupos de Array
        var n_partes = parseInt(cantidadRegistrosTD / size);

        //*** Mensaje de Progreso
        progresoGeneracion(cantidadRegistrosTD, size + 1);

        //*** Separación de Array = listaContratos
        var lista_contratos_split = separarArray(listaContratos, n_partes);

        //*** Total de Lotes
        cantidadLotesTD = lista_contratos_split.length;

        //*** Ciclo de Ejecución de Estados
        for (var i = 0; i < cantidadLotesTD; i++) {

            //*** Array Hijo resultado de la separación
            var arrayHijo = lista_contratos_split[i];

            //*** Se asigna el Array Hijo
            parametrosGenerar.lista_contratos = arrayHijo;
            parametrosGenerar.num_lote = i + 1;

            //*** 10.3 - Llamar a CargarAsociados AJAX
            EjecutarGeneracionEstados(parametrosGenerar);
        }

    } else {

        //*** Total de Lotes
        cantidadLotesTD = 1;

        //*** Mensaje de Progreso
        progresoGeneracion(cantidadRegistrosTD, 1);

        //*** Se asigna solo 1 vez
        parametrosGenerar.lista_contratos = listaContratos;
        parametrosGenerar.num_lote = 1;

        //*** 10.3 - Llamar a CargarAsociados AJAX
        EjecutarGeneracionEstados(parametrosGenerar);
    }
}

// *** 11 - (10.3) -  Fin - Generación de Estados #2 - AJAX GenerarEstados
function EjecutarGeneracionEstados(parametros) {

    //var ajaxTime = new Date().getTime();
    //*** 11.1 - Convierte a cadena JSON
    var parametrosGenerar = JSON.stringify(parametros);

    //*** 11.2 - Ejecución AJAX
    $.ajax({
        type: "POST",
        url: ApiUrl + '/GenerarEstadoCuentaTD',
        data: parametrosGenerar,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = "Error: " + xhr.status + "<br>" + xhr.responseJSON.ExceptionMessage + "\n" + xhr.responseText, "\n"  + thrownError;

            //*** Establecer en 100% y Cerrar
            dialogoProgreso.progress(99);
            dialogoProgreso.hide();

            mensajeError(MensajeAlerta, "ESTADO DE CUENTA MASIVO");

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 10000);
        },
        success: function (data) {

            //*** Resultado del Lote
            if (data.Exito === 1) {

                //*** Total de registros procesados y número de Lote
                totalRegistrosTDProcesados += data.Data;
                totalLotesTDProcesados += 1;

                var porcentaje = calculoPorcentaje(cantidadRegistrosTD, totalRegistrosTDProcesados);

                //*** Actualización de dialogoProgreso
                var mensajeP = 'Registros procesados : ( ' + totalRegistrosTDProcesados.toString() + ' de ' + cantidadRegistrosTD.toString() + ' ) Progreso: ' + porcentaje.toString() + '%';
                mensajeP = mensajeP + '</br></br>' + 'LOTE #' + totalLotesTDProcesados.toString() + ' ' + data.Mensaje.toString();
                dialogoProgreso.mensaje(mensajeP);
                dialogoProgreso.progress(porcentaje);

                //*** Mensaje
                mensajeAlertaToast = 'LOTE #' + totalLotesTDProcesados.toString() + ' FINALIZADO. REGISTROS: ' + data.Data;
                tipoIcono = 'info';
            }
            else {

                //*** Mensaje
                mensajeAlertaToast = 'LOTE #' + totalLotesTDProcesados.toString() + ' FINALIZADO CON ERRORES. REGISTROS: ' + data.Data + ' , ERROR: ' + data.Mensaje.toString();
                mensajeAlerta
                tipoIcono = 'error';
            }
            //*** Mensaje de reporte de Errores acumulado
            mensajeAlerta += mensajeAlertaToast + '</br>';
            //*** Mensaje Toast ( icon =  warning, error, success, info, question )
            mensajeToast(tipoIcono, mensajeAlertaToast);

            //*** Finalizó el procesamiento de los lotes cantidadLotesTD === totalLotesTDProcesados
            if (cantidadLotesTD === totalLotesTDProcesados) {

                //*** Alerta Final
                if (cantidadRegistrosTD === totalRegistrosTDProcesados) {

                    //*** Mensaje de finalización
                    mensajeAlerta = 'Proceso finalizado con exito. Registros procesados: ' + totalRegistrosTDProcesados + ' de ' + cantidadRegistrosTD;
                    tipoMsj = 'green';

                } else {
                    //*** Mensaje de finalización
                    mensajeAlerta = 'Proceso finalizado con errores. Registros procesados: ' + totalRegistrosTDProcesados + ' de ' + cantidadRegistrosTD + '</br></br>' + mensajeAlerta;
                    tipoMsj = 'red';
                }

                setTimeout(function () {

                    //*** Finalizar dialogoProgreso
                    dialogoProgreso.hide();

                    //*** Mensaje Toast ( icon =  warning, error, success, info, question )
                    mensajeToast(tipoIcono, mensajeAlerta);
                    mensajeConfirm(mensajeAlerta, 'Estados de Cuenta', 'material', tipoMsj, 'fa-envelope');

                    //*** Resetear Hilo de mensajes de Alerta
                    mensajeAlerta = "";

                    //*** Reinicio de variables
                    cantidadRegistrosTD = 0;
                    totalRegistrosTDProcesados = 0;
                    totalLotesTDProcesados = 0;
                    cantidadLotesTD = 0;

                }, 5000); 
            }
        }
    });
}

// *** 12 - Obtener Fecha de Corte Ahorro Vista - alertaSweetAlert2(type, title, text, timer) 
function getFechaCorte() {

    //*** alertaSweetAlert2(type, title, text, timer) 
    Swal.fire({
        icon: 'info',
        title: 'FECHA DE CORTE',
        html: 'Elija la fecha de corte del estado de cuenta Ahorro Vista, puede ser Histórico o Actual.</br></br>' +
            '<div class= "input-group date"> <div class="input-group-addon"><i class="fa fa-calendar"></i></div>' +
            '<input type="text" class="form-control pull-right datepicker textoCentrado" id="datepicker">' +
            '</div>',
        timer: 0,
        footer: '<a href>Fecha de Corte para el reporte de Ahorro Vista.</a>',
        confirmButtonText: '<i class="fa fa-thumbs-up"></i> GUARDAR',
        focusConfirm: true,
        showCancelButton: false,
        showCloseButton: false,
        allowOutsideClick: false,
        allowEscapeKey: false,
        cancelButtonColor: '#d33',
        cancelButtonText: '<i class="fa fa-thumbs-down"></i> CANCELAR',
        backdrop: true,
        toast: false,
        timerProgressBar: false,
        onOpen: function () {
            $('#datepicker').datepicker({
                format: "mm-yyyy",
                showButtonPanel: true,
                viewMode: "months",
                minViewMode: "months",
                orientation: "bottom",
                autoclose: true
            });
        },
        preConfirm: function (result) {

            if ($('#datepicker').val() === '') {
                $('#datepicker').focus();
                return false;
            }
        }
    }).then(function (result) {
        if (result.value) {
            if ($('#datepicker').val() !== '') {

                //*** Conversión de Fecha de Corte - gl_fec_corte
                var fechaDatePicker = $('#datepicker').datepicker('getDate');
                var fecha_corte = new Date(fechaDatePicker.getFullYear(), fechaDatePicker.getMonth() + 1, 0);
                gl_fec_corte = $.datepicker.formatDate("dd/mm/yy", fecha_corte);

                Swal.fire(
                    'GUARDADO CON EXITO',
                    '<h5>Fecha de Corte de Ahorro Vista </br><b class="text-danger">' + gl_fec_corte + '</b></h5>',
                    'success'
                )

                //*** Título de la Tabla - Fecha de Corte
                $('#infoTitulo').remove();
                if (!$('#infoTitulo').length) {
                    $("<button id='infoTitulo' class='dataTables_info dt-button buttons-html5' style='border: darkred solid 1px !important;' type='button'>FECHA DE CORTE: <b style='font-size: large;'>" + gl_fec_corte + "</h4></button>").appendTo("#tbl_asociados_wrapper .dt-buttons");
                    // ***  3.10 - Obtener Fecha de Corte
                    $('#infoTitulo').on('click', function (e) {
                        getFechaCorte();
                    });
                }

            }
        }
    });
}

// *** 12.1 - Proceso de Generación
function progresoGeneracion(totalRegistros, n_lotes) {

    //*** Mensaje Inicial
    var msjLotes = n_lotes > 1 ? ' lotes' : ' lote';
    var msjRegistros = totalRegistros > 1 ? ' registros' : ' registro';
    var mensaje = 'Procesando <b>' + totalRegistros + '</b> ' + msjRegistros + ' en <b>' + n_lotes + '</b>' + msjLotes + ' <span class="glyphicon glyphicon-hourglass"></span>';

    //*** Iniciar Dialogo de Progreso ( Estados de Cuenta )
    dialogoProgreso.show(mensaje, { dialogSize: 'm', progressType: 'info' });

    //*** Progreso 1%
    dialogoProgreso.progress(1);
}


// *** 13 - Pre-Carga - inicializarPreCarga()
function inicializarPreCarga() {

    //*** Iniciar ICheck
    $('input').iCheck({
        checkboxClass: 'icheckbox_futurico',
        radioClass: 'iradio_futurico',
        increaseArea: '25%'            // optional
    });

    //*** Nombre de Directorio = Usuario
    nombreDirectorio = formatDate(new Date());

    //*** Tipo de Busqueda
    $("#txtBuscar").val("IDENTIFICACIÓN");

    //*** Eventos ICheck
    // ifChecked
    $('#ckUbicacion').on('ifChecked', function (event) {

        if ($('#ckUbicacion').is(':checked')) {
            $("#txtDirectorio").removeAttr('disabled');
        }
    });
    // ifUnchecked
    $('#ckUbicacion').on('ifUnchecked', function (event) {

        $("#txtDirectorio").val();
        $("#txtDirectorio").attr("disabled", "disabled");
        //Disparar el evento de cambio para que cambie la ruta
        $("#txtDirectorio").trigger("change");
    });

    //*** Etiqueta lbRuta
    $("#ddlDisco, #txtDirectorio ").on("change paste keyup", function () {
        $("#lbRuta").text(" " + $("#ddlDisco").val() + "Estados\\" + login + "\\" + $("#txtDirectorio").val());
    });

    //*** Modal Ubicación
    $('#modalUbicacion').on('show.bs.modal', function (e) {
        // Directorio
        $("#txtDirectorio").val(nombreDirectorio);

        // Label Ruta 
        $("#lbRuta").text(" " + $("#ddlDisco").val() + "Estados\\" + login + "\\" + $("#txtDirectorio").val());
        $("#txtFormato").val("PDF");
    });

    //*** Botón Formato
    $("#btnFormato").on("click", 'li a', function () {
        $("#txtFormato").val($(this).text());
    });

    //*** Modal Generar
    $("#btnModalGenerar").on("click", function () {
        $("#txtFormato").removeAttr('disabled');
    });

    //*** Botón Buscar
    $("#btnBuscar").on("click", 'li a', function () {
        $("#txtBuscar").val($(this).text());
    });
}

// *** 14 - Pre-Carga - DropDownList
// *** #1 - DropDownList(Provincia, Canton, Distrito)
// *** #2 - DropDownList(Centro, Institución, LugarTrabajo)
// ***
// *** 14.1 - Cargar Cantones
function CargarCantones() {

    $("#ddlCanton").attr("disabled", "disabled");
    $('#ddlDistrito').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    $("#ddlDistrito").attr("disabled", "disabled");

    if ($('#ddlProvincia').val() === "0") {
        $('#ddlCanton').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
        $('#ddlDistrito').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    }
    else {

        $('#ddlCanton').empty().append('<option selected="selected" value="0">Cargando...</option>');

        // Guardar parámetros y usar Libreria de JSON
        var parametros = new Object();
        parametros.provinciaId = $("#ddlProvincia").val();

        // Convierte a cadena JSON
        parametros = JSON.stringify(parametros);

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarCantones',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                //*** Cargar Cantones
                LlenarControl(data.d, $("#ddlCanton"));
            },
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// *** 14.2 - Carga Distritos
function CargarDistritos() {

    $("#ddlDistrito").attr("disabled", "disabled");
    if ($('#ddlCanton').val() === "0") {
        $('#ddlDistrito').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    }
    else {

        $('#ddlDistrito').empty().append('<option selected="selected" value="0">Cargando...</option>');

        // Guardar parámetros y usar Libreria de JSON
        var parametros = new Object();
        parametros.provinciaId = $("#ddlProvincia").val();
        parametros.cantonId = $("#ddlCanton").val();

        // Convierte a cadena JSON
        parametros = JSON.stringify(parametros);

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarDistritos',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                //*** Cargar Distritos
                LlenarControl(data.d, $("#ddlDistrito"));
            },
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// *** 14.3 - Cargar Instituciones
function CargarInstituciones() {

    $("#ddlInstitucion").attr("disabled", "disabled");
    $('#ddlLugarTrabajo').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    $("#ddlLugarTrabajo").attr("disabled", "disabled");

    if ($('#ddlCentro').val() === "0") {

        $('#ddlInstitucion').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
        $('#ddlLugarTrabajo').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    }
    else {

        $('#ddlInstitucion').empty().append('<option selected="selected" value="0">Cargando...</option>');

        // Guardar parámetros y usar Libreria de JSON
        var parametros = new Object();
        parametros.centroId = $("#ddlCentro").val();

        //Convierte a cadena JSON
        parametros = JSON.stringify(parametros);

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarInstituciones',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                //*** Cargar Instituciones
                LlenarControl(data.d, $("#ddlInstitucion"));
            },
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// *** 14.4 - Carga LugaresTrabajo
function CargarLugaresTrabajo() {

    $("#ddlLugarTrabajo").attr("disabled", "disabled");

    if ($('#ddlInstitucion').val() === "0") {

        $('#ddlLugarTrabajo').empty().append('<option selected="selected" value="0">Seleccione un valor</option>');
    }
    else {

        $('#ddlLugarTrabajo').empty().append('<option selected="selected" value="0">Cargando...</option>');

        // Guardar parámetros y usar Libreria de JSON
        var parametros = new Object();
        parametros.centroId = $("#ddlCentro").val();
        parametros.institucionId = $("#ddlInstitucion").val();

        //Convierte a cadena JSON
        parametros = JSON.stringify(parametros);

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarLugaresTrabajo',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (data) {

                //*** Cargar Lugares de Trabajo
                LlenarControl(data.d, $("#ddlLugarTrabajo"));
            },
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// *** 14.5 - Llenar LlenarControl
function LlenarControl(list, control) {

    if (list.length > 0) {

        control.removeAttr("disabled");
        control.empty().append('<option selected="selected" value="0">Seleccione un valor</option>');

        // Canton
        if (control.attr('id') === "ddlCanton") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_canton']).html(this['nombre']));
            });
        }
        // Distrito
        else if (control.attr('id') === "ddlDistrito") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_distrito']).html(this['nombre']));
            });
        }
        // Institucion
        else if (control.attr('id') === "ddlInstitucion") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_institucion']).html(this['nombre']));
            });
        }
        // LugarTrabajo
        else {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_lugar_trabajo']).html(this['nombre']));
            });
        }
    }
    else {
        control.empty().append('<option selected="selected" value="0">No existen registros!<option>');
    }
}


