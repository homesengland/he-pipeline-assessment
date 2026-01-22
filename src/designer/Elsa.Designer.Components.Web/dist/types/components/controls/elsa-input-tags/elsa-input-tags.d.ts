import { EventEmitter } from '../../../stencil-public-runtime';
export declare class ElsaInputTags {
  fieldName?: string;
  fieldId?: string;
  placeHolder?: string;
  values?: Array<string>;
  valueChanged: EventEmitter<Array<string>>;
  currentValues?: Array<string>;
  valuesChangedHandler(newValue: Array<string>): void;
  componentWillLoad(): void;
  addItem(item: string): Promise<void>;
  onInputKeyDown(e: KeyboardEvent): Promise<void>;
  onInputBlur(e: Event): Promise<void>;
  onDeleteTagClick(e: Event, tag: string): Promise<void>;
  render(): any;
}
