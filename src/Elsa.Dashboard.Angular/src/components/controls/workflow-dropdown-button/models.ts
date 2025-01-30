import { Map } from '../../../utils/utils';

export enum DropdownButtonOrigin {
  TopLeft,
  TopRight
}

export interface DropdownButtonItem {
  name?: string;
  value?: any;
  text: string;
  url?: string;
  queryParams?: any;
  btnClass?: string
  isSelected?: boolean;
  handler?: () => {};
}
