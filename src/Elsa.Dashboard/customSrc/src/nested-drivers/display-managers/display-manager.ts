import { HeCheckListDriver } from "../he-check-list-driver";
import { HeCheckboxDriver } from "../he-checkbox-driver";
import { HeCheckboxOptionsDriver } from "../he-checkbox-options-driver";
import { HeCronDriver } from "../he-cron-driver";
import { HeDictionaryDriver } from "../he-dictionary-driver";
import { HeDropdownDriver } from "../he-dropdown-driver";
import { HeJsonDriver } from "../he-json-driver";
import { HeMultiLineDriver } from "../he-multi-line-driver";
import { HeMultiTextDriver } from "../he-multi-text-driver";
import { HeRadioListDriver } from "../he-radio-list-driver";
import { HeRadioOptionsDriver } from "../he-radio-options-driver";
import { HePotScoreRadioOptionsDriver } from "../he-potscore-radio-options-driver";
import { HeScriptDriver } from "../he-script-driver";
import { HeSingleLineDriver } from "../he-single-line-driver";
import { HeSwitchCaseDriver } from "../he-switch-cases-driver";
import { HeTextActivityDriver } from "../he-text-activity-driver";
import { HeDataTableDriver } from "../he-data-table-driver";
import {  Dictionary, NestedProperty } from "../../models/custom-component-models";
import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { HeQuestionDataDictionaryDriver } from "../he-question-data-dictionary-driver";
import { HeWeightedRadioDriver } from "../he-weighted-radio-driver";
import { HeWeightedCheckboxDriver } from "../he-weighted-checkbox-driver";
import { HeNumericDriver } from "../he-numeric-driver";
import { HeTextGroupDriver } from "../he-text-group-driver";
import { HeValidationDriver } from "../he-validation-driver"


export interface HePropertyDisplayDriver {
  display(model: ActivityModel, property: ActivityPropertyDescriptor)
  displayNested(model: ActivityModel, property: NestedProperty, onUpdate: Function)
}

export class DefaultDriversFactory {
  constructor() {
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
  drivers: Dictionary<HePropertyDisplayDriver> = {};
}



export class HePropertyDisplayManager {
  constructor() {
    this.driverFactory = new DefaultDriversFactory();
  }

  driverFactory: DefaultDriversFactory;

  getDriver(type: string) {
    return this.driverFactory.drivers[type];
  }

  display(model: ActivityModel, property: ActivityPropertyDescriptor) {
    const driver: HePropertyDisplayDriver = this.getDriver(property.uiHint);
    return driver.display(model, property)
  }

  displayNested(model: ActivityModel, property: NestedProperty, onUpdate: Function) {
    const driver: HePropertyDisplayDriver = this.getDriver(property.descriptor.uiHint);

    return driver.displayNested(model, property, onUpdate)
  }
}


