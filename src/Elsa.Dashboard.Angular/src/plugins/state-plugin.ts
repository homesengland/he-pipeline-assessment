import { WorkflowPlugin } from "../services/workflow-plugin";
import { eventBus } from '../services/event-bus';
import { ActivityDesignDisplayContext, EventTypes, SyntaxNames } from "../models";
import { htmlEncode, parseJson } from "../utils/utils";

export class StatePlugin implements WorkflowPlugin {
  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(context: ActivityDesignDisplayContext) {
    const activityModel = context.activityModel;

    if (activityModel.type !== 'State')
      return;

    const props = activityModel.properties || [];
    const stateNameProp = props.find(x => x.name == 'StateName') || { name: 'Text', expressions: { 'Literal': '' }, syntax: SyntaxNames.Literal };
    context.displayName = htmlEncode(stateNameProp.expressions[stateNameProp.syntax || 'Literal'] || 'State');
  }
}
