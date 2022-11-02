function initDataTable (tableId) {
    $('#'+tableId).DataTable({
        order: [[6, 'desc']],

        initComplete: function () {
            this.api()
                .columns($('[data-searchable]'))
                .every(function () {
                    var column = this;

                    var label = $('<div class="govuk-date-input__item"><div id="' + column.header().textContent.replaceAll(/\s/g, '') + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + ' </label></div></div>')
                        .appendTo('#search-div');

                    var select = $('<select class="govuk-select"><option value=""></option></select>')
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