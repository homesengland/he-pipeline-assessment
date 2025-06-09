import { from } from 'rxjs';
import { propertyDisplayManager } from '../services/property-display-manager';
import { SingleLineDriver } from '../drivers/single-line-driver';
import { MultiLineDriver } from '../drivers/multi-line-driver';
import { WorkflowPlugin } from 'src/services/workflow-plugin';
import { PropertyDisplayDriver } from 'src/services/property-display-driver';
import { WorkflowStudio } from 'src/models';
import { JsonDriver } from 'src/drivers/json-driver';
// Import DropDownDriver if it exists
import { DropDownDriver } from 'src/drivers/drop-down-driver';

export class DefaultDriversPlugin implements WorkflowPlugin {
  constructor() {
    this.addDriver('single-line', () => new SingleLineDriver());
    this.addDriver('multi-line', () => new MultiLineDriver());
    this.addDriver('json', () => new JsonDriver());
    this.addDriver('json', () => new DropDownDriver());
  }

  addDriver<T extends PropertyDisplayDriver>(controlType: string, c: (workflowStudio: WorkflowStudio) => T) {
    propertyDisplayManager.addDriver(controlType, c);
  }
}
