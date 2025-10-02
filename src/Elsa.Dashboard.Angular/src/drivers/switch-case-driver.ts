import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable, signal, Signal, Type } from '@angular/core';
import { SwitchCaseProperty } from 'src/components/editors/properties/switch-case-property/switch-case-property';
import { AltSwitchCaseProperty } from 'src/components/editors/properties/switch-case-property/alt-switch-case-property';

@Injectable({
  providedIn: 'root',
})
export class SwitchCaseDriver implements PropertyDisplayDriver {
  constructor() {
    console.log('SwitchCaseDriver initialized');
  }

  display(model: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, onUpdated?: () => void, isEncrypted?: boolean) {
    console.log("Switch Case Driver Setup");
    console.log("Activity Model", model());
    console.log("Property Model", property());
    const propertyModel = getOrCreateProperty(model(), property().name);
    const propertyModelSignal = signal<ActivityDefinitionProperty>(propertyModel);

    console.log('property model for Switch Case Driver', propertyModel);
    return {
      componentType: AltSwitchCaseProperty,
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
