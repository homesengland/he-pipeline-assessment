import { PropertyDisplayManager } from "../services/property-display-manager";
import { PluginManager } from "../services/property-display-manager";
import { ConfirmDialogService } from "../services/property-display-manager";
import { ActivityIconProvider } from "../services/property-display-manager";
import { ToastNotificationService } from "../services/toast-notification-service";
import { WorkflowClient } from '../services/workflow-client';
import EventBus from "../services/custom-event-bus";
import { AxiosInstance } from "axios";
import { ActivityDefinitionProperty } from "./domain";
import { ActivityModel } from "./view";

export interface WorkflowStudio {
  serverUrl: string;
  basePath: string;
  features: any;
  serverFeatures: Array<string>;
  serverVersion: string;
  pluginManager: PluginManager;
  propertyDisplayManager: PropertyDisplayManager;
  workflowClientFactory: () => Promise<WorkflowClient>;
  httpClientFactory: () => Promise<AxiosInstance>;
  eventBus: EventBus;
  activityIconProvider: ActivityIconProvider;
  confirmDialogService: ConfirmDialogService;
  toastNotificationService: ToastNotificationService;
  getOrCreateProperty: (activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string) => ActivityDefinitionProperty;
  htmlToElement: (html: string) => Element;
}

//import { PluginManager, ActivityIconProvider, ConfirmDialogService, PropertyDisplayManager } from "../services";
//import { ElsaClient, ToastNotificationService } from "../services";
//import EventBus from "../services/custom-event-bus";
//import { AxiosInstance } from "axios";
//import { ActivityDefinitionProperty } from "./domain";
//import { ActivityModel } from "./view";

//export interface Studio {
//  serverUrl: string;
//  basePath: string;
//  features: any;
//  serverFeatures: Array<string>;
//  serverVersion: string;
//  pluginManager: PluginManager;
//  propertyDisplayManager: PropertyDisplayManager;
//  elsaClientFactory: () => Promise<ElsaClient>;
//  httpClientFactory: () => Promise<AxiosInstance>;
//  eventBus: EventBus;
//  activityIconProvider: ActivityIconProvider;
//  confirmDialogService: ConfirmDialogService;
//  toastNotificationService: ToastNotificationService;
//  getOrCreateProperty: (activity: ActivityModel, name: string, defaultExpression?: () => string, defaultSyntax?: () => string) => ActivityDefinitionProperty;
//  htmlToElement: (html: string) => Element;
//}
