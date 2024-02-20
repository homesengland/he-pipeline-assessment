import { Component, h, Listen, Prop } from '@stencil/core';
import { DataDictionaryGroup } from '../models/custom-component-models';
import state  from '../stores/store';
import { StoreConfig } from '../models/StoreConfig';
import { StoreStatus } from '../constants/constants';
import { IntellisenseGatherer } from '../utils/intellisenseGatherer';

@Component({
  tag: 'he-dashboard',
  shadow: false,
})

export class HeDashboard {

  @Prop({ attribute: 'data-dictionary', reflect: true }) dataDictionaryGroup: string;
  @Prop({ attribute: 'store-config', reflect: true }) storeConfig: string;
  config: StoreConfig;
  dictionary: Array<DataDictionaryGroup>;
  intellisenseGatherer: IntellisenseGatherer;

  async componentWillLoad() {
    this.setStoreConfig();
    this.setDataDictionary();
    //this.setDictionaryLib();
    console.log("Intellisense", this.config.dataDictionaryIntellisense);
    this.intellisenseGatherer = new IntellisenseGatherer();
  }

  async componentDidLoad() {
  }

  async componentWillRender() {
  }

  @Listen('shown', { target: 'window' })
  async modalHandlerShow(event: CustomEvent<any>) {
    event = event;
    var url_string = document.URL;
    var n = url_string.lastIndexOf('/');
    var workflowDef = url_string.substring(n + 1);
    state.workflowDefinitionId = workflowDef;

    await this.getIntellisense();
  }

  @Listen('hidden', { target: 'window' })
  async modalHandlerHidden(event: CustomEvent<any>) {
    event = event;
    state.javaScriptTypeDefinitions = '';
    state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
    await this.getIntellisense();
  }

  async getIntellisense() {
    this.intellisenseGatherer.getIntellisense();
  }

  setDataDictionary() {
    if (this.dataDictionaryGroup != null) {
      this.dictionary = JSON.parse(this.dataDictionaryGroup);
      if (this.dictionary != null) {
        console.log("Setting dictionary", this.dictionary)
        state.dictionaryGroups = this.dictionary;
      }
    }
    this.dataDictionaryGroup = null;
  }

  setStoreConfig() {
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

    return (
      <slot/>
    );
  }
}
