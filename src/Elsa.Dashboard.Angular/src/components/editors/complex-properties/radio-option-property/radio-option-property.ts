import { Component, EventEmitter, Output, model, ViewChild, Input, output, input, SimpleChanges, OnInit, ModelSignal, AfterViewInit, ElementRef } from '@angular/core';

import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement, IntellisenseContext } from '../../../../models/elsa-interfaces';
import { ActivityModel } from '../../../../models/view';
import { NestedActivityDefinitionProperty } from '../../../../models/custom-component-models';
import { SyntaxNames } from '../../../../constants/constants';
// import { ISortableSharedComponent, SortableComponent } from 'src/components/sortable-component';
// import { DisplayToggle, IDisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson, getUniversalUniqueId, Map } from '../../../../utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from '../../../../models/constants';
// import { MultiExpressionEditor } from '../../../editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../../../editors/expression-editor/expression-editor';
// import { ISharedComponent } from '../../../base-component';
import Sortable from 'sortablejs';

//// From previous Sortable Component
//export interface ISortableSharedComponent extends ISharedComponent {
//  container: HTMLElement;
//}

//// From previous Base Component
//export interface ISharedComponent {
//  activityModel: ReturnType<typeof model<ActivityModel>>;
//  propertyDescriptor: ReturnType<typeof model<ActivityPropertyDescriptor>>;
//  propertyModel: ReturnType<typeof model<ActivityDefinitionProperty>>;
//  modelSyntax: string;
//  properties: NestedActivityDefinitionProperty[];
//  // expressionChanged: ReturnType<typeof output<EventEmitter<string>>>;
//  expressionChanged: ReturnType<typeof output<string>>;
//  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
//  keyId: string;
//}

//// From previous Display Toggle Component
//export interface IDisplayToggle {
//  // dictionary: { [key: string]: string };
//  dictionary: Map<string>;
//  displayValue: string;
//  hiddenValue: string;
//}

@Component({
  selector: 'radio-option-property',
  templateUrl: './radio-option-property.html',
  standalone: false,
})
export class RadioOptionProperty implements OnInit, AfterViewInit {

  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  modelSyntax: string = SyntaxNames.Json;
  keyId: string;
  properties: NestedActivityDefinitionProperty[] = [];
  // expressionChanged: ReturnType<typeof output<EventEmitter<string>>>;
  expressionChanged: ReturnType<typeof output<string>>;
  dictionary: Map<string> = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  // container: HTMLElement; //// From previous Sortable Component
  @ViewChild('containerTable', { static: false }) container: ElementRef<HTMLTableElement>;
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';
  
    
  json: string = '';
  activityIconProvider: ActivityIconProvider;
  defaultSyntax = SyntaxNames.Json; // This is used on the multi expression editor to set the default syntax
  supportedSyntaxForMultiExpressionEditor = [SyntaxNames.Json];
  stringifiedHardCodedJsonDataForExpressionPropertyInMultiExpressionEditor = {
    Json: "[{\"name\":\"A\",\"syntax\":\"JavaScript\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"true\"},\"type\":\"radio\"},{\"name\":\"B\",\"syntax\":\"Literal\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"false\"},\"type\":\"radio\"}]"
  };

  context: IntellisenseContext;

  getExpressions() {
    return { Json: JSON.stringify(this.properties ?? [], null, 2) };
  }

  //@ViewChild('multiExpressionEditor') multiExpressionEditor: MultiExpressionEditor;
  @ViewChild('expressionEditor') expressionEditor: ExpressionEditor;
  @ViewChild('prePopulatedExpressionEditor') prePopulatedExpressionEditor: ExpressionEditor;
  
  radioOptionsSyntaxPrePopulated = RadioOptionsSyntax.PrePopulated;
  onlyJavaScriptSyntaxes: string[] = this.supportedSyntaxes.filter(x => x === SyntaxNames.JavaScript);

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;

