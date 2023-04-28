import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";
import { HePropertyDisplayDriver } from "./display-managers/display-manager";
import { DataDictionaryGroup,NestedProperty } from "../models/custom-component-models";

export class HeQuestionDataDictionaryDriver implements HePropertyDisplayDriver {

  display(activity: ActivityModel, property: ActivityPropertyDescriptor) {
    const prop = getOrCreateProperty(activity, property.name);
    return <he-question-data-dictionary-property activityModel={activity} propertyDescriptor={property} propertyModel={prop} />;
  }

  displayNested(activity: ActivityModel, property: NestedProperty, onExpressionChanged: Function, dataDictionaryGroup: Array<DataDictionaryGroup>) {
    console.log("Loading Dicitionary Component --- dataDictionaryGroup --- value", dataDictionaryGroup)
    return <he-question-data-dictionary-property activityModel={activity} propertyDescriptor={property.descriptor} propertyModel={property.value} onExpressionChanged={e => onExpressionChanged(e, property)} dataDictionaryGroup={dataDictionaryGroup} />;
  }
}
