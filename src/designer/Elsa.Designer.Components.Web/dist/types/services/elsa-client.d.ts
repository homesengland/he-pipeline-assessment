import { AxiosInstance } from "axios";
import { ActivityDefinition, ActivityDescriptor, ConnectionDefinition, IntellisenseContext, OrderBy, PagedList, SelectList, VersionOptions, WorkflowBlueprint, WorkflowBlueprintSummary, WorkflowContextOptions, WorkflowDefinition, WorkflowDefinitionSummary, WorkflowDefinitionVersion, WorkflowExecutionLogRecord, WorkflowInstance, WorkflowInstanceSummary, WorkflowPersistenceBehavior, WorkflowProviderDescriptor, WorkflowStatus, WorkflowStorageDescriptor } from "../models";
export declare const createHttpClient: (baseAddress: string) => Promise<AxiosInstance>;
export declare const createElsaClient: (serverUrl: string) => Promise<ElsaClient>;
export interface ElsaClient {
  activitiesApi: ActivitiesApi;
  workflowDefinitionsApi: WorkflowDefinitionsApi;
  workflowRegistryApi: WorkflowRegistryApi;
  workflowInstancesApi: WorkflowInstancesApi;
  workflowExecutionLogApi: WorkflowExecutionLogApi;
  scriptingApi: ScriptingApi;
  designerApi: DesignerApi;
  activityStatsApi: ActivityStatsApi;
  workflowStorageProvidersApi: WorkflowStorageProvidersApi;
  workflowProvidersApi: WorkflowProvidersApi;
  workflowChannelsApi: WorkflowChannelsApi;
  workflowTestApi: WorkflowTestApi;
  featuresApi: FeaturesApi;
  versionApi: VersionApi;
  authenticationApi: AuthenticationApi;
}
export interface ActivitiesApi {
  list(): Promise<Array<ActivityDescriptor>>;
}
export interface AuthenticationApi {
  getUserDetails(): Promise<UserDetail>;
  getAuthenticationConfguration(): Promise<AuthenticationConfguration>;
}
export interface FeaturesApi {
  list(): Promise<Array<string>>;
}
export interface VersionApi {
  get(): Promise<string>;
}
export interface WorkflowDefinitionsApi {
  list(page?: number, pageSize?: number, versionOptions?: VersionOptions, searchTerm?: string): Promise<PagedList<WorkflowDefinitionSummary>>;
  getMany(ids: Array<string>, versionOptions?: VersionOptions): Promise<Array<WorkflowDefinitionSummary>>;
  getVersionHistory(id: string): Promise<Array<WorkflowDefinitionVersion>>;
  getByDefinitionAndVersion(definitionId: string, versionOptions: VersionOptions): Promise<WorkflowDefinition>;
  save(request: SaveWorkflowDefinitionRequest): Promise<WorkflowDefinition>;
  delete(definitionId: string, versionOptions: VersionOptions): Promise<void>;
  retract(workflowDefinitionId: string): Promise<WorkflowDefinition>;
  publish(workflowDefinitionId: string): Promise<WorkflowDefinition>;
  revert(workflowDefinitionId: string, version: number): Promise<WorkflowDefinition>;
  export(workflowDefinitionId: string, versionOptions: VersionOptions): Promise<ExportWorkflowResponse>;
  import(workflowDefinitionId: string, file: File): Promise<WorkflowDefinition>;
  restore(file: File): Promise<void>;
}
export interface WorkflowTestApi {
  execute(request: WorkflowTestExecuteRequest): Promise<WorkflowTestExecuteResponse>;
  restartFromActivity(request: WorkflowTestRestartFromActivityRequest): Promise<void>;
  stop(request: WorkflowTestStopRequest): Promise<void>;
}
export interface WorkflowRegistryApi {
  list(providerName: string, page?: number, pageSize?: number, versionOptions?: VersionOptions): Promise<PagedList<WorkflowBlueprintSummary>>;
  listAll(versionOptions?: VersionOptions): Promise<Array<WorkflowBlueprintSummary>>;
  findManyByDefinitionVersionIds(definitionVersionIds: Array<string>): Promise<Array<WorkflowBlueprintSummary>>;
  get(id: string, versionOptions: VersionOptions): Promise<WorkflowBlueprint>;
}
export interface WorkflowInstancesApi {
  list(page?: number, pageSize?: number, workflowDefinitionId?: string, workflowStatus?: WorkflowStatus, orderBy?: OrderBy, searchTerm?: string, correlationId?: string): Promise<PagedList<WorkflowInstanceSummary>>;
  get(id: string): Promise<WorkflowInstance>;
  cancel(id: string): Promise<void>;
  delete(id: string): Promise<void>;
  retry(id: string): Promise<void>;
  bulkCancel(request: BulkCancelWorkflowsRequest): Promise<BulkCancelWorkflowsResponse>;
  bulkDelete(request: BulkDeleteWorkflowsRequest): Promise<BulkDeleteWorkflowsResponse>;
  bulkRetry(request: BulkRetryWorkflowsRequest): Promise<BulkRetryWorkflowsResponse>;
}
export interface WorkflowExecutionLogApi {
  get(workflowInstanceId: string, page?: number, pageSize?: number): Promise<PagedList<WorkflowExecutionLogRecord>>;
}
export interface BulkCancelWorkflowsRequest {
  workflowInstanceIds: Array<string>;
}
export interface BulkCancelWorkflowsResponse {
  cancelledWorkflowCount: number;
}
export interface BulkDeleteWorkflowsRequest {
  workflowInstanceIds: Array<string>;
}
export interface BulkDeleteWorkflowsResponse {
  deletedWorkflowCount: number;
}
export interface BulkRetryWorkflowsRequest {
  workflowInstanceIds: Array<string>;
}
export interface BulkRetryWorkflowsResponse {
  retriedWorkflowCount: number;
}
export interface ScriptingApi {
  getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string>;
}
export interface DesignerApi {
  runtimeSelectItemsApi: RuntimeSelectItemsApi;
}
export interface RuntimeSelectItemsApi {
  get(providerTypeName: string, context?: any): Promise<SelectList>;
}
export interface ActivityStatsApi {
  get(workflowInstanceId: string, activityId: string): Promise<ActivityStats>;
}
export interface WorkflowStorageProvidersApi {
  list(): Promise<Array<WorkflowStorageDescriptor>>;
}
export interface WorkflowProvidersApi {
  list(): Promise<Array<WorkflowProviderDescriptor>>;
}
export interface WorkflowChannelsApi {
  list(): Promise<Array<string>>;
}
export interface SaveWorkflowDefinitionRequest {
  workflowDefinitionId?: string;
  name?: string;
  displayName?: string;
  description?: string;
  tag?: string;
  channel?: string;
  variables?: string;
  contextOptions?: WorkflowContextOptions;
  isSingleton?: boolean;
  persistenceBehavior?: WorkflowPersistenceBehavior;
  deleteCompletedInstances?: boolean;
  publish?: boolean;
  activities: Array<ActivityDefinition>;
  connections: Array<ConnectionDefinition>;
}
export interface WorkflowTestExecuteRequest {
  workflowDefinitionId?: string;
  version?: number;
  signalRConnectionId?: string;
  startActivityId?: string;
}
export interface WorkflowTestRestartFromActivityRequest {
  workflowDefinitionId: string;
  version: number;
  activityId: string;
  lastWorkflowInstanceId: string;
  signalRConnectionId: string;
}
export interface WorkflowTestStopRequest {
  workflowInstanceId: string;
}
export interface WorkflowTestExecuteResponse {
  isSuccess: boolean;
  isAnotherInstanceRunning: boolean;
}
export interface ExportWorkflowResponse {
  fileName: string;
  data: Blob;
}
export interface ActivityStats {
  fault?: ActivityFault;
  averageExecutionTime: string;
  fastestExecutionTime: string;
  slowestExecutionTime: string;
  lastExecutedAt: Date;
  eventCounts: Array<ActivityEventCount>;
}
export interface UserDetail {
  name: string;
  tenantId: string;
  isAuthenticated: boolean;
}
export interface AuthenticationConfguration {
  authenticationStyles: string[];
  currentTenantAccessorName: string;
  tenantAccessorKeyName: string;
}
interface ActivityEventCount {
  eventName: string;
  count: number;
}
interface ActivityFault {
  message: string;
}
export {};
