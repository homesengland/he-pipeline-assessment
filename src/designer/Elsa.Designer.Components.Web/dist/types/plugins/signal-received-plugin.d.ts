import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class SignalReceivedPlugin implements ElsaPlugin {
  constructor();
  onActivityDisplaying(context: ActivityDesignDisplayContext): void;
}
