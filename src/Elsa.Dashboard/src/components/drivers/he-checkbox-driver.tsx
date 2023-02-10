import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../../models/utils";
import { HePropertyDisplayDriver } from "../../models/display-driver";

export class HECheckboxDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor, onUpdate: Function) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-checkbox-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} onUpdate={onUpdate} />;
  }
}
