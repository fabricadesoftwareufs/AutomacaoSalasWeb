$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();

    if (document.querySelector('.horarios-planejamento'))
        document.getElementById("btn-create-planejamento").disabled = false;
    else 
        document.getElementById("btn-create-planejamento").disabled = true;

});

function AdicionarNovoHorario() {

    let horarioInicio = $('#horarioInicio').val();
    let horarioFim = $('#horarioFim').val();
    let dia = $('#input-dia-semana').val();
    let indice = 0;
    var novoHorario = new Array();

    if (horarioInicio.length > 0 && horarioFim.length > 0 && dia.length > 0) {

        let horarios = document.getElementsByClassName('horarios-planejamento');
        if (document.querySelector('.horarios-planejamento')) {
            novoHorario.push(adicionaHorarioNaTabela(indice, horarioInicio, horarioFim, dia));

            for (indice = 0; indice < horarios.length; indice++) {

                let idItem = 'novo-horario-' + (indice + 1);

                dia = horarios[indice].childNodes[2].childNodes[0].value;
                horarioInicio = horarios[indice].childNodes[0].childNodes[0].value;
                horarioFim = horarios[indice].childNodes[1].childNodes[0].value;

                novoHorario.push(adicionaHorarioNaTabela(indice + 1, horarioInicio, horarioFim, dia));
            }

        } else
            novoHorario.push(adicionaHorarioNaTabela(indice, horarioInicio, horarioFim, dia));

        document.getElementById('container-horarios').innerHTML = "";
        for (var i = 0; i < novoHorario.length; i++)
            $('#container-horarios').append(novoHorario[i]);

        document.getElementById("btn-create-planejamento").disabled = false;

    }
}

function adicionaHorarioNaTabela(indice, horarioInicio, horarioFim, dia){
    let idItem = 'novo-horario-' + indice;
    let horario =
        '<tr id="' + idItem + '" class="horarios-planejamento">' +
        '<td>' +
        '<input class="form-control" type="time" name="Horarios[' + indice + '].HorarioInicio" readonly  value = "' + horarioInicio + '"/>' +
        '</td>' +
        ' / '+
        '<td>' +
        '<input class="form-control" type="time" name="Horarios[' + indice + '].HorarioFim" readonly  value = "' + horarioFim + '"/>' +
        '</td>' +
        ' - ' +
        '<td>' +
        '<input class="form-control" name="Horarios[' + indice + '].DiaSemana" readonly value = "' + dia + '"/>' +
        '</td>' +

        '<td>' +
        '<a id="remove-novo-horario"  onclick="RemoveNovoHorario(' + '\''+idItem + '\''+ ')" class="btn btn-danger"><i class="nav-icon fa fa-trash text-white"></i> </a>' +
        '</td>' +
        '</tr>';

    return horario;
}


function RemoveNovoHorario(idItem) {

    document.getElementById(idItem).remove();

    let horarios = document.getElementsByClassName('horarios-planejamento');
    for (var indice = 0; indice < horarios.length; indice++) 
        var e = horarios[indice].id = 'novo-horario-' + indice;

    if (!document.querySelector('.horarios-planejamento'))
        document.getElementById("btn-create-planejamento").disabled = true;     
} 