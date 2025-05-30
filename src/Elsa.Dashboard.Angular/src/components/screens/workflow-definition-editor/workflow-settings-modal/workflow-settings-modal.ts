import { Component, Input, OnInit, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { EventTypes, WorkflowContextFidelity, WorkflowContextOptions, WorkflowDefinition } from 'src/models';
import { HTMLElsaMonacoElement, MonacoValueChangedArgs } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';
import { ModalDialog } from 'src/components/shared/modal-dialog/modal-dialog';
import { FormContext, SelectOption } from 'src/Utils/forms';
import { ElsaClientService } from 'src/services/elsa-client';

interface VariableDefinition {
  name?: string;
  value?: string;
}

const persistenceBehaviorOptionsConst: Array<SelectOption> = [{
  text: 'Suspended',
  value: 'Suspended'
}, {
  text: 'Workflow Burst',
  value: 'WorkflowBurst'
}, {
  text: 'Activity Executed',
  value: 'ActivityExecuted'
}];
@Component({
  selector: 'workflow-settings-modal',
  templateUrl: './workflow-settings-modal.html',
  standalone: false,
})
export class WorkflowSettingsModal implements OnInit {
  @ViewChild(ModalDialog) dialog;
  @Input() workflowDefinition: WorkflowDefinition;

  serverUrl: string;
  // workflowDefinition: WorkflowDefinition;
  workflowDefinitionInternal: WorkflowDefinition;
  newVariable: VariableDefinition = {};
  monacoEditor: HTMLElsaMonacoElement; // ?? Not sure if this is correct
  formContext: FormContext;
  workflowChannels: Array<string>;
  tabs = ['Settings', 'Variables', 'Workflow Context', 'Advanced'];
  selectedTab: string = 'Settings';
  inactiveClass = 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
  selectedClass = 'elsa-border-blue-500 elsa-text-blue-600';
  isModalVisible = false;
  persistenceBehaviorOptions: Array<SelectOption> = persistenceBehaviorOptionsConst;
  workflowChannelOptions: Array<SelectOption>;
  fidelityOptions: Array<SelectOption> = [{
      text: 'Burst',
      value: 'Burst'
    }, {
      text: 'Activity',
      value: 'Activity'
    }]
  contextOptions: WorkflowContextOptions;

  constructor(private elsaClientService: ElsaClientService) {}

  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  ngOnInit(): void {
    this.isModalVisible = false;
    this.workflowDefinitionInternal = this.workflowDefinition;
    eventBus.on(EventTypes.ShowWorkflowSettings, this.onShowSettingsModal);
  }

  ngAfterViewChecked(): void {
    this.workflowDefinitionInternal = this.workflowDefinition;
  }

  onShowSettingsModal = async () => {
    this.isModalVisible = true;
    await this.dialog.show();
  };

  closeModal() {
    this.dialog.hide();
    this.isModalVisible = false;
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['WorkflowDefinition'] && changes['WorkflowDefinition'].currentValue) {
      this.handleWorkflowDefinitionChanged(changes['WorkflowDefinition'].currentValue);
    }
  }

  handleWorkflowDefinitionChanged(newValue: WorkflowDefinition) {
    this.workflowDefinitionInternal = { ...newValue };
    this.formContext = new FormContext(this.workflowDefinitionInternal, newValue => (this.workflowDefinitionInternal = newValue));
    this.workflowChannelOptions = [{
      text: '',
      value: null
    }, ...this.workflowChannels.map(x => ({text: x, value: x}))];
  }

  async componentWillLoad() {
    this.handleWorkflowDefinitionChanged(this.workflowDefinition);

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.workflowChannels = await client.workflowChannelsApi.list();
  }

  // @Watch('workflowDefinition')
  // handleWorkflowDefinitionChanged(newValue: WorkflowDefinition) {
  //   this.workflowDefinitionInternal = {...newValue};
  //   this.formContext = new FormContext(this.workflowDefinitionInternal, newValue => this.workflowDefinitionInternal = newValue);
  // }

  // async componentWillLoad() {
  //   this.handleWorkflowDefinitionChanged(this.workflowDefinition);

  //   const client = await createElsaClient(this.serverUrl);
  //   this.workflowChannels = await client.workflowChannelsApi.list();
  // }

  componentDidLoad() {
    eventBus.on(EventTypes.ShowWorkflowSettings, async () => await this.dialog.show());
  }

  onTabClick(e: Event, tab: string) {
    e.preventDefault();
    this.selectedTab = tab;
  }

  async onCancelClick() {
    await this.dialog.hide();
  }

  async onSubmit(e: Event) {
    e.preventDefault();
    await this.dialog.hide();
    setTimeout(() => eventBus.emit(EventTypes.UpdateWorkflowSettings, this, this.workflowDefinitionInternal), 250);
  }

  // onMonacoValueChanged(e: MonacoValueChangedArgs) {
  //   // Don't try and parse JSON if it contains errors.
  //   const errorCount = e.markers.filter(x => x.severity == MarkerSeverity.Error).length;

  //   if (errorCount > 0)
  //     return;

  //   this.workflowDefinitionInternal.variables = e.value;
  // }

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

  // renderSettingsTab() {
  //   const workflowDefinition = this.workflowDefinitionInternal;
  //   const formContext = this.formContext;

  //   console.log('renderSettingsTab');

  //   return textInput(formContext, 'name', 'Name', workflowDefinition.id, 'The technical name of the workflow.', 'workflowName');

  //   // {textInput(formContext, 'name', 'Name', workflowDefinition.name, 'The technical name of the workflow.', 'workflowName')
  //   // {textInput(formContext, 'displayName', 'Display Name', workflowDefinition.displayName, 'A user-friendly display name of the workflow.', 'workflowDisplayName')}
  //   // {textArea(formContext, 'description', 'Description', workflowDefinition.description, null, 'workflowDescription')}
  // }

  renderAdvancedTab() {
    const workflowDefinition = this.workflowDefinitionInternal;
    // const formContext = this.formContext;
    // const workflowChannelOptions: Array<SelectOption> = [{
    //   text: '',
    //   value: null
    // }, ...this.workflowChannels.map(x => ({text: x, value: x}))];

    // const persistenceBehaviorOptions: Array<SelectOption> = [{
    //   text: 'Suspended',
    //   value: 'Suspended'
    // }, {
    //   text: 'Workflow Burst',
    //   value: 'WorkflowBurst'
    // }, {
    //   text: 'Activity Executed',
    //   value: 'ActivityExecuted'
    // }];

    // return (
    //   <div class="elsa-flex elsa-px-8">
    //     <div class="elsa-space-y-8 elsa-w-full">
    //       {textInput(formContext, 'tag', 'Tag', workflowDefinition.tag, 'Tags can be used to query workflow definitions with.', 'tag')}
    //       {selectField(formContext, 'persistenceBehavior', 'Persistence Behavior', workflowDefinition.persistenceBehavior, persistenceBehaviorOptions, 'The persistence behavior controls how often a workflow instance is persisted during workflow execution.', 'workflowContextFidelity')}
    //       {workflowChannelOptions.length > 0 ? selectField(formContext, 'channel', 'Channel', workflowDefinition.channel, workflowChannelOptions, 'Select a channel for this workflow to execute in.', 'channel') : undefined}
    //       {checkBox(formContext, 'isSingleton', 'Singleton', workflowDefinition.isSingleton, 'Singleton workflows will only have one active instance executing at a time.')}
    //     </div>
    //   </div>
    // );
  }

  renderVariablesTab() {
    const workflowDefinition = this.workflowDefinitionInternal;
    const value = workflowDefinition.variables || '{}';
    const language = 'json';

    // return (
    //   <div class="elsa-flex elsa-px-8">
    //     <div class="elsa-space-y-8 elsa-w-full elsa-h-30">
    //       <elsa-monaco value={value} language={language} editor-height="30em"
    //                    onValueChanged={e => this.onMonacoValueChanged(e.detail)} ref={el => this.monacoEditor = el}/>
    //     </div>
    //   </div>
    // );
  }

  renderWorkflowContextTab() {
    const workflowDefinition = this.workflowDefinitionInternal;
    // const formContext = this.formContext;

    const contextOptions: WorkflowContextOptions = workflowDefinition.contextOptions || {
      contextType: undefined,
      contextFidelity: WorkflowContextFidelity.Burst,
    };

    // const fidelityOptions: Array<SelectOption> = [{
    //   text: 'Burst',
    //   value: 'Burst'
    // }, {
    //   text: 'Activity',
    //   value: 'Activity'
    // }]

    // return (
    //   <div class="elsa-flex elsa-px-8">
    //     <div class="elsa-space-y-8 elsa-w-full">
    //       {textInput(formContext, 'contextOptions.contextType', 'Type', contextOptions.contextType, 'The fully qualified workflow context type name.', 'workflowContextType')}
    //       {selectField(formContext, 'contextOptions.contextFidelity', 'Fidelity', contextOptions.contextFidelity, fidelityOptions, 'The workflow context refresh fidelity controls the behavior of when to load and persist the workflow context.', 'workflowContextFidelity')}
    //     </div>
    //   </div>
    // );
    return;
  }
}
