import { ActivityModel } from "./view";
import { ActivityDescriptor } from "./domain";
export declare const EventTypes: {
  Root: {
    Initializing: string;
    Initialized: string;
  };
  ActivityEditor: {
    Show: string;
    Rendering: string;
    Rendered: string;
    Appearing: string;
    Disappearing: string;
  };
  Dashboard: {
    Appearing: string;
  };
  ShowActivityPicker: string;
  ShowWorkflowSettings: string;
  ActivityPicked: string;
  UpdateActivity: string;
  UpdateActivityProperties: string;
  UpdateWorkflowSettings: string;
  WorkflowModelChanged: string;
  ActivityDesignDisplaying: string;
  ActivityDescriptorDisplaying: string;
  ActivityPluginUpdated: string;
  ActivityPluginValidating: string;
  WorkflowPublished: string;
  WorkflowRetracted: string;
  WorkflowImported: string;
  WorkflowUpdated: string;
  WorkflowExecuted: string;
  WorkflowRestarted: string;
  HttpClientConfigCreated: string;
  HttpClientCreated: string;
  WorkflowInstanceBulkActionsLoading: string;
  ShowConfirmDialog: string;
  HideConfirmDialog: string;
  ShowModalDialog: string;
  HideModalDialog: string;
  ShowToastNotification: string;
  HideToastNotification: string;
  ConfigureFeature: string;
  WorkflowRegistryLoadingColumns: string;
  WorkflowRegistryUpdating: string;
  WorkflowRegistryUpdated: string;
  ClipboardPermissionDenied: string;
  ClipboardCopied: string;
  TestActivityMessageReceived: string;
  FlyoutPanelTabSelected: string;
  ComponentLoadingCustomButton: string;
  ComponentCustomButtonClick: string;
  HubConnectionCreated: string;
  HubConnectionStarted: string;
  HubConnectionConnected: string;
  HubConnectionFailed: string;
  HubConnectionClosed: string;
};
export interface AddActivityEventArgs {
  sourceActivityId?: string;
}
export interface ActivityPickedEventArgs {
  activityType: string;
}
export interface ActivityDesignDisplayContext {
  activityModel: ActivityModel;
  activityDescriptor: ActivityDescriptor;
  activityIcon?: any;
  displayName?: string;
  bodyDisplay?: string;
  outcomes: Array<string>;
  expanded?: boolean;
}
export interface ActivityUpdatedContext {
  activityModel: ActivityModel;
  data?: string;
}
export interface ActivityValidatingContext {
  activityType: string;
  prop: string;
  value?: string;
  isValidated: boolean;
  data: any;
  isValid: boolean;
}
export interface ActivityDescriptorDisplayContext {
  activityDescriptor: ActivityDescriptor;
  activityIcon: any;
}
export interface ConfigureDashboardMenuContext {
  data: any;
}
export interface ConfigureWorkflowRegistryColumnsContext {
  data: any;
}
export interface ConfigureWorkflowRegistryUpdatingContext {
  params: any;
}
export interface ConfigureComponentCustomButtonContext {
  component: string;
  activityType: string;
  prop: string;
  data?: any;
}
export interface ComponentCustomButtonClickContext {
  component: string;
  activityType: string;
  prop: string;
  params: any;
}
