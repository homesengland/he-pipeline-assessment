import { b as WebPlugin } from './credential-manager-plugin-CMMtbydv.js';
import './index-FqXAnKkK.js';
import './events-CpKc8CLe.js';
import './index-DFwgVhXo.js';
import './elsa-client-CbFnoFr9.js';
import './index-CL6j2ec2.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';
import './dashboard-DaK-DIvX.js';
import './index-C-8L13GY.js';
import './store-B_H_ZDGs.js';

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
//# sourceMappingURL=web-CM-betJh.js.map

//# sourceMappingURL=web-CM-betJh.js.map