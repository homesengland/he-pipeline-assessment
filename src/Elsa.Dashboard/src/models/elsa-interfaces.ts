import { HTMLStencilElement } from '@stencil/core/internal';
import { Map } from './utils'
//import { x } from '@elsa-workflows/elsa-workflows-studio';
//Not In Use.  Using as a placeholder to prompt developers to draw, and copy any interfaces they need from Elsa directly.

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
  static readonly SQL = "SQL";
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

export interface HTMLElsaExpressionEditorElement extends ElsaExpressionEditor, HTMLStencilElement {
}
var HTMLElsaExpressionEditorElement: {
  prototype: HTMLElsaExpressionEditorElement;
  new(): HTMLElsaExpressionEditorElement;
};

export interface ElsaExpressionEditor {
  "context"?: IntellisenseContext;
  "editorHeight": string;
  "expression": string;
  "language": string;
  "padding": string;
  "serverUrl": string;
  "setExpression": (value: string) => Promise<void>;
  "singleLineMode": boolean;
  "workflowDefinitionId": string;
}

export interface IntellisenseContext {
  activityTypeName: string;
  propertyName: string;
}

export interface SwitchCase {
  name: string;
  expressions?: Map<string>;
  syntax?: string;
}

export interface MonacoValueChangedArgs {
  value: string;
  markers: Array<any>;
}

export interface SelectList {
  items: Array<SelectListItem> | Array<string>;
  isFlagsEnum: boolean;
}

export interface SelectListItem {
  text: string;
  value: string;
}

export interface RuntimeSelectListProviderSettings {
  runtimeSelectListProviderType: string;
  context?: any;
}

export interface CustomMonacoElement {
  "addJavaScriptLib": (libSource: string, libUri: string) => Promise<void>;
  "editorHeight": string;
  "language": string;
  "monacoLibPath": string;
  "padding": string;
  "setValue": (value: string) => Promise<void>;
  "singleLineMode": boolean;
  "value": string;
}
