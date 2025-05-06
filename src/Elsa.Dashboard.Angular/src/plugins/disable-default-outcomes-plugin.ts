import { WorkflowPlugin } from '../services/workflow-plugin';
import { eventBus } from '../services/event-bus';
import { parseJson } from '../utils/utils';
import { ActivityDesignDisplayContext, EventTypes } from 'src/models';

export class DisableDefaultOutcomesPlugin implements WorkflowPlugin {
  static readonly ActivitiesWithFilteredOutcomes = ['QuestionScreen', 'CheckYourAnswersScreen'];

  static readonly ActivitiesWithDoneOutcome = ['ConfirmationScreen'];
  static readonly DoneOutcome = 'Done';
  static readonly DefaultOutcome = 'Default';

  constructor() {
    eventBus.on(EventTypes.ActivityDesignDisplaying, this.onActivityDesignDisplaying);
  }

  onActivityDesignDisplaying(eventContext: ActivityDesignDisplayContext) {
    if (DisableDefaultOutcomesPlugin.ActivitiesWithFilteredOutcomes.includes(eventContext.activityModel.type))
      eventContext.outcomes = eventContext.outcomes.filter(x => x !== DisableDefaultOutcomesPlugin.DefaultOutcome && x !== DisableDefaultOutcomesPlugin.DoneOutcome);
    else if (DisableDefaultOutcomesPlugin.ActivitiesWithDoneOutcome.includes(eventContext.activityModel.type)) {
      eventContext.outcomes = eventContext.outcomes.filter(x => x == DisableDefaultOutcomesPlugin.DoneOutcome);
    }
  }
}
