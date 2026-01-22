import { r as registerInstance } from './index-ea213ee1.js';
import { t as toastNotificationService } from './index-e19c34cd.js';
import { E as EventTypes } from './index-0f68dbd6.js';
import { e as eventBus } from './event-bus-5d6f3774.js';
import './elsa-client-17ed10a4.js';
import './axios-middleware.esm-fcda64d5.js';
import './collection-ba09a015.js';
import './_commonjsHelpers-6cb8dacb.js';
import './utils-db96334c.js';
import './cronstrue-37d55fa1.js';

let ElsaWorkflowEditorNotifications = class {
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
