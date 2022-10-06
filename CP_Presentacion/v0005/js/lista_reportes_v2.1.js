//*** Parametros Globales
var paginaUrl = 'ListaReportes.aspx';
var tablaReportes, dataReportes;
var objTablaReportes = $('#tbl_reportes');

var inicio;
var MensajeAlerta;
var tipoMsj;
var nombreReportePerfil = 'Lista de Reportes - Report Manager';
var animacion;

// *** Iniciar los Procesos de Perfiles
$(document).ready(function () {

    // ************************ Iniciar Tabla
    // Reportes
    iniciarDataTableReportes();

    //Cargar Subdirectorios del Modal
    CargarSubdirectorios();

});

// ******************************************************************************************** TABLA  REPORTES
// *** Iniciar Tabla Reportes
function iniciarDataTableReportes() {

    // Capturar Tiempo de Inicio
    // inicio = new Date().getTime();

    tablaReportes = objTablaReportes.DataTable({
        //data: filas,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excelHtml5',
                text: "Exportar a EXCEL",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6, 7]
                },
                title: function () {
                    return nombreReportePerfil;
                },
                sheetName: 'Reportes'
            }]
        ,
        language: {
            "url": "bower_components/datatables-buttons/json/spanish.json"
        }
        , "fnDrawCallback": function () {

            var tbl = objTablaReportes.DataTable();

            if (tbl.data().length === 0)
                tbl.buttons('.dt-button').disable();
            else
                tbl.buttons('.dt-button').enable();

            // Agrupar
            var api = this.api();
            var rows = api.rows({ page: 'current' }).nodes();
            var last = null;

            api.column(1, { page: 'current' }).data().each(function (group, i) {
                if (last !== group) {
                    $(rows).eq(i).before(
                        '<tr class="group"><td colspan="10" style="text-align: center;" >' + group + '</td></tr>'
                    );
                    last = group;
                }
            });


        }, "initComplete": function () {
            // Quitar thead del Scroll #1
            $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });

        },

        "aaSorting": [[1, 'asc'], [3, 'asc']],
        'bDestroy': true,
        "bDeferRender": true,
        "paging": false,
        "scrollY": $(document).height() - 270 + "px", //30vh

        "scrollCollapse": true,
        "scroller": true,
        "bSort": true,
        "autoWidth": false,  // Automático = False
        "responsive": true,

        "aoColumns": [
            { "sWidth": "70px", "sClass": "textoCentrado" },      //*** ID 
            null,                                                 //*** SISTEMA 
            null,                                                 //*** NOMBRE
            null,                                                 //*** COD_ALTERNO 
            null,                                                 //*** PROPIETARIO
            {"visible": false},                                   //*** UBICACION
            null,                                                 //*** NODO
            null,                                                 //*** SUBNODO
            null,                                                 //*** ESTADO
            { "sClass": "center", "bSortable": false },           //*** GENERAR REPORTE
            { "bSortable": false, "visible": validaControl('E') } //*** ACCIONES
        ]
    });


    // Ordenar por el grupo
    $('#tbl_reportes tbody').on('click', 'tr.group', function () {
        var currentOrder = tablaReportes.order()[0];
        if (currentOrder[0] === 1 && currentOrder[1] === 'asc') {
            tablaReportes.order([1, 'desc'], [3, 'asc']).draw();
        }
        else {
            tablaReportes.order([1, 'asc'], [3, 'asc']).draw();
        }
    });

    //*** Carga de Reportes por Usuario
    CargarReportesPorUsuario();
}

