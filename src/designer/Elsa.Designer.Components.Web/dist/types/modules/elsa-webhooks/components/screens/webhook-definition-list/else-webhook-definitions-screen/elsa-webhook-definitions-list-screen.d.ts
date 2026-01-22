import { PagedList } from "../../../../../../models";
import { WebhookDefinitionSummary } from "../../../../models";
import { RouterHistory } from "@stencil/router";
export declare class ElsaWebhookDefinitionsListScreen {
  history?: RouterHistory;
  serverUrl: string;
  basePath: string;
  culture: string;
  webhookDefinitions: PagedList<WebhookDefinitionSummary>;
  confirmDialog: HTMLElsaConfirmDialogElement;
  componentWillLoad(): Promise<void>;
  onDeleteClick(e: Event, webhookDefinition: WebhookDefinitionSummary): Promise<void>;
  loadWebhookDefinitions(): Promise<void>;
  render(): any;
}
