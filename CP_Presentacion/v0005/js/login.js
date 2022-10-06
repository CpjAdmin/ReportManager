//************************************************** Recuperar Contraseña
var paginaUrl = 'Login.aspx';

var usuarioDigitado;

// mensajeErrorLogin
function mensajeErrorLogin(mensaje, titulo) {

    $.confirm({
        theme: 'material',
        title: titulo,
        content: mensaje,
        columnClass: 'medium',
        icon: 'fa fa-warning',
        type: 'red',
        draggable: true,
        dragWindowGap: 0,
        animation: 'rotate',
        closeAnimation: 'zoom',
        animationBounce: 1.5,
        escapeKey: 'cerrar',
        backgroundDismiss: false,

        //Botones del Form principal
        buttons: {
            recuperarClave: {
                text: 'Recuperar Contraseña',
                btnClass: 'btn-primary',
                action: function () {

                    // Función recuperar contraseña
                    recuperarClave();
                }
            },
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        }
    });
}
// mensajeConfirmLogin
function mensajeConfirmLogin(mensaje, titulo, theme, type, icon) {

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
            recuperarClave: {
                text: 'Recuperar Contraseña',
                btnClass: 'btn-primary',
                action: function () {

                    // Función recuperar contraseña
                    recuperarClave();
                }
            },
            cerrar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        }
    });
}
// recuperarClave
function recuperarClave() {

    //Confirmación de recuperación de contraseña
    $.confirm({

        theme: 'modern',
        title: 'Recuperar Contraseña!',
        content: '' +
        '<form action="" class="formName">' +
        '<div class="form-group">' +
        '<label>Ingrese su usuario de REPORT MANAGER <b style="color:darkred;">(Ejemplo: ASALAZAR)</b></label>' +
        '<input type="text" id="usuarioIngresado" value="' + $('#usuario').val() + '"placeholder="Ingrese el Usuario" class="login InputUser form-control" required style="text-transform:uppercase" />' +
        '</div>' +
        '</form>',
        columnClass: 'medium',
        icon: 'fa fa-envelope',
        type: 'blue',
        draggable: true,
        dragWindowGap: 0,
        animation: 'rotate',
        closeAnimation: 'zoom',
        animationBounce: 1.5,
        escapeKey: 'cancelar',
        backgroundDismiss: false,

        //Botones del Form Recuperar Contraeña
        buttons: {
            formSubmit: {
                text: 'Enviar',
                btnClass: 'btn-primary',

                //Acción del Botón Enviar
                action: function () {

                    usuarioDigitado = $("#usuarioIngresado").val();

                    if (!usuarioDigitado) {

                        mensajeAlerta = 'Ingrese un usuario valido!';
                        titulo = 'Usuario Incorrecto!';

                        //Mostar Mensaje
                        mensaje('material', mensajeAlerta, titulo, 'fa fa-user-times', 'blue', 'medium');

                        return false;
                    } else {

                        recuperacionClaveUsuario(usuarioDigitado);
                    }
                }
            },
            //Cerrar
            cancelar: {
                text: 'Cerrar',
                btnClass: 'btn-red'
            }
        },
        onContentReady: function () {
            // Enlazar a eventos
            var jc = this;

            this.$content.find('form').on('submit', function (e) {
                // Si el usuario envía el formulario presionando Enter en el campo.
                e.preventDefault();
                jc.$$formSubmit.trigger('click'); // reference the button and click it
            });

            $('.InputUser').bind('change keyup paste', function () {
                soloLetras($(this));
            });
        }
    });
}
// recuperacionClaveUsuario
function recuperacionClaveUsuario(usuarioDigitado) {

    // Guardar parámetros y usar Libreria de JSON
    var parametros = new Object();

    // Parametros Default
    parametros.usuario = usuarioDigitado;

    data = JSON.stringify(parametros);

    $.ajax({
        type: 'POST',
        url: paginaUrl + '/EnviarClaveEmail',
        data: data,
        contentType: 'application/json; charset=utf-8',
        error: function (xhr, ajaxOptions, thrownError) {

            mensajeAlerta = xhr.statusText + '\n' + xhr.responseText + '\n' + thrownError;
            titulo = 'Alerta - Envio de Contraseña';

            //Mostar Mensaje
            mensaje('material', mensajeAlerta, titulo, 'fa fa-warning', 'red', 'large');

        },
        success: function (data) {

            if (data.d === "Enviado") {

                mensajeAlerta = '<h3>La clave del usuario <strong style="color:skyBlue;"> ' + usuarioDigitado + '</strong> se envió por correo!</h3>';
                titulo = 'Contraseña Enviada';

                //Mostar Mensaje
                mensaje('supervan', mensajeAlerta, titulo, 'fa fa-thumbs-up', 'green', 'large')

            } else if (data.d === "ErrorEnvio") {

                mensajeAlerta = "Error - La contraseña no se envío! Contacte al administrador del sistema.";
                titulo = 'Alerta - Envio de Contraseña';

                //Mostar Mensaje
                mensaje('material', mensajeAlerta, titulo, 'fa fa-warning', 'blue', 'medium')

            } else if (data.d === "ErrorUsuario") {

                mensajeAlerta = 'El usuario ingresado no existe en la base de datos! ';
                titulo = 'Usuario NO existe!';

                //Mostar Mensaje
                mensaje('material', mensajeAlerta, titulo, 'fa fa-user-times', 'blue', 'medium')

            } else {

                mensajeAlerta = "Error - La contraseña no se envío!" + data.d;
                titulo = 'Alerta - Envio de Contraseña';

                //Mostar Mensaje
                mensaje('material', mensajeAlerta, titulo, 'fa fa-warning', 'red', 'medium')
            }
        }
    });
}

$(document).ready(function () {

    //*** 1.1 - Bloqueo de Inspección de Elementos ( ACTIVAR ANTES DE PUBLICAR )
    bloqueoInspectElement();

    //*** tooltip
    $('[data-toggle="tooltip"]').tooltip();
    $('[data-toggle="tooltip"]').tooltip().off("focusin focusout");

    $('.InputUser').bind('change keyup paste', function () {
        soloLetras($(this));
    });
});

//Solo Letras
function soloLetras(key) {
    var tecla = key;
    tecla.val(tecla.val().replace(/[^a-zA-Z]/g, ''));
}



