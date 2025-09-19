import { AfterViewInit, Component, Input, Output, EventEmitter } from '@angular/core';
import Sortable from 'sortablejs';
import { ActivityModel } from '../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../models/domain';
import { MultiExpressionEditor } from '../components/editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../components/editors/expression-editor/expression-editor';


import { NestedActivityDefinitionProperty } from '../models/custom-component-models';
import { getUniversalUniqueId, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
import { SyntaxNames } from '../constants/constants';

export interface ISortableSharedComponent extends ISharedComponent {
  container: HTMLElement;
}

export interface ISharedComponent {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  modelSyntax: string;
  properties: Array<NestedActivityDefinitionProperty>;
  expressionChanged: EventEmitter<string>;
  multiExpressionEditor: MultiExpressionEditor;
  keyId: string;
}

@Component({
  selector: 'sortable-component',
  template: '',
})
export class SortableComponent implements AfterViewInit {
  @Input() component!: ISortableSharedComponent;
  @Output() expressionChanged = new EventEmitter<string>();

  // below is original base component code
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

  UpdateSyntax(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: ExpressionEditor) {
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

  // below is original Sortable component code
  ngAfterViewInit() {
    const dragEventHandler = this.onDragActivity.bind(this);
    Sortable.create(this.component.container, {
      animation: 150,
      handle: '.sortablejs-custom-handle',
      ghostClass: 'dragTarget',
      onEnd(evt) {
        dragEventHandler(evt.oldIndex, evt.newIndex);
      },
    });
  }

  onDragActivity(oldIndex: number, newIndex: number) {
    const propertiesJson = JSON.stringify(this.component.properties);
    let propertiesClone: Array<NestedActivityDefinitionProperty> = JSON.parse(propertiesJson);
    const activity = propertiesClone.splice(oldIndex, 1)[0];
    propertiesClone.splice(newIndex, 0, activity);
    this.component.properties = propertiesClone;
    this.updatePropertyModel();
  }
}
