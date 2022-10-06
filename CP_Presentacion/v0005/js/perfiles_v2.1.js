//*** Parametros Globales
var paginaUrl = 'Perfiles.aspx';

var tablaPerfiles, data;
var objTablaPerfiles = $('#tbl_perfiles');

var tablaReportes, dataReportes;
var objTablaReportes = $('#tbl_reportes');

var tablaUsuarios, dataUsuarios;
var objTablaUsuarios = $('#tbl_usuarios');

var inicio;

var MensajeAlerta;
var tipoMsj;

var triggeredByChildReportes = false;
var triggeredByChildUsuarios = false;

var nombreReportePerfil = 'Lista de Perfiles - Report Manager';

// ToolTip ( Implementación )
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').tooltip().off("focusin focusout");
});


// *** Iniciar los Procesos de Perfiles
$(document).ready(function () {

    // ************************ Iniciar Tablas
    //*** Inicializar

    // Perfiles
    iniciarDataTablePerfiles();
    // Usuarios
    iniciarDataTableUsuarios();
    // Reportes
    iniciarDataTableReportes();

    //***  Cargar
    // Carga de Usuarios
    CargarUsuarios();
    // Carga de Reportes
    CargarReportes();
    // Carga de Perfiles
    AjaxCargarPerfiles();

    //setTimeout(function () {

    //}, 100);
});

// **************************************************************************************************************  INICIO DATATABLES
// ******************************************************************************************** TABLA  REPORTES
// *** Iniciar Tabla PERFILES
function iniciarDataTablePerfiles() {
    tablaPerfiles = objTablaPerfiles.DataTable({
        //data: filas,
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'excelHtml5',
                text: "Exportar a EXCEL",
                exportOptions: {
                    columns: [0, 1, 2, 3]
                },
                title: function () {
                    return nombreReportePerfil;
                },
                sheetName: 'Perfiles'
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
            var tbl = objTablaPerfiles.DataTable();
            if (tbl.data().length === 0)
                tbl.buttons('.dt-button').disable();
            else
                tbl.buttons('.dt-button').enable();
        },
        "aaSorting": [[0, 'desc']],
        'bDestroy': true,
        "bDeferRender": true,
        "paging": false,
        "scrollY": '147px', //30vh
        "scrollCollapse": true,
        "scroller": true,
        "bSort": true,
        "autoWidth": true,  // Automático = False
        "responsive": false,
        "aoColumns": [
            { "sWidth": "70px", "sClass": "textoCentrado" },  //*** ID
            null,                                             //*** NOMBRE
            null,                                             //*** DESCRIPCION
            null,                                             //*** ESTADO
            { "bSortable": false }                             //*** ACCIONES
        ]
    });
}

// *** Agregar Filas a la Tabla Perfiles
function agregarFilasDT_Perfiles(data) {

    // Limpiar Tabla despues de cualquier cambio 
    objTablaPerfiles.dataTable().fnClearTable();

    for (var i = 0; i < data.length; i++) {

        var filas = new Array();

        filas[0] = data[i].id;
        filas[1] = data[i].nombre;
        filas[2] = data[i].descripcion;
        filas[3] = spanEstado(data[i].estado);
        filas[4] = '<button type="button" value="Actualizar" title="Editar" class="btn btn-primary  btn-xs  btn-edit btn_datatable"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></button>';

        objTablaPerfiles.dataTable().fnAddData(filas, false);
    }

    // Función Condicional de Acción
    function spanAccion(estado) {

        var btnAccion = '<button type="button" value="Actualizar" title="Actualizar" class="btn btn-primary  btn-xs  btn-edit btn_datatable"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></button>';

        return btnAccion;
    }
    //*** Dibujar la Tabla
    tablaPerfiles.draw();

    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').tooltip().off("focusin focusout");

    if (!$('#infoTituloPerfiles').length) {

        //*** Titulo de la Tabla
        $("<span id='infoTituloPerfiles' class='dataTables_info' role='status' aria-live='polite'>LISTA DE PERFILES DE USUARIO&nbsp;&nbsp;&nbsp;</span>").prependTo("#tbl_perfiles_wrapper div.dt-buttons");
    }

}

