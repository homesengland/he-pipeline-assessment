import { IntellisenseContext } from "../models/elsa-interfaces";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import  { createAuth0Client,Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';


export class IntellisenseGatherer {
  private _httpClient: AxiosInstance = null;
  private _baseUrl: string = null;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  
  constructor() {

    let auth0Params: AuthorizationParams = {
      audience: '@audience'
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: '@clientId',
      domain: '@domain',
      useRefreshTokens: true
    };

    this.options = auth0Options;

    this._baseUrl = "https://localhost:7227/";
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

    // Are we in a redirect back from Auth0 receiving a code?
    const query = window.location.search;
    const hasCode = query.includes("code=");

    if (hasCode) {
      try {
        // Let auth0 SDK handle the code parsing.
        await this.auth0.handleRedirectCallback();

        // Update address to remove code query string.
        window.history.replaceState({}, document.title, "/");
        return;
      } catch (err) {
        console.log("Error parsing redirect:", err);
        return;
      }
    }

    // Redirect to Auth0 for the user to authenticate themselves.
    const origin = window.location.origin;

    const redirectOptions = {
      redirect_uri: origin
    };

    await this.auth0.loginWithRedirect(redirectOptions);
  };

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
    console.log("HttpClient", httpClient);
    const response = await httpClient.post<string>(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
    return response.data;
}
}
