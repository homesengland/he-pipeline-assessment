import { r as registerInstance, d as getAssetPath, h } from './index-CL6j2ec2.js';
import './index-CBYiEsWN.js';
import { G as GetIntlMessage } from './intl-message-C60V_pHc.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { T as Tunnel } from './dashboard-DaK-DIvX.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './index-C-8L13GY.js';

const resources = {
    'en': {
        'default': {
            'Welcome': 'Welcome to {{title}}',
            'Tagline': 'Use the dashboard app to manage all the things.'
        }
    },
    'zh-CN': {
        'default': {
            'Welcome': '欢迎使用{{title}}',
            'Tagline': '使用应用程序仪表盘来管理所有事情。'
        }
    },
    'nl-NL': {
        'default': {
            'Welcome': 'Welkom bij {{title}}',
            'Tagline': 'Gebruik het dashboard om alles te regelen.'
        }
    },
    'fa-IR': {
        'default': {
            'Welcome': 'به {{title}} خوش آمدید',
            'Tagline': 'میتوانید از این داشبورد برای مدیریت بخش های مختلف استفاده نمایید.'
        }
    },
    'de-DE': {
        'default': {
            'Welcome': 'Willkommen bei {{title}}',
            'Tagline': 'Benutze das Dashboard um alles zu steuern'
        }
    },
};

const ElsaStudioHome = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    render() {
        const visualPath = getAssetPath('./assets/undraw_breaking_barriers_vnf3.svg');
        const IntlMessage = GetIntlMessage(this.i18next);
        const serverVersion = this.serverVersion;
        return (h("div", { key: '7d3b867aaeca6a898ae630fc9e254a481252d671', class: "elsa-home-wrapper elsa-relative elsa-bg-gray-800 elsa-overflow-hidden elsa-h-screen" }, h("main", { key: 'b3ddcbcf4336cb8081d4f8dd885671c4df279bcb', class: "elsa-mt-16 sm:elsa-mt-24" }, h("div", { key: 'e0876707e447616f63eaf19d70c2da7b661234a0', class: "elsa-mx-auto elsa-max-w-7xl" }, h("div", { key: '894fa90b32e5b8f5ebd2d2313373fd77c4425bfc', class: "lg:elsa-grid lg:elsa-grid-cols-12 lg:elsa-gap-8" }, h("div", { key: '536e8f8e2546178610a37e646be3c7fa09e8f744', class: "elsa-px-4 sm:elsa-px-6 sm:elsa-text-center md:elsa-max-w-2xl md:elsa-mx-auto lg:elsa-col-span-6 lg:elsa-text-left lg:flex lg:elsa-items-center" }, h("div", { key: 'f295810d192c8adb92c0f34882ab77bb45562b92', class: "elsa-home-caption-wrapper" }, h("h1", { key: '593e4864f5e3e13ffafc839db2e5f5b7c4b6edc2', class: "elsa-mt-4 elsa-text-4xl elsa-tracking-tight elsa-font-extrabold elsa-text-white sm:elsa-mt-5 sm:elsa-leading-none lg:elsa-mt-6 lg:elsa-text-5xl xl:elsa-text-6xl" }, h("span", { key: 'f0b29d9aeef383872551c59edeb5a2da6ce500f4', class: "md:elsa-block" }, h(IntlMessage, { key: '1439a3a0217a9b6c7027658662e834360b39fd4c', label: "Welcome", dangerous: true, title: `<span class='elsa-text-teal-400 md:elsa-block'>Elsa Workflows</span> <span>${serverVersion}</span>` }))), h("p", { key: 'edcd26009ff6284a71c6266ea5850b96d8fe8401', class: "tagline elsa-mt-3 elsa-text-base elsa-text-gray-300 sm:elsa-mt-5 sm:elsa-text-xl lg:elsa-text-lg xl:elsa-text-xl" }, h(IntlMessage, { key: '3d1f887147008398dc871e89a543ec509c4dff4c', label: "Tagline" })))), h("div", { key: '9e83294f33c29e684fa75a801a34ba9be918df51', class: "elsa-mt-16 sm:elsa-mt-24 lg:elsa-mt-0 lg:elsa-col-span-6" }, h("div", { key: '9b37568a861348d6408b9816614b1b298aa87fbb', class: "sm:elsa-max-w-md sm:elsa-w-full sm:elsa-mx-auto sm:elsa-rounded-lg sm:elsa-overflow-hidden" }, h("div", { key: '5f8c46e4bec72824ed8501c3dd7198d5500891f9', class: "elsa-px-4 elsa-py-8 sm:elsa-px-10" }, h("img", { key: 'dbdc28642e26e4f42661bcbb98b12e4ea91d6eea', class: "elsa-home-visual", src: visualPath, alt: "", width: 400 })))))))));
    }
    static get assetsDirs() { return ["assets"]; }
};
Tunnel.injectProps(ElsaStudioHome, ['culture', 'serverVersion']);

export { ElsaStudioHome as elsa_studio_home };
//# sourceMappingURL=elsa-studio-home.entry.esm.js.map
