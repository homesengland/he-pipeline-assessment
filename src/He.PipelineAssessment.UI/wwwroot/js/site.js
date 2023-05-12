// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var groupCheckboxes = document.querySelectorAll('input[group][type="checkbox"]');
let enabledSettings = []

// Use Array.forEach to add an event listener to each checkbox.
groupCheckboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', function () {
        var hasBehaviourExclusiveToQuestion = (checkbox.getAttribute('group-data-behaviour') === 'exclusivetoquestion');
        var hasBehaviourExclusive = (checkbox.getAttribute('group-data-behaviour') === 'exclusivetogroup');
        if (hasBehaviourExclusive) {
            unCheckAllInputsExcept(checkbox);
        } else {
            unCheckExclusiveInputs(checkbox);
        }
    });
});

unCheckAllInputsExcept = function ($input) {
    var allInputsWithSameGroup = document.querySelectorAll('input[group="' + $input.getAttribute("group") + '"][type="checkbox"]');
    nodeListForEach(allInputsWithSameGroup, function ($inputWithSameGroup) {
        var hasSameFormOwner = ($input.form === $inputWithSameGroup.form);
        if (hasSameFormOwner && $inputWithSameGroup !== $input) {
            $inputWithSameGroup.checked = false;
        }
    });
};

unCheckExclusiveInputs = function ($input) {
    var allInputsWithSameGroupAndExclusiveBehaviour = document.querySelectorAll(
        'input[group="' + $input.getAttribute("group") + '"][group-data-behaviour="exclusivetogroup"][type="checkbox"]'
    );

    nodeListForEach(allInputsWithSameGroupAndExclusiveBehaviour, function ($exclusiveInput) {
        var hasSameFormOwner = ($input.form === $exclusiveInput.form);
        if (hasSameFormOwner) {
            $exclusiveInput.checked = false;
        }
    });
};

function nodeListForEach(nodes, callback) {
    if (window.NodeList.prototype.forEach) {
        return nodes.forEach(callback)
    }
    for (var i = 0; i < nodes.length; i++) {
        callback.call(window, nodes[i], i, nodes);
    }
}