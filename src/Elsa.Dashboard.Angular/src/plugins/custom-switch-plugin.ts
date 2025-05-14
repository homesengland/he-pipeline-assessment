import { WorkflowPlugin } from '../services/workflow-plugin';
import { eventBus } from '../services/event-bus';
import { parseJson } from '../utils/utils';
import { ActivityDesignDisplayContext, EventTypes } from 'src/models';

export class CustomSwitchPlugin implements WorkflowPlugin {
  readonly SwitchSyntax = 'Switch';

  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(eventContext: ActivityDesignDisplayContext) {
    const activityModel = eventContext.activityModel;
    const activityDescriptor = eventContext.activityDescriptor;
    const propertyDescriptors = activityDescriptor.inputProperties;
    const switchCaseProperties = propertyDescriptors.filter(x => x.uiHint == 'custom-switch-case-builder');
    if (switchCaseProperties.length == 0) return;
    let outcomesHash = {};
    const syntax = 'Switch';
    for (const propertyDescriptor of switchCaseProperties) {
      const props = activityModel.properties || [];
      const casesProp = props.find(x => x.name == propertyDescriptor.name) || { expressions: { SwitchSyntax: '' }, syntax: syntax };
      const expression = casesProp.expressions[syntax] || [];
      const cases = !!expression['$values'] ? expression['$values'] : Array.isArray(expression) ? expression : parseJson(expression) || [];
      for (const c of cases) outcomesHash[c.name] = true;
    }
    const outcomes = Object.keys(outcomesHash);
    eventContext.outcomes = [...outcomes];
  }
}
