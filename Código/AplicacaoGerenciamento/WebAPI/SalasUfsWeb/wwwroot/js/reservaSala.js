function loadBlocosSalas() {
    let selectBlocos = document.getElementById('select-bloco');
    let selectSalas = document.getElementById('select-sala');
    let id = $('#select-org').val();

    let url = "/ReservaSala/GetBlocosByOrg";

    if (id != 0) {
        $.get(url, { id: id }, function (data) {
            clearSelect(selectBlocos);
            for (let i = 0; i < data.length; i++) {
                let option = document.createElement("option");
                option.text = data[i].id + " | " + data[i].titulo;
                option.value = data[i].id;
                console.log(data[i].titulo);
                selectBlocos.add(option);
            }
            if (data != null) {
                let url = "/ReservaSala/GetSalasByBloco";
                $.get(url, { id: data[0].id }, function (response) {
                    clearSelect(selectSalas);
                    for (let i = 0; i < response.length; i++) {
                        let option = document.createElement("option");
                        option.text = response[i].id + " | " + response[i].titulo;
                        option.value = response[i].id;

                        selectSalas.add(option);
                    }
                })
            }
        })
    }
}

function loadSalas() {
    let selectSalas = document.getElementById('select-sala');
    let id = $('#select-bloco').val();
    let url = "/ReservaSala/GetSalasByBloco";
    $.get(url, { id: id }, function (response) {
        clearSelect(selectSalas);
        for (let i = 0; i < response.length; i++) {
            let option = document.createElement("option");
            option.text = response[i].id + " | " + response[i].titulo;
            option.value = response[i].id;
            console.log(response[i].id);
            selectSalas.add(option);
        }
    })
}
function clearSelect(element) {
    element.innerHTML = "";
}