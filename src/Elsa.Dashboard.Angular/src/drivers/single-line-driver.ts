import { PropertyDisplayDriver } from '../services/property-display-driver';
import { ActivityModel, ActivityPropertyDescriptor } from '../models';
import { getOrCreateProperty } from '../utils/utils';
import { Injectable } from '@angular/core';
import { SingleLineProperty } from 'src/components/editors/properties/single-line-property/single-line-property';

@Injectable({
  providedIn: 'root',
})
export class SingleLineDriver implements PropertyDisplayDriver {
  display(activity: ActivityModel, property: ActivityPropertyDescriptor, onUpdated?: () => void, isEncrypted?: boolean) {
    const propertyModel = getOrCreateProperty(activity, property.name);

    return {
      componentType: SingleLineProperty,
      inputs: {
        activityModel: activity,
        propertyDescriptor: property,
        propertyModel: propertyModel,
        isEncrypted: isEncrypted,
        propertyChanged: onUpdated,
      },
    };
  }
}
