import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { ActivityDesignDisplayContext, EventTypes, SyntaxNames } from "../models";
import { htmlEncode } from "../utils/utils";
import cronstrue from "cronstrue";

export class CronPlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;
    if (activityModel.type !== 'Cron')
      return;

    const props = activityModel.properties || [];
    const condition = props.find(x => x.name == 'CronExpression') || { name: 'CronExpression', expressions: { 'Literal': '' }, syntax: SyntaxNames.Literal };
    const expression = htmlEncode(condition.expressions[condition.syntax || 'Literal'] || '');
    const cronDescription = cronstrue.toString(expression, { throwExceptionOnParseError: false });
    context.bodyDisplay = `<p style="overflow: hidden;text-overflow: ellipsis;" title="${cronDescription}">${cronDescription}</p>`;
  }
}
