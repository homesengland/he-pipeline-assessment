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

            // Handle multi-select columns (Project Manager, Local Authority)
            this.api()
                .columns($('[data-multisearchable]'))
                .every(function () {
                    var column = this;
                    var colName = column.header().textContent.replaceAll(/\s/g, '');
                    var dropdownId = colName + '_dropdown';

                    var label = $('<div class="govuk-input__item"><div id="' + colName + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + '</label></div></div>')
                        .appendTo('#' + searchElementId);

                    // Wrapper for positioning
                    var wrapper = $('<div class="custom-multiselect-wrapper"></div>');
                    
                    // Create select-styled div
                    var toggle = $('<div class="govuk-select margin-right-10 custom-multiselect-toggle" id="' + dropdownId + '_toggle" tabindex="0">' +
                        '<span class="multiselect-value"></span>' +
                    '</div>');

                    // Create dropdown panel
                    var panel = $('<div class="custom-multiselect-panel" id="' + dropdownId + '_panel" style="display: none;">' +
                        '<div class="govuk-checkboxes" data-module="govuk-checkboxes"></div>' +
                    '</div>');

                    wrapper.append(toggle).append(panel);
                    label.find('.govuk-form-group').append(wrapper);

                    var checkboxContainer = panel.find('.govuk-checkboxes');

                    // Build checkboxes
                    column.data().unique().sort().each(function (d, j) {
                        var dom_nodes = $($.parseHTML(d));
                        var node = dom_nodes[0];
                        var value = node?.innerText != null ? node.innerText : d;
                        
                        if (value) {
                            var checkboxId = colName + '_checkbox_' + j;
                            var checkbox = 
                                '<div class="govuk-checkboxes__item">' +
                                    '<input class="govuk-checkboxes__input" id="' + checkboxId + '" type="checkbox" value="' + value + '">' +
                                    '<label class="govuk-label govuk-checkboxes__label" for="' + checkboxId + '">' + value + '</label>' +
                                '</div>';
                            checkboxContainer.append(checkbox);
                        }
                    });

                    // Toggle dropdown
                    toggle.on('click', function (e) {
                        e.stopPropagation();
                        var isOpen = panel.is(':visible');
                        
                        // Close all dropdowns
                        $('.custom-multiselect-panel').hide();
                        
                        // Toggle this one
                        if (!isOpen) {
                            panel.show();
                        }
                    });

                    // Update display and filter
                    function updateSelection() {
                        var checked = checkboxContainer.find('input:checked');
                        var count = checked.length;
                        
                        toggle.find('.multiselect-value').text(count > 0 ? count + ' selected' : '');
                        
                        var selectedValues = [];
                        checked.each(function () {
                            selectedValues.push($.fn.dataTable.util.escapeRegex($(this).val()));
                        });

                        if (selectedValues.length > 0) {
                            var searchRegex = '^(' + selectedValues.join('|') + ')$';
                            column.search(searchRegex, true, false).draw();
                        } else {
                            column.search('', true, false).draw();
                        }
                    }

                    checkboxContainer.on('change', 'input[type="checkbox"]', updateSelection);

                    // Close on outside click
                    $(document).on('click', function (e) {
                        if (!$(e.target).closest('#' + dropdownId + '_toggle, #' + dropdownId + '_panel').length) {
                            panel.hide();
                        }
                    });
                });

            // Handle single-select columns (Status)
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
                            var dom_nodes = $($.parseHTML(d));
                            var node = dom_nodes[0];
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


            //Prototype for date to and from filter
            this.api().columns('[data-datesearchable]').every(function () {
                var column = this;
                var th = $(column.header());
                var colName = th.text().replaceAll(/\s/g, '');
                var fromInputId = colName + '_fromDate';
                var toInputId = colName + '_toDate';

                // Create a new grid row for the date filters
                var gridRow = $(
                    '<div class="govuk-grid-row" style="margin-top: 16px;">' +
                        '<div class="govuk-grid-column-one-half">' +
                            '<div class="govuk-form-group">' +
                                '<label class="govuk-label govuk-date-input__label" for="' + fromInputId + '">From Last Updated</label>' +
                                '<input type="date" id="' + fromInputId + '" class="govuk-input" placeholder="YYYY-MM-DD"/>' +
                            '</div>' +
                        '</div>' +
                        '<div class="govuk-grid-column-one-half">' +
                            '<div class="govuk-form-group">' +
                                '<label class="govuk-label govuk-date-input__label" for="' + toInputId + '">To Last Updated</label>' +
                                '<input type="date" id="' + toInputId + '" class="govuk-input" placeholder="YYYY-MM-DD"/>' +
                            '</div>' +
                        '</div>' +
                    '</div>'
                );
                // Append the grid row after the other filters
                $('#' + searchElementId).append(gridRow);

                // Custom filter for "date from" and "date to"
                $.fn.dataTable.ext.search.push(function (settings, data, dataIndex) {
                    if (settings.nTable.id !== column.table().node().id) return true; // Only apply to this table

                    var cellHtml = data[column.index()];
                    // Get first 8 characters which is in YYYYMMDD format
                    var rowDate = cellHtml.substring(0, 8);

                    var from = $('#' + fromInputId).val();
                    var to = $('#' + toInputId).val();
                    var fromVal = from ? from.replaceAll('-', '') : '';
                    var toVal = to ? to.replaceAll('-', '') : '';

                    // If no from/to date, show all. Otherwise, only show rows in range.
                    if ((fromVal === '' || rowDate >= fromVal) && (toVal === '' || rowDate <= toVal)) {
                        return true;
                    }
                    return false;
                });

                // Redraw table on date input change
                $('#' + fromInputId + ', #' + toInputId).on('change', function () {
                    column.table().draw();
                });
            });

            
            //#region Prototype for date string matching filter
            //this.api().columns('[data-datesearchable]').every(function () {
            //    var column = this;
            //    var th = $(column.header());
            //    var colName = th.text().replaceAll(/\s/g, '');
            //    var container = $('<div class="govuk-input__item"><div id="' + colName + '_datesearch" class="govuk-form-group"></div></div>')
            //        .appendTo('#' + searchElementId);

            //    var label = $('<label class="govuk-label govuk-date-input__label">' + th.text() + '</label>');
            //    var input = $('<input type="date" class="govuk-input margin-right-10" aria-labelledby="' + colName + '_datesearch" placeholder="YYYY-MM-DD"/>')
            //        .on('input', function () {
            //            var val = $(this).val();
            //            // Convert to long date format (e.g., "22 January 2026") to match table display
            //            var longDate = "";
            //            if (val) {
            //                var dateObj = new Date(val);
            //                longDate = dateObj.toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' });
            //                console.log(longDate);
            //            }
            //            //Using partial match regex as DataTables searches for both hidden and visible data in the table. Look into custom render for date column if implementing in future
            //            column.search(longDate ? longDate : '', true, false).draw();
            //        });

            //    label.appendTo(container.find('.govuk-form-group'));
            //    input.appendTo(container.find('.govuk-form-group'));
            //});
           //#endregion
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