import { WorkflowBlueprint } from "../../../../models";
export declare class ElsaWorkflowBlueprintPropertiesPanel {
  workflowId: string;
  culture: string;
  serverUrl: string;
  workflowBlueprint: WorkflowBlueprint;
  publishedVersion: number;
  private i18next;
  workflowIdChangedHandler(newWorkflowId: string): Promise<void>;
  componentWillLoad(): Promise<void>;
  render(): any;
  createClient(): Promise<import("../../../../services").ElsaClient>;
  loadPublishedVersion(): Promise<void>;
  loadWorkflowBlueprint(workflowId?: string): Promise<void>;
}
