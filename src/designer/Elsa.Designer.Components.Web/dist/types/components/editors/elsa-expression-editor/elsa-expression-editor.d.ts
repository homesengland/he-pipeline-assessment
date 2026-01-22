import { EventEmitter } from '../../../stencil-public-runtime';
import { MonacoValueChangedArgs } from '../../controls/elsa-monaco/elsa-monaco';
import { IntellisenseContext } from '../../../models';
export declare class ElsaExpressionEditor {
  expressionChanged: EventEmitter<string>;
  opensModal: boolean;
  language: string;
  expression: string;
  editorHeight: string;
  singleLineMode: boolean;
  padding: string;
  context?: IntellisenseContext;
  serverUrl: string;
  workflowDefinitionId: string;
  currentExpression?: string;
  monacoEditor: HTMLElsaMonacoElement;
  expressionChangedHandler(newValue: string): void;
  setExpression(value: string): Promise<void>;
  componentWillLoad(): Promise<void>;
  componentDidLoad(): Promise<void>;
  onMonacoValueChanged(e: MonacoValueChangedArgs): Promise<void>;
  onEditorClick: (e: any) => void;
  render(): any;
}
