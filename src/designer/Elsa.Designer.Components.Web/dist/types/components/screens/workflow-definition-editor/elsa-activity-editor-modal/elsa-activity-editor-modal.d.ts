import { Event } from '../../../../stencil-public-runtime';
import { ActivityDescriptor, ActivityModel, ActivityPropertyDescriptor, WorkflowStorageDescriptor } from "../../../../models";
import { FormContext } from "../../../../utils/forms";
import { i18n } from "i18next";
export interface TabModel {
  tabName: string;
  renderContent: () => any;
}
export interface ActivityEditorRenderProps {
  activityDescriptor?: ActivityDescriptor;
  activityModel?: ActivityModel;
  propertyCategories?: Array<string>;
  defaultProperties?: Array<ActivityPropertyDescriptor>;
  tabs?: Array<TabModel>;
  selectedTabName?: string;
}
export interface ActivityEditorEventArgs {
  activityDescriptor: ActivityDescriptor;
  activityModel: ActivityModel;
}
export interface ActivityEditorAppearingEventArgs extends ActivityEditorEventArgs {
}
export interface ActivityEditorDisappearingEventArgs extends ActivityEditorEventArgs {
}
export declare class ElsaActivityEditorModal {
  culture: string;
  workflowStorageDescriptors: Array<WorkflowStorageDescriptor>;
  activityModel: ActivityModel;
  activityDescriptor: ActivityDescriptor;
  renderProps: ActivityEditorRenderProps;
  i18next: i18n;
  dialog: HTMLElsaModalDialogElement;
  formContext: FormContext;
  timestamp: Date;
  connectedCallback(): void;
  disconnectedCallback(): void;
  componentWillLoad(): Promise<void>;
  t: (key: string) => string;
  updateActivity(formData: FormData): void;
  componentWillRender(): Promise<void>;
  componentDidRender(): Promise<void>;
  onCancelClick(): Promise<void>;
  onSubmit: (e: Event) => Promise<void>;
  onTabClick: (e: Event, tab: TabModel) => void;
  onShowActivityEditor: (activity: ActivityModel, animate: boolean) => Promise<void>;
  show: (animate: boolean) => Promise<void>;
  hide: (animate: boolean) => Promise<void>;
  onKeyDown: (event: KeyboardEvent) => Promise<void>;
  onDialogShown: () => Promise<void>;
  onDialogHidden: () => Promise<void>;
  render(): any;
  renderTabs(tabs: Array<TabModel>): any[];
  renderStorageTab(activityModel: ActivityModel, activityDescriptor: ActivityDescriptor): any;
  renderCommonTab(activityModel: ActivityModel): any;
  renderPropertiesTab(activityModel: ActivityModel): any;
  renderCategoryTab(activityModel: ActivityModel, activityDescriptor: ActivityDescriptor, category: string): any;
  renderPropertyEditor(activity: ActivityModel, property: ActivityPropertyDescriptor): any;
  getHiddenClass(tab: string): "" | "hidden";
}
