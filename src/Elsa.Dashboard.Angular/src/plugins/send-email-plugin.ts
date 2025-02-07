import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { ActivityDesignDisplayContext, EventTypes, SyntaxNames } from "../models";
import { htmlEncode } from "../utils/utils";

export class SendEmailPlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;

    if (activityModel.type !== 'SendEmail')
      return;

    const props = activityModel.properties || [];
    const to = props.find(x => x.name == 'To') || { expressions: { 'Json': '' }, syntax: SyntaxNames.Json };
    const expression = to.expressions[to.syntax || SyntaxNames.Json] || '';
    const description = activityModel.description;
    const bodyText = htmlEncode(description && description.length > 0 ? description : expression);
    context.bodyDisplay = `<p>To: ${bodyText}</p>`;
  }
}
