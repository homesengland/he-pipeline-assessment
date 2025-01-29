import { Component, HostListener, Input, OnInit, ViewChild } from "@angular/core";
import { MenuItem } from "./models";
import { leave, toggle } from 'el-transition'

@Component({
  selector: 'workflow-context-menu',
  templateUrl: './workflow-context-menu.html',
  styleUrls: ['./workflow-context-menu.css'],
  standalone: false
})
export class WorkflowPager implements OnInit {
  @Input() menuItems: Array<MenuItem> = [];

  navigate: (path: string) => void;
  @ViewChild('element') element;
  @ViewChild('contextMenu') contextMenu;


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
    if (!!this.contextMenu)
      leave(this.contextMenu);
  }

  toggleMenu() {
    toggle(this.contextMenu);
  }

  async onMenuItemClick(e: Event, menuItem: MenuItem) {
    e.preventDefault();

    if (!!menuItem.anchorUrl) {
      this.navigate(menuItem.anchorUrl);
    } else if (!!menuItem.clickHandler) {
      menuItem.clickHandler(e);
    }

    this.closeContextMenu();
  }

  
}

