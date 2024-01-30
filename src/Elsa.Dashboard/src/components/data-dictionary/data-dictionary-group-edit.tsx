import { Component, h, State, Prop } from '@stencil/core';
import { MatchResults, RouterHistory } from '@stencil/router';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import state from '../../stores/store';
import { DataDictionary, DataDictionaryGroup } from '../../models/custom-component-models';
import * as collection from 'lodash/collection';
import { GetAuth0Options, CreateClient } from '../../http-clients/http-client-services';

@Component({
  tag: 'data-dictionary-group-edit',
  shadow: false,
})
export class DataDictionaryGroupScreen {
  @State() private dataDictionaryItems: Array<DataDictionary>;
  @State() private group: DataDictionaryGroup = { Name: '', Id: null, DataDictionaryList: [], IsArchived: false };
  @Prop() basePath: string;
  @Prop() match: MatchResults;
  @Prop() history: RouterHistory;
  @Prop() isEditable: boolean;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private serverUrl: string = null;
  id?: string;


  constructor() {
    this.options = GetAuth0Options();
    this.serverUrl = state.serverUrl;
    this.isEditable = false;
  }

  async loadDataDictionaryGroups(id: any) {
    await this.initialize();
    this.group = state.dictionaryGroups.find(x => x.Id == id);
    //const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    //const response = await (await elsaClient.get<Array<DataDictionaryGroup>>(`activities/dictionary/`, { params: { includeArchived: 'true' } }));
    //this.group = response.data.find(x => x.Id == id);
    this.dataDictionaryItems = this.group.DataDictionaryList;
    console.log("Group", this.group);
    console.log("Items", this.dataDictionaryItems);
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

  async componentWillLoad() {
    let id = this.match.params.id;

    if (!!id && id.toLowerCase() == 'new')
      id = null;

    this.id = id;
    await this.loadDataDictionaryGroups(this.id);
  }

  async onSubmit(e: Event) {
    e.preventDefault();
    await this.updateGroup();
    this.updateState();
  }

  async onSubmitArchive(e: Event) {
    e.preventDefault();
    this.group.IsArchived = !this.group.IsArchived;
    await this.archive(this.group.Id, this.group.IsArchived);
    this.updateState();
  }

  async updateGroup() {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    let updateCommand = {
      Group: this.group
      }
    await elsaClient.post<string>(`datadictionary/UpdateDataDictionaryGroup`, updateCommand);
    await this.loadDataDictionaryGroups(this.id);
  }

    updateState() {
      let groups: DataDictionaryGroup[] = state.dictionaryGroups;
      let group: DataDictionaryGroup = groups.find(x => x.Id == this.group.Id);
      groups[this.group.Id] = group;
      state.dictionaryGroups = groups;
    }

  async archive(id: any, isArchived: boolean) {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    let archiveCommand = {
      Id: id,
      isArchived: isArchived
    }
    await elsaClient.post<string>(`datadictionary/ArchiveDataDictionaryGroup`, archiveCommand);
    await this.loadDataDictionaryGroups(this.id);
  }

  handleChange(event) {
    this.group.Name = event.target.value.replaceAll(" ", "");
  }

  onCheckChanged(event) {
    const checkbox = (event.target as HTMLInputElement);
    this.isEditable = checkbox.checked;
  }

  archiveText(): string {
    return this.group.IsArchived ? "Restore" : "Archive";
  }

  render() {
    return (
      <div>
        <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
          <div class="elsa-flex-1 elsa-min-w-0">
            <h1 class="elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate">
              Data Dictionary Group 
            </h1>
          </div>
          <div class="elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4">
            <a href="/data-dictionary">Back</a>
          </div>
        </div>
        <form onSubmit={e => this.onSubmit(e)} class='activity-editor-form'>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-block sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <div >
              <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Name</label>
            </div>
            <div >
              <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-text-xs elsa-text-gray-500">* Names must be consistent between setup and production environments</label>
            </div>
          </div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <input disabled={!this.isEditable} type="text" value={this.group.Name} onInput={(event) => this.handleChange(event)} class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
          </div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-left sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
          <input  id="isEditable" name="isEditable" type="checkbox" checked={this.isEditable}
            onChange={e => this.onCheckChanged(e)}
              class="focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            <div class="elsa-ml-3 elsa-text-sm">
              <label htmlFor="isEditable" class="elsa-font-medium elsa-font-bold elsa-text-gray-700">{"WARNING: Do not change the name of the Data Dictionary Group unless it was input in error.  Doing so may cause errors when running workflows."}</label>
            </div>
          </div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <button type="submit" aria-has-popup="true" class=" elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
              Update Name </button>
          </div>
        </form>

        <form onSubmit={e => this.onSubmitArchive(e)} class='activity-editor-form'>


          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <button type="submit" aria-has-popup="true" class=" elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
              {this.archiveText()} </button>
          </div>
        </form>

        <table class="elsa-min-w-full">
          <thead>
            <tr class="elsa-border-t elsa-border-gray-200">
              <th
                class="elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider"><span
                  class="lg:elsa-pl-2">Data Dictionary Items</span>
              </th>
              <th
                class="elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider"><span
                  class="lg:elsa-pl-2">Is Archived?</span>
              </th>
            </tr>
          </thead>
          <tbody class="elsa-bg-white elsa-divide-y elsa-divide-gray-100">
            {collection.map(this.dataDictionaryItems, dataTag => {
              const editUrl = `/data-dictionary/group/${this.id}/item/${dataTag.Id}`;
              const name = dataTag.Name;
              const isArchived = dataTag.IsArchive != null ? dataTag.IsArchived.toString(): 'false';
              console.log(isArchived);
              const editIcon = (
                <svg class="elsa-h-5 elsa-w-5 elsa-text-gray-500" width="24" height="24" viewBox="0 0 24 24"
                  xmlns="http://www.w3.org/2000/svg" fill="none" stroke="currentColor" stroke-width="2"
                  stroke-linecap="round"
                  stroke-linejoin="round">
                  <path d="M11 4H4a2 2 0 0 0-2 2v14a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2v-7" />
                  <path d="M18.5 2.5a2.121 2.121 0 0 1 3 3L12 15l-4 1 1-4 9.5-9.5z" />
                </svg>
              );
              return (
                <tr>
                  <td
                    class="elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900">
                    <div class="elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2">
                      <stencil-route-link anchorClass="elsa-truncate hover:elsa-text-gray-600">
                        <span>{name}</span></stencil-route-link>
                    </div>
                  </td>
                  <td
                    class="elsa-px-6 elsa-py-3 elsa-whitespace-no-wrap elsa-text-sm elsa-leading-5 elsa-font-medium elsa-text-gray-900">
                    <div class="elsa-flex elsa-items-center elsa-space-x-3 lg:elsa-pl-2">
                      {isArchived}
                      </div>
                  </td>
                  <td class="elsa-pr-6">
                    <elsa-context-menu-new history={this.history} menuItems={[
                      { text: 'Edit', anchorUrl: editUrl, icon: editIcon },
                    ]} />
                  </td>
                </tr>
              );
            })
            }
          </tbody>
        </table>
        <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
          <stencil-route-link url={`/data-dictionary/group/${this.id}/item/new`}
            class="elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
            Create Item
          </stencil-route-link>
        </div>
      </div>
    );
  }

}
