import { r as registerInstance, a as createEvent, h, j as Host } from './index-CL6j2ec2.js';
import { l as leave, t as toggle } from './index-jup-zNrU.js';
import { T as Tunnel } from './workflow-editor-pBAZ9Py8.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import './index-C-8L13GY.js';

const resources = {
    'en': {
        'default': {
            'Publish': 'Publish',
            'Publishing': 'Publishing',
            'Unpublish': 'Unpublish',
            'Import': 'Import',
            'Export': 'Export',
        }
    },
    'zh-CN': {
        'default': {
            'Publish': '发布',
            'Publishing': '发布中',
            'Unpublish': '取消发布',
            'Import': '导入',
            'Export': '导出',
        }
    },
    'nl-NL': {
        'default': {
            'Publish': 'Publiceren',
            'Publishing': 'Bezig met publiceren',
            'Unpublish': 'Niet-publiceren',
            'Import': 'Importeren',
            'Export': 'Exporteren',
        }
    },
    'fa-IR': {
        'default': {
            'Publish': 'انتشار',
            'Publishing': 'در حال انتشار',
            'Unpublish': 'Unpublish',
            'Import': 'Import',
            'Export': 'Export',
        }
    },
    'de-DE': {
        'default': {
            'Publish': 'Veröffentlichen',
            'Publishing': 'Am veröffentlichen',
            'Unpublish': 'Veröffentlichung zurückziehen',
            'Import': 'Importieren',
            'Export': 'Exportieren',
        }
    },
};

