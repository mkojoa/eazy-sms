$(document).ready(function () {
    // Initialize Datatables
    $.fn.dataTable.ext.classes.sPageButton = 'btn btn-outline-info btn-sm m-1 justify-content-center'; 

    $('#example').DataTable({
        searching: true,
        info: true,
        lengthChange: true,
        responsive: true,
        autoWidth: true,
    });
});