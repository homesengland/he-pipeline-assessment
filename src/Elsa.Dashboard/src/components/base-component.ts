import { EventEmitter } from "@stencil/core";
import Sortable from "sortablejs";
import { SyntaxNames } from "../constants/constants";
import { NestedActivityDefinitionProperty } from "../models/custom-component-models";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, HTMLElsaExpressionEditorElement, HTMLElsaMultiExpressionEditorElement } from "../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson } from "../utils/utils";



export interface ISharedComponent {
activityModel: ActivityModel;
propertyDescriptor: ActivityPropertyDescriptor;
propertyModel: ActivityDefinitionProperty;
modelSyntax: string;
properties: Array<NestedActivityDefinitionProperty>;
expressionChanged: EventEmitter<string>;
multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
}

export interface ISortableSharedComponent extends ISharedComponent {
  container: HTMLElement;
}


export class BaseComponent {

  constructor(public component: ISharedComponent) { }

  componentWillLoad() {
    const propertyModel = this.component.propertyModel;
    const modelJson = propertyModel.expressions[this.component.modelSyntax]
    this.component.properties = parseJson(modelJson) || [];
    }

  componentDidLoad() { }

  componentWillRender() { }

  render() { }

  updatePropertyModel() {
    this.component.propertyModel.expressions[this.component.modelSyntax] = JSON.stringify(this.component.properties);
    this.component.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.component.properties, null, 2);
    this.component.expressionChanged.emit(JSON.stringify(this.component.propertyModel))
  }

  CustomUpdateExpression(e: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string) {
    property.expressions[syntax] = e.detail;
    this.updatePropertyModel();
  }

  StandardUpdateExpression(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    property.expressions[syntax] = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  UpdateExpressionFromInput(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    let elementToUpdate = e.currentTarget as HTMLInputElement;
    let valueToUpdate = elementToUpdate.value.trim();
    property.expressions[syntax] = valueToUpdate;
    this.updatePropertyModel();
  }

  UpdateCheckbox(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    const checkboxElement = (e.currentTarget as HTMLInputElement);
    property.expressions[syntax] = checkboxElement.checked.toString();
    this.updatePropertyModel();
    console.table(property)
  }

  UpdateName(e: Event, property: NestedActivityDefinitionProperty) {
    property.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  UpdateDropdown(e: Event, property: NestedActivityDefinitionProperty, syntax: string) {
    const select = e.currentTarget as HTMLSelectElement;
    property.expressions[syntax] = select.value;
    this.updatePropertyModel();
  }

  UpdateSyntax(e: Event, property: NestedActivityDefinitionProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.updatePropertyModel();
  }

  UpdateProperties(parsed: Array<NestedActivityDefinitionProperty>) {
    this.component.properties = parsed
  }

  OnMultiExpressionEditorValueChanged(e: CustomEvent<string>, syntax: string = SyntaxNames.Json): any {
  const json = e.detail;
  const parsed = parseJson(json);

  if (!parsed)
    return;

  if (!Array.isArray(parsed))
    return;

    this.component.propertyModel.expressions[syntax] = json;
    this.UpdateProperties(parsed);
  }


}

export class SortableComponent extends BaseComponent {

  constructor(public component: ISortableSharedComponent) {
      super(component);
  }

  componentDidLoad() {
    super.componentDidLoad();

    const dragEventHandler = this.onDragActivity.bind(this);
    //creates draggable area
    Sortable.create(this.component.container, {
      animation: 150,
      handle: ".sortablejs-custom-handle",
      ghostClass: 'dragTarget',

      onEnd(evt) {
        console.log("Dragging event", evt);
        console.log("Item", evt.item);
        dragEventHandler(evt.oldIndex, evt.newIndex);
      }
    });
  }

  onDragActivity(oldIndex: number, newIndex: number) {
    console.log("Event handler triggered");
    console.log("Old Index:", oldIndex);
    console.log("new Index", newIndex);
    const activity = this.component.properties.splice(oldIndex, 1)[0];
    this.component.properties.splice(newIndex, 0, activity);
    this.updatePropertyModel();
  }
}
