import { Component, h, Prop, Event, EventEmitter } from '@stencil/core';
import {
  ActivityPropertyDescriptor,
  IntellisenseContext,
} from "../../models/elsa-interfaces";

import { IconProvider } from "../icon-provider/icon-provider";
import TrashCanIcon from '../../icons/trash-can';

import { IOutcomeProperty, ITextProperty } from "../../models/custom-component-models"
import { SyntaxNames } from '../../constants/Constants';

@Component({
  tag: 'custom-outcome-property',
  shadow: false,
})
export class CustomOutcomeProperty {

  @Event() valueChanged: EventEmitter<IOutcomeProperty>;
  @Event() delete: EventEmitter<IOutcomeProperty>;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() outcome: IOutcomeProperty;
  @Prop() intellisenseContext: IntellisenseContext;
  @Prop({ attribute: 'editor-height', reflect: true }) editorHeight: string = '10em';
  @Prop({ attribute: 'single-line', reflect: true }) singleLineMode: boolean = false;
  @Prop({ attribute: 'context', reflect: true }) context?: string;
  @Prop() showLabel: boolean = true;
  @Prop() supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript];
  @Prop() iconProvider: IconProvider = new IconProvider();
  @Prop() index: number;
  syntaxSwitchCount: number = 0;


  onConditionChanged(event: CustomEvent<ITextProperty>) {
    let condition = event.detail;
    let outcomeModel: IOutcomeProperty = { text: this.outcome.text, condition: condition }
    this.valueChanged.emit(outcomeModel)
  }

  onTextChanged(event: CustomEvent<ITextProperty>) {
    let text = event.detail;
    let outcomeModel: IOutcomeProperty = { text: text, condition: this.outcome.condition }
    this.valueChanged.emit(outcomeModel);
  }

  onDelete() {
    this.delete.emit();
  }

  render() {
    const conditionProperty = this.outcome.condition;
    const textProperty = this.outcome.text;

    return <div>

      <tr>
        <th
          class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Text
        </th>
        <td class="elsa-py-2 elsa-pr-5">
          <custom-input-property
            onValueChanged={e => this.onConditionChanged(e)}
            customProperty={textProperty}
            propertyDescriptor={this.propertyDescriptor}
            intellisenseContext={this.intellisenseContext}
            editorHeight={this.editorHeight}
            singleLineMode={this.singleLineMode}
            context={this.context}
            showLabel={this.showLabel}
            supportedSyntaxes={this.supportedSyntaxes}
            index={this.index}
          >
          </custom-input-property>
        </td>
      </tr>
      <tr>
        <th
          class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Condition
        </th>
        <td class="elsa-py-2 pl-5">
          <custom-input-property
            onValueChanged={e => this.onConditionChanged(e) }
            customProperty={conditionProperty}
            propertyDescriptor={this.propertyDescriptor}
            intellisenseContext={this.intellisenseContext}
            editorHeight={this.editorHeight}
            singleLineMode={this.singleLineMode}
            context={this.context}
            showLabel={this.showLabel}
            supportedSyntaxes={this.supportedSyntaxes}
            index={this.index}
          >
          </custom-input-property>
        </td>
      </tr>
      <tr>

        <th
          class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-1/12">&nbsp;
        </th>
        <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
          <button type="button" onClick={() => this.onDelete()}
            class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
            <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
          </button>
        </td>
      </tr>
    </div>
  }
}
