import { r as registerInstance, i as getAssetPath, h } from './index-ea213ee1.js';
import './index-842ad20c.js';
import { G as GetIntlMessage } from './intl-message-63e92c76.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import './index-2db7bf78.js';

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

let ElsaStudioHome = class {
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
    return (h("div", { class: "elsa-home-wrapper elsa-relative elsa-bg-gray-800 elsa-overflow-hidden elsa-h-screen" }, h("main", { class: "elsa-mt-16 sm:elsa-mt-24" }, h("div", { class: "elsa-mx-auto elsa-max-w-7xl" }, h("div", { class: "lg:elsa-grid lg:elsa-grid-cols-12 lg:elsa-gap-8" }, h("div", { class: "elsa-px-4 sm:elsa-px-6 sm:elsa-text-center md:elsa-max-w-2xl md:elsa-mx-auto lg:elsa-col-span-6 lg:elsa-text-left lg:flex lg:elsa-items-center" }, h("div", { class: "elsa-home-caption-wrapper" }, h("h1", { class: "elsa-mt-4 elsa-text-4xl elsa-tracking-tight elsa-font-extrabold elsa-text-white sm:elsa-mt-5 sm:elsa-leading-none lg:elsa-mt-6 lg:elsa-text-5xl xl:elsa-text-6xl" }, h("span", { class: "md:elsa-block" }, h(IntlMessage, { label: "Welcome", dangerous: true, title: `<span class='elsa-text-teal-400 md:elsa-block'>Elsa Workflows</span> <span>${serverVersion}</span>` }))), h("p", { class: "tagline elsa-mt-3 elsa-text-base elsa-text-gray-300 sm:elsa-mt-5 sm:elsa-text-xl lg:elsa-text-lg xl:elsa-text-xl" }, h(IntlMessage, { label: "Tagline" })))), h("div", { class: "elsa-mt-16 sm:elsa-mt-24 lg:elsa-mt-0 lg:elsa-col-span-6" }, h("div", { class: "sm:elsa-max-w-md sm:elsa-w-full sm:elsa-mx-auto sm:elsa-rounded-lg sm:elsa-overflow-hidden" }, h("div", { class: "elsa-px-4 elsa-py-8 sm:elsa-px-10" }, h("img", { class: "elsa-home-visual", src: visualPath, alt: "", width: 400 })))))))));
  }
  static get assetsDirs() { return ["assets"]; }
};
Tunnel.injectProps(ElsaStudioHome, ['culture', 'serverVersion']);

export { ElsaStudioHome as elsa_studio_home };
