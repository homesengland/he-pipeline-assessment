import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { ActivityDesignDisplayContext, EventTypes, SyntaxNames } from "../models";
import { htmlEncode } from "../utils/utils";

export class SendSignalPlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDisplaying);
  }

  onActivityDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;

    if (activityModel.type !== 'SendSignal')
      return;

    const props = activityModel.properties || [];
    const signalName = props.find(x => x.name == 'Signal') || { name: 'Signal', expressions: { 'Literal': '', syntax: SyntaxNames.Literal } };
    const syntax = signalName.syntax || SyntaxNames.Literal;
    const bodyDisplay = htmlEncode(signalName.expressions[syntax]);
    context.bodyDisplay = `<p>${bodyDisplay}</p>`;
  }
}