    console.log('Constructor executed');
  }

  ngOnInit() {

    // Safely initialize context after models are available
    this.context = {
      activityTypeName: this.activityModel()?.type ?? '',
      propertyName: this.propertyDescriptor()?.name ?? '',
    };

    this.onComponentInitialised();
    this.keyId = getUniversalUniqueId();
  }

  // 1. CODE FROM SORTABLE COMPONENT
  ngAfterViewInit() {
    // Presuming this is the correct lifecycle hook to use instead of Stencil's componentDidLoad, confirm if this is correct
    const dragEventHandler = this.onDragActivity.bind(this);
    Sortable.create(this.container.nativeElement, {
      animation: 150,
      handle: '.sortablejs-custom-handle',
      ghostClass: 'dragTarget',
      onEnd(evt) {
        dragEventHandler(evt.oldIndex, evt.newIndex);
      },
    });
  }

  onDragActivity(oldIndex: number, newIndex: number) {
    const propertiesJson = JSON.stringify(this.properties); // Get component.properties and makes a copy of this converting from JSON object to string
    let propertiesClone: Array<NestedActivityDefinitionProperty> = JSON.parse(propertiesJson); // Parse the string back to an array of NestedActivityDefinitionProperty
    const activity = propertiesClone.splice(oldIndex, 1)[0]; // Removes the activity at oldIndex and stores it in activity variable
    propertiesClone.splice(newIndex, 0, activity); // Inserts the activity at newIndex
    this.properties = propertiesClone; // Assigns the modified array back to component.properties
    this.updatePropertyModel(); // Updates the property model to reflect the changes
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['properties']) {
      this.updateJsonExpressionsVariable();
    }
  }

  onAddOptionClick() {
    const optionName = newOptionLetter(this.IdentifierArray());
    const newOption: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' },
      type: PropertyOutputTypes.Radio,
    };
    this.properties = [...this.properties, newOption];
    this.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: any) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel().expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this.onToggleDisplay(index);
  }

  updateJsonExpressionsVariable() {
    this.json = JSON.stringify(this.properties, null, 2);
  }

  // 2. CODE FROM BASE COMPONENT
  onComponentInitialised() {
    if (this.propertyDescriptor() != null) {
      if (
        this.propertyDescriptor().defaultSyntax != null &&
        this.propertyDescriptor().defaultSyntax != undefined &&
        this.propertyDescriptor().defaultSyntax != ''
      ) {
        this.modelSyntax = this.propertyDescriptor().defaultSyntax;
      }
    }
    const propertyModel = this.propertyModel();
    const modelJson = propertyModel.expressions[this.modelSyntax];
    this.properties = parseJson(modelJson) || [];
  }

  updatePropertyModel() {
    this.propertyModel().expressions[this.modelSyntax] = JSON.stringify(this.properties);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.properties, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel()));

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
    if (this.properties?.length > 0) {
      propertyIdentifiers = this.properties.map(function (v) {
        return v.name;
      });
    }
    console.log('IF was false hence I dropped here');
    return propertyIdentifiers;
  }

  UpdateProperties(parsed: Array<NestedActivityDefinitionProperty>) {
    this.properties = parsed;
  }

  OnMultiExpressionEditorValueChanged(e: CustomEvent<string>, syntax: string = SyntaxNames.Json): any {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel().expressions[syntax] = json;
    this.UpdateProperties(parsed);
  }

  // 3. CODE FROM DISPLAY TOGGLE COMPONENT
  onToggleDisplay(index: any) {
    let tempValue = this.toggleDictionaryDisplay(index, this.dictionary);
    this.dictionary = { ...this.dictionary, tempValue };
  }

  private toggleDictionaryDisplay(index: any, dict: Map<string>): any {
    let tempValue = Object.assign(dict);
    let tableRowClass = dict[index];
    if (tableRowClass == null) {
      tempValue[index] = this.displayValue;
    } else {
      dict[index] == this.hiddenValue ? (tempValue[index] = this.displayValue) : (tempValue[index] = this.hiddenValue);
    }
    return tempValue;
  }

  getRenderCaseEditor(): any {
    const cases = this.properties;
    const supportedSyntaxes = this.supportedSyntaxes;



    return cases.map((radioOption: NestedActivityDefinitionProperty, index: number) => {
      const expression = radioOption.expressions[radioOption.syntax];
      const syntax = radioOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];
           
      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);

      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let colWidth = '100%';
      const optionsDisplay = this.dictionary[index] ?? 'none';

      return {
        keyId: this.keyId,
        radioOption: radioOption,
        syntaxSwitchCount: this.syntaxSwitchCount,
        key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.keyId}`,
        index: index,
        supportedSyntaxes: supportedSyntaxes,
        
        monacoLanguage: monacoLanguage,
        prePopulatedExpression: prePopulatedExpression,
        prePopulatedLanguage: prePopulatedLanguage,
        optionsDisplay: optionsDisplay,
        expression: expression,
        prePopulatedExpressionEditor: prePopulatedExpressionEditor,
      };
    });
  }
}
