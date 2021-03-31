$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();

    $('#codigos').hide();
    $('#ccodigo').hide();
    $('#coperacao').hide();
    $('#cbotao').hide();

    if ($('#checkbox').prop('checked')) {
        $('#codigos').fadeIn();
        $('#ccodigo').fadeIn();
        $('#coperacao').fadeIn();
        $('#cbotao').fadeIn();
    }; 
    $('#checkbox').on('click', function () {
        if ($(this).prop('checked')) {
            $('#codigos').fadeIn();
            $('#ccodigo').fadeIn();
            $('#coperacao').fadeIn();
            $('#cbotao').fadeIn();
        }
        else {
            $('#codigos').hide();
            $('#ccodigo').hide();
            $('#coperacao').hide();
            $('#cbotao').hide();
        }
    });

});

function AdicionarNovoCodigo() {

    let codigo = $('#codigo').val();
    let idOperacao = $('#operacao option:selected').val();
    let operacao = $('#operacao option:selected').text();
    let indice = 0;

        var novoCodigo = new Array();
        novoCodigo.push(adicionaCodigoNaTabela(indice, codigo, idOperacao, operacao));

        let codigos = document.getElementsByClassName('equipamento-codigo');
        if (document.querySelector('.equipamento-codigo')) {
            for (indice = 0; indice < codigos.length; indice++) {

                codigo = codigos[indice].childNodes[0].childNodes[0].value;
                idOperacao = codigos[indice].childNodes[0].childNodes[1].value;
                operacao = codigos[indice].childNodes[0].childNodes[2].value;

                novoCodigo.push(adicionaCodigoNaTabela(indice + 1, codigo, idOperacao, operacao));
            }
        }

        document.getElementById('container-codigo').innerHTML = "";
        for (var i = 0; i < novoCodigo.length; i++)
            $('#container-codigo').append(novoCodigo[i]);

        document.getElementById("btn-create-codigo").disabled = false;
    
}

function adicionaCodigoNaTabela(indice, codigo, idOperacao, operacao) {
    let idItem = 'novo-codigo-' + indice;
    let codig =
        '<tr id="' + idItem + '" class="equipamento-codigo">' +
        '<td>' +
        '<input class="form-control" type="text" name="Codigos[' + indice + '].Codigo" hidden readonly value = "' + codigo + '"/>' +
        '<input class="form-control" type="text" name="Codigos[' + indice + '].IdOperacao" hidden readonly  value = "' + idOperacao + '"/>' +
        '<input class="form-control" type="text" name="Codigos[' + indice + '].Operacao" hidden readonly  value = "' + operacao + '"/>' +

        '<div class="card text-white bg-info mb-3" style="max-width: 18rem;"><div class="card-header">' + operacao + '</div><div class="card-body"><p class="card-text">' + codigo + '</p></div></div>' +
        '</td>' +

        '<td>' +
        '<a id="remove-novo-horario"  onclick="RemoveNovoCodigo(' + '\'' + idItem + '\'' + ')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return codig;
}


function RemoveNovoCodigo(idItem) {

    document.getElementById(idItem).remove();

    let codigos = document.getElementsByClassName('equipamento-codigo');

    var novosCodigos = new Array();
    for (var indice = 0; indice < codigos.length; indice++)
        novosCodigos.push(adicionaCodigoNaTabela(indice,
            codigos[indice].childNodes[0].childNodes[0].value,
            codigos[indice].childNodes[0].childNodes[1].value,
            codigos[indice].childNodes[0].childNodes[2].value),
        );

    document.getElementById('container-codigo').innerHTML = "";
    for (var indice = 0; indice < novosCodigos.length; indice++)
        $('#container-codigo').append(novosCodigos[indice]);

}

