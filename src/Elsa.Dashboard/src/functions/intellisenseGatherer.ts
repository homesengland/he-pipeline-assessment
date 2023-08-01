import { IntellisenseContext } from "../models/elsa-interfaces";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;

  constructor(url: string, domain: string, audience: string, clientId: string, useRefreshTokens: boolean) {

    let auth0Params: AuthorizationParams = {
      audience: audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: clientId,
      domain: domain,
      useRefreshTokens: useRefreshTokens
    };

    this.options = auth0Options;

    this._baseUrl = url;
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

  private async createHttpClient(): Promise<AxiosInstance> {

    await this.initialize();

    if (!!this._httpClient) {
      return this._httpClient;
    }


    console.log("Auth0 Client", this.auth0);

    const token = await this.auth0.getTokenSilently();

    let config: AxiosRequestConfig;
    if (!!token) {
      config = {
        baseURL: this._baseUrl,
        headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` },
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


  async getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string> {
    let httpClient = await this.createHttpClient();
    console.log("HttpClient", httpClient);
    const response = await httpClient.post<string>(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
    return response.data;
  }
}
