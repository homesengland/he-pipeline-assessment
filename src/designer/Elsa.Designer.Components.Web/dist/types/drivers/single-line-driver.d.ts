import { PropertyDisplayDriver } from "../services";
import { ActivityModel, ActivityPropertyDescriptor } from "../models";
export declare class SingleLineDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor): any;
}
