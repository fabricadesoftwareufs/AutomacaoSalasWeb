
function loadBlocosAndUsuarios() {
    loadBlocos();
    loadUsuarios();
}

function loadBlocos() {
    let idOrg = $('#select-organizacao').val();
    let selectBlocos = document.getElementById('select-bloco');
    let url = "/MetodosAuxiliares/GetBlocosByIdOrganizacao";

    $.get(url, { idOrganizacao: idOrg }, function (data) {
        selectBlocos.innerHTML = "";
        for (let i = 0; i < data.length; i++) {
            let option = document.createElement("option");
            option.text = data[i].id + " | " + data[i].titulo;
            option.value = data[i].id;

            selectBlocos.add(option);
        }

        if (document.querySelector('#select-sala'))
            loadSalas();
    });
}

function loadUsuarios() {
    let idOrg = $('#select-organizacao').val();
    let selectUsuarios = document.getElementById('select-usuario');
    let url = "/MetodosAuxiliares/GetUsuariosByIdOrganizacao";

    $.get(url, { idOrganizacao: idOrg }, function (data) {
        selectUsuarios.innerHTML = "";
        for (let i = 0; i < data.length; i++) {
            let option = document.createElement("option");
            option.text = data[i].cpf + " | " + data[i].nome;
            option.value = data[i].id;

            selectUsuarios.add(option);
        }
    });
}

function loadSalas() {
    let selectSalas = document.getElementById('select-sala');
    let url = "/MetodosAuxiliares/GetSalasByIdBloco";
    let blocoId = $('#select-bloco').val();

    $.get(url, { idBloco: blocoId }, function (data) {
        selectSalas.innerHTML = "";
        for (let i = 0; i < data.length; i++) {
            let option = document.createElement("option");
            option.text = data[i].id + " | " + data[i].titulo;
            option.value = data[i].id;

            selectSalas.add(option);
        }
    });
}

//function loadHardwares() {
//    let selectSalas = document.getElementById('select-hardwares');
//    let url = "/MetodosAuxiliares/GetAtuadorNotUsed";
//    let blocoId = $('#select-sala').val();

//    $.get(url, { idBloco: blocoId }, function (data) {
//        selectSalas.innerHTML = "";
//        for (let i = 0; i < data.length; i++) {
//            let option = document.createElement("option");
//            option.text = data[i].id + " | " + data[i].titulo;
//            option.value = data[i].id;

//            selectSalas.add(option);
//        }
//    });
//}