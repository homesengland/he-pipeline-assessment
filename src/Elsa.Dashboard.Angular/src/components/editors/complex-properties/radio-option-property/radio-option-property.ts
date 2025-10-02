import { Component, EventEmitter, Output, model, ViewChild, Input, output, input, SimpleChanges, ModelSignal } from '@angular/core';

import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement, IntellisenseContext } from '../../../../models/elsa-interfaces';
import { ActivityModel } from '../../../../models/view';
import { NestedActivityDefinitionProperty } from '../../../../models/custom-component-models';
import { SyntaxNames } from '../../../../constants/constants';
// import { ISortableSharedComponent, SortableComponent } from 'src/components/sortable-component';
// import { DisplayToggle, IDisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson } from '../../../../utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from '../../../../models/constants';
import { MultiExpressionEditor } from '../../../editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../../../editors/expression-editor/expression-editor';
import Sortable from 'sortablejs';

@Component({
  selector: 'radio-option-property',
  templateUrl: './radio-option-property.html',
  standalone: false,
})
export class RadioOptionProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  modelSyntax: string = SyntaxNames.Json;
  keyId: string;
  properties: NestedActivityDefinitionProperty[] = [];
  json: string = '';

  // _base: SortableComponent;
  // _toggle: DisplayToggle;

  activityIconProvider: ActivityIconProvider;
  expressionChanged = output<string>();
  dictionary: { [key: string]: any } = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';
  container: HTMLElement;

  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;

  defaultSyntax: string = SyntaxNames.Json;
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  supportedSyntaxForMultiExpressionEditor = [SyntaxNames.Json];
  syntaxSwitchCount: number = 0;

  context: IntellisenseContext = {
    activityTypeName: this.activityModel().type,
    propertyName: this.propertyDescriptor().name,
  };

  getExpressions() {
    return { Json: JSON.stringify(this.properties ?? [], null, 2) };
  }

  radioOptionsSyntaxPrePopulated = RadioOptionsSyntax.PrePopulated;
  onlyJavaScriptSyntaxes: string[] = this.supportedSyntaxes.filter(x => x === SyntaxNames.JavaScript);

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    // this._base = new SortableComponent();
    // this._toggle = new DisplayToggle();
  }

  ngOnInit() {
    this.onComponentInitialised();
  }

  // need to read up more on simplechanges and what this involves. Do we want this if it means it's firing on every key stroke?
  // if simple changes does fire on every key stroke would this affect performance?
  // if simple changes isn't the right way, then what would be?
  ngOnChanges(changes: SimpleChanges) {
    if (changes['properties']) {
      this.updateJsonExpressionsVariable();
    }
  }

  // Would this be required or would it just be done in the template?
  // thinking it would still stay but if we're using two way binding then the updatePropertyModel either wouldn't be required or would need to be updated.
  // more investigation needed.
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

  // See comments on onAddOptionClick
  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
    this.updatePropertyModel();
  }

  // figure out if e:any is valid for angular here?
  // confirm what this method is doing? Not sure on the !
  onMultiExpressionEditorValueChanged(e: any) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel().expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  // figure out if e:any is valid for angular here?
  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this.onToggleDisplay(index);
  }

  updateJsonExpressionsVariable() {
    this.json = JSON.stringify(this.properties, null, 2);
  }

  // #########################################################
  //        *** COPIED OVER FROM BASE COMPONENT ***
  // #########################################################

  onComponentInitialised() {
    if (this.propertyDescriptor() != null) {
      if (this.propertyDescriptor().defaultSyntax != null && this.propertyDescriptor().defaultSyntax != undefined && this.propertyDescriptor().defaultSyntax != '') {
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
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }

  IdentifierArray(): Array<string> {
    let propertyIdentifiers: Array<string> = [];
    if (this.properties.length > 0) {
      propertyIdentifiers = this.properties.map(function (v) {
        return v.name;
      });
    }
    return propertyIdentifiers;
  }

  CustomUpdateExpression(e: CustomEvent<string>, property: NestedActivityDefinitionProperty, syntax: string) {
    property.expressions[syntax] = e.detail;
    this.updatePropertyModel();
  }

  // #########################################################
  //        *** COPIED OVER FROM SORTABLE COMPONENT ***
  // #########################################################

  ngAfterViewInit() {
    // Presuming this is the correct lifecycle hook to use instead of Stencil's componentDidLoad, confirm if this is correct
    const dragEventHandler = this.onDragActivity.bind(this);
    Sortable.create(this.container, {
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

  // #########################################################
  //        *** COPIED OVER FROM TOGGLE COMPONENT ***
  // #########################################################

  onToggleDisplay(index: any) {
    let tempValue = this.toggleDictionaryDisplay(index, this.dictionary);
    this.dictionary = { ...this.dictionary, tempValue };
  }

  private toggleDictionaryDisplay(index: any, dict: { [key: string]: any }): any {
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
        radioOption: radioOption,
        syntaxSwitchCount: this.syntaxSwitchCount,
        keyId: this.keyId,
        index: index,
        key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.keyId}`,
        monacoLanguage: monacoLanguage,
        prePopulatedExpression: prePopulatedExpression,
        prePopulatedLanguage: prePopulatedLanguage,
        optionsDisplay: optionsDisplay,
        expression: expression,
      };
    });
  }
}
