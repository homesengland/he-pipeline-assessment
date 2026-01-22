import { ElsaPlugin } from "./elsa-plugin";
import { ElsaStudio } from "../models";
export declare class PluginManager {
  pluginFactories: Array<any>;
  elsaStudio: ElsaStudio;
  initialized: boolean;
  constructor();
  initialize(elsaStudio: ElsaStudio): void;
  registerPlugins(pluginFactories: Array<any>): void;
  registerPlugin(pluginType: any): void;
  registerPluginFactory(pluginFactory: (studio: ElsaStudio) => ElsaPlugin): void;
  private createPlugin;
}
export declare const pluginManager: PluginManager;
