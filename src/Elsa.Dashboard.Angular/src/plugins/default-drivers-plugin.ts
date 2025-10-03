import { from } from 'rxjs';
import { propertyDisplayManager } from '../services/property-display-manager';
import { SingleLineDriver } from '../drivers/single-line-driver';
import { MultiLineDriver } from '../drivers/multi-line-driver';
import { CheckboxDriver } from '../drivers/checkbox-driver';
import { WorkflowPlugin } from 'src/services/workflow-plugin';
import { PropertyDisplayDriver } from 'src/services/property-display-driver';
import { WorkflowStudio } from 'src/models';
import { JsonDriver } from 'src/drivers/json-driver';
import { DropDownDriver } from 'src/drivers/drop-down-driver';
import { CheckListDriver } from 'src/drivers/check-list-driver';
import { RadioListDriver } from 'src/drivers/radio-list-driver';
import { MultiTextDriver } from 'src/drivers/multi-text-driver';
import { DictionaryDriver } from 'src/drivers/dictionary-driver';
import { SwitchCaseDriver } from '../drivers/switch-case-driver';
import { RadioOptionsDriver } from 'src/drivers/radio-options-driver';

export class DefaultDriversPlugin implements WorkflowPlugin {
  constructor() {
    this.addDriver('single-line', () => new SingleLineDriver());
    this.addDriver('multi-line', () => new MultiLineDriver());
    this.addDriver('checkbox', () => new CheckboxDriver());
    this.addDriver('json', () => new JsonDriver());
    this.addDriver('dropdown', () => new DropDownDriver());
    this.addDriver('check-list', () => new CheckListDriver());
    this.addDriver('radio-list', () => new RadioListDriver());
    this.addDriver('multi-text', () => new MultiTextDriver());
    this.addDriver('dictionary', () => new DictionaryDriver());
    this.addDriver('switch-case', () => new SwitchCaseDriver());
    this.addDriver('radio-options', () => new RadioOptionsDriver());
  }

  addDriver<T extends PropertyDisplayDriver>(controlType: string, c: (workflowStudio: WorkflowStudio) => T) {
    console.log("Adding Driver", controlType);
    propertyDisplayManager.addDriver(controlType, c);
  }
}
