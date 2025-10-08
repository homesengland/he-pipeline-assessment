// Removed axios & axios-middleware in favor of Fetch API
import * as collection from 'lodash/collection';
import { createBaseFetch, fetchJson, sendJson, sendFormData, fetchBlob } from './fetch-client';
import {eventBus} from './event-bus';
import {
  ActivityDefinition,
  ActivityDescriptor,
  ConnectionDefinition,
  EventTypes,
  getVersionOptionsString, IntellisenseContext, ListModel,
  OrderBy,
  PagedList,
  SelectList,
  VersionOptions,
  WorkflowBlueprint,
  WorkflowBlueprintSummary,
  WorkflowContextOptions,
  WorkflowDefinition,
  WorkflowDefinitionSummary, WorkflowDefinitionVersion,
  WorkflowExecutionLogRecord,
  WorkflowInstance,
  WorkflowInstanceSummary,
  WorkflowPersistenceBehavior,
  WorkflowProviderDescriptor,
  WorkflowStatus,
  WorkflowStorageDescriptor
} from "../models";

let _fetchWithBase: ((input: string, init?: RequestInit) => Promise<Response>) | null = null;
let _elsaClient: ElsaClient = null;

export const createHttpClient = async function (baseAddress: string): Promise<(input: string, init?: RequestInit) => Promise<Response>> {
  if (_fetchWithBase)
    return _fetchWithBase;

  // Ensure the base address ends with a single trailing slash for consistent URL joining.
  const normalizedBase = baseAddress ? (baseAddress.endsWith('/') ? baseAddress : baseAddress + '/') : '/';
  const config = { baseURL: normalizedBase };
  await eventBus.emit(EventTypes.HttpClientConfigCreated, this, { config });

  const requestInterceptors: Array<(url: string, init: RequestInit) => Promise<[string, RequestInit]> | [string, RequestInit]> = [];

  const applyInterceptors = async (url: string, init: RequestInit): Promise<[string, RequestInit]> => {
    let currentUrl = url;
    let currentInit: RequestInit = init;
    for (const interceptor of requestInterceptors) {
      const result = await interceptor(currentUrl, currentInit);
      currentUrl = result[0];
      currentInit = result[1];
    }
    return [currentUrl, currentInit];
  };

  const fetchWithBase = async (input: string, init: RequestInit = {}) => {
    if (input.startsWith('http')) {
      const [outUrl, outInit] = await applyInterceptors(input, init);
      return fetch(outUrl, outInit);
    }
    const relative = input.startsWith('/') ? input.slice(1) : input;
    const url = normalizedBase + relative;
    const [outUrl, outInit] = await applyInterceptors(url, init);
    return fetch(outUrl, outInit);
  };

  const registerRequestInterceptor = (fn: (url: string, init: RequestInit) => Promise<[string, RequestInit]> | [string, RequestInit]) => {
    requestInterceptors.push(fn);
  };

  await eventBus.emit(EventTypes.HttpClientCreated, this, { fetch: fetchWithBase, registerRequestInterceptor });
  _fetchWithBase = fetchWithBase;
  return fetchWithBase;
}

