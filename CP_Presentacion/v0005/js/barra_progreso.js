(function (root, factory) {
    'use strict';

    if (typeof define === 'function' && define.amd) {
        define(['jquery'], function ($) {
            return (root.dialogoProgreso = factory($));
        });
    }
    else {
        root.dialogoProgreso = root.dialogoProgreso || factory(root.jQuery);
    }

}(this, function ($) {
    'use strict';

	/**
	 * Constructor Dialog DOM
	 */
    function contructorDialogo($dialogo) {
        // Eliminar la encarnación anterior del diálogo
        if ($dialogo) {
            $dialogo.remove();
        }
        return $(
            '<div class="modal fade"  id="modalCargando" data-backdrop="static" data-keyboard="false" tabindex="-1" role="dialog" aria-hidden="true" style="padding-top:15%; overflow-y:visible;">' +
            '<div class="modal-dialog modal-m">' +
            '<div class="modal-content">' +
            '<div class="modal-header"><h3 style="margin:0;"></h3></div>' +
            '<div class="modal-body">' +
            '<div class="progress progress-striped active" style="margin-bottom:0;"><div class="progress-bar" style="width: 100%"></div></div>' +
            '</div>' +
            '</div></div></div>'
        );
    }

    // Dialog object
    var $dialogo, config, cache = {};

    return {
		/**
		* Abre nuestro diálogo
        * @param mensaje Mensaje personalizado
        * @param opciones Opciones personalizadas:
        * opciones.headerText - si la opción está configurada en boolean false,
        * ocultará el encabezado y el "mensaje" se establecerá en un párrafo encima de la barra de progreso.
        * Cuando headerText es una cadena no vacía, "mensaje" se convierte en un contenido
        * encima de la barra de progreso y la cadena del texto del encabezado se establecerá como un texto dentro del H3;
        * opciones.headerSize - esto generará un encabezado correspondiente al número de tamaño. Me gusta <h1>, <h2>, <h3> etc;
        * opciones.headerClass - clase (s) extra para la etiqueta del encabezado;
        * opciones.dialogSize - postfix bootstrap para tamaño de diálogo, p. "sm", "m";
        * opciones.progressType - postfix bootstrap para el tipo de barra de progreso, p. "éxito", "advertencia";
        * opciones.contentElement: determina la etiqueta del elemento de contenido.
        * Por defecto es "p", que generará una etiqueta <p>;
        * opciones.contentClass - clase (s) extra para la etiqueta de contenido.
		 */
        show: function (mensaje, opciones) {
            // Assigning defaults
            if (typeof opciones === 'undefined') {
                opciones = {};
            }
            if (typeof mensaje === 'undefined') {
                mensaje = 'Cargando';
            }
            var ajustes = $.extend({
                headerText: '',
                headerSize: 3,
                headerClass: 'modalCenter',
                dialogSize: 'm',
                progressType: '',
                contentElement: 'p',
                contentClass: 'content',
                rtl: true,
                timer: 500,   // Útil para la función animada
                timeout: 0,   // Útil para la función animada
                onHide: null  // Esta devolución de llamada se ejecuta después de que el cuadro de diálogo estaba oculto
            }, opciones),
                $headerTag, $contentTag;
            config = ajustes;
            $dialogo = contructorDialogo($dialogo);

            // Configurando el diálogo
            $dialogo.find('.modal-dialog').attr('class', 'modal-dialog').addClass('modal-' + ajustes.dialogSize);
            $dialogo.find('.progress-bar').attr('class', 'progress-bar');
            if (ajustes.progressType) {
                $dialogo.find('.progress-bar').addClass('progress-bar-' + ajustes.progressType);
            }
            if (ajustes.rtl) {
                $dialogo.find('.progress-bar').css('float', 'left').end().attr('dir', 'rtl')
            }
            // Generar etiqueta de encabezado
            $headerTag = $('<h' + ajustes.headerSize + ' />');
            $headerTag.css({ 'margin': 0});
            if (ajustes.headerClass) {
                $headerTag.addClass(ajustes.headerClass);
            }

            // Generar etiqueta de contenido
            $contentTag = $('<' + ajustes.contentElement + ' />');
            if (ajustes.contentClass) {
                $contentTag.addClass(ajustes.contentClass);
            }

            if (ajustes.headerText === false) {
                $contentTag.html(mensaje);
                $dialogo.find('.modal-body').prepend($contentTag);
            }
            else if (ajustes.headerText) {
                $headerTag.html(ajustes.headerText);
                $dialogo.find('.modal-header').html($headerTag).show();

                $contentTag.html(mensaje);
                $dialogo.find('.modal-body').prepend($contentTag);
            }
            else {
                $headerTag.html(mensaje);
                $dialogo.find('.modal-header').html($headerTag).show();
            }

            // Agregar devoluciones de llamada
            if (typeof ajustes.onHide === 'function') {
                $dialogo.off('hidden.bs.modal').on('hidden.bs.modal', function () {
                    ajustes.onHide.call($dialogo);
                });
            }
            // Dialogo de apertura
            $dialogo.modal();
        },

		/**
		 * Cierra el diálogo
		 */
        hide: function () {
            if (typeof $dialogo !== 'undefined') {
                $dialogo.modal('hide');
            }
        },


        mensaje: function (newmensaje) {
            if (typeof $dialogo !== 'undefined') {
                if (typeof newmensaje !== 'undefined') {
                    return $dialogo.find('.modal-header>h' + config.headerSize).html(newmensaje);
                }
                else {
                    return $dialogo.find('.modal-header>h' + config.headerSize).html();
                }
            }
        }
		/**
		 * Animar el mensaje cada período es igual a 'temporizador',
         * e inicia esta animación después de que un retraso sea igual a 'timeout'
         *
         * @param mensajes pueden ser:
         * - string: será una matriz, es decir: mensajes = "waitingings" -> mensajes = ["waiting ...", "waiting ....", "" waiting "......"]
         * - array
         * - función
         * @param timer periodo
         * @ param timeout si es 0 -> comienza inmediatamente
         *
		 * */

        , animate: function (mensajes, timer, timeout) {
            timer = timer || config.timer;
            timeout = timeout || config.timeout;

            mensajes = mensajes || $dialogo.find('.modal-header>h' + config.headerSize).html();
            cache.animate = cache.animate || [];
            if (typeof mensajes === 'string') {


                mensajes = ['..', '...', '....'].map(function (e) {
                    return e + mensajes;
                });
            }

            if (typeof mensajes === 'object' && mensajes instanceof Array) {
                var old = mensajes;
                mensajes = function (container) {
                    var current = old.indexOf(container.html());
                    if (current < 0) {
                        container.html(old[0]);
                    } else {
                        var indx = (current + 1 >= old.length) ? 0 : current + 1
                        container.html(old[indx]);

                    }
                }

            }

            if (typeof mensajes === "function") {
                if (timeout < timer) {
                    setTimeout(function () {
                        mensajes.call($dialogo, $dialogo.find('.modal-header>h' + config.headerSize))
                    }, timeout)
                }
                var job = setInterval(function () {
                    mensajes.call($dialogo, $dialogo.find('.modal-header>h' + config.headerSize))
                }, timer);
                cache.animate.push(job);
                return job;
            }



        },
		/**
		* detener el trabajo con la identificación especificada.
        * si no se especifica una identificación, stopAnimate detendrá el último trabajo en ejecución.
        * @ param id
        */
        stopAnimate: function (id) {
            id = id || cache.animate[cache.animate.length - 1];
            clearInterval(id);
            delete cache.animate[cache.animate.indexOf(id)];
            return $dialogo;
        },
		/**
		* @param percentOrCurrent
        * @param total
        * Llamar con:
        * - Sin Argumento -> obtener el progreso actual
        * - Un argumento  -> "percentOrCurrent" es el porcentaje de progreso
        * - Dos argumentos ---> "percentOrCurrent" es la cantidad actual, "total" es la cantidad total.
		*/
        progress: function (percentOrCurrent, total) {
            if (!arguments.length) {
                return parseInt($dialogo.find('.progress-bar')[0].style.width);
            }
            if (total) {
                percentOrCurrent = parseInt(percentOrCurrent / total * 100);
            }
            $dialogo.find('.progress-bar').css('width', percentOrCurrent + '%');
            return $dialogo;
        }
    };

}));
