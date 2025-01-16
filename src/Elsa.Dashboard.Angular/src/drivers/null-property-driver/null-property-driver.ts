import { PropertyDisplayDriver } from "../../services/property-display-driver";
import { ActivityModel, ActivityPropertyDescriptor } from "../../models";

export class NullPropertyDriver implements PropertyDisplayDriver {


  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    return undefined;
  }

  update(activity: ActivityModel, property: ActivityPropertyDescriptor, form: FormData) {
  }

}

