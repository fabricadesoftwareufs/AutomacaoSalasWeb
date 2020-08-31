function containsAssociacao(id) {
    let url = "/Usuario/HasPlanOrReserv";
    if (id != 0) {
        $.get(url, { idUsuario: id }, function (data) {
            if (data) {
                $('.obs').text('O usuário contem planejamento ou reservas associadas.');
                $('.obs').addClass('text-danger');
            }
            else {
                $('.obs').text('O usuário não contém nenhuma observação.');
                $('.obs').removeClass('text-danger');
            }
        });
        
    }
}
