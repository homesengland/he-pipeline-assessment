export declare enum DropdownButtonOrigin {
  TopLeft = 0,
  TopRight = 1
}
export interface DropdownButtonItem {
  name?: string;
  value?: any;
  text: string;
  url?: string;
  btnClass?: string;
  isSelected?: boolean;
  handler?: () => {};
}
