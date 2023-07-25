import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty, getUniversalUniqueId } from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { NestedProperty } from "../models/custom-component-models";

export class HeCheckListDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    const keyId = getUniversalUniqueId();
    return <he-check-list-property key={`${keyId}`} activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function) {
    const keyId = getUniversalUniqueId();
    return <he-check-list-property key={`${keyId}`}  activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property    )} />;
  }
}
