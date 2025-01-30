import { Component, EventEmitter, HostListener, Input, OnInit, Output, ViewChild} from '@angular/core';
import { DropdownButtonItem, DropdownButtonOrigin } from './models';
import { leave, toggle } from 'el-transition'
import { WorkflowInstanceSummary } from '../../../models';
import { parseQuery } from "../../../utils/utils";

@Component({
  selector: 'workflow-dropdown-button',
  templateUrl: './workflow-dropdown-button.html',
  styleUrls: ['./workflow-dropdown-button.css'],
  standalone: false
})
export class WorkflowDropdownButton implements OnInit {
  @Input() text: string;
  @Input() iconPath?: string;
  @Input() btnClass?: string = " elsa-w-full elsa-bg-white elsa-border elsa-border-gray-300 elsa-rounded-md elsa-shadow-sm elsa-px-4 elsa-py-2 elsa-inline-flex elsa-justify-center elsa-text-sm elsa-font-medium elsa-text-gray-700 hover:elsa-bg-gray-50 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500"
  @Input() origin: DropdownButtonOrigin = DropdownButtonOrigin.TopLeft;
  @Input() items: Array<DropdownButtonItem> = [];

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

  ngOnInit(): void {
    this.items.forEach(item => {
      item.queryParams = colle
    }
    )
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

