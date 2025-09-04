import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from '../models';
import { getOrCreateProperty, setActivityModelProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { HeRadioOptionProperty } from 'src/components/common-properties/he-radio-option-property/he-radio-option-property';

@Injectable({
  providedIn: 'root',
})
export class HeRadioOptionsDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('HeRadioOptionsDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('HeRadioOptions Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for HeRadioOptions Driver', propertyModel);
    return {
      componentType: HeRadioOptionProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal
      },
    };
  }
}
