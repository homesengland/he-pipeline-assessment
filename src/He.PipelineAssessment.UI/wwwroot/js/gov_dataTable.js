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

                    var select = $('<select class="govuk-select margin-right-10" aria-labelledby="' + column.header().textContent.replaceAll(/\s/g, '') + '" ><option value=""></option></select>')
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
                            //console.log("J", j)
                            var dom_nodes = $($.parseHTML(d));
                            var node = dom_nodes[0];
                            //console.log("Node", node);
                            var option = document.createElement('option');
                            if (node?.innerText != null) {
                                option.value = node.innerText;
                                option.textContent = node.innerText;
                            }
                            else {
                                option.value = d;
                                option.textContent = d;
                            }
                            select.append(option);
                        });
                });

            this.api().columns('[data-datesearchable]').every(function () {
                var column = this;
                var th = $(column.header());
                var colName = th.text().replaceAll(/\s/g, '');
                var container = $('<div class="govuk-input__item"><div id="' + colName + '_datesearch" class="govuk-form-group"></div></div>')
                    .appendTo('#' + searchElementId);

                var label = $('<label class="govuk-label govuk-date-input__label">' + th.text() + '</label>');
                var input = $('<input type="date" class="govuk-input margin-right-10" aria-labelledby="' + colName + '_datesearch" placeholder="YYYY-MM-DD"/>')
                    .on('input', function () {
                        var val = $(this).val();
                        // Convert to long date format (e.g., "22 January 2026") to match table display
                        var longDate = "";
                        if (val) {
                            var dateObj = new Date(val);
                            longDate = dateObj.toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' });
                            console.log(longDate);
                        }
                        //Using partial match regex as DataTables searches for both hidden and visible data in the table. Look into custom render for date column if implementing in future
                        column.search(longDate ? longDate : '', true, false).draw();
                    });

                label.appendTo(container.find('.govuk-form-group'));
                input.appendTo(container.find('.govuk-form-group'));
            });
        },

    });   
}

function initInterventionDataTable(tableId, columnNumberToSort, searchElementId) {
    $('#' + tableId).DataTable({
        "bLengthChange": false,
        order: [[columnNumberToSort, 'desc']],
        "columnDefs": [
            { "width": "5%", "targets": 0 },
            { "width": "15%", "targets": 1 },
            { "width": "15%", "targets": 2 },
            { "width": "10%", "targets": 3 },
            { "width": "10%", "targets": 4 },
            { "width": "5%", "targets": 5 },
            { "width": "10%", "targets": 6 },
            { "width": "10%", "targets": 7 }
        ],
        "oLanguage": {
            "sSearch": "Search"
        },
        searchPanes: {
            preSelect: [{
                rows: ['Pending'],
                column: 4
            }]
        },
        "searchCols": [
            null,
            null,
            null,
            null,
            { "search": "Pending" }
        ],
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
                    console.log(column);
                    var label = $('<div class="govuk-input__item"><div id="' + column.header().textContent.replaceAll(/\s/g, '') + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + ' </label></div></div>')
                        .appendTo('#' + searchElementId);

                    var select = $('<select class="govuk-select margin-right-10" aria-labelledby="' + column.header().textContent.replaceAll(/\s/g, '') +'"><option value=""></option></select>')
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
                            var option = document.createElement('option');

                            if (node.innerText != null) {
                                var selected = node.innerText == 'Pending' ? 'selected' : '';
                                option.value = node.innerText;
                                option.textContent = node.innerText;
                                if (selected) {
                                    option.setAttribute('selected', 'selected');
                                }
                            } else {
                                option.value = d;
                                option.textContent = d;
                            }
                            select.append(option);

                        });
                });
        },

    });
}