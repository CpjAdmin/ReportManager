// *** Variables Globales
var paginaUrl = "Login.aspx";
//************************************
var rol = $("#Rol").val();
var login = $("#Login").val();
var cod_usuario = $("#UsuarioID").val();
//************************************
var clavesIguales;
var modalCambiarClave = $("#modal_cambiarClave");
var sistema = "";

// ****************************************************************************************************************** ( INICIO CAMBIO CLAVE )
// *** Iniciar Acciones
$(document).ready(function () {

    //*** 1.1 - Bloqueo de Inspección de Elementos ( ACTIVAR ANTES DE PUBLICAR )
    //bloqueoInspectElement();

    // SIC
    $("#btnSICCambiarClave").click(function (e) {
        e.preventDefault();

        sistema = $(this).attr('name');

        //Llenar el Modal
        llenarDataModalCambioClave();

    });
    // REPORT MANAGER
    $("#btnRMCambiarClave").click(function (e) {
        e.preventDefault();

        sistema = $(this).attr('name');

        //Llenar el Modal
        llenarDataModalCambioClave();
    });

    // CPC
    $("#btnCPCCambiarClave").click(function (e) {

        e.preventDefault();

        sistema = $(this).attr('name');

        //Llenar el Modal
        llenarDataModalCambioClave();
    });

    // CPC
    $("#btnCENDEISSSCambiarClave").click(function (e) {

        e.preventDefault();

        sistema = $(this).attr('name');

        //Llenar el Modal
        llenarDataModalCambioClave();
    });

    // Actualizar Clave
    $("#btnModalClaveActualizar").click(function (e) {

        e.preventDefault();

        if (clavesIguales) {

            //Actualizar Clave
            actualizarClaveAjax(sistema);

            //Cerrar Modal
            cerrarModalClave();
        }
    });

    // Btn Cerrar Modal 
    $("#btnModalClaveCancelar").click(function (e) {

        e.preventDefault();
        cerrarModalClave();
    });

    // FN Modal Hideen
    modalCambiarClave.on('hidden.bs.modal', function (e) {
        //Borrar Modal
        $('#spanError').remove();
        //Borrar Campos modalCambiarClave
        modalCambiarClave.find("input[type='password']").val("");
    });

    // FN Modal Hideen
    modalCambiarClave.on('shown.bs.modal', function (e) {
        //Enfocar
        $('#ModalTxtClave1').focus();

    });

});

//******************************************************************************** FUNCIONES (INICIO)

// FN Salir
function salir() {

    window.close();
    return false;
}

// FN Llenar Modal Cambio Clave
function llenarDataModalCambioClave() {

    var titulo = "";
    var icono = "";
    // IF Control de Contenido Modal
    if (sistema === "SIC") {

        icono = "sic.png";
        titulo = "<strong>Cambio de contraseña ( " + sistema + "</strong><ul class='nav navbar-nav'><li class='dropdown user user-menu'><img src='img/iconos/" + icono + "' class='user-image'></li></ul> )";

    } else if (sistema === 'CPC') {

        icono = "cpc.png";
        titulo = "<strong>Cambio de contraseña ( " + sistema + "</strong><ul class='nav navbar-nav'><li class='dropdown user user-menu'><img src='img/iconos/" + icono + "' class='user-image'></li></ul> )";


    } else if (sistema === "REPORT MANAGER") {

        icono = "rm.jpg";
        titulo = "<strong>Cambio de contraseña ( " + sistema + "</strong><ul class='nav navbar-nav'><li class='dropdown user user-menu'><img src='img/iconos/" + icono + "' class='user-image'></li></ul> )";

    } else if (sistema === "CENDEISSS") {

        icono = "cendeisss.png";
        titulo = "<strong>Cambio de contraseña ( " + sistema + "</strong><ul class='nav navbar-nav'><li class='dropdown user user-menu'><img src='img/iconos/" + icono + "' class='user-image'></li></ul> )";
    } else {
        icono = "rm.jpg";
        titulo = "<strong>Cambio de contraseña </strong><ul class='nav navbar-nav'><li class='dropdown user user-menu'><img src='img/iconos/" + icono + "' class='user-image'></li></ul>";

    }

    //Datos Modal
    $("#modalTituloCambiarClave").html(titulo);
    $('#ModalTxtClave1').focus();

    // Verificar Claves
    verificarClaves();
}

