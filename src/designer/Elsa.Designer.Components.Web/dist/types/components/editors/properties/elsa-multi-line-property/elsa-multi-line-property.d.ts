import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, PropertySettings } from "../../../../models";
export declare class ElsaMultiLineProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  currentValue: string;
  componentWillLoad(): void;
  getEditorHeight(options?: PropertySettings): {
    propertyEditor: string;
    textArea: number;
  };
  onChange(e: Event): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  render(): any;
}
