import { Component, Input } from '@angular/core';
import moment from 'moment';
import { clip, durationToString } from '../../../utils/utils';
import { WorkflowFault } from 'src/models';
import { NgIf, NgTemplateOutlet } from '@angular/common';

@Component({
  selector: 'workflow-fault-information',
  templateUrl: './workflow-fault-information.html',
  styleUrls: ['./workflow-fault-information.css'],
  standalone: true,
  imports: [NgTemplateOutlet, NgIf],
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
