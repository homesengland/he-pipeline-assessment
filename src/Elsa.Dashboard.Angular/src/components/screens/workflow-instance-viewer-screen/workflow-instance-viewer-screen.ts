import { Component, ElementRef, HostListener, input, OnDestroy, OnInit, viewChild } from '@angular/core';
import { Store } from '@ngrx/store';
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
  WorkflowInstance,
  WorkflowModel,
  WorkflowPersistenceBehavior,
  WorkflowStatus,
} from 'src/models';
import { ActivityStats } from 'src/services/workflow-client';
import { ActivityContextMenuState, LayoutDirection } from 'src/components/designers/tree/models';
import { ElsaClientService } from 'src/services/elsa-client';
import * as collection from 'lodash/collection';

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
  readonly workflowId = input<string>(undefined);
  workflowInstance: WorkflowInstance;
  workflowBlueprint: WorkflowBlueprint;
  workflowModel: WorkflowModel;
  selectedActivityId?: string;
  activityStats?: ActivityStats;
  serverUrl: string;
  activityContextMenuState: ActivityContextMenuState = {
    shown: false,
    x: 0,
    y: 0,
    activity: null,
  };

  readonly contextMenu = viewChild.required<ElementRef>('contextMenu');

  layoutDirection = LayoutDirection.TopBottom;
  activityDescriptors: ActivityDescriptor[];
  workflowStorageDescriptors: import('c:/source/github/he-pipeline-assessment/src/Elsa.Dashboard.Angular/src/models/domain').WorkflowStorageDescriptor[];
  private clearRouteChangedListeners: () => void;

  constructor(private store: Store, private elsaClientService: ElsaClientService, private location: Location, private el: ElementRef) {}
  ngOnDestroy(): void {
    if (this.clearRouteChangedListeners) {
      this.clearRouteChangedListeners();
    }
  }

  async ngOnInit(): Promise<void> {
    this.clearRouteChangedListeners = this.location.onUrlChange(async (url, state) => {
      const workflowInstanceId = url.split('/').pop();
      await this.workflowInstanceIdChangedHandler(workflowInstanceId);
    });
    this.setVariablesFromAppState();
    await this.loadActivityDescriptors();
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

  async getServerUrl(): Promise<string> {
    return this.serverUrl;
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

  async loadWorkflowStorageDescriptors() {
    const client = await this.elsaClientService.createElsaClient(this.serverUrl);
    this.workflowStorageDescriptors = await client.workflowStorageProvidersApi.list();
  }

  updateModels(workflowInstance: WorkflowInstance, workflowBlueprint: WorkflowBlueprint) {
    this.workflowInstance = workflowInstance;
    this.workflowBlueprint = workflowBlueprint;
    this.workflowModel = this.mapWorkflowModel(workflowBlueprint, workflowInstance);
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
}
