import { r as registerInstance, h, j as Host } from './index-CL6j2ec2.js';
import { e as eventBus } from './event-bus-axQqcjdg.js';
import { E as EventTypes } from './index-C5Nxo9O-.js';

const ElsaWebhookEditorNotifications = class {
    constructor(hostRef) {
        registerInstance(this, hostRef);
        this.onWebhookSaved = async (webhookDefinition) => await this.toastNotificationElement.show({
            autoCloseIn: 1500,
            title: 'Webhook Saved',
            message: `Webhook successfully saved with name ${webhookDefinition.name}.`
        });
    }
    connectedCallback() {
        eventBus.on(EventTypes.WebhookSaved, this.onWebhookSaved);
    }
    disconnectedCallback() {
        eventBus.detach(EventTypes.WebhookSaved, this.onWebhookSaved);
    }
    render() {
        return (h(Host, { key: '01fa238905070d4f9a92bd610d1fa6785c012a0c', class: "elsa-block" }, h("elsa-toast-notification", { key: '771d66dee55a6f0099a831f8e29e5ccf1673cd8e', ref: el => this.toastNotificationElement = el })));
    }
};

export { ElsaWebhookEditorNotifications as elsa_webhook_definition_editor_notifications };
//# sourceMappingURL=elsa-webhook-definition-editor-notifications.entry.esm.js.map
