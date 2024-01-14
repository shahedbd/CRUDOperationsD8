

var loadBigModal = function (url) {
    $("#BigModalDiv").load(url, function () {
        $("#BigModal").modal("show");
        $('#Name').focus();
    });
};

var SwalSimpleAlert = function (Message, icontype) {
    Swal.fire({
        title: Message,
        icon: icontype
    });
}