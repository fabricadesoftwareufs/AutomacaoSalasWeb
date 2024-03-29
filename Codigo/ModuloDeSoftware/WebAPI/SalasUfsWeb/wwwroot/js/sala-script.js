﻿$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();
});


function AdicionarHardware() {

    let enderecoMac = $('#input-mac').val();
    let tipoHardwareId = $('#select-tipo-hardware').val();
    let tipoHardwareTexto = $('#select-tipo-hardware option:selected').text();
    let enderecoIp = $('#input-ip').val();
    let indice = 0;

    if (!validacoesHardwareExistente(enderecoMac, enderecoIp, tipoHardwareId)) {
        document.getElementById("mensagem-erro-hardwares").hidden = true;
        var novoHardware = new Array();
        novoHardware.push(adicionaHardwareNaTabela(indice, enderecoMac, tipoHardwareId, tipoHardwareTexto, tipoHardwareId == 1 ? enderecoIp : ""));

        let hardwares = document.getElementsByClassName('hardware-sala');
        if (document.querySelector('.hardware-sala')) {
            for (indice = 0; indice < hardwares.length; indice++) {

                enderecoMac = hardwares[indice].childNodes[0].childNodes[1].value;
                tipoHardwareId = hardwares[indice].childNodes[0].childNodes[0].value;
                tipoHardwareTexto = hardwares[indice].childNodes[0].childNodes[2].value;
                enderecoIp = hardwares[indice].childNodes[0].childNodes[3].value;

                novoHardware.push(adicionaHardwareNaTabela(indice + 1, enderecoMac, tipoHardwareId, tipoHardwareTexto, tipoHardwareId == 1 ? enderecoIp : ""));
            }
        }

        document.getElementById('container-hardwares').innerHTML = "";
        for (var i = 0; i < novoHardware.length; i++)
            $('#container-hardwares').append(novoHardware[i]);

    }
}

function validacoesHardwareExistente(enderecoMac, enderecoIp, tipoHardwareId) {

    let hardwareExistente = false;

    if (tipoHardwareId != 1 || (enderecoIp != undefined && enderecoIp.length > 0)) {

        if (enderecoMac.normalize('NFD').replace(/([\u0300-\u036f]|[^0-9a-zA-Z])/g, '').length == 12) {
            let hardwares = document.getElementsByClassName('hardware-sala');
            if (document.querySelector('.hardware-sala')) {
                for (indice = 0; indice < hardwares.length; indice++) {

                    let MAC = hardwares[indice].childNodes[0].childNodes[1].value;
                    let ip = hardwares[indice].childNodes[0].childNodes[3].value;

                    if (MAC == enderecoMac) {
                        hardwareExistente = true;
                        document.getElementById("mensagem-erro-hardwares").innerText = "Um hardware com esse endereço MAC já foi adicionado!";
                        document.getElementById("mensagem-erro-hardwares").hidden = false;
                    }

                    if (ip == enderecoIp && tipoHardwareId == 1) {
                        hardwareExistente = true;
                        document.getElementById("mensagem-erro-hardwares").innerText = "Um hardware com esse endereço IP já foi adicionado!";
                        document.getElementById("mensagem-erro-hardwares").hidden = false;
                    }
                }
            }
        } else {
            hardwareExistente = true;
            document.getElementById("mensagem-erro-hardwares").innerText = "Preencha o MAC corretamente antes de adicionar um hardware!";
            document.getElementById("mensagem-erro-hardwares").hidden = false;
        }
    } else {
        hardwareExistente = true;
        document.getElementById("mensagem-erro-hardwares").innerText = "Preencha o campo IP corretamente antes de adicionar um hardware!";
        document.getElementById("mensagem-erro-hardwares").hidden = false;
    }

    return hardwareExistente;

}

function adicionaHardwareNaTabela(indice, enderecoMac, tipoHardwareId, tipoHardwareTexto, enderecoIp) {
    let idItem = 'novo-hardware-' + indice;

    let hardware =
        '<tr id="' + idItem + '" class="hardware-sala">' +
        '<td>' +
        '<input class="form-control" name="HardwaresSala[' + indice + '].TipoHardwareId.Id" hidden value="' + tipoHardwareId + '" />' +
        '<input class="form-control" name="HardwaresSala[' + indice + '].MAC" hidden value="' + enderecoMac + '" />' +
        '<input class="form-control" name="HardwaresSala[' + indice + '].TipoHardwareId.Descricao" value="' + tipoHardwareTexto + '" hidden/>' +
        '<input class="form-control" name="HardwaresSala[' + indice + '].Ip" value="' + enderecoIp + '" hidden />' +
        '<p class="text-overflow form-control">' + enderecoMac + ' | ' + (enderecoIp.length > 0 ? enderecoIp + ' | ' : '')  + tipoHardwareTexto + '</p>' +
        '</td>' +
        '<td>' +
        '<a id="remove-novo-hardware" onclick="removeNovoHardware(\'' + idItem + '\')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return hardware;
}


function removeNovoHardware(idItem) {

    document.getElementById(idItem).remove();

    let hardwares = document.getElementsByClassName('hardware-sala');

    var novosHardwares = new Array();
    for (var indice = 0; indice < hardwares.length; indice++)
        novosHardwares.push(adicionaHardwareNaTabela(indice,
            hardwares[indice].childNodes[0].childNodes[1].value,
            hardwares[indice].childNodes[0].childNodes[0].value,
            hardwares[indice].childNodes[0].childNodes[2].value,
            hardwares[indice].childNodes[0].childNodes[3].value));

    document.getElementById('container-hardwares').innerHTML = "";
    for (var indice = 0; indice < novosHardwares.length; indice++)
        $('#container-hardwares').append(novosHardwares[indice]);
} 