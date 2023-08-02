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
    this.intellisenseGatherer = new IntellisenseGatherer();
  }

  async componentDidLoad() {
  }

  async componentWillRender() {
  }

  @Listen('workflowSaved')
  async savedHandler(event: CustomEvent<any>) {
    state.workflowDefinitionId = event.detail.definitionId;
    state.javaScriptTypeDefinitions = '';
    state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
    console.log("clearing intellisense on Saved");
    await this.getIntellisense();
  }

  @Listen('shown', { target: 'window' })
  async modalHandleralt(event: CustomEvent<any>) {
    console.log("Modal is appearing", event);
    await this.getIntellisense();
  }

  async getIntellisense() {
    this.intellisenseGatherer.getIntellisense();
  }

  setDataDictionary() {
    if (this.dataDictionaryGroup != null) {
      this.dictionary = JSON.parse(this.dataDictionaryGroup);
      if (this.dictionary != null) {
        state.dictionaryGroups = this.dictionary;
      }
    }
  }

  setStoreConfig() {
    if (this.storeConfig != null) {
      this.config = JSON.parse(this.storeConfig);
      console.log("Config", this.config);
      if (this.config != null) {
        state.audience = this.config.audience;
        state.serverUrl = this.config.serverUrl;
        state.clientId = this.config.clientId;
        state.domain = this.config.domain;
        state.useRefreshToken = this.config.useRefreshTokens;
        state.useRefreshTokenFallback = this.config.useRefreshTokensFallback;
      }
    }
  }

  render() {

    return (
      <slot/>
    );
  }
}