// *** Agregar Filas a la Tabla Reportes
function agregarFilasDT_Reportes(data) {

    // Limpiar Tabla despues de cualquier cambio 
    objTablaReportes.dataTable().fnClearTable();

    var filas = [];

    for (var i = 0; i < data.length; i++) {

        filas.push([
            data[i].cod_reporte,
            data[i].sistema,
            data[i].nombre,
            data[i].cod_alterno,
            data[i].propietario,
            data[i].ubicacion,
            data[i].directorio,
            data[i].subdirectorio,
            spanEstado(data[i].estado),
            fnEjecutar(data[i].estado, data[i].cod_alterno, data[i].url),
            '<button type="button" value="Actualizar" title="Actualizar" style="margin: 2px;"class="btn btn-xs  btn-primary btn-edit btn_datatable" data-target="#modal_reportes" data-toggle="modal"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></button>&nbsp;' +
            '<button type="button" value="Eliminar" title="Eliminar" style="margin: 2px;"class="btn btn-xs btn-danger  btn-delete btn_datatable"><i class="fa fa-minus-square-o" aria-hidden="true"></i></button>'
        ]);
    }

    objTablaReportes.dataTable().fnAddData(filas, false);

    if (!$('#infoTitulo').length) {

        var btnAgregar;

        var btnMostrarTodos = '<span class="form-group-todos" >'+
                                  '<input type="checkbox" name="chkTodos" id="chkTodos" autocomplete="off" />'+
                                    '<div class="[ btn-group ]" > '+
                                    '<label for="chkTodos" class="[ btn btn-success ]" >'+
                                        '<span class="[ glyphicon glyphicon-ok ]"></span>'+
                                    '<span > </span > '+
                                       '</label > '+
                                       '<label for="chkTodos" class="[ btn btn-default active ]">Mostrar TODOS los Reportes'+
                                    '</label > '+
                                  '</div > '+
                             '</span >&nbsp;&nbsp;&nbsp;';

        // Evento check #chkTodos
        $(document).on("change", "#chkTodos", function () {
            //if (this.checked) {}
            CargarReportesPorUsuario();
        });

        if (validaControl('A')) {
            btnAgregar = "<button runat='server' id='btn_ListaReportes_Agregar' type='button' class='Reportes dt-button buttons-html5' data-target='#modal_reportes' data-toggle='modal'><i class='glyphicon glyphicon-plus'></i><span>&nbsp;Agregar Reporte</span></button>";
        } else {
            btnAgregar = "";

        }
        //*** Titulo de la Tabla
        $("<span id='infoTitulo' class='dataTables_info' role='status' aria-live='polite'>Lista de Reportes&nbsp;&nbsp;&nbsp;" + btnAgregar +'&nbsp;&nbsp;&nbsp;'+ btnMostrarTodos +"</span>").prependTo("div.dt-buttons");
    }

    //************ Quitar thead del Scroll #2
    $('#tbl_reportes').on('draw.dt', function () {
        $('.dataTables_scrollBody thead tr').css({ visibility: 'collapse' });
    });

    //*** Dibujar la Tabla
    tablaReportes.draw();
}

// *** Función Condicional de Estado
function spanEstado(estado) {

    if (estado === "Activo") {
        return '<span class="label label-success" style="font-size:9px; padding-top: 1px; padding-bottom: 1px;">' + estado + '</span>';
    }
    else if (estado === "Inactivo") {
        return '<span class="label label-danger"  style="font-size:9px; padding-top: 1px; padding-bottom: 1px;">' + estado + '</span>';
    }
    else if (estado === "Bloqueado") {
        return '<span class="label label-warning"  style="font-size:9px; padding-top: 1px; padding-bottom: 1px;">' + estado + '</span>';
    }
    else {
        return '<span class="label label-primary"  style="font-size:9px; padding-top: 1px; padding-bottom: 1px;">' + estado + '</span>';
    }
}

// *** Función validaControl()
function validaControl(tipo) {

    validacion = false;

    if (tipo === 'A') {
        if ($('#FnAgregar').length) {
            validacion = true;
        }
    } else if (tipo === 'E') {

        if ($('#FnEditar').length) {
            validacion = true;
        }
    }

    return validacion;
}

// *** Función Ejecutar
function fnEjecutar(estado, cod_alterno, url) {

    if (estado === "Activo") {
        return '<button type="button" value="' + url + '" title="Generar ' + cod_alterno + '" style="margin: 2px;" class="btn btn-xs  btn-warning btn-generar btn_datatable"><i class="glyphicon glyphicon-ok" aria-hidden="true"></i></button>';
    }
    else {
        return '';
    }
}

// ***  Carga total de Reportes - CargarReportes()
function CargarReportesPorUsuario() {

    //*** Iniciar Dialogo de Progreso
    dialogoProgreso.show('Cargando reportes ' + '<span class="glyphicon glyphicon-hourglass"></span>');
    //*** Animación -  cambio de mensaje cada 2 segundos + inicio después del retraso de 1 segundo
    animacion = dialogoProgreso.animate();

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();
    var p_tipo;

    // T = Todos - // U = Usuario
    if (!$('#chkTodos').length) {
        p_tipo = 'U';
    } else {

        if ($('#chkTodos').is(':checked')) {
            p_tipo = 'T'; 
        } else {
            p_tipo = 'U'; 
        }
    }

    parametros.login = login;
    parametros.tipo = p_tipo;

    // *** Llamar a AjaxCargarReportesUsuario AJAX
    AjaxCargarReportesUsuario(parametros);
}

