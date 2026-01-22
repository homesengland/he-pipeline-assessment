import { b as WebPlugin } from './credential-manager-plugin-CkE8S9pr.js';
import './index-FqXAnKkK.js';
import './events-CpKc8CLe.js';
import './index-cDkf_GGD.js';
import './elsa-client-03-1ZsIW.js';
import './index-CLMl_isK.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './dashboard-a8oBUC8T.js';
import './index-C-8L13GY.js';
import './store-Dxa5xyVH.js';

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
//# sourceMappingURL=web-DQ4sUsdT.js.map

//# sourceMappingURL=web-DQ4sUsdT.js.map