//*************************************************** Alertas Tipo:  primary, secundary, success, danger, warning, info, dark
function Alerta(id, mensaje, tipo, duracionSegundos) {

    $("#" + id + "").html('<div class="alert alert-' + tipo + ' fade in alert-dismissible">' +
        '<button class="close" aria-hidden="true" type="button" data-dismiss="alert">×</button>' +
        '<strong>' + 'ATENCIÓN' + '!</strong> ' + mensaje + '</div > ');

    window.setTimeout(function () {
        $(".alert").fadeTo(500, 0).slideUp(500, function () {
            $(this).remove();
        });
    }, duracionSegundos);

    $('.alertp .close').on("click", function (e) {
        $(this).parent().fadeTo(duracionSegundos, 0).slideUp(500);
    });
}

function mensajeError(mensaje, titulo) {

    if (mensaje.length > 500) {
        columnClass = 'large';
    } else {
        columnClass = 'medium';
    }

    $.confirm({
        theme: 'material',
        title: titulo,
        content: mensaje,
        columnClass: columnClass,
        icon: 'fa fa-warning',
        type: 'red',
        draggable: true,
        dragWindowGap: 0,
        animation: 'scale',
        closeAnimation: 'zoom',
        animationBounce: 2.5,
        escapeKey: 'cerrar',
        backgroundDismiss: false,

        //Botones del Form principal
        buttons: {
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        }
    });
}

function mensajeConfirm(mensaje, titulo, theme, type, icon) {

    var columnClass;

    if (mensaje.length > 500) {
        columnClass = 'large';
    } else {
        columnClass = 'medium';
    }

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
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        }
    });
}

function mensaje(theme, mensaje, titulo, icon, type, columnClass) {

    $.confirm({
        theme: theme,
        title: titulo,
        content: mensaje,
        columnClass: columnClass,
        icon: icon,
        type: type,
        draggable: true,
        dragWindowGap: 0,
        animation: 'rotate',
        closeAnimation: 'zoom',
        animationBounce: 1.5,
        escapeKey: 'cerrar',
        backgroundDismiss: false,

        //Botones del Form principal
        buttons: {
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red',
            }
        }
    });
}


//*** Alertas sweetAlert2 - Swal.fire
function alertaExitoMin(detalle) {
    toastr.success(detalle);
}

function alertaExitoMin2(detalle) {
    toastr.info(detalle);
}

//*** Calcular porcentaje % con base en 2 números
function calculoPorcentaje(numero_anterior, numero_actual) {

    var decremento = numero_anterior - numero_actual;
    var resultado = (decremento / numero_anterior) * 100;

    return 100 - resultado.toFixed(0);
}

//*** Formato de Fecha
function formatDate(dateVal) {

    var newDate = new Date(dateVal);
    var sMonth = padValue(newDate.getMonth() + 1);
    var sDay = padValue(newDate.getDate());
    var sYear = newDate.getFullYear();
    var sHour = newDate.getHours();
    var sMinute = padValue(newDate.getMinutes());
    var sAMPM = "AM";

    var iHourCheck = parseInt(sHour);

    if (iHourCheck > 12) {
        sAMPM = "PM";
        sHour = iHourCheck - 12;
    }
    else if (iHourCheck === 0) {
        sHour = "12";
    }

    sHour = padValue(sHour);

    //return sDay + "_" + sMonth + "_" + sYear + "_" + sHour + "_" + sMinute + "" + sAMPM;
    return sDay + "_" + sMonth + "_" + sYear;
}

function padValue(value) {
    return (value < 10) ? "0" + value : value;
}

function validarEmail(email) {
    var emailReg = /^([\w-\.]+@([\w-]+\.)+[\w-]{2,4})?$/;
    return emailReg.test(email);
}

//*** Mayusculas
function BlodMayus(e, idElement) {

    var elemento = $('#' + idElement + '');

    var CapsLock = event.getModifierState && event.getModifierState('CapsLock');

    if (CapsLock) {

        elemento.tooltip('show');

        setTimeout(function () {
            elemento.tooltip('hide');
        }, 3000);

    } else {
        elemento.tooltip('hide');
    }
}

/**
 * Devuelve una matriz con matrices de una dimensión dada.
 *
 * @param miArreglo {Array} Array a separar
 * @param n_partes {Integer} Tamaño de cada grupo
 */
function separarArray(miArreglo, n_partes) {
    var resultado = [];

    while (miArreglo.length) {
        resultado.push(miArreglo.splice(0, n_partes));
    }

    return resultado;
}

//*** (3) #7 - Función Obtener Navegador
function get_browser_info() {
    var ua = navigator.userAgent, tem, M = ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
    if (/trident/i.test(M[1])) {
        tem = /\brv[ :]+(\d+)/g.exec(ua) || [];
        return { name: 'IE ', version: tem[1] || '' };
    }
    if (M[1] === 'Chrome') {
        tem = ua.match(/\bOPR\/(\d+)/);
        if (tem !== null) { return { name: 'Opera', version: tem[1] }; }
    }
    M = M[2] ? [M[1], M[2]] : [navigator.appName, navigator.appVersion, '-?'];
    if ((tem = ua.match(/version\/(\d+)/i)) !== null) { M.splice(1, 1, tem[1]); }
    return {
        name: M[0],
        version: M[1]
    };
}

function bloqueoInspectElement() {

    //*** Bloqueo de Click Derecho
    $(document).bind("contextmenu", function (e) {
        return false;
    });

    //*** Bloqueo de Teclas
    $(document).keydown(function (event) {
        if (event.keyCode === 123) {
            // Prevenir F12
            return false;
        } else if (event.ctrlKey && event.shiftKey && event.keyCode === 73) {
            // Prevenir Ctrl+Shift+I 
            return false;
        }
    });
}


