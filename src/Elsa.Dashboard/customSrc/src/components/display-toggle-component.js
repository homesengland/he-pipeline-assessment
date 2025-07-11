export class DisplayToggle {
    constructor(component) {
        this.component = component;
        component.dictionary = {};
    }
    onToggleDisplay(index) {
        let tempValue = this.toggleDictionaryDisplay(index, this.component.dictionary);
        this.component.dictionary = Object.assign(Object.assign({}, this.component.dictionary), { tempValue });
    }
    toggleDictionaryDisplay(index, dict) {
        let tempValue = Object.assign(dict);
        let tableRowClass = dict[index];
        if (tableRowClass == null) {
            tempValue[index] = this.component.displayValue;
        }
        else {
            dict[index] == this.component.hiddenValue ? tempValue[index] = this.component.displayValue : tempValue[index] = this.component.hiddenValue;
        }
        return tempValue;
    }
}
