import { r as registerInstance, h } from './index-CL6j2ec2.js';
import './index-fZDMH_YE.js';
import { D as DropdownButtonOrigin } from './models-DnZLya3J.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import { b as createElsaClient } from './elsa-client-q6ob5JPZ.js';
import './event-bus-axQqcjdg.js';
import './index-D7wXd6HU.js';
import './events-CpKc8CLe.js';
import './utils-C0M_5Llz.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './index-C-8L13GY.js';
import './fetch-client-1OcjQcrw.js';

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
//# sourceMappingURL=elsa-user-context-menu.entry.esm.js.map
