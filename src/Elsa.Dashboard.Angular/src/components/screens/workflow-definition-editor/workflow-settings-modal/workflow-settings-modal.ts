import { Component, effect, input, signal, OnInit, EventEmitter, Output, SimpleChanges, TemplateRef, ViewChild, model } from '@angular/core';
import { EventTypes, WorkflowContextOptions, WorkflowDefinition, WorkflowContextFidelity } from 'src/models';
import { MonacoValueChangedArgs } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';
import { ModalDialog } from 'src/components/shared/modal-dialog/modal-dialog';
import { FormContext, SelectOption } from 'src/Utils/forms';
import { ElsaClientService } from 'src/services/elsa-client';
import { MarkerSeverity } from 'src/components/monaco/types';
import { HTMLMonacoElement } from 'src/models/monaco-elements';
import { Uri } from 'src/constants/constants';


interface VariableDefinition {
  name?: string;
  value?: string;
}

const persistenceBehaviorOptionsConst: Array<SelectOption> = [
  {
    text: 'Suspended',
    value: 'Suspended',
  },
  {
    text: 'Workflow Burst',
    value: 'WorkflowBurst',
  },
  {
    text: 'Activity Executed',
    value: 'ActivityExecuted',
  },
];
@Component({
  selector: 'workflow-settings-modal',
  templateUrl: './workflow-settings-modal.html',
  standalone: false,
})
export class WorkflowSettingsModal implements OnInit {
  @ViewChild(ModalDialog) dialog;
  @ViewChild('monacoVariablesEditor') monacoVariablesEditor: HTMLMonacoElement;
  workflowDefinition = input.required<WorkflowDefinition>();
  workflowDefinitionModel = signal<WorkflowDefinition>(null);
  @Output() onWorkflowDefinitionSettingsChanged: EventEmitter<WorkflowDefinition> = new EventEmitter<WorkflowDefinition>();

  baseWorkflowDefinitionJson: string = '';
  serverUrl: string;
  newVariable: VariableDefinition = {};
  workflowChannels: Array<string>;
  tabs = ['Settings', 'Variables', 'Workflow Context', 'Advanced'];
  selectedTab: string = 'Settings';
  inactiveClass = 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
  selectedClass = 'elsa-border-blue-500 elsa-text-blue-600';
  isModalVisible = false;
  initialized = false;
  persistenceBehaviorOptions: Array<SelectOption> = persistenceBehaviorOptionsConst;
  workflowChannelOptions: Array<SelectOption>;
  fidelityOptions: Array<SelectOption> = [
    {
      text: 'Burst',
      value: 'Burst',
    },
    {
      text: 'Activity',
      value: 'Activity',
    },
  ];
  contextOptions: WorkflowContextOptions;

  constructor(private elsaClientService: ElsaClientService) {
    effect(() => {
      console.log("Effect Occurring");
      var jsonString = JSON.stringify(this.workflowDefinition());
      var tempWorkflow: WorkflowDefinition = JSON.parse(jsonString);
      this.workflowDefinitionModel.set(tempWorkflow);
      });
  }

  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  get variablesValue(): string {
    if (!this.workflowDefinitionModel().variables) {
      return '{}';
    }
    else return this.workflowDefinitionModel().variables;
  }

  get variablesLanguage(): string {
    return 'json';
  }

  async ngOnInit() {
    this.isModalVisible = false;
    //eventBus.on(EventTypes.ShowWorkflowSettings, this.onShowSettingsModal);
    this.initContextOptions();
    console.log("Loading", this.workflowDefinitionModel());
    //this.handleWorkflowDefinitionInit(this.workflowDefinition());

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.workflowChannels = await client.workflowChannelsApi.list();
    this.initialized = true;
  }

  ngAfterViewInit(): void {
    eventBus.on(EventTypes.ShowWorkflowSettings, this.onShowSettingsModal);
  }


  //ngAfterViewChecked(): void {
  //  this.workflowDefinition = this.workflowDefinition;
  //}

  onShowSettingsModal = async () => {
    this.isModalVisible = true;
    await this.dialog.show();
  };

  closeModal() {
    this.dialog.hide();
    this.isModalVisible = false;
  }

