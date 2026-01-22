import { ActivityModel, ActivityPropertyDescriptor, ElsaStudio } from "../models/";
import { PropertyDisplayDriver } from "./property-display-driver";
import { Map } from '../utils/utils';
import { SecretModel, SecretPropertyDescriptor } from "../modules/credential-manager/models/secret.model";
export declare type PropertyDisplayDriverMap = Map<(elsaStudio: ElsaStudio) => PropertyDisplayDriver>;
export declare class PropertyDisplayManager {
  elsaStudio: ElsaStudio;
  initialized: boolean;
  drivers: PropertyDisplayDriverMap;
  initialize(elsaStudio: ElsaStudio): void;
  addDriver<T extends PropertyDisplayDriver>(controlType: string, driverFactory: (elsaStudio: ElsaStudio) => T): void;
  display(model: ActivityModel | SecretModel, property: ActivityPropertyDescriptor | SecretPropertyDescriptor, onUpdated?: () => void): any;
  update(model: ActivityModel | SecretModel, property: ActivityPropertyDescriptor | SecretPropertyDescriptor, form: FormData): any;
  getDriver(type: string): PropertyDisplayDriver;
}
export declare const propertyDisplayManager: PropertyDisplayManager;
