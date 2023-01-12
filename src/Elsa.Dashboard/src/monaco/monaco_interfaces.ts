import { HTMLStencilElement } from "@stencil/core/internal";
import { IntellisenseContext } from "../models/elsa-interfaces";

//declare global {
  interface HTMLElsaMonacoElement extends ElsaMonaco, HTMLStencilElement {
  }
  export var HTMLElsaMonacoElement: {
    prototype: HTMLElsaMonacoElement;
    new(): HTMLElsaMonacoElement;
  };

  interface HTMLElsaExpressionEditorElement extends ElsaExpressionEditor, HTMLStencilElement {
  }
  var HTMLElsaExpressionEditorElement: {
    prototype: HTMLElsaExpressionEditorElement;
    new(): HTMLElsaExpressionEditorElement;
  };
//}

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

  interface ElsaExpressionEditor {
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