const ElsaWorkflowPublishButton = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.publishClicked = createEvent(this, "publishClicked", 7);
        this.unPublishClicked = createEvent(this, "unPublishClicked", 7);
        this.revertClicked = createEvent(this, "revertClicked", 7);
        this.exportClicked = createEvent(this, "exportClicked", 7);
        this.importClicked = createEvent(this, "importClicked", 7);
        this.deleteClicked = createEvent(this, "deleteClicked", 7);
        this.t = (key) => this.i18next.t(key);
        this.onPublishClick = (e) => {
            e.preventDefault();
            this.publishClicked.emit();
            leave(this.menu);
        };
        this.onUnPublishClick = (e) => {
            e.preventDefault();
            this.unPublishClicked.emit();
            leave(this.menu);
        };
        this.onRevertClick = (e) => {
            e.preventDefault();
            this.revertClicked.emit();
            leave(this.menu);
        };
        this.onExportClick = async (e) => {
            e.preventDefault();
            this.exportClicked.emit();
            leave(this.menu);
        };
        this.onImportClick = async (e) => {
            e.preventDefault();
            this.fileInput.value = null;
            this.fileInput.click();
            leave(this.menu);
        };
        this.onDeleteClick = async (e) => {
            e.preventDefault();
            this.deleteClicked.emit();
            leave(this.menu);
        };
        this.renderMainButton = () => {
            const workflowDefinition = this.workflowDefinition;
            const isPublished = workflowDefinition.isPublished;
            if (isPublished)
                return this.publishing ? this.renderUnpublishingButton() : this.renderUnpublishButton();
            else
                return this.publishing ? this.renderPublishingButton() : this.renderPublishButton();
        };
        this.renderPublishButton = () => {
            const workflowDefinition = this.workflowDefinition;
            const isLatest = workflowDefinition.isLatest;
            const version = workflowDefinition.version;
            if (isLatest)
                return this.renderButton('Publish', this.onPublishClick);
            return this.renderButton(`Revert version ${version}`, this.onRevertClick);
        };
        this.renderUnpublishButton = () => {
            return this.renderButton('Unpublish', e => this.onUnPublishClick(e));
        };
        this.renderPublishMenuItem = () => {
            return this.renderMenuItem('Publish', this.onPublishClick);
        };
        this.renderUnpublishMenuItem = () => {
            return this.renderMenuItem('Unpublish', this.onUnPublishClick);
        };
        this.renderMenuItem = (text, handler) => {
            if (!this.workflowDefinition.isPublished)
                return undefined;
            const t = this.t;
            return (h("div", { class: "elsa-py-1", role: "none" }, h("a", { href: "#", onClick: e => handler(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, t(text))));
        };
        this.renderUnpublishingButton = () => {
            return this.renderLoadingButton('Unpublishing');
        };
        this.renderButton = (text, handler) => {
            const t = this.t;
            return (h("button", { type: "button", onClick: e => handler(e), class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-rounded-l-md elsa-border elsa-border-gray-300 elsa-bg-white elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-z-10 focus:elsa-outline-none focus:elsa-ring-1 focus:elsa-ring-blue-500 focus:elsa-border-blue-500" }, text));
        };
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    onWindowClicked(event) {
        const target = event.target;
        if (!this.element.contains(target))
            this.closeMenu();
    }
    closeMenu() {
        leave(this.menu);
    }
    toggleMenu() {
        toggle(this.menu);
    }
    async onFileInputChange(e) {
        const files = this.fileInput.files;
        if (files.length == 0) {
            return;
        }
        this.importClicked.emit(files[0]);
    }
    render() {
        const t = this.t;
        return (h(Host, { key: '65186c75ffbba97676f54e1f87917f89826161e0', class: "elsa-block", ref: el => this.element = el }, h("span", { key: '3c604ce8eb3d5e5bac6ebf2b5a8c9e91e4a5e671', class: "elsa-relative elsa-z-0 elsa-inline-flex elsa-shadow-sm elsa-rounded-md" }, this.renderMainButton(), h("span", { key: '4a4a6ecf3a114227b8686fd03abe326d628bdec0', class: "-elsa-ml-px elsa-relative elsa-block" }, h("button", { key: '8a848bcc64cf77c663b1ec0cf723b962d62bedef', onClick: () => this.toggleMenu(), id: "option-menu", type: "button", class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-2 elsa-py-2 elsa-rounded-r-md elsa-border elsa-border-gray-300 elsa-bg-white elsa-text-sm elsa-font-medium elsa-text-gray-500 hover:elsa-bg-gray-50 focus:elsa-z-10 focus:elsa-outline-none focus:elsa-ring-1 focus:elsa-ring-blue-500 focus:elsa-border-blue-500" }, h("span", { key: '2bba72b83d90dee585a96f3d3ad58dd1e57f9270', class: "elsa-sr-only" }, "Open options"), h("svg", { key: '99c19e2aae75a57b60f6c39cb1541276356c2111', class: "elsa-h-5 elsa-w-5", "x-description": "Heroicon name: solid/chevron-down", xmlns: "http://www.w3.org/2000/svg", viewBox: "0 0 20 20", fill: "currentColor", "aria-hidden": "true" }, h("path", { key: '727544401c0de10b405ec7ebe1c131608fbcd700', "fill-rule": "evenodd", d: "M5.293 7.293a1 1 0 011.414 0L10 10.586l3.293-3.293a1 1 0 111.414 1.414l-4 4a1 1 0 01-1.414 0l-4-4a1 1 0 010-1.414z", "clip-rule": "evenodd" }))), h("div", { key: 'eb827061423b19cdef2d0144a7c02a517fdee917', ref: el => this.menu = el, "data-transition-enter": "elsa-transition elsa-ease-out elsa-duration-100", "data-transition-enter-start": "elsa-transform elsa-opacity-0 elsa-scale-95", "data-transition-enter-end": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave": "elsa-transition elsa-ease-in elsa-duration-75", "data-transition-leave-start": "elsa-transform elsa-opacity-100 elsa-scale-100", "data-transition-leave-end": "elsa-transform elsa-opacity-0 elsa-scale-95", class: "hidden origin-bottom-right elsa-absolute elsa-right-0 elsa-bottom-10 elsa-mb-2 -elsa-mr-1 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5" }, h("div", { key: '12d62d61183f221e2c1b234feb0b54195d167a62', class: "elsa-divide-y elsa-divide-gray-100 focus:elsa-outline-none", role: "menu", "aria-orientation": "vertical", "aria-labelledby": "option-menu" }, h("div", { key: '5ac5605c67ef8ef2abb0c65bf4c79823a480fa8e', class: "elsa-py-1", role: "none" }, h("a", { key: '87842169a31ddf7ce2ed210341719b4223db81c6', href: "#", onClick: e => this.onExportClick(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, t('Export')), h("a", { key: '747d68daf6cd2f9a35c8f3d5e816dd0e39f6dc4a', href: "#", onClick: e => this.onImportClick(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-gray-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, t('Import')), h("a", { key: 'abcb97d738506aeecb6d15d06e8f281438bd4813', href: "#", onClick: e => this.onDeleteClick(e), class: "elsa-block elsa-px-4 elsa-py-2 elsa-text-sm elsa-text-red-700 hover:elsa-bg-gray-100 hover:elsa-text-gray-900", role: "menuitem" }, t('Delete'))))))), h("input", { key: '3374c5fba1780158dd586cc6115b86f9c22730e4', type: "file", class: "hidden", onChange: e => this.onFileInputChange(e), ref: el => this.fileInput = el })));
    }
    renderPublishingButton() {
        const workflowDefinition = this.workflowDefinition;
        const isLatest = workflowDefinition.isLatest;
        const version = workflowDefinition.version;
        const text = isLatest ? 'Publishing' : `Publishing version ${version}`;
        return this.renderLoadingButton(text);
    }
    renderLoadingButton(text) {
        const t = this.t;
        return (h("button", { type: "button", disabled: true, class: "elsa-relative elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-rounded-l-md elsa-border elsa-border-gray-300 elsa-bg-white elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-z-10 focus:elsa-outline-none focus:elsa-ring-1 focus:elsa-ring-blue-500 focus:elsa-border-blue-500" }, h("svg", { class: "elsa-animate-spin -elsa-ml-1 elsa-mr-3 elsa-h-5 elsa-w-5 elsa-text-blue-400", xmlns: "http://www.w3.org/2000/svg", fill: "none", viewBox: "0 0 24 24" }, h("circle", { class: "elsa-opacity-25", cx: "12", cy: "12", r: "10", stroke: "currentColor", "stroke-width": "4" }), h("path", { class: "elsa-opacity-75", fill: "currentColor", d: "M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z" })), t(text)));
    }
};
Tunnel.injectProps(ElsaWorkflowPublishButton, ['serverUrl']);

export { ElsaWorkflowPublishButton as elsa_workflow_publish_button };
//# sourceMappingURL=elsa-workflow-publish-button.entry.esm.js.map
