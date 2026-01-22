import { ActivityModel } from "../../../../models";
import { Map } from "../../../../utils/utils";
export declare enum WorkflowDesignerMode {
  Edit = 0,
  Instance = 1,
  Blueprint = 2,
  Test = 3
}
export interface ActivityContextMenuState {
  shown: boolean;
  x: number;
  y: number;
  activity?: ActivityModel | null;
  selectedActivities?: Map<ActivityModel>;
}
export declare enum LayoutDirection {
  LeftRight = "leftright",
  TopBottom = "topbottom",
  RightLeft = "rightleft",
  BottomTop = "bottomtop"
}
