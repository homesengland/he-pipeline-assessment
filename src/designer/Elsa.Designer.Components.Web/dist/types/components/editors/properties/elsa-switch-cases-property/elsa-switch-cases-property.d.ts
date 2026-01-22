import { EventEmitter } from '../../../../stencil-public-runtime';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "../../../../models";
import { SwitchCase } from "./models";
export declare class ElsaSwitchCasesProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  cases: Array<SwitchCase>;
  valueChange: EventEmitter<Array<any>>;
  supportedSyntaxes: Array<string>;
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number;
  componentWillLoad(): Promise<void>;
  updatePropertyModel(): void;
  onDefaultSyntaxValueChanged(e: CustomEvent): void;
  onAddCaseClick(): void;
  onDeleteCaseClick(switchCase: SwitchCase): void;
  onCaseNameChanged(e: Event, switchCase: SwitchCase): void;
  onCaseExpressionChanged(e: CustomEvent<string>, switchCase: SwitchCase): void;
  onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: HTMLElsaExpressionEditorElement): void;
  onMultiExpressionEditorValueChanged(e: CustomEvent<string>): void;
  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>): void;
  render(): any;
}
