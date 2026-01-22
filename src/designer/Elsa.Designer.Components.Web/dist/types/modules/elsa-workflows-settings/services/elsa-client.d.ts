import { AxiosInstance } from "axios";
import { WorkflowSettings } from "../models";
export declare const createHttpClient: (baseAddress: string) => AxiosInstance;
export declare const createElsaWorkflowSettingsClient: (serverUrl: string) => Promise<ElsaWorkflowSettingsClient>;
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
