import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from '../models';
import { getOrCreateProperty, setActivityModelProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { DictionaryProperty } from 'src/components/editors/properties/dictionary-property/dictionary-property';

@Injectable({
  providedIn: 'root',
})
export class DictionaryDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('DictionaryDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Dictionary Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Dictionary Driver', propertyModel);
    return {
      componentType: DictionaryProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal
      },
    };
  }
}
