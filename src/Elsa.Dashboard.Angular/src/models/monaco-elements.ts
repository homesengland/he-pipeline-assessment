import { Component } from "@angular/core";

export interface HTMLModalDialogElement extends Component {
  show();
}

export interface HTMLMonacoElement extends Component {
  language: string;
  setValue(value: string);
}

export interface MonacoValueChangedArgs {
  value: string;
  markers: Array<any>;
}
