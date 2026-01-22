import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class IfPlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
