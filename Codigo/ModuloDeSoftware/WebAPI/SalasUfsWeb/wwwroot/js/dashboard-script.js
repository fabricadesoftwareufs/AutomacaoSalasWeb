$(document).ready(function () {
    let codDia = new Date().getDay();
    loadSalasByDiaSemana(getDiaSemana(codDia));
    checkButtonByCodigoDia(codDia);
});

function submitForm(formId, idItem, salaParticular) {
    if (salaParticular)
        document.getElementById('tela-carregamento').hidden = false;
    else
        document.getElementById('tela-carregamento-reserva').hidden = false;

    setValueSwitch(idItem);
    if (document.querySelector('#'+formId))
        document.getElementById(formId).submit();
}

function setValueSwitch(idMonitoramento) {
    if ($('#luzes-' + idMonitoramento).is(":checked"))
        $('#luzes-' + idMonitoramento).val(true);
    else
        $('#luzes-' + idMonitoramento).val(false);

    if ($('#arCondicionado-' + idMonitoramento).is(":checked"))
        $('#arCondicionado-' + idMonitoramento).val(true);
    else
        $('#arCondicionado-' + idMonitoramento).val(false);
}


function loadSalasByDiaSemana(dia) {
    let url = "/Home/GetReservasUsuario";

    checkButtonByCodigoDia(getCodigoSemana(dia));
    document.getElementById('container-reservas').innerHTML = "";
    $.get(url, { diaSemana: dia }, function (data) {
        if (data.salasUsuario.length > 0) {
            for (var indice = 0; indice < data.salasUsuario.length; indice++)
                addReserva(data.salasUsuario[indice], indice ,dia);

        } else $('#container-reservas').append('<p class="text-center"> Não há nenhuma reserva para este dia nessa semana! </p>');
    });
}

function addReserva(data, indice ,dia) {
    var item = '<div class="card">' +
        '<div class="card-header card-title">' +
        '<h5 class="align-element">' + data.bloco.titulo + '<br/>' + data.sala.titulo + '</h5>' +
        '<div class="align-element float-right">' +
        '<h5 class="text-right">' + getDiaSemanaCompleto(dia) + '<br />' + moment(new Date(data.horarioSala.data), 'YYYY-MM-DD', true).format('DD/MM/YYYY')+'</h6>'+ 
        '</div>' +
        '</div>' +
        '<div class="card-body">' +
        '<div class="align-element">' +
        '<h5 class="card-text">' + convertMsToHM(data.horarioSala.horarioInicio.totalMilliseconds) + ' às ' + convertMsToHM(data.horarioSala.horarioFim.totalMilliseconds) + '</h5>' +
        '<form asp-controller="Home" asp-action="CancelarReserva" method="post" action="/Home/CancelarReserva">' +
        '<input class="form-control" name="idReserva" value="' + data.horarioSala.id + '" hidden>' +
        '<input type="submit" class="btn btn-danger" value="Cancelar" />' +
        '</form>' +
        '</div>' +
        '<div class="float-right">' +
            '<div class="float-right">' + 
                '<div class="align-element">' +   
                    '<form method="post" action="/Home/MonitorarSala" asp-controller="Home" asp-action="MonitorarSala" id="form-' + indice + "-" + data.monitoramentoLuzes.id + "-" + data.horarioSala.id + '">' +
                        '<div class="form-control" hidden>' +
                            '<input class="form-control" name="SalaId" value="' + data.sala.id + '" />' +
                            '<input class="form-control" name="Id" value="' + data.monitoramentoLuzes.id + '" />' +
                            '<input class="form-control" name="EquipamentoId" value="' + data.monitoramentoLuzes.equipamentoId +'" />' +
                            '<input class="form-control" name="SalaParticular" value="False" />' +
                        '</div>' +

                         '<div class="align-element">' +
                             '<h5 class="card-text">Luzes</h5>' +
                                    '<label class="switch" onchange="submitForm(\'form-' + indice + "-" + data.monitoramentoLuzes.id + "-" + data.horarioSala.id + '\',\'' + indice + "-" + data.monitoramentoLuzes.id + "-" + data.horarioSala.id + '\',false)">' +
                                    '<input type="checkbox" name="Estado" id="luzes-' + indice + "-" + data.monitoramentoLuzes.id + "-" + data.horarioSala.id + '" value="' + data.monitoramentoLuzes.estado + '"' + new String(data.monitoramentoLuzes.estado ? "checked" : "") + '/>' +
                                    '<span class="slider round"></span>' +
                             '</label>' +
                        '</div>' +
                      '</form>' +
                '</div>' +
            '</div>' +

            '<div class="float-right">' + 
                '<div class="align-element">' +    
                    '<form method="post" action="/Home/MonitorarSala" asp-controller="Home" asp-action="MonitorarSala" id="form-' + indice + "-" + data.monitoramentoCondicionadores.id + "-" + data.horarioSala.id + '">' +
                        '<div class="form-control" hidden>' +
                            '<input class="form-control" name="SalaId" value="' + data.sala.id + '" />' +
                            '<input class="form-control" name="Id" value="' + data.monitoramentoCondicionadores.id + '" />' +
                            '<input class="form-control" name="EquipamentoId" value="' + data.monitoramentoCondicionadores.equipamentoId + '" />' +
                            '<input class="form-control" name="SalaParticular" value="False" />' +
                        '</div>' +

                        '<div class="align-element">' +
                                '<h5 class="card-text">Condicionadores</h5>' +
                                '<label class="switch" onchange="submitForm(\'form-' + indice + "-" + data.monitoramentoCondicionadores.id + "-" + data.horarioSala.id + '\',\'' + indice + "-" + data.monitoramentoCondicionadores.id + "-" + data.horarioSala.id + '\',false)">' +
                                '<input type="checkbox" name="Estado" id="arCondicionado-' + indice + "-" + data.monitoramentoCondicionadores.id + "-" + data.horarioSala.id + '" value="' + data.monitoramentoCondicionadores.estado + '"' + new String(data.monitoramentoCondicionadores.estado ? "checked" : "") +'/>' +
                                '<span class="slider round"></span>' +
                                '</label>' +
                        '</div>' +
                    '</form>' +
                '</div>' +
            '</div>' +

        '</div>'
        '</div>' +
        '</div>';

    $('#container-reservas').append(item);
}

