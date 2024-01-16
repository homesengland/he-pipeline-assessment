import { Component, h, State, Prop } from '@stencil/core';
import { RouterHistory } from '@stencil/router';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import state from '../../stores/store';
import { MatchResults } from '@stencil/router';
import { GetAuth0Options, CreateClient } from '../../http-clients/http-client-services';

@Component({
  tag: 'data-dictionary-item-create',
  shadow: false,
})
export class DataDictionaryItemCreate {
  @State() itemName: string;
  @State() itemLegacyName: string;
  @Prop() history: RouterHistory;
  @Prop() match: MatchResults;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private serverUrl: string = null;
  id?: string;

  constructor() {
    this.options = GetAuth0Options();
    this.serverUrl = state.serverUrl;
  }

  async componentWillLoad() {
    let id = this.match.params.id;

    if (!!id && id.toLowerCase() == 'new')
      id = null;

    this.id = id;
  }

  async createItem() {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    let createDataDictionaryItem = {
      Name: this.itemName,
      LegacyName: this.itemLegacyName
    }
    await elsaClient.post<string>(`datadictionary/CreateDataDictionaryItem`, createDataDictionaryItem);
  }

  initialize = async () => {
    const options = this.options;
    const { domain } = options;

    if (!domain || domain.trim().length == 0)
      return;

    this.auth0 = await createAuth0Client(this.options);
    const isAuthenticated = await this.auth0.isAuthenticated();

    if (isAuthenticated)
      return;
  };

   async onSubmit(e: Event) {
    e.preventDefault();
    await this.createItem();
    this.history.push(`/data-dictionary/group/${this.id}`);
  }

  handleNameChange(event) {
    this.itemName = event.target.value.replaceAll(" ", "");
  }
  handleLegacyNameChange(event) {
    this.itemLegacyName = event.target.value.replaceAll(" ", "");
  }

  render() {
    const backUrl = `/data-dictionary/group/${this.id}`;
    return (
      <div>
        <div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <div class="elsa-flex-1 elsa-min-w-0">
              <h1 class="elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate">
                Create Data Dictionary Item
              </h1>
            </div>
            <div class="elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4">
              <a href={backUrl}>Back</a>
            </div>
          </div>
        </div>
        <form onSubmit={e => this.onSubmit(e)} class='activity-editor-form'>
          <div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-block sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Name</label>
              </div>
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-text-xs elsa-text-gray-500">* Names must be consistent between setup and production environments</label>
              </div>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input type="text" value={this.itemName} onInput={(event) => this.handleNameChange(event)} class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <label htmlfor="LegacyName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Legacy Name</label>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input type="text" value={this.itemLegacyName} onInput={(event) => this.handleLegacyNameChange(event)} class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <button type="submit" aria-has-popup="true" class=" elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
                Create </button>
            </div>
          </div>
        </form>
      </div>
    );
  }
}
