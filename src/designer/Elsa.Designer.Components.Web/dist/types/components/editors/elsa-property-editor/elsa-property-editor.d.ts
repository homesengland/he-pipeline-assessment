import { EventEmitter } from '../../../stencil-public-runtime';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "../../../models";
export declare class ElsaPropertyEditor {
  defaultSyntaxValueChanged: EventEmitter<string>;
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  editorHeight: string;
  singleLineMode: boolean;
  context?: string;
  showLabel: boolean;
  onSyntaxChanged(e: CustomEvent<string>): void;
  onExpressionChanged(e: CustomEvent<string>): void;
  render(): any;
}
