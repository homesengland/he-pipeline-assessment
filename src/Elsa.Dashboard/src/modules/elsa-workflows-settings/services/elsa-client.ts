// Removed axios in favor of Fetch API
import * as collection from 'lodash/collection';
import {eventBus} from '../../../services/event-bus';
import { createBaseFetch, fetchJson, sendJson } from '../../../services/fetch-client';
import {EventTypes} from "../../../models";
import {WorkflowSettings} from "../models";


let _fetchWithBase: ((input: string, init?: RequestInit) => Promise<Response>) | null = null;
let _elsaWorkflowSettingsClient: ElsaWorkflowSettingsClient = null;

export const createHttpClient = function(baseAddress: string)  {
  if(_fetchWithBase)
    return _fetchWithBase;
  const config = { baseURL: baseAddress };
  eventBus.emit(EventTypes.HttpClientConfigCreated, this, { config });
  const fetchWithBase = createBaseFetch(baseAddress);
  eventBus.emit(EventTypes.HttpClientCreated, this, { fetch: fetchWithBase });
  _fetchWithBase = fetchWithBase;
  return fetchWithBase;
}

export const createElsaWorkflowSettingsClient = async function (serverUrl: string): Promise<ElsaWorkflowSettingsClient> {

  if (!!_elsaWorkflowSettingsClient)
    return _elsaWorkflowSettingsClient;

  const httpClient = await createHttpClient(serverUrl);

  _elsaWorkflowSettingsClient = {
    workflowSettingsApi: {
      list: async () => {
        return await fetchJson<Array<WorkflowSettings>>(httpClient, `v1/workflow-settings`);
      },
      save: async request => {
        return await sendJson<WorkflowSettings>(httpClient, 'POST', 'v1/workflow-settings', request);
      },
      delete: async id => {
        const resp = await httpClient(`v1/workflow-settings/${id}`, { method: 'DELETE' });
        if(!resp.ok) throw new Error('Failed to delete workflow settings');
      },
    }
  }

  return _elsaWorkflowSettingsClient;
}

export interface ElsaWorkflowSettingsClient {
  workflowSettingsApi: WorkflowSettingsApi;
}

export interface WorkflowSettingsApi {

  list(): Promise<Array<WorkflowSettings>>;

  save(request: SaveWorkflowSettingsRequest): Promise<WorkflowSettings>;

  delete(workflowSettingsId: string): Promise<void>;
}

export interface SaveWorkflowSettingsRequest {
  id?: string;
  workflowBlueprintId?: string;
  key?: string;
  value?: string;
}
