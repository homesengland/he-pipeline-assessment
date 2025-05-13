import { Component, Input } from '@angular/core';

@Component({
  selector: 'tab-content',
  standalone: false,
  templateUrl: './tab-content.component.html',
})
export class TabContentComponent {
  @Input() tab: string;
  @Input() active: boolean;

  getClassNames(): string {
    return this.active ? '' : 'elsa-hidden';
  }
}
