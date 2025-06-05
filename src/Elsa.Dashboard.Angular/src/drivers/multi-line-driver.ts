import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { MultiLineProperty } from 'src/components/editors/properties/multi-line-property/multi-line-property';

@Injectable({
  providedIn: 'root',
})
export class MultiLineDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('MultiLineDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log("Multi Line Driver Setup");
    console.log("Activity Model", model());
    console.log("Property Model", property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Multi Line Driver', propertyModel);
    return {
      componentType: MultiLineProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal,
        isEncrypted: isEncrypted,
        propertyChanged: onUpdated,
      },
    };
  }
}