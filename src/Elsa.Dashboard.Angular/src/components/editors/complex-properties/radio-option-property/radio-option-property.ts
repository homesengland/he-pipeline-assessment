import { Component, EventEmitter, Output, model, ViewChild, Input, output, input, SimpleChanges, OnInit } from '@angular/core';

import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement, IntellisenseContext } from '../../../../models/elsa-interfaces';
import { ActivityModel } from '../../../../models/view';
import { NestedActivityDefinitionProperty } from '../../../../models/custom-component-models';
import { SyntaxNames } from '../../../../constants/constants';
import { ISortableSharedComponent, SortableComponent } from 'src/components/sortable-component';
import { DisplayToggle, IDisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson } from '../../../../utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from '../../../../models/constants';
// import { MultiExpressionEditor } from '../../../editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../../../editors/expression-editor/expression-editor';
import { ISharedComponent } from '../../../base-component';

@Component({
  selector: 'radio-option-property',
  templateUrl: './radio-option-property.html',
  standalone: false,
})
export class RadioOptionProperty implements ISortableSharedComponent, IDisplayToggle, OnInit {

  // TECHNIQUE 1 - Appears our initialisation of values isn't correctly happening here (NOT GETTING ANY ERRORS IN THE CONSOLE)
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  modelSyntax: string = SyntaxNames.Json;
  properties: NestedActivityDefinitionProperty[] = [];
  expressionChanged: ReturnType<typeof output<EventEmitter<string>>>;
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  keyId: string;
  container: HTMLElement;

  //// Declaring component to be an input signal so that its the same as in the SortableComponent which we're extending
  // component = input<ISortableSharedComponent>();

  //component = input<ISortableSharedComponent>({
  //  activityModel: this.activityModel,
  //  propertyDescriptor: this.propertyDescriptor,
  //  propertyModel: this.propertyModel,
  //  modelSyntax: this.modelSyntax,
  //  properties: this.properties,
  //  expressionChanged: this.expressionChanged,
  //  multiExpressionEditor: this.multiExpressionEditor,
  //  keyId: this.keyId,
  //  container: this.container
  //});


  //// TECHNIQUE 2 - Here we're initialising values for each TYPE but it's not getting associated with component (BUT WE'RE NOT GETTING ANY ERRORS IN THE CONSOLE either)
  //activityModel = model<ActivityModel>({
  //    activityId: '',
  //    type: 'RadioOption',
  //    name: 'TestRadioOption',
  //    displayName: 'Test HeRadioOption',
  //    description: 'A Stub activity to display a HeRadioOption property',
  //    outcomes: ['Done'],
  //    properties: [],
  //    persistWorkflow: true,
  //    loadWorkflowContext: undefined,
  //    saveWorkflowContext: undefined,
  //    propertyStorageProviders: undefined,
  //});

  //propertyDescriptor = model<ActivityPropertyDescriptor>({
  //    // conditionalActivityTypes: ['RadioQuestion'],
  //    // expectedOutputType: 'radio',
  //    // hasNestedProperties: true,
  //    // hasColletedProperties: false,
  //    name: 'TestRadioOption',
  //    // type: 'System.String',
  //    uiHint: 'radio-options',
  //    label: 'Test Label for Radio Options',
  //    hint: 'Test Hint for Radio Options',
  //    options: {
  //      items: [
  //        { text: 'Option 1', value: '1' },
  //        { text: 'Option 2', value: '2' },
  //        { text: 'Option 3', value: '3' },
  //      ],
  //      isFlagsEnum: false,
  //    },
  //    // order: 0,
  //    defaultValue: null,
  //    supportedSyntaxes: [],
  //    isReadOnly: false,
  //    //isBrowsable: true,
  //    //isDesignerCritical: false,
  //    disableWorkflowProviderSelection: false,
  //    considerValuesAsOutcomes: false,
  //    defaultSyntax: null,
  //});

  //propertyModel = model<ActivityDefinitionProperty>({
  //  syntax: undefined,
  //  value: 'string',
  //  name: 'TestRadioOption',
  //  expressions: {
  //    Json: '[{"name":"A","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"},{"name":"B","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"}]'
  //  },
  //});

  //modelSyntax: string = SyntaxNames.Json;
  //properties: NestedActivityDefinitionProperty[] = [];
  //// expressionChanged = output<string>();
  //expressionChanged: ReturnType<typeof output<EventEmitter<string>>>;
  //multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  //keyId: string;
  //container: HTMLElement;


  //// TECHNIQUE 3 - Here we're creating the component signal and trying to initialise properties in the interface which are signals also
  //component = input<ISortableSharedComponent>({

