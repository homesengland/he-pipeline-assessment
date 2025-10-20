import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';

const resources = {
    'en': {
        'default': {
            'Yes': 'Yes',
            'No': 'No'
        }
    },
    'zh-CN': {
        'default': {
            'Yes': '是',
            'No': '否'
        }
    },
    'nl-NL': {
        'default': {
            'Yes': 'Ja',
            'No': 'Nee',
        }
    },
    'fa-IR': {
        'default': {
            'Yes': 'بله',
            'No': 'خیر'
        }
    },
    'de-DE': {
        'default': {
            'Yes': 'Ja',
            'No': 'Nein',
        }
    },
};

const ElsaConfirmDialog = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    async show(caption, message) {
        this.caption = caption;
        this.message = message;
        await this.dialog.show(true);
        return new Promise((fulfill, reject) => {
            this.fulfill = fulfill;
            this.reject = reject;
        });
    }
    async hide() {
        await this.dialog.hide(true);
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    async onDismissClick() {
        this.fulfill(false);
        await this.hide();
    }
    async onAcceptClick() {
        this.fulfill(true);
        this.fulfill = null;
        this.reject = null;
        await this.hide();
    }
    render() {
        const t = x => this.i18next.t(x);
        return (h(Host, { key: '6fc35381d19c0e14f442f2a225472f57089de57c' }, h("elsa-modal-dialog", { key: '0448339165355406c04c57a6a7be110ab9859ed4', ref: el => this.dialog = el }, h("div", { key: '62d737006f82f3153cab0d3b980c788334f2ce7e', slot: "content", class: "elsa-py-8 elsa-px-4" }, h("div", { key: '63b58d57bb39ecc1a6a2c4554411072028cec833', class: "hidden sm:elsa-block elsa-absolute elsa-top-0 elsa-right-0 elsa-pt-4 elsa-pr-4" }, h("button", { key: 'cd849a78235533859c72c3c74389adf858e0c0ba', type: "button", onClick: () => this.onDismissClick(), class: "elsa-bg-white elsa-rounded-md elsa-text-gray-400 hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, h("span", { key: 'e533c999c4a642c195fd092a667835be418d90a8', class: "elsa-sr-only" }, "Close"), h("svg", { key: 'da81611ad77f35b6b9e415eb203e53f11adbffa6', class: "elsa-h-6 elsa-w-6", "x-description": "Heroicon name: outline/x", xmlns: "http://www.w3.org/2000/svg", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor", "aria-hidden": "true" }, h("path", { key: 'abf1e92b84e4253941c3f93eeeef8ad5d4048f4b', "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M6 18L18 6M6 6l12 12" })))), h("div", { key: '4079ca6aa0756a5412e4bf2424c5d49d66da32f6', class: "sm:elsa-flex sm:elsa-items-start" }, h("div", { key: 'c3018ff14346519a213d44dd67aaadc0cdc9a6dd', class: "elsa-mx-auto elsa-flex-shrink-0 elsa-flex elsa-items-center elsa-justify-center elsa-h-12 elsa-w-12 elsa-rounded-full elsa-bg-red-100 sm:elsa-mx-0 sm:elsa-h-10 sm:elsa-w-10" }, h("svg", { key: '5a6ca97e54e53375c5be4508fcb0524997cab561', class: "elsa-h-6 elsa-w-6 elsa-text-red-600", "x-description": "Heroicon name: outline/exclamation", xmlns: "http://www.w3.org/2000/svg", fill: "none", viewBox: "0 0 24 24", stroke: "currentColor", "aria-hidden": "true" }, h("path", { key: '0059587a9bb1fbe50f274b268011a7baea934d13', "stroke-linecap": "round", "stroke-linejoin": "round", "stroke-width": "2", d: "M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" }))), h("div", { key: '0f130d9a52ed4c9b0017ec4fa5b77c98ed19c7b2', class: "elsa-mt-3 elsa-text-center sm:elsa-mt-0 sm:elsa-ml-4 sm:elsa-text-left" }, h("h3", { key: '8d9db985ae5e69377273c421e365f225dc5f9d29', class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900", id: "modal-title" }, this.caption), h("div", { key: 'd10537620785063b72772feee11a754593b1d4fd', class: "elsa-mt-2" }, h("p", { key: '88de9a9eddf28192d5b8e5b2aa0e1dfb7f1df643', class: "elsa-text-sm elsa-text-gray-500" }, this.message))))), h("div", { key: '5746e8c44446b7e24c23e888923a4afceeff339f', slot: "buttons" }, h("div", { key: 'eb07f7e40e68e83d067f611acded7883e7ccf36d', class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { key: 'ead6073087263f608f282e0702ad636aaf284f45', type: "button", onClick: () => this.onAcceptClick(), class: "elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-transparent elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-red-600 elsa-text-base elsa-font-medium elsa-text-white hover:elsa-bg-red-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-red-500 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, t('Yes')), h("button", { key: '9a48427d21e6cc1bd123988ac7828ea1b8fd708c', type: "button", onClick: () => this.onDismissClick(), class: "elsa-mt-3 elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-text-gray-500 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:ring-indigo-500 sm:elsa-mt-0 sm:elsa-w-auto sm:elsa-text-sm" }, t('No')))))));
    }
};

export { ElsaConfirmDialog as elsa_confirm_dialog };
//# sourceMappingURL=elsa-confirm-dialog.entry.esm.js.map
