import { IntellisenseContext } from '../Models/elsa-interfaces'
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
//import state from '../stores/store';
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../constants/constants";
import { StoreConfig } from '../Models/storeConfig'
import { selectStoreConfig } from '../components/state/selectors/app.state.selectors';
import { Store } from '@ngrx/store';


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private context: IntellisenseContext = {
      activityTypeName: "",
      propertyName: ""
  };
  private storeConfig: StoreConfig;


  constructor(private store: Store) {

    this.store.select(selectStoreConfig).subscribe(data => {
      this.storeConfig = data;
    });

    let auth0Params: AuthorizationParams = {
      audience: this.storeConfig.audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: this.storeConfig.clientId,
      domain: this.storeConfig.domain,
      useRefreshTokens: this.storeConfig.useRefreshTokens,
      useRefreshTokensFallback: this.storeConfig.useRefreshTokensFallback,
      cacheLocation: "memory"
    };

    this.options = auth0Options;

      this._baseUrl = this.storeConfig.serverUrl;
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
    let definitions = await this.getJavaScriptTypeDefinitions(this.storeConfig.workflowDefinitionId, this.context);
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

    await this.getAuth0Client();

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
