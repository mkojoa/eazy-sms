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

class DateHandler {

    formatDate = (date, format) => {
        date = newDate(date) || "";
        return newIntl.DateTimeFormat(`en-${format.toUpperCase()}`).format(date)
            .split('/').map(x => x.length < 2 ? '0' + x : x).join('/') || "";
    }

    getCurrentDate = (format = 'us', seperator = '/') => {
        let date = new Date();
        let sep = seperator;
        let year = date.getFullYear();
        let month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        let day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        format = format.toLowerCase()
        return format === 'us' ? `${month}${sep}${day}${sep}${year}` :
            format === 'gb' ? `${day}${sep}${month}${sep}${year}` : 'Invalid Date'
    }

    customDate = (date, format = 'dmy', separator = '/') => {
        date = new Date(date)
        let s = separator
        format = format.toLowerCase()
        let year = date.getFullYear();
        let month = (1 + date.getMonth()).toString();
        month = month.length > 1 ? month : '0' + month;
        let day = date.getDate().toString();
        day = day.length > 1 ? day : '0' + day;
        return format === "dmy" ? `${day}${s}${month}${s}${year}` : format === "mdy" ?
            `${month}${s}${day}${s}${year}` : format === "ymd" ?
                `${year}${s}${month}${s}${day}` : 'Invalid Date'
    }

    addDays = (date, NumWorkDays) => {
        date = new Date(date);
        date.setDate(date.getDate() + NumWorkDays);
        return date;
    }

    calendarHelp = (date, sep) => {
        date = new Date(date);
        let months = ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec']
        let year = date.getFullYear();
        let month = (1 + date.getMonth());
        let day = date.getDate().toString();
        return `${day}${sep}${months[month - 1]}${sep}${year}`
    }

    getCalculatedAge = (birthdate) => {
        let today = new Date();
        let birthDate = new Date(birthdate);
        let age = today.getFullYear() - birthDate.getFullYear();
        let m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        return age;
    }
}

const fetchData = () => {
    const url = `${location.pathname.replace("/index.html", "")}/api/sms`;

    let xf = null;
    $.get({
        url: url,
        xhrFields: xf,
        success: function (data) {
            fetchDataToProgressBar(data); // populate progress bar
            fetchDataToActivitySms(data); // populate activity
            fetchDataToSMSTable(data); // populate datatable
            fetchDataToChatBar(data);  // populate chat bar
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

const fetchDataToProgressBar = (data) => {

    let progressElement = document.querySelector('#progress-sms-bar');

    var recordTotal = data.length;
    var recordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 })
    var recordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 })

    var sentPercentageRecord = (recordSentFilter.length / recordTotal) * 100
    var failedPercentageRecord = (recordFailedFilter.length / recordTotal) * 100

    progressElement.innerHTML = `
                            <div class="col-sm">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="info-card">
                                                <h4 class="info-title">Sent SMS<span class="info-stats">${recordSentFilter.length}</span></h4>
                                                <div class="progress" style="height: 3px;">
                                                    <div class="progress-bar bg-success" role="progressbar" style="width: ${sentPercentageRecord}%" aria-valuenow='${sentPercentageRecord}' aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="info-card">
                                                <h4 class="info-title">Failed SMS<span class="info-stats">${recordFailedFilter.length}</span></h4>
                                                <div class="progress" style="height: 3px;">
                                                    <div class="progress-bar bg-danger" role="progressbar" style="width: ${failedPercentageRecord}%" aria-valuenow='${sentPercentageRecord}' aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-sm">
                                    <div class="card">
                                        <div class="card-body">
                                            <div class="info-card">
                                                <h4 class="info-title">Total SMS Sent<span class="info-stats">${recordTotal}</span></h4>
                                                <div class="progress" style="height: 3px;">
                                                    <div class="progress-bar bg-info" role="progressbar" style="width: ${failedPercentageRecord}%" aria-valuenow='${sentPercentageRecord}' aria-valuemin="0" aria-valuemax="100"></div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                        `;
}

