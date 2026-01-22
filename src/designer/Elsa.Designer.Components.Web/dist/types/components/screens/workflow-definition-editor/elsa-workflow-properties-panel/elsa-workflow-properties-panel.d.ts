import { WorkflowDefinition } from "../../../../models";
export declare class ElsaWorkflowPropertiesPanel {
  workflowDefinition: WorkflowDefinition;
  culture: string;
  serverUrl: string;
  publishedVersion: number;
  expanded: boolean;
  private i18next;
  el: HTMLElement;
  workflowDefinitionChangedHandler(newWorkflow: WorkflowDefinition, oldWorkflow: WorkflowDefinition): Promise<void>;
  componentWillLoad(): Promise<void>;
  render(): any;
  createClient(): Promise<import("../../../../services").ElsaClient>;
  loadPublishedVersion(): Promise<void>;
  toggle: () => void;
}
