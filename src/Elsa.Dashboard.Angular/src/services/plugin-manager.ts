import { WorkflowPlugin } from "./workflow-plugin";
import { ActivityIconProviderPlugin } from "../plugins/activity-icon-provider-plugin";
import { WorkflowStudio } from "../models";
import { Auth0ClientOptions } from '@auth0/auth0-spa-js';
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: 'root'
})
export class PluginManager {
  pluginFactories: Array<any> = [];
  workflowStudio: WorkflowStudio;
  initialized: boolean;
  audience: string;
  clientId: string;
  domain: string;

  constructor() {
    this.pluginFactories = [
      () => new ActivityIconProviderPlugin(),
    ];
  }

  initialize(workflowStudio: WorkflowStudio, options: Auth0ClientOptions) {
    if (this.initialized)
      return;

    this.workflowStudio = workflowStudio;

    for (const pluginType of this.pluginFactories) {
      this.createPlugin(pluginType);
    }
    this.initialized = true;
  }

  registerPlugins(pluginFactories: Array<any>) {
    for (const pluginFactory of pluginFactories) {
      this.registerPlugin(pluginFactory);
    }
  }

  registerPlugin(pluginType: any) {
    const factory = () => new pluginType(this.workflowStudio);
    this.registerPluginFactory(factory);
  }

  registerPluginFactory(pluginFactory: (studio: WorkflowStudio) => WorkflowPlugin) {
    this.pluginFactories.push(pluginFactory);

    if (this.initialized)
      this.createPlugin(pluginFactory);
  }

  private createPlugin = (pluginFactory: (studio: WorkflowStudio) => WorkflowPlugin): WorkflowPlugin => (pluginFactory(this.workflowStudio) as WorkflowPlugin)
}

export const pluginManager = new PluginManager();
