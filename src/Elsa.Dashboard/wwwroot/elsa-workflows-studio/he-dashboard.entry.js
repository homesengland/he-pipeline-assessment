import { r as registerInstance, h } from './index-1542df5c.js';
import { s as state } from './store-40346019.js';
import { a as StoreStatus } from './constants-6ea82f24.js';
import { I as IntellisenseGatherer } from './intellisenseGatherer-b418d374.js';
import './index-0d4e8807.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './events-d0aab14a.js';
import './auth0-spa-js.production.esm-eb2e28a3.js';

const HeDashboard = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.dataDictionaryGroup = undefined;
    this.storeConfig = undefined;
  }
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

export { HeDashboard as he_dashboard };