// ********************************************  TABLA  REPORTES
// *** Iniciar Tabla Reportes
function iniciarDataTableReportes() {

    // Capturar Tiempo de Inicio
    // inicio = new Date().getTime();

    tablaReportes = objTablaReportes.DataTable({
        //data: filas,
        language: {
            "url": "bower_components/datatables-buttons/json/spanish.json"
        },
        "aaSorting": [[1, 'asc']],
        'bDestroy': true,
        "bDeferRender": true,
        "paging": false,
        "scrollY": $(document).height() - 613 + "px", //30vh

        "scrollCollapse": true,
        "bSort": true,
        "autoWidth": true,  // Automático = False
        "responsive": true,
        "aoColumns": [
            { "sWidth": "60px", "sSortDataType": "dom-checkbox", "sClass": "centerEnTabla" }, //*** CHECK 
            { "sWidth": "80px", "sClass": "textoCentrado" },   //*** ID 
            null,                     //*** SISTEMA 
            null,                     //*** NOMBRE
            null,                     //*** COD_ALTERNO 
            null,                     //*** UBICACIÓN
            null,                     //*** PROPIETARIO
            null                      //*** ESTADO
        ]
    });

    //$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    //    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
    //        return $('input', td).prop('checked') ? '1' : '0';
    //    });
    //}

}

// *** Agregar Filas a la Tabla Reportes
function agregarFilasDT_Reportes(data) {

    // Limpiar Tabla despues de cualquier cambio 
    objTablaReportes.dataTable().fnClearTable();

    var filas = [];

    for (var i = 0; i < data.length; i++) {

        filas.push(['<input type="checkbox" class="check" value=' + data[i].cod_reporte + '>', data[i].cod_reporte, data[i].sistema, data[i].nombre
                  , data[i].cod_alterno, data[i].ubicacion, data[i].propietario, spanEstado(data[i].estado)]);
    }

    objTablaReportes.dataTable().fnAddData(filas, false);

    //*** Dibujar la Tabla
    tablaReportes.draw();

    //*** iCheck
    iCheckAplicar('tbl_reportes_wrapper', 'iCheckReportes');

    //*** Titulo de la Tabla
    if (!$('#infoTituloReportes').length) {

        //*** Titulo de la Tabla
        $("<div id='infoTituloReportes' class='dataTables_info' role='status' aria-live='polite'>LISTA DE REPORTES</div>").appendTo("#tbl_reportes_wrapper .col-sm-6:first");
    }

    //*** Tiempo Total de Ejecución
    //$("#txtDuracion").text("Duración : " + (new Date().getTime() - inicio) / 1000 + " Segundos");
}

// ******************************************************************************************** TABLA  USUARIOS
// Iniciar Tabla Usuarios
function iniciarDataTableUsuarios() {

    tablaUsuarios = objTablaUsuarios.DataTable({
        //data: filas,
        language: {
            "url": "bower_components/datatables-buttons/json/spanish.json"
        },
        "aaSorting": [[2, 'asc']],
        'bDestroy': true,
        "bDeferRender": true,
        "paging": false,
        "scrollY": $(document).height() - 613 + "px", //30vh

        "scrollCollapse": true,
        "bSort": true,
        "autoWidth": true,  // Automático = False
        "responsive": true,
        "aoColumns": [
            { "sWidth": "60px", "sSortDataType": "dom-checkbox", "sClass": "centerEnTabla" }, //*** CHECK 
            { "sWidth": "80px", "sClass": "textoCentrado" },   //*** ID 
            null,  //*** Nombre
            null,  //*** Sucursal
            null,  //*** Departamento
            null,  //*** Rol
            null,  //*** Login
            null   //*** Estado
        ]
    });
}

