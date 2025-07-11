import { h } from "@stencil/core";
import { getOrCreateProperty, getUniversalUniqueId } from "../utils/utils";
export class HeDictionaryDriver {
    display(activity, property) {
        const prop = getOrCreateProperty(activity, property.name);
        const keyId = getUniversalUniqueId();
        return h("he-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property, propertyModel: prop });
    }
    displayNested(activity, property, onExpressionChanged) {
        const keyId = getUniversalUniqueId();
        return h("he-dictionary-property", { key: `${keyId}`, activityModel: activity, propertyDescriptor: property.descriptor, propertyModel: property.value, onExpressionChanged: e => onExpressionChanged(e, property) });
    }
}
