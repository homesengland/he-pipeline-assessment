import { Component, ElementRef, EventEmitter, HostListener, Input, OnInit, Output, ViewChild, viewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ActivityContextMenuState, LayoutDirection, WorkflowDesignerMode } from 'src/components/designers/tree/models';
import {
  ActivityDefinition,
  ActivityDescriptor,
  ActivityModel,
  ComponentCustomButtonClickContext,
  ConfigureComponentCustomButtonContext,
  ConnectionDefinition,
  ConnectionModel,
  EventTypes,
  VersionOptions,
  WorkflowDefinition,
  WorkflowDefinitionVersion,
  WorkflowInstance,
  WorkflowModel,
  WorkflowPersistenceBehavior,
  WorkflowStorageDescriptor,
  WorkflowTestActivityMessage,
  WorkflowTestActivityMessageStatus,
} from 'src/models';
import { ActivityStats, SaveWorkflowDefinitionRequest } from 'src/services/workflow-client';
import { HTMLElsaConfirmDialogElement } from 'src/models/elsa-interfaces';
import { eventBus } from 'src/services/event-bus';
import { ElsaClientService } from 'src/services/elsa-client';
import { downloadFromBlob } from 'src/utils/download';
import { Location } from '@angular/common';
import { AppStateActionGroup } from 'src/components/state/actions/app.state.actions';
import { Store } from '@ngrx/store';
import { featuresDataManager } from 'src/services/features-data-manager';
import { selectServerUrl } from 'src/components/state/selectors/app.state.selectors';

@Component({
  selector: 'workflow-definition-editor-screen',
  templateUrl: './workflow-definition-editor-screen.html',
  styleUrls: ['./workflow-definition-editor-screen.css'],
  standalone: false,
  host: { class: 'elsa-flex elsa-flex-col elsa-w-full elsa-relative' },
})
export class WorkflowDefinitionEditorScreen implements OnInit {
  @Output() workflowSaved: EventEmitter<WorkflowDefinition>;
  @Input() workflowDefinitionId: string;
  serverUrl: string;
  // @Input() monacoLibPath: string;
  // @Input() features: string;
  basePath: string;
  // @Input() serverFeatures: Array<string> = [];

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

  //helpDialog: ElementRef;
  @ViewChild('designerTree') designerTree;
  // readonly helpDialog = viewChild.required<ElementRef>('helpDialog');
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

  el: HTMLElement;
  configureComponentCustomButtonContext: ConfigureComponentCustomButtonContext = null;
  @ViewChild('activityContextMenu') activityContextMenu;
  componentCustomButton: HTMLDivElement;
  confirmDialog: HTMLElsaConfirmDialogElement;
  activityDescriptors: ActivityDescriptor[];
  editWorkflowDesignerMode: WorkflowDesignerMode = WorkflowDesignerMode.Edit;
  workflowStorageDescriptors: WorkflowStorageDescriptor[] = [];

  constructor(private store: Store, private elsaClientService: ElsaClientService, private router: Router, private route: ActivatedRoute, private location: Location) {}

  async getServerUrl(): Promise<string> {
    return this.serverUrl;
  }

  async getWorkflowDefinitionId(): Promise<string> {
    return this.workflowDefinition.definitionId;
  }

  private setVariablesFromAppState(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
  }

