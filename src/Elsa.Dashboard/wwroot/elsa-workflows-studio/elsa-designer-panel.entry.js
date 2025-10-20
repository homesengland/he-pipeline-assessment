import { r as registerInstance, a as createEvent, h, j as Host } from './index-CL6j2ec2.js';
import { f as featuresDataManager } from './index-fZDMH_YE.js';
import { L as LayoutDirection } from './models-0-8LsKgc.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './event-bus-axQqcjdg.js';
import './events-CpKc8CLe.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './index-D7wXd6HU.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';

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
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    render() {
        const features = featuresDataManager.getUIFeatureList();
        const { t } = this;
        return (h(Host, { key: '029481392693cc41d52f1746aed11c3e1098ecae' }, h("div", { key: '0862fde319981f5c7de7d278b198245ca42e619c', class: "elsa-mt-4" }, features.map(name => {
            const feature = featuresDataManager.getFeatureConfig(name);
            return (h("div", null, h("div", { class: "elsa-relative elsa-flex elsa-items-start elsa-ml-1" }, h("div", { class: "elsa-flex elsa-items-center elsa-h-5" }, h("input", { id: name, name: name, type: "checkbox", value: `${feature.enabled}`, checked: feature.enabled, onChange: e => this.onToggleChange(e, name), class: "focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 rounded" })), h("div", { class: "elsa-ml-3 elsa-text-sm" }, h("label", { htmlFor: name, class: "elsa-font-medium elsa-text-gray-700" }, t(`${name}Name`)), h("p", { class: "elsa-text-gray-500" }, t(`${name}Description`)))), h("div", { class: "elsa-ml-1 elsa-my-4" }, this.renderFeatureData(name, feature))));
        }))));
    }
};

export { ElsaDesignerPanel as elsa_designer_panel };
//# sourceMappingURL=elsa-designer-panel.entry.esm.js.map
