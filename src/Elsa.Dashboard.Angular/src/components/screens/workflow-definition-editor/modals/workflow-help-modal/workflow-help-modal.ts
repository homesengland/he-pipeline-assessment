import { Component, effect, input, signal, OnInit, EventEmitter, Output, SimpleChanges, TemplateRef, ViewChild, model } from '@angular/core';
import { EventTypes, WorkflowContextOptions, WorkflowDefinition, WorkflowContextFidelity } from 'src/models';
import { MonacoValueChangedArgs } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';
import { ModalDialog } from 'src/components/shared/modal-dialog/modal-dialog';
import { ElsaClientService } from 'src/services/elsa-client';


@Component({
  selector: 'workflow-help-modal',
  templateUrl: './workflow-help-modal.html',
  standalone: false,
})
export class WorkflowHelpModal implements OnInit {
  @ViewChild(ModalDialog) dialog;
  @Output() onWorkflowDefinitionSettingsChanged: EventEmitter<WorkflowDefinition> = new EventEmitter<WorkflowDefinition>();

  serverUrl: string;
  isModalVisible = false;

  contextOptions: WorkflowContextOptions;


  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  async ngOnInit() {
    this.isModalVisible = false;
  }

  ngAfterViewInit(): void {
    eventBus.on(EventTypes.ShowWorkflowHelp, this.onShowSettingsModal);
  }

  onShowSettingsModal = async () => {
    this.isModalVisible = true;
    await this.dialog.show();
  };

  closeModal() {
    this.dialog.hide();
    this.isModalVisible = false;
  }

  componentDidLoad() {
    console.log('WorkflowSettingsModal componentDidLoad called');
    eventBus.on(EventTypes.ShowWorkflowHelp, async () => await this.dialog.show());
  }



}
