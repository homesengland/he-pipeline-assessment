import { RouterHistory } from "@stencil/router";
import { MenuItem } from "./models";
export declare class ElsaContextMenu {
  history: RouterHistory;
  menuItems: Array<MenuItem>;
  navigate: (path: string) => void;
  contextMenu: HTMLElement;
  element: HTMLElement;
  componentWillLoad(): void;
  onWindowClicked(event: Event): void;
  closeContextMenu(): void;
  toggleMenu(): void;
  onMenuItemClick(e: Event, menuItem: MenuItem): Promise<void>;
  render(): any;
}
