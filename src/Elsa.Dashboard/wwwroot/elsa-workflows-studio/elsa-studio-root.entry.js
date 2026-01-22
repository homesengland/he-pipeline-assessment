import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { T as Tunnel } from './dashboard-beb9b1e8.js';
import { p as pluginManager, b as propertyDisplayManager, c as confirmDialogService, t as toastNotificationService, f as featuresDataManager } from './index-892f713d.js';
import './index-1654a48d.js';
import { g as getOrCreateProperty, b as htmlToElement } from './utils-db96334c.js';
import { a as state } from './store-8fc2fe8a.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes } from './events-d0aab14a.js';
import { b as createElsaClient, c as createHttpClient, a as activityIconProvider } from './elsa-client-8304c78c.js';
import './index-2db7bf78.js';
import './fetch-client-f0dc2a52.js';
import './_commonjsHelpers-6cb8dacb.js';
import './cronstrue-37d55fa1.js';
import './collection-ba09a015.js';
import './index-0d4e8807.js';

const ElsaStudioRoot = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.initializing = createEvent(this, "initializing", 7);
    this.initialized = createEvent(this, "initialized", 7);
    this.onShowConfirmDialog = (e) => e.promise = this.confirmDialog.show(e.caption, e.message);
    this.onHideConfirmDialog = async () => await this.confirmDialog.hide();
    this.onShowToastNotification = async (e) => await this.toastNotificationElement.show(e);
    this.onHideToastNotification = async () => await this.toastNotificationElement.hide();
    this.serverUrl = undefined;
    this.monacoLibPath = undefined;
    this.culture = undefined;
    this.basePath = '';
    this.useX6Graphs = false;
    this.features = undefined;
    this.config = undefined;
    this.featuresConfig = undefined;
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
    return (h(Tunnel.Provider, { state: tunnelState }, h("slot", null), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el, culture: this.culture }), h("elsa-toast-notification", { ref: el => this.toastNotificationElement = el })));
  }
};

export { ElsaStudioRoot as elsa_studio_root };