// *** Agregar Filas a la Tabla Reportes
function agregarFilasDT_Usuarios(data) {

    // Limpiar Tabla despues de cualquier cambio a los Usuarios
    objTablaUsuarios.dataTable().fnClearTable();

    for (var i = 0; i < data.length; i++) {

        var filas = new Array();

        filas[0] = '<input type="checkbox" class="check" value=' + data[i].cod_usuario + '>';
        filas[1] = data[i].cod_usuario;
        filas[2] = data[i].nombre;
        filas[3] = data[i].sucursal.nombre;
        filas[4] = data[i].departamento.nombre;
        filas[5] = data[i].rol.nombre;
        filas[6] = data[i].login;
        filas[7] = spanEstado(data[i].i_estado);

        objTablaUsuarios.dataTable().fnAddData(filas, false);
    }

    //*** Dibujar la Tabla
    tablaUsuarios.draw();

    //*** Estilo iCheck Para los Input de la Tabla
    iCheckAplicar('tbl_usuarios_wrapper', 'iCheckUsuarios');

    //*** Titulo de la Tabla
    if (!$('#infoTituloUsuarios').length) {

        //*** Titulo de la Tabla
        $("<div id='infoTituloUsuarios' class='dataTables_info' role='status' aria-live='polite'>LISTA DE USUARIOS</div>").appendTo("#tbl_usuarios_wrapper .col-sm-6:first");
    }


}

// ******************************************************************************************** FIN TABLAS


// ******************************************************************************************** INICIO EVENTOS 

// *** Evento Click en Botón Registrar
$(function () {

    //Ordenar CheckBox de las tablas Reportes y Usuarios
    $.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
        return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
            return $('input', td).prop('checked') ? '0' : '1';
        });
    };

    //Renderiza las tablas para que el Header se ajuste ( Se necesita por el estado Hidden )
    $('a[href="#tab_2_usuarios"], a[href="#tab_1_reportes"]').on("shown.bs.tab", function (e) {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    });

    // Actualizar Perfil ( Botón Actualizar del Modal )
    $("#btn_Perfiles_Editar").on("click", function (e) {
        // Actualizar Perfil
        ActualizarPerfil();
    });

    // *** Botón Guardar / Registrar
    $("#btn_Perfiles_Agregar").on("click", function (e) {

        //*** Buscar el Perfil en la Tabla de Existentes
        var valorNombrePerfil = $('#txtNombrePerfil').val();

        if (valorNombrePerfil !== '') {

            //*** Buscar el Perfil
            var existe = 0;
            var listaArray;

            //Arreglo con los Nombres de Perfil Existentes
            listaArray = tablaPerfiles.rows().column(1).data().toArray();

            var valorIDPerfil = $('#txtIDPerfil').val();

            if (valorIDPerfil !== '') {

                //Llenar el Modal ( modal_actualizar )
                llenarDataModal();
                // Mostrar Modal
                $("#modal_actualizar").modal('show');

            } else {

                if ($('#FnAgregar').length) {
                    // Verificar si Existe
                    existe = jQuery.inArray(valorNombrePerfil, listaArray);

                    if (existe === -1) {
                        //Crear el Perfil
                        CrearPerfil();
                    } else {
                        //Verificar si hay registros en la Tabla
                        if (tablaPerfiles.data().any()) {
                            Alerta('mensajeAlerta', 'El Perfil ( ' + valorNombrePerfil + ' ) ya Existe!', 'danger', 2000);
                        }
                    }
                } else {
                    Alerta('mensajeAlerta', 'No tiene permiso para Agregar perfiles!', 'danger', 2000);
                }
            }
        } else {
            // Enfocar el campo Nombre
            $('#txtNombrePerfil').focus();
        }

    });

    // *** Botón Limpiar
    $("#btnCancelar").on("click", function (e) {

        // Limpiar Perfil Seleccionado
        limpiarRegistroPerfil();
    });



    // *** Botón Editar Perfil - btn-edit
    $(document).on('click', '.btn-edit:not(.disabled)', function (e) {


        // Limpiar Perfil Seleccionado
        limpiarRegistroPerfil();

        //Obtener Fila seleccionada
        var fila = $(this).parent().parent()[0];

        //Llenar variable data con los datos de fila
        data = objTablaPerfiles.fnGetData(fila);

        // Llenar Formulario Perfiles
        llenarRegistroPerfiles();

        // Paso 1 - Cargar Reportes por Perfil
        CargarReportesUsuariosPorPerfil();

    });

    // *** Evento Enter en Registrar Perfil
    $(document).keypress(function (e) {

        //*** Buscar el Perfil en la Tabla de Existentes
        var key = e.which;

        if (key === 13) {

            var valorNombrePerfil = $('#txtNombrePerfil').val();

            if (valorNombrePerfil !== '') {

                // Buscar el Valor
                var existe = 0;
                var listaArray;

                //Arreglo con los Nombres de Perfil Existentes
                listaArray = tablaPerfiles.rows().column(1).data().toArray();

                var valorIDPerfil = $('#txtIDPerfil').val();

                if (valorIDPerfil !== '') {

                    //Llenar el Modal ( modal_actualizar )
                    llenarDataModal();
                    // Mostrar Modal
                    $("#modal_actualizar").modal('show');

                } else {

                    // Verificar si Existe
                    existe = jQuery.inArray(valorNombrePerfil, listaArray);

                    if (existe === -1) {
                        //Crear el Perfil
                        CrearPerfil();
                    } else {
                        //Verificar si hay registros en la Tabla
                        if (tablaPerfiles.data().any()) {
                            Alerta('mensajeAlerta', 'El Perfil ( ' + valorNombrePerfil + ' ) ya Existe!', 'danger', 2000);
                        }
                    }
                }
            } else {
                // Enfocar el campo Nombre
                $('#txtNombrePerfil').focus();
            }
        }
    });

}); // Fin Function


