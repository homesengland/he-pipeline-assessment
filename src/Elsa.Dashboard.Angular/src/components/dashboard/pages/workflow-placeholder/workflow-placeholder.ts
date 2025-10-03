import { IntellisenseService } from '../../../../services/intellisense-service';
import { Component, OnInit, signal, input, Signal } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor, ActivityDefinitionProperty } from '../../../../models';
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../../../../store/actions/app.state.actions';
import { EditorModel } from 'src/components/monaco/types';

@Component({
  selector: 'workflow-placeholder',
  templateUrl: './workflow-placeholder.html',
  styleUrls: ['./workflow-placeholder.css'],
  standalone: false,
})
export class WorkflowPlaceholder implements OnInit {
  private store: Store;
  id = input<string>('5e4506339c934e199a17ca7a2e44f874');
  singleLineActivityModel = signal<ActivityModel | null>(null);
  multiLineActivityModel = signal<ActivityModel | null>(null);
  checkboxActivityModel = signal<ActivityModel | null>(null);
  jsonActivityModel = signal<ActivityModel | null>(null);
  dropDownActivityModel = signal<ActivityModel | null>(null);
  checkListActivityModel = signal<ActivityModel | null>(null);
  radioListActivityModel = signal<ActivityModel | null>(null);
  switchCaseActivityModel = signal<ActivityModel | null>(null);
  multiTextActivityModel = signal<ActivityModel | null>(null);
  dictionaryActivityModel = signal<ActivityModel | null>(null);
  radioOptionActivityModel = signal<ActivityModel | null>(null);
  propertyDescriptor = signal<ActivityPropertyDescriptor | null>(null);
  propertyModel = signal<ActivityDefinitionProperty | null>(null);
  intellisenseGatherer: IntellisenseService;
  intellisenseLoaded = signal<boolean>(false);
  options = {
    theme: 'vs-dark',
  };
  toggleLanguage = true;
  code: string;
  cssCode = `.my-class {
    color: red;
  }`;
  jsCode = `function hello() {
    alert('Hello world!');
 }`;
  jsonCode = ['{', '    "p1": "v3",', '    "p2": false', '}'].join('\n');
  model: EditorModel = {
    value: this.jsonCode,
    language: 'typescript',
  };
  activityProperties: Signal<ActivityPropertyDescriptor>[] = [];

  constructor(store: Store) {
    this.store = store;
    this.intellisenseGatherer = new IntellisenseService(this.store);
    this.singleLineActivityModel.set(this.getSingleLineModel());
    this.multiLineActivityModel.set(this.getMultiLineModel());
    this.checkboxActivityModel.set(this.getCheckboxModel());
    this.jsonActivityModel.set(this.getJsonModel());
    this.dropDownActivityModel.set(this.getDropDownModel());
    this.checkListActivityModel.set(this.getCheckListModel());
    this.radioListActivityModel.set(this.getRadioListModel());
    this.checkListActivityModel.set(this.getDropDownModel());
    this.switchCaseActivityModel.set(this.getSwitchCaseModel());
    this.multiTextActivityModel.set(this.getMultiTextModel());
    this.dictionaryActivityModel.set(this.getDictionaryModel());
    this.radioOptionActivityModel.set(this.getRadioOptionModel());

    
    const singleLineDescriptor = signal<ActivityPropertyDescriptor>(this.getSingleLineDescriptor());
    const multiLineDescriptor = signal<ActivityPropertyDescriptor>(this.getMultiLineDescriptor());
    const checkboxDescriptor = signal<ActivityPropertyDescriptor>(this.getCheckboxDescriptor());
    const jsonDescriptor = signal<ActivityPropertyDescriptor>(this.getJsonDescriptor());
    const dropdownDescriptor = signal<ActivityPropertyDescriptor>(this.getDropDownDescriptor());
    const checkListDescriptor = signal<ActivityPropertyDescriptor>(this.getCheckListDescriptor());
    const radioListDescriptor = signal<ActivityPropertyDescriptor>(this.getRadioListDescriptor());
    const switchCaseDescriptor = signal<ActivityPropertyDescriptor>(this.getSwitchCaseDescriptor());
    const multiTextDescriptor = signal<ActivityPropertyDescriptor>(this.getMultiTextDescriptor());
    const dictionaryDescriptor = signal<ActivityPropertyDescriptor>(this.getDictionaryDescriptor());
    const heRadioOptionDescriptor = signal<ActivityPropertyDescriptor>(this.getRadioOptionDescriptor());
    this.activityProperties.push(singleLineDescriptor);
    this.activityProperties.push(multiLineDescriptor);
    this.activityProperties.push(checkboxDescriptor);
    this.activityProperties.push(jsonDescriptor);
    this.activityProperties.push(dropdownDescriptor);
    this.activityProperties.push(checkListDescriptor);
    this.activityProperties.push(radioListDescriptor);
    this.activityProperties.push(switchCaseDescriptor);
    this.activityProperties.push(multiTextDescriptor);
    this.activityProperties.push(dictionaryDescriptor);
    this.activityProperties.push(heRadioOptionDescriptor);
  }

