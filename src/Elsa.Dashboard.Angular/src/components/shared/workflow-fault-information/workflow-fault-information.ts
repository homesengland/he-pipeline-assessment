import { Component, Input } from '@angular/core';
import * as moment from 'moment';
import { clip, durationToString } from '../../../utils/utils';
import { WorkflowFault } from 'src/models';

@Component({
  selector: 'workflow-fault-information',
  templateUrl: './workflow-fault-information.html',
  styleUrls: ['./workflow-fault-information.css'],
  standalone: false,
})
export class WorkflowFaultInformation {
  @Input() workflowFault: WorkflowFault;
  @Input() faultedAt: Date;
  moment: typeof moment;
  durationToString: (duration: moment.Duration) => string;
  JSON: JSON;

  constructor() {
    this.moment = moment;
    this.durationToString = durationToString;
  }

  clip(e: Event) {
    clip(e.target as HTMLElement);
  }
}
