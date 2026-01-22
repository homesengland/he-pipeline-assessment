import { EventEmitter } from '../../../stencil-public-runtime';
import { LocationSegments, RouterHistory } from "@stencil/router";
import { i18n } from "i18next";
export interface PagerData {
  page: number;
  pageSize: number;
  totalCount: number;
}
export declare class ElsaPager {
  page: number;
  pageSize: number;
  totalCount: number;
  location: LocationSegments;
  history?: RouterHistory;
  culture: string;
  paged: EventEmitter<PagerData>;
  i18next: i18n;
  basePath: string;
  componentWillLoad(): Promise<void>;
  t: (key: string, options?: any) => import("i18next").TFunctionDetailedResult<object>;
  navigate(path: string, page: number): void;
  onNavigateClick(e: Event, page: number): void;
  render(): any;
}
