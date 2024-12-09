import { Auth0Client, Auth0ClientOptions, AuthorizationParams } from "@auth0/auth0-spa-js";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import state from "../stores/store";

export async function CreateClient(auth0: Auth0Client, serverUrl: string): Promise<AxiosInstance> {

  const token = await auth0.getTokenSilently();
  
  let config: AxiosRequestConfig;

  if (!!token) {
    config = {
      baseURL: serverUrl,
      headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` }};
  } else
  {
    config = {
      baseURL: serverUrl,
      headers: { 'Content-Type': `application/json; charset=UTF-8` },
   };
  }
    const httpClient = axios.create(config);

    return httpClient;
}

export function GetAuth0Options(): Auth0ClientOptions {
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
  return auth0Options;
}

