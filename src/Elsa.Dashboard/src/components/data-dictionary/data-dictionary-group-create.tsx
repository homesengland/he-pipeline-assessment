import { Component, h, State, Prop} from '@stencil/core';
import { RouterHistory } from '@stencil/router';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import state from '../../stores/store';
import { GetAuth0Options, CreateClient } from '../../http-clients/http-client-services';
import { AxiosInstance } from "axios";

@Component({
  tag: 'data-dictionary-group-create',
  shadow: false,
})
export class DataDictionaryGroupCreate {
  @State() groupName: string;
  @Prop() history: RouterHistory;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private serverUrl: string = null;
  private elsaClient: AxiosInstance = null;

  constructor() {

    this.options = GetAuth0Options();
    this.serverUrl = state.serverUrl;   
  }

  async componentWillLoad() {
  }

  async createGroup() {
    await this.getHttpClient();
    let createDataDictionaryGroup = {
      Name: this.groupName
      }
    await this.elsaClient.post<string>(`datadictionary/CreateDataDictionaryGroup`, createDataDictionaryGroup);
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

   async getHttpClient() {
      await this.initialize();
      if (this.elsaClient == null) {
        this.elsaClient = await CreateClient(this.auth0, this.serverUrl);
      }
    }

  async onSubmit(e: Event) {
    e.preventDefault();
    await this.createGroup();
    this.history.push('/data-dictionary');
  }

  handleChange(event) {
    this.groupName = event.target.value;
  }

  render() {
    return (
      <div>
        <div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <div class="elsa-flex-1 elsa-min-w-0">
              <h1 class="elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate">
                Create Data Dictionary Group
              </h1>
            </div>
            <div class="elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4">
              <a href="/data-dictionary">Back</a>
            </div>
          </div>
        </div>
        <form onSubmit={e => this.onSubmit(e)} class='activity-editor-form'>
        <div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <label htmlfor="GroupName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Name</label>
          </div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input type="text" value={this.groupName} onInput={(event) => this.handleChange(event)}  class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
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
