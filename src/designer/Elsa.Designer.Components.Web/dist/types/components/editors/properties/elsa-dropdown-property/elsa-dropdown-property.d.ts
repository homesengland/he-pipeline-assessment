import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList } from "../../../../models";
export declare class ElsaDropdownProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  serverUrl: string;
  currentValue?: string;
  selectList: SelectList;
  componentWillLoad(): Promise<void>;
  private reloadSelectListFromDeps;
  private onChange;
  private onDefaultSyntaxValueChanged;
  render(): any;
}