  async exportWorkflow() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const workflowDefinition = this.workflowDefinition;
    const versionOptions: VersionOptions = { version: workflowDefinition.version };
    const response = await client.workflowDefinitionsApi.export(workflowDefinition.definitionId, versionOptions);
    downloadFromBlob(response.data, { contentType: 'application/json', fileName: response.fileName });
  }

  async importWorkflow(file: File) {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);

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

  async workflowDefinitionIdChangedHandler(newValue: string) {
    const workflowDefinitionId = newValue;
    let workflowDefinition: WorkflowDefinition = WorkflowDefinitionEditorScreen.createWorkflowDefinition();
    workflowDefinition.definitionId = workflowDefinitionId;
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);

    if (workflowDefinitionId && workflowDefinitionId.length > 0) {
      try {
        workflowDefinition = await client.workflowDefinitionsApi.getByDefinitionAndVersion(workflowDefinitionId, { isLatest: true });
      } catch {
        console.warn(`The specified workflow definition does not exist. Creating a new one.`);
      }
    }

    this.updateWorkflowDefinition(workflowDefinition);
  }

  async serverUrlChangedHandler(newValue: string) {
    if (newValue && newValue.length > 0) {
      await this.loadActivityDescriptors();
      await this.loadWorkflowStorageDescriptors();
    }
  }

  async loadActivityDescriptors() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const activityDescriptors = await client.activitiesApi.list();
    this.activityDescriptors = activityDescriptors;
    this.store.dispatch(
      AppStateActionGroup.setActivityDefinitions({
        activityDefinitions: this.activityDescriptors,
      }),
    );
  }

  async loadWorkflowStorageDescriptors() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const workflowStorageDescriptors = await client.workflowStorageProvidersApi.list();
    this.workflowStorageDescriptors = workflowStorageDescriptors;
  }

  @HostListener('document:click', ['$event'])
  onWindowClicked(event: Event): void {
    const target = event.target as HTMLElement;
    //if (!this.componentCustomButton.contains(target)) this.handleContextMenuTestChange(0, 0, false, null);

    if (!this.activityContextMenu.nativeElement.contains(target)) this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null, selectedActivities: {} });
  }

  updateWorkflowDefinition(value: WorkflowDefinition) {
    this.workflowDefinition = value;
    this.workflowModel = this.mapWorkflowModel(value);
  }

  private static createWorkflowDefinition(): WorkflowDefinition {
    return {
      definitionId: null,
      version: 1,
      isLatest: true,
      isPublished: false,
      activities: [],
      connections: [],
      persistenceBehavior: WorkflowPersistenceBehavior.WorkflowBurst,
    };
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe(async params => {
      const id = params.get('workflowDefinitionId');
      if (id) {
        this.workflowDefinitionId = id;
        await this.workflowDefinitionIdChangedHandler(this.workflowDefinitionId);
      }
    });
    this.setVariablesFromAppState();
    await this.loadActivityDescriptors();
    await this.workflowDefinitionIdChangedHandler(this.workflowDefinitionId);
    this.workflowDesignerMode = WorkflowDesignerMode.Edit;

    const layoutFeature = featuresDataManager.getFeatureConfig(featuresDataManager.supportedFeatures.workflowLayout);
    if (layoutFeature && layoutFeature.enabled) {
      this.layoutDirection = layoutFeature.value as LayoutDirection;
    }
  }

  ngAfterViewInit() {
    if (!this.designerTree) {
      const designerTreeElement = this.designerTree.nativeElement;
      if (designerTreeElement) {
        designerTreeElement.model = this.workflowModel;
      }
    }
  }

  ngAfterViewChecked() {
    if (this.el && this.componentCustomButton) {
      let modalX = this.activityContextMenuTestState.x + 64;
      let modalY = this.activityContextMenuTestState.y - 256;

      // Fit the modal to the canvas bounds
      const canvasBounds = this.el?.getBoundingClientRect();
      const modalBounds = this.componentCustomButton.getBoundingClientRect();
      const modalWidth = modalBounds?.width;
      const modalHeight = modalBounds?.height;
      modalX = Math.min(canvasBounds.width, modalX + modalWidth + 32) - modalWidth - 32;
      modalY = Math.min(canvasBounds.height, modalY + modalHeight) - modalHeight - 32;
      modalY = Math.max(0, modalY);

      this.componentCustomButton.style.left = `${modalX}px`;
      this.componentCustomButton.style.top = `${modalY}px`;
    }
  }

  connectedCallback() {
    eventBus.on(EventTypes.UpdateWorkflowSettings, this.onUpdateWorkflowSettings);
    eventBus.on(EventTypes.FlyoutPanelTabSelected, this.onFlyoutPanelTabSelected);
    eventBus.on(EventTypes.TestActivityMessageReceived, this.onTestActivityMessageReceived);
    eventBus.on(EventTypes.UpdateActivity, this.onUpdateActivity);
  }

  disconnectedCallback() {
    eventBus.detach(EventTypes.UpdateWorkflowSettings, this.onUpdateWorkflowSettings);
    eventBus.detach(EventTypes.FlyoutPanelTabSelected, this.onFlyoutPanelTabSelected);
    eventBus.detach(EventTypes.TestActivityMessageReceived, this.onTestActivityMessageReceived);
    eventBus.detach(EventTypes.UpdateActivity, this.onUpdateActivity);
  }

  async configureComponentCustomButton(message: WorkflowTestActivityMessage) {
    this.configureComponentCustomButtonContext = {
      component: 'elsa-workflow-definition-editor-screen',
      activityType: message.activityType,
      prop: null,
      data: null,
    };
    await eventBus.emit(EventTypes.ComponentLoadingCustomButton, this, this.configureComponentCustomButtonContext);
  }

  async tryUpdateActivityInformation(activityId: string) {
    if (!this.workflowInstanceId) {
      this.activityStats = null;
      this.workflowInstance = null;

      return;
    }

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);

    this.activityStats = await client.activityStatsApi.get(this.workflowInstanceId, activityId);

    if (!this.workflowInstance || this.workflowInstance.id !== this.workflowInstanceId) this.workflowInstance = await client.workflowInstancesApi.get(this.workflowInstanceId);
  }

  async publishWorkflow() {
    this.publishing = true;
    await this.saveWorkflow(true);
    this.publishing = false;
    await eventBus.emit(EventTypes.WorkflowPublished, this, this.workflowDefinition);
  }

  async unpublishWorkflow() {
    await this.unpublishWorkflowInternal();
    await eventBus.emit(EventTypes.WorkflowRetracted, this, this.workflowDefinition);
  }

  async revertWorkflow() {
    await this.revertWorkflowInternal();
  }

  async saveWorkflow(publish?: boolean) {
    await this.saveWorkflowInternal(null, publish);
  }

  async unpublishWorkflowInternal() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const workflowDefinitionId = this.workflowDefinition.definitionId;
    this.unPublishing = true;

    try {
      this.workflowDefinition = await client.workflowDefinitionsApi.retract(workflowDefinitionId);
      this.unPublishing = false;
      this.unPublished = true;
      setTimeout(() => (this.unPublished = false), 500);
    } catch (e) {
      console.error(e);
      this.unPublishing = false;
      this.unPublished = false;
      this.networkError = e.message;
      setTimeout(() => (this.networkError = null), 2000);
    }
  }

  async revertWorkflowInternal() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const workflowDefinitionId = this.workflowDefinition.definitionId;
    const version = this.workflowDefinition.version;
    this.reverting = true;

    try {
      this.workflowDefinition = await client.workflowDefinitionsApi.revert(workflowDefinitionId, version);
      this.reverting = false;
      this.reverted = true;
      setTimeout(() => (this.reverted = false), 500);
    } catch (e) {
      console.error(e);
      this.reverting = false;
      this.reverted = false;
      this.networkError = e.message;
      setTimeout(() => (this.networkError = null), 2000);
    }
  }

  private onUpdateWorkflowSettings = async (workflowDefinition: WorkflowDefinition) => {
    this.updateWorkflowDefinition(workflowDefinition);
    await this.saveWorkflowInternal(this.workflowModel);
  };

  private onFlyoutPanelTabSelected = async args => {
    const tab = args;
    if (tab === 'general') this.workflowDesignerMode = WorkflowDesignerMode.Edit;
    if (tab === 'test') this.workflowDesignerMode = WorkflowDesignerMode.Test;
  };

  onUpdateActivity = (activity: ActivityModel) => {
    const message = this.workflowTestActivityMessages.find(x => x.activityId === activity.activityId);

    if (message) {
      message.status = WorkflowTestActivityMessageStatus.Modified;
      this.clearSubsequentWorkflowTestMessages(activity.activityId);
    }
  };

  private clearSubsequentWorkflowTestMessages(activityId: string) {
    const targetActivityId = this.workflowDefinition.connections.find(x => x.sourceActivityId === activityId)?.targetActivityId;

    if (!targetActivityId) return;

    this.workflowTestActivityMessages = this.workflowTestActivityMessages.filter(x => x.activityId !== targetActivityId || x.status === WorkflowTestActivityMessageStatus.Failed);

    this.clearSubsequentWorkflowTestMessages(targetActivityId);
  }

  onTestActivityMessageReceived = async args => {
    const message = args as WorkflowTestActivityMessage;

    if (!!message) {
      this.workflowInstanceId = message.workflowInstanceId;
      this.workflowTestActivityMessages = this.workflowTestActivityMessages.filter(x => x.activityId !== message.activityId);
      this.workflowTestActivityMessages = [...this.workflowTestActivityMessages, message];
    } else {
      this.workflowTestActivityMessages = [];
      this.workflowInstanceId = null;
    }
  };

  async saveWorkflowInternal(workflowModel?: WorkflowModel, publish?: boolean) {
    if (!this.serverUrl || this.serverUrl.length == 0) return;

    workflowModel = workflowModel || this.workflowModel;

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    let workflowDefinition = this.workflowDefinition;
    const isNew = typeof workflowDefinition.definitionId === 'undefined' && typeof this.workflowDefinitionId === 'undefined';

    const request: SaveWorkflowDefinitionRequest = {
      workflowDefinitionId: workflowDefinition.definitionId || this.workflowDefinitionId,
      contextOptions: workflowDefinition.contextOptions,
      deleteCompletedInstances: workflowDefinition.deleteCompletedInstances,
      description: workflowDefinition.description,
      displayName: workflowDefinition.displayName,
      isSingleton: workflowDefinition.isSingleton,
      name: workflowDefinition.name,
      tag: workflowDefinition.tag,
      channel: workflowDefinition.channel,
      persistenceBehavior: workflowDefinition.persistenceBehavior,
      publish: publish || false,
      variables: workflowDefinition.variables,
      activities: workflowModel.activities.map<ActivityDefinition>(x => ({
        activityId: x.activityId,
        type: x.type,
        name: x.name,
        displayName: x.displayName,
        description: x.description,
        x: x.x,
        y: x.y,
        persistWorkflow: x.persistWorkflow,
        loadWorkflowContext: x.loadWorkflowContext,
        saveWorkflowContext: x.saveWorkflowContext,
        properties: x.properties,
        propertyStorageProviders: x.propertyStorageProviders,
        category: '',
      })),
      connections: workflowModel.connections.map<ConnectionDefinition>(x => ({
        sourceActivityId: x.sourceId,
        targetActivityId: x.targetId,
        outcome: x.outcome,
      })),
    };

    this.saving = !publish;
    this.publishing = publish;

    try {
      console.debug('Saving workflow...');

      workflowDefinition = await client.workflowDefinitionsApi.save(request);
      this.workflowDefinition = workflowDefinition;
      this.workflowModel = this.mapWorkflowModel(workflowDefinition);

      this.saving = false;
      this.saved = !publish;
      this.publishing = false;
      setTimeout(() => (this.saved = false), 500);
      this.workflowSaved.emit(workflowDefinition);
      if (isNew) {
        this.updateUrl(workflowDefinition.definitionId);
      }
    } catch (e) {
      console.error(e);
      this.saving = false;
      this.saved = false;
      this.networkError = e.message;
      setTimeout(() => (this.networkError = null), 10000);
    }
  }

  updateUrl(workflowInstanceId) {
    const currentQueryParams = this.route.snapshot.queryParams;

    const url = this.router.createUrlTree(['workflow-instances', workflowInstanceId], { queryParams: currentQueryParams }).toString();

    this.location.go(url);

    this.workflowInstanceId = workflowInstanceId;
  }

  mapWorkflowModel(workflowDefinition: WorkflowDefinition): WorkflowModel {
    return {
      activities: workflowDefinition.activities.map(activity => this.mapActivityModel(activity)),
      connections: workflowDefinition.connections.map(connection => this.mapConnectionModel(connection)),
      persistenceBehavior: workflowDefinition.persistenceBehavior,
    };
  }

  mapActivityModel(source: ActivityDefinition): ActivityModel {
    const activityDescriptors: Array<ActivityDescriptor> = this.activityDescriptors;
    const activityDescriptor = activityDescriptors.find(x => x.type == source.type);
    const outcomes = !!activityDescriptor ? activityDescriptor.outcomes : ['Done'];

    return {
      activityId: source.activityId,
      description: source.description,
      displayName: source.displayName,
      x: source.x,
      y: source.y,
      name: source.name,
      type: source.type,
      properties: source.properties,
      outcomes: [...outcomes],
      persistWorkflow: source.persistWorkflow,
      saveWorkflowContext: source.saveWorkflowContext,
      loadWorkflowContext: source.loadWorkflowContext,
      propertyStorageProviders: source.propertyStorageProviders,
    };
  }

  mapConnectionModel(source: ConnectionDefinition): ConnectionModel {
    return {
      sourceId: source.sourceActivityId,
      targetId: source.targetActivityId,
      outcome: source.outcome,
    };
  }

  async handleContextMenuChange(state: ActivityContextMenuState) {
    this.activityContextMenuState = state;
  }

  handleContextMenuTestChange(x: number, y: number, shown: boolean, activity: ActivityModel) {
    this.activityContextMenuTestState = {
      shown,
      x,
      y,
      activity,
    };
  }

  async onShowWorkflowSettingsClick() {
    await eventBus.emit(EventTypes.ShowWorkflowSettings);
  }

  async onDeleteClicked() {
    const result = await this.confirmDialog.show('DeleteConfirmationModel.Title', 'DeleteConfirmationModel.Message');

    if (!result) return;

    const elsaClient = await this.elsaClientService.createElsaClient(this.serverUrl);
    await elsaClient.workflowDefinitionsApi.delete(this.workflowDefinition.definitionId, { allVersions: true });
    this.updateUrl(`${this.basePath}/workflow-definitions`);
  }

  async onPublishClicked() {
    await this.publishWorkflow();
  }

  async onUnPublishClicked() {
    await this.unpublishWorkflow();
  }

  async onRevertClicked() {
    await this.revertWorkflow();
  }

  async onExportClicked() {
    await this.exportWorkflow();
  }

  async onImportClicked(file: File) {
    await this.importWorkflow(file);
  }

  onDeleteActivityClick = async (e: Event) => {
    e.preventDefault();
    const { activity, selectedActivities } = this.activityContextMenuState;

    if (selectedActivities[activity.activityId]) {
      await this.designerTree.removeSelectedActivities();
    } else {
      await this.designerTree.removeActivity(activity);
    }

    this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null, selectedActivities: {} });
    await eventBus.emit(EventTypes.HideModalDialog);
  };

  onEditActivityClick = async (e: Event) => {
    e.preventDefault();
    await this.designerTree.showActivityEditor(this.activityContextMenuState.activity, true);
    this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null });
  };

  onActivityContextMenuButtonClicked = async (e: ActivityContextMenuState) => {
    this.activityContextMenuState = e;
  };

  onActivityContextMenuButtonTestClicked = async (e: ActivityContextMenuState) => {
    this.selectedActivityId = e.activity.activityId;

    if (!e.shown) {
      return;
    }

    await this.tryUpdateActivityInformation(this.selectedActivityId);
  };

  onActivitySelected = async (e: ActivityModel) => {
    this.selectedActivityId = e.activityId;
  };

  onActivityDeselected = async (e: ActivityModel) => {
    if (this.selectedActivityId == e.activityId) this.selectedActivityId = null;
  };

  async onRestartActivityButtonClick() {
    await eventBus.emit(EventTypes.WorkflowRestarted, this, this.selectedActivityId);

    this.handleContextMenuTestChange(0, 0, false, null);
  }

  async onComponentCustomButtonClick(message: WorkflowTestActivityMessage) {
    let workflowModel = { ...this.workflowModel };
    const activityModel = workflowModel.activities.find(x => x.activityId == message.activityId);
    const input = message.data['Input'];

    const componentCustomButtonClickContext: ComponentCustomButtonClickContext = {
      component: 'elsa-workflow-definition-editor-screen',
      activityType: message.activityType,
      prop: null,
      params: [activityModel, input],
    };
    await eventBus.emit(EventTypes.ComponentCustomButtonClick, this, componentCustomButtonClickContext);
  }

  private canBeRestartedFromCurrentActivity() {
    if (!this.selectedActivityId) return false;
    if (this.workflowDesignerMode !== WorkflowDesignerMode.Test) return false;

    if (this.workflowTestActivityMessages.some(x => x.activityId === this.selectedActivityId)) return true;

    const sourceActivityId = this.workflowDefinition.connections.find(x => x.targetActivityId === this.selectedActivityId)?.sourceActivityId;

    return sourceActivityId && this.workflowTestActivityMessages.some(x => x.activityId === sourceActivityId && x.status !== WorkflowTestActivityMessageStatus.Failed);
  }

  showHelpModal = async () => {
    //await this.helpDialog().nativeElement.show();
  };

  handleFeatureChange = (e: CustomEvent<string>) => {
    const feature = e.detail;

    if (feature === featuresDataManager.supportedFeatures.workflowLayout) {
      const layoutFeature = featuresDataManager.getFeatureConfig(feature);
      this.layoutDirection = layoutFeature.value as LayoutDirection;
    }
  };

  handleFeatureStatusChange = (e: CustomEvent<string>) => {
    const feature = e.detail;

    if (feature === featuresDataManager.supportedFeatures.workflowLayout) {
      const layoutFeature = featuresDataManager.getFeatureConfig(feature);
      if (layoutFeature.enabled) {
        this.layoutDirection = layoutFeature.value as LayoutDirection;
      } else {
        this.layoutDirection = LayoutDirection.TopBottom;
      }
    }
  };

  onVersionSelected = async (e: CustomEvent<WorkflowDefinitionVersion>) => {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const version = e.detail;
    const workflowDefinition = await client.workflowDefinitionsApi.getByDefinitionAndVersion(version.definitionId, { version: version.version });
    this.updateWorkflowDefinition(workflowDefinition);
  };

  onDeleteVersionClicked = async (e: CustomEvent<WorkflowDefinitionVersion>) => {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const version = e.detail;
    await client.workflowDefinitionsApi.delete(version.definitionId, { version: version.version });
    this.updateWorkflowDefinition({ ...this.workflowDefinition }); // Force a rerender.
  };

  onRevertVersionClicked = async (e: CustomEvent<WorkflowDefinitionVersion>) => {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    const version = e.detail;
    const workflowDefinition = await client.workflowDefinitionsApi.revert(version.definitionId, version.version);
    this.updateWorkflowDefinition(workflowDefinition);
  };

  getActivityContextMenuButton() {}

  renderActivityContextMenuButton = (activity: ActivityModel): string => {
    return `<div class="context-menu-wrapper elsa-flex-shrink-0">
<button aria-haspopup="true"
        class="elsa-w-8 elsa-h-8 elsa-inline-flex elsa-items-center elsa-justify-center elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
  <svg class="elsa-h-6 elsa-w-6 elsa-text-gray-400" width="24" height="24" viewBox="0 0 24 24" stroke-width="2"
       stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
    <path stroke="none" d="M0 0h24v24H0z"/>
    <circle cx="5" cy="12" r="1"/>
    <circle cx="12" cy="12" r="1"/>
    <circle cx="19" cy="12" r="1"/>
  </svg>
</button>
</div>`;
  };

  renderActivityStatsButton = (activity: ActivityModel): string => {
    const testActivityMessage = this.workflowTestActivityMessages.find(x => x.activityId == activity.activityId);
    if (testActivityMessage == undefined) return '';

    let icon: string;

    switch (testActivityMessage.status) {
      case WorkflowTestActivityMessageStatus.Done:
      default:
        icon = `<svg class="elsa-h-8 elsa-w-8 elsa-text-green-500"  fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/>
                </svg>`;
        break;
      case WorkflowTestActivityMessageStatus.Waiting:
        icon = `<svg version="1.1" class="svg-loader" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 80 80" xml:space="preserve">
                  <path id="spinner" fill="#7eb0de" d="M40,72C22.4,72,8,57.6,8,40C8,22.4,
                  22.4,8,40,8c17.6,0,32,14.4,32,32c0,1.1-0.9,2-2,2
                  s-2-0.9-2-2c0-15.4-12.6-28-28-28S12,24.6,12,40s12.6,
                  28,28,28c1.1,0,2,0.9,2,2S41.1,72,40,72z">
                    <animateTransform attributeType="xml" attributeName="transform" type="rotate" from="0 40 40" to="360 40 40" dur="0.75s" repeatCount="indefinite" />
                  </path>
                  </path>
              </svg>`;
        break;
      case WorkflowTestActivityMessageStatus.Failed:
        icon = `<svg class="elsa-h-8 elsa-w-8 elsa-text-red-500"  viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="15" y1="9" x2="9" y2="15" />
                  <line x1="9" y1="9" x2="15" y2="15" />
                </svg>`;
        break;
      case WorkflowTestActivityMessageStatus.Modified:
        icon = `<svg class="h-6 w-6 elsa-text-yellow-500" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <circle cx="12" cy="12" r="10"></circle>
                    <line x1="12" y1="16" x2="12" y2="12"></line>
                    <line x1="12" y1="8" x2="12.01" y2="8"></line>
                </svg>`;
        break;
    }

    return `<div class="context-menu-wrapper elsa-flex-shrink-0">
            <button aria-haspopup="true"
                    class="elsa-w-8 elsa-h-8 elsa-inline-flex elsa-items-center elsa-justify-center elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
              ${icon}
            </button>
          </div>`;
  };

  get selectedActivities(): string[] {
    return Object.keys(this.activityContextMenuState?.selectedActivities || {});
  }
}
