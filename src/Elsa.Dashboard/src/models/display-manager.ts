import { HeCheckListDriver } from "../components/drivers/he-check-list-driver";
import { HeCheckboxDriver } from "../components/drivers/he-checkbox-driver";
import { HeCheckboxOptionsDriver } from "../components/drivers/he-checkbox-options-driver";
import { HeCronDriver } from "../components/drivers/he-cron-driver";
import { HeDictionaryDriver } from "../components/drivers/he-dictionary-driver";
import { HeDropdownDriver } from "../components/drivers/he-dropdown-driver";
import { HeJsonDriver } from "../components/drivers/he-json-driver";
import { HeMultiLineDriver } from "../components/drivers/he-multi-line-driver";
import { HeMultiTextDriver } from "../components/drivers/he-multi-text-driver";
import { HeRadioListDriver } from "../components/drivers/he-radio-list-driver";
import { HeRadioOptionsDriver } from "../components/drivers/he-radio-options-driver";
import { HeScriptDriver } from "../components/drivers/he-script-driver";
import { HeSingleLineDriver } from "../components/drivers/he-single-line-driver";
import { HeSwitchCaseDriver } from "../components/drivers/he-switch-cases-driver";
import { Dictionary, NestedProperty } from "./custom-component-models";
import { ActivityModel, ActivityPropertyDescriptor } from "./elsa-interfaces";


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
    this.drivers["he-checkbox-options"] = new HeCheckboxOptionsDriver();
  }
  drivers: Dictionary<HePropertyDisplayDriver> = { };
}



export class HePropertyDisplayManager {
  constructor() {
    this.driverFactory = new DefaultDriversFactory();
  }

  driverFactory: DefaultDriversFactory;

  getDriver(type: string) {
    console.log("Key", type);
    console.log("Factory", this.driverFactory);
    console.log("Drivers:", this.driverFactory.drivers)
    return this.driverFactory.drivers[type];
  }

  display(model: ActivityModel, property: ActivityPropertyDescriptor) {
    const driver: HePropertyDisplayDriver = this.getDriver(property.uiHint);
    return driver.display(model, property)
  }

  displayNested(model: ActivityModel, property: NestedProperty, onUpdate: Function) {
    const driver: HePropertyDisplayDriver = this.getDriver(property.descriptor.uiHint);
    console.log("Driver", driver);
    return driver.displayNested(model, property, onUpdate)
  }
}


