import { Component, h, State } from '@stencil/core';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import state from '../../stores/store';
import * as collection from 'lodash/collection';
import { MenuItem } from '../../models/elsa-interfaces';
import { GetAuth0Options, CreateClient } from '../../http-clients/http-client-services';

@Component({
  tag: 'data-dictionary-groups',
  shadow: false,
})

export class DataDictionaryGroups {
  @State() private dataDictionaryGroups: string;
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;
  private serverUrl: string = null;

  constructor() {
    this.options = GetAuth0Options();
    this.serverUrl = state.serverUrl;   
  }

  async componentWillLoad() {
    this.loadDataDictionaryGroups();
  }

  async loadDataDictionaryGroups() {
    await this.initialize();
    const elsaClient = await CreateClient(this.auth0, this.serverUrl);
    const response = await elsaClient.get<string>(`activities/dictionary/`);
    this.dataDictionaryGroups = response.data;
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

  render() {
    const renderContextMenu = (
      editUrl: string,
      editIcon: any) => {
      console.log(editIcon);
      let menuItems: MenuItem[] = [];
      menuItems = [...menuItems, ...[{ text: 'Edit', anchorUrl: editUrl, icon: editIcon }]];

      return (<td class="elsa-pr-6">
        <elsa-context-menu menuItems={menuItems} />
      </td>)
    }

    return (
      <div>
        <table class="elsa-min-w-full">
          <thead>
            <tr class="elsa-border-t elsa-border-gray-200">
              <th
                class="elsa-px-6 elsa-py-3 elsa-border-b elsa-border-gray-200 elsa-bg-gray-50 elsa-text-left elsa-text-xs elsa-leading-4 elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-uppercase elsa-tracking-wider"><span
                  class="lg:elsa-pl-2">Groups</span>
              </th>              
            </tr>
          </thead>
          <tbody class="elsa-bg-white elsa-divide-y elsa-divide-gray-100">
            {collection.map(this.dataDictionaryGroups, group => {
              const editUrl = `/data-dictionary/group/${group.Id}`;
              const name = group.Name;
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
                    <stencil-route-link  anchorClass="elsa-truncate hover:elsa-text-gray-600">
                      <span>{name}</span></stencil-route-link>
                  </div>
                  </td>
                  {renderContextMenu( editUrl, editIcon)}
                  </tr>
              );
            })
              }
          </tbody>
        </table>
      </div>
    );
  }
}

