import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
export class HeJsonDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        return h("he-json-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return h("he-json-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
