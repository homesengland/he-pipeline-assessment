import { Component, Output, EventEmitter, Input } from '@angular/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  HTMLElsaExpressionEditorElement,
} from '../models/elsa-interfaces';
import { NestedActivityDefinitionProperty } from '../models/custom-component-models';
import { getUniversalUniqueId, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
import { SyntaxNames } from '../constants/constants';

export interface ISharedComponent {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  modelSyntax: string;
  properties: NestedActivityDefinitionProperty[];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  keyId: string;
}

@Component({
  selector: 'base-component',
  template: '',
})
export class BaseComponent {
  @Input() component!: ISharedComponent;
  @Output() expressionChanged = new EventEmitter<string>();

  ngOnInit() {
    this.componentWillLoad();
  }

  componentWillLoad() {
    if (this.component.propertyDescriptor != null) {
      if (
        this.component.propertyDescriptor.defaultSyntax != null &&
        this.component.propertyDescriptor.defaultSyntax != undefined &&
        this.component.propertyDescriptor.defaultSyntax != ''
      ) {
        this.component.modelSyntax = this.component.propertyDescriptor.defaultSyntax;
      }
    }
    const propertyModel = this.component.propertyModel;
    const modelJson = propertyModel.expressions[this.component.modelSyntax];
    this.component.properties = parseJson(modelJson) || [];
  }

  componentWillRender() {
    this.component.keyId = getUniversalUniqueId();
  }

  updatePropertyModel() {
    this.component.propertyModel.expressions[this.component.modelSyntax] = JSON.stringify(this.component.properties);
    this.component.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.component.properties, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.component.propertyModel));
  }

  CustomUpdateExpression(e: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string) {
    property.expressions[syntax] = e.detail;
    this.updatePropertyModel();
  }

  StandardUpdateExpression(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    property.expressions[syntax] = (e.target as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  UpdateExpressionFromInput(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    let elementToUpdate = e.target as HTMLInputElement;
    let valueToUpdate = elementToUpdate.value.trim();
    property.expressions[syntax] = valueToUpdate;
    this.updatePropertyModel();
  }

  UpdateCheckbox(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    const checkboxElement = e.target as HTMLInputElement;
    property.expressions[syntax] = checkboxElement.checked.toString();
    this.updatePropertyModel();
    console.table(property);
  }

  UpdateName(e: Event, property: NestedActivityDefinitionProperty) {
    property.name = (e.target as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  UpdateDropdown(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    const select = e.target as HTMLSelectElement;
    property.expressions[syntax] = select.value;
    this.updatePropertyModel();
  }

  UpdateSyntax(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.target as HTMLSelectElement;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.updatePropertyModel();
  }

  IdentifierArray(): Array<string> {
    let propertyIdentifiers: Array<string> = [];
    if (this.component.properties.length > 0) {
      propertyIdentifiers = this.component.properties.map(function (v) {
        return v.name;
      });
    }
    return propertyIdentifiers;
  }

  UpdateProperties(parsed: Array<NestedActivityDefinitionProperty>) {
    this.component.properties = parsed;
  }

  OnMultiExpressionEditorValueChanged(e: CustomEvent<string>, syntax: string = SyntaxNames.Json): any {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.component.propertyModel.expressions[syntax] = json;
    this.UpdateProperties(parsed);
  }
}
