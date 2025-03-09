import { ActivityModel, ActivityPropertyDescriptor, WorkflowStudio } from '../models';
import { PropertyDisplayDriver } from './property-display-driver';
import { NullPropertyDriver } from '../drivers/null-property-driver/null-property-driver';
import { Map } from '../utils/utils';
/*import { SecretModel, SecretPropertyDescriptor } from "../modules/credential-manager/models/secret.model";*/

export type PropertyDisplayDriverMap = Map<(elsaStudio: WorkflowStudio) => PropertyDisplayDriver>;

export class PropertyDisplayManager {
  workflowStudio: WorkflowStudio;
  initialized: boolean;
  drivers: PropertyDisplayDriverMap = {};

  initialize(elsaStudio: WorkflowStudio) {
    if (this.initialized) return;

    this.workflowStudio = elsaStudio;
    this.initialized = true;
  }

  addDriver<T extends PropertyDisplayDriver>(controlType: string, driverFactory: (workflowStudio: WorkflowStudio) => T) {
    this.drivers[controlType] = driverFactory;
  }

  display(model: ActivityModel /* | SecretModel*/, property: ActivityPropertyDescriptor /* | SecretPropertyDescriptor*/, onUpdated?: () => void, isEncrypted?: boolean) {
    const driver = this.getDriver(property.uiHint);
    return driver.display(model, property, onUpdated, isEncrypted);
  }

  update(model: ActivityModel /* | SecretModel*/, property: ActivityPropertyDescriptor /*| SecretPropertyDescriptor*/, form: FormData) {
    const driver = this.getDriver(property.uiHint);
    const update = driver.update;

    if (!update) return;

    return update(model, property, form);
  }

  getDriver(type: string) {
    const driverFactory = this.drivers[type] || ((_: WorkflowStudio) => new NullPropertyDriver());
    return driverFactory(this.workflowStudio);
  }
}

export const propertyDisplayManager = new PropertyDisplayManager();
