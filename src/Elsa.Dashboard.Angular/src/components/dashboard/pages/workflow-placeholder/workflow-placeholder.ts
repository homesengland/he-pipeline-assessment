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
    const singleLineDescriptor = signal<ActivityPropertyDescriptor>(this.getSingleLineDescriptor());
    const multiLineDescriptor = signal<ActivityPropertyDescriptor>(this.getMultiLineDescriptor());
    const checkboxDescriptor = signal<ActivityPropertyDescriptor>(this.getCheckboxDescriptor());
    const jsonDescriptor = signal<ActivityPropertyDescriptor>(this.getJsonDescriptor());
    const dropdownDescriptor = signal<ActivityPropertyDescriptor>(this.getDropDownDescriptor());
    this.activityProperties.push(singleLineDescriptor);
    this.activityProperties.push(multiLineDescriptor);
    this.activityProperties.push(checkboxDescriptor);
    this.activityProperties.push(jsonDescriptor);
    this.activityProperties.push(dropdownDescriptor);
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
      supportedSyntaxes: ['Literal'],
      isReadOnly: false,
      isBrowsable: true,
      isDesignerCritical: false,
      disableWorkflowProviderSelection: false,
      considerValuesAsOutcomes: false,
      defaultSyntax: 'Literal',
    };
    return model;
  }

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
      default:
        throw new Error(`Unknown activity type: ${activityType}`);
    }
  }
}
