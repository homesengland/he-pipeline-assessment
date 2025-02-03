import { Component, EventEmitter, HostListener, Input, OnInit, Output, ViewChild } from "@angular/core";
import { MenuItem } from "./models";
import { leave, toggle } from 'el-transition'
import { Location } from '@angular/common';
import { DropdownButtonItem } from "../workflow-dropdown-button/models";

@Component({
  selector: 'workflow-context-menu',
  templateUrl: './workflow-context-menu.html',
  styleUrls: ['./workflow-context-menu.css'],
  standalone: false
})
export class WorkflowContextMenu implements OnInit {
  @Input() menuItems: Array<MenuItem> = [];

  @ViewChild('element') element;
  @ViewChild('contextMenu') contextMenu;

  constructor(private location:Location) {

  }

  ngOnInit(): void {
    this.menuItems.map(item => !!item.anchorUrl ? item.anchorUrl : "#");
  }

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
    toggle(this.contextMenu.nativeElement);
  }

  async onMenuItemClick(e: Event, menuItem: MenuItem) {
    e.preventDefault();
    if (!!menuItem.anchorUrl) {
      this.location.go(menuItem.anchorUrl);
    }
    else if (!!menuItem.clickHandler) {
      menuItem.clickHandler(e);
    }
    this.closeContextMenu();
  }

  
}