// *** AjaxCargarReportesUsuario ( TODOS )
function AjaxCargarReportesUsuario(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/ReportesPorUsuario',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = "Error: " + xhr.status + "<br>" + xhr.responseJSON.Message, "\n" + thrownError;

            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);

        },
        success: function (data) {

            //console.log(data.d);

            if (data.d.length > 0) {

                //Agregar Filas
                agregarFilasDT_Reportes(data.d);

            } else {

                MsjPersonalizado = 'No existen reportes en la base de datos!';
                tipoMsj = 'info';

                Alerta('mensajesAlerta', MsjPersonalizado, tipoMsj, 2000);
            }
        }
    }).done(function () {

        //*** Detener dialogoProgreso 
        if (animacion !== undefined) {
            setTimeout(function () {
                dialogoProgreso.hide();
                dialogoProgreso.stopAnimate(animacion);
            }, 500);
        }
    });
}

// *** Botón Cancelar
$("#btnModalCancelar").click(function (e) {
    e.preventDefault();
    cerrarModal();
});

// *** Función Ocultar Modal 
function cerrarModal() {

    //Ocultar Modal
    $("#modal_reportes").modal('hide');
}

// *** Limpiar Modal al Ocultarlo
$(document).on('hidden.bs.modal', '#modal_reportes', function (e) {
    //Limpiar Modal
    limpiarDataModal();
});

// *** Cuando se muestre el Modal
$(document).on('shown.bs.modal', '#modal_reportes', function (e) {

    // Enfocar el campo codigoReporte
    $('#codigoReporte').focus();
});

// *** Evento click  Actualizar Reporte
$(document).on('click', '.btn-edit', function (e) {

    //Evitar el PostBack
    e.preventDefault();

    //Guarda en variable data la info del reporte
    var fila = $(this).parent().parent()[0];
    dataReportes = objTablaReportes.fnGetData(fila);

    var accion = "Editar";

    //Llenar el Modal
    //setTimeout(function () {
    llenarDataModal(accion);
    //}, 1000);

});

// *** Generar el Reporte
$(document).on('click', '.btn-generar', function (e) {

    //Evitar el PostBack
    e.preventDefault();

    //Ejecutar 
    var href = $(this).val();

    //console.log(href);
    window.open(href, '_blank');

});

// *** Evento click Eliminar 
$(document).on('click', '.btn-delete', function (e) {
    //Evitar el PostBack
    e.preventDefault();

    var row = $(this).closest('tr');
    var colEstado = row.find('td:eq(7)').text();

    if (colEstado !== "Inactivo") {

        //Inactivar Reporte
        InactivarReporte(row);

    } else {
        MensajeAlerta = 'El reporte ya está inactivo!';
        Alerta('mensajesAlerta', MensajeAlerta, 'warning', 2000);
    }
});

// *** Evento click  Agregar Reporte 
$(document).on('click', '#btn_ListaReportes_Agregar', function (e) {

    //Evitar el PostBack
    e.preventDefault();

    var accion = "Agregar";

    //Llenar el Modal
    llenarDataModal(accion);
});

// *** Cargar datos en el Modal Actualizar
function llenarDataModal(accion) {

    var titulo = "";
    var estado;

    if (accion === "Editar") {

        // Define el Título y ArchivoRPT
        titulo = "Actualizar Reporte (" + dataReportes[3] + ")";
        var archivoRpt = dataReportes[5].substring(dataReportes[5].lastIndexOf("/") + 1, dataReportes[5].length);

        // Define los datos adicionales 
        $("#modalTitulo").text(titulo);
        $("#idReporte").val(dataReportes[0]);
        $("#codigoReporte").val(dataReportes[3]);
        $("#nombreReporte").val(dataReportes[2]);
        $("#archivoRpt").val(archivoRpt);
        $("#ddlSistema").val($("#ddlSistema option:contains('" + dataReportes[1] + "')").val());
        $("#ddlDirectorio").val($("#ddlDirectorio option:contains('" + dataReportes[6] + "')").val());
        $("#ddlSubdirectorio").val($("#ddlSubdirectorio option:contains('" + dataReportes[7] + "')").val());
        $("#ddlPropietario").val($("#ddlPropietario option:contains('" + dataReportes[4] + "')").val());
        $("#ubicacionReporte").val(dataReportes[5]);

        //console.log($.parseHTML(dataReportes[7])[0].innerHTML);

        // Define el Estado
        if ($.parseHTML(dataReportes[8])[0].innerHTML === "Bloqueado") {
            estado = "Activo";
        } else {
            estado = $.parseHTML(dataReportes[8])[0].innerHTML;
        }

        $("#ddlEstado").val($("#ddlEstado option:contains('" + estado + "')").val());

    } else {
        titulo = "Registro de Reportes";
        var subdirectorio = "";

        $("#modalTitulo").text(titulo);

        estado = "Activo";
        $("#ddlEstado").val($("#ddlEstado option:contains('" + estado + "')").val());
    }
}

