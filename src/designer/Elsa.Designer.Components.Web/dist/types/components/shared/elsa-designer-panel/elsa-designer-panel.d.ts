import { Event, EventEmitter } from '../../../stencil-public-runtime';
import { FeatureConfig } from "../../../services";
import { i18n } from "i18next";
export declare class ElsaDesignerPanel {
  culture: string;
  lastChangeTime: Date;
  featureChanged: EventEmitter<string>;
  featureStatusChanged: EventEmitter<string>;
  i18next: i18n;
  componentWillLoad(): Promise<void>;
  t: (key: string, options?: any) => import("i18next").TFunctionDetailedResult<object>;
  render(): any;
  renderFeatureData: (name: string, feature: FeatureConfig) => any;
  onToggleChange: (e: Event, name: string) => void;
  onPropertyChange: (e: Event, name: string) => void;
}
