$(document).ready(function () {
    document.title = 'Categories';

    $("#tblCategories").DataTable({
        paging: true,
        select: true,
        "order": [[0, "desc"]],
        dom: 'Bfrtip',


        buttons: [
            'pageLength',
        ],


        "processing": true,
        "serverSide": true,
        "filter": true, //Search Box
        "orderMulti": false,
        "stateSave": true,

        "ajax": {
            "url": "/Categories/GetDataTabelData",
            "type": "POST",
            "datatype": "json"
        },


        "columns": [
            {
                data: "id", "name": "id", render: function (data, type, row) {
                    return "<a href='#' onclick=Details('" + row.id + "');>" + row.id + "</a>";
                }
            },
            { "data": "name", "name": "name" },
            { "data": "description", "name": "description" },
            {
                "data": "createdDate",
                "name": "createdDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "createdBy", "name": "createdBy" },
            {
                "data": "modifiedDate",
                "name": "modifiedDate",
                "autoWidth": true,
                "render": function (data) {
                    var date = new Date(data);
                    var month = date.getMonth() + 1;
                    return (month.length > 1 ? month : month) + "/" + date.getDate() + "/" + date.getFullYear();
                }
            },
            { "data": "modifiedBy", "name": "modifiedBy" },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-info btn-xs' onclick=AddEdit('" + row.id + "');>Edit</a>";
                }
            },
            {
                data: null, render: function (data, type, row) {
                    return "<a href='#' class='btn btn-danger btn-xs' onclick=Delete('" + row.id + "'); >Delete</a>";
                }
            }
        ],

        'columnDefs': [{
            'targets': [7, 8],
            'orderable': false,
        }],
        "lengthMenu": [[10, 20, 15, 25, 50, 100, 200], [20, 10, 15, 25, 50, 100, 200]]
    });

});

