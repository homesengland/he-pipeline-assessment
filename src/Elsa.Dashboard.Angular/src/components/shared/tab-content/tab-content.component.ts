import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-content',
  standalone: false,
  template: `<div [class]="getClassNames()">
    <ng-content></ng-content>
  </div>`,
})
export class TabContentComponent {
  @Input() tab: string;
  @Input() active: boolean;

  getClassNames(): string {
    return this.active ? '' : 'elsa-hidden';
  }
}
