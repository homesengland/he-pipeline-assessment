import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class WriteLinePlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying(context: ActivityDesignDisplayContext): void;
}
