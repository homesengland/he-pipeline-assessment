import { Map } from '../utils/utils';

export interface Question {
  category: string;
  customAttributes: any;
  description: string;
  displayName: string;
  inputProperties: QuestionProperties[];
  type: string;
}

export interface QuestionModel {
  id?: string;
  type: string;
  name?: string;
  displayName?: string;
  properties: Array<QuestionDefinitionProperty>;
}

export interface QuestionDefinitionProperty {
  name: string;
  syntax?: string;
  expressions: Map<string>;
  value?: any;
}

export interface QuestionDescriptor {
  type: string;
  displayName: string;
  description?: string;
  category: string;
  outcomes: Array<string>;
  browsable: boolean;
  inputProperties: Array<QuestionPropertyDescriptor>;
  customAttributes: any;
}

export interface QuestionPropertyDescriptor {
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
}

export interface QuestionProperties {
  disableWorkflowProviderSelection?: boolean;
  hint?: string;
  isBrowsable?: boolean;
  isReadOnly?: boolean;
  label?: string;
  name?: string;
  order?: number;
  supportedSyntaxes?: string[];
  type?: string;
  uiHint?: string;
}

export interface QuestionEditorRenderProps {
  questionDescriptor?: QuestionDescriptor;
  questionModel?: QuestionModel;
  defaultProperties?: Array<QuestionPropertyDescriptor>;
}
