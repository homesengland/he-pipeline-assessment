import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateNestedProperty} from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { NestedProperty } from "../models/custom-component-models";

export class HeTextGroupDriver implements HePropertyDisplayDriver {

  //Display will never be called in current incarnation, as when it is generated directly on the conclusion screen, it is done so via the Plugin Driver
  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateNestedProperty(activity, property.name, '');
    return <he-text-group-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function) {
    return <he-text-group-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} />;
  }
}
