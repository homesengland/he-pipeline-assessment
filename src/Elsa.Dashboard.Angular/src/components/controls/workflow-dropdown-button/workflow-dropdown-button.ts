import { Component, EventEmitter, HostListener, OnInit, Output, ViewChild, computed, input } from '@angular/core';
import { DropdownButtonItem, DropdownButtonOrigin } from './models';
import { leave, toggle } from 'el-transition'

@Component({
  selector: 'workflow-dropdown-button',
  templateUrl: './workflow-dropdown-button.html',
  styleUrls: ['./workflow-dropdown-button.css'],
  standalone: false
})
export class WorkflowDropdownButton {
  readonly text = input<string>(undefined);
  readonly iconPath = input<string>(undefined);
  readonly btnClass = input<string>(" elsa-w-full elsa-bg-white elsa-border elsa-border-gray-300 elsa-rounded-md elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-inline-flex elsa-justify-center elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500");
  readonly origin = input<DropdownButtonOrigin>(DropdownButtonOrigin.TopLeft);
  readonly items = input<Array<DropdownButtonItem>>([]);

  originClass = computed(() => this.getOriginClass());
  
  @Output() onItemSelected = new EventEmitter<DropdownButtonItem>();

  @ViewChild('element') element;
  @ViewChild('contextMenu') contextMenu;

  selectedWorkflowInstanceIds: any[];

  @HostListener('document:click', ['$event'])
  onWindowClicked(event: Event) {
    const target = event.target as HTMLElement;

    if (!this.element.nativeElement.contains(target))
      this.closeContextMenu();
  }

  closeContextMenu() {
    if (!!this.contextMenu.nativeElement)
      leave(this.contextMenu.nativeElement);
  }

  toggleMenu() {
    if (!!this.contextMenu.nativeElement)
      toggle(this.contextMenu.nativeElement);
  }

  getOriginClass(): string {
    switch (this.origin()) {
      case DropdownButtonOrigin.TopLeft:
        return `elsa-left-0 elsa-origin-top-left`;
      case DropdownButtonOrigin.TopRight:
      default:
        return 'elsa-right-0 elsa-origin-top-right';
    }
  }

  async onItemClick(e: Event, menuItem: DropdownButtonItem) {
    e.preventDefault();
    this.onItemSelected.emit(menuItem);
    this.closeContextMenu();
  }
}

