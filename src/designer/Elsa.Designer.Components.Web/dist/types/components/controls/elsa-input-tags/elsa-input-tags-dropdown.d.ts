import { EventEmitter } from '../../../stencil-public-runtime';
import { SelectListItem } from '../../../models';
export declare class ElsaInputTagsDropdown {
  fieldName?: string;
  fieldId?: string;
  placeHolder?: string;
  values?: Array<string | SelectListItem>;
  dropdownValues?: Array<SelectListItem>;
  valueChanged: EventEmitter<Array<string | SelectListItem>>;
  currentValues?: Array<SelectListItem>;
  valuesChangedHandler(newValue: Array<string | SelectListItem>): void;
  componentWillLoad(): void;
  onTagSelected(e: any): Promise<void>;
  onDeleteTagClick(e: any, currentTag: SelectListItem): Promise<void>;
  render(): any;
}
