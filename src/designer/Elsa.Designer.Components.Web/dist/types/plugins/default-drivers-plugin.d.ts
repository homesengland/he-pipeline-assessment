import { ElsaPlugin, PropertyDisplayDriver } from "../services";
import { ElsaStudio } from "../models";
export declare class DefaultDriversPlugin implements ElsaPlugin {
  constructor();
  addDriver<T extends PropertyDisplayDriver>(controlType: string, c: (elsaStudio: ElsaStudio) => T): void;
}
