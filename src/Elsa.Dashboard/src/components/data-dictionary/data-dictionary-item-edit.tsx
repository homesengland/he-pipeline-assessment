import { Component, h, State, Prop } from '@stencil/core';
import { RouterHistory } from '@stencil/router';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import state from '../../stores/store';
import { MatchResults } from '@stencil/router';
import { DataDictionary, DataDictionaryGroup } from '../../models/custom-component-models';
import { GetAuth0Options, CreateClient } from '../../http-clients/http-client-services';

@Component({
  tag: 'data-dictionary-item-edit',
  shadow: false,
})
export class DataDictionaryItemEdit {
  @State() item: DataDictionary;
  @State() groupId: number;
  @Prop() history: RouterHistory;
  @Prop() match: MatchResults;
  @Prop() isEditable: boolean;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private serverUrl: string = null;
  id?: string;
  itemId?: string;

  constructor() {
    this.options = GetAuth0Options();
    this.serverUrl = state.serverUrl;
    this.serverUrl = state.serverUrl;
    this.isEditable = false;
  }

  async componentWillLoad() {
    let id = this.match.params.id;
    let itemId = this.match.params.itemId;
    if (!!id && id.toLowerCase() == 'new') 
      id = null;
    if (!!itemId && itemId.toLowerCase() == 'new')
      itemId = null;
    
    this.itemId = itemId;
    this.id = id;
    await this.loadDataDictionaryItem(this.id, this.itemId);
  }

  async loadDataDictionaryItem(id: any, itemId: any) {
    await this.initialize();
    //const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    //const response = await elsaClient.get<Array<DataDictionaryGroup>>(`activities/dictionary/`);
    let group = state.dictionaryGroups.find(x => x.Id == id);
    this.item = group.DataDictionaryList.find(x=>x.Id ==itemId);
  }

  async updateItem() {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    let createDataDictionaryItem = {
      Item: this.item
    }
    await elsaClient.post<DataDictionary>(`datadictionary/UpdateDataDictionaryItem`, createDataDictionaryItem);
  }

  updateState() {
    let groups: DataDictionaryGroup[] = state.dictionaryGroups;
    let group: DataDictionaryGroup = groups.find(x => x.Id == this.groupId);
    let objIndex: number = group.DataDictionaryList.findIndex(x => x.Id == this.item.Id);
    group.DataDictionaryList[objIndex] = this.item;
    groups[this.groupId] = group;
    state.dictionaryGroups = groups;
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
    await this.updateItem();
    this.updateState();
    this.history.push(`/data-dictionary/group/${this.id}`);
  }

  async onSubmitArchive(e: Event) {
    e.preventDefault();
    this.item.IsArchived = !this.item.IsArchived
    await this.archive(this.item.Id, this.item.IsArchived);
    this.updateState();
    this.history.push(`/data-dictionary/group/${this.id}`);
  }

  handleItemNameChange(event) {
    this.item.Name = event.target.value.replaceAll(" ", "");
  }

  handleItemLegacyNameChange(event) {
    this.item.LegacyName = event.target.value.replaceAll(" ", "");
  }

  onCheckChanged(event) {
    const checkbox = (event.target as HTMLInputElement);
    this.isEditable = checkbox.checked;
  }

  archiveText(): string {
    return this.item.IsArchived ? "Restore" : "Archive";
  }

  async archive(id: any, isArchived: boolean) {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    let archiveCommand = {
      Id: id,
      IsArchived: isArchived
    }
    await elsaClient.post<string>(`datadictionary/ArchiveDataDictionaryItem`, archiveCommand);
    this.updateState();
  }

  render() {
    const backUrl = `/data-dictionary/group/${this.id}`;
    const warningText = "WARNING: Do not change the name of the Data Dictionary Item unless it was input in error.  Doing so may cause errors when running workflows.  Tick checkbox to enable editing.";

    return (
      <div>
        <div>
          <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
            <div class="elsa-flex-1 elsa-min-w-0">
              <h1 class="elsa-text-lg elsa-font-medium elsa-leading-6 elsa-text-gray-900 sm:elsa-truncate">
                Update Data Dictionary Item
              </h1>
            </div>
            <div class="elsa-mt-4 elsa-flex sm:elsa-mt-0 sm:elsa-ml-4">
              <a href={backUrl}>Back</a>
            </div>
          </div>
        </div>
        <form onSubmit={e => this.onSubmit(e)} class='activity-editor-form'>
          <div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-left sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input id="isEditable" name="isEditable" type="checkbox" checked={this.isEditable}
                onChange={e => this.onCheckChanged(e)}
                class="focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              <div class="elsa-ml-3 elsa-text-sm">
                <label htmlFor="isEditable" class="elsa-font-medium elsa-font-bold elsa-text-gray-700">{warningText}</label>
              </div>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-block sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Name</label>
              </div>
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-text-xs elsa-text-gray-500">* Names must be consistent between setup and production environments</label>
              </div>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input disabled={!this.isEditable} type="text" value={this.item.Name} onInput={(event) => this.handleItemNameChange(event)} class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-block sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Legacy Name</label>
              </div>
              <div >
                <label htmlfor="ItemName" class="elsa-block elsa-text-sm elsa-text-xs elsa-text-gray-500">Legacy names correspond with values stored within the legacy systems</label>
              </div>
            </div>
            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center sm:elsa-justify-between sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <input disabled={!this.isEditable} type="text" value={this.item.LegacyName} onInput={(event) => this.handleItemLegacyNameChange(event)} class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"></input>
            </div>


            <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
              <button type="submit" aria-has-popup="true" class=" elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
                Update </button>
            </div>

          </div>
        </form>
        <form onSubmit={e => this.onSubmitArchive(e)} class='activity-editor-form'>
        

        <div class="elsa-border-b elsa-border-gray-200 elsa-px-4 elsa-py-4 sm:elsa-flex sm:elsa-items-center  sm:elsa-px-6 lg:elsa-px-8 elsa-bg-white">
          <button type="submit" aria-has-popup="true" class=" elsa-order-0 elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-order-1 sm:elsa-ml-3">
            {this.archiveText()} </button>
          </div>
        </form>
      </div>
    );
  }
}

