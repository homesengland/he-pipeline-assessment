import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
export class HeDropdownDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        return h("he-dropdown-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return h("he-dropdown-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
