$(document).ready(function () {
    var rows = [];
    $('#AddColumn').prop('disabled', true);
    $('#saveTable').prop('disabled', true);
    $('#save').prop('disabled', true);

    $("input.mandatory").blur(function () {
        var isDiable = false;
        $("input.mandatory").each(function (index) {
            if ($.trim($(this).val()) === '') {
                isDiable = true;
            }
        });
        $('#save').prop('disabled', isDiable);
    });

    $('#tableName').blur(function (e) {
        var tableName = $(this).val();
        $.ajax({
            url: '/Table/CheckExistTable',
            type: 'POST',
            data: { tableName: tableName },
            beforeSend: function () {

                $('#loading').show();

            },
            success: function (data) {
                $('#checkTable').html("");
                if (data === true) {
                    $('#valid').hide();
                    setTimeout(function () {
                        $('#loading').hide();
                        $('#checkTable').html("Table name is already existed.");
                    }, 1000);

                    $('#AddColumn').prop('disabled', true);
                } else {
                    $('#checkTable').html("");
                    setTimeout(function () {
                        $('#loading').hide();
                        $('#valid').show();
                        $('#AddColumn').prop('disabled', false);
                    }, 1000);
                }
            },
            error: function (xhr) {
                alert(xhr.responseText);

            }
        });
    });

    $('#AddColumn').click(function () {
        if ($('#tableName').val() == "")
            $('#checkTable').html("Table name cannot empty.");
        else {
            $('#checkTable').html("");
            $('#formAddColumn').modal();
        }
    });

    $('#save').click(function () {
        getRows();
    });
    $('#saveTable').click(function () {
        $('#columns').val(JSON.stringify(rows));
        $('#createTable-form').submit();
    });
    $('#datatype').change(function () {
        var sel = $('#datatype').val();
        if (sel === "VarChar") {
            $('#length').removeAttr('readonly');
        } else {
            $('#length').attr('readonly', true);
        }
    });
    function getRows() {
        var columnName = $('#columnName').val();

        if (jQuery.isNumeric(columnName)) {
            $('#checkColumn').html("Column name must be string only.");
        } else {
            var row = {
                name: $('#columnName').val(),
                datatype: $('#datatype option:selected').val(),
                size: $('#length').val(),
                nullable: $('#allowNull:checked').val() == "on" ? true : false,
                display: $('#displayName').val()
            }
            rows.push(row);
            $('#formAddColumn').modal('hide');
            loadData();
        }

    }


    function loadData() {
        $('#tblColumns').bootstrapTable('destroy');
        $('#tblColumns').bootstrapTable({
            data: rows
        });
        $('#saveTable').prop('disabled', false);
    }
});