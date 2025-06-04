import { ActivityModel, ActivityPropertyDescriptor, WorkflowStudio } from '../models';
import { PropertyDisplayDriver } from './property-display-driver';
import { NullPropertyDriver } from '../drivers/null-property-driver/null-property-driver';
import { Map } from '../utils/utils';
import { Signal } from '@angular/core';
import { SingleLineDriver } from 'src/drivers/single-line-driver';
/*import { SecretModel, SecretPropertyDescriptor } from "../modules/credential-manager/models/secret.model";*/

export type PropertyDisplayDriverMap = Map<(elsaStudio: WorkflowStudio) => PropertyDisplayDriver>;

export class PropertyDisplayManager {
  workflowStudio: WorkflowStudio;
  initialized: boolean;
  drivers: PropertyDisplayDriverMap = {};

  constructor(elsaStudio?: WorkflowStudio) {
    this.initialize(elsaStudio);
  }

  initialize(elsaStudio: WorkflowStudio) {
    if (this.initialized) return;

    this.workflowStudio = elsaStudio;
   // this.addDriver('SingleLine', (studio) => new SingleLineDriver());
    this.initialized = true;
  }

  addDriver<T extends PropertyDisplayDriver>(controlType: string, driverFactory: (workflowStudio: WorkflowStudio) => T) {
    this.drivers[controlType] = driverFactory;
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    const driver = this.getDriver(property().uiHint);
    return driver.display(model, property, onUpdated, isEncrypted);
  }

  update(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, form: FormData) {
    const driver = this.getDriver(property().uiHint);
    const update = driver.update;

    if (!update) return;

    return update(model, property, form);
  }

  getDriver(type: string) : PropertyDisplayDriver {
    const driverFactory = this.drivers[type] || ((_: WorkflowStudio) => new NullPropertyDriver());
    return driverFactory(this.workflowStudio);
  }
}

export const propertyDisplayManager = new PropertyDisplayManager();