const fetchDataToActivitySms = (data) => {
    let progressElement = document.querySelector('#sms-activity');

    let htmlData = `<ul class="list-unstyled" id="sms-activity">`;


    data.forEach(function (sms) {

        let message = JSON.parse(sms.message ? sms.message : '');

        htmlData += `
                <li>
                    <div>
                        <span class="notification-badge-custom bg-${sms.sentStatus ? 'success' : 'danger'}"><b>S</b></span>
                        <span class="notification-${sms.sentStatus ? 'success' : 'danger'}">
                            <span class="notification-info">${message.campaign} 15min ago</span>
                        </span>
                    </div>
                </li>
        `;
    });

    htmlData += `</ul>`;
    progressElement.innerHTML = htmlData;
}

const fetchDataToChatBar = (data) => {
    "use strict";

    //Jan
    var JanRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 1 }).length;
    var JanRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 1 }).length
    var JanRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 1 }).length

    //Feb
    var FebRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 2 }).length;
    var FebRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 2 }).length
    var FebRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 2 }).length

    // Mar
    var MarRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 3 }).length;
    var MarRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 3 }).length
    var MarRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 3 }).length

    // Apr
    var AprRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 4 }).length;
    var AprRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 4 }).length
    var AprRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 4 }).length


    // May
    var MayRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 5 }).length;
    var MayRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 5 }).length
    var MayRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 5 }).length

    // Jun
    var JunRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 6 }).length;
    var JunRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 6 }).length
    var JunRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 6 }).length

    // Jul
    var JulRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 7 }).length;
    var JulRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 7 }).length
    var JulRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 7 }).length

    // Aug
    var AugRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 8 }).length;
    var AugRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 8 }).length
    var AugRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 8 }).length

    // Sep
    var SepRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 9 }).length;
    var SepRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 9 }).length
    var SepRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 9 }).length


    // Oct
    var OctRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 10 }).length;
    var OctRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 10 }).length
    var OctRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 10 }).length

    // Nov
    var NovRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 11 }).length;
    var NovRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 11 }).length
    var NovRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 11 }).length

    // Dec
    var DecRecordTotal = [...data].filter((item) => { return new Date(item.updatedAt).getMonth() + 1 == 12 }).length;
    var DecRecordSentFilter = [...data].filter((item) => { return item.sentStatus == 1 && new Date(item.updatedAt).getMonth() + 1 == 12 }).length
    var DecRecordFailedFilter = [...data].filter((item) => { return item.sentStatus == 0 && new Date(item.updatedAt).getMonth() + 1 == 12 }).length


    var options = {
        chart: {
            height: 350,
            type: 'bar',

            toolbar: {
                show: false
            }
        },
        plotOptions: {
            bar: {
                horizontal: false,
                columnWidth: '55%',
                endingShape: 'rounded'
            },
        },

        colors: ['#58eda0', '#ed5887', '#6a04fb'],




        dataLabels: {
            enabled: false
        },
        stroke: {
            show: true,
            width: 2,
            colors: ['transparent']
        },
        series: [{
            name: 'Sent SMS',
            data: [JanRecordSentFilter, FebRecordSentFilter, MarRecordSentFilter, AprRecordSentFilter, MayRecordSentFilter, JunRecordSentFilter, JulRecordSentFilter, AugRecordSentFilter, SepRecordSentFilter, NovRecordSentFilter, DecRecordSentFilter]
        }, {
            name: 'Failed SMS',
            data: [JanRecordFailedFilter, FebRecordFailedFilter, MarRecordFailedFilter, AprRecordFailedFilter, MayRecordFailedFilter, JunRecordFailedFilter, JulRecordFailedFilter, AugRecordFailedFilter, SepRecordFailedFilter, NovRecordFailedFilter, DecRecordFailedFilter]
        },
        {
            name: 'Total SMS',
            data: [JanRecordTotal, FebRecordTotal, MarRecordTotal, AprRecordTotal, MayRecordTotal, JunRecordTotal, JulRecordTotal, AugRecordTotal, SepRecordTotal, NovRecordTotal, DecRecordTotal]
        }],
        xaxis: {
            categories: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec'],
        },
        fill: {
            opacity: 1
        },
        tooltip: {
            y: {
                formatter: function (val) {
                    return "sum " + val + " "
                }
            }
        }
    }
    var chart = new ApexCharts(
        document.querySelector("#apex1"),
        options
    );

    chart.render();
}

