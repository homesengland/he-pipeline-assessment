import { r as registerInstance, a as createEvent, h, j as Host } from './index-CL6j2ec2.js';
import { i as injectHistory } from './index-Bs3-6O4N.js';
import { c as WorkflowTestActivityMessageStatus, a as WorkflowPersistenceBehavior } from './index-D7wXd6HU.js';
import { f as featuresDataManager, m as monacoEditorDialogService } from './index-fZDMH_YE.js';
import { a as state } from './store-B_H_ZDGs.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { T as Tunnel$1 } from './dashboard-DaK-DIvX.js';
import { L as LayoutDirection, W as WorkflowDesignerMode } from './models-0-8LsKgc.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { c as collectionExports } from './collection-B4sYCr2r.js';
import { b as createElsaClient } from './elsa-client-q6ob5JPZ.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import './active-router-B4NtjqjH.js';
import './index-C-8L13GY.js';
import './match-path-B_GgAMi4.js';
import './location-utils-Bznh9xeH.js';
import './utils-C0M_5Llz.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './cronstrue-BvVNjwUa.js';
import './fetch-client-1OcjQcrw.js';

function downloadFromUrl(url, options) {
    const anchorElement = document.createElement('a');
    anchorElement.href = url;
    anchorElement.download = options.fileName !== null && options.fileName !== void 0 ? options.fileName : '';
    anchorElement.click();
    anchorElement.remove();
}
function downloadFromBytes(content, options) {
    const contentType = options.contentType || 'application/octet-stream';
    const base64String = btoa(String.fromCharCode(...content));
    const url = `data:${contentType}";base64,${base64String}`;
    downloadFromUrl(url, { fileName: options.fileName });
}
function downloadFromText(content, options) {
    const utf8Encode = new TextEncoder();
    const bytes = utf8Encode.encode(content);
    downloadFromBytes(bytes, options);
}
function downloadFromBlob(content, options) {
    const url = window.URL.createObjectURL(content);
    downloadFromUrl(url, options);
}

const resources = {
    'en': {
        'default': {
            'Publishing': 'Publishing...',
            'Published': 'Published',
            "ActivityContextMenu": {
                "Edit": "Edit",
                "Delete": "Delete",
                "DeleteSelected": "Delete Selected"
            },
            "ConnectionContextMenu": {
                "Paste": "Paste",
            },
            'Restart': 'Restart',
            'DeleteConfirmationModel': {
                'Title': 'Delete Workflow Definition',
                'Message': 'Are you sure you wish to permanently delete this workflow, including all of its workflow instances?'
            }
        }
    },
    'zh-CN': {
        'default': {
            'Publishing': '发布中...',
            'Published': '已发布',
            "ActivityContextMenu": {
                "Edit": "编辑",
                "Delete": "删除",
                "DeleteSelected": "删除所选"
            },
            "ConnectionContextMenu": {
                "Paste": "粘贴",
            },
            'Restart': '重启',
            'DeleteConfirmationModel': {
                'Title': '删除工作流程定义',
                'Message': '你确定要永久删除这个工作流程，包括它的所有工作流程实例？'
            }
        }
    },
    'nl-NL': {
        'default': {
            'Publishing': 'Publiceren...',
            'Published': 'Gepubliceerd',
            "ActivityContextMenu": {
                "Edit": "Bewerken",
                "Delete": "Verwijderen",
                "DeleteSelected": "Verwijder Geselecteerde"
            },
            "ConnectionContextMenu": {
                "Paste": "Plakken",
            },
            'Restart': 'Herstarten',
            'DeleteConfirmationModel': {
                'Title': 'Verwijder Workflow Definitie',
                'Message': 'Weet je zeker dat je deze workflow permanent wilt verwijderen, inclusief alle bijbehorende workflow instanties?'
            }
        }
    },
    'fa-IR': {
        'default': {
            'Publishing': 'در حال انتشار...',
            'Published': 'منتشر شده',
            "ActivityContextMenu": {
                "Edit": "ویرایش",
                "Delete": "حذف",
                "DeleteSelected": "حذف موارد انتخاب شده"
            },
            "ConnectionContextMenu": {
                "Paste": "Paste",
            },
            'Restart': 'راه اندازی مجدد',
            'DeleteConfirmationModel': {
                'Title': 'حذف تعریف فرآیند',
                'Message': 'آیا از حذف این فرآیند و همه جریانهای آن اطمینان دارید?'
            }
        }
    },
    'de-DE': {
        'default': {
            'Publishing': 'Am veröffentlichen',
            'Published': 'Veröffentlicht',
            "ActivityContextMenu": {
                "Edit": "Bearbeiten",
                "Delete": "Entfernen",
                "DeleteSelected": "Ausgewählte entfernen"
            },
            "ConnectionContextMenu": {
                "Paste": "Einfügen",
            },
            'Restart': 'Neustarten',
            'DeleteConfirmationModel': {
                'Title': 'Ablaufdefinition entfernen',
                'Message': 'Sind Sie sicher, dass sie die Definition und die dazugehörigen Instanzen unwiderruflich löschen wollen?'
            }
        }
    },
};

const elsaWorkflowDefinitionEditorScreenCss = ".svg-loader{height:2rem;vertical-align:top}";

