import { ActivityModel, ActivityPropertyDescriptor } from "../../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../../models/utils";
import { HePropertyDisplayDriver } from "../../models/display-manager";
import { HeProperty } from "../../models/custom-component-models";

export class HeSingleLineDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-single-line-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: HeProperty, onExpressionChanged: Function) {
    return <he-single-line-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} />
  }
}
