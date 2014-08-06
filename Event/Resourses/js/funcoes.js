var nErro = 0;

var masks = ['(00) 00000-0000', '(00) 0000-00009'],
    maskBehavior = function (val, e, field, options) {
        return val.length > 14 ? masks[0] : masks[1];
    };

$('.telefone').mask(maskBehavior, {
    onKeyPress:
        function (val, e, field, options) {
            field.mask(maskBehavior(val, e, field, options), options);
        }
});
$('.data').mask('00/00/0000');
$('.time').mask('00:00');

$(".calendario").datepicker({
    showOtherMonths: true,
    selectOtherMonths: true,
    dateFormat: 'dd/mm/yy',
    buttonImageOnly: true,
    showOn: "button",
    buttonImage: "/Images/datepicker.gif",
    dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado', 'Domingo'],
    dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
    dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
    monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
    monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez']
});

function recusarConvite(id) {
    $.ajax({
        type: "POST",
        url: '/Modulo/RecusarConvite/',
        data: { id: id },
        success: function () {
            alert("Convite Recusado!");
            location.reload();
        }
    });
}

function aceitarConvite(id) {
    $.ajax({
        type: "POST",
        url: '/Modulo/AceitarConvite/',
        data: { id: id },
        success: function() {
            alert("Convite Aceito!");
            location.reload();
        }
    });
}

function validarDataMaior() {
    var dataForm = (document.forms[0]["tnascimento"].value).split("/");
    var hoje = new Date();
    var dataInformada = new Date(dataForm[2], dataForm[1] - 1, dataForm[0]);

    if (hoje < dataInformada) {
        document.getElementById("erroData").innerHTML = "Data não pode ser maior que hoje";
        //document.getElementById("tnascimento").focus();
        nErro++;
    } else {
        document.getElementById("erroData").innerHTML = "";
    }
}

function validarDataMenor() {
    var dataForm = (document.forms[0]["tDataInicio"].value).split("/");
    var hoje = new Date();
    var dataInformada = new Date(dataForm[2], dataForm[1] - 1, dataForm[0]);

    if (hoje > dataInformada) {
        document.getElementById("erroDataInicio").innerHTML = "Data não pode ser menor que hoje";
        //document.getElementById("tDataInicio").focus();
        nErro++;
    } else {
        document.getElementById("erroDataInicio").innerHTML = "";
    }
}
function validarDiferencaData() {
    var dataInicio = (document.forms[0]["tDataInicio"].value).split("/");
    var dataFim = (document.forms[0]["tfim"].value).split("/");
    var dataInformadaInicio = new Date(dataInicio[2], dataInicio[1] - 1, dataInicio[0]);
    var dataInformadaFim = new Date(dataFim[2], dataFim[1] - 1, dataFim[0]);

    if (dataInformadaFim < dataInformadaInicio) {
        document.getElementById("erroDataFim").innerHTML = "Data fim não pode ser menor data inicio";
        //document.getElementById("tfim").focus();
        nErro++;
    } else {
        document.getElementById("erroDataFim").innerHTML = "";
    }
}
function validarSenha(e) {
    var senha1 = $('#tsenha1').val();
    var senha2 = $('#tsenha2').val();
    if (senha1 != senha2) {
        document.getElementById("erroSenha").innerHTML = "Senhas não conferem";
        //document.getElementById("tsenha1").focus();
        nErro++;
        e.preventDefault();
    } else {
        document.getElementById("erroSenha").innerHTML = "";
    }
}

function validarHoraInicio() {
    var hrs = (document.forms[0].tinicio.value.substring(0, 2));
    var min = (document.forms[0].tinicio.value.substring(3, 5));
    if ((hrs < 00) || (hrs > 23) || (min < 00) || (min > 59)) {
        document.getElementById("erroHoraInicio").innerHTML = "Hora invalida";
        //document.getElementById("tinicio").focus();
        nErro++;
    } else {
        document.getElementById("erroHoraInicio").innerHTML = "";
    }
}
function validarHoraFim() {
    var hrs = (document.forms[0].tfim.value.substring(0, 2));
    var min = (document.forms[0].tfim.value.substring(3, 5));
    if ((hrs < 00) || (hrs > 23) || (min < 00) || (min > 59)) {
        document.getElementById("erroHoraFim").innerHTML = "Hora invalida";
        //document.getElementById("tfim").focus();
    } else {
        document.getElementById("erroHoraFim").innerHTML = "";
    }
}
function validarDiferencaHora() {
    var hrsInicio = (document.forms[0].tinicio.value.substring(0, 2));
    var minInicio = (document.forms[0].tinicio.value.substring(3, 5));
    var hrsFim = (document.forms[0].tfim.value.substring(0, 2));
    var minFim = (document.forms[0].tfim.value.substring(3, 5));
    if ((hrsInicio >= hrsFim && minInicio > minFim) || hrsInicio > hrsFim) {
        
            document.getElementById("erroDiferencaHora").innerHTML = "Hora fim deve ser após hora inicio";
            //document.getElementById("tinicio").focus();
         
    } else {
        document.getElementById("erroDiferencaHora").innerHTML = "";
        document.getElementById("erroHoraFim").innerHTML = "";
    }
    
}

function validarNumero(e) {
    var tecla = (window.event) ? event.keyCode : e.which;
    if ((tecla > 47 && tecla < 58)) return true;
    else {
        if (tecla == 8 || tecla == 0) return true;
        else return false;
    }
}
function validarDataModulo(id) {
    $.ajax({
        type: "GET",
        url: '/Modulo/validarDataModulo/',
        data: { id: id },
        success: function (res) {
            var dataForm = (document.forms[0]["tDataInicio"].value).split("/");
            var dataInformada = new Date(dataForm[2], dataForm[1] - 1, dataForm[0]);
            var dInicio = (res.dataInicio).split("/");
            var inicio = new Date(dInicio[2], dInicio[1] - 1, dInicio[0]);
            var dFim = (res.dataFim).split("/");
            var fim = new Date(dFim[2], dFim[1] - 1, dFim[0]);
            if (dataInformada < inicio || dataInformada > fim) {
                document.getElementById("erroDataInicio").innerHTML = "Data deve ser no periodo do evento";
            } else {
                document.getElementById("erroDataInicio").innerHTML = "";
            }

        }
    });
}