const ElsaWorkflowDefinitionEditorScreen = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.workflowSaved = createEvent(this, "workflowSaved", 7);
        this.serverFeatures = [];
        this.workflowTestActivityMessages = [];
        this.layoutDirection = LayoutDirection.TopBottom; //???
        this.activityContextMenuState = {
            shown: false,
            x: 0,
            y: 0,
            activity: null,
            selectedActivities: {},
        };
        // @State() connectionContextMenuState: ActivityContextMenuState = {
        //   shown: false,
        //   x: 0,
        //   y: 0,
        //   activity: null,
        // };
        this.activityContextMenuTestState = {
            shown: false,
            x: 0,
            y: 0,
            activity: null,
        };
        this.configureComponentCustomButtonContext = null;
        this.t = (key) => this.i18next.t(key);
        // onConnectionContextMenuButtonClicked(e: CustomEvent<ActivityContextMenuState>) {
        //   this.connectionContextMenuState = e.detail;
        // }
        this.onTestActivityMessageReceived = async (args) => {
            const message = args;
            if (!!message) {
                this.workflowInstanceId = message.workflowInstanceId;
                this.workflowTestActivityMessages = this.workflowTestActivityMessages.filter(x => x.activityId !== message.activityId);
                this.workflowTestActivityMessages = [...this.workflowTestActivityMessages, message];
            }
            else {
                this.workflowTestActivityMessages = [];
                this.workflowInstanceId = null;
            }
            this.render();
        };
        this.onUpdateWorkflowSettings = async (workflowDefinition) => {
            this.updateWorkflowDefinition(workflowDefinition);
            await this.saveWorkflowInternal(this.workflowModel);
        };
        this.onFlyoutPanelTabSelected = async (args) => {
            const tab = args;
            if (tab === 'general')
                this.workflowDesignerMode = WorkflowDesignerMode.Edit;
            if (tab === 'test')
                this.workflowDesignerMode = WorkflowDesignerMode.Test;
            this.render();
        };
        this.onUpdateActivity = (activity) => {
            const message = this.workflowTestActivityMessages.find(x => x.activityId === activity.activityId);
            if (message) {
                message.status = WorkflowTestActivityMessageStatus.Modified;
                this.clearSubsequentWorkflowTestMessages(activity.activityId);
            }
        };
        this.renderActivityStatsButton = (activity) => {
            const testActivityMessage = this.workflowTestActivityMessages.find(x => x.activityId == activity.activityId);
            if (testActivityMessage == undefined)
                return '';
            let icon;
            switch (testActivityMessage.status) {
                case WorkflowTestActivityMessageStatus.Done:
                default:
                    icon = `<svg class="elsa-h-8 elsa-w-8 elsa-text-green-500"  fill="none" viewBox="0 0 24 24" stroke="currentColor">
                  <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M9 12l2 2 4-4m6 2a9 9 0 11-18 0 9 9 0 0118 0z"/>
                </svg>`;
                    break;
                case WorkflowTestActivityMessageStatus.Waiting:
                    icon = `<svg version="1.1" class="svg-loader" xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" viewBox="0 0 80 80" xml:space="preserve">
                  <path id="spinner" fill="#7eb0de" d="M40,72C22.4,72,8,57.6,8,40C8,22.4,
                  22.4,8,40,8c17.6,0,32,14.4,32,32c0,1.1-0.9,2-2,2
                  s-2-0.9-2-2c0-15.4-12.6-28-28-28S12,24.6,12,40s12.6,
                  28,28,28c1.1,0,2,0.9,2,2S41.1,72,40,72z">
                    <animateTransform attributeType="xml" attributeName="transform" type="rotate" from="0 40 40" to="360 40 40" dur="0.75s" repeatCount="indefinite" />
                  </path>
                  </path>
              </svg>`;
                    break;
                case WorkflowTestActivityMessageStatus.Failed:
                    icon = `<svg class="elsa-h-8 elsa-w-8 elsa-text-red-500"  viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                  <circle cx="12" cy="12" r="10" />
                  <line x1="15" y1="9" x2="9" y2="15" />
                  <line x1="9" y1="9" x2="15" y2="15" />
                </svg>`;
                    break;
                case WorkflowTestActivityMessageStatus.Modified:
                    icon = `<svg class="h-6 w-6 elsa-text-yellow-500" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
                    <circle cx="12" cy="12" r="10"></circle>
                    <line x1="12" y1="16" x2="12" y2="12"></line>
                    <line x1="12" y1="8" x2="12.01" y2="8"></line>
                </svg>`;
                    break;
            }
            return `<div class="context-menu-wrapper elsa-flex-shrink-0">
            <button aria-haspopup="true"
                    class="elsa-w-8 elsa-h-8 elsa-inline-flex elsa-items-center elsa-justify-center elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
              ${icon}
            </button>
          </div>`;
        };
        this.renderTestActivityMenu = () => {
            const message = this.workflowTestActivityMessages.find(x => x.activityId == this.selectedActivityId);
            const renderActivityTestError = () => {
                var _a, _b;
                if (message == undefined || !message)
                    return;
                if (!message.error)
                    return;
                return (h("div", { class: "elsa-ml-4" }, h("elsa-workflow-fault-information", { workflowFault: (_a = this.workflowInstance) === null || _a === void 0 ? void 0 : _a.faults.find(x => x.faultedActivityId == this.selectedActivityId), faultedAt: (_b = this.workflowInstance) === null || _b === void 0 ? void 0 : _b.faultedAt })));
            };
            const renderPerformanceStats = () => {
                if (!!message.error)
                    return;
                return (h("div", { class: "elsa-ml-4" }, h("elsa-workflow-performance-information", { activityStats: this.activityStats })));
            };
            const renderRestartButton = () => {
                if (!this.canBeRestartedFromCurrentActivity())
                    return undefined;
                return (h("button", { type: "button", onClick: () => this.onRestartActivityButtonClick(), class: "elsa-ml-0 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-transparent elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-blue-600 elsa-text-base elsa-font-medium elsa-text-white hover:elsa-bg-red-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-red-500 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, this.t('Restart')));
            };
            const renderMessage = () => {
                var _a, _b;
                const t = this.t;
                if (message == undefined || !message)
                    return;
                this.configureComponentCustomButton(message);
                const filteredData = {};
                const wellKnownDataKeys = { State: true, Input: null, Outcomes: true, Exception: true };
                let dataKey = null;
                for (const key in message.data) {
                    if (!message.data.hasOwnProperty(key))
                        continue;
                    if (!!wellKnownDataKeys[key])
                        continue;
                    const value = message.data[key];
                    if (!value && value != 0)
                        continue;
                    let valueText = null;
                    dataKey = key;
                    if (typeof value == 'string')
                        valueText = value;
                    else if (typeof value == 'object')
                        valueText = JSON.stringify(value, null, 1);
                    else if (typeof value == 'undefined')
                        valueText = null;
                    else
                        valueText = value.toString();
                    filteredData[key] = valueText;
                }
                const hasBody = !!((_b = (_a = message.data) === null || _a === void 0 ? void 0 : _a.Input) === null || _b === void 0 ? void 0 : _b.Body);
                return (h("div", { class: "elsa-relative elsa-grid elsa-gap-6 elsa-bg-white px-5 elsa-py-6 sm:elsa-gap-8 sm:elsa-p-8" }, h("div", { class: "elsa-flex elsa-flex-row elsa-justify-between" }, h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, t('Status')), h("p", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, message.status)), h("div", null, renderRestartButton())), collectionExports.map(filteredData, (v, k) => (h("div", { class: "elsa-ml-4" }, h("p", { class: "elsa-text-base elsa-font-medium elsa-text-gray-900" }, k), h("pre", { class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500 elsa-overflow-x-auto", style: { "max-width": "30rem" } }, v)))), hasBody ? renderComponentCustomButton() : undefined, renderActivityTestError(), renderPerformanceStats()));
            };
            const renderComponentCustomButton = () => {
                if (this.configureComponentCustomButtonContext.data == null)
                    return;
                const label = this.configureComponentCustomButtonContext.data.label;
                return (h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("button", { type: "button", onClick: () => this.onComponentCustomButtonClick(message), class: "elsa-ml-0 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-transparent elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-blue-600 elsa-text-base elsa-font-medium elsa-text-white hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, label)));
            };
            const renderLoader = function () {
                return h("div", { class: "elsa-p-6 elsa-bg-white" }, "Loading...");
            };
            return (h("div", { "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-enter-end": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: `${this.activityContextMenuTestState.shown ? '' : 'hidden'} elsa-absolute elsa-z-10 elsa-mt-3 elsa-px-2 elsa-w-screen elsa-max-w-xl sm:elsa-px-0`, style: {
                    left: `${this.activityContextMenuTestState.x + 64}px`,
                    top: `${this.activityContextMenuTestState.y - 256}px`,
                }, ref: el => (this.componentCustomButton = el) }, h("div", { class: "elsa-rounded-lg elsa-shadow-lg elsa-ring-1 elsa-ring-black elsa-ring-opacity-5 elsa-overflow-x-hidden elsa-overflow-y-auto", style: { "max-height": "700px" } }, !!message ? renderMessage() : renderLoader())));
        };
        this.showHelpModal = async () => {
            await this.helpDialog.show();
        };
        this.renderDesignerPanel = () => {
            const isFeaturePanelVisible = featuresDataManager.getUIFeatureList().length != 0;
            if (isFeaturePanelVisible) {
                return [
                    h("elsa-tab-header", { tab: "designer", slot: "header" }, "Designer"),
                    h("elsa-tab-content", { tab: "designer", slot: "content" }, h("elsa-designer-panel", { onFeatureChanged: this.handleFeatureChange, onFeatureStatusChanged: this.handleFeatureStatusChange })),
                ];
            }
        };
        this.renderVersionHistoryPanel = (workflowDefinition) => {
            return [
                h("elsa-tab-header", { tab: "versionHistory", slot: "header" }, "Version History"),
                h("elsa-tab-content", { tab: "versionHistory", slot: "content" }, h("elsa-version-history-panel", { workflowDefinition: workflowDefinition, onVersionSelected: e => this.onVersionSelected(e), onDeleteVersionClicked: e => this.onDeleteVersionClicked(e), onRevertVersionClicked: e => this.onRevertVersionClicked(e) })),
            ];
        };
        this.handleFeatureChange = (e) => {
            const feature = e.detail;
            if (feature === featuresDataManager.supportedFeatures.workflowLayout) {
                const layoutFeature = featuresDataManager.getFeatureConfig(feature);
                this.layoutDirection = layoutFeature.value;
            }
        };
        this.handleFeatureStatusChange = (e) => {
            const feature = e.detail;
            if (feature === featuresDataManager.supportedFeatures.workflowLayout) {
                const layoutFeature = featuresDataManager.getFeatureConfig(feature);
                if (layoutFeature.enabled) {
                    this.layoutDirection = layoutFeature.value;
                }
                else {
                    this.layoutDirection = LayoutDirection.TopBottom;
                }
            }
        };
        this.onVersionSelected = async (e) => {
            const client = await createElsaClient(this.serverUrl);
            const version = e.detail;
            const workflowDefinition = await client.workflowDefinitionsApi.getByDefinitionAndVersion(version.definitionId, { version: version.version });
            this.updateWorkflowDefinition(workflowDefinition);
        };
        this.onDeleteVersionClicked = async (e) => {
            const client = await createElsaClient(this.serverUrl);
            const version = e.detail;
            await client.workflowDefinitionsApi.delete(version.definitionId, { version: version.version });
            this.updateWorkflowDefinition(Object.assign({}, this.workflowDefinition)); // Force a rerender.
        };
        this.onRevertVersionClicked = async (e) => {
            const client = await createElsaClient(this.serverUrl);
            const version = e.detail;
            const workflowDefinition = await client.workflowDefinitionsApi.revert(version.definitionId, version.version);
            this.updateWorkflowDefinition(workflowDefinition);
        };
    }
    //connectionContextMenu: HTMLDivElement;
    async getServerUrl() {
        return this.serverUrl;
    }
    async getWorkflowDefinitionId() {
        return this.workflowDefinition.definitionId;
    }
    async exportWorkflow() {
        const client = await createElsaClient(this.serverUrl);
        const workflowDefinition = this.workflowDefinition;
        const versionOptions = { version: workflowDefinition.version };
        const response = await client.workflowDefinitionsApi.export(workflowDefinition.definitionId, versionOptions);
        downloadFromBlob(response.data, { contentType: 'application/json', fileName: response.fileName });
    }
    async importWorkflow(file) {
        const client = await createElsaClient(this.serverUrl);
        this.importing = true;
        this.imported = false;
        this.networkError = null;
        try {
            const workflowDefinition = await client.workflowDefinitionsApi.import(this.workflowDefinition.definitionId, file);
            this.workflowDefinition = workflowDefinition;
            this.workflowModel = this.mapWorkflowModel(workflowDefinition);
            this.updateUrl(workflowDefinition.definitionId);
            this.importing = false;
            this.imported = true;
            setTimeout(() => (this.imported = false), 500);
            await eventBus.emit(EventTypes.WorkflowImported, this, this.workflowModel);
        }
        catch (e) {
            console.error(e);
            this.importing = false;
            this.imported = false;
            this.networkError = e.message;
            setTimeout(() => (this.networkError = null), 10000);
        }
    }
    async workflowDefinitionIdChangedHandler(newValue) {
        const workflowDefinitionId = newValue;
        let workflowDefinition = ElsaWorkflowDefinitionEditorScreen.createWorkflowDefinition();
        workflowDefinition.definitionId = workflowDefinitionId;
        const client = await createElsaClient(this.serverUrl);
        if (workflowDefinitionId && workflowDefinitionId.length > 0) {
            try {
                workflowDefinition = await client.workflowDefinitionsApi.getByDefinitionAndVersion(workflowDefinitionId, { isLatest: true });
            }
            catch (_a) {
                console.warn(`The specified workflow definition does not exist. Creating a new one.`);
            }
        }
        this.updateWorkflowDefinition(workflowDefinition);
    }
    async serverUrlChangedHandler(newValue) {
        if (newValue && newValue.length > 0) {
            await this.loadActivityDescriptors();
            await this.loadWorkflowStorageDescriptors();
        }
    }
    async monacoLibPathChangedHandler(newValue) {
        state.monacoLibPath = newValue;
    }
    async workflowChangedHandler(event) {
        const workflowModel = event.detail;
        await this.saveWorkflowInternal(workflowModel);
    }
    onWindowClicked(event) {
        const target = event.target;
        if (!this.componentCustomButton.contains(target))
            this.handleContextMenuTestChange(0, 0, false, null);
        if (!this.activityContextMenu.contains(target))
            this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null, selectedActivities: {} });
        // if (!this.connectionContextMenu.contains(target))
        //   this.handleConnectionContextMenuChange({x: 0, y: 0, shown: false, activity: null});
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
        this.workflowDesignerMode = WorkflowDesignerMode.Edit;
        const layoutFeature = featuresDataManager.getFeatureConfig(featuresDataManager.supportedFeatures.workflowLayout);
        if (layoutFeature && layoutFeature.enabled) {
            this.layoutDirection = layoutFeature.value;
        }
        await this.serverUrlChangedHandler(this.serverUrl);
        await this.workflowDefinitionIdChangedHandler(this.workflowDefinitionId);
        await this.monacoLibPathChangedHandler(this.monacoLibPath);
    }
    async componentDidLoad() {
        if (!this.designer) {
            if (state.useX6Graphs) {
                this.designer = this.el.querySelector("x6-designer");
            }
            else {
                this.designer = this.el.querySelector('elsa-designer-tree');
            }
            this.designer.model = this.workflowModel;
        }
    }
    componentDidRender() {
        var _a;
        if (this.el && this.componentCustomButton) {
            let modalX = this.activityContextMenuTestState.x + 64;
            let modalY = this.activityContextMenuTestState.y - 256;
            // Fit the modal to the canvas bounds
            const canvasBounds = (_a = this.el) === null || _a === void 0 ? void 0 : _a.getBoundingClientRect();
            const modalBounds = this.componentCustomButton.getBoundingClientRect();
            const modalWidth = modalBounds === null || modalBounds === void 0 ? void 0 : modalBounds.width;
            const modalHeight = modalBounds === null || modalBounds === void 0 ? void 0 : modalBounds.height;
            modalX = Math.min(canvasBounds.width, modalX + modalWidth + 32) - modalWidth - 32;
            modalY = Math.min(canvasBounds.height, modalY + modalHeight) - modalHeight - 32;
            modalY = Math.max(0, modalY);
            this.componentCustomButton.style.left = `${modalX}px`;
            this.componentCustomButton.style.top = `${modalY}px`;
        }
    }
    connectedCallback() {
        eventBus.on(EventTypes.UpdateWorkflowSettings, this.onUpdateWorkflowSettings);
        eventBus.on(EventTypes.FlyoutPanelTabSelected, this.onFlyoutPanelTabSelected);
        eventBus.on(EventTypes.TestActivityMessageReceived, this.onTestActivityMessageReceived);
        eventBus.on(EventTypes.UpdateActivity, this.onUpdateActivity);
    }
    disconnectedCallback() {
        eventBus.detach(EventTypes.UpdateWorkflowSettings, this.onUpdateWorkflowSettings);
        eventBus.detach(EventTypes.FlyoutPanelTabSelected, this.onFlyoutPanelTabSelected);
        eventBus.detach(EventTypes.TestActivityMessageReceived, this.onTestActivityMessageReceived);
        eventBus.detach(EventTypes.UpdateActivity, this.onUpdateActivity);
    }
    async configureComponentCustomButton(message) {
        this.configureComponentCustomButtonContext = {
            component: 'elsa-workflow-definition-editor-screen',
            activityType: message.activityType,
            prop: null,
            data: null,
        };
        await eventBus.emit(EventTypes.ComponentLoadingCustomButton, this, this.configureComponentCustomButtonContext);
    }
    async loadActivityDescriptors() {
        const client = await createElsaClient(this.serverUrl);
        state.activityDescriptors = await client.activitiesApi.list();
    }
    async loadWorkflowStorageDescriptors() {
        const client = await createElsaClient(this.serverUrl);
        state.workflowStorageDescriptors = await client.workflowStorageProvidersApi.list();
    }
    async tryUpdateActivityInformation(activityId) {
        if (!this.workflowInstanceId) {
            this.activityStats = null;
            this.workflowInstance = null;
            return;
        }
        const client = await createElsaClient(this.serverUrl);
        this.activityStats = await client.activityStatsApi.get(this.workflowInstanceId, activityId);
        if (!this.workflowInstance || this.workflowInstance.id !== this.workflowInstanceId)
            this.workflowInstance = await client.workflowInstancesApi.get(this.workflowInstanceId);
    }
    updateWorkflowDefinition(value) {
        this.workflowDefinition = value;
        this.workflowModel = this.mapWorkflowModel(value);
    }
    async publishWorkflow() {
        this.publishing = true;
        await this.saveWorkflow(true);
        this.publishing = false;
        await eventBus.emit(EventTypes.WorkflowPublished, this, this.workflowDefinition);
    }
    async unpublishWorkflow() {
        await this.unpublishWorkflowInternal();
        await eventBus.emit(EventTypes.WorkflowRetracted, this, this.workflowDefinition);
    }
    async revertWorkflow() {
        await this.revertWorkflowInternal();
    }
    async saveWorkflow(publish) {
        await this.saveWorkflowInternal(null, publish);
    }
    async saveWorkflowInternal(workflowModel, publish) {
        if (!this.serverUrl || this.serverUrl.length == 0)
            return;
        workflowModel = workflowModel || this.workflowModel;
        const client = await createElsaClient(this.serverUrl);
        let workflowDefinition = this.workflowDefinition;
        const isNew = typeof workflowDefinition.definitionId === 'undefined' && typeof this.workflowDefinitionId === 'undefined';
        const request = {
            workflowDefinitionId: workflowDefinition.definitionId || this.workflowDefinitionId,
            contextOptions: workflowDefinition.contextOptions,
            deleteCompletedInstances: workflowDefinition.deleteCompletedInstances,
            description: workflowDefinition.description,
            displayName: workflowDefinition.displayName,
            isSingleton: workflowDefinition.isSingleton,
            name: workflowDefinition.name,
            tag: workflowDefinition.tag,
            channel: workflowDefinition.channel,
            persistenceBehavior: workflowDefinition.persistenceBehavior,
            publish: publish || false,
            variables: workflowDefinition.variables,
            activities: workflowModel.activities.map(x => ({
                activityId: x.activityId,
                type: x.type,
                name: x.name,
                displayName: x.displayName,
                description: x.description,
                x: x.x,
                y: x.y,
                persistWorkflow: x.persistWorkflow,
                loadWorkflowContext: x.loadWorkflowContext,
                saveWorkflowContext: x.saveWorkflowContext,
                properties: x.properties,
                propertyStorageProviders: x.propertyStorageProviders,
                category: '',
            })),
            connections: workflowModel.connections.map(x => ({
                sourceActivityId: x.sourceId,
                targetActivityId: x.targetId,
                outcome: x.outcome,
            })),
        };
        this.saving = !publish;
        this.publishing = publish;
        try {
            console.debug('Saving workflow...');
            workflowDefinition = await client.workflowDefinitionsApi.save(request);
            this.workflowDefinition = workflowDefinition;
            this.workflowModel = this.mapWorkflowModel(workflowDefinition);
            this.saving = false;
            this.saved = !publish;
            this.publishing = false;
            setTimeout(() => (this.saved = false), 500);
            this.workflowSaved.emit(workflowDefinition);
            if (isNew) {
                this.updateUrl(workflowDefinition.definitionId);
            }
        }
        catch (e) {
            console.error(e);
            this.saving = false;
            this.saved = false;
            this.networkError = e.message;
            setTimeout(() => (this.networkError = null), 10000);
        }
    }
    async unpublishWorkflowInternal() {
        const client = await createElsaClient(this.serverUrl);
        const workflowDefinitionId = this.workflowDefinition.definitionId;
        this.unPublishing = true;
        try {
            this.workflowDefinition = await client.workflowDefinitionsApi.retract(workflowDefinitionId);
            this.unPublishing = false;
            this.unPublished = true;
            setTimeout(() => (this.unPublished = false), 500);
        }
        catch (e) {
            console.error(e);
            this.unPublishing = false;
            this.unPublished = false;
            this.networkError = e.message;
            setTimeout(() => (this.networkError = null), 2000);
        }
    }
    async revertWorkflowInternal() {
        const client = await createElsaClient(this.serverUrl);
        const workflowDefinitionId = this.workflowDefinition.definitionId;
        const version = this.workflowDefinition.version;
        this.reverting = true;
        try {
            this.workflowDefinition = await client.workflowDefinitionsApi.revert(workflowDefinitionId, version);
            this.reverting = false;
            this.reverted = true;
            setTimeout(() => (this.reverted = false), 500);
        }
        catch (e) {
            console.error(e);
            this.reverting = false;
            this.reverted = false;
            this.networkError = e.message;
            setTimeout(() => (this.networkError = null), 2000);
        }
    }
    updateUrl(id) {
        if (this.history) {
            this.history.push(`${this.basePath}/workflow-definitions/${id}`, {});
        }
    }
    mapWorkflowModel(workflowDefinition) {
        return {
            activities: workflowDefinition.activities.map(this.mapActivityModel),
            connections: workflowDefinition.connections.map(this.mapConnectionModel),
            persistenceBehavior: workflowDefinition.persistenceBehavior,
        };
    }
    mapActivityModel(source) {
        const activityDescriptors = state.activityDescriptors;
        const activityDescriptor = activityDescriptors.find(x => x.type == source.type);
        const outcomes = !!activityDescriptor ? activityDescriptor.outcomes : ['Done'];
        return {
            activityId: source.activityId,
            description: source.description,
            displayName: source.displayName,
            x: source.x,
            y: source.y,
            name: source.name,
            type: source.type,
            properties: source.properties,
            outcomes: [...outcomes],
            persistWorkflow: source.persistWorkflow,
            saveWorkflowContext: source.saveWorkflowContext,
            loadWorkflowContext: source.loadWorkflowContext,
            propertyStorageProviders: source.propertyStorageProviders,
        };
    }
    mapConnectionModel(source) {
        return {
            sourceId: source.sourceActivityId,
            targetId: source.targetActivityId,
            outcome: source.outcome,
        };
    }
    async handleContextMenuChange(state) {
        this.activityContextMenuState = state;
    }
    // handleConnectionContextMenuChange(state: ActivityContextMenuState) {
    //   this.connectionContextMenuState = state;
    // }
    handleContextMenuTestChange(x, y, shown, activity) {
        this.activityContextMenuTestState = {
            shown,
            x,
            y,
            activity,
        };
    }
    async onShowWorkflowSettingsClick() {
        await eventBus.emit(EventTypes.ShowWorkflowSettings);
    }
    async onDeleteClicked() {
        const t = this.t;
        const result = await this.confirmDialog.show(t('DeleteConfirmationModel.Title'), t('DeleteConfirmationModel.Message'));
        if (!result)
            return;
        const elsaClient = await createElsaClient(this.serverUrl);
        await elsaClient.workflowDefinitionsApi.delete(this.workflowDefinition.definitionId, { allVersions: true });
        this.history.push(`${this.basePath}/workflow-definitions`, {});
    }
    async onPublishClicked() {
        await this.publishWorkflow();
    }
    async onUnPublishClicked() {
        await this.unpublishWorkflow();
    }
    async onRevertClicked() {
        await this.revertWorkflow();
    }
    async onExportClicked() {
        await this.exportWorkflow();
    }
    async onImportClicked(file) {
        await this.importWorkflow(file);
    }
    async onDeleteActivityClick(e) {
        e.preventDefault();
        const { activity, selectedActivities } = this.activityContextMenuState;
        if (selectedActivities[activity.activityId]) {
            await this.designer.removeSelectedActivities();
        }
        else {
            await this.designer.removeActivity(activity);
        }
        this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null, selectedActivities: {} });
        await eventBus.emit(EventTypes.HideModalDialog);
    }
    async onEditActivityClick(e) {
        e.preventDefault();
        await this.designer.showActivityEditor(this.activityContextMenuState.activity, true);
        this.handleContextMenuChange({ x: 0, y: 0, shown: false, activity: null });
    }
    // async onPasteActivityClick(e: Event) {
    //   e.preventDefault();
    //   let activityModel = this.connectionContextMenuState.activity;
    //   await eventBus.emit(EventTypes.PasteActivity, this, activityModel);
    //   this.handleConnectionContextMenuChange({x: 0, y: 0, shown: false, activity: null});
    // }
    async onActivityContextMenuButtonClicked(e) {
        this.activityContextMenuState = e.detail;
    }
    async onActivityContextMenuButtonTestClicked(e) {
        this.activityContextMenuTestState = e.detail;
        this.selectedActivityId = e.detail.activity.activityId;
        if (!e.detail.shown) {
            return;
        }
        await this.tryUpdateActivityInformation(this.selectedActivityId);
    }
    async onActivitySelected(e) {
        this.selectedActivityId = e.detail.activityId;
    }
    async onActivityDeselected(e) {
        if (this.selectedActivityId == e.detail.activityId)
            this.selectedActivityId = null;
    }
    async onRestartActivityButtonClick() {
        await eventBus.emit(EventTypes.WorkflowRestarted, this, this.selectedActivityId);
        this.handleContextMenuTestChange(0, 0, false, null);
    }
    clearSubsequentWorkflowTestMessages(activityId) {
        var _a;
        const targetActivityId = (_a = this.workflowDefinition.connections.find(x => x.sourceActivityId === activityId)) === null || _a === void 0 ? void 0 : _a.targetActivityId;
        if (!targetActivityId)
            return;
        this.workflowTestActivityMessages = this.workflowTestActivityMessages.filter(x => x.activityId !== targetActivityId || x.status === WorkflowTestActivityMessageStatus.Failed);
        this.clearSubsequentWorkflowTestMessages(targetActivityId);
    }
    renderMonacoEditorDialog() {
        return (h("elsa-modal-dialog", { ref: el => {
                monacoEditorDialogService.monacoEditorDialog = el;
            } }, h("div", { slot: "content", class: "elsa-py-8 elsa-px-4" }, h("elsa-monaco", { value: "", language: "javascript", "editor-height": "400px", "single-line": false, onValueChanged: e => {
                monacoEditorDialogService.currentValue = e.detail.value;
            }, ref: el => (monacoEditorDialogService.monacoEditor = el) })), h("div", { slot: "buttons" }, h("div", { class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { type: "button", onClick: () => {
                monacoEditorDialogService.save();
                monacoEditorDialogService.monacoEditorDialog.hide();
            }, class: "elsa-ml-3 elsa-inline-flex elsa-justify-center elsa-py-2 elsa-px-4 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, "Save"), h("button", { type: "button", onClick: () => {
                monacoEditorDialogService.monacoEditorDialog.hide();
            }, class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, "Cancel")))));
    }
    render() {
        const tunnelState = {
            serverUrl: this.serverUrl,
            workflowDefinitionId: this.workflowDefinition.definitionId,
            serverFeatures: this.serverFeatures,
        };
        return (h(Host, { key: 'fc9439e5afb7598471dc5bc5a68a3945f489eedb', class: "elsa-flex elsa-flex-col elsa-w-full", ref: el => (this.el = el) }, h(Tunnel.Provider, { key: 'c13efaea7e95227812e72113c4ed36e99fbfd69b', state: tunnelState }, this.renderCanvas(), this.renderActivityPicker(), this.renderActivityEditor(), this.renderMonacoEditorDialog())));
    }
    renderCanvas() {
        const activityContextMenuButton = (activity) => `<div class="context-menu-wrapper elsa-flex-shrink-0">
            <button aria-haspopup="true"
                    class="elsa-w-8 elsa-h-8 elsa-inline-flex elsa-items-center elsa-justify-center elsa-text-gray-400 elsa-rounded-full elsa-bg-transparent hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-text-gray-500 focus:elsa-bg-gray-100 elsa-transition elsa-ease-in-out elsa-duration-150">
              <svg class="elsa-h-6 elsa-w-6 elsa-text-gray-400" width="24" height="24" viewBox="0 0 24 24" stroke-width="2"
                   stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
                <path stroke="none" d="M0 0h24v24H0z"/>
                <circle cx="5" cy="12" r="1"/>
                <circle cx="12" cy="12" r="1"/>
                <circle cx="19" cy="12" r="1"/>
              </svg>
            </button>
          </div>`;
        return (h("div", { class: "elsa-flex-1 elsa-flex elsa-relative" }, !state.useX6Graphs && (h("elsa-designer-tree", { model: this.workflowModel, mode: this.workflowDesignerMode, layoutDirection: this.layoutDirection, activityContextMenuButton: this.workflowDesignerMode == WorkflowDesignerMode.Edit ? activityContextMenuButton : this.renderActivityStatsButton, onActivityContextMenuButtonClicked: e => this.onActivityContextMenuButtonClicked(e), onActivityContextMenuButtonTestClicked: e => this.onActivityContextMenuButtonTestClicked(e), activityContextMenu: this.workflowDesignerMode == WorkflowDesignerMode.Edit ? this.activityContextMenuState : this.activityContextMenuTestState, enableMultipleConnectionsFromSingleSource: false, selectedActivityIds: [this.selectedActivityId], onActivitySelected: e => this.onActivitySelected(e), onActivityDeselected: e => this.onActivityDeselected(e), class: "elsa-flex-1", ref: el => (this.designer = el) })), state.useX6Graphs && (h("x6-designer", { model: this.workflowModel, mode: this.workflowDesignerMode, layoutDirection: this.layoutDirection, activityContextMenuButton: this.workflowDesignerMode == WorkflowDesignerMode.Edit ? (() => '') : this.renderActivityStatsButton, onActivityContextMenuButtonClicked: e => this.onActivityContextMenuButtonClicked(e), onActivityContextMenuButtonTestClicked: e => this.onActivityContextMenuButtonTestClicked(e), activityContextMenu: this.workflowDesignerMode == WorkflowDesignerMode.Edit ? this.activityContextMenuState : this.activityContextMenuTestState, enableMultipleConnectionsFromSingleSource: false, selectedActivityIds: [this.selectedActivityId], onActivitySelected: e => this.onActivitySelected(e), onActivityDeselected: e => this.onActivityDeselected(e), class: "elsa-workflow-wrapper", ref: el => (this.designer = el) })), this.renderWorkflowSettingsButton(), this.renderWorkflowHelpButton(), this.renderWorkflowPanel(), this.renderActivityContextMenu(), h("elsa-workflow-settings-modal", { workflowDefinition: this.workflowDefinition }), h("elsa-workflow-definition-editor-notifications", null), h("div", { class: "elsa-fixed elsa-bottom-10 elsa-right-12" }, h("div", { class: "elsa-flex elsa-items-center elsa-space-x-4" }, this.renderSavingIndicator(), this.renderNetworkError(), this.renderPublishButton())), this.renderTestActivityMenu(), h("elsa-confirm-dialog", { ref: el => this.confirmDialog = el, culture: this.culture })));
    }
    async onComponentCustomButtonClick(message) {
        let workflowModel = Object.assign({}, this.workflowModel);
        const activityModel = workflowModel.activities.find(x => x.activityId == message.activityId);
        const input = message.data['Input'];
        const componentCustomButtonClickContext = {
            component: 'elsa-workflow-definition-editor-screen',
            activityType: message.activityType,
            prop: null,
            params: [activityModel, input],
        };
        await eventBus.emit(EventTypes.ComponentCustomButtonClick, this, componentCustomButtonClickContext);
    }
    canBeRestartedFromCurrentActivity() {
        var _a;
        if (!this.selectedActivityId)
            return false;
        if (this.workflowDesignerMode !== WorkflowDesignerMode.Test)
            return false;
        if (this.workflowTestActivityMessages.some(x => x.activityId === this.selectedActivityId))
            return true;
        const sourceActivityId = (_a = this.workflowDefinition.connections.find(x => x.targetActivityId === this.selectedActivityId)) === null || _a === void 0 ? void 0 : _a.sourceActivityId;
        return sourceActivityId && this.workflowTestActivityMessages.some(x => x.activityId === sourceActivityId && x.status !== WorkflowTestActivityMessageStatus.Failed);
    }
    renderActivityContextMenu() {
        var _a;
        const t = this.t;
        const selectedActivities = Object.keys((_a = this.activityContextMenuState.selectedActivities) !== null && _a !== void 0 ? _a : {});
        const { activity } = this.activityContextMenuState;
        return (h("div", { "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-enter-end": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: `${this.activityContextMenuState.shown ? '' : 'hidden'} context-menu elsa-z-20 elsa-mx-3 elsa-w-48 elsa-mt-1 elsa-rounded-md elsa-shadow-lg elsa-fixed`, style: { left: `${this.activityContextMenuState.x}px`, top: `${this.activityContextMenuState.y}px` }, ref: el => (this.activityContextMenu = el) }, h("div", { class: "elsa-rounded-md elsa-bg-white elsa-shadow-xs", role: "menu", "aria-orientation": "vertical", "aria-labelledby": "pinned-project-options-menu-0" }, h("div", { class: "elsa-py-1" }, h("a", { onClick: e => this.onEditActivityClick(e), href: "#", class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-leading-5 elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900 focus:elsa-outline-none focus:elsa-bg-gray-100 focus:elsa-text-gray-900", role: "menuitem" }, t('ActivityContextMenu.Edit'))), h("div", { class: "elsa-border-t elsa-border-gray-100" }), h("div", { class: "elsa-py-1" }, h("a", { onClick: e => this.onDeleteActivityClick(e), href: "#", class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-leading-5 elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900 focus:elsa-outline-none focus:elsa-bg-gray-100 focus:elsa-text-gray-900", role: "menuitem" }, selectedActivities.length > 1 && selectedActivities.indexOf(activity.activityId) !== -1 ? t('ActivityContextMenu.DeleteSelected') : t('ActivityContextMenu.Delete'))))));
    }
    // renderConnectionContextMenu() {
    //   const t = this.t;
    //
    //   return <div
    //     data-transition-enter="elsa-transition elsa-ease-out elsa-duration-100"
    //     data-transition-enter-start="elsa-transform elsa-opacity-0 elsa-scale-95"
    //     data-transition-enter-end="elsa-transform elsa-opacity-100 elsa-scale-100"
    //     data-transition-leave="elsa-transition elsa-ease-in elsa-duration-75"
    //     data-transition-leave-start="elsa-transform elsa-opacity-100 elsa-scale-100"
    //     data-transition-leave-end="elsa-transform elsa-opacity-0 elsa-scale-95"
    //     class={`${this.connectionContextMenuState.shown ? '' : 'hidden'} context-menu elsa-z-10 elsa-mx-3 elsa-w-48 elsa-mt-1 elsa-rounded-md elsa-shadow-lg elsa-absolute`}
    //     style={{left: `${this.connectionContextMenuState.x}px`, top: `${this.connectionContextMenuState.y - 64}px`}}
    //     ref={el => this.connectionContextMenu = el}
    //   >
    //     <div class="elsa-rounded-md elsa-bg-white elsa-shadow-xs" role="menu" aria-orientation="vertical"
    //          aria-labelledby="pinned-project-options-menu-0">
    //       <div class="elsa-py-1">
    //         <a
    //           onClick={e => this.onPasteActivityClick(e)}
    //           href="#"
    //           class="elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-leading-5 elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900 focus:elsa-outline-none focus:elsa-bg-gray-100 focus:elsa-text-gray-900"
    //           role="menuitem">
    //           {t('ConnectionContextMenu.Paste')}
    //         </a>
    //       </div>
    //     </div>
    //   </div>
    // }
    renderActivityPicker() {
        return h("elsa-activity-picker-modal", null);
    }
    renderActivityEditor() {
        return h("elsa-activity-editor-modal", { culture: this.culture });
    }
    renderWorkflowSettingsButton() {
        return (h("button", { onClick: () => this.onShowWorkflowSettingsClick(), type: "button", class: "workflow-settings-button elsa-fixed elsa-top-20 elsa-right-12 elsa-inline-flex elsa-items-center elsa-p-2 elsa-rounded-full elsa-border elsa-border-transparent elsa-bg-white shadow elsa-text-gray-400 hover:elsa-text-blue-500 focus:elsa-text-blue-500 hover:elsa-ring-2 hover:elsa-ring-offset-2 hover:elsa-ring-blue-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, h("svg", { xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 24 24", stroke: "currentColor", fill: "none", class: "elsa-h-8 elsa-w-8" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z" }), h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M15 12a3 3 0 11-6 0 3 3 0 016 0z" }))));
    }
    renderWorkflowHelpButton() {
        return (h("span", null, h("button", { type: "button", onClick: this.showHelpModal, class: "workflow-settings-button elsa-fixed elsa-top-20 elsa-right-28 elsa-inline-flex elsa-items-center elsa-p-2 elsa-rounded-full elsa-border elsa-border-transparent elsa-bg-white shadow elsa-text-gray-400 hover:elsa-text-blue-500 focus:elsa-text-blue-500 hover:elsa-ring-2 hover:elsa-ring-offset-2 hover:elsa-ring-blue-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, h("svg", { xmlns: "http://www.w3.org/2000/svg", class: "elsa-h-8 elsa-w-8", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor" }, h("path", { "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z" }))), h("elsa-modal-dialog", { ref: el => (this.helpDialog = el) }, !state.useX6Graphs && (h("div", { slot: "content", class: "elsa-p-8" }, h("h3", { class: "elsa-text-lg elsa-font-medium" }, "Actions"), h("dl", { class: "elsa-mt-2 elsa-border-t elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Delete connections"), h("dd", { class: "elsa-text-gray-900" }, "RIGHT-click the connection to delete.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Connect outcomes to existing activity"), h("dd", { class: "elsa-text-gray-900" }, "Press and hold SHIFT while LEFT-clicking the outcome to connect. Release SHIFT and LEFT-click the target activity.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Pan"), h("dd", { class: "elsa-text-gray-900" }, "Click anywhere on the designer and drag mouse.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Zoom"), h("dd", { class: "elsa-text-gray-900" }, "Use scroll-wheel on mouse."))))), state.useX6Graphs && (h("div", { slot: "content", class: "elsa-p-8" }, h("h3", { class: "elsa-text-lg elsa-font-medium" }, "Actions"), h("dl", { class: "elsa-mt-2 elsa-border-t elsa-border-b elsa-border-gray-200 elsa-divide-y elsa-divide-gray-200" }, h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Delete connections"), h("dd", { class: "elsa-text-gray-900" }, "Hover on the connection and click the red X.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Edit activities"), h("dd", { class: "elsa-text-gray-900" }, "Right click on the activity node and click Edit.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Pan"), h("dd", { class: "elsa-text-gray-900" }, "Hold Ctrl, click anywhere on the designer and drag mouse.")), h("div", { class: "elsa-py-3 elsa-flex elsa-justify-between elsa-text-sm elsa-font-medium" }, h("dt", { class: "elsa-text-gray-500" }, "Zoom"), h("dd", { class: "elsa-text-gray-900" }, "Use scroll-wheel on mouse while holding Ctrl."))))))));
    }
    renderSavingIndicator() {
        if (this.publishing)
            return undefined;
        const t = this.t;
        const message = this.unPublishing
            ? t('Unpublishing...')
            : this.unPublished
                ? t('Unpublished')
                : this.saving
                    ? 'Saving...'
                    : this.saved
                        ? 'Saved'
                        : this.importing
                            ? 'Importing...'
                            : this.imported
                                ? 'Imported'
                                : null;
        if (!message)
            return undefined;
        return (h("div", null, h("span", { class: "elsa-text-gray-400 elsa-text-sm" }, message)));
    }
    renderNetworkError() {
        if (!this.networkError)
            return undefined;
        return (h("div", null, h("span", { class: "elsa-text-rose-400 elsa-text-sm" }, "An error occurred: ", this.networkError)));
    }
    renderPublishButton() {
        return (h("elsa-workflow-publish-button", { publishing: this.publishing, workflowDefinition: this.workflowDefinition, onPublishClicked: () => this.onPublishClicked(), onUnPublishClicked: () => this.onUnPublishClicked(), onRevertClicked: () => this.onRevertClicked(), onExportClicked: () => this.onExportClicked(), onImportClicked: e => this.onImportClicked(e.detail), onDeleteClicked: e => this.onDeleteClicked(), culture: this.culture }));
    }
    static createWorkflowDefinition() {
        return {
            definitionId: null,
            version: 1,
            isLatest: true,
            isPublished: false,
            activities: [],
            connections: [],
            persistenceBehavior: WorkflowPersistenceBehavior.WorkflowBurst,
        };
    }
    renderWorkflowPanel() {
        const workflowDefinition = this.workflowDefinition;
        return (h("elsa-flyout-panel", { expandButtonPosition: 3 }, h("elsa-tab-header", { tab: "general", slot: "header" }, "General"), h("elsa-tab-content", { tab: "general", slot: "content" }, h("elsa-workflow-properties-panel", { workflowDefinition: workflowDefinition })), this.renderTestPanel(), this.renderDesignerPanel(), this.renderVersionHistoryPanel(workflowDefinition)));
    }
    renderTestPanel() {
        const testingEnabled = this.serverFeatures.find(x => x == 'WorkflowTesting');
        if (!testingEnabled)
            return;
        return [
            h("elsa-tab-header", { tab: "test", slot: "header" }, "Test"),
            h("elsa-tab-content", { tab: "test", slot: "content" }, h("elsa-workflow-test-panel", { workflowDefinition: this.workflowDefinition, workflowTestActivityId: this.selectedActivityId, selectedActivityId: this.selectedActivityId })),
        ];
    }
    static get watchers() { return {
        "workflowDefinitionId": ["workflowDefinitionIdChangedHandler"],
        "serverUrl": ["serverUrlChangedHandler"],
        "monacoLibPath": ["monacoLibPathChangedHandler"]
    }; }
};
injectHistory(ElsaWorkflowDefinitionEditorScreen);
Tunnel$1.injectProps(ElsaWorkflowDefinitionEditorScreen, ['serverUrl', 'culture', 'monacoLibPath', 'basePath', 'serverFeatures']);
ElsaWorkflowDefinitionEditorScreen.style = elsaWorkflowDefinitionEditorScreenCss;

export { ElsaWorkflowDefinitionEditorScreen as elsa_workflow_definition_editor_screen };
//# sourceMappingURL=elsa-workflow-definition-editor-screen.entry.esm.js.map
