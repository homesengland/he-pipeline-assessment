import axios from "axios";
import state from '../stores/store';
import { createAuth0Client } from '@auth0/auth0-spa-js';
import { StoreStatus } from "../constants/constants";
export class IntellisenseGatherer {
    constructor() {
        this._httpClient = null;
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
        let httpClient = await this.createHttpClient();
        const response = await httpClient.post(`v1/scripting/javascript/type-definitions/${workflowDefinitionId}?t=${new Date().getTime()}`, context);
        return response.data;
    }
    async createHttpClient() {
        await this.getAuth0Client();
        if (!!this._httpClient) {
            return this._httpClient;
        }
        const token = await this.auth0.getTokenSilently();
        let config;
        if (!!token) {
            config = {
                baseURL: this._baseUrl,
                headers: { 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` }
            };
        }
        else {
            config = {
                baseURL: this._baseUrl,
                headers: { 'Content-Type': `application/json; charset=UTF-8` },
            };
        }
        this._httpClient = axios.create(config);
        return this._httpClient;
    }
}
