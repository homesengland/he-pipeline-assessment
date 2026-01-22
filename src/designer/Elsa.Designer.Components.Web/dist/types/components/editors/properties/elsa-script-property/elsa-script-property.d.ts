import { ActivityDefinitionProperty, ActivityPropertyDescriptor, ActivityValidatingContext, ConfigureComponentCustomButtonContext, ActivityModel } from "../../../../models";
import { MonacoValueChangedArgs } from "../../../controls/elsa-monaco/elsa-monaco";
export declare class ElsaScriptProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  editorHeight: string;
  singleLineMode: boolean;
  syntax?: string;
  serverUrl: string;
  workflowDefinitionId: string;
  currentValue?: string;
  monacoEditor: HTMLElsaMonacoElement;
  activityValidatingContext: ActivityValidatingContext;
  configureComponentCustomButtonContext: ConfigureComponentCustomButtonContext;
  componentWillLoad(): Promise<void>;
  componentDidLoad(): Promise<void>;
  configureComponentCustomButton(): Promise<void>;
  mapSyntaxToLanguage(syntax: string): any;
  onComponentCustomButtonClick(e: Event): void;
  onMonacoValueChanged(e: MonacoValueChangedArgs): void;
  validate(value: string): void;
  render(): any;
}
