import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
export class HeSingleLineDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        return h("he-single-line-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return (h("he-single-line-property", { class: "sm:elsa-col-span-6", activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) }));
    }
}
