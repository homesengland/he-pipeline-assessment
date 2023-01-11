import { HTMLStencilElement } from "@stencil/core/internal";
import { IntellisenseContext } from "../models/elsa-interfaces";


export interface HTMLElsaMonacoElement extends ElsaMonaco, HTMLStencilElement {
}

export interface ElsaMonaco {
  "addJavaScriptLib": (libSource: string, libUri: string) => Promise<void>;
  "editorHeight": string;
  "language": string;
  "monacoLibPath": string;
  "padding": string;
  "setValue": (value: string) => Promise<void>;
  "singleLineMode": boolean;
  "value": string;
}

export interface HTMLElsaExpressionEditorElement extends ElsaExpressionEditor, HTMLStencilElement {
}

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
