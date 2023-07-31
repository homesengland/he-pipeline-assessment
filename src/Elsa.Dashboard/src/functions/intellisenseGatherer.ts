import { IntellisenseContext } from "../models/elsa-interfaces";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;

  private constructor(serverUrl: string) {
    this._baseUrl = serverUrl;
  }

  private async createHttpClient(): Promise<AxiosInstance> {
    if (!!this._httpClient)
      return this._httpClient;

    const config: AxiosRequestConfig = {
      baseURL: this._baseUrl
    };
    this._httpClient = axios.create(config);

    return this._httpClient;
  }


  async getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string> {
    let httpClient = await this.createHttpClient();
    const response = await httpClient.post<string>(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
    return response.data;
}
}
