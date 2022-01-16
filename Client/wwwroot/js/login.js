//dijalankan kalo kita klik submit
$("#form-login").submit(function (event) {
/*    event.preventDefault(); //buat nahan ga reload*/
});

$("#form-login").validate({
    //validasi
    rules: {
        //atribut name
        Email: {
            required: true,
            email: true
        },
        Password: {
            required: true
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
    }
});