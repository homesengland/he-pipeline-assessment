import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
export class HeSwitchCaseDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        return h("he-switch-cases-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return h("he-switch-cases-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
