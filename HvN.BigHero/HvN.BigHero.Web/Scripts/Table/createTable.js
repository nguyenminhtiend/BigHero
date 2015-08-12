function nameFormatter(value) {
    return '<a href="javascript:void(0)" class="edit">' + value + '</a>';

};

$(document).ready(function () {
    var rows = [];
    var isNew = false;
    var editRow, idex;

    $('#AddColumn').prop('disabled', true);
    $('#saveTable').prop('disabled', true);
    $('#save').prop('disabled', true);

    window.operateEvents = {
        'click .edit': function (e, value, row, index) {
            $('#columnName').val(row.name);
            $('#datatype').val(row.datatype);
            $('#length').val(row.size);
            $('#allowNull').val(row.nullable);
            $('#displayName').val(row.display);
            $('#formAddColumn').modal();
            isNew = false;
            editRow = row;
            idex = index;
        }
    }

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
        $('#valid').hide();
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
        isNew = true;
        $('#formAddColumn').modal();
    });

    $('#save').click(function () {
        if (!isExistedColum($('#columnName').val())) {
            if (isNew) {
                getRows();
            }
            else {
                updateRow(editRow, idex);
            }
        }
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

    function updateRow(row, idex) {
        $('#tblColumns').bootstrapTable('updateRow', {
            index: idex,
            row: {
                name: $('#columnName').val(),
                datatype: $('#datatype option:selected').val(),
                size: $('#length').val(),
                nullable: $('#allowNull:checked').val() == "on" ? true : false,
                display: $('#displayName').val()
            }
        });
        $('#formAddColumn').modal('hide');
        loadData();

    }

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

    function isExistedColum(columnName) {
        var length = rows.length;
        for (var i = 0; i < length; i++) {
            if (rows[i].name == columnName) {
                $('#checkColumn').html("This column is existed");
                return true;
            } else {
                $('#checkColumn').html("");
                return false;
            }
        }
    }
    $('#delColumn').click(function () {
        var delRow = $('#tblColumns').bootstrapTable('getSelections');
        for (var i = 0; i < rows.length; i++) {
            for (var j = 0; j < delRow.length; j++) {
                if (rows[i].name === delRow[j].name) {
                    rows.splice(i, 1);
                }
            }
        }

        loadData();
    });
});