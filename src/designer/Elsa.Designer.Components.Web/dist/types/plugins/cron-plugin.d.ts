import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class CronPlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
