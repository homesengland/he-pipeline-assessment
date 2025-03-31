import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-content',
  standalone: true,
  imports: [CommonModule],
  template: '<ng-content></ng-content>',
})
export class TabContentComponent {
  @Input() tab: string;
}
