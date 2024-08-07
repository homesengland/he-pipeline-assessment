import { IntellisenseContext } from "../models/elsa-interfaces";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import state from '../stores/store';
import { StoreStatus } from "../constants/constants";
import { Auth0ClientInitializer }  from "./auth0ClientInitializer"
import { Auth0Client } from "@auth0/auth0-spa-js";


export class IntellisenseGatherer {
  private _auth0ClientGatherer: Auth0ClientInitializer = new Auth0ClientInitializer
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;
  private context: IntellisenseContext = {
      activityTypeName: "",
      propertyName: ""
  };

  constructor() {
    this._baseUrl = state.serverUrl;
  }

  async getIntellisense(): Promise<string> {
    if (state.javaScriptTypeDefinitionsFetchStatus == StoreStatus.Available && this.hasDefinitions) {
        return state.javaScriptTypeDefinitions;
    }
    else {
      this.tryFetchDefinitions();
      return state.javaScriptTypeDefinitions;
    }
  }

  private async tryFetchDefinitions() {
    state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Fetching;
    let definitions = await this.getJavaScriptTypeDefinitions(state.workflowDefinitionId, this.context);
    state.javaScriptTypeDefinitions = definitions;
    if (state.javaScriptTypeDefinitions != null) {
      this.appendGatheredValues();
      state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Available;
    }
    else {
      state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
      state.javaScriptTypeDefinitions = '';
    }
    return state.javaScriptTypeDefinitions;
  }

  hasDefinitions() {
    return state.javaScriptTypeDefinitions != null && state.javaScriptTypeDefinitions.trim().length > 0;
  }

  private appendGatheredValues() {
    let intellisense = state.javaScriptTypeDefinitions + state.dataDictionaryIntellisense;
    state.javaScriptTypeDefinitions = intellisense;
  }


  private async getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string> {
    let httpClient = await this.createHttpClient();
    const response = await httpClient.post<string>(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
    return response.data;
  }

  private async createHttpClient(): Promise<AxiosInstance> {


    if (!!this._httpClient) {
      return this._httpClient;
    }
    let auth0: Auth0Client = await this._auth0ClientGatherer.getClient();
    const token = await auth0.getTokenSilently();

    let config: AxiosRequestConfig;
    if (!!token) {
      console.log("Intellisense Token", token);
      config = {
        baseURL: this._baseUrl,
        headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` }
      };
    } else {
      config = {
        baseURL: this._baseUrl,
        headers: { 'Content-Type': `application/json; charset=UTF-8` },
      };
    }
    this._httpClient = axios.create(config);

    return this._httpClient;
  }
}
