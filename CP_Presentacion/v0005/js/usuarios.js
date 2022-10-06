
//*** Parametros Globales
var tabla, data;
var objTabla = $('#tbl_usuarios');
var paginaUrl = 'Registro_Usuarios.aspx';

var nombreReporte = 'Lista de Usuarios Report Manager';

function validaControl(tipo) {

    validacion = false;

    if (tipo === 'E') {

        if ($('#FnEditar').length) {
            validacion = true;
        }
    }

    return validacion;
}

function vistaPreviaImagen(imagen) {

    if (imagen.files && imagen.files[0]) {

        var reader = new FileReader();
        reader.onload = function (e) {
            $('#imagenUsuario').css('visibility', 'visible');
            $('#imagenUsuario').attr('src', e.target.result);
        };
        reader.readAsDataURL(imagen.files[0]);
    }

}

// Iniciar Tabla
function iniciarDataTable() {

    // inicio = new Date().getTime();
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
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                title: function () {
                    return nombreReporte;
                }
            },
            {
                extend: 'excelHtml5',
                text: "EXCEL",
                exportOptions: {
                    columns: [0, 1, 2, 3, 4, 5, 6]
                },
                title: function () {
                    return nombreReporte;
                },
                sheetName: 'Usuarios'
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
        "aaSorting": [[0, 'desc']],
        'bDestroy': true,
        "bDeferRender": true,
        "paging": false,
        "scrollY": $(document).height() - 440 + "px", //30vh

        "scrollCollapse": true,
        "scroller": true,
        "bSort": true,
        "autoWidth": false,
        "responsive": true,

        "aoColumns": [
            { "sWidth": "100px", "sClass": "textoCentrado" },
            null,
            null,
            null,
            null,
            null,
            null,
            { "bSortable": false, "visible": validaControl('E') }    //*** ACCIONES
        ]
    });
}

// Agregar Filas a la Tabla
function agregarFilasDT(data) {

    // Limpiar Tabla despues de cualquier cambio a los Usuarios
    objTabla.dataTable().fnClearTable();

    var filas = [];

    for (var i = 0; i < data.length; i++) {

        filas.push([ data[i].cod_usuario, data[i].nombre, data[i].sucursal.nombre, data[i].departamento.nombre, data[i].rol.nombre, data[i].login, spanEstado(data[i].i_estado)
                   , '<button type="button" value="Actualizar" title="Actualizar" class="btn btn-xs btn-primary btn-edit btn_datatable" data-target="#modal_actualizar" data-toggle="modal"><i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></button>&nbsp;' +
                     '<button type="button" value="Eliminar" title="Eliminar" class="btn btn-xs btn-danger btn-delete btn_datatable"><i class="fa fa-minus-square-o" aria-hidden="true"></i></button>']);
    }

    objTabla.dataTable().fnAddData(filas, false);

    // Función Condicional de Estado
    function spanEstado(estado) {

        if (estado === "Activo") {
            return '<span class="label label-success" style="font-size:9px;">' + estado + '</span>';
        }
        else {
            return '<span class="label label-danger"  style="font-size:9px;">' + estado + '</span>';
        }
    }

    //*** Dibujar la Tabla
    tabla.draw();

    //if (!$('#infoTitulo').length) {
    //    $("<div id='infoTitulo' class='dataTables_info' role='status' aria-live='polite'>Lista de Usuarios</div>").appendTo("#tbl_usuarios_wrapper .col-sm-6:first");
    //}

    //*** Tiempo Total de Ejecución
    //$("#txtDuracion").text("Duración de Carga: " + (new Date().getTime() - inicio) / 1000 + " Segundos");
}

// enviarDatosAjax ListarUsuarios
function enviarDatosAjax() {
    $.ajax({
        type: 'POST',
        url: paginaUrl + '/ListarUsuarios',
        data: {},
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            console.log(xhr.status + "\n" + xhr.responseText, "\n" + thrownError);
        },
        success: function (data) {
            //console.log(data.d);
            agregarFilasDT(data.d);
        }
    });
}


// Llamar a la funcion enviarDatosAjax al cargar documento
//Carga al Iniciar Metodo 1
$(function () {
    iniciarDataTable();
    enviarDatosAjax();
});

