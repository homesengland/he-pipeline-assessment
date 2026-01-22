import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, PropertySettings } from "../../../../models";
import { MonacoValueChangedArgs } from "../../../controls/elsa-monaco/elsa-monaco";
export declare class ElsaJsonProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  currentValue: string;
  componentWillLoad(): void;
  getEditorHeight(options?: PropertySettings): "20em" | "15em";
  onMonacoValueChanged(e: MonacoValueChangedArgs): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  render(): any;
}
