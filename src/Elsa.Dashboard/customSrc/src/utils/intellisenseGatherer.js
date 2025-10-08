import state from '../stores/store';
import { createAuth0Client } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../constants/constants";
export class IntellisenseGatherer {
    constructor() {
    this._fetchWithAuth = null;
        this._baseUrl = null;
        this.context = {
            activityTypeName: "",
            propertyName: ""
        };
        this.initialize = async () => {
            const options = this.options;
            const { domain } = options;
            if (!domain || domain.trim().length == 0)
                return;
            this.auth0 = await createAuth0Client(this.options);
            const isAuthenticated = await this.auth0.isAuthenticated();
            // Nothing to do if authenticated.
            if (isAuthenticated)
                state.auth0Client = this.auth0;
            return;
        };
        let auth0Params = {
            audience: state.audience,
        };
        let auth0Options = {
            authorizationParams: auth0Params,
            clientId: state.clientId,
            domain: state.domain,
            useRefreshTokens: state.useRefreshToken,
            useRefreshTokensFallback: state.useRefreshTokenFallback,
            cacheLocation: "memory"
        };
        this.options = auth0Options;
        this._baseUrl = state.serverUrl;
    }
    async getAuth0Client() {
        if (state.auth0Client == null) {
            await this.initialize();
        }
        return state.auth0Client;
    }
    async getIntellisense() {
        if (state.javaScriptTypeDefinitionsFetchStatus == StoreStatus.Available && this.hasDefinitions) {
            return state.javaScriptTypeDefinitions;
        }
        else {
            this.tryFetchDefinitions();
            return state.javaScriptTypeDefinitions;
        }
    }
    async tryFetchDefinitions() {
        state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Fetching;
        let definitions = await this.getJavaScriptTypeDefinitions(state.workflowDefinitionId, this.context);
        state.javaScriptTypeDefinitions = definitions;
        if (state.javaScriptTypeDefinitions != null) {
            this.appendGatheredValues();
            state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Available;
        }
        else {
            state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
            state.javaScriptTypeDefinitions = '';
        }
        return state.javaScriptTypeDefinitions;
    }
    hasDefinitions() {
        return state.javaScriptTypeDefinitions != null && state.javaScriptTypeDefinitions.trim().length > 0;
    }
    appendGatheredValues() {
        let intellisense = state.javaScriptTypeDefinitions + state.dataDictionaryIntellisense;
        state.javaScriptTypeDefinitions = intellisense;
    }
    async getJavaScriptTypeDefinitions(workflowDefinitionId, context) {
        const fetchWithAuth = await this.createHttpClient();
        const resp = await fetchWithAuth(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json; charset=UTF-8' },
            body: JSON.stringify(context)
        });
        if (!resp.ok)
            throw new Error('Failed to fetch type definitions');
        return await resp.text();
    }
    async createHttpClient() {
        await this.getAuth0Client();
        if (!!this._fetchWithAuth) {
            return this._fetchWithAuth;
        }
        const token = await this.auth0.getTokenSilently();
        const defaultHeaders = { 'Content-Type': 'application/json; charset=UTF-8' };
        if (token)
            defaultHeaders['Authorization'] = `Bearer ${token}`;
        const base = this._baseUrl;
        const fetchWithAuth = (input, init = {}) => {
            const url = input.startsWith('http') ? input : `${base}${input}`;
            const headers = Object.assign(Object.assign({}, defaultHeaders), (init.headers || {}));
            return fetch(url, Object.assign(Object.assign({}, init), { headers }));
        };
        this._fetchWithAuth = fetchWithAuth;
        return fetchWithAuth;
    }
}
