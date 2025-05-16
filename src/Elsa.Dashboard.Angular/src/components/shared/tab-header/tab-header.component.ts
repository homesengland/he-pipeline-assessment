import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'tab-header',
  standalone: false,
  templateUrl: 'tab-header.component.html',
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
