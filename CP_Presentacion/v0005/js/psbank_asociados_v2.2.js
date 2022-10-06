//***  Parametros Globales
var paginaUrl = 'Estados_de_Cuenta.aspx';

var tabla, data;
var objTabla = $('#tbl_asociados');
var inicio;
var ruta;
var rutaEnvios;
var animacion;

var MsjTerminarEstados;
var tipoMsj;
var conEmail;
var borrarDirectorio;
var nombreDirectorio;

var nombreReporte = 'Estados de Cuenta - Lista de Asociados';

var emailEntrada;
var mensajeAlerta;
var mensajeExito;
var tituloExito;

var clic = false;

// Iniciar la creación de la Tabla
$(document).ready(function () {
    iniciarDataTable();

    //Nombre de Directorio = Usuario
    nombreDirectorio = formatDate(new Date()) + "_GEN";
    login = $('#Login').val().trim();

    rutaEnvios = $('#ddlDisco').val() + "Estados\\" + login + "\\" + formatDate(new Date()) + "_ENVIOS";
});

//Iniciar Tabla
function iniciarDataTable() {

    tabla = objTabla.DataTable({
        //data: filas,
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
        "scrollY": $(document).height() - 540 + "px", //30vh
        "scrollCollapse": true,
        "scroller": true,

        "bSort": false,
        "autoWidth": true,
        "responsive": true,

        "aoColumns": [
            { "sWidth": "80px", "sClass": "textoCentrado" },  //*** Código 
            { "sWidth": "110px", "sClass": "textoCentrado" }, //*** Identificación
            null,
            null,
            null,
            null,
            { "sWidth": "80px" }, //*** Provincia
            null,
            null,
            null,
            { "bSortable": false, "sWidth": "40px" } // Acciones
        ]
    });
}

// Agregar Filas a la Tabla
function agregarFilasDTAsociados(data) {

    // Limpiar Tabla despues de cualquier cambio a los Usuarios
    //objTabla.dataTable().fnClearTable();

    var filas = [];

    for (var i = 0; i < data.length; i++) {

        filas.push([data[i].codigo, data[i].identificacion, data[i].nombre, data[i].centro, data[i].institucion, data[i].lugar_trabajo, data[i].provincia, data[i].canton, data[i].distrito, data[i].email
            , '<button type="button" id="' + data[i].identificacion + '" value="' + data[i].email + '" style="margin: 2px;" title="Enviar Email" class="btn btn-primary btn-enviar btn_datatable btn-xs"><i class="fa fa-envelope-o" aria-hidden="true"></i></button>' +
        '<button type="button" value="Eliminar" style="margin: 2px;" title="Eliminar" class="btn btn-danger btn-delete btn_datatable btn-xs"><i class="fa fa-minus" aria-hidden="true"></i></button>']);
    }

    objTabla.dataTable().fnAddData(filas);

    //*** Dibujar la Tabla
    //tabla.draw();

    //*** Título de la Tabla
    //if (!$('#infoTitulo').length) {
    //    $("<div id='infoTitulo' class='dataTables_info' role='status' aria-live='polite'>Lista de Asociados ( Generación de Estados de Cuenta )</div>").appendTo("#tbl_asociados_wrapper .col-sm-6:first");
    //}

    //*** Tiempo Total de Ejecución
    //$("#txtDuracion").text("Duración : " + (new Date().getTime() - inicio) / 1000 + " Segundos");
}

