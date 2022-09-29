 // import {Map} from "../../../../../Elsa-Core-HE/src/designer/elsa-workflows-studio/src/utils/utils";
 type Map<T> = {
     [key: string]: T
   };

// export interface MultiChoiceRecord {
//     name: string;
//     expressions?: Map<string>;
//     syntax?: string;
// }

export interface MultiChoiceRecord {
    answer: string;
    isSingle: boolean;
}

export interface ActivityPropertyDescriptor {
  name: string;
  uiHint: string;
  label?: string;
  hint?: string;
  options?: any;
  category?: string;
  defaultValue?: any;
  defaultSyntax?: string;
  supportedSyntaxes: Array<string>;
  isReadOnly?: boolean;
  defaultWorkflowStorageProvider?: string;
  disableWorkflowProviderSelection: boolean;
  considerValuesAsOutcomes: boolean;
}

export interface ActivityDefinitionProperty {
  name: string;
  syntax?: string;
  expressions: Map<string>;
  value?: any;
}

export class SyntaxNames {
  static readonly Literal = "Literal";
  static readonly JavaScript = "JavaScript";
  static readonly Liquid = "Liquid";
  static readonly Json = "Json";
  static Variable: string;
  static Output: string;
}

export interface ActivityModel {
  activityId: string;
  type: string;
  name?: string;
  displayName?: string;
  description?: string;
  outcomes: Array<string>;
  properties: Array<ActivityDefinitionProperty>;
  persistWorkflow?: boolean;
  loadWorkflowContext?: boolean;
  saveWorkflowContext?: boolean;
  propertyStorageProviders: Map<string>;
}


export interface HTMLElsaMultiExpressionEditorElement extends ElsaMultiExpressionEditor, HTMLElement {
  componentOnReady(): Promise<this>;
}

export interface ElsaMultiExpressionEditor {
  "context"?: IntellisenseContext;
  "defaultSyntax": string;
  "editorHeight": string;
  "expressions": Map<string>;
  "fieldName"?: string;
  "isReadOnly"?: boolean;
  "label": string;
  "singleLineMode": boolean;
  "supportedSyntaxes": Array<string>;
  "syntax"?: string;
}

export interface IntellisenseContext {
  activityTypeName: string;
  propertyName: string;
}
