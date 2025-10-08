// Removed Axios in favor of Fetch API
import { createHttpClient } from '../../../services';
import { fetchJson } from '../../../services/fetch-client';

let _elsaOauth2Client: ElsaOauth2Client = null;

export const createElsaOauth2Client = async function (serverUrl: string): Promise<ElsaOauth2Client> {

  if (!!_elsaOauth2Client)
    return _elsaOauth2Client;

  const httpClient = await createHttpClient(serverUrl);

  _elsaOauth2Client = {
    oauth2Api: {
      getUrl: async secretId => {
        // The endpoint returns plain text (string URL). Use fetchJson fallback if it's JSON; otherwise text.
        const resp = await httpClient(`v1/oauth2/url/${secretId}`);
        if(!resp.ok) throw new Error('Failed to get oauth2 url');
        const text = await resp.text();
        return text;
      }
    }
  }

  return _elsaOauth2Client;
}

export interface ElsaOauth2Client {
  oauth2Api: Oauth2Api;
}

export interface Oauth2Api {
  getUrl(secretId: string): Promise<string>;
}
