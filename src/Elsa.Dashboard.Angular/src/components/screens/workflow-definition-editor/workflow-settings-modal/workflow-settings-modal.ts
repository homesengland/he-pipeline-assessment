import { Component, OnInit, ViewChild } from '@angular/core';
import { EventTypes, WorkflowDefinition } from 'src/models';
import { HTMLElsaMonacoElement, MonacoValueChangedArgs } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';
import { ModalDialog } from 'src/components/shared/modal-dialog/modal-dialog';

interface VariableDefinition {
  name?: string;
  value?: string;
}
@Component({
  selector: 'workflow-settings-modal',
  templateUrl: './workflow-settings-modal.html',
  standalone: false,
})
export class WorkflowSettingsModal implements OnInit {
  @ViewChild(ModalDialog) dialog;
  serverUrl: string;
  workflowDefinition: WorkflowDefinition;
  workflowDefinitionInternal: WorkflowDefinition;
  selectedTab: string = 'Settings';
  newVariable: VariableDefinition = {};
  monacoEditor: HTMLElsaMonacoElement; // ?? Not sure if this is correct
  // formContext: FormContext; // To be converted.
  workflowChannels: Array<string>;

  ngOnInit(): void {}

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
    await this.dialog.close();
  }

  async onSubmit(e: Event) {
    e.preventDefault();
    await this.dialog.close();
    setTimeout(() => eventBus.emit(EventTypes.UpdateWorkflowSettings, this, this.workflowDefinitionInternal), 250);
  }

  // onMonacoValueChanged(e: MonacoValueChangedArgs) {
  //   // Don't try and parse JSON if it contains errors.
  //   const errorCount = e.markers.filter(x => x.severity == MarkerSeverity.Error).length;

  //   if (errorCount > 0)
  //     return;

  //   this.workflowDefinitionInternal.variables = e.value;
  // }
}
