import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-header',
  standalone: false,
  template: '<ng-content></ng-content>',
})
export class TabHeaderComponent {
  @Input() tab: string;
  @Input() active: boolean = false;

  getClassNames(): string {
    return this.active ? 'border-blue-500 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300';
  }
}