// FN verificar Contraseñas
function verificarClaves() {

    var clave1 = $('#ModalTxtClave1');
    var clave2 = $('#ModalTxtClave2');

    var confirmacion = "Las contraseñas SI coinciden";
    var negacion = "Las contraseñas NO coinciden";

    var longitud = "La contraseña debe contener mínimo 6 carácteres.";
    var vacio = "La contraseña no puede estar vacía";

    if ($('#spanError').length > 0) {
        //Si existe el spanError se oculta
        $('#spanError').hide();
    } else {
        // Creo el elemento spanError despues de clave2
        var spanError = $('<span class="spanError" id="spanError"></span>').insertAfter(clave2);
    }

    // FN Comparar las contraseñas
    function coincidePassword() {
        var valor1 = clave1.val();
        var valor2 = clave2.val();

        // Muestra el span y remueve las clases
        spanError.show().removeClass('negacion confirmacion');

        // Condiciones
        if (valor1 !== valor2) {
            //Incorrecto
            spanError.text(negacion).addClass('negacion');
            clavesIguales = false;
        }
        if (valor1.length === 0 || valor1 === "") {
            //Incorrecto
            spanError.text(vacio).addClass('negacion');
            clavesIguales = false;
        }
        if (valor1.length < 6) {
            //Incorrecto
            spanError.text(longitud).addClass('negacion');
            clavesIguales = false;
        }
        if (valor1.length !== 0 && valor1.length >= 6 && valor1 === valor2) {
            //Confirmacion
            spanError.text(confirmacion).removeClass("negacion").addClass('confirmacion');
            clavesIguales = true;
        }
    }

    // Ejecuto la función al soltar la tecla
    clave2.keyup(function () {
        coincidePassword();
    });

    // Ejecuto la función al soltar la tecla
    clave1.keyup(function () {
        if (clave2.val().length >= 6) {
            coincidePassword();
        }
    });
}

// FN Cerrar Modal
function cerrarModalClave() {

    modalCambiarClave.modal('hide');
}

//******************************************************************************** FUNCIONES (FIN)

//******************************************************************************** AJAX (INICIO)
//*** Ajax - Actualizar Clave
function actualizarClaveAjax(sistema) {

    // JSON.stringify
    var obj = JSON.stringify({ login: login, id: cod_usuario, clave: $("#ModalTxtClave1").val(), sistema: sistema });

    $.ajax({
        type: "POST",
        url: "Inicio.aspx/ActualizarClave",
        data: obj,
        dataType: "json",
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            MensajeAlerta = xhr.status + "\n" + xhr.responseText, "\n" + thrownError;
            mensajeError(MensajeAlerta, paginaUrl);
        },
        success: function (response) {
            if (response.d) {
                // Mensaje
                MensajeAlerta = 'Contraseña de <strong> ' + sistema + ' </strong> actualizada correctamente!';
                Alerta('mensajesAlerta', MensajeAlerta, 'success', 2000);

            } else {
                // Mensaje
                MensajeAlerta = 'Contraseña de <strong> ' + sistema + ' </strong>  no se actualizó!';
                Alerta('mensajesAlerta', MensajeAlerta, 'danger', 2000);
            }
        }
    });
}

//******************************************************************************** AJAX (FIN)
//****************************************************************************************************************** ( FIN CAMBIO DE CLAVE )

// Cargar Menú de Mantenimiento
$(document).ready(function () {

    $('.dropdown-submenu a.subMenu').on("click", function (e) {
        $('.dropdown-submenu > ul').not(this).find('ul').hide();
        $(this).next('ul').toggle();
        e.stopPropagation();
        e.preventDefault();
    });
});

//Definir el Height del IFrame para PowerBI
$('#FramePBI').on('load', function () {
    $('#FramePBI').height($(document).height() - 114 + "px");
});

// Click en los link de Power BI, get del ID y se forma el SRC del iframe
$('.linkPBI').click(function () {
    window.location.href = './PowerBI_Informe.aspx?r=' + this.id + '&c=' + $(this).attr('name') + '&t=' + $(this).attr('title');
    return false;
});

// *** Cerrar Paneles al hacer Click en un Link del Menú
$(document).on('click', '.link', function (e) {

    // Cerrar Panel Izquierdo
    if (!$("body").hasClass("sidebar-collapse")) {
        $('[data-toggle="push-menu"]').pushMenu('toggle');
    }

    // Cerrar Panel Derecho
    if ($("#sidebarDerecha").hasClass("control-sidebar-open")) {
        $('#btnConfiguracion').click();
    }
});

//Eventos pushMenu
$(document).on('collapsed.pushMenu', function () {

    //Cerrar los Nodos  
    //CerrarNodos();
});

// *** MENSAJES TOAST
// *********************************************************************
// *** Mensaje Toast ( icon =  warning, error, success, info, question )
const Toast = Swal.mixin({
    toast: true,
    position: 'top-end',
    showConfirmButton: false,
    showCloseButton: true,
    timer: 5000,
    timerProgressBar: true,
    onOpen: (toast) => {
        toast.addEventListener('mouseenter', Swal.stopTimer)
        toast.addEventListener('mouseleave', Swal.resumeTimer)
    }
})
// *** Mensaje Toast ( icon =  warning, error, success, info, question )
function mensajeToast(icon, mensaje) {
    Toast.fire({
        icon: icon,
        title: mensaje
    });
}
// *** Mensaje Toast ( icon =  warning, error, success, info, question )
// *** Mensaje Toast ( position =  'top', 'top-start', 'top-end', 'center', 'center-start', 'center-end', 'bottom', 'bottom-start', 'bottom-end')
function mensajeToast2(icon, mensaje, position, timer) {
    Toast.fire({
        icon: icon,
        title: mensaje,
        timer: timer,
        position: position,
    });
}

