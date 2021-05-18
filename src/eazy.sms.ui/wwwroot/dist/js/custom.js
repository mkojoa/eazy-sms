let authType;
const routePrefix = {
    url: "",
    set setUrl(route) {
        this.url = route;
    }
}

const init = (config) => {
    routePrefix.setUrl = config.routePrefix;
    //fetchData();
}

$(document).ready(function () {
    const url = `${location.pathname.replace("/index.html", "")}/api/sms`;

    // Initialize Datatables
    $.fn.dataTable.ext.classes.sPageButton = 'btn btn-outline-info btn-sm m-1 justify-content-center'; 

    var smsTable = $("#sms-table").DataTable({
        "deferRender": true,
        "lengthMenu": [[5, 10, 25, 50], [5, 10, 25, 50]],
        "columnDefs": [
            { "className": "dt-center", "targets": "_all" },
            {
                "width": "3%",
                "targets": 0,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            },
            {
                "width": "10%",
                "targets": 1,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            },
            {
                "width": "25%",
                "targets": 2,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            },
            {
                "width": "6%",
                "targets": 3,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            },
            {
                "width": "2%",
                "targets": 4,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            },
            {
                "width": "3%",
                "targets": 5,
                "sClass": "cell-center",
                "sortable": false,
                "searchable": false,
                "orderable": false
            }
        ],
        "order": [[1, 'asc']],
        "ajax": {
            "url": `${url}`,
            "dataSrc": ""
        },
        "aoColumns": [
            {
                "mData": null,
            },
            {
                "mData": "Subject",
                "render": function (data, type, sms) {
                    return "Unsubscribe Created";
                }
            },
            {
                "mData": "Message",
                "render": function (data, type, sms) {
                    return "Reply STOP to unsubscribe or HELP for help. 4 msgs per month, Msg&Data rates may apply.";
                }
            },
            {
                "mData": "Date",
                "render": function (data, type, sms) {
                    return "Dec 15, 2019";
                }
            },
            {
                "mData": "Status",
                "render": function (data, type, sms) {
                    return `<span class="badge badge-success">Sent</span>`;
                }
            },
            {
                "mData": "pkId",
                "render": function (data, type, sms) {
                    console.log(sms.message.campaign);
                    var t = 
                           `<button type="button" class="btn btn-outline-info btn-sm" data-toggle="modal" data-target="#exampleModal">
                                <i class="fa fa-eye" title="view message details"></i>
                            </button>
                            <button class="btn btn-outline-success btn-sm">
                                <i class="fa fa-redo" title="resend failed message"></i>
                            </button>`;

                    return t;
                }
            },
        ]
       
        //    {
        //        "mData": "id",
        //        "render": function (data, type, sms) {
        //            console.log(data)
        //            console.log(type)
        //            console.log(sms)
        //            btn_lock = `<a href="#" class="btn btn-warning btn-xs lock-row" data-toggle="tooltip"
        //                            data-sms='${JSON.stringify(sms)}'
        //                            data-placement="top" title="Lock Staff" data-original-title="Lock User">
        //                            <span><i class="fa fa-lock"></i></span>
        //                        </a>`;

        //            btn_delete = `<a href="#" class="btn btn-danger btn-xs delete-row" title="SoftDelete Staff" data-toggle="tooltip"
        //                            data-sms='${JSON.stringify(sms)}'
        //                            data-placement="top" data-original-title="SoftDelete Delete">
        //                            <i class="fa fa-recycle"></i>
        //                          </a>`;

        //            return `${btn_lock}  ${btn_delete} `;
        //        }
        //    }
    });
    smsTable.on('order.dt search.dt', function () {
        smsTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

});

//const fetchData = () => {
//    const url = `${location.pathname.replace("/index.html", "")}/api/sms`;
//    let xf = null;

//    $.get({
//        url: url,
//        xhrFields: xf,
//        success: function (data) {

//            //console.log(data);

//            //data.logs.forEach(function (log) {
//            //    let exception = "";
//            //    if (log.exception != undefined) {
//            //        exception =
//            //            `<a href="#" title="Click to view" class="modal-trigger" data-type="text">
//            //            View <span style="display: none">${log.exception}</span>
//            //        </a>`;
//            //    }
        
//            //    $(tbody).append(row);
//            //});
//        }
//    }).fail(function (error) {
//        if (error.status === 500) {
//            const x = JSON.parse(error.responseJSON.errorMessage);
//            alert(x.errorMessage);
//        } else {
//            alert(error.responseText);
//        }
//    });
//}
//var table = $('#sms-table').DataTable({
//    "ajax": {
//        "type": "GET",
//        "url": url,
//        "dataSrc": "",
//        "success": function (data) {
//            console.log(data);
//        },
//        "error": function (error) {
//            console.log(error);
//        },
//    },
//    "columns": [
//        { "data": "name" },
//        { "data": "position" },
//        { "data": "office" },
//        { "data": "extn" },
//        { "data": "start_date" },
//        { "data": "salary" }
//    ]
//});
//table.on('order.dt search.dt', function () {
//    table.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
//        cell.innerHTML = i + 1;
//    });
//}).draw();

 //$('#example').DataTable({
    //    searching: true,
    //    info: true,
    //    lengthChange: true,
    //    responsive: true,
    //    autoWidth: true,
    //});