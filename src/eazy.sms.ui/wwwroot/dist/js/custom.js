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
                    console.log(c)

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
                                        <th scope="row" style="font-weight: normal">${message.message}</th>
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

        $('#exampleModal').modal('show');
    });

});
