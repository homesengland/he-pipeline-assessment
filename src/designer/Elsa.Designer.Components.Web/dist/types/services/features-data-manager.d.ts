import { ElsaStudio } from "../models/";
export interface FeatureConfig {
  enabled: boolean;
  ui: boolean;
  defaultValue: string;
  value?: string;
}
declare type FeatureConfigMap = {
  [key: string]: FeatureConfig;
};
export declare class FeaturesDataManager {
  elsaStudio: ElsaStudio;
  initialized: boolean;
  features: FeatureConfigMap;
  supportedFeatures: {
    workflowLayout: string;
  };
  initialize(elsaStudio: ElsaStudio): void;
  getFeatureList: () => string[];
  getUIFeatureList: () => string[];
  getFeatureConfig: (name: string) => {
    value: string;
    enabled: boolean;
    ui: boolean;
    defaultValue: string;
  };
  setFeatureConfig: (name: string, value: string) => void;
  setEnableStatus: (name: string, value: boolean) => void;
}
export declare const featuresDataManager: FeaturesDataManager;
export {};