//*** Eventos Botones y Busqueda
$(function () {

    // ***  Evento Agregar 1.1 - Click
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

            // Verificar si Existe
            existe = jQuery.inArray(valorBuscado, listaArray);

            if (existe === -1) {
                EjecutarBusquedaAsociado();
            } else {
                //Verificar si hay registros en la Tabla
                if (tabla.data().any()) {
                    Alerta('mensajeAlerta', 'El Asociado ya existe en la Tabla!', 'warning', 2000);
                }
            }
        }
    });

    // ***  Evento Agregar 1 - Enter
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

    // ***  Evento Agregar 2 - Busqueda Masiva
    $('#btnAgregar2').on('click', function (e) {

        //Evitar el PostBack
        e.preventDefault();

        //Tomar el tiempo de Ejecución
        //inicio = new Date().getTime();

        //*** Iniciar Dialogo de Progreso
        dialogoProgreso.show('Cargando Asociados ' + '<span class="glyphicon glyphicon-hourglass"></span>');

        //*** Animación -  cambio de mensaje cada 2 segundos + inicio después del retraso de 1 segundo
        animacion = dialogoProgreso.animate();

        if ($('#ddlCentro').val() === '*') {
            $('#ddlCentro').focus();

        } else if ($('#ddlInstitucion').val() === '*') {
            $('#ddlInstitucion').focus();

        } else if ($('#ddlLugarTrabajo').val() === '*') {
            $('#ddlLugarTrabajo').focus();
        }
        else {
            //**** Limpiar Tabla
            objTabla.dataTable().fnClearTable();

            //**** Ejecutar Busqueda Masiva
            EjecutarBusquedaMasiva();
        }
    });

    // *** Busqueda Masiva - EjecutarBusquedaMasiva
    function EjecutarBusquedaMasiva() {
        //*** Guardar parámetros y usar Libreria de JSON
        var parametros = new Object();

        //*** Solo Asociados con Email
        if ($('#conEmail').is(':checked')) {
            conEmail = 'S';
        } else {
            conEmail = 'N';
        }

        //*** Parametros Default
        parametros.cod_provincia = $("#ddlProvincia").val();
        parametros.cod_canton = $("#ddlCanton").val();
        parametros.cod_distrito = $("#ddlDistrito").val();
        parametros.cod_centro = $("#ddlCentro").val();
        parametros.cod_institucion = $("#ddlInstitucion").val();
        parametros.cod_lugar = $("#ddlLugarTrabajo").val();
        parametros.con_email = conEmail;

        //*** Parametros Default
        parametros.codigo = '0';
        parametros.identificacion = '0';

        //*** Llamar a CargarAsociados MASIVO AJAX
        buscarAsociados(parametros, 2);
    }

    // *** No permite cambiar el directorio por un nombre que no incluya el Nombre de Usuario
    $('#txtDirectorio').on('input', function () {
        //*** Directorio
        if (!$('#txtDirectorio').val().includes(nombreDirectorio)) {
            $('#txtDirectorio').val(nombreDirectorio);
        } else {
            ruta = $('#ddlDisco').val() + "Estados\\" + login + "\\" + $('#txtDirectorio').val();
        }
    });

    // ********************************************** Evento Generar Estados - Botón Modal 
    $('#btnModalGenerar').on('click', function (e) {

        //Total de Registros en la Tabla
        var totalRegistros = objTabla.DataTable().rows().count();
        var tipo = "";

        //*** Esconder Modal
        $("#modalUbicacion").modal('hide');

        if (totalRegistros > 0) {

            //Tiempo estimado
            if (totalRegistros < 38) {
                tiempoEstimado = totalRegistros / 1.6;   // 1.6 Estados por segundo
                tipo = "Segundos";
            } else {
                tiempoEstimado = totalRegistros / 38;    // 38 Estados por minuto
                tipo = "Minutos";
            }

            var tiempoEstimadoRedondeado = Math.round(tiempoEstimado * 100) / 100;

            var tiempoEstimadoSegundos;
            //*** Total de Segundos para el TimeOut
            if (tipo === "Minutos") {
                tiempoEstimadoSegundos = Math.round(tiempoEstimadoRedondeado * 60) * 1000; // 1000 para los Segundos
            } else {
                tiempoEstimadoSegundos = Math.round(tiempoEstimadoRedondeado) * 1000;
            }

            var mensaje = 'Procesando ' + totalRegistros + ' registros en ' + '<span class="glyphicon glyphicon-hourglass"></span>' + tiempoEstimadoRedondeado + ' ' + tipo;

            //*** Iniciar Dialogo de Progreso ( Estados de Cuenta )
            dialogoProgreso.show(mensaje, { dialogSize: 'm', progressType: 'info' });

            //*** Progreso 0%
            dialogoProgreso.progress(0);

            //Se asigna el valor de ruta, por si no se realiza un cambio
            ruta = $('#ddlDisco').val() + "Estados\\" + login + "\\" + $('#txtDirectorio').val();
            var msjRutaEnServidor = 'Ir a: \\\\Omega\\Estados\\' + login + '\\' + $('#txtDirectorio').val() + ' ';

            //*** Progreso Continuo
            var arregloProgreso = [{ msj: 'Espere un momento por favor', prog: 1 }, { prog: 2 }, { prog: 4 }, { prog: 6 }, { prog: 8 }, { prog: 10 }
                , { prog: 12 }, { prog: 14 }, { prog: 16 }, { prog: 18 }, { msj: 'Ruta : ' + ruta + '', prog: 20 }
                , { prog: 22 }, { prog: 24 }, { prog: 26 }, { prog: 28 }, { msj: 'Descargando estados', prog: 30 }
                , { prog: 32 }, { prog: 34 }, { prog: 36 }, { prog: 38 }, { msj: 'Ruta : ' + ruta + '', prog: 40 }
                , { prog: 42 }, { prog: 44 }, { prog: 46 }, { prog: 48 }, { prog: 50 }
                , { prog: 52 }, { prog: 54 }, { prog: 56 }, { prog: 58 }, { prog: 60 }
                , { prog: 62 }, { prog: 64 }, { prog: 66 }, { prog: 68 }, { prog: 70 }
                , { prog: 72 }, { prog: 74 }, { prog: 76 }, { prog: 78 }, { prog: 80 }
                , { prog: 82 }, { prog: 84 }, { prog: 86 }, { prog: 88 }, { prog: 90 }
                , { prog: 91 }, { prog: 92 }, { prog: 93 }, { prog: 94 }, { prog: 95 }, { prog: 96 }, { prog: 97 }, { prog: 98 }, { msj: msjRutaEnServidor, prog: 98 }];

            //*** Recorrer el arregloProgreso
            arregloProgreso.forEach(function (e, i) {
                setTimeout(function () {
                    if (e.msj) {
                        var mensajeP = e.msj + '   ' + e.prog.toString() + '%';
                        dialogoProgreso.mensaje(mensajeP);
                    } else {

                        var estimacionTiempo = tiempoEstimadoRedondeado - (tiempoEstimadoRedondeado * (e.prog / 100));
                        var mensajeProgreso = 'Tiempo estimado: <span class="glyphicon glyphicon-hourglass"></span>' + Math.round(estimacionTiempo * 100) / 100 + ' ' + tipo;
                        dialogoProgreso.mensaje(mensajeProgreso + '   ' + e.prog.toString() + '%');
                    }
                    dialogoProgreso.progress(e.prog);
                }, tiempoEstimadoSegundos * (e.prog / 100));
            });

            //Generar los Estados de Cuenta
            GenerarEstados();

        } else {
            MsjTerminarEstados = 'No hay Estados de Cuenta para procesar!';
            tipoMsj = 'warning';

            Alerta('mensajeAlerta', MsjTerminarEstados, tipoMsj, 3000);
        }
    });
});

