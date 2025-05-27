import { PropertyDisplayDriver } from '../../services/property-display-driver';
import { ActivityModel, ActivityPropertyDescriptor } from '../../models';
import { Signal } from '@angular/core';

export class NullPropertyDriver implements PropertyDisplayDriver {
  display(activity: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>) {
    return undefined;
  }

  update(activity: Signal<ActivityModel>, property: Signal<ActivityPropertyDescriptor>, form: FormData) {}
}