// *** limpiarDataModal
function limpiarDataModal() {

    // *** Valores Iniciales
    $("#modalTitulo").text("");
    $("#idReporte").val("");
    $("#codigoReporte").val("");
    $("#nombreReporte").val("");
    $("#archivoRpt").val("");
    $("#ddlSistema").val("1");
    $("#ddlDirectorio").val("1");
    $("#ddlSubdirectorio").val("1");
    $("#ddlPropietario").val("1");
    $("#ubicacionReporte").val("");
    $("#ddlEstado").val("1");
}

// *** Cambio en TextBox Ubicación
$("#modal_reportes").on("change paste keyup", "#archivoRpt ", function () {

    //Actualizar Ubicación
    ActualizarUbicacion();
});

// *** Cambio de Directorio
$("#modal_reportes").on("change", "#ddlDirectorio", function () {
    //Cargar Directorio
    CargarSubdirectorios();
});

// *** Cambio de Directorio
$("#modal_reportes").on("change", "#ddlSubdirectorio", function () {

    //Actualizar Ubicación
    ActualizarUbicacion();
});

// *** Evento click  Agregar Reporte 
$(document).on('click', '#btnModalAccion', function (e) {
    procesarAccion();
});

// *** keypress modal_reportes ( ENTER )
$("#modal_reportes").on('keypress', function (e) {

    if (e.which === 13) {// Tecla Enter 
        $('#btnModalAccion').click();
    }
});

// *** procesarAccion
function procesarAccion() {

    //Evitar el PostBack
    // e.preventDefault(); // No se pone para que se procese REQUIRED

    var vacios = verificarInputVacios();

    if (vacios === false) {
        //Verificar si existe el código
        var idReporte = $('#idReporte').val();

        if (idReporte === "") {

            var codigoReporte = $('#codigoReporte').val();

            var existe = 0;
            var listaArray;

            //Arreglo con los Codigos de Reporte
            listaArray = tablaReportes.rows().column(3).data().toArray();

            // Verificar si Existe
            existe = jQuery.inArray(codigoReporte, listaArray);

            if (existe === -1) {
                //Registrar el Reporte
                RegistrarReporte();
            } else {
                //Verificar si hay registros en la Tabla
                if (tablaReportes.data().any()) {
                    Alerta('mensajesAlerta', 'El Código de Reporte ( ' + codigoReporte + ' ) ya Existe!', 'danger', 2000);
                    $('#codigoReporte').focus();
                }
            }
        } else {
            ActualizarReporte();
        }
        //Ocultar Modal
        cerrarModal();
    }
}

// *** verificarInputVacios
function verificarInputVacios() {

    var codigoReporte = $("#codigoReporte").val();
    var nombreReporte = $("#nombreReporte").val();
    var archivoRpt = $("#archivoRpt").val();

    if (codigoReporte === "") {
        $("#codigoReporte").focus();
    } else if (nombreReporte === "") {
        $("#nombreReporte").focus();
    } else if (archivoRpt === "") {
        $("#archivoRpt").focus();
    } else {
        return false;
    }
}

// ***  Crear Reporte
function RegistrarReporte() {

    var accion = 'C';
    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();
    //Convierte a cadena JSON
    parametros = JSON.stringify(ObtenerValores(accion, '0'));

    //Ajax
    $.ajax({
        type: "POST",
        url: paginaUrl + '/MantenimientoReportes',
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (data) {

            // Carga de Reportes por Usuario
            CargarReportesPorUsuario();

            // Mensaje
            MensajeAlerta = 'El reporte se registró correctamente!';
            Alerta('mensajesAlerta', MensajeAlerta, 'success', 2000);
        }
    });
}

