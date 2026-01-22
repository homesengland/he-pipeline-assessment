import { b as WebPlugin } from './credential-manager-plugin-845ea5e6.js';
import './index-53e05b34.js';
import './active-router-16dd3465.js';
import './index-1542df5c.js';
import './index-2db7bf78.js';
import './match-path-02f6df12.js';
import './location-utils-6419c2b3.js';
import './index-1654a48d.js';
import './events-d0aab14a.js';
import './index-892f713d.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';
import './auth0-spa-js.production.esm-eb2e28a3.js';
import './dashboard-beb9b1e8.js';
import './store-8fc2fe8a.js';
import './index-0d4e8807.js';

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
