import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty, setActivityModelProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { RadioListProperty } from 'src/components/editors/properties/radio-list-property/radio-list-property';

@Injectable({
  providedIn: 'root',
})
export class RadioListDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('RadioListDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Radio List Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Radio List Driver', propertyModel);
    return {
      componentType: RadioListProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal
      },
    };
  }
}