  //  activityModel: model<ActivityModel>({
  //    activityId: '',
  //    type: 'RadioOption',
  //    name: 'TestRadioOption',
  //    displayName: 'Test HeRadioOption',
  //    description: 'A Stub activity to display a HeRadioOption property',
  //    outcomes: ['Done'],
  //    properties: [],
  //    persistWorkflow: true,
  //    loadWorkflowContext: undefined,
  //    saveWorkflowContext: undefined,
  //    propertyStorageProviders: undefined,
  //  }),
  //  propertyDescriptor: model<ActivityPropertyDescriptor>({
  //    // conditionalActivityTypes: ['RadioQuestion'],
  //    // expectedOutputType: 'radio',
  //    // hasNestedProperties: true,
  //    // hasColletedProperties: false,
  //    name: 'TestRadioOption',
  //    // type: 'System.String',
  //    uiHint: 'radio-options',
  //    label: 'Test Label for Radio Options',
  //    hint: 'Test Hint for Radio Options',
  //    options: {
  //      items: [
  //        { text: 'Option 1', value: '1' },
  //        { text: 'Option 2', value: '2' },
  //        { text: 'Option 3', value: '3' },
  //      ],
  //      isFlagsEnum: false,
  //    },
  //    // order: 0,
  //    defaultValue: null,
  //    supportedSyntaxes: [],
  //    isReadOnly: false,
  //    //isBrowsable: true,
  //    //isDesignerCritical: false,
  //    disableWorkflowProviderSelection: false,
  //    considerValuesAsOutcomes: false,
  //    defaultSyntax: null,
  //  }),
  //  propertyModel: model<ActivityDefinitionProperty>({
  //    syntax: undefined,
  //    value: 'string',
  //    name: 'TestRadioOption',
  //    expressions: {
  //      Json: '[{"name":"A","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"},{"name":"B","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"}]'
  //    },
  //  }),
  //  modelSyntax: '',

  //  // properties: [],
  //  properties: [
  //    {
  //      name: 'A',
  //      syntax: SyntaxNames.Literal,
  //      expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' },
  //      type: PropertyOutputTypes.Radio,
  //    },
  //    {
  //      name: 'B',
  //      syntax: SyntaxNames.Literal,
  //      expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' },
  //      type: PropertyOutputTypes.Radio,
  //    }
  //  ],

  //  expressionChanged: output<EventEmitter<string>>(),

  //  // multiExpressionEditor: HTMLElsaMultiExpressionEditorElement,
  //  // @ViewChild('multiExpressionEditor') multiExpressionEditor: HTMLElsaMultiExpressionEditorElement,

  //  keyId: '',

  //  // container: HTMLElement,

  //});

    
  json: string = '';

  _base: SortableComponent;
  _toggle: DisplayToggle;

  activityIconProvider: ActivityIconProvider;
  
  dictionary: { [key: string]: any } = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';
  
  defaultSyntax = SyntaxNames.Json; // This is used on the multi expression editor to set the default syntax

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];

  supportedSyntaxForMultiExpressionEditor = [SyntaxNames.Json];

  stringifiedHardCodedJsonDataForExpressionPropertyInMultiExpressionEditor = {
    Json: "[{\"name\":\"A\",\"syntax\":\"JavaScript\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"true\"},\"type\":\"radio\"},{\"name\":\"B\",\"syntax\":\"Literal\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"false\"},\"type\":\"radio\"}]"
  };

  syntaxSwitchCount: number = 0;

  context: IntellisenseContext;

  getExpressions() {
    return { Json: JSON.stringify(this.properties ?? [], null, 2) };
    // return { Json: JSON.stringify(this.component().properties ?? [], null, 2) };
  }

  //@ViewChild('multiExpressionEditor') multiExpressionEditor: MultiExpressionEditor;
  @ViewChild('expressionEditor') expressionEditor: ExpressionEditor;
  @ViewChild('prePopulatedExpressionEditor') prePopulatedExpressionEditor: ExpressionEditor;

  radioOptionsSyntaxPrePopulated = RadioOptionsSyntax.PrePopulated;
  onlyJavaScriptSyntaxes: string[] = this.supportedSyntaxes.filter(x => x === SyntaxNames.JavaScript);

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    this._base = new SortableComponent();
    this._toggle = new DisplayToggle();
  }

  ngOnInit() {

    //this.component.set({
    //  activityModel: this.activityModel,
    //  propertyDescriptor: this.propertyDescriptor,
    //  propertyModel: this.propertyModel,
    //  modelSyntax: this.modelSyntax,
    //  properties: this.properties,
    //  expressionChanged: this.expressionChanged,
    //  multiExpressionEditor: this.multiExpressionEditor,
    //  keyId: this.keyId,
    //  container: this.container
    //});

    this._base.onComponentInitialised();

    // Safely initialize context after models are available
    this.context = {
      activityTypeName: this.activityModel()?.type ?? '',
      propertyName: this.propertyDescriptor()?.name ?? '',
      //activityTypeName: this.component().activityModel()?.type ?? '',
      //propertyName: this.component().propertyDescriptor()?.name ?? '',
    };
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['properties']) {
      this.updateJsonExpressionsVariable();
    }
  }

  onAddOptionClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newOption: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' },
      type: PropertyOutputTypes.Radio,
    };
    this.properties = [...this.properties, newOption];
    // this.component().properties = [...this.component().properties, newOption];
    this._base.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
    //this.component().properties = this.component().properties.filter(x => x != switchCase);
    this._base.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: any) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel().expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
    //this.component().propertyModel().expressions[SyntaxNames.Json] = json;
    //this.component().properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index);
  }

  updateJsonExpressionsVariable() {
    this.json = JSON.stringify(this.properties, null, 2);
    //this.json = JSON.stringify(this.component().properties, null, 2);
  }

  getRenderCaseEditor(): any {
    const cases = this.properties;
    //const cases = this.component().properties;

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
      const optionsDisplay = this._toggle.component.dictionary[index] ?? 'none';

      return {
        radioOption: radioOption,
        syntaxSwitchCount: this.syntaxSwitchCount,
        keyId: this.keyId,
        //keyId: this.component().keyId,
        index: index,
        key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.keyId}`,
        //key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.component().keyId}`,
        monacoLanguage: monacoLanguage,
        prePopulatedExpression: prePopulatedExpression,
        prePopulatedLanguage: prePopulatedLanguage,
        optionsDisplay: optionsDisplay,
        expression: expression,
      };
    });
  }


}
