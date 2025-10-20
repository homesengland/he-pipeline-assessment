import { r as registerInstance, a as createEvent, h } from './index-CL6j2ec2.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import { p as pluginManager, t as toastNotificationService, c as confirmDialogService, b as propertyDisplayManager, f as featuresDataManager } from './index-fZDMH_YE.js';
import './index-D7wXd6HU.js';
import { h as htmlToElement, g as getOrCreateProperty } from './utils-C0M_5Llz.js';
import { a as state } from './store-B_H_ZDGs.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import { b as createElsaClient, c as createHttpClient, a as activityIconProvider } from './elsa-client-q6ob5JPZ.js';
import './index-C-8L13GY.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './collection-B4sYCr2r.js';
import './fetch-client-1OcjQcrw.js';

const ElsaStudioRoot = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.initializing = createEvent(this, "initializing", 7);
        this.initialized = createEvent(this, "initialized", 7);
        this.basePath = '';
        this.useX6Graphs = false;
        this.onShowConfirmDialog = (e) => e.promise = this.confirmDialog.show(e.caption, e.message);
        this.onHideConfirmDialog = async () => await this.confirmDialog.hide();
        this.onShowToastNotification = async (e) => await this.toastNotificationElement.show(e);
        this.onHideToastNotification = async () => await this.toastNotificationElement.hide();
    }
    async addPlugins(pluginTypes) {
        pluginManager.registerPlugins(pluginTypes);
    }
    async addPlugin(pluginType) {
        pluginManager.registerPlugin(pluginType);
    }
    workflowChangedHandler(event) {
        eventBus.emit(EventTypes.WorkflowModelChanged, this, event.detail);
    }
    connectedCallback() {
        eventBus.on(EventTypes.ShowConfirmDialog, this.onShowConfirmDialog);
        eventBus.on(EventTypes.HideConfirmDialog, this.onHideConfirmDialog);
        eventBus.on(EventTypes.ShowToastNotification, this.onShowToastNotification);
        eventBus.on(EventTypes.HideToastNotification, this.onHideToastNotification);
    }
    disconnectedCallback() {
        eventBus.detach(EventTypes.ShowConfirmDialog, this.onShowConfirmDialog);
        eventBus.detach(EventTypes.HideConfirmDialog, this.onHideConfirmDialog);
        eventBus.detach(EventTypes.ShowToastNotification, this.onShowToastNotification);
        eventBus.detach(EventTypes.HideToastNotification, this.onHideToastNotification);
    }
    async componentWillLoad() {
        state.useX6Graphs = this.useX6Graphs;
        const elsaClientFactory = () => createElsaClient(this.serverUrl);
        const httpClientFactory = () => createHttpClient(this.serverUrl);
        if (this.config) {
            await fetch(`${document.location.origin}/${this.config}`)
                .then(response => {
                if (!response.ok) {
                    throw new Error("HTTP error " + response.status);
                }
                return response.json();
            })
                .then(data => {
                this.featuresConfig = data;
            }).catch((error) => {
                console.error(error);
            });
        }
        const elsaStudio = this.elsaStudio = {
            serverUrl: this.serverUrl,
            basePath: this.basePath,
            features: this.featuresConfig,
            serverFeatures: [],
            serverVersion: null,
            eventBus,
            pluginManager,
            propertyDisplayManager,
            activityIconProvider,
            confirmDialogService,
            toastNotificationService,
            elsaClientFactory,
            httpClientFactory,
            getOrCreateProperty: getOrCreateProperty,
            htmlToElement
        };
        this.initializing.emit(elsaStudio);
        pluginManager.initialize(elsaStudio);
        await eventBus.emit(EventTypes.Root.Initializing);
        propertyDisplayManager.initialize(elsaStudio);
        featuresDataManager.initialize(elsaStudio);
        const elsaClient = await elsaClientFactory();
        elsaStudio.serverFeatures = await elsaClient.featuresApi.list();
        elsaStudio.serverVersion = await elsaClient.versionApi.get();
    }
    async componentDidLoad() {
        this.initialized.emit(this.elsaStudio);
        await eventBus.emit(EventTypes.Root.Initialized);
    }
    render() {
        const culture = this.culture;
        const tunnelState = {
            serverUrl: this.serverUrl,
            basePath: this.basePath,
            serverFeatures: this.elsaStudio.serverFeatures,
            serverVersion: this.elsaStudio.serverVersion,
            culture,
            monacoLibPath: this.monacoLibPath
        };
        return (h(Tunnel.Provider, { key: 'c1e2f22c4147a5fbcc9011551f8f2ccd9a83ebb5', state: tunnelState }, h("slot", { key: '23d6e42abe72acfe12f768bc5c8e87ca9a8c7649' }), h("elsa-confirm-dialog", { key: 'c799c273c561b2e216bbf065ada7d71938175bff', ref: el => this.confirmDialog = el, culture: this.culture }), h("elsa-toast-notification", { key: '0226e1fe8f305dc3f1da61652ea55894f548e734', ref: el => this.toastNotificationElement = el })));
    }
};

export { ElsaStudioRoot as elsa_studio_root };
//# sourceMappingURL=elsa-studio-root.entry.esm.js.map
