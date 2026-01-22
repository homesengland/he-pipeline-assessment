import { PropertyDisplayDriver } from "../services";
import { ActivityModel, ActivityPropertyDescriptor } from "../models";
export declare class DictionaryDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor): any;
  update(activity: ActivityModel, property: ActivityPropertyDescriptor, form: FormData): void;
}
