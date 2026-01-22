import { ActivityDesignDisplayContext, ConfigureDashboardMenuContext, ElsaPlugin } from "../../..";
export declare class CredentialManagerPlugin implements ElsaPlugin {
  constructor();
  onActivityDisplaying(context: ActivityDesignDisplayContext): void;
  onLoadingMenu(context: ConfigureDashboardMenuContext): void;
}
