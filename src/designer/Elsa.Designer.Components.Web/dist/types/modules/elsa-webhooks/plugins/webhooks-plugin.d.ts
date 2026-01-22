import { ElsaPlugin } from "../../../services";
import { ActivityDesignDisplayContext, ConfigureDashboardMenuContext } from "../../../models";
export declare class WebhooksPlugin implements ElsaPlugin {
  constructor();
  onActivityDisplaying(context: ActivityDesignDisplayContext): void;
  onLoadingMenu(context: ConfigureDashboardMenuContext): void;
}
