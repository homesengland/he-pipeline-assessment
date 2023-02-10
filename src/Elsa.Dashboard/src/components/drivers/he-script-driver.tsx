import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../../models/utils";
import { HePropertyDisplayDriver } from "../../models/display-manager";

export class HeScriptDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-script-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: HeProperty, onExpressionChanged: Function) {
    const prop = getOrCreateProperty(activity, property.descriptor.name);
    return <he-script-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={prop.value} onExpressionChanged={e => onExpressionChanged(e, property)} />;
  }
}