// Inicio - Generar Estados de Cuenta
function GenerarEstados() {

    var tabla = $('#tbl_asociados').DataTable();

    //Datos de Códigos y Cédulas en un Array
    var listaCodigos = tabla.rows().column(0).data().toArray();
    var listaCedulas = tabla.rows().column(1).data().toArray();

    if (listaCodigos.length > 0) {

        //*** Formato
        var formato = $("#txtFormato").val();

        //*** Borrar Archivos del Directorio
        if ($('#borrarDirectorio').is(':checked')) {
            borrarDirectorio = 'S';
        } else {
            borrarDirectorio = 'N';
        }

        //*** Guardar parámetros y usar Libreria de JSON
        var parametrosGenerar = new Object();

        parametrosGenerar.codigos = listaCodigos;
        parametrosGenerar.cedulas = listaCedulas;
        parametrosGenerar.ruta = ruta;
        parametrosGenerar.formato = formato;
        parametrosGenerar.id = cod_usuario;
        parametrosGenerar.borrarDirectorio = borrarDirectorio;

        //Llamar a CargarAsociados AJAX
        EjecutarGeneracionEstados(parametrosGenerar);

    } else {
        MsjTerminarEstados = 'No hay Estados de Cuenta para procesar!';
        tipoMsj = 'warning';
    }
}

