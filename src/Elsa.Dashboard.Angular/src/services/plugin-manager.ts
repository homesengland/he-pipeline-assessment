import { WorkflowPlugin } from "./workflow-plugin";
//import { IfPlugin } from "../plugins/if-plugin";
//import { HttpEndpointPlugin } from "../plugins/http-endpoint-plugin";
//import { TimerPlugin } from "../plugins/timer-plugin";
//import { WriteLinePlugin } from "../plugins/write-line-plugin";
//import { SendEmailPlugin } from "../plugins/send-email-plugin";
/*import { DefaultDriversPlugin } from "../plugins/default-drivers-plugin";*/
import { ActivityIconProviderPlugin } from "../plugins/activity-icon-provider-plugin";
//import { SwitchPlugin } from "../plugins/switch-plugin";
//import { WhilePlugin } from "../plugins/while-plugin";
//import { StartAtPlugin } from "../plugins/start-at-plugin";
//import { CronPlugin } from "../plugins/cron-plugin";
//import { SignalReceivedPlugin } from "../plugins/signal-received-plugin";
//import { SendSignalPlugin } from "../plugins/send-signal-plugin";
//import { StatePlugin } from "../plugins/state-plugin";
//import { SendHttpRequestPlugin } from "../plugins/send-http-request-plugin";
//import { DynamicOutcomesPlugin } from "../plugins/dynamic-outcomes-plugin";
import { WorkflowStudio } from "../models";

export class PluginManager {
  pluginFactories: Array<any> = [];
  workflowStudio: WorkflowStudio;
  initialized: boolean;

  constructor() {
    this.pluginFactories = [
/*      () => new DefaultDriversPlugin(),*/
      () => new ActivityIconProviderPlugin(),
      //() => new IfPlugin(),
      //() => new WhilePlugin(),
      //() => new SwitchPlugin(),
      //() => new HttpEndpointPlugin(),
      //() => new SendHttpRequestPlugin(),
      //() => new TimerPlugin(),
      //() => new StartAtPlugin(),
      //() => new CronPlugin(),
      //() => new SignalReceivedPlugin(),
      //() => new SendSignalPlugin(),
      //() => new WriteLinePlugin(),
      //() => new StatePlugin(),
      //() => new SendEmailPlugin(),
      //() => new DynamicOutcomesPlugin()
    ];
  }

  initialize(workflowStudio: WorkflowStudio) {
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
