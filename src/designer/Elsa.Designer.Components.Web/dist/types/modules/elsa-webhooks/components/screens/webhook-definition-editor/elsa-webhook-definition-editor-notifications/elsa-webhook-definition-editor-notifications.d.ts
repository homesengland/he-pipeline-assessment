import { WebhookDefinition } from "../../../../models";
export declare class ElsaWebhookEditorNotifications {
  toastNotificationElement: HTMLElsaToastNotificationElement;
  connectedCallback(): void;
  disconnectedCallback(): void;
  onWebhookSaved: (webhookDefinition: WebhookDefinition) => Promise<void>;
  render(): any;
}
