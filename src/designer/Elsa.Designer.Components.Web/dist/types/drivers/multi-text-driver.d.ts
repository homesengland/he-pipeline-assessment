import { PropertyDisplayDriver } from "../services/property-display-driver";
import { ActivityModel, ActivityPropertyDescriptor } from "../models";
export declare class MultiTextDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor, onUpdated?: () => void): any;
  update(activity: ActivityModel, property: ActivityPropertyDescriptor, form: FormData): void;
}
