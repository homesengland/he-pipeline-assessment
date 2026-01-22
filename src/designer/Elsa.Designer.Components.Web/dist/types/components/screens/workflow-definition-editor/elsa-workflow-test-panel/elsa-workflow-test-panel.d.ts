import { HubConnection } from '@microsoft/signalr';
import { WorkflowDefinition, WorkflowTestActivityMessage } from "../../../../models";
import { i18n } from "i18next";
export declare class ElsaWorkflowTestPanel {
  workflowDefinition: WorkflowDefinition;
  workflowTestActivityId: string;
  culture: string;
  serverUrl: string;
  selectedActivityId?: string;
  hubConnection: HubConnection;
  workflowTestActivityMessages: Array<WorkflowTestActivityMessage>;
  workflowStarted: boolean;
  i18next: i18n;
  signalRConnectionId: string;
  message: WorkflowTestActivityMessage;
  confirmDialog: HTMLElsaConfirmDialogElement;
  workflowTestActivityMessageChangedHandler(newMessage: string, oldMessage: string): Promise<void>;
  componentWillLoad(): Promise<void>;
  private connectMessageHub;
  connectedCallback(): void;
  disconnectedCallback(): void;
  onExecuteWorkflowClick(): Promise<void>;
  onRestartWorkflow: (selectedActivityId: string) => Promise<void>;
  onStopWorkflowClick(): Promise<void>;
  render(): any;
}
