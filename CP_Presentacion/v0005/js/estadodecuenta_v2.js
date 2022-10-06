//Variables Globales
var paginaUrl = 'Estados_de_Cuenta.aspx';
var nombreDirectorio;

// Iniciar la creación de la Tabla
$(document).ready(function () {

    $('input').iCheck({
        checkboxClass: 'icheckbox_futurico',
        radioClass: 'iradio_futurico',
        increaseArea: '25%'            // optional
    });

    //Nombre de Directorio = Usuario
    nombreDirectorio = formatDate(new Date());
    login = $('#Login').val().trim();
});

//*** Evento click en check 
$('#ckUbicacion').on('ifChecked', function (event) {

    if ($('#ckUbicacion').is(':checked')) {
        $("#txtDirectorio").removeAttr('disabled');
    } 
});

$('#ckUbicacion').on('ifUnchecked', function (event) {

        $("#txtDirectorio").val();
        $("#txtDirectorio").attr("disabled", "disabled");
        //Disparar el evento de cambio para que cambie la ruta
        $("#txtDirectorio").trigger("change");
});

//Label lbRuta
$("#ddlDisco, #txtDirectorio ").on("change paste keyup", function () {
    $("#lbRuta").text(" " + $("#ddlDisco").val() + "Estados\\" + login + "\\" + $("#txtDirectorio").val());
});

$('#modalUbicacion').on('show.bs.modal', function (e) {
    //Directorio
    $("#txtDirectorio").val(nombreDirectorio);

    //Label Ruta 
    $("#lbRuta").text(" " + $("#ddlDisco").val() + "Estados\\" + login + "\\" + $("#txtDirectorio").val());
    $("#txtFormato").val("PDF");
});

$(function () {
    $("#btnFormato").on("click", 'li a', function () {
        $("#txtFormato").val($(this).text());
    });
});

$(function () {
    $("#btnModalGenerar").on("click", function () {
        $("#txtFormato").removeAttr('disabled');
    });
});

// Controles Buscar Asociado
$(function () {
    //Buscar Por 
    $("#txtBuscar").val("IDENTIFICACIÓN");
});

$(function () {
    $("#btnBuscar").on("click", 'li a', function () {
        $("#txtBuscar").val($(this).text());
    });
});

// ************** Carga del DropDownList Provincia y Canton

// 1   - Cargar Cantones
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
        //Convierte a cadena JSON
        parametros = JSON.stringify(parametros);
        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarCantones',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: IniciarCargaCantones,
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// 1.1 - Cargar Cantones
function IniciarCargaCantones(response) {
    LlenarControl(response.d, $("#ddlCanton"));
}

// 2   -  Carga Distritos
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
        //Convierte a cadena JSON
        parametros = JSON.stringify(parametros);

        $.ajax({
            type: "POST",
            url: paginaUrl + '/CargarDistritos',
            data: parametros,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: IniciarCargaDistritos,
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// 2.1   -  Carga Distritos
function IniciarCargaDistritos(response) {
    LlenarControl(response.d, $("#ddlDistrito"));
}

// 3   - Cargar Centros
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
            success: IniciarCargaInstituciones,
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// 3.1 - Cargar Centros
function IniciarCargaInstituciones(response) {
    LlenarControl(response.d, $("#ddlInstitucion"));
}

// 4  -  Carga LugaresTrabajo
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
            success: IniciarCargaLugaresTrabajo,
            error: function (xhr, ajaxOptions, thrownError) {

                MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
                mensajeError(MensajeAlerta, paginaUrl);
            }
        });
    }
}

// 4.1   -  Carga LugaresTrabajo
function IniciarCargaLugaresTrabajo(response) {

    LlenarControl(response.d, $("#ddlLugarTrabajo"));
}

// Llenar LlenarControl
function LlenarControl(list, control) {

    if (list.length > 0) {
        control.removeAttr("disabled");
        control.empty().append('<option selected="selected" value="0">Seleccione un valor</option>');

        //Canton
        if (control.attr('id') === "ddlCanton") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_canton']).html(this['nombre']));
            });
        }
        //Distrito
        else if (control.attr('id') === "ddlDistrito") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_distrito']).html(this['nombre']));
            });
        }
        //Institucion
        else if (control.attr('id') === "ddlInstitucion") {
            $.each(list, function () {
                control.append($("<option></option>").val(this['cod_institucion']).html(this['nombre']));
            });
        }
        //LugarTrabajo
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