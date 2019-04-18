/*
|--------------------------------------------------------------------------
| DataTable
|--------------------------------------------------------------------------
*/

APP.component.DataTable = {

    init: function (_nomeTable) {

        this.setup();
        return this.setDataTable(_nomeTable);

    },

    setup: function () {

        //

    },

    setDataTable : function (_nomeTable) {
            table = $(_nomeTable).DataTable({
                "destroy": true,
                "responsive": true,
                "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, _options.tabelaTodosRegistros]],
                "iDisplayLength": 10,
                "language": {
                    "lengthMenu": _options.tabelaLengthMenu,
                    "zeroRecords": _options.tabelaZeroRecords,
                    "info": _options.tabelaInfo,
                    "infoEmpty": _options.tabelaInfoEmpty,
                    "infoFiltered": _options.tabelaInfoFiltered,
                    "search": _options.tabelaSearch,
                    "searchPlaceholder": _options.tabelaSearchPlaceholder,
                    "paginate": {
                        "next": _options.tabelaPaginateNext,
                        "previous": _options.tabelaPaginatePrevious
                    }
                },
                "createdRow": function (row, data, dataIndex) {
                    $(row).children().addClass("details-control");
                    $(row).attr("id", dataIndex);
                },

            });

            // Apply the search
            $("table thead th input[type=text]").on('keyup change', function () {
                table
                    .column($(this).closest('th').index() + ':visible')
                    .search(this.value)
                    .draw();
            });
            // Apply the search
            $("table thead th select").on('change', function () {
                var val = $.fn.dataTable.util.escapeRegex(
                    $(this).val()
                );
                table
                    .column($(this).closest('th').index() + ':visible')
                    .search(val ? '^' + val + '$' : '', true, false)
                    .draw();
            });

            table.on('responsive-resize', function (e, datatable, columns) {
                var i = 0;
                $("#hr1 > th").each(function () {
                    if (columns[i]) {
                        $(this).removeClass("hide");
                    } else {
                        $(this).addClass("hide");
                    }
                    i++;
                });
            });

            $(table.column(0).header()).addClass('never');
            table.responsive.rebuild();
            table.responsive.recalc();

            $(table.column(0).header()).removeClass('never');
            table.responsive.rebuild();
            table.responsive.recalc();
    },

    setDataTableParam: function (_nomeTable, _positionConlumn, _ascOrDesc, searchPlaceholder) {

        table = $(_nomeTable).DataTable({
            "destroy": true,
            "responsive": true,
            "lengthMenu": [[10, 50, 100, -1], [10, 50, 100, _options.tabelaTodosRegistros]],
            "iDisplayLength": 10,
            "order": [[_positionConlumn, _ascOrDesc]],
            "language": {
                "lengthMenu": _options.tabelaLengthMenu,
                "zeroRecords": _options.tabelaZeroRecords,
                "info": _options.tabelaInfo,
                "infoEmpty": _options.tabelaInfoEmpty,
                "infoFiltered": _options.tabelaInfoFiltered,
                "search": _options.tabelaSearch,
                "searchPlaceholder": searchPlaceholder,
                "paginate": {
                    "next": _options.tabelaPaginateNext,
                    "previous": _options.tabelaPaginatePrevious
                }
            },
            "createdRow": function (row, data, dataIndex) {
                $(row).children().addClass("details-control");
                $(row).attr("id", dataIndex);
            },

        });

        // Apply the search
            $("table thead th input[type=text]").on('keyup change', function () {
                table
                    .column($(this).closest('th').index() + ':visible')
                    .search(this.value)
                    .draw();
            });
            // Apply the search
            $("table thead th select").on('change', function () {
                var val = $.fn.dataTable.util.escapeRegex(
                    $(this).val()
                );
                table
                    .column($(this).closest('th').index() + ':visible')
                    .search(val ? '^' + val + '$' : '', true, false)
                    .draw();
            });

            table.on('responsive-resize', function (e, datatable, columns) {
                var i = 0;
                $("#hr1 > th").each(function () {
                    if (columns[i]) {
                        $(this).removeClass("hide");
                    } else {
                        $(this).addClass("hide");
                    }
                    i++;
                });
            });

            $(table.column(0).header()).addClass('never');
            table.responsive.rebuild();
            table.responsive.recalc();

            $(table.column(0).header()).removeClass('never');
            table.responsive.rebuild();
            table.responsive.recalc();
        
        resizeTable(table);

    },

};