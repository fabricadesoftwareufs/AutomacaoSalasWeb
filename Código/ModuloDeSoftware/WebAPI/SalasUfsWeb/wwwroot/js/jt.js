$(document).ready(function () {
    if (document.querySelector('#mensagem-retorno'))
        document.getElementById("mensagem-retorno").click();

    $('#codigos').hide();

    $('#checkbox').on('click', function () {
        if ($(this).prop('checked')) {
            $('#codigos').fadeIn();
        }
        else {
            $('#codigos').hide();
        }
    });
});