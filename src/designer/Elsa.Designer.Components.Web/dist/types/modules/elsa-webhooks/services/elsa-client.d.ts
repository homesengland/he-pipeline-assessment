import { AxiosInstance } from "axios";
import { PagedList } from "../../../models";
import { WebhookDefinition, WebhookDefinitionSummary } from "../models";
export declare const createHttpClient: (baseAddress: string) => AxiosInstance;
export declare const createElsaWebhooksClient: (serverUrl: string) => ElsaWebhooksClient;
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
