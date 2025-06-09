import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty, setActivityModelProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { CheckboxProperty } from 'src/components/editors/properties/checkbox-property/checkbox-property';

@Injectable({
  providedIn: 'root',
})
export class CheckboxDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('CheckboxDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Checkbox Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Checkbox Driver', propertyModel);
    return {
      componentType: CheckboxProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal
      },
    };
  }
}
