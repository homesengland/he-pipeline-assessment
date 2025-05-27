import { Signal, WritableSignal } from '@angular/core';
import { ActivityModel, ActivityPropertyDescriptor } from '../models';

export interface PropertyDisplayDriver {
  display(
    model: Signal<ActivityModel> | WritableSignal<ActivityModel>,
    property: Signal<ActivityPropertyDescriptor> | WritableSignal<ActivityPropertyDescriptor>,
    onUpdated?: () => void,
    isEncrypted?: boolean,
  );

  update?(model: Signal<ActivityModel> | WritableSignal<ActivityModel>, property: Signal<ActivityPropertyDescriptor> | WritableSignal<ActivityPropertyDescriptor>, form: FormData);
}