// ********************************************************************************************  FUNCIONES 

function iCheckAplicar(idTablaWrapper, idCheckPadre) {

    // Ejemplo: idTablaWrapper = tbl_usuarios_wrapper  / idCheckPadre = ( Check Principal)

    //********************************************* iCheck
    $('#' + idTablaWrapper + ' input').iCheck({
        checkboxClass: 'icheckbox_futurico',
        radioClass: 'iradio_futurico'
    });

    //********************************************* Events iCheck ***
    $('#' + idCheckPadre + '').on('ifChecked', function (event) {
        $('#' + idTablaWrapper + ' .check').iCheck('check');

        if (idCheckPadre === 'iCheckReportes') {
            triggeredByChildReportes = false;
        } else {
            triggeredByChildUsuarios = false;
        }
    });

    //********************************************* Uncheck All
    $('#' + idCheckPadre + '').on('ifUnchecked', function (event) {

        if (idCheckPadre === 'iCheckReportes') {

            if (!triggeredByChildReportes) {
                $('#' + idTablaWrapper + ' .check').iCheck('uncheck');
            }
            triggeredByChildReportes = false;
        } else {

            if (!triggeredByChildUsuarios) {
                $('#' + idTablaWrapper + ' .check').iCheck('uncheck');
            }
            triggeredByChildUsuarios = false;
        }
    });

    // Se elimina el estado verificado de "Todos" si alguna casilla de verificación está desmarcada ***
    $('#' + idTablaWrapper + ' .check').on('ifUnchecked', function (event) {

        if (idCheckPadre === 'iCheckReportes') {
            triggeredByChildReportes = true;
            $('#' + idCheckPadre + '').iCheck('uncheck');
        } else {
            triggeredByChildUsuarios = true;
            $('#' + idCheckPadre + '').iCheck('uncheck');
        }
    });

    // Marcar Check Padre si marco el ultimo Check ***
    $('#' + idTablaWrapper + ' .check').on('ifChecked', function (event) {

        if ($('#' + idTablaWrapper + ' .check').filter(':checked').length === $('#' + idTablaWrapper + ' .check').length) {
            $('#' + idCheckPadre + '').iCheck('check');
        }
    });
}

// *** Función Condicional de Estado
function spanEstado(estado) {

    if (estado === "Activo") {
        return '<span class="label label-success" style="font-size:9px;">' + estado + '</span>';
    }
    else if (estado === "Inactivo") {
        return '<span class="label label-danger"  style="font-size:9px;">' + estado + '</span>';
    }
    else {
        return '<span class="label label-warning"  style="font-size:9px;">' + estado + '</span>';
    }
}

