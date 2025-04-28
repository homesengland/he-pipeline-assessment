import { WorkflowPlugin } from '../services/workflow-plugin';
import { eventBus } from '../services/event-bus';
import { ActivityDescriptorDisplayContext, ActivityDesignDisplayContext, EventTypes } from '../models';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class ActivityIconProviderPlugin implements WorkflowPlugin {
  constructor(private activityIconProvider: ActivityIconProvider) {
    eventBus.on(EventTypes.ActivityDescriptorDisplaying, context => this.onActivityDescriptorDisplaying(context));
    eventBus.on(EventTypes.ActivityDesignDisplaying, context => this.onActivityDesignDisplaying(context));
  }

  onActivityDescriptorDisplaying(context: ActivityDescriptorDisplayContext) {
    const descriptor = context.activityDescriptor;
    const iconEntry = this.activityIconProvider.getIcon(descriptor.type, context.activityIconColour ?? 'grey');
    if (iconEntry) context.activityIcon = iconEntry;
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;
    const iconEntry = this.activityIconProvider.getIcon(activityModel.type, context.activityIconColour ?? 'grey');
    if (iconEntry) context.activityIcon = iconEntry;
  }
}
