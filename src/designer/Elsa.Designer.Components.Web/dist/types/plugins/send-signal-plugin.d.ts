import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class SendSignalPlugin implements ElsaPlugin {
  constructor();
  onActivityDisplaying(context: ActivityDesignDisplayContext): void;
}
