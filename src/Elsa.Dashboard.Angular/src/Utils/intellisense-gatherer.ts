import { WorkflowDefinition } from './../models/domain';
import { IntellisenseContext } from '../models/intellisense';
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
//import state from '../stores/store';
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../models/constants";
import { StoreConfig } from '../models/store-config'
import { selectDataDictionaryIntellisense, selectJavaScriptTypeDefinitions, selectJavaScriptTypeDefinitionsFetchStatus, selectStoreConfig, selectWorkflowDefinitionId } from '../components/state/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { AppStateActionGroup } from '../components/state/actions/app.state.actions';
import { Observable } from 'rxjs';


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance;
  private _baseUrl: string = "";
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private context: IntellisenseContext = {
    activityTypeName: "",
    propertyName: ""
  };
  private storeConfig: StoreConfig;
  private workflowDefinitionId: string;
  private javaScriptTypeDefinitionsFetchStatus = "";
  private javaScriptTypeDefinitions = "";
  private dataDictionaryIntellisense = "";


  constructor(private store: Store) {

    this.store.select(selectStoreConfig).subscribe(data => {
      this.storeConfig = data;
    });
    this.store.select(selectWorkflowDefinitionId).subscribe(data => {
      this.workflowDefinitionId = data;
    })
    this.store.select(selectJavaScriptTypeDefinitionsFetchStatus).subscribe(data => {
      this.javaScriptTypeDefinitionsFetchStatus = data;
    })
    this.store.select(selectJavaScriptTypeDefinitions).subscribe(data => {
      this.javaScriptTypeDefinitions = data;
    })
    this.store.select(selectDataDictionaryIntellisense).subscribe(data => {
      this.dataDictionaryIntellisense = data;
    })

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
    if (isAuthenticated) {
      var storeconfig = JSON.parse(JSON.stringify(this.storeConfig))
      storeconfig.auth0Client = this.auth0;

      this.store.dispatch(AppStateActionGroup.setStoreConfig({
        storeConfig: storeconfig
      }));
    }
    return;
  };

  async getAuth0Client(): Promise<Auth0Client> {
    if (this.storeConfig.auth0Client == null) {
      await this.initialize();
    }
    return this.storeConfig.auth0Client;
  }


  async getIntellisense(): Promise<string> {
    if (this.javaScriptTypeDefinitionsFetchStatus == StoreStatus.Available && this.hasDefinitions()) {
      return this.javaScriptTypeDefinitions;
    }
    else {
      this.tryFetchDefinitions();
      return this.javaScriptTypeDefinitions;
    }
  }

  private async tryFetchDefinitions() {
    this.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Fetching;
    let definitions = await this.getJavaScriptTypeDefinitions(this.context);
    this.javaScriptTypeDefinitions = definitions;
    if (this.javaScriptTypeDefinitions != null) {
      this.appendGatheredValues();
      this.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Available;
    }
    else {
      this.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
      this.javaScriptTypeDefinitions = '';
    }
    return this.javaScriptTypeDefinitions;
  }

  hasDefinitions() {
    return this.javaScriptTypeDefinitions != null && this.javaScriptTypeDefinitions.trim().length > 0;
  }

  private appendGatheredValues() {
    let intellisense = this.javaScriptTypeDefinitions + this.dataDictionaryIntellisense;
    this.javaScriptTypeDefinitions = intellisense;
  }


  private async getJavaScriptTypeDefinitions(context?: IntellisenseContext): Promise<string> {

    var workflowDefinitionId = await selectWorkflowDefinitionId(this.store);
    console.log("workflowDefinitionId", workflowDefinitionId);
    let httpClient = await this.createHttpClient();
    var url = `v1/scripting/javascript/type-definitions/${workflowDefinitionId}`;
    console.log("Url", url);
    const response = await httpClient.post<string>(url, context);
    return response.data;
  }

  private async createHttpClient(): Promise<AxiosInstance> {

    var client = await this.getAuth0Client();

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
