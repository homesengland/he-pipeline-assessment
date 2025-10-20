import { r as registerInstance } from './index-CL6j2ec2.js';
import { t as toastNotificationService } from './index-fZDMH_YE.js';
import './index-D7wXd6HU.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './events-CpKc8CLe.js';
import './elsa-client-q6ob5JPZ.js';
import './fetch-client-1OcjQcrw.js';
import './collection-B4sYCr2r.js';
import './_commonjsHelpers-Cf5sKic0.js';
import './utils-C0M_5Llz.js';
import './cronstrue-BvVNjwUa.js';

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
//# sourceMappingURL=elsa-workflow-definition-editor-notifications.entry.esm.js.map
