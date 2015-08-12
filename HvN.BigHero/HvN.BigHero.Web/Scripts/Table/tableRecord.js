$(document).ready(function () {
    tableRecord.init();
});

var tableRecord = (function (module, undefined) {
    'use strict';

    module.init = function () {
        $('.datetimepicker').datetimepicker({
            format: 'MM/DD/YYYY'
        });
        $('#saveRecord').click(function () {
            $('#saveRecord-form').submit();
        });
        $("input.mandatory").blur(function () {
            var isDiable = false;
            $("input.mandatory").each(function(index) {
               if ($.trim($(this).val()) === '') {
                   isDiable = true;
               }
            });
            $('#saveRecord').prop('disabled', isDiable);
        });
        $('.numeric').on('input', function (event) {
            this.value = this.value.replace(/[^0-9]/g, '');
        });
    }

    module.deleteRow = function (tableName, primaryColumn, rowId) {
        $('#confirmationDelete').modal('show');
        $('#deleteRecord').click(function () {
            $('#confirmationDelete').modal('hide');
            $.ajax({
                url: '/Table/DeleteRecord',
                type: 'POST',
                data: { tableName: tableName, primaryColumn: primaryColumn, rowId: rowId },
                success: function (data) {
                    reloadTable(tableName);
                },
                error: function (xhr) {
                    alert(xhr.responseText);
                }
            });
        });

    }

    function reloadTable(tableName) {
        $.ajax({
            url: '/Table/GetTableDetail',
            type: 'GET',
            data: { tableName: tableName },
            success: function (data) {
                $('#tableDetail').empty().html(data);
            },
            error: function (xhr) {
                alert(xhr.responseText);
            }
        });
    }

    return module;
})(tableRecord || {});