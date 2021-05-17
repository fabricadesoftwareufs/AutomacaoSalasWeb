$(document).ready(function () {

    if ($('#mensagem-retorno') != null)
       $("mensagem-retorno").click();

    $('#login_auth').inputmask({
        mask: '999.999.999-99',
        keepStatic: true
    }); 

    $('#span_cpf').hide();

    $('#show_pass').prop('checked', false);
    $('#senha_auth').prop("type", "password");

     /* DIV DE MSG */
    if (window.location.href.includes("msg")) {
        $('#div-msg').addClass("alert alert-danger text-center");
        if (window.location.href.includes("msg=error")) {
            $('#div-msg').text("Erro, credenciais incorretas!");
        }
        else if ((window.location.href.includes("msg=invalidCpf"))) {
            $('#div-msg').text("Erro, CPF inválido!");
        }

        $("#div-msg").fadeTo(2000, 500).slideUp(500, function () {
            $("#div-msg").slideUp(500);
        });
    }
  
});

function validaCpf() {
    var cpf = $('#login_auth').val();
    var terminouDig = cpf.match(/\d$/g);

    if (terminouDig) {
        let url = "/Login/ValidaCpf";

        cpf = cpf.replace(/[^0-9a-zA-Z]/g, "");

        $.post(url, { cpf: cpf }, function (data) {
            if (!data) {
                $('#span_cpf').text("CPF inválido!");
                $('#span_cpf').show();
            } else {
                $('#span_cpf').hide();
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

