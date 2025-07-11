import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { HeActivityPropertyDescriptor, NestedActivityDefinitionProperty, NestedProperty } from "../models/custom-component-models";

export class HeRadioOptionsDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-radio-options-property activityModel={activity} propertyDescriptor={property as HeActivityPropertyDescriptor} propertyModel={prop as NestedActivityDefinitionProperty} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function) {
    return <he-radio-options-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} />;
  }
}
