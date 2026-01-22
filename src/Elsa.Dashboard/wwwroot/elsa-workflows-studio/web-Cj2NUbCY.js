import { b as WebPlugin } from './credential-manager-plugin-DUzW1Hqm.js';
import '@stencil-community/router';
import './index-FqXAnKkK.js';
import './events-CpKc8CLe.js';
import './index-Ba0EgaQC.js';
import './elsa-client-vTNcky8U.js';
import './index-Co5PE-97.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './dashboard-DAUekgN5.js';
import './index-C-8L13GY.js';
import './store-C2B016J5.js';

class BrowserWeb extends WebPlugin {
    constructor() {
        super();
        this._lastWindow = null;
    }
    async open(options) {
        this._lastWindow = window.open(options.url, options.windowName || '_blank');
    }
    async close() {
        return new Promise((resolve, reject) => {
            if (this._lastWindow != null) {
                this._lastWindow.close();
                this._lastWindow = null;
                resolve();
            }
            else {
                reject('No active window to close!');
            }
        });
    }
}
const Browser = new BrowserWeb();

export { Browser, BrowserWeb };
//# sourceMappingURL=web-Cj2NUbCY.js.map

//# sourceMappingURL=web-Cj2NUbCY.js.map