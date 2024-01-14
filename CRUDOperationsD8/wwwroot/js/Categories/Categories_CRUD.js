var Details = function (id) {
    var url = "/Categories/Details?id=" + id;
    $('#titleBigModal').html("Categories Details");
    loadBigModal(url);
};

var AddEdit = function (id) {
    var url = "/Categories/AddEdit?id=" + id;
    if (id > 0) {
        $('#titleBigModal').html("Edit Categories");
    }
    else {
        $('#titleBigModal').html("Add Categories");
    }
    loadBigModal(url);
};

var Save = function () {

    var _frmCategories = $("#frmCategories").serialize();
    $("#btnSave").val("Please Wait");
    $('#btnSave').attr('disabled', 'disabled');
    $.ajax({
        type: "POST",
        url: "/Categories/AddEdit",
        data: _frmCategories,
        success: function (result) {
            if (result.isSuccess) {
                document.getElementById("btnClose").click();
                $("#btnSave").val("Save");
                $('#btnSave').removeAttr('disabled');
                $('#tblCategories').DataTable().ajax.reload();

                SwalSimpleAlert(result.alertMessage, "info");
            }
            else {
                SwalSimpleAlert(result.alertMessage, "warning");
            }
        },
        error: function (errormessage) {
            SwalSimpleAlert(errormessage.responseText, "warning");
        }
    });
}

var Delete = function (id) {
    Swal.fire({
        title: 'Do you want to delete this item?',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                type: "DELETE",
                url: "/Categories/Delete?id=" + id,
                success: function (result) {
                    var message = "Categories has been deleted successfully. Categories ID: " + result.id;
                    SwalSimpleAlert(message, "info");
                    $('#tblCategories').DataTable().ajax.reload();
                }
            });
        }
    });
};
