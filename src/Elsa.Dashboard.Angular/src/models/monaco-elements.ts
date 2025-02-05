import { Component } from "@angular/core";
import { Monaco } from "../components/controls/monaco/monaco-utils";
import { MonacoEditor } from "../components/controls/monaco/monaco-editor";
import { ExpressionEditor } from "../components/editors/expression-editor/expression-editor";

export interface HTMLModalDialogElement extends MonacoEditor, HTMLElement {
  show();
}

export interface HTMLMonacoElement extends MonacoEditor, HTMLElement {

}

export interface HTMLExpressionEditorElement extends ExpressionEditor, HTMLElement {

}

export interface MonacoValueChangedArgs {
  value: string;
  markers: Array<any>;
}
