import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class TimerPlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
