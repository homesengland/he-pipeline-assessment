import { WorkflowPlugin } from "../services/workflow-plugin";
import eventBus from '../services/custom-event-bus';
import { ActivityDescriptorDisplayContext, ActivityDesignDisplayContext, EventTypes } from "../models";

export class ActivityIconProviderPlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDescriptorDisplaying, this.onActivityDescriptorDisplaying);
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDescriptorDisplaying(context: ActivityDescriptorDisplayContext) {
    const descriptor = context.activityDescriptor;
    const iconEntry = activityIconProvider.getIcon(descriptor.type);

    if (iconEntry)
      context.activityIcon = iconEntry;
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;
    const iconEntry = activityIconProvider.getIcon(activityModel.type);

    if (iconEntry)
      context.activityIcon = iconEntry;
  }
}