export const createElsaClient = async function (serverUrl: string): Promise<ElsaClient> {

  if (!!_elsaClient)
    return _elsaClient;

  const httpClient = await createHttpClient(serverUrl);
  const getJson = <T>(url: string) => fetchJson<T>(httpClient, url);
  const sendJsonReq = <T>(method: string, url: string, body?: any) => sendJson<T>(httpClient, method, url, body);
  const postFormData = <T>(url: string, form: FormData) => sendFormData<T>(httpClient, url, form, 'POST');
  const postGetBlob = (url: string) => fetchBlob(httpClient, 'POST', url);

  _elsaClient = {
    activitiesApi: {
      list: async () => {
        return await getJson<Array<ActivityDescriptor>>('v1/activities');
      }
    },
    workflowDefinitionsApi: {
      list: async (page?: number, pageSize?: number, versionOptions?: VersionOptions, searchTerm?: string) => {
        const queryString = {
          version: getVersionOptionsString(versionOptions)
        };

        if (!!searchTerm)
          queryString['searchTerm'] = searchTerm;

        if (!!page || page === 0)
          queryString['page'] = page;

        if (!!pageSize)
          queryString['pageSize'] = pageSize;

        const queryStringItems = collection.map(queryString, (v, k) => `${k}=${v}`);
        const queryStringText = queryStringItems.length > 0 ? `?${queryStringItems.join('&')}` : '';
        return await getJson<PagedList<WorkflowDefinitionSummary>>(`v1/workflow-definitions${queryStringText}`);
      },
      getMany: async (ids: Array<string>, versionOptions?: VersionOptions) => {
        const versionOptionsString = getVersionOptionsString(versionOptions);
        const response = await getJson<ListModel<WorkflowDefinitionSummary>>(`v1/workflow-definitions?ids=${ids.join(',')}&version=${versionOptionsString}`);
        return response.items;
      },
      getVersionHistory: async (definitionId: string): Promise<Array<WorkflowDefinitionVersion>> => {
        const response = await getJson<ListModel<WorkflowDefinitionVersion>>(`v1/workflow-definitions/${definitionId}/history`);
        return response.items;
      },
      getByDefinitionAndVersion: async (definitionId: string, versionOptions: VersionOptions) => {
        const versionOptionsString = getVersionOptionsString(versionOptions);
        return await getJson<WorkflowDefinition>(`v1/workflow-definitions/${definitionId}/${versionOptionsString}`);
      },
      save: async request => {
  return await sendJsonReq<WorkflowDefinition>('POST','v1/workflow-definitions', request);
      },
      delete: async (definitionId, versionOptions?: VersionOptions) => {

        let path = `v1/workflow-definitions/${definitionId}`;

        if (!!versionOptions) {
          const versionOptionsString = getVersionOptionsString(versionOptions);
          path = `${path}/${versionOptionsString}`;
        }

        await httpClient(path, { method: 'DELETE' });
      },
      retract: async workflowDefinitionId => {
  return await sendJsonReq<WorkflowDefinition>('POST', `v1/workflow-definitions/${workflowDefinitionId}/retract`);
      },
      publish: async workflowDefinitionId => {
  return await sendJsonReq<WorkflowDefinition>('POST', `v1/workflow-definitions/${workflowDefinitionId}/publish`);
      },
      revert: async (workflowDefinitionId, version) => {
  return await sendJsonReq<WorkflowDefinition>('POST', `v1/workflow-definitions/${workflowDefinitionId}/revert/${version}`);
      },
      export: async (workflowDefinitionId, versionOptions): Promise<ExportWorkflowResponse> => {
        const versionOptionsString = getVersionOptionsString(versionOptions);
  const blob = await postGetBlob(`v1/workflow-definitions/${workflowDefinitionId}/${versionOptionsString}/export`);
        // Fetch lower-cases headers via get() API
        // Content-Disposition may not be exposed unless configured on CORS
        const resp = new Response(blob); // placeholder to reuse logic if needed
        const contentDispositionHeader = resp.headers.get('content-disposition');
        const fileName = contentDispositionHeader ? contentDispositionHeader.split(';')[1].split('=')[1] : `workflow-definition-${workflowDefinitionId}.json`;
        const data = blob;

        return {
          fileName: fileName,
          data: data
        };
      },
      import: async (workflowDefinitionId, file: File): Promise<WorkflowDefinition> => {
        const formData = new FormData();
        formData.append("file", file);
  return await postFormData<WorkflowDefinition>(`v1/workflow-definitions/${workflowDefinitionId}/import`, formData);
      },
      restore: async (file: File): Promise<void> => {
        const formData = new FormData();
        formData.append("file", file);
        await httpClient(`v1/workflow-definitions/restore`, { method: 'POST', body: formData });
      }
    },
    workflowTestApi: {
      execute: async (request) => {
  return await sendJsonReq<WorkflowTestExecuteResponse>('POST', `v1/workflow-test/execute`, request);
      },
      restartFromActivity: async (request) => {
  await sendJsonReq<void>('POST', `v1/workflow-test/restartFromActivity`, request);
      },
      stop: async (request) => {
  await sendJsonReq<void>('POST', `v1/workflow-test/stop`, request);
      }
    },
    workflowRegistryApi: {
      list: async (providerName: string, page?: number, pageSize?: number, versionOptions?: VersionOptions): Promise<PagedList<WorkflowBlueprintSummary>> => {
        const queryString = {
          version: getVersionOptionsString(versionOptions)
        };

        if (!!page || page === 0)
          queryString['page'] = page;

        if (!!pageSize)
          queryString['pageSize'] = pageSize;

        const queryStringItems = collection.map(queryString, (v, k) => `${k}=${v}`);
        const queryStringText = queryStringItems.length > 0 ? `?${queryStringItems.join('&')}` : '';
        return await getJson<PagedList<WorkflowBlueprintSummary>>(`v1/workflow-registry/by-provider/${providerName}${queryStringText}`);
      },
      listAll: async (versionOptions?: VersionOptions): Promise<Array<WorkflowBlueprintSummary>> => {
        const queryString = {
          version: getVersionOptionsString(versionOptions)
        };

        const queryStringItems = collection.map(queryString, (v, k) => `${k}=${v}`);
        const queryStringText = queryStringItems.length > 0 ? `?${queryStringItems.join('&')}` : '';
        return await getJson<Array<WorkflowBlueprintSummary>>(`v1/workflow-registry${queryStringText}`);
      },
      findManyByDefinitionVersionIds: async (definitionVersionIds: Array<string>): Promise<Array<WorkflowBlueprintSummary>> => {

        if (definitionVersionIds.length == 0)
          return [];

        const idsQuery = definitionVersionIds.join(",")
        return await getJson<Array<WorkflowBlueprintSummary>>(`v1/workflow-registry/by-definition-version-ids?ids=${idsQuery}`);
      },

      get: async (id: string, versionOptions: VersionOptions) => {
        const versionOptionsString = getVersionOptionsString(versionOptions);
        return await getJson<WorkflowBlueprint>(`v1/workflow-registry/${id}/${versionOptionsString}`);
      }
    },
    workflowInstancesApi: {
      list: async (page?: number, pageSize?: number, workflowDefinitionId?: string, workflowStatus?: WorkflowStatus, orderBy?: OrderBy, searchTerm?: string, correlationId?: string): Promise<PagedList<WorkflowInstanceSummary>> => {
        const queryString = {};

        if (!!workflowDefinitionId)
          queryString['workflow'] = workflowDefinitionId;

        if (!!correlationId)
          queryString['correlationId'] = correlationId;

        if (workflowStatus != null)
          queryString['status'] = workflowStatus;

        if (!!orderBy)
          queryString['orderBy'] = orderBy;

        if (!!searchTerm)
          queryString['searchTerm'] = searchTerm;

        if (!!page)
          queryString['page'] = page;

        if (!!pageSize)
          queryString['pageSize'] = pageSize;

        const queryStringItems = collection.map(queryString, (v, k) => `${k}=${v}`);
        const queryStringText = queryStringItems.length > 0 ? `?${queryStringItems.join('&')}` : '';
        return await getJson<PagedList<WorkflowInstanceSummary>>(`v1/workflow-instances${queryStringText}`);
      },
      get: async id => {
        return await getJson<WorkflowInstance>(`v1/workflow-instances/${id}`);
      },
      cancel: async id => {
  await sendJsonReq<void>('POST', `v1/workflow-instances/${id}/cancel`);
      },
      delete: async id => {
        await httpClient(`v1/workflow-instances/${id}`, { method: 'DELETE' });
      },
      retry: async id => {
  await sendJsonReq<void>('POST', `v1/workflow-instances/${id}/retry`, { runImmediately: false });
      },
      bulkCancel: async request => {
  return await sendJsonReq<any>('POST', `v1/workflow-instances/bulk/cancel`, request);
      },
      bulkDelete: async request => {
        // Emulate axios delete with body (not standard, but some servers allow). Using fetch with method DELETE and JSON body.
        const resp = await httpClient(`v1/workflow-instances/bulk`, { method: 'DELETE', headers: { 'Content-Type': 'application/json' }, body: JSON.stringify(request) });
        if(!resp.ok) throw new Error('DELETE bulk workflow-instances failed');
        return await resp.json();
      },
      bulkRetry: async request => {
  return await sendJsonReq<any>('POST', `v1/workflow-instances/bulk/retry`, request);
      }
    },
    workflowExecutionLogApi: {
      get: async (workflowInstanceId: string, page?: number, pageSize?: number): Promise<PagedList<WorkflowExecutionLogRecord>> => {
        const queryString = {};

        if (!!page)
          queryString['page'] = page;

        if (!!pageSize)
          queryString['pageSize'] = pageSize;

        const queryStringItems = collection.map(queryString, (v, k) => `${k}=${v}`);
        const queryStringText = queryStringItems.length > 0 ? `?${queryStringItems.join('&')}` : '';
        return await getJson<PagedList<WorkflowExecutionLogRecord>>(`v1/workflow-instances/${workflowInstanceId}/execution-log${queryStringText}`);
      }
    },
    scriptingApi: {
      getJavaScriptTypeDefinitions: async (workflowDefinitionId: string, context?: IntellisenseContext): Promise<string> => {
  return await sendJsonReq<string>('POST', `v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
      }
    },
    designerApi: {
      runtimeSelectItemsApi: {
        get: async (providerTypeName: string, context?: any): Promise<SelectList> => {
          return await sendJsonReq<SelectList>('POST', 'v1/designer/runtime-select-list', {
            providerTypeName,
            context
          });
        }
      }
    },
    activityStatsApi: {
      get: async (workflowInstanceId: string, activityId?: any): Promise<ActivityStats> => {
        return await getJson<ActivityStats>(`v1/workflow-instances/${workflowInstanceId}/activity-stats/${activityId}`);
      }
    },
    workflowStorageProvidersApi: {
      list: async () => {
        return await getJson<Array<WorkflowStorageDescriptor>>('v1/workflow-storage-providers');
      }
    },
    workflowProvidersApi: {
      list: async () => {
        return await getJson<Array<WorkflowProviderDescriptor>>('v1/workflow-providers');
      }
    },
    workflowChannelsApi: {
      list: async () => {
        return await getJson<Array<string>>('v1/workflow-channels');
      }
    },
    featuresApi: {
      list: async () => {
        const response = await getJson<FeaturesModel>('v1/features');
        return response.features;
      }
    },
    versionApi: {
      get: async () => {
        const response = await getJson<VersionModel>('v1/version');
        return response.version;
      }
    },
    authenticationApi:{
      getUserDetails: async () => {
        const resp = await httpClient('v1/elsaAuthentication/userinfo');
        if(!resp.ok) return null;
        const contentType = resp.headers.get('content-type');
        if(contentType === 'text/html; charset=utf-8') return null;
        const data: UserDetail = await resp.json();
        return data.isAuthenticated ? data : null;
      },
      getAuthenticationConfguration: async () => {
        return await getJson<AuthenticationConfguration>('v1/ElsaAuthentication/options');
      }
    }
  }

  return _elsaClient;
}

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
  authenticationApi : AuthenticationApi;
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
  getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string>
}

export interface DesignerApi {
  runtimeSelectItemsApi: RuntimeSelectItemsApi;
}

export interface RuntimeSelectItemsApi {
  get(providerTypeName: string, context?: any): Promise<SelectList>
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
  workflowDefinitionId?: string,
  version?: number,
  signalRConnectionId?: string
  startActivityId?: string;
}

export interface WorkflowTestRestartFromActivityRequest {
  workflowDefinitionId: string,
  version: number,
  activityId: string,
  lastWorkflowInstanceId: string,
  signalRConnectionId: string
}

export interface WorkflowTestStopRequest {
  workflowInstanceId: string
}

export interface WorkflowTestExecuteResponse {
  isSuccess: boolean,
  isAnotherInstanceRunning: boolean
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
  tenantId : string;
  isAuthenticated : boolean;
}

export interface AuthenticationConfguration  {
  authenticationStyles: string[];
  currentTenantAccessorName : string;
  tenantAccessorKeyName:string;
}



interface ActivityEventCount {
  eventName: string;
  count: number;
}

interface ActivityFault {
  message: string;
}

interface FeaturesModel {
  features: Array<string>;
}

interface VersionModel {
  version: string;
}
