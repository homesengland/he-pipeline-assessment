import { Map } from "../utils/utils";



export interface IDisplayToggle {
  dictionary: Map<string>;
  displayValue: string;
  hiddenValue: string;
}

export class DisplayToggle {
  constructor(public component: IDisplayToggle) {
    component.dictionary = {};
  }

  onToggleDisplay(index: number) {
    let tempValue = this.toggleDictionaryDisplay(index, this.component.dictionary)
    this.component.dictionary = { ... this.component.dictionary, tempValue }
  }

  private toggleDictionaryDisplay(index: number, dict: Map<string>): any {
    let tempValue = Object.assign(dict);
    let tableRowClass = dict[index];
    if (tableRowClass == null) {
      tempValue[index] = this.component.displayValue;
    } else {
      dict[index] == this.component.hiddenValue ? tempValue[index] = this.component.displayValue : tempValue[index] = this.component.hiddenValue;
    }
    return tempValue;
  }
}
