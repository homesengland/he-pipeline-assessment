import { PropertyDisplayDriver } from "../services";
import { ActivityModel, ActivityPropertyDescriptor } from "../models";
export declare class JsonDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor): any;
  update(activity: ActivityModel, property: ActivityPropertyDescriptor, form: FormData): void;
}
