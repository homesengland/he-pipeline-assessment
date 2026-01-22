import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class StatePlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
