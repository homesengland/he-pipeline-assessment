import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../../models/utils";
import { HePropertyDisplayDriver } from "../../models/display-driver";

export class HECheckListDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor, onUpdate: Function) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-checklist-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} onUpdate={onUpdate} />;
  }
}
