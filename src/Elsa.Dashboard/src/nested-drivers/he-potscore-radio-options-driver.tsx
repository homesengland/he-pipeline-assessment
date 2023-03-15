
import { ActivityModel, ActivityPropertyDescriptor } from "../models/elsa-interfaces";
import { HePotscoreRadioPropertyDisplayDriver } from "./display-managers/potscore-radio-display-manager";
import { h } from "@stencil/core";
import { getOrCreateProperty } from "../utils/utils";

export class HePotScoreRadioOptionsDriver implements HePotscoreRadioPropertyDisplayDriver {
  displayWithPotScoreOptions(model: ActivityModel, property: ActivityPropertyDescriptor, potScoreOptions: Array<any>) {
    const prop = getOrCreateProperty(model, property.name);
    return <he-potscore-radio-options-property activityModel={model} propertyDescriptor={property} propertyModel={prop} potScoreOptions={potScoreOptions} />;
  }
}
