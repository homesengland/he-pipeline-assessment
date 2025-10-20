import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { b as propertyDisplayManager } from './index-fZDMH_YE.js';
import { a as state } from './store-B_H_ZDGs.js';
import './index-D7wXd6HU.js';
import { F as FormContext, s as selectField, a as section, c as checkBox, t as textInput, b as textArea } from './forms-CaCATBuQ.js';
import { l as loadTranslations } from './i18n-loader-DJQycacf.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';

const resources = {
    'en': {
        'default': {
            "Buttons": {
                'Save': 'Save',
                'Cancel': 'Cancel'
            },
            'Tabs': {
                'Properties': {
                    'Name': 'Properties'
                },
                'Common': {
                    'Name': 'Common',
                    'Fields': {
                        'Name': {
                            'Label': 'Name',
                            'Hint': 'The technical name of the activity.'
                        },
                        'DisplayName': {
                            'Label': 'Display Name',
                            'Hint': 'The friendly name of the activity.'
                        },
                        'Description': {
                            'Label': 'Description',
                            'Hint': 'A custom description for this activity.'
                        }
                    }
                },
                'Storage': {
                    'Name': 'Storage'
                }
            }
        }
    },
    'zh-CN': {
        'default': {
            "Buttons": {
                'Save': '保存',
                'Cancel': '取消'
            },
            'Tabs': {
                'Properties': {
                    'Name': '属性'
                },
                'Common': {
                    'Name': '通用',
                    'Fields': {
                        'Name': {
                            'Label': '名称',
                            'Hint': '该活动的技术名称。'
                        },
                        'DisplayName': {
                            'Label': '展示名称',
                            'Hint': '该活动的呈现名称。'
                        },
                        'Description': {
                            'Label': '描述',
                            'Hint': '对这项活动的自定义描述。'
                        }
                    }
                },
                'Storage': {
                    'Name': '储存'
                }
            }
        }
    },
    'nl-NL': {
        'default': {
            'Buttons': {
                'Save': 'Opslaan',
                'Cancel': 'Annuleren'
            },
            'Tabs': {
                'Properties': {
                    'Name': 'Eigenschappen'
                },
                'Common': {
                    'Name': 'Algemeen',
                    'Fields': {
                        'Name': {
                            'Label': 'Naam',
                            'Hint': 'De technische naam van de activity.'
                        },
                        'DisplayName': {
                            'Label': 'Weergave Naam',
                            'Hint': 'De gebruikersvriendelijke naam van de activity.'
                        },
                        'Description': {
                            'Label': 'Omschrijving',
                            'Hint': 'Een aangepaste omschrijving van de activity.'
                        }
                    }
                },
                'Storage': {
                    'Name': 'Opslag'
                }
            }
        }
    },
    'fa-IR': {
        'default': {
            "Buttons": {
                'Save': 'ذخیره',
                'Cancel': 'لغو'
            },
            'Tabs': {
                'Properties': {
                    'Name': 'ویژگیها'
                },
                'Common': {
                    'Name': 'عمومی',
                    'Fields': {
                        'Name': {
                            'Label': 'نام',
                            'Hint': 'نام سیستمی Activity'
                        },
                        'DisplayName': {
                            'Label': 'عنوان نمایشی',
                            'Hint': 'عنوانی که Acitvity را توضیح میدهد'
                        },
                        'Description': {
                            'Label': 'توضیحات',
                            'Hint': 'توضیحات این Activity'
                        }
                    }
                },
                'Storage': {
                    'Name': 'Storage'
                }
            }
        }
    },
    'de-DE': {
        'default': {
            'Buttons': {
                'Save': 'Speichern',
                'Cancel': 'Abbrechen'
            },
            'Tabs': {
                'Properties': {
                    'Name': 'Eigenschaften'
                },
                'Common': {
                    'Name': 'Allgemein',
                    'Fields': {
                        'Name': {
                            'Label': 'Name',
                            'Hint': 'Der technische Name der Aktivität'
                        },
                        'DisplayName': {
                            'Label': 'Anzeigename',
                            'Hint': 'Der freundliche Name der Aktivität'
                        },
                        'Description': {
                            'Label': 'Beschreibung',
                            'Hint': 'Eine individuelle Beschreibung der Aktivität'
                        }
                    }
                },
                'Storage': {
                    'Name': 'Speicherort'
                }
            }
        }
    },
};

