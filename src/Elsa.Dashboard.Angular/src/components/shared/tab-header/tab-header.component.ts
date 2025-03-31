import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-header',
  standalone: true,
  imports: [CommonModule],
  template: '<ng-content></ng-content>',
})
export class TabHeaderComponent {
  @Input() tab: string;
}
