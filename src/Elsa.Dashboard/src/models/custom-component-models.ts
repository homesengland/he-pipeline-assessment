import { IActivityData } from '../components/providers/question-provider/question-provider';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from './elsa-interfaces';
import { Map } from '../utils/utils'

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

export interface IAltConditionalText extends ICheckboxValue {
}

export interface ITextProperty {
  expressions?: Map<string>;
  syntax?: string;
}

export interface IOutcomeProperty {
  text: ITextProperty;
  condition: ITextProperty;
}

// Question Screens

//export class QuestionScreenProperty {
//  questions: Array<QuestionModel> = [];
//}

export interface INestedActivityListProperty {
  activities: Array<NestedPropertyModel>;
}

export class QuestionScreenProperty implements INestedActivityListProperty {
  activities: Array<NestedPropertyModel> = [];
}

export class NestedProperty
{
  value: NestedActivityDefinitionProperty;
  descriptor: HeActivityPropertyDescriptor;
}

export class NestedPropertyModel {
  value: ActivityDefinitionProperty;
  descriptor: Array<HeActivityPropertyDescriptor> = [];
  ActivityType: IActivityData;
}

export interface Dictionary<T> {
  [Key: string]: T;
}

export interface NestedActivityDefinitionProperty extends ActivityDefinitionProperty {
  type: string;
}

export interface NestedActivityDefinitionPropertyAlt extends ActivityDefinitionProperty {
  type: string | null
  propertyDescriptors: Array<HeActivityPropertyDescriptor> | null
}

export interface PotScoreNestedActivityDefinitionProperty extends NestedActivityDefinitionProperty {
  potScore: string;
}

export interface ICheckboxValue extends NestedActivityDefinitionProperty {
  isSingle: boolean;
}

export interface DataDictionaryGroup {
  Id: number;
  Name: string;
  DataDictionaryList: Array<DataDictionary>;
  IsArchived: boolean;
}

export interface DataDictionary{
  Id: number;
  Name: string;
  LegacyName: string;
  IsArchived: boolean;
}
