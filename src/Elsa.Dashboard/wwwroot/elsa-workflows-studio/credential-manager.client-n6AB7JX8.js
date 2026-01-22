import { c as createHttpClient } from './elsa-client-CbFnoFr9.js';

let _elsaSecretsClient = null;
const createElsaSecretsClient = async function (serverUrl) {
    if (!!_elsaSecretsClient)
        return _elsaSecretsClient;
    const httpClient = await createHttpClient(serverUrl);
    _elsaSecretsClient = {
        secretsApi: {
            list: async () => {
                const response = await httpClient.get(`v1/secrets`);
                return response.data;
            },
            save: async (request) => {
                const response = await httpClient.post('v1/secrets', request);
                return response.data;
            },
            delete: async (id) => {
                await httpClient.delete(`v1/secrets/${id}`);
            },
        }
    };
    return _elsaSecretsClient;
};

export { createElsaSecretsClient as c };
//# sourceMappingURL=credential-manager.client-n6AB7JX8.js.map

//# sourceMappingURL=credential-manager.client-n6AB7JX8.js.map