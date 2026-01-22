import { h } from './index-1542df5c.js';
import { b as getOrCreateProperty, g as getUniversalUniqueId, c as getOrCreateNestedProperty } from './utils-89b7e981.js';

class HeCheckListDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    const keyId = getUniversalUniqueId();
    return h("he-check-list-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    const keyId = getUniversalUniqueId();
    return h("he-check-list-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeCheckboxDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-checkbox-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-checkbox-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeCheckboxOptionsDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-checkbox-options-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-checkbox-options-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeCronDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-cron-expression-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-cron-expression-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeDictionaryDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    const keyId = getUniversalUniqueId();
    return h("he-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    const keyId = getUniversalUniqueId();
    return h("he-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeDropdownDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-dropdown-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-dropdown-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeJsonDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-json-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-json-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeMultiLineDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-multi-line-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-multi-line-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeMultiTextDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-multi-text-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-multi-text-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeRadioListDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-radio-list-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-radio-list-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeRadioOptionsDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-radio-options-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-radio-options-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HePotScoreRadioOptionsDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-potscore-radio-options-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-potscore-radio-options-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeScriptDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-script-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-script-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeSingleLineDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-single-line-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return (h("he-single-line-property", { class: "sm:elsa-col-span-6", activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) }));
  }
}

class HeSwitchCaseDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-switch-cases-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-switch-cases-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeTextActivityDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-text-activity-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-text-activity-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeDataTableDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    const keyId = getUniversalUniqueId();
    return h("he-data-table-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    const keyId = getUniversalUniqueId();
    return h("he-data-table-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeQuestionDataDictionaryDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    const keyId = getUniversalUniqueId();
    return h("he-question-data-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    const keyId = getUniversalUniqueId();
    return h("he-question-data-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeWeightedRadioDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-weighted-radio-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-weighted-radio-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeWeightedCheckboxDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-weighted-checkbox-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-weighted-checkbox-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeNumericDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-numeric-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return (h("he-numeric-property", { class: "sm:elsa-col-span-6", activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) }));
  }
}

class HeTextGroupDriver {
  //Display will never be called in current incarnation, as when it is generated directly on the conclusion screen, it is done so via the Plugin Driver
  display(activity, property) {
    const prop = getOrCreateNestedProperty(activity, property.name, '');
    return h("he-text-group-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-text-group-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class HeValidationDriver {
  display(activity, property) {
    const prop = getOrCreateProperty(activity, property.name);
    return h("he-validation-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
  }
  displayNested(activity, property, onExpressionChanged) {
    return h("he-validation-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
  }
}

class DefaultDriversFactory {
  constructor() {
    this.drivers = {};
    this.drivers["he-single-line"] = new HeSingleLineDriver();
    this.drivers["he-multi-line"] = new HeMultiLineDriver();
    this.drivers["he-json"] = new HeJsonDriver();
    this.drivers["he-check-list"] = new HeCheckListDriver();
    this.drivers["he-radio-list"] = new HeRadioListDriver();
    this.drivers["he-checkbox"] = new HeCheckboxDriver();
    this.drivers["he-dropdown"] = new HeDropdownDriver();
    this.drivers["he-multi-text"] = new HeMultiTextDriver();
    this.drivers["he-code-editor"] = new HeScriptDriver();
    this.drivers["he-switch-case-builder"] = new HeSwitchCaseDriver();
    this.drivers["he-dictionary"] = new HeDictionaryDriver();
    this.drivers["he-cron-expression"] = new HeCronDriver();
    this.drivers["he-radio-options"] = new HeRadioOptionsDriver();
    this.drivers["he-potscore-radio-options"] = new HePotScoreRadioOptionsDriver();
    this.drivers["he-checkbox-options"] = new HeCheckboxOptionsDriver();
    this.drivers["he-text-activity"] = new HeTextActivityDriver();
    this.drivers["he-question-data-dictionary"] = new HeQuestionDataDictionaryDriver();
    this.drivers["he-weighted-radio"] = new HeWeightedRadioDriver();
    this.drivers["he-weighted-checkbox"] = new HeWeightedCheckboxDriver();
    this.drivers["he-data-table"] = new HeDataTableDriver();
    this.drivers["he-numeric"] = new HeNumericDriver();
    this.drivers["he-text-group"] = new HeTextGroupDriver();
    this.drivers["he-validation-property"] = new HeValidationDriver();
  }
}
class HePropertyDisplayManager {
  constructor() {
    this.driverFactory = new DefaultDriversFactory();
  }
  getDriver(type) {
    return this.driverFactory.drivers[type];
  }
  display(model, property) {
    const driver = this.getDriver(property.uiHint);
    return driver.display(model, property);
  }
  displayNested(model, property, onUpdate) {
    const driver = this.getDriver(property.descriptor.uiHint);
    return driver.displayNested(model, property, onUpdate);
  }
}

export { HePropertyDisplayManager as H };
