import { Component, HostListener, OnInit, Output, ElementRef, input, viewChild } from '@angular/core';
import { MenuItem } from './models';
import { leave, toggle } from 'el-transition';
import { Location, NgIf } from '@angular/common';
import { Router } from '@angular/router';

@Component({
  selector: 'workflow-context-menu',
  templateUrl: './workflow-context-menu.html',
  styleUrls: ['./workflow-context-menu.css'],
  standalone: true,
  imports: [NgIf],
})
export class WorkflowContextMenu implements OnInit {
  readonly menuItems = input<Array<MenuItem>>([]);

  readonly element = viewChild.required<ElementRef>('element');
  readonly contextMenu = viewChild.required<ElementRef>('contextMenu');

  constructor(private location: Location, private router: Router) {}

  ngOnInit(): void {
    this.menuItems().map(item => (!!item.anchorUrl ? item.anchorUrl : '#'));
  }

  @HostListener('document:click', ['$event'])
  onWindowClicked(event: Event): void {
    const target = event.target as HTMLElement;

    if (!this.element().nativeElement.contains(target)) this.closeContextMenu();
  }

  closeContextMenu(): void {
    const contextMenu = this.contextMenu();
    if (!!contextMenu.nativeElement) leave(contextMenu.nativeElement);
  }

  toggleMenu(): void {
    toggle(this.contextMenu().nativeElement);
  }

  async onMenuItemClick(e: Event, menuItem: MenuItem): Promise<void> {
    e.preventDefault();
    if (!!menuItem.anchorUrl) {
      this.router.navigate([menuItem.anchorUrl]);
    } else if (!!menuItem.clickHandler) {
      menuItem.clickHandler(e);
    }
    this.closeContextMenu();
  }
}
