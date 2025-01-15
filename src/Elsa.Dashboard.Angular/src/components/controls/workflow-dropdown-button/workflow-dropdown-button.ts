import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { leave, toggle } from 'el-transition'
import { DropdownButtonItem, DropdownButtonOrigin } from "./models";
import { CommonModule, NgFor } from '@angular/common';

@Component({
  selector: 'workflow-dropdown-button',
  styleUrl: 'workflow-dropdown-button.css',
  templateUrl: 'workflow-dropdown-button.html',
  imports: [CommonModule]
})
export class WorkflowDropdownButton{
  @Input() text: string;
  @Input() icon?: any;
  @Input() btnClass?: string = " elsa-w-full elsa-bg-white elsa-border elsa-border-gray-300 elsa-rounded-md elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-inline-flex elsa-justify-center elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500"
  @Input() origin: DropdownButtonOrigin = DropdownButtonOrigin.TopLeft;
  @Input() items: Array<DropdownButtonItem> = [];

  @Output() itemSelected: EventEmitter<DropdownButtonItem> = new EventEmitter();

  originClass = this.getOriginClass();

  @ViewChild('element', { static: false }) element: ElementRef;
  @ViewChild('contextMenu', { static: false }) contextMenu: ElementRef;

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
    switch (this.origin) {
      case DropdownButtonOrigin.TopLeft:
        return `hidden elsa-left-0 elsa-origin-top-left elsa-z-10 elsa-absolute elsa-mt-2 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5`;
      case DropdownButtonOrigin.TopRight:
      default:
        return `hidden elsa-left-0 elsa-origin-top-right elsa-z-10 elsa-absolute elsa-mt-2 elsa-w-56 elsa-rounded-md elsa-shadow-lg elsa-bg-white elsa-ring-1 elsa-ring-black elsa-ring-opacity-5`;
    }
  }

  selectedCssClass(item: DropdownButtonItem) {
    return item.isSelected ? "elsa-bg-blue-600 hover:elsa-bg-blue-700 elsa-text-white" : "hover:elsa-bg-gray-100 elsa-text-gray-700 hover:elsa-text-gray-900";
  }

  async onItemClick(e: Event, menuItem: DropdownButtonItem) {
    e.preventDefault();
    this.itemSelected.emit(menuItem);
    this.closeContextMenu();
  }

}




