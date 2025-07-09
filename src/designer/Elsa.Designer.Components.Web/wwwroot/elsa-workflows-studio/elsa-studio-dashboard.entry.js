import { r as registerInstance, i as getAssetPath, h } from './index-ea213ee1.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { G as GetIntlMessage } from './intl-message-63e92c76.js';
import { T as Tunnel } from './dashboard-c739a7dd.js';
import { E as EventTypes } from './index-0f68dbd6.js';
import './index-c5018c3a.js';
import { e as eventBus } from './event-bus-6625fc04.js';
import './index-842ad20c.js';
import './index-2db7bf78.js';
import './elsa-client-ecb85def.js';
import './axios-middleware.esm-fcda64d5.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

const resources = {
  'en': {
    default: {
      'WorkflowDefinitions': 'Workflow Definitions',
      'WorkflowInstances': 'Workflow Instances',
      'WorkflowRegistry': 'Workflow Registry',
      'WebhookDefinitions': 'Webhook Definitions',
    }
  },
  'zh-CN': {
    default: {
      'WorkflowDefinitions': '工作流定义',
      'WorkflowInstances': '工作流实例',
      'WorkflowRegistry': '工作流程注册表',
      'WebhookDefinitions': 'Webhook定义',
    }
  },
  'nl-NL': {
    default: {
      'WorkflowDefinitions': 'Workflow Definities',
      'WorkflowInstances': 'Workflows',
      'WorkflowRegistry': 'Workflow Register',
      'WebhookDefinitions': 'Webhook Definities',
    }
  },
  'fa-IR': {
    default: {
      'WorkflowDefinitions': 'فرآیندها',
      'WorkflowInstances': 'فرآیندهای اجرا شده',
      'WorkflowRegistry': 'Workflow Registry',
      'WebhookDefinitions': 'مشخصات Webhookها',
    },
  },
  'es-ES': {
    default: {
      'WorkflowDefinitions': 'Definiciones de flujos',
      'WorkflowInstances': 'Ejecuciones de flujos',
      'WorkflowRegistry': 'Registro de flujos',
      'WebhookDefinitions': 'Definiciones de webhooks',
    },
    menuItems: {
      'workflow-definitions': 'Definiciones de flujos',
      'workflow-instances': 'Ejecuciones de flujos',
      'workflow-registry': 'Registro de flujos',
    }
  },
  'de-DE': {
    default: {
      'WorkflowDefinitions': 'Ablaufdefinitionen',
      'WorkflowInstances': 'Ablaufinstanzen',
      'WorkflowRegistry': 'Ablaufverzeichnis',
      'WebhookDefinitions': 'Webhook Definitionen',
    },
    menuItems: {
      'workflow-definitions': 'Ablaufdefinitionen',
      'workflow-instances': 'Ablaufinstanzen',
      'workflow-registry': 'Ablaufverzeichnis',
    }
  }
};

let ElsaStudioDashboard = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.basePath = '';
    this.dashboardMenu = {
      data: {
        menuItems: [
          ['workflow-definitions', 'Workflow Definitions'],
          ['workflow-instances', 'Workflow Instances'],
          ['workflow-registry', 'Workflow Registry'],
        ],
        routes: [
          ['', 'elsa-studio-home', true],
          ['workflow-registry', 'elsa-studio-workflow-registry', true],
          ['workflow-registry/:id', 'elsa-studio-workflow-blueprint-view'],
          ['workflow-definitions', 'elsa-studio-workflow-definitions-list', true],
          ['workflow-definitions/:id', 'elsa-studio-workflow-definitions-edit'],
          ['workflow-instances', 'elsa-studio-workflow-instances-list', true],
          ['workflow-instances/:id', 'elsa-studio-workflow-instances-view'],
          ['oauth2-authorized', 'elsa-oauth2-authorized', true],
        ]
      }
    };
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
    await eventBus.emit(EventTypes.Dashboard.Appearing, this, this.dashboardMenu);
  }
  render() {
    const logoPath = getAssetPath('./assets/logo.png');
    const basePath = this.basePath || '';
    const IntlMessage = GetIntlMessage(this.i18next);
    const menuItemsNamespace = "menuItems";
    let menuItems = (this.dashboardMenu.data != null ? this.dashboardMenu.data.menuItems : [])
      .map(([route, label]) => this.i18next.exists(`${menuItemsNamespace}:${route}`) ?
      [route, this.i18next.t(`${menuItemsNamespace}:${route}`)] :
      [route, label]);
    let routes = this.dashboardMenu.data != null ? this.dashboardMenu.data.routes : [];
    const renderFeatureMenuItem = (item, basePath) => {
      return (h("stencil-route-link", { url: `${basePath}/${item[0]}`, anchorClass: "elsa-text-gray-300 hover:elsa-bg-gray-700 hover:elsa-text-white elsa-px-3 elsa-py-2 elsa-rounded-md elsa-text-sm elsa-font-medium", activeClass: "elsa-text-white elsa-bg-gray-900" }, h(IntlMessage, { label: `${item[1]}` })));
    };
    const renderFeatureRoute = (item, basePath) => {
      return (h("stencil-route", { url: `${basePath}/${item[0]}`, component: `${item[1]}`, exact: item[2] }));
    };
    return (h("div", { class: "elsa-h-screen elsa-bg-gray-100" }, h("nav", { class: "elsa-bg-gray-800" }, h("div", { class: "elsa-px-4 sm:elsa-px-6 lg:elsa-px-8" }, h("div", { class: "elsa-flex elsa-items-center elsa-justify-between elsa-h-16" }, h("div", { class: "elsa-flex elsa-items-center" }, h("div", { class: "elsa-flex-shrink-0" }, h("stencil-route-link", { url: `${basePath}/` }, h("img", { class: "elsa-h-8 elsa-w-8", src: logoPath, alt: "Workflow" }))), h("div", { class: "hidden md:elsa-block" }, h("div", { class: "elsa-ml-10 elsa-flex elsa-items-baseline elsa-space-x-4" }, menuItems.map(item => renderFeatureMenuItem(item, basePath))))), h("elsa-user-context-menu", null)))), h("main", null, h("stencil-router", null, h("stencil-route-switch", { scrollTopOffset: 0 }, routes.map(item => renderFeatureRoute(item, basePath)))))));
  }
  static get assetsDirs() { return ["assets"]; }
};
Tunnel.injectProps(ElsaStudioDashboard, ['culture', 'basePath']);

export { ElsaStudioDashboard as elsa_studio_dashboard };
