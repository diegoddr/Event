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
