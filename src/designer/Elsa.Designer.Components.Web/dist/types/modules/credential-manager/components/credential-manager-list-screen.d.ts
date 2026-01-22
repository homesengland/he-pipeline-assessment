import { RouterHistory } from "@stencil/router";
import { WebhookDefinitionSummary } from "../../elsa-webhooks/models";
import { ElsaSecretsClient } from "../services/credential-manager.client";
import { SecretDescriptor, SecretModel } from "../models/secret.model";
export declare class CredentialManagerListScreen {
  history?: RouterHistory;
  serverUrl: string;
  basePath: string;
  culture: string;
  secrets: Array<SecretModel>;
  confirmDialog: HTMLElsaConfirmDialogElement;
  client: ElsaSecretsClient;
  componentWillLoad(): Promise<void>;
  connectedCallback(): Promise<void>;
  disconnectedCallback(): Promise<void>;
  listenToMessages: (e: MessageEvent) => void;
  onSecretPicked: (args: any) => Promise<void>;
  onSecretEdit(e: any, secret: any): Promise<void>;
  mapProperties(properties: any): any;
  showSecretEditorInternal(secret: SecretModel, animate: boolean): Promise<void>;
  newSecret(secretDescriptor: SecretDescriptor): any;
  onDeleteClick(e: Event, webhookDefinition: WebhookDefinitionSummary): Promise<void>;
  loadSecrets(): Promise<void>;
  render(): any;
}
