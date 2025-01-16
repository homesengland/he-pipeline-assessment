import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { activityIconProvider } from '../services/activity-icon-provider'
import { ActivityDescriptorDisplayContext, ActivityDesignDisplayContext, EventTypes } from "../models";

export class ActivityIconProviderPlugin implements WorkflowPlugin {

  activityIconProvider;
  constructor() {
    eventBus.on(EventTypes.ActivityDescriptorDisplaying, this.onActivityDescriptorDisplaying);
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDescriptorDisplaying(context: ActivityDescriptorDisplayContext) {
    const descriptor = context.activityDescriptor;
    const iconEntry = activityIconProvider.getIcon(descriptor.type, context.activityIcon);
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;
    const iconEntry = activityIconProvider.getIcon(activityModel.type, context.activityIcon);
  }
}
