import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext } from "../models";
export declare class DynamicOutcomesPlugin implements ElsaPlugin {
  constructor();
  onActivityDesignDisplaying: (context: ActivityDesignDisplayContext) => void;
}