const fetchDataToSMSTable = (data) => {

    const url = `${location.pathname.replace("/index.html", "")}/api/sms`;

    // Initialize Datatables
    $.fn.dataTable.ext.classes.sPageButton = 'btn btn-outline-info btn-sm m-1 justify-content-center';

    //let _BodyRows = "", _count = 0;
    //data.forEach((sms) => {
    //    _count += 1;


    //    let message = JSON.parse(sms.message ? sms.message : '');


    //    console.log(message)

    //    _BodyRows += `
    //            <tr>
    //                <td scope="row">${_count}</td>
    //                <td scope="row">${message.campaign}</td>
    //                <td scope="row">${message.message}</td>
    //                <td scope="row">${sms.updatedAt}</td>
    //                <td scope="row">
    //                    <span class="badge badge-${sms.sentStatus ? 'success' : 'danger'}">${sms.sentStatus ? 'Sent' : 'Failed'}</span>
    //                </td>
    //                <td scope="row">
    //                    <div class="btn-group p-0 m-0">
    //                        <button type="button" data-content='${JSON.stringify(sms)}' class="btn btn-outline-info btn-sm get--details">
    //                            <i class="fa fa-eye" title="view message details"></i>
    //                        </button>
    //                        <button class="btn btn-outline-success btn-sm">
    //                            <i class="fa fa-redo" title="resend failed message"></i>
    //                        </button>
    //                    </div>
    //                </td>
    //            </tr>`;
    //});
    //$("#sms-table-body").html(_BodyRows);

    //ProcessToDataTable("sms-table")

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
                    try {
                        let message = JSON.parse(sms.message ? sms.message : '');
                        return message.campaign
                    } catch (err) {

                    }
                    return "Unsubscribe Created";
                }
            },
            {
                "mData": "Message",
                "render": function (data, type, sms) {
                    try {
                        let message = JSON.parse(sms.message ? sms.message : '');
                        return message.message
                    } catch (err) {

                    }
                    return "Reply STOP to unsubscribe or HELP for help. 4 msgs per month, Msg&Data rates may apply.";
                }
            },
            {
                "mData": "Date",
                "render": function (data, type, sms) {
                    try {
                        return DateHandler.calendarHelp(sms.updatedAt, '-') || 'No date';
                    } catch (err) {

                    }
                    return "Dec 15, 2019";
                }
            },
            {
                "mData": "Status",
                "render": function (data, type, sms) {
                    try {

                        return `<span class="badge badge-${sms.sentStatus ? 'success' : 'danger'}">${sms.sentStatus ? 'Sent' : 'Failed'}</span>`
                    } catch (err) {

                    }
                    return `<span class="badge badge-success">Sent</span>`;
                }
            },
            {
                "mData": "pkId",
                "render": function (data, type, sms) {
                    let c = JSON.parse(sms.message ? sms.message : '');
                    var t =
    `<button type="button" data-content='${JSON.stringify(sms)}' class="btn btn-outline-info btn-sm get--details">
            <i class="fa fa-eye" title="view message details"></i>
        </button>
        <button class="btn btn-outline-success btn-sm">
            <i class="fa fa-redo" title="resend failed message"></i>
        </button>`;

                    return t;
                }
            },
        ]
    });
    smsTable.on('order.dt search.dt', function () {
        smsTable.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
            cell.innerHTML = i + 1;
        });
    }).draw();

    setInterval(function () {
        smsTable.ajax.reload();
    }, 30000);

    $(document).on('click', '.get--details', function () {
        let { id, message, createdAt, resultMessage, resultStatus, sentStatus, updatedAt } = $(this).data('content');

        try {
            message = JSON.parse(message ? message : '')
        } catch (err) {
            console.log(err)
        }

        let tableElement = document.querySelector('#message--display');


        tableElement.innerHTML = `<tr>
                                    <th scope="col">#</th>
                                        <th scope="row" style="font-weight: normal"><code>${id}</code ></th>
                                    </tr>
                                    <tr>
                                        <th scope="col">Subject</th>
                                        <th scope="row" style="font-weight: normal">${message.campaign}</th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Message</th>
                                        <th scope="row" style="font-weight: normal">${message.file ? '<span type="button" class="audio-file"><i class="fa fa-file-audio btn btn-info btn-sm"></i></span>' : message.message}</th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Recipient</th>
                                        <th scope="row" style="font-weight: normal"><code>${Array.isArray(message.recipient) ? message.recipient.join(', ') : ''}</code ></th >
                                    </tr>
                                    <tr>

                                        <th scope="col">Status</th>
                                        <th scope="row" style="font-weight: normal"><span class="badge badge-${sentStatus ? 'success' : 'danger'}">${sentStatus ? 'Sent' : 'Failed'}</span></th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Sender</th>
                                        <th scope="row" style="font-weight: normal"><code>${message.sender}</code></th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Enable Schedule</th>
                                        <th scope="row" style="font-weight: normal">${message.is_schedule ? 'Yes' : 'No'}</th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Schedule Date</th>
                                        <th scope="row" style="font-weight: normal">${message.schedule_date ? message.schedule_date : 'No date'}</th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Attachment Name</th>
                                        <th scop;e="row" style="font-weight: normal">${message.file ? message.file.replace(/^.*[\\\/]/, '') : 'No file attached'}</th>
                                    </tr>
                                    <tr>

                                        <th scope="col">Attachment Path</th>
                                        <th scop;e="row" style="font-weight: normal"><small>${message.file ? "\wwwroot" + message.file.split('wwwroot').pop().split(';')[0] : 'No file attached'} </small></th>
                                    </tr>`;


        //<button onclick-"play();" class="btn btn-info btn-sm" ><i class="fa fa-file"></i></button>

        $('#exampleModal').modal('show');
    });

    // audio file
    $(document).on("click", ".audio-file", function () {
        var audio = new Audio("C:\\Users\\MAmeyaw\\RiderProjects\\eazy-sms\\samples\\easy.sms.test\\wwwroot\\Template\\Voice\\ringtone.mp3");

        console.log(audio);

        audio.play();
    });
}

