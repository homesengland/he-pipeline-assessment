import { Auth0Client, Auth0ClientOptions, AuthorizationParams } from "@auth0/auth0-spa-js";
// Removed axios import, using Fetch API instead
import state from "../stores/store";


/**
 * Returns a fetch wrapper that automatically adds auth headers and base URL.
 */
export async function CreateClient(auth0: Auth0Client, serverUrl: string) {
  const token = await auth0.getTokenSilently();
  const defaultHeaders: Record<string, string> = {
    'Content-Type': 'application/json; charset=UTF-8',
  };
  if (token) {
    defaultHeaders['Authorization'] = `Bearer ${token}`;
  }

  /**
   * Fetch wrapper that applies base URL and default headers.
   * @param input Path (relative to base) or full URL
   * @param init Fetch options
   */
  function fetchWithAuth(input: string, init: RequestInit = {}) {
    const url = input.startsWith('http') ? input : `${serverUrl}${input}`;
    const headers = { ...defaultHeaders, ...(init.headers || {}) };
    return fetch(url, { ...init, headers });
  }

  return fetchWithAuth;
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