//*********************************************** Fin - Generación de Estados
function EjecutarGeneracionEstados(parametros) {

    //*** Convierte a cadena JSON
    parametrosGenerar = JSON.stringify(parametros);
    var ajaxTime = new Date().getTime();

    $.ajax({
        type: "POST",
        url: paginaUrl + '/GenerarEstados',
        data: parametrosGenerar,
        contentType: 'application/json; charset=utf-8',
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = "Error: " + xhr.status + "<br>" + xhr.responseJSON.Message, "\n" + thrownError;

            //Establecer en 100% y Cerrar
            dialogoProgreso.progress(99);
            dialogoProgreso.hide();

            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);
        },
        success: function (data) {

            var resultado = JSON.parse(data.d);
            var mensaje;
            var mensajeDetalle;

            $.each(resultado, function (i, item) {
                mensaje = item[0];
                mensajeDetalle = item[1];
            });

            //*** Guardar el Mensaje a mostrar
            if (mensaje === "0") {
                MsjTerminarEstados = mensajeDetalle;
                tipoMsj = 'success';
            }
            else if (mensaje === "1") {
                MsjTerminarEstados = mensajeDetalle;
                tipoMsj = 'warning';
            }
            else if (mensaje === "-1") {
                MsjTerminarEstados = mensajeDetalle;
                tipoMsj = 'danger';
            }
        }
    }).done(function () {
        //Establecer en 100% y Cerrar
        dialogoProgreso.progress(99);
        dialogoProgreso.hide();

    }).done(function () {
        MensajeAlerta = MsjTerminarEstados;
        mensajeConfirm(MensajeAlerta, 'Estados de Cuenta', 'material', 'green', 'fa-envelope');
    });
}

// Busqueda Asociado - EjecutarBusquedaAsociado 
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

    //*** Tipo de Busqueda
    if ($("#txtBuscar").val() === "IDENTIFICACIÓN") {
        parametros.identificacion = textoBusqueda;
        parametros.codigo = '0';
    } else {
        parametros.codigo = textoBusqueda;
        parametros.identificacion = '0';
    }

    // Parametros Default
    parametros.cod_provincia = '0';
    parametros.cod_canton = '0';
    parametros.cod_distrito = '0';
    parametros.cod_centro = '0';
    parametros.cod_institucion = '0';
    parametros.cod_lugar = '0';
    parametros.con_email = conEmail;

    //Llamar a CargarAsociados AJAX
    buscarAsociados(parametros, 1);
}

//*** Botón Limpiar Tabla
$(function () {

    $("#btnLimpiarTabla").on("click", function (e) {
        //Evitar el PostBack
        e.preventDefault();

        objTabla.dataTable().fnClearTable();

    });

    // ***  3.9 - Último Cod Cliente Generado
    $('#chkUltimoCodCliente').on('ifChanged', function (event) {

        if ($('#chkUltimoCodCliente').is(':checked')) {
            $('#txtUltimoCodCliente').prop('disabled', false);
        } else {
            $('#txtUltimoCodCliente').prop('disabled', true);
        }
    });


});

