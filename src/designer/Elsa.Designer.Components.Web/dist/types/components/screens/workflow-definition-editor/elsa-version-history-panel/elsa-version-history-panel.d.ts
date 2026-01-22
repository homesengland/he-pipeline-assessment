import { Event, EventEmitter } from '../../../../stencil-public-runtime';
import { WorkflowDefinition, WorkflowDefinitionVersion } from "../../../../models";
export declare class ElsaVersionHistoryPanel {
  workflowDefinition: WorkflowDefinition;
  serverUrl: string;
  versionSelected: EventEmitter<WorkflowDefinitionVersion>;
  deleteVersionClicked: EventEmitter<WorkflowDefinitionVersion>;
  revertVersionClicked: EventEmitter<WorkflowDefinitionVersion>;
  versions: Array<WorkflowDefinitionVersion>;
  confirmDialog: HTMLElsaConfirmDialogElement;
  onWorkflowDefinitionChanged(value: WorkflowDefinition): Promise<void>;
  componentWillLoad(): Promise<void>;
  loadVersions: () => Promise<void>;
  onViewVersionClick: (e: Event, version: WorkflowDefinitionVersion) => void;
  onDeleteVersionClick: (e: Event, version: WorkflowDefinitionVersion) => Promise<void>;
  onRevertVersionClick: (e: Event, version: WorkflowDefinitionVersion) => void;
  render(): any;
}
