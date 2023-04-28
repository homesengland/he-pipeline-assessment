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
import { DataDictionaryGroup, Dictionary, NestedProperty } from "../../models/custom-component-models";
import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { HeQuestionDataDictionaryDriver } from "../he-question-data-dictionary-driver";


export interface HePropertyDisplayDriver {
  display(model: ActivityModel, property: ActivityPropertyDescriptor)
  displayNested(model: ActivityModel, property: NestedProperty, onUpdate: Function, dataDictionaryGroup: Array<DataDictionaryGroup>)
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

  displayNested(model: ActivityModel, property: NestedProperty, onUpdate: Function, dataDictionaryGroup: Array<DataDictionaryGroup> = null) {
    const driver: HePropertyDisplayDriver = this.getDriver(property.descriptor.uiHint);

    return driver.displayNested(model, property, onUpdate, dataDictionaryGroup)
  }
}


