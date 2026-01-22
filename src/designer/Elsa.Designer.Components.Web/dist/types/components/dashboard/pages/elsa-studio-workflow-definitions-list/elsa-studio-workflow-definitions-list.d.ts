import { RouterHistory } from "@stencil/router";
import 'i18next-wc';
export declare class ElsaStudioWorkflowDefinitionsList {
  history: RouterHistory;
  culture: string;
  basePath: string;
  serverUrl: string;
  private i18next;
  private fileInput;
  private workflowDefinitionsListScreen;
  private menu;
  componentWillLoad(): Promise<void>;
  onFileInputChange(e: Event): Promise<void>;
  restoreWorkflows: (e: Event) => Promise<void>;
  toggleMenu(e?: Event): void;
  render(): any;
}
