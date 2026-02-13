// Prevents scroll-to-top when clicking DataTable tabs
(function() {
    'use strict';

    /**
     * Initializes tab scroll prevention for a specific data table
     * @param {string} tabsId - The ID of the tabs container
     */
    function initializeDataTableTabs(tabsId) {
        var tabsContainer = document.getElementById(tabsId);
        if (!tabsContainer) {
            return;
        }

        var tabLinks = tabsContainer.querySelectorAll('.govuk-tabs__tab');
        tabLinks.forEach(function(link) {
            link.addEventListener('click', function(e) {
                // Store current scroll position (using modern scrollY instead of deprecated pageYOffset)
                var scrollTop = window.scrollY || document.documentElement.scrollTop;
                
                // Allow GOV.UK Frontend to handle tab switching
                setTimeout(function() {
                    // Restore scroll position after tab switch
                    window.scrollTo(0, scrollTop);
                }, 0);
            });
        });
    }

    // Expose the function globally for use in Razor views
    window.DataTableTabs = {
        init: initializeDataTableTabs
    };
})();