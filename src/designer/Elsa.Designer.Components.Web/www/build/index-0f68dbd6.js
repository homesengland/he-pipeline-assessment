var WorkflowContextFidelity;
(function (WorkflowContextFidelity) {
  WorkflowContextFidelity["Burst"] = "Burst";
  WorkflowContextFidelity["Activity"] = "Activity";
})(WorkflowContextFidelity || (WorkflowContextFidelity = {}));
var WorkflowPersistenceBehavior;
(function (WorkflowPersistenceBehavior) {
  WorkflowPersistenceBehavior["Suspended"] = "Suspended";
  WorkflowPersistenceBehavior["WorkflowBurst"] = " WorkflowBurst";
  WorkflowPersistenceBehavior["WorkflowPassCompleted"] = "WorkflowPassCompleted";
  WorkflowPersistenceBehavior["ActivityExecuted"] = "ActivityExecuted";
})(WorkflowPersistenceBehavior || (WorkflowPersistenceBehavior = {}));
var WorkflowStatus;
(function (WorkflowStatus) {
  WorkflowStatus["Idle"] = "Idle";
  WorkflowStatus["Running"] = "Running";
  WorkflowStatus["Finished"] = "Finished";
  WorkflowStatus["Suspended"] = "Suspended";
  WorkflowStatus["Faulted"] = "Faulted";
  WorkflowStatus["Cancelled"] = "Cancelled";
})(WorkflowStatus || (WorkflowStatus = {}));
var OrderBy;
(function (OrderBy) {
  OrderBy["Started"] = "Started";
  OrderBy["LastExecuted"] = "LastExecuted";
  OrderBy["Finished"] = "Finished";
})(OrderBy || (OrderBy = {}));
var ActivityTraits;
(function (ActivityTraits) {
  ActivityTraits[ActivityTraits["Action"] = 1] = "Action";
  ActivityTraits[ActivityTraits["Trigger"] = 2] = "Trigger";
  ActivityTraits[ActivityTraits["Job"] = 4] = "Job";
})(ActivityTraits || (ActivityTraits = {}));
class SyntaxNames {
}
SyntaxNames.Literal = 'Literal';
SyntaxNames.JavaScript = 'JavaScript';
SyntaxNames.Liquid = 'Liquid';
SyntaxNames.Json = 'Json';
SyntaxNames.Variable = 'Variable';
SyntaxNames.Output = 'Output';
const getVersionOptionsString = (versionOptions) => {
  if (!versionOptions)
    return '';
  return versionOptions.allVersions
    ? 'AllVersions'
    : versionOptions.isDraft
      ? 'Draft'
      : versionOptions.isLatest
        ? 'Latest'
        : versionOptions.isPublished
          ? 'Published'
          : versionOptions.isLatestOrPublished
            ? 'LatestOrPublished'
            : versionOptions.version.toString();
};
var WorkflowTestActivityMessageStatus;
(function (WorkflowTestActivityMessageStatus) {
  WorkflowTestActivityMessageStatus["Done"] = "Done";
  WorkflowTestActivityMessageStatus["Waiting"] = "Waiting";
  WorkflowTestActivityMessageStatus["Failed"] = "Failed";
  WorkflowTestActivityMessageStatus["Modified"] = "Modified";
})(WorkflowTestActivityMessageStatus || (WorkflowTestActivityMessageStatus = {}));

const EventTypes = {
  Root: {
    Initializing: 'root.initializing',
    Initialized: 'root.initialized'
  },
  ActivityEditor: {
    Show: 'show-activity-editor',
    Rendering: 'activity-editor-rendering',
    Rendered: 'activity-editor-rendered',
    Appearing: 'activity-editor-appearing',
    Disappearing: 'activity-editor-disappearing'
  },
  Dashboard: {
    Appearing: 'dashboard.appearing'
  },
  ShowActivityPicker: 'show-activity-picker',
  ShowWorkflowSettings: 'show-workflow-settings',
  ActivityPicked: 'activity-picked',
  UpdateActivity: 'update-activity',
  UpdateActivityProperties: 'update-activity-properties',
  UpdateWorkflowSettings: 'update-workflow-settings',
  WorkflowModelChanged: 'workflow-model-changed',
  ActivityDesignDisplaying: 'activity-design-displaying',
  ActivityDescriptorDisplaying: 'activity-descriptor-displaying',
  ActivityPluginUpdated: 'activity-plugin-updated',
  ActivityPluginValidating: 'activity-plugin-validating',
  WorkflowPublished: 'workflow-published',
  WorkflowRetracted: 'workflow-retracted',
  WorkflowImported: 'workflow-imported',
  WorkflowUpdated: 'workflow-updated',
  WorkflowExecuted: 'workflow-executed',
  WorkflowRestarted: 'workflow-restarted',
  HttpClientConfigCreated: 'http-client-config-created',
  HttpClientCreated: 'http-client-created',
  WorkflowInstanceBulkActionsLoading: 'workflow-instance-bulk-actions-loading',
  ShowConfirmDialog: 'show-confirm-dialog',
  HideConfirmDialog: 'hide-confirm-dialog',
  ShowModalDialog: 'show-modal-dialog',
  HideModalDialog: 'hide-modal-dialog',
  ShowToastNotification: 'show-toast-notification',
  HideToastNotification: 'hide-toast-notification',
  ConfigureFeature: 'configure-feature',
  WorkflowRegistryLoadingColumns: 'workflow-registry.loading-columns',
  WorkflowRegistryUpdating: 'workflow-registry.updating',
  WorkflowRegistryUpdated: 'workflow-registry.updated',
  ClipboardPermissionDenied: 'clipboard-permission-denied',
  ClipboardCopied: 'clipboard-copied',
  //PasteActivity: 'paste-activity',
  TestActivityMessageReceived: 'test-activity-message-received',
  FlyoutPanelTabSelected: 'flyout-panel-tab-selected',
  ComponentLoadingCustomButton: 'component-loading-custom-button',
  ComponentCustomButtonClick: 'component-custom-button-click',
  HubConnectionCreated: 'hubconnection-created',
  HubConnectionStarted: 'hubconnection-started',
  HubConnectionConnected: 'hubconnection-connected',
  HubConnectionFailed: 'hubconnection-failed',
  HubConnectionClosed: 'hubconnection-closed'
};

const FlowchartEvents = {
  ConnectionCreated: 'connection-created'
};

export { ActivityTraits as A, EventTypes as E, FlowchartEvents as F, OrderBy as O, SyntaxNames as S, WorkflowContextFidelity as W, WorkflowPersistenceBehavior as a, WorkflowStatus as b, WorkflowTestActivityMessageStatus as c, getVersionOptionsString as g };
