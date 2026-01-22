import { r as registerInstance, h } from './index-1542df5c.js';
import './index-892f713d.js';
import { D as DropdownButtonOrigin } from './models-9655958f.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import { b as createElsaClient } from './elsa-client-8304c78c.js';
import './event-bus-5d6f3774.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './fetch-client-f0dc2a52.js';
import './utils-db96334c.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './index-2db7bf78.js';

const ElsaUserContextMenu = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.userDetail = null;
    this.logoutStrategy = {
      "OpenIdConnect": function () {
        window.location.href = 'v1/ElsaAuthentication/logout';
      },
      "ServerManagedCookie": function () {
        window.location.href = 'v1/ElsaAuthentication/logout';
      },
      "JwtBearerToken": ""
    };
    this.serverUrl = undefined;
  }
  async componentWillRender() {
    try {
      this.userDetail = await (await (await createElsaClient(this.serverUrl)).authenticationApi.getUserDetails());
      this.authenticationConfguration = await (await (await createElsaClient(this.serverUrl)).authenticationApi.getAuthenticationConfguration());
    }
    catch (err) {
      this.userDetail = null;
    }
  }
  async menuItemSelected(item) {
    if (item.value == 'logout') {
      this.authenticationConfguration.authenticationStyles.forEach(x => {
        this.logoutStrategy[x]();
      });
    }
  }
  render() {
    if (this.userDetail == null) {
      return ('');
    }
    const ddlitems = [{ 'text': ("logout"), value: "logout" }].map(x => {
      const item = { text: x.text, isSelected: false, value: x.value };
      return item;
    }); // this dropdown only used for logout for now
    return (h("elsa-dropdown-button", { text: this.userDetail.name, items: ddlitems, btnClass: 'elsa-bg-gray-800 elsa-text-gray-300 elsa-w-full   elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-inline-flex elsa-justify-center elsa-text-sm elsa-font-medium', origin: DropdownButtonOrigin.TopRight, onItemSelected: e => this.menuItemSelected(e.detail) }));
  }
};
Tunnel.injectProps(ElsaUserContextMenu, ['serverUrl']);

export { ElsaUserContextMenu as elsa_user_context_menu };
