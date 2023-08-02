import { Component, h, Listen, Prop } from '@stencil/core';
import { DataDictionaryGroup } from '../models/custom-component-models';
import state  from '../stores/store';
import { StoreConfig } from '../models/StoreConfig';
import { getWorkflowDefinitionIdFromUrl } from '../utils/utils';
import { StoreStatus } from '../constants/constants';

@Component({
  tag: 'he-dashboard',
  shadow: false,
})

export class HeDashboard {

  @Prop() dataDictionaryGroup: Array<DataDictionaryGroup>;
  @Prop({ attribute: 'store-config', reflect: true }) storeConfig: string;
  config: StoreConfig;

  async componentWillLoad() {
    console.log("The Dashboard is Loading");
    console.log("Store Config string", this.storeConfig);
    if (this.storeConfig != null) {
      this.config = JSON.parse(this.storeConfig);
      console.log("Config", this.config);
      if (this.config != null) {
        state.audience = this.config.audience;
        state.serverUrl = this.config.serverUrl;
        state.clientId = this.config.clientId;
        state.domain = this.config.domain;
        state.workflowDefinitionId = getWorkflowDefinitionIdFromUrl();
      }
    }

    state.dictionaryGroups = this.dataDictionaryGroup;
  }

  async componentDidLoad() {
  }

  async componentWillRender() {
  }

  @Listen('workflowSaved')
  savedHandler(event: CustomEvent<any>) {
    state.workflowDefinitionId = event.detail.definitionId;
    state.javaScriptTypeDefinitions = '';
    state.javaScriptTypeDefinitionsFetchStatus = StoreStatus.Empty;
  }

  render() {

    return (
      <slot/>
    );
  }
}