const ElsaActivityEditorModal = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.workflowStorageDescriptors = [];
        this.renderProps = {};
        // Force a new key every time we show the editor to make sure Stencil creates new components.
        // This prevents the issue where the designer has e.g. one activity where the user edits the properties, cancels out, then opens the editor again, seeing the entered value still there.
        this.timestamp = new Date();
        this.t = (key) => this.i18next.t(key);
        this.onSubmit = async (e) => {
            e.preventDefault();
            const form = e.target;
            const formData = new FormData(form);
            this.updateActivity(formData);
            await eventBus.emit(EventTypes.UpdateActivity, this, this.activityModel);
            await this.hide(true);
        };
        this.onTabClick = (e, tab) => {
            e.preventDefault();
            this.renderProps = Object.assign(Object.assign({}, this.renderProps), { selectedTabName: tab.tabName });
        };
        this.onShowActivityEditor = async (activity, animate) => {
            this.activityModel = JSON.parse(JSON.stringify(activity));
            this.activityDescriptor = state.activityDescriptors.find(x => x.type == activity.type);
            this.workflowStorageDescriptors = state.workflowStorageDescriptors;
            this.formContext = new FormContext(this.activityModel, newValue => this.activityModel = newValue);
            this.timestamp = new Date();
            this.renderProps = {};
            await this.show(animate);
        };
        this.show = async (animate) => await this.dialog.show(animate);
        this.hide = async (animate) => await this.dialog.hide(animate);
        this.onKeyDown = async (event) => {
            if (event.ctrlKey && event.key === 'Enter') {
                this.dialog.querySelector('button[type="submit"]').click();
            }
        };
        this.onDialogShown = async () => {
            const args = {
                activityModel: this.activityModel,
                activityDescriptor: this.activityDescriptor
            };
            await eventBus.emit(EventTypes.ActivityEditor.Appearing, this, args);
        };
        this.onDialogHidden = async () => {
            const args = {
                activityModel: this.activityModel,
                activityDescriptor: this.activityDescriptor,
            };
            await eventBus.emit(EventTypes.ActivityEditor.Disappearing, this, args);
        };
    }
    connectedCallback() {
        eventBus.on(EventTypes.ActivityEditor.Show, this.onShowActivityEditor);
    }
    disconnectedCallback() {
        eventBus.detach(EventTypes.ActivityEditor.Show, this.onShowActivityEditor);
    }
    async componentWillLoad() {
        this.i18next = await loadTranslations(this.culture, resources);
    }
    updateActivity(formData) {
        const activity = this.activityModel;
        const activityDescriptor = this.activityDescriptor;
        const inputProperties = activityDescriptor.inputProperties;
        for (const property of inputProperties)
            propertyDisplayManager.update(activity, property, formData);
    }
    async componentWillRender() {
        const activityDescriptor = this.activityDescriptor || {
            displayName: '',
            type: '',
            outcomes: [],
            category: '',
            traits: 0,
            browsable: false,
            inputProperties: [],
            outputProperties: [],
            description: '',
            customAttributes: {}
        };
        const propertyCategories = activityDescriptor.inputProperties.filter(x => x.category).map(x => x.category).distinct();
        const defaultProperties = activityDescriptor.inputProperties.filter(x => !x.category || x.category.length == 0);
        const activityModel = this.activityModel || {
            type: '',
            activityId: '',
            outcomes: [],
            properties: [],
            propertyStorageProviders: {}
        };
        const t = this.t;
        let tabs = [];
        if (defaultProperties.length > 0) {
            tabs.push({
                tabName: t('Tabs.Properties.Name'),
                renderContent: () => this.renderPropertiesTab(activityModel)
            });
        }
        for (const category of propertyCategories) {
            const categoryTab = {
                tabName: category,
                renderContent: () => this.renderCategoryTab(activityModel, activityDescriptor, category)
            };
            tabs.push(categoryTab);
        }
        tabs.push({
            tabName: t('Tabs.Common.Name'),
            renderContent: () => this.renderCommonTab(activityModel)
        });
        tabs.push({
            tabName: t('Tabs.Storage.Name'),
            renderContent: () => this.renderStorageTab(activityModel, activityDescriptor)
        });
        this.renderProps = {
            activityDescriptor,
            activityModel,
            propertyCategories,
            defaultProperties,
            tabs,
            selectedTabName: this.renderProps.selectedTabName
        };
        await eventBus.emit(EventTypes.ActivityEditor.Rendering, this, this.renderProps);
        let selectedTabName = this.renderProps.selectedTabName;
        tabs = this.renderProps.tabs;
        if (!selectedTabName)
            this.renderProps.selectedTabName = tabs[0].tabName;
        if (tabs.findIndex(x => x.tabName === selectedTabName) < 0)
            this.renderProps.selectedTabName = selectedTabName = tabs[0].tabName;
    }
    async componentDidRender() {
        await eventBus.emit(EventTypes.ActivityEditor.Rendered, this, this.renderProps);
    }
    async onCancelClick() {
        await this.hide(true);
    }
    render() {
        const renderProps = this.renderProps;
        const activityDescriptor = renderProps.activityDescriptor;
        const tabs = renderProps.tabs;
        const selectedTabName = renderProps.selectedTabName;
        const inactiveClass = 'elsa-border-transparent elsa-text-gray-500 hover:elsa-text-gray-700 hover:elsa-border-gray-300';
        const selectedClass = 'elsa-border-blue-500 elsa-text-blue-600';
        const t = this.t;
        return (h(Host, { key: 'bfc4d8cf843c8a22042c8303edc793f43952ad9d', class: "elsa-block" }, h("elsa-modal-dialog", { key: 'eb5727f14ad3313938934caf4d8054164406fb81', ref: el => this.dialog = el, onShown: this.onDialogShown, onHidden: this.onDialogHidden }, h("div", { key: 'b51ca0399ebeee8635ca317171c02ca10922e5eb', slot: "content", class: "elsa-py-8 elsa-pb-0" }, h("form", { onSubmit: e => this.onSubmit(e), key: this.timestamp.getTime().toString(), onKeyDown: this.onKeyDown, class: 'activity-editor-form' }, h("div", { key: '5dce508038e466127f2fd0abc86decc04755c665', class: "elsa-flex elsa-px-8" }, h("div", { key: 'a36bee486175b720c631d2682b891ad54be6da86', class: "elsa-space-y-8 elsa-divide-y elsa-divide-gray-200 elsa-w-full" }, h("div", { key: '3af18595869ad5d2d188c1e45eea46e870bd80dc' }, h("div", { key: 'b7fe6f2a0648134581bf9913da240e4d5ed6a299' }, h("h3", { key: '024f32d69fbf86ca96cb2b32637a97f4ab534758', class: "elsa-text-lg elsa-leading-6 elsa-font-medium elsa-text-gray-900" }, activityDescriptor.type), h("p", { key: 'e5d4729d8dfc3912a065d76fe646b0f6e9b4bb19', class: "elsa-mt-1 elsa-text-sm elsa-text-gray-500" }, activityDescriptor.description)), h("div", { key: '54bfe7a5093c902555c431db4c9d217eb6a7f094', class: "elsa-border-b elsa-border-gray-200" }, h("nav", { key: 'b4401d1ae35ae8a47ef495718de584f68443e4d9', class: "-elsa-mb-px elsa-flex elsa-space-x-8", "aria-label": "Tabs" }, tabs.map(tab => {
            const isSelected = tab.tabName === selectedTabName;
            const cssClass = isSelected ? selectedClass : inactiveClass;
            return h("a", { href: "#", onClick: e => this.onTabClick(e, tab), class: `${cssClass} elsa-whitespace-nowrap elsa-py-4 elsa-px-1 elsa-border-b-2 elsa-font-medium elsa-text-sm` }, tab.tabName);
        }))), h("div", { key: 'bb88a783bbf9685fab85473d32b0e5c8503ace30', class: "elsa-mt-8" }, this.renderTabs(tabs))))), h("div", { key: '03962a8dd1e45487b4bb9b24ecafee77955c118e', class: "elsa-pt-5" }, h("div", { key: 'c8c2a51b548e227084f8ea5d29a45666eebb8a94', class: "elsa-bg-gray-50 elsa-px-4 elsa-py-3 sm:elsa-px-6 sm:elsa-flex sm:elsa-flex-row-reverse" }, h("button", { key: 'd2c1e20a9f2db8b2954644469cd4acdf679940e4', type: "submit", class: "elsa-ml-3 elsa-inline-flex elsa-justify-center elsa-py-2 elsa-px-4 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500" }, t('Buttons.Save')), h("button", { key: 'c85ee3856f1138f2a2ed07cfb073558d938a2ac4', type: "button", onClick: () => this.onCancelClick(), class: "elsa-w-full elsa-inline-flex elsa-justify-center elsa-rounded-md elsa-border elsa-border-gray-300 elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-bg-white elsa-text-base elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 sm:elsa-mt-0 sm:elsa-ml-3 sm:elsa-w-auto sm:elsa-text-sm" }, t('Buttons.Cancel')))))), h("div", { key: '6a933231ff9effec0e1e31ef8d36f609eae6f54d', slot: "buttons" }))));
    }
    renderTabs(tabs) {
        return tabs.map(x => (h("div", { class: `flex ${this.getHiddenClass(x.tabName)}` }, h("elsa-control", { content: x.renderContent() }))));
    }
    renderStorageTab(activityModel, activityDescriptor) {
        const formContext = this.formContext;
        const t = this.t;
        let storageDescriptorOptions = this.workflowStorageDescriptors.map(x => ({
            value: x.name,
            text: x.displayName
        }));
        let outputProperties = activityDescriptor.outputProperties.filter(x => !x.disableWorkflowProviderSelection);
        let inputProperties = activityDescriptor.inputProperties.filter(x => !x.disableWorkflowProviderSelection);
        storageDescriptorOptions = [{ value: null, text: 'Default' }, ...storageDescriptorOptions];
        const renderPropertyStorageSelectField = function (propertyDescriptor) {
            const propertyName = propertyDescriptor.name;
            const fieldName = `propertyStorageProviders.${propertyName}`;
            return selectField(formContext, fieldName, propertyName, activityModel.propertyStorageProviders[propertyName], storageDescriptorOptions, null, fieldName);
        };
        return (h("div", { class: "elsa-space-y-8 elsa-w-full" }, section('Workflow Context'), checkBox(formContext, 'loadWorkflowContext', 'Load Workflow Context', activityModel.loadWorkflowContext, 'When enabled, this will load the workflow context into memory before executing this activity.', 'loadWorkflowContext'), checkBox(formContext, 'saveWorkflowContext', 'Save Workflow Context', activityModel.saveWorkflowContext, 'When enabled, this will save the workflow context back into storage after executing this activity.', 'saveWorkflowContext'), section('Workflow Instance'), checkBox(formContext, 'persistWorkflow', 'Save Workflow Instance', activityModel.persistWorkflow, 'When enabled, this will save the workflow instance back into storage right after executing this activity.', 'persistWorkflow'), Object.keys(outputProperties).length > 0 ? ([section('Activity Output', 'Configure the desired storage for each output property of this activity.'), outputProperties.map(renderPropertyStorageSelectField)]) : undefined, Object.keys(inputProperties).length > 0 ? ([section('Activity Input', 'Configure the desired storage for each input property of this activity.'), inputProperties.map(renderPropertyStorageSelectField)]) : undefined));
    }
    renderCommonTab(activityModel) {
        const formContext = this.formContext;
        const t = this.t;
        return (h("div", { class: "elsa-space-y-8 elsa-w-full" }, textInput(formContext, 'name', t('Tabs.Common.Fields.Name.Label'), activityModel.name, t('Tabs.Common.Fields.Name.Hint'), 'activityName'), textInput(formContext, 'displayName', t('Tabs.Common.Fields.DisplayName.Label'), activityModel.displayName, t('Tabs.Common.Fields.DisplayName.Hint'), 'activityDisplayName'), textArea(formContext, 'description', t('Tabs.Common.Fields.Description.Label'), activityModel.description, t('Tabs.Common.Fields.Description.Hint'), 'activityDescription')));
    }
    renderPropertiesTab(activityModel) {
        const propertyDescriptors = this.renderProps.defaultProperties;
        if (propertyDescriptors.length == 0)
            return undefined;
        const key = `activity-settings:${activityModel.activityId}`;
        const t = this.t;
        return (h("div", { key: key, class: `elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6` }, propertyDescriptors.map(property => this.renderPropertyEditor(activityModel, property))));
    }
    renderCategoryTab(activityModel, activityDescriptor, category) {
        const propertyDescriptors = activityDescriptor.inputProperties;
        const descriptors = propertyDescriptors.filter(x => x.category == category);
        const key = `activity-settings:${activityModel.activityId}:${category}`;
        return h("div", { key: key, class: `elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6` }, descriptors.map(property => this.renderPropertyEditor(activityModel, property)));
    }
    renderPropertyEditor(activity, property) {
        const key = `activity-property-input:${activity.activityId}:${property.name}`;
        const display = propertyDisplayManager.display(activity, property);
        const id = `${property.name}Control`;
        return h("elsa-control", { key: key, id: id, class: "sm:elsa-col-span-6", content: display });
    }
    getHiddenClass(tab) {
        return this.renderProps.selectedTabName == tab ? '' : 'hidden';
    }
};

export { ElsaActivityEditorModal as elsa_activity_editor_modal };
//# sourceMappingURL=elsa-activity-editor-modal.entry.esm.js.map