//*************************************************** Buscar Asociados
function buscarAsociados(parametros, tipo) {

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
        parametros.tipo_consulta = 'UNIFICADO';

        //*** 5.1 - Convierte a cadena JSON
        var data = new Object();

        var data = parametros;
        var data = { data };

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarAsociados',
            data: JSON.stringify(data),
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

                if (data.d.length === 0) {
                    if (tipo === 1) {

                        var conEmailMarcado = '';

                        //*** Solo Asociados con Email
                        if ($('#conEmail').is(':checked')) {
                            conEmailMarcado = 'Check solo con Email ACTIVO';
                        }
                        //*** Alerta
                        Alerta('mensajeAlerta', 'No se encontró el Asociado: ' + codigo + ', ' + conEmailMarcado, 'warning', 2000);

                    } else {
                        //*** Alerta
                        Alerta('mensajeAlerta', 'No se encontraron Asociados!', 'warning', 2000);

                    }
                } else {
                    //**** Agregar Filas a la Tabla
                    agregarFilasDTAsociados(data.d);
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


// *** Evento click Eliminar
$(document).on('click', '.btn-delete', function (e) {
    //Evitar el PostBack
    e.preventDefault();

    //Tabla de Asociados
    var tabla = $('#tbl_asociados').DataTable();
    //primer método: eliminar la fila del datatable
    var fila = $(this).parent().parent()[0];

    //Eliminar de la tabla
    tabla.row(fila).remove().draw();
    //Eliminar de la tabla

});

// *** Evento click Enviar por Email
$(document).on('click', '.btn-enviar', function (e) {
    //Evitar el PostBack
    e.preventDefault();
    var fila = $(this).parent().parent()[0];
    var datos = objTabla.fnGetData($(fila));

    var envCodigo = datos[0];
    var envIdentificacion = datos[1];
    var envEmail = datos[9];
    var envInstitucion = datos[4];
    var envNombre = datos[2];

    //Enviar Email
    mensaje = ' <form action="" class="formName">' +
        ' <div class="form-group">' +
        ' <p> Se enviará el estado de cuenta del asociado seleccionado al correo que aparece registrado en <b> PSBANKER </b>, si lo requiere puede cambiar la dirección de correo.<br>' +
        ' <b class="text-danger">( El cambio de correo NO afecta los datos de PSBANKER )</b>.<br><br>' +
        ' <b> Institución: </b>' + envInstitucion + '<br>' +
        ' <b> Cédula: </b>' + envIdentificacion + '<br>' +
        ' <b> Nombre: </b>' + envNombre + '<br>' +
        ' <b> Correo: </b> <input style="width:300px;" type="email" id="emailAsociado" value="' + envEmail + '" /><br>' +
        ' </p>' +
        ' </div>' +
        ' </form>';

    titulo = 'Envío de Estado de Cuenta';
    tema = 'material';
    tipo = 'blue';
    icono = 'fa-envelope';
    tituloConfirmacion = 'Enviar';
    mensajeConfirma = 'Está seguro que desea enviar el estado de cuenta del asociado <b>' + envNombre + '</b> al correo ';
    tituloExito = 'Enviado con Éxito';
    mensajeExito = 'Estado de cuenta <b>Enviado</b> al correo: ';
    tituloCancel = 'Envío Cancelado';
    mensajeCancel = 'El envío há sido <b> Cancelado</b >.';

    //Mensajes
    mensajeEnvio(mensaje, titulo, tema, tipo, icono, tituloConfirmacion, mensajeConfirma, tituloExito, mensajeExito, tituloCancel, mensajeCancel, envCodigo, envIdentificacion, envNombre, envEmail);

});

function mensajeEnvio(mensaje, titulo, theme, type, icon, btnConfirmacion, msjConfirmacion, tituloExito, mensajeExito, tituloCancel, mensajeCancel, envCodigo, envIdentificacion, envNombre, envEmail) {

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

    $.confirm({
        theme: tema,
        title: titulo,
        content: mensaje,
        columnClass: columnClass,
        icon: 'fa ' + icono,
        type: tipo, //red,green,blue
        draggable: true,
        dragWindowGap: 0,
        animation: 'scale',
        closeAnimation: 'zoom',
        animationBounce: 2.5,
        escapeKey: 'cerrar',
        backgroundDismiss: false,

        //Botones del Form principal
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


                                            //Enviar Estado de Cuenta
                                            EnviarEstadoCuenta(envCodigo, envIdentificacion, envNombre, emailEntrada, rutaEnvios);
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

function EnviarEstadoCuenta(codigo, identificacion, nombre, email, directorio) {

    var parametros = new Object();

    parametros.codigo = codigo;
    parametros.cedula = identificacion;
    parametros.nombre = nombre;
    parametros.email = email;
    parametros.ruta = directorio;
    parametros.id = cod_usuario;

    data = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/EnviarEstadoCuenta',
        data: data,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = "Error: " + xhr.status + "<br>" + xhr.responseJSON.Message, "\n" + thrownError;

            //Mostar Mensaje
            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);
        },
        success: function (data) {

            var resultado = JSON.parse(data.d);
            var mensaje;
            var mensajeDetalle;

            $.each(resultado, function (i, item) {
                mensaje = item[0];
                mensajeDetalle = item[1];
            });

            if (mensaje === "0") {

                mensajeAlerta = mensajeExito + '<b class="text-danger">' + emailEntrada + '</b>.';
                titulo = mensajeDetalle;

                mensajeConfirm(mensajeAlerta, titulo, 'material', 'green', 'fa-envelope');
            }
            else if (mensaje === "-1") {

                mensajeAlerta = 'Ocurrió un error. El estado de cuenta no se generó.';
                titulo = mensajeDetalle;

                mensajeConfirm(mensajeAlerta, titulo, 'material', 'red', 'fa-warning');
            }
            else if (mensaje === "-2") {

                mensajeAlerta = 'Ocurrió un error. El estado de cuenta no se envió.';
                titulo = mensajeDetalle;

                mensajeConfirm(mensajeAlerta, titulo, 'material', 'red', 'fa-warning');
            }
        }
    });
}
