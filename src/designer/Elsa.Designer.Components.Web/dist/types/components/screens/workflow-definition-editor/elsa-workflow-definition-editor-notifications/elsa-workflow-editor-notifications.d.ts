import { WorkflowDefinition } from "../../../../models";
export declare class ElsaWorkflowEditorNotifications {
  toastNotificationElement: HTMLElsaToastNotificationElement;
  connectedCallback(): void;
  disconnectedCallback(): void;
  onWorkflowPublished: (workflowDefinition: WorkflowDefinition) => void;
  onWorkflowRetracted: (workflowDefinition: WorkflowDefinition) => void;
  onWorkflowImported: () => void;
  onClipboardPermissionsDenied: () => void;
  onClipboardCopied: (title: string, body: string) => void;
}
