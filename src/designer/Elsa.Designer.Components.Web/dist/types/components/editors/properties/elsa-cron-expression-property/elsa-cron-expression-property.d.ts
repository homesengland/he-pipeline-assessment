import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "../../../../models";
export declare class ElsaCronExpressionProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  currentValue: string;
  valueDescription: string;
  onChange(e: Event): void;
  componentWillLoad(): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  updateDescription(): void;
  render(): any;
}
