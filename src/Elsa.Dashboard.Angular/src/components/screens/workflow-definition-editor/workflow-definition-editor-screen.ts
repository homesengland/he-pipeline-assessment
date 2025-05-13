import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ActivityContextMenuState, LayoutDirection, WorkflowDesignerMode } from 'src/components/designers/tree/models';
import { ConfigureComponentCustomButtonContext, EventTypes, VersionOptions, WorkflowDefinition, WorkflowInstance, WorkflowModel, WorkflowTestActivityMessage } from 'src/models';
import { ActivityStats } from 'src/services/workflow-client';
import { HTMLElsaConfirmDialogElement } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';

@Component({
  selector: 'workflow-definition-editor-screen',
  templateUrl: './workflow-definition-editor-screen.html',
  styleUrls: ['./workflow-definition-editor-screen.css'],
  standalone: false,
})
export class WorkflowDefinitionEditorScreen implements OnInit {
  @Output() workflowSaved: EventEmitter<WorkflowDefinition>;
  @Input() workflowDefinitionId: string;
  @Input() serverUrl: string;
  @Input() monacoLibPath: string;
  @Input() features: string;
  @Input() culture: string;
  @Input() basePath: string;
  @Input() serverFeatures: Array<string> = [];
  //@Input() history: RouterHistory;
  workflowDefinition: WorkflowDefinition;
  workflowModel: WorkflowModel;
  publishing: boolean;
  unPublishing: boolean;
  unPublished: boolean;
  saving: boolean;
  saved: boolean;
  importing: boolean;
  imported: boolean;
  reverting: boolean;
  reverted: boolean;
  networkError: string;
  selectedActivityId?: string;
  workflowDesignerMode: WorkflowDesignerMode;
  workflowTestActivityMessages: Array<WorkflowTestActivityMessage> = [];
  workflowInstance?: WorkflowInstance;
  workflowInstanceId?: string;
  activityStats?: ActivityStats;
  layoutDirection: LayoutDirection = LayoutDirection.TopBottom;

  activityContextMenuState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
    selectedActivities: {},
  };

  activityContextMenuTestState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
  };

  //i18next: i18n;
  el: HTMLElement;
  designer: any; // Replace 'any' with the correct type if available or import it if it's missing
  configureComponentCustomButtonContext: ConfigureComponentCustomButtonContext = null;
  helpDialog: HTMLElement; // Replace with the correct type if HTMLElsaModalDialogElement is defined elsewhere
  activityContextMenu: HTMLDivElement;
  componentCustomButton: HTMLDivElement;
  confirmDialog: HTMLElsaConfirmDialogElement;

  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  async getWorkflowDefinitionId(): Promise<string> {
    return this.workflowDefinition.definitionId;
  }

  async exportWorkflow() {
    const client = await createElsaClient(this.serverUrl);
    const workflowDefinition = this.workflowDefinition;
    const versionOptions: VersionOptions = { version: workflowDefinition.version };
    const response = await client.workflowDefinitionsApi.export(workflowDefinition.definitionId, versionOptions);
    downloadFromBlob(response.data, { contentType: 'application/json', fileName: response.fileName });
  }

  async importWorkflow(file: File) {
    const client = await createElsaClient(this.serverUrl);

    this.importing = true;
    this.imported = false;
    this.networkError = null;

    try {
      const workflowDefinition = await client.workflowDefinitionsApi.import(this.workflowDefinition.definitionId, file);
      this.workflowDefinition = workflowDefinition;
      this.workflowModel = this.mapWorkflowModel(workflowDefinition);
      this.updateUrl(workflowDefinition.definitionId);

      this.importing = false;
      this.imported = true;
      setTimeout(() => (this.imported = false), 500);
      await eventBus.emit(EventTypes.WorkflowImported, this, this.workflowModel);
    } catch (e) {
      console.error(e);
      this.importing = false;
      this.imported = false;
      this.networkError = e.message;
      setTimeout(() => (this.networkError = null), 10000);
    }
  }

  ngOnInit(): void {}
}
