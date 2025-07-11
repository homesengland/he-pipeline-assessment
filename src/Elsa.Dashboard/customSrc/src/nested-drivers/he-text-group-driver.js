import { h } from "@stencil/core";
import { getOrCreateNestedProperty } from "../utils/utils";
export class HeTextGroupDriver {
    //Display will never be called in current incarnation, as when it is generated directly on the conclusion screen, it is done so via the Plugin Driver
    display(activity, property) {
        const prop = getOrCreateNestedProperty(activity, property.name, '');
        return h("he-text-group-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return h("he-text-group-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
