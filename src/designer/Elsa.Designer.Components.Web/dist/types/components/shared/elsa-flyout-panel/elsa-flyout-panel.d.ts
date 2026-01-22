export declare class ElsaFlyoutPanel {
  expandButtonPosition: number;
  autoExpand: boolean;
  hidden: boolean;
  silent: boolean;
  updateCounter: number;
  expanded: boolean;
  currentTab: string;
  headerTabs: HTMLElsaTabHeaderElement[];
  contentTabs: HTMLElsaTabContentElement[];
  el: HTMLElement;
  componentDidLoad(): Promise<void>;
  updateTabs(): Promise<void>;
  componentDidRender(): Promise<void>;
  render(): any;
  toggle: () => void;
  selectTab(tab: string, expand?: boolean): Promise<void>;
}
