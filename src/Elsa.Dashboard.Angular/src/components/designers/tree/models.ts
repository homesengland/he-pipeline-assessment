import { ActivityModel } from 'src/models';
import { Map } from 'src/utils/utils';

export interface ActivityContextMenuState {
  shown: boolean;
  x: number;
  y: number;
  activity?: ActivityModel | null;
  selectedActivities?: Map<ActivityModel>;
}

export enum LayoutDirection {
  LeftRight = 'leftright',
  TopBottom = 'topbottom',
  RightLeft = 'rightleft',
  BottomTop = 'bottomtop',
}

export enum WorkflowDesignerMode {
  Edit,
  Instance,
  Blueprint,
  Test,
}
