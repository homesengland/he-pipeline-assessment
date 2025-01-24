import { Component } from "@angular/core";
import { Monaco } from "../components/controls/monaco/monaco-utils";
import { HeMonaco } from "../components/controls/monaco/monaco";

export interface HTMLModalDialogElement extends HeMonaco {
  show();
}

export interface HTMLMonacoElement extends HeMonaco {
  language: string;
  setValue(value: string);
}

export interface MonacoValueChangedArgs {
  value: string;
  markers: Array<any>;
}
