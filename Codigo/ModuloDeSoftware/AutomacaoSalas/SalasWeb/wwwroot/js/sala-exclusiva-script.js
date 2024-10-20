$(document).ready(function () {

    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();

    if (document.querySelector('.responsavel-sala'))
        document.getElementById("btn-create-sala-exclusiva").disabled = false;
    else
        document.getElementById("btn-create-sala-exclusiva").disabled = true;
});



function adicionarNovoResponsavel() {
    let idUser = $('#select-usuario').val();
    let texto = $("#select-usuario option:selected").text();
    let indice = 0;

    if (!verificaResponsavelRepetido(idUser)) {

        let novosResponsaveis = new Array();
        novosResponsaveis.push(adicionarResponsavelNaTabela(indice, idUser, texto));

        if (document.querySelector('.responsavel-sala')) {
            let responsaveis = document.getElementsByClassName('responsavel-sala');
            for (indice = 0; indice < responsaveis.length; indice++) {

                idUser = responsaveis[indice].childNodes[0].childNodes[0].defaultValue;
                texto = responsaveis[indice].childNodes[0].childNodes[1].innerText;

                novosResponsaveis.push(adicionarResponsavelNaTabela(indice + 1, idUser, texto));
            }
        }

        document.getElementById('container-responsaveis').innerHTML = "";
        for (var i = 0; i < novosResponsaveis.length; i++)
            $('#container-responsaveis').append(novosResponsaveis[i]);


        document.getElementById("btn-create-sala-exclusiva").disabled = false;
    }
}


function adicionarResponsavelNaTabela(indice, idUser, texto) {
    let idItem = 'novo-responsavel-' + indice;
    let responsavel =
        '<tr id="' + idItem + '" class="responsavel-sala">' +
        '<td>' +
        '<input class="form-control" name="Responsaveis[' + indice + '].Id" hidden  value = "' + idUser + '"/>' +
        '<p class="text-overflow form-control" >' + texto + '</p>' +
        '<input class="form-control" name="Responsaveis[' + indice + '].Nome" hidden value="' + texto.split('|')[1] + '" />' +
        '<input class="form-control" name="Responsaveis[' + indice + '].Cpf" hidden value="' + texto.split('|')[0]+'" />' +
        '</td>' +
        '<td>' +
        '<a id="remove-novo-horario"  onclick="removeNovoResponsavel(' + '\'' + idItem + '\'' + ')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return responsavel;

}

function removeNovoResponsavel(idItem) {

    document.getElementById(idItem).remove();

    let responsaveis = document.getElementsByClassName('responsavel-sala');

    var novosResponsaveis = new Array();
    for (var indice = 0; indice < responsaveis.length; indice++)
        novosResponsaveis.push(adicionarResponsavelNaTabela(indice,
            responsaveis[indice].childNodes[0].childNodes[0].defaultValue,
            responsaveis[indice].childNodes[0].childNodes[1].innerText));

    document.getElementById('container-responsaveis').innerHTML = "";
    for (var i = 0; i < novosResponsaveis.length; i++)
        $('#container-responsaveis').append(novosResponsaveis[i]);

    if (!document.querySelector('.responsavel-sala'))
        document.getElementById("btn-create-sala-exclusiva").disabled = true;
}

function verificaResponsavelRepetido(idUser) {

    let responsaveis = document.getElementsByClassName('responsavel-sala');

    for (indice = 0; indice < responsaveis.length; indice++) {
        if (idUser == responsaveis[indice].childNodes[0].childNodes[0].defaultValue)
            return true;
    }
    return false;
}