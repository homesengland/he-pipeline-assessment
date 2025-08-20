import { Map } from '../utils/utils';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from './elsa-interfaces';

export interface HeActivityPropertyDescriptor extends ActivityPropertyDescriptor {
  name: string;
  uiHint: string;
  label?: string;
  hint?: string;
  options?: any;
  category?: string;
  defaultValue?: any;
  defaultSyntax?: string;
  supportedSyntaxes: Array<string>;
  isReadOnly?: boolean;
  isBrowsable?: boolean;
  isDesignerCritical: boolean;
  defaultWorkflowStorageProvider?: string;
  disableWorkflowProviderSelection: boolean;
  considerValuesAsOutcomes: boolean;
  displayInDesigner: boolean;
  conditionalActivityTypes?: Array<string>;
  expectedOutputType: string;
  hasNestedProperties: boolean;
  hasColletedProperties: boolean;
  nestedProperties: Array<HeActivityPropertyDescriptor>;
}

//Outcome Screens

export interface ITextProperty {
  expressions?: Map<string>;
  syntax?: string;
}

export interface IOutcomeProperty {
  text: ITextProperty;
  condition: ITextProperty;
}

export interface Dictionary<T> {
  [Key: string]: T;
}

export interface DataDictionaryGroup {
  Id: number;
  Name: string;
  DataDictionaryList: Array<DataDictionary>;
  IsArchived: boolean;
}

export interface DataDictionary {
  Id: number;
  Name: string;
  LegacyName: string;
  IsArchived: boolean;
}

export class NestedProperty {
  value: NestedActivityDefinitionProperty;
  descriptor: HeActivityPropertyDescriptor;
}

export interface NestedActivityDefinitionProperty extends ActivityDefinitionProperty {
  type: string;
}
