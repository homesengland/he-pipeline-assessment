import { PropertyDisplayDriver } from "../services/property-display-driver";
import { ActivityModel, ActivityPropertyDescriptor } from "../models";
export declare class CronExpressionDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor): any;
  update(activity: ActivityModel, property: ActivityPropertyDescriptor, form: FormData): void;
}
