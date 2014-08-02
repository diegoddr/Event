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

$(".calendario").datepicker({
    showOtherMonths: true,
    selectOtherMonths: true,
    dateFormat: 'dd/mm/yy',
    buttonImageOnly: true,
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
        document.getElementById("tnascimento").focus();
        nErro++;
    } else {
    }
}

function validarDataMenor() {
    var dataForm = (document.forms[0]["tinicio"].value).split("/");
    var hoje = new Date();
    var dataInformada = new Date(dataForm[2], dataForm[1] - 1, dataForm[0]);

    if (hoje > dataInformada) {
        document.getElementById("erroDataInicio").innerHTML = "Data não pode ser menor que hoje";
        document.getElementById("tinicio").focus();
        nErro++;
    } else {
    }
}
function validarDiferencaData() {
    var dataInicio = (document.forms[0]["tinicio"].value).split("/");
    var dataFim = (document.forms[0]["tfim"].value).split("/");
    var dataInformadaInicio = new Date(dataInicio[2], dataInicio[1] - 1, dataInicio[0]);
    var dataInformadaFim = new Date(dataFim[2], dataFim[1] - 1, dataFim[0]);

    if (dataInformadaFim < dataInformadaInicio) {
        document.getElementById("erroDataFim").innerHTML = "Data fim não pode ser menor data inicio";
        document.getElementById("tfim").focus();
        nErro++;
    } else {
    }
}
function validarSenha() {
    var senha1 = $('#tsenha1').val();
    var senha2 = $('#tsenha2').val();
    if (senha1 != senha2) {
        document.getElementById("erroSenha").innerHTML = "Senhas não conferem";
        document.getElementById("tsenha1").focus();
        nErro++;
    }
}

function validarCampos() {
    if (nErro > 0) {
        alert("Preencha corretamente os campos em vermelho!");
        nErro = 0;
    }
}

