import { EventEmitter } from '../../../../stencil-public-runtime';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, SelectListItem, SelectList, ActivityModel } from "../../../../models";
export declare class ElsaMultiTextProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  serverUrl: string;
  currentValue?: string;
  valueChange: EventEmitter<Array<string | number | boolean | SelectListItem>>;
  selectList: SelectList;
  componentWillLoad(): Promise<void>;
  onValueChanged(newValue: Array<string | number | boolean | SelectListItem>): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  createKeyValueOptions(options: Array<SelectListItem>): SelectListItem[];
  componentWillRender(): Promise<void>;
  render(): any;
}