  //ngOnChanges(changes: SimpleChanges) {
  //  if (changes['WorkflowDefinition'] && changes['WorkflowDefinition'].currentValue) {
  //    this.handleWorkflowDefinitionChanged(changes['WorkflowDefinition'].currentValue);
  //  }
  //}

  revertChanges() {
    console.log("Reverting Changes", this.workflowDefinition())
    this.workflowDefinitionModel.set(this.workflowDefinition());
  }

  //handleWorkflowDefinitionInit(newValue: WorkflowDefinition) {
  //  this.workflowDefinition() = { ...newValue };
  //  this.formContext = new FormContext(this.workflowDefinition, newValue => (this.workflowDefinition = newValue));
  //  this.workflowChannelOptions = [
  //    {
  //      text: '',
  //      value: null,
  //    },
  //    ...this.workflowChannels.map(x => ({ text: x, value: x })),
  //  ];
  //}

  handleSaveWorkflow() {
    this.onWorkflowDefinitionSettingsChanged.emit(this.workflowDefinitionModel());
  }

  initContextOptions() {
    let defaultContextOptions: WorkflowContextOptions = {
      contextType: '',
      contextFidelity: WorkflowContextFidelity.Burst
    };
    if (!this.workflowDefinitionModel().contextOptions || this.workflowDefinitionModel().contextOptions == undefined) {
      this.workflowDefinitionModel.update(model => ({
        ...model, contextOptions: defaultContextOptions

      }));
    }
    else {
      defaultContextOptions.contextFidelity = this.workflowDefinitionModel().contextOptions.contextFidelity || WorkflowContextFidelity.Burst;
      defaultContextOptions.contextType = this.workflowDefinitionModel().contextOptions.contextType || '';

      this.workflowDefinitionModel.update(model => ({
        ...model, contextOptions: defaultContextOptions
      }));
    }
  }


  componentDidLoad() {
    console.log('WorkflowSettingsModal componentDidLoad called');
    eventBus.on(EventTypes.ShowWorkflowSettings, async () => await this.dialog.show());
  }

  onTabClick(e: Event, tab: string) {
    e.preventDefault();
    this.selectedTab = tab;
  }

  async onCancelClick() {
    this.revertChanges();
    this.dialog.hide();
    this.isModalVisible = false;
  }

  async onSaveClick(e: Event) {
    e.preventDefault();
    this.handleSaveWorkflow();
    await this.dialog.hide();
    setTimeout(() => eventBus.emit(EventTypes.UpdateWorkflowSettings, this, this.workflowDefinition), 250);
  }

  onMonacoValueChanged(e: MonacoValueChangedArgs) {
    // Don't try and parse JSON if it contains errors.
    const errorCount = e.markers?.filter(x => x.severity == MarkerSeverity.Error).length;

    if (errorCount > 0) return;

    this.workflowDefinitionModel.update(model => ({
      ...model, variables: e.value
    }));
  }

  async onMonacoInit(e:MonacoValueChangedArgs){
    console.log('Monaco editor initialized', e);
    await this.monacoVariablesEditor.addJsonLib();
    this.setLanguage(this.variablesLanguage);
    this.setExpression(this.variablesValue);
  }

  async setExpression(value: string) {
    console.log("Setting expression in Monaco editor:", value);
    await this.monacoVariablesEditor.setValue(value);
  }
  async setLanguage(language: string) {
    await this.monacoVariablesEditor.setLanguage(language);
  }

  @ViewChild('settingsTab', { static: true }) settingsTab!: TemplateRef<any>;
  @ViewChild('variablesTab', { static: true }) variablesTab!: TemplateRef<any>;
  @ViewChild('workflowContextTab', { static: true }) workflowContextTab!: TemplateRef<any>;
  @ViewChild('advancedTab', { static: true }) advancedTab!: TemplateRef<any>;

  renderSelectedTab(): TemplateRef<any> {
    const selectedTab = this.selectedTab;

    switch (this.selectedTab) {
      case 'Workflow Context':
        return this.workflowContextTab;
      case 'Variables':
        return this.variablesTab;
      case 'Advanced':
        return this.advancedTab;
      case 'Settings':
      default:
        return this.settingsTab;
    }
  }

}