// Cargar datos en el Modal Actualizar
function llenarDataModal() {

    var titulo = "Actualizar el Perfil ( " + $("#txtNombrePerfil").val() + " )";
    var perfilDetalle = "ID: " + $("#txtIDPerfil").val() + "     |     Nombre: " + $("#txtNombrePerfil").val() + "     |     Estado: " + $("#ddlEstado option:selected").text();
    var perfilDescripcion = $('#txtDescripcionPerfil').val();
    var perfilReportes = reportesSeleccionados();
    var perfilUsuarios = usuariosSeleccionadosLogin();

    //Llenar Modal
    $("#ModalTitulo").text(titulo);
    $("#ModalTxtPerfilDetalle").val(perfilDetalle);
    $("#ModalTxtPerfilDescripcion").val(perfilDescripcion);
    $("#ModalTxtPerfilReportes").val(perfilReportes);
    $("#ModalTxtPerfilUsuarios").val(perfilUsuarios);
}

// *** AJAX - Crear el PerFil - CrearPerfil()
function CrearPerfil() {

    // *** Variables
    var Estado;

    // *** Obtener valores
    var Id = "0";
    var NombrePerfil = $("#txtNombrePerfil").val();
    var DescripcionPerfil = $("#txtDescripcionPerfil").val();

    if ($("#txtIDPerfil").val() === "") {
        Estado = "P";
    } else {
        Estado = $("#ddlEstado").val();
    }

    // Obtener Reportes Marcados
    var reportesMarcados = reportesSeleccionados();

    // Obtener Usuarios Marcados
    var usuariosMarcados = usuariosSeleccionados();

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    // Parametros Default
    parametros.id = Id;
    parametros.nombre = NombrePerfil;
    parametros.descripcion = DescripcionPerfil;
    parametros.estado = Estado;
    parametros.listaReportes = reportesMarcados;
    parametros.listaUsuarios = usuariosMarcados;
    parametros.login = login;
    parametros.cod_usuario = cod_usuario;

    //console.log(parametros);

    // *** Llamar a CrearPerfil AJAX
    AjaxCrearPerfil(parametros);
}

// *** reportesSeleccionados()
function reportesSeleccionados() {
    // *** Obtener Reportes Seleccionados
    var listaArray;

    listaArray = tablaReportes.$("input:checkbox:checked").map(function () {
        return $(this).val();
    }).toArray();

    return listaArray;
}

// *** usuariosSeleccionados()
function usuariosSeleccionados() {
    // *** Obtener Usuarios Seleccionados
    var listaArray;

    listaArray = tablaUsuarios.$("input:checkbox:checked").map(function () {
        return $(this).val();
    }).toArray();

    return listaArray;
}

// *** usuariosSeleccionadosLogin()
function usuariosSeleccionadosLogin() {

    // *** Obtener Usuarios Seleccionados ( Login )
    var listaArray;

    //Obtener todas las filas con check
    listaArray = $(tablaUsuarios.$("input:checkbox:checked").map(function () {
        //Obtener el valor de la columna 6
        return $(this).closest('tr').find("td").eq(6).html();

    })).toArray().join(' - ');

    return listaArray;
}

// *** Actualizar el PerFil - ActualizarPerfil()
function ActualizarPerfil() {

    // Obtener valores
    var Id = $("#txtIDPerfil").val();
    var NombrePerfil = $("#txtNombrePerfil").val();
    var DescripcionPerfil = $("#txtDescripcionPerfil").val();
    var Estado = $("#ddlEstado").val();

    // Obtener Reportes Marcados
    var reportesMarcados = reportesSeleccionados();

    // Obtener Usuarios Marcados
    var usuariosMarcados = usuariosSeleccionados();

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    // Parametros Default
    parametros.id = Id;
    parametros.nombre = NombrePerfil;
    parametros.descripcion = DescripcionPerfil;
    parametros.estado = Estado;
    parametros.listaReportes = reportesMarcados;
    parametros.listaUsuarios = usuariosMarcados;
    parametros.login = login;
    parametros.cod_usuario = cod_usuario;

    //Llamar a ActualizarPerfil AJAX
    AjaxActualizarPerfil(parametros);
}