// *** Actualizar Reporte
function ActualizarReporte() {

    var accion = 'U';
    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();
    //Convierte a cadena JSON
    parametros = JSON.stringify(ObtenerValores(accion, '0', '0'));

    //Ajax
    $.ajax({
        type: "POST",
        url: paginaUrl + '/MantenimientoReportes',
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (data) {
            //**** Agregar Filas a la Tabla
            CargarReportesPorUsuario();

            // Mensaje
            MensajeAlerta = 'El reporte se actualizó correctamente!';
            Alerta('mensajesAlerta', MensajeAlerta, 'success', 2000);
        }

    });
}

// *** Borrar Reporte
function InactivarReporte(row) {

    var idReporte = row.find('td:eq(0)').text();
    var codigo = row.find('td:eq(3)').text();
    var accion = 'D';
    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    //Convierte a cadena JSON
    parametros = JSON.stringify(ObtenerValores(accion, idReporte, codigo));

    //Ajax
    $.ajax({
        type: "POST",
        url: paginaUrl + '/MantenimientoReportes',
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (data) {

            // Carga de Reportes por Usuario
            CargarReportesPorUsuario();

            // Mensaje
            MensajeAlerta = 'El reporte se inactivó correctamente!';
            Alerta('mensajesAlerta', MensajeAlerta, 'success', 2000);
        }
    });
}

// *** ObtenerValores
function ObtenerValores(accion, idReporte, codigo) {

    var parametros = new Object();
    var id;

    if (accion === 'D') {

        id = idReporte;

        parametros.accion = accion;
        parametros.id = id;
        parametros.codigo = codigo;
        parametros.nombre = "";
        parametros.archivoRpt = "";
        parametros.cod_sistema = "0";
        parametros.cod_directorio = "0";
        parametros.cod_subdirectorio = "0";
        parametros.ubicacion = "0";
        parametros.cod_estado = "";
        parametros.cod_usuario = cod_usuario;
        parametros.cod_propietario = "1";

    } else {
        id = $("#idReporte").val();

        if (id === "") {
            id = "0";
        }

        parametros.accion = accion;
        parametros.id = id;
        parametros.codigo = $("#codigoReporte").val();
        parametros.nombre = $("#nombreReporte").val();
        parametros.archivoRpt = $("#archivoRpt").val();
        parametros.cod_sistema = $("#ddlSistema option:selected").val();
        parametros.cod_directorio = $("#ddlDirectorio option:selected").val();
        parametros.cod_subdirectorio = $("#ddlSubdirectorio option:selected").val();
        parametros.ubicacion = $("#ubicacionReporte").val();
        parametros.cod_estado = $("#ddlEstado option:selected").val();
        parametros.cod_usuario = cod_usuario;
        parametros.cod_propietario = $("#ddlPropietario option:selected").val();
    }
    return parametros;
}

// *** Cargar Subdirectorios
function CargarSubdirectorios() {

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();
    parametros.cod_directorio = $("#ddlDirectorio option:selected").val();

    //Convierte a cadena JSON
    parametros = JSON.stringify(parametros);
    //Ajax
    $.ajax({
        type: "POST",
        url: paginaUrl + '/CargarSubdirectorios',
        data: parametros,
        contentType: "application/json; charset=utf-8",
        dataType: "json",
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: IniciarCargaSubdirectorios

    });
}

// *** IniciarCargaSubdirectorios
function IniciarCargaSubdirectorios(response) {


    if (response.d.length > 0) {
        //Limpiar Lista
        $('#ddlSubdirectorio').empty();
        //Llenar el DrodDownList
        $.each(response.d, function () {
            $('#ddlSubdirectorio').append($("<option></option>").val(this['cod_subdirectorio']).html(this['nombre']));
        });

    } else {
        $('#ddlSubdirectorio').empty().append('<option selected="selected" value="0">NO EXISTEN SUBDIRECTORIOS</option>');
    }

    //Actualizar Ubicación
    ActualizarUbicacion();

}

// *** ActualizarUbicacion
function ActualizarUbicacion() {
    var subdirectorio = "";

    //ubicacionReporte
    if ($("#ddlSubdirectorio option:selected").val() !== "0") {
        subdirectorio = "/" + $("#ddlSubdirectorio option:selected").text();
    }

    $("#ubicacionReporte").val("/" + $("#ddlDirectorio option:selected").text() + subdirectorio + "/" + $("#archivoRpt").val() + "");
}