// *********************************************************** Evento click  Actualizar
$(document).on('click', '.btn-edit', function (e) {
    //Evitar el PostBack
    e.preventDefault();

    //console.log("Boton Actualizar")
    //console.log($(this).parent().parent().children().first().text());

    var fila = $(this).parent().parent()[0];
    data = objTabla.fnGetData(fila);

    //Llenar el Modal de Modificación de Usuarios
    llenarDataModal();
});

// Cargar datos en el Modal Actualizar
function llenarDataModal() {

    $("#ModalTxtNombre").val(data[1]);
    $('#ModalDdlSucursal').val($("#ModalDdlSucursal option:contains('" + data[2] + "')").val());
    $('#ModalDdlDepartamento').val($("#ModalDdlDepartamento option:contains('" + data[3] + "')").val());
    $('#ModalDdlRol').val($("#ModalDdlRol option:contains('" + data[4] + "')").val());
    $("#ModalTxtClave").val("        ");
    $('#ModalDdlEstado').val($("#ModalDdlEstado option:contains('" + $.parseHTML(data[6])[0].innerHTML + "')").val());
}

// *********************************** Actualizar , Cancelar y Cerrar Modal
$("#btn_Usuarios_Editar").click(function (e) {
    e.preventDefault();

    actualizarDataAjax();

    //Actualizar Tabla
    enviarDatosAjax();

    //Cerrar Modal
    cerrarModalActualizar();
});

// Btn Cerrar Modal 
$("#btnModalCancelar").click(function (e) {
    e.preventDefault();
    cerrarModalActualizar();
});

//Función Cerrar Modal Actualizar
function cerrarModalActualizar() {
    $("#modal_actualizar").modal('hide');
}


function actualizarDataAjax() {
    // JSON.stringify = Convertir a Cadena
    var obj = JSON.stringify({
        id: cod_usuario, cod_usuario_actualzar: data[0], login_actualizar: data[5], nombre: $("#ModalTxtNombre").val()
        , cod_sucursal: $("#ModalDdlSucursal").val(), cod_departamento: $("#ModalDdlDepartamento").val()
        , cod_rol: $("#ModalDdlRol").val(), clave: $("#ModalTxtClave").val(), i_estado: $("#ModalDdlEstado").val()
    });
    //console.log(obj);
    $.ajax({
        type: "POST",
        url: "Registro_Usuarios.aspx/ActualizarUsuario",
        data: obj,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (response) {
            if (response.d) {
                MensajeAlerta = 'Registro actualizado correctamente.';
                Alerta('mensajesAlerta', MensajeAlerta, 'success', 2000);
            } else {
                MensajeAlerta = 'No se pudo actualizar el Registro.';
                Alerta('mensajesAlerta', MensajeAlerta, 'danger', 2000);
            }
        }
    });
}

// ************************************************************** Evento click Eliminar ( Capturar Text Row)
$(document).on('click', '.btn-delete', function (e) {
    //Evitar el PostBack
    e.preventDefault();

    var row = $(this).closest('tr');
    var colEstado = row.find('td:eq(6)').text();

    if (colEstado !== "Inactivo") {

        //primer método: eliminar la fila del datatable
        //var row = $(this).parent().parent()[0];
        //var dataRow = objTabla.fnGetData(row);

        var parametros = new Object();

        // Parametros Default
        parametros.id = cod_usuario;
        parametros.cod_usuario = row.find('td:eq(0)').text();;
        parametros.login = row.find('td:eq(1)').text();;

        // paso 1: enviar el id al servidor por medio de ajax
        deleteDataAjax(parametros);

        // paso 2: Actualizar DataTable
        enviarDatosAjax();

    } else {
        MensajeAlerta = 'El usuario ya está inactivo!';
        Alerta('mensajesAlerta', MensajeAlerta, 'warning', 2000);
    }
});

function deleteDataAjax(data) {

    // Convierte parametros a cadena JSON
    var parametros = JSON.stringify(data);

    $.ajax({
        type: "POST",
        url: "Registro_Usuarios.aspx/EliminarUsuario",
        data: parametros,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {
            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (response) {
            if (response.d) {
                MensajeAlerta = 'Usuario inactivado correctamente!';
                Alerta('mensajesAlerta', MensajeAlerta, 'danger', 2000);
            } else {
                MensajeAlerta = 'No fue posible inactivar el registro!';
                Alerta('mensajesAlerta', MensajeAlerta, 'danger', 2000);
            }
        }
    });
}
