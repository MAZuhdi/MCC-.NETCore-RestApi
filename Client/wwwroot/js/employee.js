$.ajax({
    url: "https://localhost:44367/Api/Employees/Registered"
}).done((result) => {
    console.log(result);
})

$(document).ready(function () {
    $('#tableEmployee').DataTable({
        ajax: {
            'url': "https://localhost:44367/Api/Employees/Registered",
            'dataType': 'json',
            'dataSrc': 'data'
        },
        dom: 'Bfrtip',
        buttons: [
            {
                extend: 'copy',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            },
            {
                extend: 'csv',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            },
            {
                extend: 'excel',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            },
            {
                extend: 'pdfHtml5',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            },
            {
                extend: 'print',
                exportOptions: { columns: [0, 1, 2, 3, 4] }
            },
        ],
        columns: [
            {
                data:'fullName'
            },
            {
                data: 'gender',
                render: function (data, type, row) {
                    if (data == 0) {
                        return "Male";
                    } else if (data == 1) {
                        return "Female";
                    } else {
                        return "Undefined";
                    }
                }
            },
            {
                data: 'email',
            },
            {
                data: 'universityName'
            },
            {
                data: 'salary',
                render: function (data, type, row) {
                    return "Rp" + data;
                }
            },
            {
                data: null,
                bsortable: false,
                render: function (data, type, row) {
                    return `<button class="btn btn-primary" data-toggle="modal" data-target="#pokeModal" onclick="getDetails(\`${row['url']}\`)">Detail</button>`;
                }
            }
        ]
    });
});