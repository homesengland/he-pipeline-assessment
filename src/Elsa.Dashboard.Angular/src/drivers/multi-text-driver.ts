import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal } from '@angular/core';
import { MultiTextProperty } from 'src/components/editors/properties/multi-text-property/multi-text-property';

@Injectable({
  providedIn: 'root',
})
export class MultiTextDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('MultiTextDriver initialised');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Multi Text Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Multi Text Driver', propertyModel);
    return {
      componentType: MultiTextProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal,
      },
    };
  }
}
