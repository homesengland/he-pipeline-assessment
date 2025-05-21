import { IntellisenseService } from '../../../../services/intellisense-service';
import { Component, OnInit, signal, input } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor, ActivityDefinitionProperty } from '../../../../models';
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../../../../store/actions/app.state.actions';
import { SingleLineProperty } from 'src/components/editors/properties/single-line-property/single-line-property';
import { EditorModel } from 'src/components/monaco/types';
import { SingleLineDriver } from 'src/drivers/single-line-driver';
import { PropertyDisplayDriver } from 'src/services/property-display-driver';

@Component({
  selector: 'workflow-placeholder',
  templateUrl: './workflow-placeholder.html',
  styleUrls: ['./workflow-placeholder.css'],
  standalone: false,
})
export class WorkflowPlaceholder implements OnInit {
  private store: Store;
  id = input<string>('5e4506339c934e199a17ca7a2e44f874');
  activityModel = signal<ActivityModel | null>(null);
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
  activityProperties: ActivityPropertyDescriptor[] = [];
  singleLineDriver: SingleLineDriver;

  constructor(store: Store, singleLineDriver: SingleLineDriver) {
    this.store = store;
    this.singleLineDriver = singleLineDriver;
    this.intellisenseGatherer = new IntellisenseService(this.store);
    this.activityModel.set(this.getSingleLineModel());
    this.propertyModel.set(this.getSingleLineDefinition());
    this.propertyDescriptor.set(this.getSingleLineDescriptor());
    this.activityProperties = [this.propertyDescriptor()];
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
      properties: undefined,
      persistWorkflow: true,
      loadWorkflowContext: undefined,
      saveWorkflowContext: undefined,
      propertyStorageProviders: undefined,
    };
    return model;
  }

  getSingleLineDefinition(): ActivityDefinitionProperty {
    const model: ActivityDefinitionProperty = {
      syntax: undefined,
      value: 'string',
      name: 'Test',
      expressions: {
        Literal: '1',
        Javascript: '',
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
      name: 'Id',
      type: 'System.String',
      uiHint: 'single-line',
      label: 'Text',
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
}
