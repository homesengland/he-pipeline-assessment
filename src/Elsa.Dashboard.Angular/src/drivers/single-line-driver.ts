import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { SingleLineProperty } from 'src/components/editors/properties/single-line-property/single-line-property';

@Injectable({
  providedIn: 'root',
})
export class SingleLineDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('SingleLineDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model', propertyModel);
    return {
      componentType: SingleLineProperty,
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
