//$(document).su(function () {
//    //AdjustViewButton();
//});

//function AdjustViewButton() {
//    var botonImprimir = document.getElementById("ReportViewer1_ctl08_ctl00");
//    botonImprimir.value = "Ver Informe";
//}

$(document).ready(function () {

    //Botones ComboBox
    //$("input[value*='rbTrue']").   // Comienza con: ^=  / Igual a : *=
    $("input[value*='rbTrue']").siblings("label").text(" SI ");
    $("input[value*='rbFalse']").siblings("label").text(" NO ");
    
});

//Evento que se ejecuta despues de que se recarga el ReportViewer
(function () {

    var origOpen = XMLHttpRequest.prototype.open;

    XMLHttpRequest.prototype.open = function (method, url) {
        this.addEventListener('load', function () {

            $("input[value*='rbTrue']").siblings("label").text(" SI ");
            $("input[value*='rbFalse']").siblings("label").text(" NO ");
        });

        this.addEventListener('error', function () {
            console.log('XHR errored out', method, url);
        });

        origOpen.apply(this, arguments);
    };
})();

$(document).keydown(function (e) {
    if (e.which === 123) {
        return false;
    }
});

//$(document).bind("contextmenu", function (e) {
//    e.preventDefault();

//});