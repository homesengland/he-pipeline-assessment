import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-content',
  standalone: false,
  template: '<ng-content></ng-content>',
})
export class TabContentComponent {
  @Input() tab: string;
  @Input() active: boolean = false;

  
}
