import { r as registerInstance, e as createEvent, h, l as Host } from './index-1542df5c.js';
import { f as featuresDataManager } from './index-892f713d.js';
import { L as LayoutDirection } from './models-96b27412.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './event-bus-5d6f3774.js';
import './events-d0aab14a.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './index-1654a48d.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

const resources = {
  'en': {
    default: {
      'workflowLayoutName': 'Workflow Layout',
      'workflowLayoutDescription': 'Determines the direction in which activities are laid out.',
      'TopBottom': 'Top to Bottom',
      'LeftRight': 'Left to Right',
      'BottomTop': 'Bottom to Top',
      'RightLeft': 'Right to Left',
    }
  },
  'de-De': {
    default: {
      'workflowLayoutName': 'Ablauf Layout',
      'workflowLayoutDescription': 'Bestimmt die Richtung, in welche der Ablauf angezeigt wird',
      'TopBottom': 'Von Oben nach Unten',
      'LeftRight': 'Von Links nach Rechts',
      'BottomTop': 'Von Unten nach Oben',
      'RightLeft': 'Von Rechts nach Links',
    }
  }
};

const ElsaDesignerPanel = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.featureChanged = createEvent(this, "featureChanged", 7);
    this.featureStatusChanged = createEvent(this, "featureStatusChanged", 7);
    this.t = (key, options) => this.i18next.t(key, options);
    this.renderFeatureData = (name, feature) => {
      if (!feature.enabled) {
        return null;
      }
      const { t } = this;
      switch (name) {
        case featuresDataManager.supportedFeatures.workflowLayout:
          return (h("select", { id: name, name: name, onChange: e => this.onPropertyChange(e, name), class: "block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md" }, Object.keys(LayoutDirection).map(key => {
            return h("option", { value: LayoutDirection[key], selected: LayoutDirection[key] === feature.value }, t(key));
          })));
        default:
          return null;
      }
    };
    this.onToggleChange = (e, name) => {
      const element = e.target;
      featuresDataManager.setEnableStatus(name, element.checked);
      this.lastChangeTime = new Date();
      this.featureStatusChanged.emit(name);
    };
    this.onPropertyChange = (e, name) => {
      const element = e.target;
      featuresDataManager.setFeatureConfig(name, element.value.trim());
      this.featureChanged.emit(name);
    };
    this.culture = undefined;
    this.lastChangeTime = undefined;
  }
  async componentWillLoad() {
    this.i18next = await loadTranslations(this.culture, resources);
  }
  render() {
    const features = featuresDataManager.getUIFeatureList();
    const { t } = this;
    return (h(Host, null, h("div", { class: "elsa-mt-4" }, features.map(name => {
      const feature = featuresDataManager.getFeatureConfig(name);
      return (h("div", null, h("div", { class: "elsa-relative elsa-flex elsa-items-start elsa-ml-1" }, h("div", { class: "elsa-flex elsa-items-center elsa-h-5" }, h("input", { id: name, name: name, type: "checkbox", value: `${feature.enabled}`, checked: feature.enabled, onChange: e => this.onToggleChange(e, name), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 rounded" })), h("div", { class: "elsa-ml-3 elsa-text-sm" }, h("label", { htmlFor: name, class: "elsa-font-medium elsa-text-gray-700" }, t(`${name}Name`)), h("p", { class: "elsa-text-gray-500" }, t(`${name}Description`)))), h("div", { class: "elsa-ml-1 elsa-my-4" }, this.renderFeatureData(name, feature))));
    }))));
  }
};

export { ElsaDesignerPanel as elsa_designer_panel };
