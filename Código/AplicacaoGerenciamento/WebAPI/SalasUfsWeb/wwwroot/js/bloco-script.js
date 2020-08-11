$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();
});

function AdicionarHardware() {
    document.getElementById("mensagem-erro-hardwares").hidden = true;
    let enderecoMac = $('#input-mac').val();
    let indice = 0;

    if (!validacoesHardwareExistente(enderecoMac)) {

        var novoHardware = new Array();
        novoHardware.push(adicionaHardwareNaTabela(indice, enderecoMac));

        let hardwares = document.getElementsByClassName('hardware-bloco');
        if (document.querySelector('.hardware-bloco')) {
            for (indice = 0; indice < hardwares.length; indice++) {

                enderecoMac = hardwares[indice].childNodes[0].childNodes[0].value;
                novoHardware.push(adicionaHardwareNaTabela(indice + 1, enderecoMac));
            }
        }

        document.getElementById('container-hardwares').innerHTML = "";
        for (var i = 0; i < novoHardware.length; i++)
            $('#container-hardwares').append(novoHardware[i]);

    }
}

function validacoesHardwareExistente(enderecoMac) {

    let hardwareExistente = false;

    if (enderecoMac.normalize('NFD').replace(/([\u0300-\u036f]|[^0-9a-zA-Z])/g, '').length == 12) {
        let hardwares = document.getElementsByClassName('hardware-bloco');
        if (document.querySelector('.hardware-bloco')) {
            for (indice = 0; indice < hardwares.length; indice++) {

                let MAC = hardwares[indice].childNodes[0].childNodes[0].value;

                if (MAC == enderecoMac) {
                    hardwareExistente = true;
                    document.getElementById("mensagem-erro-hardwares").innerText = "Um hardware com esse endereço MAC já foi adicionado!";
                    document.getElementById("mensagem-erro-hardwares").hidden = false;
                }
            }
        }
    } else {
        hardwareExistente = true;
        document.getElementById("mensagem-erro-hardwares").innerText = "Preencha o campo MAC antes de adicionar um hardware!";
        document.getElementById("mensagem-erro-hardwares").hidden = false;
    }

    return hardwareExistente;

}

function adicionaHardwareNaTabela(indice, enderecoMac) {
    let idItem = 'novo-hardware-' + indice;

    let hardware =
        '<tr id="' + idItem + '" class="hardware-bloco">' +
        '<td>' +
        '<input class="form-control" name="Hardwares[' + indice +'].MAC" hidden value="' + enderecoMac + '" />' +
        '<input class="form-control" name="Hardwares[' + indice +'].Id" hidden value="0" />'+
        '<input class="form-control" name="Hardwares[' + indice +'].BlocoId" hidden value="0" />'+
        '<input class="form-control" name="Hardwares[' + indice + '].TipoHardwareId" value="0" hidden />' +
        '<p class="form-control">' + enderecoMac + ' / Controlador de Bloco </p>' + 
        '<td>' +
        '<a id="remove-novo-hardware" onclick="removeNovoHardware(\'' + idItem + '\')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return hardware;
}


function removeNovoHardware(idItem) {

    document.getElementById(idItem).remove();

    let hardwares = document.getElementsByClassName('hardware-bloco');

    var novosHardwares = new Array();
    for (var indice = 0; indice < hardwares.length; indice++)
        novosHardwares.push(adicionaHardwareNaTabela(indice,
            hardwares[indice].childNodes[0].childNodes[0].value));

    document.getElementById('container-hardwares').innerHTML = "";
    for (var indice = 0; indice < novosHardwares.length; indice++)
        $('#container-hardwares').append(novosHardwares[indice]);
} 