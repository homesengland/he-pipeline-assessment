import { Injectable } from '@angular/core';

export interface IDisplayToggle {
  dictionary: { [key: string]: string };
  displayValue: string;
  hiddenValue: string;
}

@Injectable({
  providedIn: 'root',
})
export class DisplayToggleService {
  constructor() {}

  initialize(component: IDisplayToggle): void {
    component.dictionary = {};
  }

  onToggleDisplay(index: any, component: IDisplayToggle): void {
    const tempValue = this.toggleDictionaryDisplay(index, component);
    component.dictionary = { ...component.dictionary, ...tempValue };
  }

  private toggleDictionaryDisplay(index: any, component: IDisplayToggle): { [key: string]: string } {
    const dict = component.dictionary;
    const tempValue = { ...dict };
    const tableRowClass = dict[index];
    if (tableRowClass == null) {
      tempValue[index] = component.displayValue;
    } else {
      tempValue[index] = dict[index] === component.hiddenValue ? component.displayValue : component.hiddenValue;
    }
    return { [index]: tempValue[index] };
  }
}
