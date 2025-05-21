import { ActivityModel, ActivityPropertyDescriptor } from '../models';

export interface PropertyDisplayDriver {
  display(model: ActivityModel, property: ActivityPropertyDescriptor, onUpdated?: () => void, isEncrypted?: boolean);

  update?(model: ActivityModel, property: ActivityPropertyDescriptor, form: FormData);
}
