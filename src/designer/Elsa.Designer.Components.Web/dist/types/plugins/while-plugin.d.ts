import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class WhilePlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
