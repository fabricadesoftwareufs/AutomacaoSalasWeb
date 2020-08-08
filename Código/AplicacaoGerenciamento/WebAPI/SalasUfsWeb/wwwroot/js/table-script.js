$(function () {
    $("#table-search").DataTable({
        "responsive": true,
        "autoWidth": false,
    });
    $('#table-no-search').DataTable({
        "paging": true,
        "lengthChange": false,
        "searching": false,
        "ordering": true,
        "info": true,
        "autoWidth": false,
        "responsive": true,
    });
});