import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'elsa-tab-header',
  standalone: false,
  template: ` <button
    type="button"
    [class.elsa-active]="active"
    (click)="onTabClick()"
    class="elsa-whitespace-nowrap elsa-py-4 elsa-px-1 elsa-border-b-2 elsa-font-medium elsa-text-sm"
    [class.elsa-border-blue-500]="active"
    [class.elsa-text-blue-600]="active"
    [class.elsa-border-transparent]="!active"
    [class.elsa-text-gray-500]="!active"
    [class.hover:elsa-text-gray-700]="!active"
    [class.hover:elsa-border-gray-300]="!active"
  >
    <ng-content></ng-content>
  </button>`,
})
export class TabHeaderComponent {
  @Input() tab: string;
  @Input() active: boolean = false;
  @Output() tabClick = new EventEmitter<string>();

  onTabClick(): void {
    this.tabClick.emit(this.tab);
  }
  getClassNames(): string {
    return this.active ? 'border-blue-500 text-blue-600' : 'border-transparent text-gray-500 hover:text-gray-700 hover:border-gray-300';
  }
}
