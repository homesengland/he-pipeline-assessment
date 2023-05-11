import { NestedActivityDefinitionProperty } from "../models/custom-component-models";
import { HTMLElsaExpressionEditorElement } from "../models/elsa-interfaces";
import { mapSyntaxToLanguage } from "../utils/utils";

//Update Model method referenced in the below functions MUST be included on the component in which they are consumed.

export function CustomUpdateExpression(e: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string) {
  property.expressions[syntax] = e.detail;
  this.updatePropertyModel();
}

export function StandardUpdateExpression(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  property.expressions[syntax] = (e.currentTarget as HTMLInputElement).value.trim();
  this.updatePropertyModel();
}

export function UpdateCheckbox(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  const checkboxElement = (e.currentTarget as HTMLInputElement);
  property.expressions[syntax] = checkboxElement.checked.toString();
  this.updatePropertyModel();
  console.table(property)
}

export function UpdateName(e: Event, property: NestedActivityDefinitionProperty) {
  property.name = (e.currentTarget as HTMLInputElement).value.trim();
  this.updatePropertyModel();
}

export function UpdateSyntax(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
  const select = e.currentTarget as HTMLSelectElement;
  property.syntax = select.value;
  expressionEditor.language = mapSyntaxToLanguage(property.syntax);
  this.updatePropertyModel();
}
