import { Component, Input } from '@angular/core';
import * as moment from 'moment';
import { durationToString } from '../../../utils/utils';
import { ActivityStats } from 'src/services/workflow-client';

@Component({
  selector: 'workflow-performance-information',
  templateUrl: './workflow-performance-information.html',
  styleUrls: ['./workflow-performance-information.css'],
  standalone: false,
})
export class WorkflowPerformanceInformation {
  @Input() activityStats: ActivityStats;
  moment: typeof moment;
  durationToString: (duration: moment.Duration) => string;

  constructor() {
    this.moment = moment;
    this.durationToString = durationToString;
  }
}
