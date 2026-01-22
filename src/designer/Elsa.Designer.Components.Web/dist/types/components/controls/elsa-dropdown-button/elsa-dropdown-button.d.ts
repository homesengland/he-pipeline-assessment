import { EventEmitter } from '../../../stencil-public-runtime';
import { DropdownButtonItem, DropdownButtonOrigin } from "./models";
export declare class ElsaContextMenu {
  text: string;
  icon?: any;
  btnClass?: string;
  origin: DropdownButtonOrigin;
  items: Array<DropdownButtonItem>;
  itemSelected: EventEmitter<DropdownButtonItem>;
  contextMenu: HTMLElement;
  element: HTMLElement;
  onWindowClicked(event: Event): void;
  closeContextMenu(): void;
  toggleMenu(): void;
  getOriginClass(): string;
  onItemClick(e: Event, menuItem: DropdownButtonItem): Promise<void>;
  render(): any;
  renderMenu(): any;
  renderItems(): any[];
  renderIcon(): any;
}
