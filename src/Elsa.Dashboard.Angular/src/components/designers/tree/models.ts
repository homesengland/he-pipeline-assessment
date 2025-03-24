import { ActivityModel } from 'src/models';

export interface ActivityContextMenuState {
  shown: boolean;
  x: number;
  y: number;
  activity?: ActivityModel | null;
  selectedActivities?: Map<string, ActivityModel>;
}

export enum LayoutDirection {
  LeftRight = 'leftright',
  TopBottom = 'topbottom',
  RightLeft = 'rightleft',
  BottomTop = 'bottomtop',
}
