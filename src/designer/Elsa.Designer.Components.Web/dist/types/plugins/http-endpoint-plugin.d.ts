import { ElsaPlugin } from "../services";
import { ActivityDesignDisplayContext, ActivityUpdatedContext, ActivityValidatingContext, ConfigureComponentCustomButtonContext, ComponentCustomButtonClickContext } from "../models";
export declare class HttpEndpointPlugin implements ElsaPlugin {
  constructor();
  onActivityDisplaying(context: ActivityDesignDisplayContext): void;
  onComponentLoadingCustomButton(context: ConfigureComponentCustomButtonContext): void;
  onComponentCustomButtonClick(context: ComponentCustomButtonClickContext): void;
  onActivityUpdated(context: ActivityUpdatedContext): void;
  onActivityValidating(context: ActivityValidatingContext): void;
}
