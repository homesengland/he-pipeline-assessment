import { r as registerInstance } from './index-1542df5c.js';
import { t as toastNotificationService } from './index-892f713d.js';
import './index-1654a48d.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes } from './events-d0aab14a.js';
import './elsa-client-8304c78c.js';
import './fetch-client-f0dc2a52.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

const ElsaWorkflowEditorNotifications = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.onWorkflowPublished = (workflowDefinition) => toastNotificationService.show('Workflow Published', `Workflow successfully published at version ${workflowDefinition.version}.`, 1500);
    this.onWorkflowRetracted = (workflowDefinition) => toastNotificationService.show('Workflow Retracted', `Workflow successfully retracted at version ${workflowDefinition.version}.`, 1500);
    this.onWorkflowImported = () => toastNotificationService.show('Workflow Imported', `Workflow successfully imported.`, 1500);
    this.onClipboardPermissionsDenied = () => toastNotificationService.show('Clipboard Error', `Clipboard pemission denied.`, 1500);
    this.onClipboardCopied = (title, body) => {
      toastNotificationService.show(title || 'Copy to Clipboard', body || 'Activities successfully copied to Clipboard.', 1500);
    };
  }
  connectedCallback() {
    eventBus.on(EventTypes.WorkflowPublished, this.onWorkflowPublished);
    eventBus.on(EventTypes.WorkflowRetracted, this.onWorkflowRetracted);
    eventBus.on(EventTypes.WorkflowImported, this.onWorkflowImported);
    eventBus.on(EventTypes.ClipboardPermissionDenied, this.onClipboardPermissionsDenied);
    eventBus.on(EventTypes.ClipboardCopied, this.onClipboardCopied);
  }
  disconnectedCallback() {
    eventBus.detach(EventTypes.WorkflowPublished, this.onWorkflowPublished);
    eventBus.detach(EventTypes.WorkflowRetracted, this.onWorkflowRetracted);
    eventBus.detach(EventTypes.WorkflowImported, this.onWorkflowImported);
    eventBus.detach(EventTypes.ClipboardPermissionDenied, this.onClipboardPermissionsDenied);
    eventBus.detach(EventTypes.ClipboardCopied, this.onClipboardCopied);
  }
};

export { ElsaWorkflowEditorNotifications as elsa_workflow_definition_editor_notifications };
