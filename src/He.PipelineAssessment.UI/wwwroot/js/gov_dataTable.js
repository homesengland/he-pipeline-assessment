function initDataTable(tableId, columnNumberToSort, searchElementId) {
    // $ = jQuery function
    // Selects the table by ID and initialises the DataTable plugin (jQuery plugin for making tables)
    $('#' + tableId).DataTable({
        "bLengthChange": false, // disables users ability to change the number of rows displayed.
        order: [[columnNumberToSort, 'asc']], // default sorting for the table
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
        //Search box label shown on the index page.
        "oLanguage": {
            "sSearch": "Search"
        },

        // function that gets called when DataTable has finished initialising
        initComplete: function () {
            $('#' + tableId).removeAttr("hidden"); // removes hidden attribute to show the table
            $('#' + tableId).removeAttr("style"); // removes any inline styles
            $("#" + tableId + "_filter").detach().appendTo('#filter-div'); // moves the search box to a custom div
            $(".dataTables_filter").children('label').children("input[type='search']").addClass('govuk-input');
            $(".dataTables_filter").children('label').addClass('govuk-label');

            // Access the jQuery DataTable API
            this.api()
                .columns($('[data-searchable]')) // Selects columns with the data-searchable attribute
                // Iterate over each selected column
                .every(function () {
                    var column = this;

                    // creates a label gets the header text of the column and formats it to be used as the id
                    var label = $('<div class="govuk-input__item"><div id="' + column.header().textContent.replaceAll(/\s/g, '') + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + ' </label></div></div>')
                        .appendTo('#' + searchElementId); // appends the label to the search element div

                    // Create a <select> dropdown for filtering this column
                    // - .addClass() adds CSS classes for styling
                    // - .appendTo() places the dropdown inside the label div
                    // - .on('change', ...) sets up an event handler for when the user selects an option
                    var select = $('<select class="govuk-select margin-right-10" aria-labelledby="' + column.header().textContent.replaceAll(/\s/g, '') + '" ><option value=""></option></select>')
                        .appendTo('#' + column.header().textContent.replaceAll(/\s/g, ''))
                        .on('change', function () {
                            // Escape the selected value for use in a regex search
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());

                            // Filter the table by the selected value (exact match)
                            // - column.search() applies the filter
                            // - .draw() updates the table display

                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });

                    // Populate the dropdown with unique values from the column
                    column
                        .data() // Get all data for this column
                        .unique() // Get only unique values
                        .sort() // Sort the values alphabetically
                        .each(function (d, j) {
                            console.log("J", j)
                            // Parse the value as HTML (in case it's HTML content)
                            var dom_nodes = $($.parseHTML(d));
                            var node = dom_nodes[0];
                            console.log("Node", node);
                            var option = document.createElement('option');
                            // If the node has innerText, use it for the option value/text
                            if (node?.innerText != null) {
                                option.value = node.innerText;
                                option.textContent = node.innerText;
                            }
                            // Otherwise, use the raw data value
                            else {
                                option.value = d;
                                option.textContent = d;
                            }
                            // Add the option to the dropdown
                            select.append(option);
                        });
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
        // Pre-select the 'Pending' value in the search pane for column 4
        searchPanes: {
            preSelect: [{
                rows: ['Pending'],
                column: 4
            }]
        },
        // Apply a default search filter for column 4 ('Pending')
        "searchCols": [
            null,
            null,
            null,
            null,
            { "search": "Pending" }
        ],

        // function that gets called when DataTable has finished initialising
        initComplete: function () {
            // removes the hidden and inline style attributes
            $('#' + tableId).removeAttr("hidden");
            $('#' + tableId).removeAttr("style");
            // Move the default search box to a custom div
            $("#" + tableId + "_filter").detach().appendTo('#filter-div');
            // Style the search input and label using GOV.UK classes
            $(".dataTables_filter").children('label').children("input[type='search']").addClass('govuk-input');
            $(".dataTables_filter").children('label').addClass('govuk-label');

            // Access the jQuery DataTable API
            this.api()
                .columns($('[data-searchable]')) // Select columns with data-searchable attribute

                .every(function () {

                    var column = this;
                    console.log(column);
                    var label = $('<div class="govuk-input__item"><div id="' + column.header().textContent.replaceAll(/\s/g, '') + '" class="govuk-form-group"><label class="govuk-label govuk-date-input__label">' + column.header().textContent + ' </label></div></div>')
                        .appendTo('#' + searchElementId);


                    // Create a <select> dropdown for filtering this column
                    var select = $('<select class="govuk-select margin-right-10" aria-labelledby="' + column.header().textContent.replaceAll(/\s/g, '') + '"><option value=""></option></select>')
                        .appendTo('#' + column.header().textContent.replaceAll(/\s/g, ''))
                        .on('change', function () {
                            // Escape the selected value for regex search
                            var val = $.fn.dataTable.util.escapeRegex($(this).val());
                            // Filter the table by the selected value (exact match)
                            column.search(val ? '^' + val + '$' : '', true, false).draw();
                        });

                    // Populate the dropdown with unique values from the column
                    column
                        .data() // Get all data for this column
                        .unique() // Get only unique values
                        .sort() // Sort the values alphabetically
                        .each(function (d, j) {

                            var dom_nodes = $($.parseHTML(d)); // Parse the value as HTML 
                            var node = dom_nodes[0];
                            var option = document.createElement('option');

                            // 
                            if (node.innerText != null) {
                                // if value is pending, set it as selected by default
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