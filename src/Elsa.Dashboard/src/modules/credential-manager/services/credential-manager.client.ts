// Removed Axios in favor of Fetch API
import { createHttpClient } from '../../../services';
import { fetchJson, sendJson } from '../../../services/fetch-client';
import { SecretModel } from "../models/secret.model";

let _elsaSecretsClient: ElsaSecretsClient = null;

export const createElsaSecretsClient = async function (serverUrl: string): Promise<ElsaSecretsClient> {

  if (!!_elsaSecretsClient)
    return _elsaSecretsClient;

  const httpClient = await createHttpClient(serverUrl);

  _elsaSecretsClient = {
    secretsApi: {
      list: async () => {
        return await fetchJson<Array<SecretModel>>(httpClient, `v1/secrets`);
      },
      save: async request => {
        return await sendJson<SecretModel>(httpClient, 'POST', 'v1/secrets', request);
      },
      delete: async id => {
        const resp = await httpClient(`v1/secrets/${id}`, { method: 'DELETE' });
        if(!resp.ok) throw new Error('Failed to delete secret');
      },
    }
  }

  return _elsaSecretsClient;
}

export interface ElsaSecretsClient {
  secretsApi: SecretsApi;
}

export interface SecretsApi {

  list(): Promise<Array<SecretModel>>;

  save(request: SecretModel): Promise<SecretModel>;

  delete(secretId: string): Promise<void>;
}
