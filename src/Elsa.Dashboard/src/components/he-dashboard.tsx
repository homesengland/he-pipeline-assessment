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

export class QuestionScreen {

  @Prop() dataDictionaryGroup: Array<DataDictionaryGroup>;
  @Prop({ attribute: 'store-config', reflect: true }) storeConfig: StoreConfig;

  async componentWillLoad() {
    console.log("The Dashboard is Loading");
    console.log("Store Config", this.storeConfig);
      if (this.storeConfig != null) {
        state.audience = this.storeConfig.audience;
        state.serverUrl = this.storeConfig.serverUrl;
        state.clientId = this.storeConfig.clientId;
        state.domain = this.storeConfig.domain;
        state.workflowDefinitionId = getWorkflowDefinitionIdFromUrl();
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
