import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { NestedProperty } from "../models/custom-component-models";

export class HePotScoreRadioOptionsDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-potscore-radio-options-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function) {
    console.log('activity HePotScoreRadioOptionsDriver', activity);
    console.log('property HePotScoreRadioOptionsDriver', property);
    return <he-potscore-radio-options-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} />;
  }
}
