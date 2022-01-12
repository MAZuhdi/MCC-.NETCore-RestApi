$.ajax({
    url: "https://localhost:44367/Api/Employees/Registered"
}).done((result) => {
    console.log(result);
})

let empTable = $('#tableEmployee');
$(document).ready(function () {
    empTable.DataTable({
        ajax: {
            'url': "https://localhost:44367/Api/Employees/Registered",
            'dataType': 'json',
            'dataSrc': 'data'
        },
        dom: 'Bfrtip',
        buttons: [
            {
                text: 'Add Employee',
                action: function (e, dt, node, config) {
                    $('#emp-create-modal').modal('show');
                }
            },
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
                    return `<button class="btn btn-info" data-toggle="modal" data-target="#emp-detail-modal" onclick="getDetails(\`${data.nik}\`)"><i class="bi bi-info-circle-fill"></i></button>
                            <button class="btn btn-primary" data-toggle="modal" data-target="#emp-update-modal" onclick="getDetails(\`${data.nik}\`)"><i class="bi bi-pencil-fill"></i></button>
                            <button class="btn btn-danger" onclick="deleteEmployee(\`${data.nik}\`)"><i class="bi bi-trash-fill"></i></button>
                            `;
                }
            }
        ]
    });
});

//// Wait for the DOM to be ready
//$(function () {
//    // Initialize form validation on the registration form.
//    // It has the name attribute "registration"
//    $("#emp").validate({
//        // Specify validation rules
//        rules: {
//            // The key name on the left side is the name attribute
//            // of an input field. Validation rules are defined
//            // on the right side
//            firstname: "required",
//            lastname: "required",
//            email: {
//                required: true,
//                // Specify that email should be validated
//                // by the built-in "email" rule
//                email: true
//            },
//            password: {
//                required: true,
//                minlength: 5
//            }
//        },
//        // Specify validation error messages
//        messages: {
//            firstname: "Please enter your firstname",
//            lastname: "Please enter your lastname",
//            password: {
//                required: "Please provide a password",
//                minlength: "Your password must be at least 5 characters long"
//            },
//            email: "Please enter a valid email address"
//        },
//        // Make sure the form is submitted to the destination defined
//        // in the "action" attribute of the form when valid
//        submitHandler: function (form) {
//            form.submit();
//        }
//    });
//});

// if modal add employee is open, call getUniversity()
$('#emp-create-modal').on('show.bs.modal', function () {
    getUniversity();
});

function getUniversity() {
    $.ajax({
        url: 'https://localhost:44367/API/universities'
    }).done((data) => {
        var universitySelect = '';
        $.each(data.data, function (key, val) {
            universitySelect += `<option value='${val.id}'>${val.name}</option>`
        });
        $("#inputUniv").html(universitySelect);
    }).fail((error) => {
        console.log(error);
    })
}
//dijalankan kalo kita klik submit
$("#form-create").submit(function (event) {
    $("#form-create").validate({
        //validasi
        rules: {
            //atribut name
            firstname: {
                required: true
            },
            lastname: {
                required: true
            },
            phone: {
                required: true
            },
            gender: {
                required: true
            },
            birthdate: {
                required: true
            },
            salary: {
                required: true,
                number: true
            },
            email: {
                required: true,
                email: true
            },
            password: {
                required: true,
                minlength: 8
            },
            univ: {
                required: true
            },
            degree: {
                required: true
            },
            gpa: {
                required: true,
                number: true,
                range: [0, 4]
            }
        },
/*        errorPlacement: function (error, element) { },*/
        highlight: function (element) {
            $(element).closest('.form-control').addClass('is-invalid');
        },
        unhighlight: function (element) {
            $(element).closest('.form-control').removeClass('is-invalid');
        },
        //yang dilakukan jika rules nya terpenuhi semua
        submitHandler: function (form) {
            postEmployee(); //request POST
        }
    });
        
    event.preventDefault(); //buat nahan ga reload
});

function postEmployee() {
    let data = Object();

    data.FirstName = $("#inputFirstName").val();
    data.LastName = $("#inputLastName").val();
    data.Phone = $("#inputPhone").val();
    data.BirthDate = $("#inputBirthDate").val();
    data.Salary = parseInt($("#inputSalary").val());
    data.Email = $("#inputEmail").val();
    data.Gender = parseInt($("#inputGender").val());
    data.Password = $("#inputPassword").val();
    data.Degree = $("#inputDegree").val();
    data.GPA = parseFloat($("#inputGPA").val());
    data.UniversityId = parseInt($("#inputUniv").val());

    console.log(data);
    console.log(JSON.stringify(data));

    $.ajax({
        url: "https://localhost:44367/Api/Employees/Register",
        type: "POST",
        headers: {
            'Accept': 'application/json',
            'Content-Type':'application/json'
        },
        dataType: "json",
        data: JSON.stringify(data)
    }).done((result) => {
        console.log("success");
        Swal.fire({
            title: 'Success!',
            text: 'Data successfuly inserted',
            icon: 'success',
            confirmButtonText: 'Cool'
        })
        $('#emp-create-modal').modal('hide');
        empTable.DataTable().ajax.reload();
    }).fail((error) => {
        Swal.fire({
            title: 'Error!',
            text: 'Do you want to continue',
            icon: 'error',
            confirmButtonText: 'Cool'
        })
    })
}

function getDetails(nik) {
    $.ajax({
        url: "https://localhost:44367/Api/Employees/Registered"
    }).done((result) => {
        let selectedObj;
        console.log(result.data);
        Object.entries(result.data).forEach(([key, val]) => {
            //console.log(key); // the name of the current key.
            //console.log(val); // the value of the current key.

            if (val.nik == nik) {
                selectedObj = val;
            }
        });

        console.log(selectedObj);
        document.getElementById("inputFirstNameUpdate").value = selectedObj.fullName;
        document.getElementById("inputDegreeUpdate").value = selectedObj.degree;
        document.getElementById("inputEmailUpdate").value = selectedObj.email;
        document.getElementById("inputGenderUpdate").value = selectedObj.gender;
        document.getElementById("inputGPAUpdate").value = selectedObj.gpa;
        document.getElementById("inputPhoneUpdate").value = selectedObj.phoneNumber;
        document.getElementById("inputSalaryUpdate").value = selectedObj.salary;
        document.getElementById("inputBirthDateUpdate").value = selectedObj.birthDate;
    }).fail((error) => {
        console.log(error);
    });
}

function deleteEmployee(nik) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: "https://localhost:44367/Api/Employees/" + nik,
                type: "DELETE",
                headers: {
                    'Accept': 'application/json',
                    'Content-Type': 'application/json'
                },
                dataType: "json",
            }).done((result) => {
                console.log("success");
                Swal.fire({
                    title: 'Deleted!',
                    text: 'Data successfuly deleted',
                    icon: 'success',
                    confirmButtonText: 'Cool'
                })
                empTable.DataTable().ajax.reload();
            }).fail((error) => {
                Swal.fire({
                    title: 'Error!',
                    text: 'Do you want to continue',
                    icon: 'error',
                    confirmButtonText: 'Cool'
                })
            })
        }
    })
}