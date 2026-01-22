import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList } from "../../../../models";
export declare class ElsaCheckListProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  serverUrl: string;
  currentValue?: string;
  monacoEditor: HTMLElsaMonacoElement;
  selectList: SelectList;
  componentWillLoad(): Promise<void>;
  onCheckChanged(e: Event): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  componentWillRender(): Promise<void>;
  render(): any;
}