// ***  Carga total de Reportes - CargarReportes()
function CargarReportes() {

    // Obtener valores iniciales ( Todos los Reportes sin Marcar)
    var cod_perfil = "0";
    var listaReportes = "";

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    // Parametros Default
    parametros.codigoPerfil = cod_perfil;
    parametros.listaReportes = listaReportes;

    // *** Llamar a AjaxCargarReportes AJAX
    AjaxCargarReportes(parametros);
}

// ***  Carga total de Usuarios - CargarUsuarios()
function CargarUsuarios() {

    // Obtener valores iniciales ( Todos los Reportes sin Marcar)
    //var cod_perfil = "0";
    //var listaReportes = "";

    //// Guardar parámetros y usar Libreria de JSON
    //var parametros = new Object();

    //// Parametros Default
    //parametros.codigoPerfil = cod_perfil;
    //parametros.listaReportes = listaReportes;

    // *** Llamar a AjaxCargarUsuarios AJAX
    AjaxCargarUsuarios();
}

// *** Carga de Reportes por Perfil - CargarReportesPerfil()
function CargarReportesUsuariosPorPerfil() {

    // Obtener valores iniciales ( Todos los Reportes sin Marcar)
    var cod_perfil = data[0];
    //var listaReportes = ""; //reportesSeleccionados();

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    // Parametros Default
    parametros.codigoPerfil = cod_perfil;
    //parametros.listaReportes = listaReportes;

    //Paso 2 - Cargar Usuarios por Perfil
    AjaxCargarUsuariosPorPerfil(parametros);

    // Paso 2 - Cargar Reportes por Perfil
    AjaxCargarReportesPorPerfil(parametros);

}

function MarcarReportesPerfil(data) {

    var listaArrayObtenido;

    var valoresArray = data;

    listaArrayObtenido = tablaReportes.$("input:checkbox.check").map(function () {

        // Verifico si existe en el Array
        if (jQuery.inArray($(this).val(), valoresArray) !== -1) {

            //Check al Objeto
            $(this).iCheck('check');

            //Retorno el valor encontrado
            return $(this).val();
        }
    }).toArray();

    //alert(listaArrayObtenido);
}

function MarcarUsuariosPerfil(data) {

    var listaArrayObtenido;

    var valoresArray = data;

    listaArrayObtenido = tablaUsuarios.$("input:checkbox.check").map(function () {

        // Verifico si existe en el Array
        if (jQuery.inArray($(this).val(), valoresArray) !== -1) {

            //Check al Objeto
            $(this).iCheck('check');

            //Retorno el valor encontrado
            return $(this).val();
        }
    }).toArray();

    //alert(listaArrayObtenido);
}

// Cargar Registro en el Registro de Perfiles
function llenarRegistroPerfiles() {

    // Obtener valores
    var Id = data[0];
    var NombrePerfil = data[1];
    var DescripcionPerfil = data[2];
    var Estado = $.parseHTML(data[3])[0].innerHTML;

    $("#txtIDPerfil").val(Id);
    $("#txtNombrePerfil").val(NombrePerfil).prop("disabled", true);
    $('#txtDescripcionPerfil').val(DescripcionPerfil);
    $('#ddlEstado').val($("#ddlEstado option:contains('" + Estado + "')").val());

}

// Limpiar Campos
function limpiarRegistroPerfil() {

    var Estado;

    if ($('#Editar').length) {
        Estado = 'Activo';
    } else {
        Estado = 'Pendiente';
    }

    $("#txtIDPerfil").val('');
    $("#txtNombrePerfil").val('');
    $('#txtDescripcionPerfil').val('');
    $('#ddlEstado').val($("#ddlEstado option:contains('" + Estado + "')").val());

    if ($('#FnAgregar').length) {
        $("#txtNombrePerfil").prop("disabled", false);
    } else {
        $("#txtNombrePerfil").prop("disabled", true);
    }

    // Limpiar Filtros de Busqueda
    objTablaReportes.dataTable().fnFilter('');
    objTablaUsuarios.dataTable().fnFilter('');

    // Limpiar Check de Reportes y Usuarios
    $('.check').iCheck('uncheck');

    // Ordenar por ID
    objTablaReportes.dataTable().fnSort([1, 'asc']);
    objTablaUsuarios.dataTable().fnSort([1, 'asc']);

}


