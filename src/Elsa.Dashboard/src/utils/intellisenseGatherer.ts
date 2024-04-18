import { IntellisenseContext } from "../models/elsa-interfaces";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import state from '../stores/store';
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../constants/constants";


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private context: IntellisenseContext = {
      activityTypeName: "",
      propertyName: ""
  };

  constructor() {

    let auth0Params: AuthorizationParams = {
      audience: state.audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: state.clientId,
      domain: state.domain,
      useRefreshTokens: state.useRefreshToken,
      useRefreshTokensFallback: state.useRefreshTokenFallback,
    };

    this.options = auth0Options;

    this._baseUrl = state.serverUrl;
  }

  initialize = async () => {
    const options = this.options;
    const { domain } = options;

    if (!domain || domain.trim().length == 0)
      return;

    this.auth0 = await createAuth0Client(this.options);
    const isAuthenticated = await this.auth0.isAuthenticated();

    // Nothing to do if authenticated.
    if (isAuthenticated)
      return;
  };



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

    await this.initialize();

    if (!!this._httpClient) {
      return this._httpClient;
    }
    const token = await this.auth0.getTokenSilently();

    let config: AxiosRequestConfig;
    if (!!token) {
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
