import { IQuestionData } from '../components/question-provider/question-provider';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from './elsa-interfaces';
import { Map } from './utils'

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
  conditionalActivityType?: string;
  expectedOutputType: string;
}


///Question Options

export interface IQuestionOption {
  identifier: string
  answer: string
}

export interface CheckboxOption extends IQuestionOption {
  isSingle: boolean;
}

export interface RadioOption extends IQuestionOption {
}

export class QuestionOptions<T extends IQuestionOption>{
  choices: Array<T> = [];
}


///Questions

export interface ISampleQuestionProperty {
  name: string;
  syntax?: string;
  expressions: Map<string>;
  value?: any;
}

export interface ISampleQuestion {
  properties: Array<ISampleQuestionProperty>;
}

export interface IQuestionComponent {
  id: string;
  title: string;
  questionGuidance: string;
  questionText: string;
  displayComments: boolean;
  questionHint: string;
  questionType: string;
  questionTypeName: string;
}

export class Question implements IQuestionComponent {
    id: string;
    title: string;
    questionGuidance: string;
    questionText: string;
    displayComments: boolean;
    questionHint: string;
    questionType: string;
    questionTypeName: string;
}

export abstract class MultipleChoiceQuestion<T extends IQuestionOption> extends Question {
  options: QuestionOptions<T> = new QuestionOptions<T>()
}

export class CheckboxQuestion extends MultipleChoiceQuestion<CheckboxOption> {
  checkbox: QuestionOptions<CheckboxOption> = this.options;
}

export class RadioQuestion extends MultipleChoiceQuestion<RadioOption> {
  radio: QuestionOptions<RadioOption> = this.options;
}

export class TextAreaQuestion extends Question {
  characterLimit: number;
}


//Activity Screens

export class MultiChoiceActivity {
  choices: Array<CheckboxOption> = [];
}

export class SingleChoiceActivity {
  choices: Array<RadioOption> = [];
}

export class QuestionActivity {
  questions: Array<IQuestionComponent> = [];
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

//Alt Question Screens

export class QuestionScreenProperty {
  questions: Array<QuestionModel> = [];
}

export class NestedProperty
{
  value: ActivityDefinitionProperty;
  descriptor: HeActivityPropertyDescriptor
}


export class QuestionModel {
  value: ActivityDefinitionProperty;
  descriptor: Array<HeActivityPropertyDescriptor> = [];
  questionType: IQuestionData;
}

export interface Dictionary<T> {
  [Key: string]: T;
}

export interface NestedActivityDefinitionProperty extends ActivityDefinitionProperty {
  type: string;
}

export interface ICheckboxValue extends NestedActivityDefinitionProperty {
  isSingle: boolean;
}