//// *** Evento click Asignar
//$(document).on('click', '.btn-delete', function (e) {
//    //Evitar el PostBack
//    e.preventDefault();

//    //Tabla de Asociados
//    var tablaPerfiles = $('#tbl_asociados').DataTable();
//    //primer método: eliminar la fila del datatable
//    var fila = $(this).parent().parent()[0];

//    //Eliminar de la tablaPerfiles
//    tablaPerfiles.row(fila).remove().draw();
//    //Eliminar de la tablaPerfiles

//});



// ********************************************************************************************  MEDOTOS AJAX
// *** AjaxCargarPerfiles 
function AjaxCargarPerfiles() {

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/CargarPerfiles',
        data: {},
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
            Alerta('mensajeAlerta', MensajeAlerta, 'danger', 6000);
        },
        success: function (data) {
            //console.log(data.d);
            agregarFilasDT_Perfiles(data.d);
        }
    });
}

//*** AjaxCargarReportes ( TODOS )
function AjaxCargarReportes(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/CargarReportes',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
            Alerta('mensajeAlerta', MensajeAlerta, 'danger', 6000);
        },
        success: function (data) {
            //console.log(data.d);
            agregarFilasDT_Reportes(data.d);
        }
    });
}

// *** AjaxCargarReportesPerfil ( POR PERFIL )
function AjaxCargarReportesPorPerfil(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/ReportesPorPerfil',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
            Alerta('mensajeAlerta', MensajeAlerta, 'danger', 6000);
        },
        success: function (data) {

            // Marcar los Reportes del Perfil   
            MarcarReportesPerfil(data.d);

            // Ordenar por Marcados
            objTablaReportes.dataTable().fnSort([0, 'asc']);
        }
    });
}

// *** AjaxCargarReportesPerfil ( POR PERFIL )
function AjaxCargarUsuariosPorPerfil(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/UsuariosPorPerfil',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
            Alerta('mensajeAlerta', MensajeAlerta, 'danger', 6000);
        },
        success: function (data) {

            // Marcar los Reportes del Perfil   
            MarcarUsuariosPerfil(data.d);

            // Ordenar por Marcados
            objTablaUsuarios.dataTable().fnSort([0, 'asc']);

        }
    });
}

// *** AjaxCrearPerfil
function AjaxCrearPerfil(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: "POST",
        url: paginaUrl + '/CrearPerfil',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);

            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);
        },
        success: function (data) {

            if (data.d === true) {
                //**** Carga de Perfiles / Incluyendo el Nuevo
                AjaxCargarPerfiles();

                // Mensaje
                MensajeAlerta = 'El perfil se registró correctamente!';
                Alerta('mensajeAlerta', MensajeAlerta, 'success', 2000);

                // Limpiar Formulario
                limpiarRegistroPerfil();
            }
        }
    });
}

// *** AjaxCrearPerfil
function AjaxActualizarPerfil(parametros) {

    // Convierte parametros a cadena JSON
    parametros = JSON.stringify(parametros);

    $.ajax({
        type: "POST",
        url: paginaUrl + '/ActualizarPerfil',
        data: parametros,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);

            mensajeError(MensajeAlerta, paginaUrl);

            setTimeout(function () {
                window.location = "./Login.aspx";
            }, 3000);
        },
        success: function (data) {

            if (data.d) {
                //**** Carga de Perfiles / Incluyendo el Nuevo
                AjaxCargarPerfiles();

                // Mensaje
                MensajeAlerta = 'El perfil se actualizó correctamente!';
                Alerta('mensajeAlerta', MensajeAlerta, 'success', 2000);

                // Limpiar Formulario
                limpiarRegistroPerfil();
            }
        }
    });
}

// enviarDatosAjax ListarUsuarios
function AjaxCargarUsuarios() {
    $.ajax({
        type: 'POST',
        url: paginaUrl + '/ListarUsuarios',
        data: {},
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
            Alerta('mensajeAlerta', MensajeAlerta, 'danger', 6000);
        },
        success: function (data) {
            //console.log(data.d);
            agregarFilasDT_Usuarios(data.d);
        }
    });
}



