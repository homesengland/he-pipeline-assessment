import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "../../../../models";
export declare class ElsaDictionaryProperty {
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  activityModel: ActivityModel;
  serverUrl: string;
  currentValue: [string, string][];
  componentWillLoad(): Promise<void>;
  jsonToDictionary: (json: string) => [string, string][];
  dictionaryToJson: (dictionary: [string, string][]) => string;
  removeInvalidKeys: (dictionary: [string, string][]) => any[];
  onRowAdded: () => void;
  onRowDeleted: (index: number) => void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  onKeyChanged(e: Event, index: number): void;
  onValueChanged(e: Event, index: number): void;
  render(): any;
}
