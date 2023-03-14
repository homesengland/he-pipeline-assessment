function initDataTable(tableId, columnNumberToSort, searchElementId) {
    $('#' + tableId).DataTable({
        "bLengthChange": false,
        order: [[columnNumberToSort, 'asc']],
        "columnDefs": [
            { "width": "5%", "targets": 0 },
            { "width": "15%", "targets": 1 },
            { "width": "15%", "targets": 2 },
            { "width": "10%", "targets": 3 },
            { "width": "10%", "targets": 4 },
            { "width": "5%", "targets": 5 },
            { "width": "10%", "targets": 6 },
            { "width": "10%", "targets": 7 },
            { "width": "10%", "targets": 8 },
            { "width": "10%", "targets": 9 }
        ],
        "oLanguage": {
            "sSearch": "Search"
        },

        initComplete: function () {
            $('#' + tableId).removeAttr("hidden");
            $('#' + tableId).removeAttr("style");
            $("#" + tableId + "_filter").detach().appendTo('#filter-div');
            $(".dataTables_filter").children('label').children("input[type='search']").addClass('govuk-input');
            $(".dataTables_filter").children('label').addClass('govuk-label');

            this.api()
                .columns($('[data-searchable]'))
                
                .every(function () {
                    var column = this;

                    var label = $('<div class="govuk-input__item"><div id="' + column.header().textContent.replaceAll(/\s/g, '') + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + ' </label></div></div>')
                        .appendTo('#'+searchElementId);

                    var select = $('<select class="govuk-select margin-right-10"><option value=""></option></select>')
                        .appendTo('#' + column.header().textContent.replaceAll(/\s/g, ''))
                        .on('change', function () {
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());

                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });

                    column
                        .data()
                        .unique()
                        .sort()
                        .each(function (d, j) {

                            var dom_nodes = $($.parseHTML(d));
                            var node = dom_nodes[0];
                            if (node.innerText != null) {
                                select.append('<option value="' + node.innerText + '">' + node.innerText + '</option>');
                            }
                            else {
                                select.append('<option value="' + d + '">' + d + '</option>');
                            }

                        });
                });
        },

    });   
}