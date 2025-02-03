import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { ActivityDefinitionProperty, ActivityDesignDisplayContext, EventTypes } from "../models";
import { htmlEncode } from "../utils/utils";

export class IfPlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;

    if (activityModel.type !== 'If')
      return;

    const props = activityModel.properties || [];
    const condition: ActivityDefinitionProperty = props.find(x => x.name == 'Condition') || { name: 'Condition', expressions: { 'Literal': '' }, syntax: 'Literal' };
    const expression = condition.expressions[condition.syntax] || '';
    const description = activityModel.description;
    const bodyText = htmlEncode(description && description.length > 0 ? description : expression);
    context.bodyDisplay = `<p>${bodyText}</p>`;
  }
}
