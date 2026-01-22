import { ElsaPlugin } from "../services/elsa-plugin";
import { ActivityDesignDisplayContext } from "../models";
export declare class SwitchPlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
