import { Component, Input, OnInit, signal, SimpleChanges, TemplateRef, ViewChild } from '@angular/core';
import { EventTypes, WorkflowContextFidelity, WorkflowContextOptions, WorkflowDefinition } from 'src/models';
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
  @Input() workflowDefinition: WorkflowDefinition;

  serverUrl: string;
  workflowDefinitionInternal: WorkflowDefinition;
  newVariable: VariableDefinition = {};
  formContext: FormContext;
  workflowChannels: Array<string>;
  tabs = ['Settings', 'Variables', 'Workflow Context', 'Advanced'];
  selectedTab: string = 'Settings';
  inactiveClass = 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
  selectedClass = 'elsa-border-blue-500 elsa-text-blue-600';
  isModalVisible = false;
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


  constructor(private elsaClientService: ElsaClientService) {}

  initContextOptions() {
    console.log("Initializing context options for workflow settings modal");
    if (this.workflowDefinitionInternal.contextOptions == null || this.workflowDefinitionInternal.contextOptions == undefined) {
      this.workflowDefinitionInternal.contextOptions = {
        contextType: '',
        contextFidelity: WorkflowContextFidelity.Burst
      };
    }
    else {
      this.workflowDefinitionInternal.contextOptions.contextFidelity = this.workflowDefinitionInternal.contextOptions.contextFidelity || WorkflowContextFidelity.Burst;
      this.workflowDefinitionInternal.contextOptions.contextType = this.workflowDefinitionInternal.contextOptions.contextType || '';
    }
  }


  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  get variablesValue(): string {
    return this.workflowDefinitionInternal.variables || '{}';
  }

  get variablesLanguage(): string {
    return 'json';
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
    this.workflowChannelOptions = [
      {
        text: '',
        value: null,
      },
      ...this.workflowChannels.map(x => ({ text: x, value: x })),
    ];
  }

  async componentWillLoad() {
    this.initContextOptions();
    this.handleWorkflowDefinitionChanged(this.workflowDefinition);

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.workflowChannels = await client.workflowChannelsApi.list();
    
    console.log("Workflow settings loading");
    console.log("settingsContextOptions", this.workflowDefinitionInternal.contextOptions);
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

  async onSaveClick(e: Event) {
    e.preventDefault();
    await this.dialog.hide();
    setTimeout(() => eventBus.emit(EventTypes.UpdateWorkflowSettings, this, this.workflowDefinitionInternal), 250);
  }

  onMonacoValueChanged(e: MonacoValueChangedArgs) {
    // Don't try and parse JSON if it contains errors.
    const errorCount = e.markers?.filter(x => x.severity == MarkerSeverity.Error).length;

    if (errorCount > 0) return;

    this.workflowDefinitionInternal.variables = e.value;
  }

  async onMonacoInit(e: MonacoValueChangedArgs) {
    console.log('Monaco editor initialized', e);
    await this.monacoVariablesEditor.addJsonLib();
    this.setLanguage(this.variablesLanguage);
    this.setExpression(this.variablesValue);
  }

  async setExpression(value: string) {
    console.log('Setting expression in Monaco editor:', value);
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
