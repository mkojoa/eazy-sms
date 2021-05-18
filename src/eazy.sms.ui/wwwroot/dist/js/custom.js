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

let authType;
const routePrefix = {
    url: "",
    set setUrl(route) {
        this.url = route;
    }
}

const init = (config) => {
    routePrefix.setUrl = config.routePrefix;
    fetchData();
}

const fetchData = () => {
    const url = `${location.pathname.replace("/index.html", "")}/api/sms`;
    let xf = null;

    $.get({
        url: url,
        xhrFields: xf,
        success: function (data) {

            //console.log(data);

            //data.logs.forEach(function (log) {
            //    let exception = "";
            //    if (log.exception != undefined) {
            //        exception =
            //            `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
            //            View <span style="display: none">${log.exception}</span>
            //        </a>`;
            //    }
        
            //    $(tbody).append(row);
            //});
        }
    }).fail(function (error) {
        if (error.status === 500) {
            const x = JSON.parse(error.responseJSON.errorMessage);
            alert(x.errorMessage);
        } else {
            alert(error.responseText);
        }
    });
}