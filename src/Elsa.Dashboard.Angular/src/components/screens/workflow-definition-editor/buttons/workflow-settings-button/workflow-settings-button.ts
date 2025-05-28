import { Component, EventEmitter, Output, output } from '@angular/core';
import { EventTypes } from 'src/models';
import { eventBus } from 'src/services/event-bus';

@Component({
  selector: 'workflow-settings-button',
  templateUrl: './workflow-settings-button.html',
  standalone: false,
})
export class WorkflowSettingsButton {

  async onShowWorkflowSettingsClick() {
    await eventBus.emit(EventTypes.ShowWorkflowSettings);
  }
}
