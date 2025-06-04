import { EditorComponent } from './../components/monaco/editor.component';
import { Component } from '@angular/core';
import { ExpressionEditor } from '../components/editors/expression-editor/expression-editor';

export interface HTMLModalDialogElement extends EditorComponent, HTMLElement {
  show();
}

export interface HTMLMonacoElement extends EditorComponent, HTMLElement {}

export interface HTMLExpressionEditorElement extends ExpressionEditor, HTMLElement {}

export interface MonacoValueChangedArgs {
  value: string;
  markers: Array<any>;
}
