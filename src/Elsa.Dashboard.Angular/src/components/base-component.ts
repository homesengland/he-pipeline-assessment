//import { Component, Output, EventEmitter, Input, ModelSignal, output, input, model } from '@angular/core';
//import { ActivityDefinitionProperty, HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement } from '../models/elsa-interfaces';
//import { ActivityModel } from '../models/view';
//import { ActivityPropertyDescriptor } from '../models/domain';
//import { NestedActivityDefinitionProperty } from '../models/custom-component-models';
//import { getUniversalUniqueId, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
//import { SyntaxNames } from '../constants/constants';

// THIS DID NOT GENERATE ANY ERRORS IN THE CONSOLE
//export interface ISharedComponent {
//  activityModel: ReturnType<typeof model<ActivityModel>>;
//  propertyDescriptor: ReturnType<typeof model<ActivityPropertyDescriptor>>;
//  propertyModel: ReturnType<typeof model<ActivityDefinitionProperty>>;
//  modelSyntax: string;
//  properties: NestedActivityDefinitionProperty[];
//  expressionChanged: ReturnType<typeof output<EventEmitter<string>>>;
//  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
//  keyId: string;
//}

//export interface ISharedComponent {
//  activityModel: ActivityModel;
//  propertyDescriptor: ActivityPropertyDescriptor;
//  propertyModel: ActivityDefinitionProperty;
//  modelSyntax: string;
//  properties: NestedActivityDefinitionProperty[];
//  expressionChanged: EventEmitter<string>;
//  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
//  keyId: string;
//}

//export class BaseComponent {
  // expressionChanged = output<string>();

  //constructor(public component: ModelSignal<ISharedComponent>) {
  //}

  //OnInit() {
  //  this.onComponentInitialised();
  //  this.component().keyId = getUniversalUniqueId();
  //}

  //onComponentInitialised() {
  //  if (this.component().propertyDescriptor() != null) {
  //    if (
  //      this.component().propertyDescriptor().defaultSyntax != null &&
  //      this.component().propertyDescriptor().defaultSyntax != undefined &&
  //      this.component().propertyDescriptor().defaultSyntax != ''
  //    ) {
  //      this.component().modelSyntax = this.component().propertyDescriptor().defaultSyntax;
  //    }
  //  }
  //  const propertyModel = this.component().propertyModel();
  //  const modelJson = propertyModel.expressions[this.component().modelSyntax];
  //  this.component().properties = parseJson(modelJson) || [];
  //}

  //updatePropertyModel() {
  //  this.component().propertyModel().expressions[this.component().modelSyntax] = JSON.stringify(this.component().properties);
  //  this.component().multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.component().properties, null, 2);
  //  this.expressionChanged.emit(JSON.stringify(this.component().propertyModel));
  //}

  //CustomUpdateExpression(e: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string) {
  //  property.expressions[syntax] = e.detail;
  //  this.updatePropertyModel();
  //}

  //StandardUpdateExpression(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  //  property.expressions[syntax] = (e.target as HTMLInputElement).value.trim();
  //  this.updatePropertyModel();
  //}

  //UpdateExpressionFromInput(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  //  let elementToUpdate = e.target as HTMLInputElement;
  //  let valueToUpdate = elementToUpdate.value.trim();
  //  property.expressions[syntax] = valueToUpdate;
  //  this.updatePropertyModel();
  //}

  //UpdateCheckbox(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  //  const checkboxElement = e.target as HTMLInputElement;
  //  property.expressions[syntax] = checkboxElement.checked.toString();
  //  this.updatePropertyModel();
  //  console.table(property);
  //}

  //UpdateName(e: Event, property: NestedActivityDefinitionProperty) {
  //  property.name = (e.target as HTMLInputElement).value.trim();
  //  this.updatePropertyModel();
  //}

  //UpdateDropdown(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
  //  const select = e.target as HTMLSelectElement;
  //  property.expressions[syntax] = select.value;
  //  this.updatePropertyModel();
  //}

  //UpdateSyntax(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
  //  const select = e.target as HTMLSelectElement;
  //  property.syntax = select.value;
  //  expressionEditor.language = mapSyntaxToLanguage(property.syntax);
  //  this.updatePropertyModel();
  //}

  //IdentifierArray(): Array<string> {
  //  let propertyIdentifiers: Array<string> = [];
  //  if (this.component().properties?.length > 0) {
  //    propertyIdentifiers = this.component().properties.map(function (v) {
  //      return v.name;
  //    });
  //  }
  //  console.log('IF was false hence I dropped here');
  //  return propertyIdentifiers;
  //}

  //UpdateProperties(parsed: Array<NestedActivityDefinitionProperty>) {
  //  this.component().properties = parsed;
  //}

  //OnMultiExpressionEditorValueChanged(e: CustomEvent<string>, syntax: string = SyntaxNames.Json): any {
  //  const json = e.detail;
  //  const parsed = parseJson(json);

  //  if (!parsed) return;

  //  if (!Array.isArray(parsed)) return;

  //  this.component().propertyModel().expressions[syntax] = json;
  //  this.UpdateProperties(parsed);
  //}
//}
