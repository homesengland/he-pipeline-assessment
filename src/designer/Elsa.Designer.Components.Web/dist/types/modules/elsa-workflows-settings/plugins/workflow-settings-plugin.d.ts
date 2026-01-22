import { ElsaPlugin } from "../../../services";
import { ConfigureWorkflowRegistryColumnsContext, ConfigureWorkflowRegistryUpdatingContext, ElsaStudio } from "../../../models";
export declare class WorkflowSettingsPlugin implements ElsaPlugin {
  serverUrl: string;
  constructor(elsaStudio: ElsaStudio);
  onLoadingColumns(context: ConfigureWorkflowRegistryColumnsContext): void;
  onUpdating(context: ConfigureWorkflowRegistryUpdatingContext): Promise<void>;
}
