import { Component, input } from '@angular/core';

@Component({
  selector: 'activity-icon',
  templateUrl: './activity-icon.html',
  styleUrls: ['./activity-icon.css'],
})
export class ActivityIcon {

  color = input<string>('sky');
  iconClass = `elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${this.color}-50 elsa-text-${this.color}-700 elsa-ring-4 elsa-ring-white`
}

