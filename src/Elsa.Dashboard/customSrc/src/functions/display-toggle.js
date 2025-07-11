export function toggleDictionaryDisplay(index, dict, displayValue = "table-row", hiddenValue = "none") {
    let tempValue = Object.assign(dict);
    let tableRowClass = dict[index];
    if (tableRowClass == null) {
        tempValue[index] = displayValue;
    }
    else {
        dict[index] == hiddenValue ? tempValue[index] = displayValue : tempValue[index] = hiddenValue;
    }
    return tempValue;
}