  async ngOnInit() {
    console.log('Id on Init:', this.id());

    //some stuff for monaco

    // back to usual stuff
    await this.store.dispatch(
      AppStateActionGroup.setWorkflowDefinitionId({
        //must pick a valid workflow definition id from the environment database.
        workflowDefinitionId: this.id(),
      }),
    );
    await this.store.dispatch(
      AppStateActionGroup.setWorkflowDefinitionId({
        workflowDefinitionId: this.id(),
      }),
    );
    await this.store.dispatch(AppStateActionGroup.fetchJavaScriptTypeDefinitions());
    await this.intellisenseLoaded.set(true);
    console.log('JavaScript Type Definitions Fetched');
  }

  updateOptions() {
    this.toggleLanguage = !this.toggleLanguage;
    if (this.toggleLanguage) {
      this.code = this.cssCode;
      this.options = Object.assign({}, this.options, { language: 'java' });
    } else {
      this.code = this.jsCode;
      this.options = Object.assign({}, this.options, { language: 'typescript' });
    }
  }

  getSingleLineModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '123',
      type: 'SingleLine',
      name: 'TestSingleLine',
      displayName: 'Test Single Line',
      description: 'A Stub activity to display a single line property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getSingleLineDefinition());
    return model;
  }

  getSingleLineDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestSingleLine',
      expressions: {
        Literal: '123',
        JavaScript: 'console.log("Hello World")',
      },
      type: '',
    };
    return model;
  }

  getSingleLineDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestSingleLine',
      type: 'System.String',
      uiHint: 'single-line',
      label: 'Test Label',
      hint: 'Test Hint',
      options: null,
      order: 0,
      defaultValue: null,
      supportedSyntaxes: ['Literal', 'JavaScript'],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
    };
    return model;
  }

  getMultiLineModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '123',
      type: 'MultiLine',
      name: 'TestMultiLine',
      displayName: 'Test Multi Line',
      description: 'A Stub activity to display a multi line property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getMultiLineDefinition());
    return model;
  }

  getMultiLineDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestMultiLine',
      expressions: {
        Literal: '123',
        JavaScript: 'console.log("Hello MultiLine")',
      },
      type: '',
    };
    return model;
  }

  getMultiLineDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestMultiLine',
      type: 'System.String',
      uiHint: 'multi-line',
      label: 'Test Label',
      hint: 'Test Hint',
      options: null,
      order: 0,
      defaultValue: null,
      supportedSyntaxes: ['JavaScript'],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
    };
    return model;
  }

  getCheckboxModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '123',
      type: 'Checkbox',
      name: 'TestCheckbox',
      displayName: 'Test Checkbox',
      description: 'A Stub activity to display a checkbox property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getCheckboxDefinition());
    return model;
  }

  getCheckboxDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestCheckbox',
      expressions: {},
      type: '',
    };
    return model;
  }

  getCheckboxDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestCheckbox',
      type: 'System.String',
      uiHint: 'checkbox',
      label: 'Test Label',
      hint: 'Test Hint',
      options: null,
      order: 0,
      defaultValue: true,
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
    };
    return model;
  }

  getJsonModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '{"p1": "v3", p2: false}',
      type: 'Json',
      name: 'TestJson',
      displayName: 'Test Json',
      description: 'A Stub activity to display a json property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getJsonDefinition());
    return model;
  }

  getJsonDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestJson',
      expressions: {
        Json: '{"p1": "v3", "p2": false}',
      },
      type: '',
    };
    return model;
  }

  getJsonDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestJson',
      type: 'System.String',
      uiHint: 'json',
      label: 'Test Label',
      hint: 'Test Hint',
      options: null,
      order: 0,
      defaultValue: null,
      supportedSyntaxes: ['Json'],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getDropDownModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'DropDown',
      name: 'TestDropDown',
      displayName: 'Test Drop Down',
      description: 'A Stub activity to display a dropdown property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getDropDownDefinition());
    return model;
  }

  getDropDownDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestDropDown',
      expressions: {
        Literal: 'Option2',
      },
      type: '',
    };
    return model;
  }

  getDropDownDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestDropDown',
      type: 'System.String',
      uiHint: 'dropdown',
      label: 'Test Label',
      hint: 'Test Hint',
      options: {
        items: [
          { text: 'Option 1', value: 'Option1' },
          { text: 'Option 2', value: 'Option2' },
          { text: 'Option 3', value: 'Option3' },
        ],
        isFlagsEnum: false,
      },
      order: 0,
      defaultValue: 'Option3',
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Literal',
    };
    return model;
  }

  getCheckListModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'CheckList',
      name: 'TestCheckList',
      displayName: 'Test Check List',
      description: 'A Stub activity to display a checklist property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getCheckListDefinition());
    return model;
  }

  getCheckListDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestCheckList',
      expressions: {
        Json: '[]',
      },
      type: '',
    };
    return model;
  }

  getCheckListDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestCheckList',
      type: 'System.String',
      uiHint: 'check-list',
      label: 'Test Label',
      hint: 'Test Hint',
      options: {
        items: [
          { text: 'Option 4', value: '4' },
          { text: 'Option 5', value: '5' },
          { text: 'Option 6', value: '6' },
        ],
        isFlagsEnum: false,
      },
      order: 0,
      defaultValue: ['4'],
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getRadioListModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'RadioList',
      name: 'TestRadioList',
      displayName: 'Test Radio List',
      description: 'A Stub activity to display a radio list property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getRadioListDefinition());
    return model;
  }

  getRadioListDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestRadioList',
      expressions: {
        Json: '[]'
      },
      type: '',
    };
    return model;
  }

  getRadioListDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestRadioList',
      type: 'System.String',
      uiHint: 'radio-list',
      label: 'Test Label',
      hint: 'Test Hint',
      options: {
        items: [
          { text: 'Option 1', value: '1' },
          { text: 'Option 2', value: '2' },
          { text: 'Option 3', value: '3' },
        ],
        isFlagsEnum: false,
      },
      order: 0,
      defaultValue: '1',
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getMultiTextModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'MultiText',
      name: 'TestMultiText',
      displayName: 'Test Multi Text',
      description: 'A Stub activity to display a multitext property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getMultiTextDefinition());
    return model;
  }

  getMultiTextDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestMultiText',
      expressions: {
        Json: "[\"123\"]",
        Literal: ""
      },
      type: '',
    };
    return model;
  }

  getMultiTextDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestMultiText',
      type: 'System.String',
      uiHint: 'multi-text',
      label: 'Test Label for Checkbox',
      hint: 'Test Hint for Checkbox',
      options: {
        items: [
          { text: 'Option 4', value: '1234' },
          { text: 'Option 5', value: '56' },
          { text: 'Option 6', value: '7' },
        ],
        isFlagsEnum: false,
      },
      order: 0,
      defaultValue: null,
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getDictionaryModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'Dictionary',
      name: 'TestDictionary',
      displayName: 'Test Dictionary',
      description: 'A Stub activity to display a dictionary property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getDictionaryDefinition());
    return model;
  }

   getSwitchCaseModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'SwitchCase',
      name: 'TestSwitchCase',
      displayName: 'Test Switch Case',
      description: 'A Stub activity to display a switch case property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getSwitchCaseDefinition());
    return model;
  }

  getSwitchCaseDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestSwitchCase',
      expressions: {
        JavaScript: 'console.log("Hello Switch Case")'
      },
      type: '',
    };
    return model;
  }

  getSwitchCaseDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestSwitchCase',
      type: 'System.String',
      uiHint: 'switch-case',
      label: 'Test Label',
      hint: 'Test Hint',
      options: {},
      order: 0,
      defaultValue: '',
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getDictionaryDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestDictionary',
      expressions: {},
      type: '',
    };
    return model;
  }

  getDictionaryDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: [],
      expectedOutputType: 'string',
      hasNestedProperties: false,
      hasColletedProperties: false,
      name: 'TestDictionary',
      type: 'System.String',
      uiHint: 'dictionary',
      label: 'Test Label for Dictionary',
      hint: 'Test Hint for Dictionary',
      options: null,
      order: 0,
      defaultValue: null,
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Json',
    };
    return model;
  }

  getRadioOptionModel(): ActivityModel {
    const model: ActivityModel = {
      activityId: '',
      type: 'RadioOption',
      name: 'TestRadioOptionFromWorkflowPlaceholder',
      displayName: 'Test HeRadioOption',
      description: 'A Stub activity to display a HeRadioOption property',
      outcomes: ['Done'],
      properties: [],
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    model.properties.push(this.getRadioOptionDefinition());
    return model;
  }

  getRadioOptionDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'TestRadioOptionFromWorkflowPlaceholder',
      expressions: {
        Json: '[{"name":"A","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"},{"name":"B","syntax":"Literal","expressions":{"Literal":"","PrePopulated":"false"},"type":"radio"}]'
      },
      type: '',
    };
    return model;
  }

  getRadioOptionDescriptor(): ActivityPropertyDescriptor {
    const model: ActivityPropertyDescriptor = {
      conditionalActivityTypes: ['RadioQuestion'],
      expectedOutputType: 'radio',
      hasNestedProperties: true,
      hasColletedProperties: false,
      name: 'TestRadioOptionFromWorkflowPlaceholder',
      type: 'System.String',
      uiHint: 'radio-options',
      label: 'Test Label for Radio Options',
      hint: 'Test Hint for Radio Options',
      options: {
        items: [
          { text: 'Option 1', value: '1' },
          { text: 'Option 2', value: '2' },
          { text: 'Option 3', value: '3' },
        ],
        isFlagsEnum: false,
      },
      order: 0,
      defaultValue: null,
      supportedSyntaxes: [],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: null,
    };
    return model;
  }

  //// ORIGINAL
  //getRadioOptionDescriptor(): ActivityPropertyDescriptor {
  //  const model: ActivityPropertyDescriptor = {
  //    conditionalActivityTypes: [],
  //    expectedOutputType: 'string',
  //    hasNestedProperties: false,
  //    hasColletedProperties: false,
  //    name: 'TestRadioOption',
  //    type: 'System.String',
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
  //    order: 0,
  //    defaultValue: '1',
  //    supportedSyntaxes: ['Json'],
  //    isReadOnly: false,
  //    isBrowsable: true,
  //    isDesignerCritical: false,
  //    disableWorkflowProviderSelection: false,
  //    considerValuesAsOutcomes: false,
  //    defaultSyntax: 'Json',
  //  };
  //  return model;
  //}

  getActivityModel(activityType: string): Signal<ActivityModel> {
    switch (activityType) {
      case 'single-line':
        return this.singleLineActivityModel;
      case 'multi-line':
        return this.multiLineActivityModel;
      case 'checkbox':
        return this.checkboxActivityModel;
      case 'json':
        return this.jsonActivityModel;
      case 'dropdown':
        return this.dropDownActivityModel;
      case 'check-list':
        return this.checkListActivityModel;
      case 'radio-list':
        return this.radioListActivityModel;
      case 'switch-case':
        return this.switchCaseActivityModel;
      case 'multi-text':
        return this.multiTextActivityModel;
      case 'dictionary':
        return this.dictionaryActivityModel;
      case 'radio-options':
        return this.radioOptionActivityModel;
      default:
        throw new Error(`Unknown activity type: ${activityType}`);
    }
  }
}
