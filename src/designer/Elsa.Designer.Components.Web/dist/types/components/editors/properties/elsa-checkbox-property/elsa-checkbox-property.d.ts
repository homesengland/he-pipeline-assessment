import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "../../../../models";
export declare class ElsaCheckBoxProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  isChecked: boolean;
  componentWillLoad(): Promise<void>;
  onCheckChanged(e: Event): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  render(): any;
}
