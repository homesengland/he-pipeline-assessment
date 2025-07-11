import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
export class HeWeightedRadioDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        return h("he-weighted-radio-property", { activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        return h("he-weighted-radio-property", { activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
