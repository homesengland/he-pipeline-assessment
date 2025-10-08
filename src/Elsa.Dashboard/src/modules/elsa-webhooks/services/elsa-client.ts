// Removed axios in favor of Fetch API
import * as collection from 'lodash/collection';
import {eventBus} from '../../../services/event-bus';
import { createBaseFetch, fetchJson, sendJson } from '../../../services/fetch-client';
import {EventTypes, PagedList} from "../../../models";
import {WebhookDefinition, WebhookDefinitionSummary} from "../models";

let _fetchWithBase: ((input: string, init?: RequestInit) => Promise<Response>) | null = null;
let _elsaWebhooksClient: ElsaWebhooksClient = null;

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

export const createElsaWebhooksClient = function (serverUrl: string): ElsaWebhooksClient {

  if (!!_elsaWebhooksClient)
    return _elsaWebhooksClient;

  const httpClient = createHttpClient(serverUrl);

  _elsaWebhooksClient = {
    webhookDefinitionsApi: {
      list: async (page?: number, pageSize?: number) => {
        return await fetchJson<PagedList<WebhookDefinitionSummary>>(httpClient, `v1/webhook-definitions`);
      },
      getByWebhookId: async (webhookId: string) => {
        return await fetchJson<WebhookDefinition>(httpClient, `v1/webhook-definitions/${webhookId}`);
      },
      save: async request => {
        return await sendJson<WebhookDefinition>(httpClient, 'POST', 'v1/webhook-definitions', request);
      },
      update: async request => {
        return await sendJson<WebhookDefinition>(httpClient, 'PUT', 'v1/webhook-definitions', request);
      },
      delete: async webhookId => {
        const resp = await httpClient(`v1/webhook-definitions/${webhookId}`, { method: 'DELETE' });
        if(!resp.ok) throw new Error('Failed to delete webhook definition');
      },
    }
  }

  return _elsaWebhooksClient;
}

export interface ElsaWebhooksClient {
  webhookDefinitionsApi: WebhookDefinitionsApi;
}

export interface WebhookDefinitionsApi {

  list(page?: number, pageSize?: number): Promise<PagedList<WebhookDefinitionSummary>>;

  getByWebhookId(webhookId: string): Promise<WebhookDefinition>;

  save(request: SaveWebhookDefinitionRequest): Promise<WebhookDefinition>;

  update(request: SaveWebhookDefinitionRequest): Promise<WebhookDefinition>;

  delete(webhookId: string): Promise<void>;
}

export interface SaveWebhookDefinitionRequest {
  id?: string;
  name?: string;
  path?: string;
  description?: string;
  payloadTypeName?: string;
  isEnabled?: boolean;
}
