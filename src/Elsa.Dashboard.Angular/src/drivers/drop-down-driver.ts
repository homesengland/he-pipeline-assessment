import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { DropDownProperty } from 'src/components/editors/properties/drop-down-property/drop-down-property';

@Injectable({
  providedIn: 'root',
})
export class DropDownDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('DropDownDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log('Drop Down Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Drop Down Driver', propertyModel);
    return {
      componentType: DropDownProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal,
      },
    };
  }
}
