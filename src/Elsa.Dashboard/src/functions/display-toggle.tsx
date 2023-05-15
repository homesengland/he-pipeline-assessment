import { Map } from '../utils/utils'


export function ToggleDictionaryDisplay(index: number, dict: Map<string>, displayValue: string = "table-row", hiddenValue: string = "none"): any {
  let tempValue = Object.assign(dict);
  let tableRowClass = dict[index];
  if (tableRowClass == null) {
    tempValue[index] = displayValue;
  } else {
    dict[index] == hiddenValue ? tempValue[index] = displayValue : tempValue[index] = hiddenValue;
  }
  return tempValue;
}
