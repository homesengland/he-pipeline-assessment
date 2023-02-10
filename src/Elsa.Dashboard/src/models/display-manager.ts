import { HeCheckListDriver } from "../components/drivers/he-check-list-driver";
import { HeCheckboxDriver } from "../components/drivers/he-checkbox-driver";
import { HeCronDriver } from "../components/drivers/he-cron-driver";
import { HeDictionaryDriver } from "../components/drivers/he-dictionary-driver";
import { HeDropdownDriver } from "../components/drivers/he-dropdown-driver";
import { HeJsonDriver } from "../components/drivers/he-json-driver";
import { HeMultiLineDriver } from "../components/drivers/he-multi-line-driver";
import { HeMultiTextDriver } from "../components/drivers/he-multi-text-driver";
import { HeRadioListDriver } from "../components/drivers/he-radio-list-driver";
import { HeScriptDriver } from "../components/drivers/he-script-driver";
import { HeSingleLineDriver } from "../components/drivers/he-single-line-driver";
import { HeSwitchCaseDriver } from "../components/drivers/he-switch-cases-driver";
import { HeProperty } from "./custom-component-models";
import { ActivityModel, ActivityPropertyDescriptor } from "./elsa-interfaces";


//export interface DefaultDrivers {
//  drivers: Record<string, HePropertyDisplayDriver>
//}

//export class DefaultDriversFactory implements DefaultDrivers {
//  constructor() {
//    this.drivers["single-line"] = new HeSingleLineDriver();
//    this.drivers["multi-line"] = new HeMultiLineDriver();
//    this.drivers["json"] = new HeJsonDriver();
//    this.drivers["check-list"] = new HeCheckListDriver();
//    this.drivers["radio-list"] = new HeRadioListDriver();
//    this.drivers["checkbox"] = new HeCheckboxDriver();
//    this.drivers["dropdown"] = new HeDropdownDriver();
//    this.drivers["multi-text"] = new HeMultiTextDriver();
//    this.drivers["code-editor"] = new HeScriptDriver();
//    this.drivers["switch-case-builder"] = new HeSwitchCaseDriver();
//    this.drivers["dictionary"] = new HeDictionaryDriver();
//    this.drivers["cron-expression"] = new HeCronDriver();
//  }
//    drivers: Record<string, HePropertyDisplayDriver>;
//}

export interface HePropertyDisplayDriver {
  display(model: ActivityModel, property: ActivityPropertyDescriptor)
  displayNested(model: ActivityModel, property: HeProperty, onUpdate: Function)
}

export class DefaultDriversFactory implements Record<string, HePropertyDisplayDriver> {
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
  }
    [uiHint: string]: HePropertyDisplayDriver;
}



export class HePropertyDisplayManager {

  driverFactory: DefaultDriversFactory = new DefaultDriversFactory();

  getDriver(type: string) {
    return this.driverFactory[type];
  }

  display(model: ActivityModel, property: ActivityPropertyDescriptor) {
    const driver = this.getDriver(property.uiHint);
    return driver.display(model, property)
  }

  displayNested(model: ActivityModel, property: HeProperty, onUpdate: Function) {
    const driver = this.getDriver(property.descriptor.uiHint);
    return driver.displayNested(model, property, onUpdate)
  }
}


