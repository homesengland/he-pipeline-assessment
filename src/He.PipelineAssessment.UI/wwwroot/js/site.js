﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
var groupCheckboxes = document.querySelectorAll('input[group][type="checkbox"]');

let enabledSettings = []

var currencyInputs = document.querySelectorAll('input[currency-formatter="true"]');

// Use Array.forEach to add an event listener to each checkbox.
groupCheckboxes.forEach(function (checkbox) {
    checkbox.addEventListener('change', function () {
        var hasBehaviourExclusiveToQuestion = (checkbox.getAttribute('group-data-behaviour') === 'exclusivetoquestion');
        if (hasBehaviourExclusiveToQuestion) {
            unCheckAllInputsInQuestionExcept(checkbox);
        } else {
            unCheckExclusiveInputsInQuestion(checkbox);
            var hasBehaviourExclusiveToGroup = (checkbox.getAttribute('group-data-behaviour') === 'exclusivetogroup');
            if (hasBehaviourExclusiveToGroup) {
                unCheckAllInputsInGroupExcept(checkbox);
            } else {
                unCheckExclusiveInputsInGroup(checkbox);
            }
        }
    });
});

currencyInputs.forEach(function (input) {
    input.addEventListener('keyup', function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;
        // format number
        input.value = input.value
            .replace(/\D/g, "")
            .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
            ;
    })
})

unCheckAllInputsInGroupExcept = function ($input) {
    var allInputsWithSameGroup = document.querySelectorAll('input[group="' + $input.getAttribute("group") + '"][type="checkbox"]');
    nodeListForEach(allInputsWithSameGroup, function ($inputWithSameGroup) {
        var hasSameFormOwner = ($input.form === $inputWithSameGroup.form);
        if (hasSameFormOwner && $inputWithSameGroup !== $input) {
            $inputWithSameGroup.checked = false;
        }
    });
};

unCheckAllInputsInQuestionExcept = function ($input) {
    var allInputsWithSameGroup = document.querySelectorAll('input[name="' + $input.getAttribute("name") + '"][type="checkbox"]');
    nodeListForEach(allInputsWithSameGroup, function ($inputWithSameGroup) {
        var hasSameFormOwner = ($input.form === $inputWithSameGroup.form);
        if (hasSameFormOwner && $inputWithSameGroup !== $input) {
            $inputWithSameGroup.checked = false;
        }
    });
};

unCheckExclusiveInputsInGroup = function ($input) {
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

unCheckExclusiveInputsInQuestion = function ($input) {
    var allInputsWithSameGroupAndExclusiveBehaviour = document.querySelectorAll(
        'input[name="' + $input.getAttribute("name") + '"][group-data-behaviour="exclusivetoquestion"][type="checkbox"]'
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



