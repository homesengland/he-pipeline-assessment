import { ActivityBlueprint, ActivityModel, Connection, ConnectionModel, WorkflowBlueprint, WorkflowModel } from "../../../../models";
export declare class ElsaWorkflowBlueprintViewerScreen {
  workflowDefinitionId: string;
  serverUrl: string;
  culture: string;
  workflowBlueprint: WorkflowBlueprint;
  workflowModel: WorkflowModel;
  el: HTMLElement;
  designer: HTMLElsaDesignerTreeElement;
  getServerUrl(): Promise<string>;
  workflowDefinitionIdChangedHandler(newValue: string): Promise<void>;
  serverUrlChangedHandler(newValue: string): Promise<void>;
  componentWillLoad(): Promise<void>;
  componentDidLoad(): void;
  loadActivityDescriptors(): Promise<void>;
  updateModels(workflowBlueprint: WorkflowBlueprint): void;
  mapWorkflowModel(workflowBlueprint: WorkflowBlueprint): WorkflowModel;
  mapActivityModel(source: ActivityBlueprint): ActivityModel;
  mapConnectionModel(connection: Connection): ConnectionModel;
  render(): any;
  renderCanvas(): any;
}
