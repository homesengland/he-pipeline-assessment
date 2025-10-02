import { Injectable, signal, Signal } from '@angular/core';
import { RadioOptionProperty } from 'src/components/editors/complex-properties/radio-option-property/radio-option-property';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from 'src/models';
import { PropertyDisplayDriver } from 'src/services/property-display-driver';
import { getOrCreateProperty } from 'src/utils/utils';

@Injectable({
  providedIn: 'root',
})
export class RadioOptionsDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('RadioOptionsDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>) {
    console.log('Radio List Driver Setup');
    console.log('Activity Model', model());
    console.log('Property Model', property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Radio List Driver', propertyModel);
    return {
      componentType: RadioOptionProperty,
      inputs: {
        activityModel: model,
        propertyDescriptor: property,
        propertyModel: propertyModelSignal,
      },
    };
  }

  // displayNested(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onExpressionChanged: Function) {
  //   return {
  //     componentType: RadioOptionProperty,
  //     inputs: {
  //       activityModel: model,
  //       propertyDescriptor: property,
  //       propertyModel: propertyModelSignal,
  //     },
  //   };
  // }
}
