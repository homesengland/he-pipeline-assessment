import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { SingleLineProperty } from 'src/components/editors/properties/single-line-property/single-line-property';
import { JsonProperty } from 'src/components/editors/properties/json-property/json-property';

@Injectable({
  providedIn: 'root',
})
export class JsonDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('SingleLineDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Json Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Json Driver', propertyModel);
    return {
      componentType: JsonProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal,
      },
    };
  }
}
