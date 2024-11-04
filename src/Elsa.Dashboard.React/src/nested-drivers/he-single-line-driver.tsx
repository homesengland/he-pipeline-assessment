import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { NestedProperty } from "../models/custom-component-models";

export class HeSingleLineDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-single-line-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function) {
    return(
      <he-single-line-property class="sm:elsa-col-span-6" activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} />
      )
  }
}
