import { Component, h, EventEmitter, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
} from "../../../models/elsa-interfaces";

@Component({
  tag: 'he-single-line-property',
  shadow: false,
})
  //Copy of Elsa Switch Case
  //Copied to allow us control over how the expression editor is displayed.
export class HeSingleLineProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() currentValue: string;
  @Event() expressionChanged: EventEmitter<string>;


  onChange(e: Event) {
    const input = e.currentTarget as HTMLInputElement;
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }

  componentWillLoad() {
    console.log("he single line property loaded")
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }

  componentWillRender() {
    console.log("Component will render - single line property");
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.currentValue = e.detail;
  }

  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const isReadOnly = propertyDescriptor.isReadOnly;
    const fieldId = propertyName;
    const fieldName = propertyName;
    let value = this.currentValue;
    console.log("Rerendering single line property", fieldName)
    console.log("Value", value)
    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }

    if (isReadOnly) {
      const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
      this.propertyModel.expressions[defaultSyntax] = value;
    }

    return (
      <elsa-property-editor
        activityModel={this.activityModel}
        propertyDescriptor={propertyDescriptor}
        propertyModel={propertyModel}
        onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
        editor-height="100%"
        single-line={true}>
        <input type="text" id={fieldId} name={fieldName} value={value} onChange={e => this.onChange(e)}
          class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
          disabled={isReadOnly} />
      </elsa-property-editor>
    );
  }
}
