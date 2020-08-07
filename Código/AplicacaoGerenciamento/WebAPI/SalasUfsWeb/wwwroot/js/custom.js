$(document).ready(function () {
    $('#login_auth').inputmask({
        mask: '999.999.999-99',
        keepStatic: true
    }); 

    $('#show_pass').prop('checked', false);
});

function validaCpf() {
    var cpf = $('#login_auth').val();
    var terminouDig = cpf.match(/\d$/g);

    if (terminouDig) {
        let url = "/Login/ValidaCpf";

        cpf = cpf.replace(/[^0-9a-zA-Z]/g, "");

        $.post(url, { cpf: cpf }, function (data) {
            if (!data) {
                $('span[data-valmsg-for="Login"]').text('CPF inválido!!');
            } else {
                $('span[data-valmsg-for="Login"]').text('');
            }
        })
    }
}

function showPass(checkbox) {
    if (checkbox.checked)
        $('#senha_auth').prop("type", "text");
    else
        $('#senha_auth').prop("type", "password");
}