function padTo2Digits(num) {
    return num.toString().padStart(2, '0');
}

function convertMsToHM(milliseconds) {
    let seconds = Math.floor(milliseconds / 1000);
    let minutes = Math.floor(seconds / 60);
    let hours = Math.floor(minutes / 60);

    seconds = seconds % 60;
    minutes = seconds >= 30 ? minutes + 1 : minutes;

    minutes = minutes % 60;

    hours = hours % 24;

    return `${padTo2Digits(hours)}:${padTo2Digits(minutes)}`;
}
function checkButtonByCodigoDia(dia) {
    switch (dia) {
        case 0: document.getElementById("option_dom").checked = true;
            break;
        case 1: document.getElementById("option_seg").checked = true;
            break;
        case 2: document.getElementById("option_ter").enable = true;
            break;
        case 3: document.getElementById("option_qua").checked = true;
            break;
        case 4: document.getElementById("option_qui").checked = true;
            break;
        case 5: document.getElementById("option_sex").checked = true;
            break;
        case 6: document.getElementById("option_sab").checked = true;
            break;
        default: return "";
    }
}

function getDiaSemana(codigoDia) {
    switch (codigoDia) {
        case 0: return "DOM";
        case 1: return "SEG";
        case 2: return "TER";
        case 3: return "QUA";
        case 4: return "QUI";
        case 5: return "SEX";
        case 6: return "SAB";
        default: return "";
    }
}

function getCodigoSemana(dia) {
    switch (dia) {
        case "DOM": return 0;
        case "SEG": return 1;
        case "TER": return 2;
        case "QUA": return 3;
        case "QUI": return 4;
        case "SEX": return 5;
        case "SAB": return 6;
        default: return 0;
    }
}

function getDiaSemanaCompleto(dia) {

    if (getDiaSemana(new Date().getDay()) == dia)
        return "Hoje";

    switch (dia) {
        case "DOM": return "Próximo Domingo";
        case "SEG": return "Próxima Segunda-Feira";
        case "TER": return "Próxima Terça-Feira";
        case "QUA": return "Próxima Quarta-Feira";
        case "QUI": return "Próxima Quinta-Feira";
        case "SEX": return "Próxima Sexta-Feira";
        case "SAB": return "Próximo Sábado";
        default: return "";
    }



}