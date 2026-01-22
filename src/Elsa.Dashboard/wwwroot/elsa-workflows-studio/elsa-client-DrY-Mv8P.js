import { c as createFetchHttpClient } from './fetch-client-1OcjQcrw.js';

let _httpClient = null;
let _elsaWebhooksClient = null;
const createHttpClient = async function (baseAddress) {
    if (!!_httpClient)
        return _httpClient;
    _httpClient = await createFetchHttpClient(baseAddress);
    return _httpClient;
};
const createElsaWebhooksClient = async function (serverUrl) {
    if (!!_elsaWebhooksClient)
        return _elsaWebhooksClient;
    const httpClient = _httpClient !== null && _httpClient !== void 0 ? _httpClient : await createHttpClient(serverUrl);
    _elsaWebhooksClient = {
        webhookDefinitionsApi: {
            list: async (page, pageSize) => {
                const response = await httpClient.get(`v1/webhook-definitions`);
                return response.data;
            },
            getByWebhookId: async (webhookId) => {
                const response = await httpClient.get(`v1/webhook-definitions/${webhookId}`);
                return response.data;
            },
            save: async (request) => {
                const response = await httpClient.post('v1/webhook-definitions', request);
                return response.data;
            },
            update: async (request) => {
                const response = await httpClient.put('v1/webhook-definitions', request);
                return response.data;
            },
            delete: async (webhookId) => {
                await httpClient.delete(`v1/webhook-definitions/${webhookId}`);
            },
        }
    };
    return _elsaWebhooksClient;
};

export { createElsaWebhooksClient as c };
//# sourceMappingURL=elsa-client-DrY-Mv8P.js.map

//# sourceMappingURL=elsa-client-DrY-Mv8P.js.map