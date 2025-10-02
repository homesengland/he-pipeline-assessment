import { Component, EventEmitter, Output, output } from '@angular/core';
import { EventTypes } from 'src/models';
import { eventBus } from 'src/services/event-bus';

@Component({
  selector: 'workflow-help-button',
  templateUrl: './workflow-help-button.html',
  standalone: false,
})
export class WorkflowHelpButton {

  async onShowWorkflowHelpClick() {
    console.log("Workflow Help Button Clicked");
    await eventBus.emit(EventTypes.ShowWorkflowHelp);
  }
}
