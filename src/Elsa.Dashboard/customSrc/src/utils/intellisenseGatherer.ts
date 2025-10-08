import { IntellisenseContext } from "../models/elsa-interfaces";
// Removed axios import, using Fetch API instead
import state from '../stores/store';
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../constants/constants";


export class IntellisenseGatherer {
  private _fetchWithAuth: ((input: string, init?: RequestInit) => Promise<Response>) | null = null;
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
      cacheLocation: "memory"
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
      state.auth0Client = this.auth0;
      return;
  };

  async getAuth0Client(): Promise<Auth0Client> {
    if (state.auth0Client == null) {
      await this.initialize();
    }
    return state.auth0Client;
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
    const fetchWithAuth = await this.createHttpClient();
    const url = `v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`;
    const response = await fetchWithAuth(url, {
      method: 'POST',
      body: JSON.stringify(context),
    });
    if (!response.ok) throw new Error('Failed to fetch type definitions');
    return await response.text();
  }

  private async createHttpClient(): Promise<(input: string, init?: RequestInit) => Promise<Response>> {
    await this.getAuth0Client();
    if (this._fetchWithAuth) {
      return this._fetchWithAuth;
    }
    const token = await this.auth0.getTokenSilently();
    const defaultHeaders: Record<string, string> = {
      'Content-Type': 'application/json; charset=UTF-8',
    };
    if (token) {
      defaultHeaders['Authorization'] = `Bearer ${token}`;
    }
    const baseUrl = this._baseUrl;
    const fetchWithAuth = (input: string, init: RequestInit = {}) => {
      const url = input.startsWith('http') ? input : `${baseUrl}${input}`;
      const headers = { ...defaultHeaders, ...(init.headers || {}) };
      return fetch(url, { ...init, headers });
    };
    this._fetchWithAuth = fetchWithAuth;
    return fetchWithAuth;
  }
}
