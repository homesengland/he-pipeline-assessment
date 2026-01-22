import { r as registerInstance, h } from './index-ea213ee1.js';
import { b as propertyDisplayManager } from './index-e19c34cd.js';
import { a as state } from './store-52e2ea41.js';
import { E as EventTypes } from './index-0f68dbd6.js';
import { F as FormContext, s as selectField, a as section, c as checkBox, t as textInput, b as textArea } from './forms-0aa787e1.js';
import { l as loadTranslations } from './i18n-loader-aa6cec69.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import './elsa-client-17ed10a4.js';
import './axios-middleware.esm-fcda64d5.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

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

let ElsaActivityEditorPanel = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.workflowStorageDescriptors = [];
    this.renderProps = {};
    this.updateCounter = 0;
    // Force a new key every time we show the editor to make sure Stencil creates new components.
    // This prevents the issue where the designer has e.g. one activity where the user edits the properties, cancels out, then opens the editor again, seeing the entered value still there.
    this.timestamp = new Date();
    this.t = (key) => this.i18next.t(key);
    this.onShowActivityEditor = async (activity, animate) => {
      this.activityModel = activity;
      this.activityDescriptor = state.activityDescriptors.find(x => x.type == activity.type);
      this.workflowStorageDescriptors = state.workflowStorageDescriptors;
      this.formContext = new FormContext(this.activityModel, newValue => (this.activityModel = newValue));
      this.timestamp = new Date();
      this.renderProps = {};
      this.updateCounter++;
    };
    this.onChange = async (e) => {
      const formData = new FormData(this.formElement);
      this.updateActivity(formData);
      await eventBus.emit(EventTypes.UpdateActivity, this, this.activityModel);
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
      customAttributes: {},
    };
    const propertyCategories = activityDescriptor.inputProperties
      .filter(x => x.category)
      .map(x => x.category)
      .distinct();
    const defaultProperties = activityDescriptor.inputProperties.filter(x => !x.category || x.category.length == 0);
    const activityModel = this.activityModel || {
      type: '',
      activityId: '',
      outcomes: [],
      properties: [],
      propertyStorageProviders: {},
    };
    const t = this.t;
    let tabs = [];
    if (defaultProperties.length > 0) {
      tabs.push({
        tabId: 'properties',
        tabName: t('Tabs.Properties.Name'),
        renderContent: () => this.renderPropertiesTab(activityModel),
      });
    }
    for (const category of propertyCategories) {
      const categoryTab = {
        tabId: 'categories',
        tabName: category,
        renderContent: () => this.renderCategoryTab(activityModel, activityDescriptor, category),
      };
      tabs.push(categoryTab);
    }
    tabs.push({
      tabId: 'common',
      tabName: t('Tabs.Common.Name'),
      renderContent: () => this.renderCommonTab(activityModel),
    });
    tabs.push({
      tabId: 'storage',
      tabName: t('Tabs.Storage.Name'),
      renderContent: () => this.renderStorageTab(activityModel, activityDescriptor),
    });
    this.renderProps = {
      activityDescriptor,
      activityModel,
      propertyCategories,
      defaultProperties,
      tabs,
      selectedTabName: this.renderProps.selectedTabName,
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
  render() {
    const renderProps = this.renderProps;
    const tabs = renderProps.tabs;
    return (h("form", { onChange: this.onChange, ref: el => this.formElement = el }, h("elsa-flyout-panel", { autoExpand: true, silent: true, updateCounter: this.updateCounter }, tabs.map(tab => [
      h("elsa-tab-header", { tab: tab.tabName, slot: "header" }, tab.tabName),
      h("elsa-tab-content", { tab: tab.tabName, slot: "content" }, h("div", { class: "elsa-pt-4 elsa-ml-1" }, h("elsa-control", { content: tab.renderContent() }))),
    ]))));
  }
  renderStorageTab(activityModel, activityDescriptor) {
    const formContext = this.formContext;
    const t = this.t;
    let storageDescriptorOptions = this.workflowStorageDescriptors.map(x => ({
      value: x.name,
      text: x.displayName,
    }));
    let outputProperties = activityDescriptor.outputProperties.filter(x => !x.disableWorkflowProviderSelection);
    let inputProperties = activityDescriptor.inputProperties.filter(x => !x.disableWorkflowProviderSelection);
    storageDescriptorOptions = [{ value: null, text: 'Default' }, ...storageDescriptorOptions];
    const renderPropertyStorageSelectField = function (propertyDescriptor) {
      const propertyName = propertyDescriptor.name;
      const fieldName = `propertyStorageProviders.${propertyName}`;
      return selectField(formContext, fieldName, propertyName, activityModel.propertyStorageProviders[propertyName], storageDescriptorOptions, null, fieldName);
    };
    return (h("div", { class: "elsa-space-y-8 elsa-w-full" }, section('Workflow Context'), checkBox(formContext, 'loadWorkflowContext', 'Load Workflow Context', activityModel.loadWorkflowContext, 'When enabled, this will load the workflow context into memory before executing this activity.', 'loadWorkflowContext'), checkBox(formContext, 'saveWorkflowContext', 'Save Workflow Context', activityModel.saveWorkflowContext, 'When enabled, this will save the workflow context back into storage after executing this activity.', 'saveWorkflowContext'), section('Workflow Instance'), checkBox(formContext, 'persistWorkflow', 'Save Workflow Instance', activityModel.persistWorkflow, 'When enabled, this will save the workflow instance back into storage right after executing this activity.', 'persistWorkflow'), Object.keys(outputProperties).length > 0
      ? [section('Activity Output', 'Configure the desired storage for each output property of this activity.'), outputProperties.map(renderPropertyStorageSelectField)]
      : undefined, Object.keys(inputProperties).length > 0
      ? [section('Activity Input', 'Configure the desired storage for each input property of this activity.'), inputProperties.map(renderPropertyStorageSelectField)]
      : undefined));
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
    return (h("div", { key: key, class: `elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6` }, descriptors.map(property => this.renderPropertyEditor(activityModel, property))));
  }
  renderPropertyEditor(activity, property) {
    const key = `activity-property-input:${activity.activityId}:${property.name}`;
    const display = propertyDisplayManager.display(activity, property, this.onChange);
    const id = `${property.name}Control`;
    return h("elsa-control", { key: key, id: id, class: "sm:elsa-col-span-6", content: display });
  }
};

export { ElsaActivityEditorPanel as elsa_activity_editor_panel };
