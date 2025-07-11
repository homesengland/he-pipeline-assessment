var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, h, Listen, Prop } from '@stencil/core';
import state from '../stores/store';
import { StoreStatus } from '../constants/constants';
import { IntellisenseGatherer } from '../utils/intellisenseGatherer';
let HeDashboard = class HeDashboard {
    async componentWillLoad() {
        this.setStoreConfig();
        this.setDataDictionary();
        this.intellisenseGatherer = new IntellisenseGatherer();
    }
    async componentDidLoad() {
    }
    async componentWillRender() {
    }
    async modalHandlerShow(event) {
        event = event;
        var url_string = document.URL;
        var n = url_string.lastIndexOf('/');
        var workflowDef = url_string.substring(n + 1);
        state.workflowDefinitionId = workflowDef;
        await this.getIntellisense();
    }
    async modalHandlerHidden(event) {
        event = event;
        state.javaScriptTypeDefinitions = '';
        state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
        await this.getIntellisense();
    }
    async getIntellisense() {
        await this.intellisenseGatherer.getIntellisense();
    }
    setDataDictionary() {
        if (this.dataDictionaryGroup != null) {
            this.dictionary = JSON.parse(this.dataDictionaryGroup);
            if (this.dictionary != null) {
                state.dictionaryGroups = this.dictionary;
            }
        }
        this.dataDictionaryGroup = null;
    }
    getClientOptions() {
        this.config = JSON.parse(this.storeConfig);
        const origin = window.location.origin;
        let auth0Params = {
            audience: this.config.audience,
            redirect_uri: origin
        };
        let auth0Options = {
            authorizationParams: auth0Params,
            clientId: this.config.clientId,
            domain: this.config.domain,
            useRefreshTokens: this.config.useRefreshTokens,
            useRefreshTokensFallback: this.config.useRefreshTokensFallback,
        };
        return auth0Options;
    }
    async setStoreConfig() {
        if (this.storeConfig != null) {
            this.config = JSON.parse(this.storeConfig);
            if (this.config != null) {
                state.audience = this.config.audience;
                state.serverUrl = this.config.serverUrl;
                state.clientId = this.config.clientId;
                state.domain = this.config.domain;
                state.useRefreshToken = this.config.useRefreshTokens;
                state.useRefreshTokenFallback = this.config.useRefreshTokensFallback;
                state.monacoLibPath = this.config.monacoLibPath;
                state.dataDictionaryIntellisense = this.config.dataDictionaryIntellisense;
            }
        }
        this.storeConfig = null;
    }
    render() {
        return (h("slot", null));
    }
};
__decorate([
    Prop({ attribute: 'data-dictionary', reflect: true })
], HeDashboard.prototype, "dataDictionaryGroup", void 0);
__decorate([
    Prop({ attribute: 'store-config', reflect: true })
], HeDashboard.prototype, "storeConfig", void 0);
__decorate([
    Listen('shown', { target: 'window' })
], HeDashboard.prototype, "modalHandlerShow", null);
__decorate([
    Listen('hidden', { target: 'window' })
], HeDashboard.prototype, "modalHandlerHidden", null);
HeDashboard = __decorate([
    Component({
        tag: 'he-dashboard',
        shadow: false,
    })
], HeDashboard);
export { HeDashboard };
