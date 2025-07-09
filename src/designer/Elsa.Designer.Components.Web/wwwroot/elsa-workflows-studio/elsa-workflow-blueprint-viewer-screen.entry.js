import { r as registerInstance, h, k as Host } from './index-ea213ee1.js';
import { c as collection } from './collection-ba09a015.js';
import { a as WorkflowPersistenceBehavior, S as SyntaxNames } from './index-0f68dbd6.js';
import './index-c5018c3a.js';
import { a as state } from './store-52e2ea41.js';
import { W as WorkflowDesignerMode } from './models-96b27412.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { b as createElsaClient } from './elsa-client-ecb85def.js';
import './_commonjsHelpers-6cb8dacb.js';
import './event-bus-6625fc04.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';
import './axios-middleware.esm-fcda64d5.js';

let ElsaWorkflowBlueprintViewerScreen = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
  }
  async getServerUrl() {
    return this.serverUrl;
  }
  async workflowDefinitionIdChangedHandler(newValue) {
    const workflowDefinitionId = newValue;
    let workflowBlueprint = {
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
      propertyStorageProviders: {}
    };
    const client = await createElsaClient(this.serverUrl);
    if (workflowDefinitionId && workflowDefinitionId.length > 0) {
      try {
        workflowBlueprint = await client.workflowRegistryApi.get(workflowDefinitionId, { isLatest: true });
      }
      catch (_a) {
        console.warn(`The specified workflow blueprint does not exist. Creating a new one.`);
      }
    }
    this.updateModels(workflowBlueprint);
  }
  async serverUrlChangedHandler(newValue) {
    if (newValue && newValue.length > 0)
      await this.loadActivityDescriptors();
  }
  async componentWillLoad() {
    await this.serverUrlChangedHandler(this.serverUrl);
    await this.workflowDefinitionIdChangedHandler(this.workflowDefinitionId);
  }
  componentDidLoad() {
    if (!this.designer) {
      if (state.useX6Graphs) {
        this.designer = this.el.querySelector("x6-designer");
      }
      else {
        this.designer = this.el.querySelector('elsa-designer-tree');
      }
      this.designer.model = this.workflowModel;
    }
  }
  async loadActivityDescriptors() {
    const client = await createElsaClient(this.serverUrl);
    state.activityDescriptors = await client.activitiesApi.list();
  }
  updateModels(workflowBlueprint) {
    this.workflowBlueprint = workflowBlueprint;
    this.workflowModel = this.mapWorkflowModel(workflowBlueprint);
  }
  mapWorkflowModel(workflowBlueprint) {
    return {
      activities: workflowBlueprint.activities.filter(x => x.parentId == workflowBlueprint.id || !x.parentId).map(this.mapActivityModel),
      connections: workflowBlueprint.connections.map(this.mapConnectionModel),
      persistenceBehavior: workflowBlueprint.persistenceBehavior,
    };
  }
  mapActivityModel(source) {
    const activityDescriptors = state.activityDescriptors;
    const activityDescriptor = activityDescriptors.find(x => x.type == source.type);
    const properties = collection.map(source.inputProperties.data, (value, key) => {
      const propertyDescriptor = activityDescriptor.inputProperties.find(x => x.name == key);
      const defaultSyntax = (propertyDescriptor === null || propertyDescriptor === void 0 ? void 0 : propertyDescriptor.defaultSyntax) || SyntaxNames.Literal;
      const expressions = {};
      expressions[defaultSyntax] = value;
      return ({ name: key, expressions: expressions, syntax: defaultSyntax });
    });
    return {
      activityId: source.id,
      description: source.description,
      displayName: source.displayName || source.name || source.type,
      name: source.name,
      type: source.type,
      x: source.x,
      y: source.y,
      properties: properties,
      outcomes: [...activityDescriptor.outcomes],
      persistWorkflow: source.persistWorkflow,
      saveWorkflowContext: source.saveWorkflowContext,
      loadWorkflowContext: source.loadWorkflowContext,
      propertyStorageProviders: source.propertyStorageProviders,
    };
  }
  mapConnectionModel(connection) {
    return {
      sourceId: connection.sourceActivityId,
      targetId: connection.targetActivityId,
      outcome: connection.outcome
    };
  }
  render() {
    return (h(Host, { class: "elsa-flex elsa-flex-col elsa-w-full elsa-relative", ref: el => this.el = el }, this.renderCanvas()));
  }
  renderCanvas() {
    return (h("div", { class: "elsa-flex-1 elsa-flex" }, !state.useX6Graphs && (h("elsa-designer-tree", { model: this.workflowModel, class: "elsa-flex-1", ref: el => this.designer = el, mode: WorkflowDesignerMode.Blueprint })), state.useX6Graphs && (h("x6-designer", { model: this.workflowModel, class: "elsa-workflow-wrapper", ref: el => this.designer = el, mode: WorkflowDesignerMode.Blueprint })), h("elsa-flyout-panel", null, h("elsa-tab-header", { tab: "general", slot: "header" }, "General"), h("elsa-tab-content", { tab: "general", slot: "content" }, h("elsa-workflow-blueprint-properties-panel", { workflowId: this.workflowDefinitionId })))));
  }
  static get watchers() { return {
    "workflowDefinitionId": ["workflowDefinitionIdChangedHandler"],
    "serverUrl": ["serverUrlChangedHandler"]
  }; }
};
Tunnel.injectProps(ElsaWorkflowBlueprintViewerScreen, ['serverUrl', 'culture']);

export { ElsaWorkflowBlueprintViewerScreen as elsa_workflow_blueprint_viewer_screen };
