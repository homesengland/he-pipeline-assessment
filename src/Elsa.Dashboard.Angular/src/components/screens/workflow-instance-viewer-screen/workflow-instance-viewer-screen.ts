import { Component, computed, effect, ElementRef, HostListener, Input, input, OnDestroy, OnInit, signal, viewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { ActivatedRoute } from '@angular/router';
import { selectServerUrl } from '../../state/selectors/app.state.selectors';
import { Location } from '@angular/common';
import {
  ActivityBlueprint,
  ActivityDefinitionProperty,
  ActivityDescriptor,
  ActivityModel,
  Connection,
  ConnectionModel,
  SyntaxNames,
  WorkflowBlueprint,
  WorkflowExecutionLogRecord,
  WorkflowFault,
  WorkflowInstance,
  WorkflowModel,
  WorkflowPersistenceBehavior,
  WorkflowStatus,
  WorkflowStorageDescriptor,
} from 'src/models';
import { ActivityStats, ActivityEventCount } from 'src/services/workflow-client';
import { ActivityContextMenuState, LayoutDirection } from 'src/components/designers/tree/models';
import { ElsaClientService } from 'src/services/elsa-client';
import * as collection from 'lodash/collection';
import { featuresDataManager } from 'src/services/features-data-manager';

@Component({
  selector: 'workflow-instance-viewer-screen',
  templateUrl: './workflow-instance-viewer-screen.html',
  styleUrls: ['./workflow-instance-viewer-screen.css'],
  standalone: false,
  host: {
    class: 'elsa-flex elsa-flex-col elsa-w-full elsa-relative',
  },
})
export class WorkflowInstanceViewerScreen implements OnInit, OnDestroy {
  @Input() workflowInstanceId = '';
  serverUrl: string;
  workflowInstance: WorkflowInstance;
  workflowBlueprint: WorkflowBlueprint;
  workflowModel: WorkflowModel;
  selectedActivityId?: string;

  activityStats?: ActivityStats;

  activityContextMenuState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
  };

  // designer: HTMLElsaDesignerTreeElement;
  // journal: HTMLElsaWorkflowInstanceJournalElement;
  readonly contextMenu = viewChild.required<ElementRef>('contextMenu');
  layoutDirection = LayoutDirection.TopBottom;

  activityDescriptors: ActivityDescriptor[];
  workflowStorageDescriptors: WorkflowStorageDescriptor[];
  private clearRouteChangedListeners: () => void;

  workflowFault: WorkflowFault;
  route: ActivatedRoute;

  constructor(private store: Store, private elsaClientService: ElsaClientService, private location: Location, private el: ElementRef) {}

  ngOnDestroy(): void {
    if (this.clearRouteChangedListeners) {
      this.clearRouteChangedListeners();
    }
  }

  async ngOnInit(): Promise<void> {
    this.clearRouteChangedListeners = this.location.onUrlChange(async (url, state) => {
      this.workflowInstanceId = url.split('/').pop();
      await this.workflowInstanceIdChangedHandler(this.workflowInstanceId);
    });
    this.setVariablesFromAppState();
    await this.loadActivityDescriptors();

    await this.workflowInstanceIdChangedHandler(this.workflowInstanceId);

    const layoutFeature = featuresDataManager.getFeatureConfig(featuresDataManager.supportedFeatures.workflowLayout);

    if (layoutFeature && layoutFeature.enabled) {
      this.layoutDirection = layoutFeature.value as LayoutDirection;
    }
  }

  @HostListener('document:click', ['$event'])
  onWindowClicked(event: Event): void {
    const target = event.target as HTMLElement;
    if (!this.contextMenu().nativeElement.contains(target)) this.handleContextMenuChange(0, 0, false, null);
  }

  private setVariablesFromAppState(): void {
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl = data;
    });
  }

  async workflowInstanceIdChangedHandler(newValue: string) {
    const workflowInstanceId = newValue;
    let workflowInstance: WorkflowInstance = {
      id: null,
      definitionId: null,
      definitionVersionId: null,
      version: null,
      workflowStatus: WorkflowStatus.Idle,
      variables: { data: {} },
      blockingActivities: [],
      scheduledActivities: [],
      scopes: [],
      currentActivity: { activityId: '' },
    };

    let workflowBlueprint: WorkflowBlueprint = {
      id: null,
      version: 1,
      activities: [],
      connections: [],
      persistenceBehavior: WorkflowPersistenceBehavior.WorkflowBurst,
      customAttributes: { data: {} },
      persistWorkflow: false,
      isLatest: false,
      isPublished: false,
      loadWorkflowContext: false,
      isSingleton: false,
      saveWorkflowContext: false,
      variables: { data: {} },
      type: null,
      inputProperties: { data: {} },
      outputProperties: { data: {} },
      propertyStorageProviders: {},
    };

    const client = await this.elsaClientService.createElsaClient(this.serverUrl);

    if (workflowInstanceId && workflowInstanceId.length > 0) {
      try {
        workflowInstance = await client.workflowInstancesApi.get(workflowInstanceId);
        workflowBlueprint = await client.workflowRegistryApi.get(workflowInstance.definitionId, { version: workflowInstance.version });
      } catch {
        console.warn(`The specified workflow definition does not exist. Creating a new one.`);
      }
    }

    this.updateModels(workflowInstance, workflowBlueprint);
  }

  async serverUrlChangedHandler(newValue: string) {
    if (newValue && newValue.length > 0) {
      await this.loadActivityDescriptors();
      await this.loadWorkflowStorageDescriptors();
    }
  }

  ngAfterViewChecked() {
    // if (this.el && this.contextMenu) {
    //   let modalX = this.activityContextMenuState.x + 64;
    //   let modalY = this.activityContextMenuState.y - 256;
    //  // Fit the modal to the canvas bounds
    //   const canvasBounds = this.el?.getBoundingClientRect();
    //   const modalBounds = this.contextMenu.getBoundingClientRect();
    //   const modalWidth = modalBounds?.width;
    //   const modalHeight = modalBounds?.height;
    //   modalX = Math.min(canvasBounds.width, modalX + modalWidth + 32) - modalWidth - 32;
    //   modalY = Math.min(canvasBounds.height, modalY + modalHeight) - modalHeight - 32;
    //   modalY = Math.max(0, modalY);
    //   this.contextMenu.style.left = `${modalX}px`;
    //   this.contextMenu.style.top = `${modalY}px`;
    // }
  }

  ngAfterViewInit() {
    //     if (!this.designer) {
    //       this.designer = this.el.querySelector('elsa-designer-tree') as HTMLElsaDesignerTreeElement;
    //       this.designer.model = this.workflowModel;
    //     }
  }

  async loadWorkflowStorageDescriptors() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.workflowStorageDescriptors = await client.workflowStorageProvidersApi.list();
  }

  updateModels(workflowInstance: WorkflowInstance, workflowBlueprint: WorkflowBlueprint) {
    this.workflowInstance = workflowInstance;
    this.workflowBlueprint = workflowBlueprint;
    this.workflowModel = this.mapWorkflowModel(workflowBlueprint, workflowInstance);
    if (this.workflowInstance != null && this.workflowInstance.faults != null) {
      this.workflowFault = this.workflowInstance.faults.find(x => x.faultedActivityId == this.selectedActivityId);
    }
  }

  mapWorkflowModel(workflowBlueprint: WorkflowBlueprint, workflowInstance: WorkflowInstance): WorkflowModel {
    const activities = workflowBlueprint.activities.filter(x => x.parentId == workflowBlueprint.id || !x.parentId).map(x => this.mapActivityModel(x, workflowInstance));
    const connections = workflowBlueprint.connections
      .filter(c => activities.findIndex(a => a.activityId == c.sourceActivityId || a.activityId == c.targetActivityId) > -1)
      .map(this.mapConnectionModel);

    return {
      activities: activities,
      connections: connections,
      persistenceBehavior: workflowBlueprint.persistenceBehavior,
    };
  }

  mapActivityModel(activityBlueprint: ActivityBlueprint, workflowInstance: WorkflowInstance): ActivityModel {
    const activityDescriptors: Array<ActivityDescriptor> = this.activityDescriptors;
    const activityDescriptor = activityDescriptors.find(x => x.type == activityBlueprint.type);
    const activityData = workflowInstance.activityData[activityBlueprint.id] || {};

    const properties: Array<ActivityDefinitionProperty> = collection.map(activityBlueprint.inputProperties.data, (value, key) => {
      const propertyDescriptor = activityDescriptor.inputProperties.find(x => x.name == key) || activityDescriptor.outputProperties.find(x => x.name == key);
      const defaultSyntax = propertyDescriptor?.defaultSyntax || SyntaxNames.Literal;
      const expressions = {};
      const v = activityData[key] || value;
      expressions[defaultSyntax] = v;
      return { name: key, value: v, expressions: expressions, syntax: defaultSyntax };
    });

    return {
      activityId: activityBlueprint.id,
      description: activityBlueprint.description,
      displayName: activityBlueprint.displayName || activityBlueprint.name || activityBlueprint.type,
      name: activityBlueprint.name,
      type: activityBlueprint.type,
      properties: properties,
      outcomes: [...activityDescriptor.outcomes],
      persistWorkflow: activityBlueprint.persistWorkflow,
      saveWorkflowContext: activityBlueprint.saveWorkflowContext,
      loadWorkflowContext: activityBlueprint.loadWorkflowContext,
      propertyStorageProviders: activityBlueprint.propertyStorageProviders,
      x: activityBlueprint.x,
      y: activityBlueprint.y,
    };
  }

  mapConnectionModel(connection: Connection): ConnectionModel {
    return {
      sourceId: connection.sourceActivityId,
      targetId: connection.targetActivityId,
      outcome: connection.outcome,
    };
  }

  async loadActivityDescriptors() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.activityDescriptors = await client.activitiesApi.list();
  }

  handleContextMenuChange(x: number, y: number, shown: boolean, activity: ActivityModel) {
    this.activityContextMenuState = {
      shown,
      x,
      y,
      activity,
    };
  }

  onRecordSelected(e: CustomEvent<WorkflowExecutionLogRecord>) {
    const record = e.detail;
    const activity = !!record ? this.workflowBlueprint.activities.find(x => x.id === record.activityId) : null;
    this.selectedActivityId = activity != null ? (activity.parentId != null ? activity.parentId : activity.id) : null;
  }

  async onActivitySelected(e: CustomEvent<ActivityModel>) {
    this.selectedActivityId = e.detail.activityId;
    // await this.journal.selectActivityRecord(this.selectedActivityId);
  }

  async onActivityDeselected(e: CustomEvent<ActivityModel>) {
    if (this.selectedActivityId == e.detail.activityId) this.selectedActivityId = null;

    // await this.journal.selectActivityRecord(this.selectedActivityId);
  }

  async onActivityContextMenuButtonClicked(e: CustomEvent<ActivityContextMenuState>) {
    this.activityContextMenuState = e.detail;
    this.activityStats = null;

    if (!e.detail.shown) {
      return;
    }

    const elsaClient = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.activityStats = await elsaClient.activityStatsApi.get(this.workflowInstanceId, e.detail.activity.activityId);
  }

  getActivityBorderColor = (activity: ActivityModel): string => {
    const workflowInstance = this.workflowInstance;
    const workflowFault = !!workflowInstance ? workflowInstance.faults : null;
    const activityData = workflowInstance.activityData[activity.activityId] || {};
    const lifecycle = activityData['_Lifecycle'] || {};
    const executing = lifecycle.executing ?? lifecycle.Executing;
    const executed = lifecycle.executed ?? lifecycle.Executed;

    if (!!workflowFault && workflowFault.find(x => x.faultedActivityId == activity.activityId)) return 'red';

    if (executed) return 'green';

    if (executing) return 'blue';

    return null;
  };

  renderActivityStatsButton = (activity: ActivityModel): string => {
    const workflowInstance = this.workflowInstance;
    const workflowFault = !!workflowInstance ? workflowInstance.faults : null;
    const activityData = workflowInstance.activityData[activity.activityId] || {};
    const lifecycle = activityData['_Lifecycle'] || {};
    const executing = lifecycle.executing ?? lifecycle.Executing;
    const executed = lifecycle.executed ?? lifecycle.Executed;

    let icon: string;

    if (!!workflowFault && workflowFault.find(x => x.faultedActivityId == activity.activityId)) {
      icon = `<svg class="elsa-flex-shrink-0 elsa-h-6 elsa-w-6 elsa-text-red-600" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10"/>
                <line x1="12" y1="8" x2="12" y2="12"/>
                <line x1="12" y1="16" x2="12.01" y2="16"/>
              </svg>`;
    } else if (executed) {
      icon = `<svg class="elsa-h-6 elsa-w-6 elsa-text-green-500"  viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10" />
                <line x1="12" y1="16" x2="12" y2="12" />
                <line x1="12" y1="8" x2="12.01" y2="8" />
              </svg>`;
    } else if (executing) {
      icon = `<svg class="elsa-h-6 elsa-w-6 elsa-text-blue-500" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10" />
                <line x1="12" y1="16" x2="12" y2="12" />
                <line x1="12" y1="8" x2="12.01" y2="8" />
              </svg>`;
    } else {
      icon = `<svg class="h-6 w-6 text-gray-300" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                <circle cx="12" cy="12" r="10" />
                <line x1="12" y1="16" x2="12" y2="12" />
                <line x1="12" y1="8" x2="12.01" y2="8" />
              </svg>`;
    }

    return `<div class="context-menu-wrapper elsa-flex-shrink-0">
            <button aria-haspopup="true"
                    class="elsa-w-8 elsa-h-8 elsa-inline-flex elsa-items-center elsa-justify-center elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
              ${icon}
            </button>
          </div>`;
  };
}
