import { Component, h, Prop, Event, EventEmitter } from '@stencil/core';
import {
  ActivityPropertyDescriptor,
  IntellisenseContext,
  HTMLElsaExpressionEditorElement
} from "../../models/elsa-interfaces";

import { mapSyntaxToLanguage } from "../../utils/utils";

import { ITextProperty } from "../../models/custom-component-models"
import { SyntaxNames } from '../../constants/Constants';

@Component({
  tag: 'custom-input-property',
  shadow: false,
})
export class CustomPropertyEditor {

  @Event() valueChanged: EventEmitter<ITextProperty>;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() customProperty: ITextProperty;
  @Prop() intellisenseContext: IntellisenseContext;
  @Prop({ attribute: 'editor-height', reflect: true }) editorHeight: string = '10em';
  @Prop({ attribute: 'single-line', reflect: true }) singleLineMode: boolean = false;
  @Prop({ attribute: 'context', reflect: true }) context?: string;
  @Prop() showLabel: boolean = true;
  @Prop() supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript];
  @Prop() index: number;
  syntaxSwitchCount: number = 0;

  onSyntaxChanged(e: CustomEvent<string>) {
    this.customProperty.syntax = e.detail;
  }

  onExpressionChanged(e: CustomEvent<string>) {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    const syntax = this.customProperty.syntax || defaultSyntax;
    this.customProperty.expressions[syntax] = e.detail;

    if (syntax != defaultSyntax)
      return;

    this.valueChanged.emit(this.customProperty);
  }

  onCaseSyntaxChanged(e: Event, property: ITextProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.customProperty = property;

    if (this.customProperty.syntax != this.propertyDescriptor.defaultSyntax)
      return;

    this.valueChanged.emit(this.customProperty);
  }

  render() {
    const property = this.customProperty;
    const syntax = this.customProperty.syntax;
    const expression = this.customProperty.expressions;
    const monacoLanguage = mapSyntaxToLanguage(syntax);
    let expressionEditor = null;

    return <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">

      <elsa-expression-editor
        key={`expression-editor-${this.index}-${this.syntaxSwitchCount}`}
        ref={el => expressionEditor = el}
        expression={expression}
        language={monacoLanguage}
        single-line={false}
        editorHeight={this.editorHeight}
        padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
        onExpressionChanged={e => this.onExpressionChanged(e)}
      />

      <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
        <select onChange={e => this.onCaseSyntaxChanged(e, property, expressionEditor)}
          class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
          {this.supportedSyntaxes.map(supportedSyntax => {
            const selected = supportedSyntax == this.customProperty.syntax;
            return <option selected={selected}>{supportedSyntax}</option>;
          })}
        </select>
      </div>
    </div>
  }
}
