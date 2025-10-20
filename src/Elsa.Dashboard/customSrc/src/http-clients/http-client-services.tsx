import { Auth0Client, Auth0ClientOptions, AuthorizationParams } from "@auth0/auth0-spa-js";
import state from "../stores/store";
import { createFetchHttpClient, HttpClient } from '../../../src/services/http/fetch-client';

export async function CreateClient(auth0: Auth0Client, serverUrl: string): Promise<HttpClient> {
  const token = await auth0.getTokenSilently();
  const httpClient = await createFetchHttpClient(serverUrl);
  httpClient.use({
    async onRequest(cfg) {
      if (token) {
        cfg.headers = { ...(cfg.headers || {}), 'Authorization': `Bearer ${token}`, 'Content-Type': 'application/json; charset=UTF-8' };
      } else {
        cfg.headers = { ...(cfg.headers || {}), 'Content-Type': 'application/json; charset=UTF-8' };
      }
      return cfg;
    }
  });
  return httpClient;
}

export function GetAuth0Options(): Auth0ClientOptions {
  const auth0Params: AuthorizationParams = {
    audience: state.audience,
  };
  return {
    authorizationParams: auth0Params,
    clientId: state.clientId,
    domain: state.domain,
    useRefreshTokens: state.useRefreshToken,
    useRefreshTokensFallback: state.useRefreshTokenFallback,
  };
}