// ********************************************************************************************* Expandir y Contraer TreeView cuando se preciona CLICK 
//*** Click Nodo Raiz ( Todos los td dentro de un tr que tiene un td con la clase NodoRaiz )
//$("#TreeView1 tr:has(td.NodoRaiz) td").children().on('click', { tipo: "NodoRaiz" }, OnNodeClicked);

////Para los nombres de los nodos
//$("#TreeView1 tr:has(td.NodoRaiz) td").children().on('click', { tipo: "NodoRaiz" }, OnNodeClicked);
////Para las imagenes
//$("#TreeView1 tr:has(td.NodoRaiz) td a").children().on('click', { tipo: "NodoRaiz" }, OnNodeClicked);

////*** Click Nodo Padre ( Todos los td dentro de un tr que tiene un td con la clase NodoPadre )
//$("#TreeView1 tr:has(td.NodoPadre) td").children().on('click', { tipo: "NodoPadre" }, OnNodeClicked);


//Funcion para Cerrar todos los nodos
//function CerrarNodos() {

//    var nodosAbiertos = $("#TreeView1 div[style='display: block;']");

//    //***** Cerrar los Nodos , Excepto el Actual
//    for (i = nodosAbiertos.length - 1; i >= 0; i--) {

//        var id = nodosAbiertos[i].id;
//        var href = $('#' + id.replace('Nodes', '') + '').attr('href');
//        document.location.href = href;
//    }

//}

//function getNumerosEnString(string) {
//    var tmp = string.split("");
//    var map = tmp.map(function (current) {
//        if (!isNaN(parseInt(current))) {
//            return current;
//        }
//    });

//    var numeros = map.filter(function (value) {
//        return value !== undefined;
//    });

//    return numeros.join("");
//}

//function OnNodeClicked(event) {

//    //Obtener tipoNodo y TreeView
//    var tipoNodo = event.data.tipo;
//    var treeView1 = $('#TreeView1');
//    var nodeIndex;
//    var NodoId;
//    var childNodesArg;
//    var NodosAbiertos;
//    var parentId;

//    //Obtener el Nodo Principal
//    if (event.target.tagName === 'IMG') {
//        // Nodo Padre
//         parentId = event.target.parentElement.id;
//        // Indice ( Numero de Nodo)
//         nodeIndex = parentId.substring(10, parentId.length);
//        if (!$.isNumeric(nodeIndex)) {
//             nodeIndex = getNumerosEnString(parentId.substring(10, parentId.length));
//        }
//        // Nodo ID
//         NodoId = "TreeView1n" + nodeIndex;
//        // ChildNodes
//         childNodesArg = NodoId + "Nodes";
//    }
//    else if (event.target.tagName === 'A') {
//        // Nodo Padre
//         NodoId = event.target.id;
//        // Indice ( Numero de Nodo)
//         nodeIndex = NodoId.substring(10, NodoId.length);

//        if (!$.isNumeric(nodeIndex)) {
//             nodeIndex = parentId.substring(10, 11);
//        }
//        // Nodo ID
//         NodoId = "TreeView1n" + nodeIndex;
//        // ChildNodes
//         childNodesArg = NodoId + "Nodes";
//    }

//    //Obtener href
//    var href = $('#' + NodoId + '').attr('href');
//    //Metodo 1
//    document.location.href = href;
//    //Metodo 2  Función = window["TreeView_ToggleNode"];
//    //TreeView_ToggleNode(TreeView1_Data, nodeIndex, document.getElementById(NodoId), ' ', document.getElementById(childNodesArg));

//    //Obtiene todos los nodos abiertos;
//    if (tipoNodo === "NodoRaiz") {
//         NodosAbiertos = $("#TreeView1 div[style='display: block;']");
//    } else {
//         NodosAbiertos = $("#TreeView1 > div[style='display: block;']").children("div[style = 'display: block;']");
//    }
//    var Nodos = treeView1.find(NodosAbiertos);
//    //***** Cerrar los Nodos , Excepto el Actual
//    var i;
//    for (i = 0; i < Nodos.length; ++i) {
//        // Verificar Nodos
//        var id = Nodos[i].id.replace("Nodes", "");
//        if (NodoId !== id) {
//             parentId = Nodos[i].id;
//             nodeIndex = parentId.charAt(parentId.length - 1);
//             childNodesArg = Nodos[i].id;
//            //Función = window["TreeView_ToggleNode"];
//            TreeView_ToggleNode(TreeView1_Data, nodeIndex, document.getElementById(parentId), ' ', document.getElementById(childNodesArg));
//        }
//    }
//    return false;
//}