const ProcessToDataTable = (dtId, dtHeight = 380) => {
    let dtable = $(`#${dtId}`).DataTable({
        "scrollY": `${dtHeight}px`,
        "scrollCollapse": true,
        "paging": false,
        "scrollX": true,
        "searching": true,
        "ordering": true,
        responsive: true,
        dom: 'Bfrtip',
        bInfo: false
    });
    //dtable.buttons().container().appendTo($('div#dt-toolbox'));



    // Toolbox
    $("div#dt-toolbox .dt-buttons")
        .addClass("float-right")
        .prop("style", "margin-bottom:0px!important;");



    // Searchbox
    $(".dataTables_filter").prop("hidden", true);
    $("div#dt-searchbox").html(`
            <div class="input-group">
                <span class="input-group-addon text-theme">
                    <i class="fa fa-search"></i>
                </span>
                <input type="text" class="form-control to-title-case" id="dt-searchbox-text" placeholder="Search..." />
                <span class="input-group-addon text-theme cursor-pointer">
                    Search
                </span>
            </div>`);
    $("input#dt-searchbox-text").bind("keyup search input paste cut", () => {
        let dt_fkey = $("input#dt-searchbox-text").val();
        dtable.search(dt_fkey).draw();
    });

    // Countbox
    let dcount = dtable.rows().count();
    $("div#dt-countbox").html(`
            <div class="text pt-2">
                <span data-toggle="tooltip" data-placement="top" title="Number of rows">Rows: <span class="text-theme">${dcount}</span></span>
            </div>`);
}