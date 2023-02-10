import { ActivityModel, ActivityPropertyDescriptor } from "./elsa-interfaces";


export interface HePropertyDisplayDriver {

  display(model: ActivityModel, property: ActivityPropertyDescriptor, onUpdate: Function)
}

