import { Component, h, Prop, State } from '@stencil/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SyntaxNames } from "../models/elsa-interfaces";

@Component({
  tag: 'custom-single-line-property',
  shadow: false,
})
export class CustomSingleLineProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() currentValue: string;

  onChange(e: Event) {
    console.log('OnChangeEvent')
    console.log("Property Model", this.propertyModel)
    console.log("Syntax", this.propertyModel.syntax)
    const input = e.currentTarget as HTMLInputElement;
    console.log("input", input)
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    console.log("Default Syntax", defaultSyntax);
    this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
    console.log("Property Model after Changes", this.propertyModel);
  }

  componentWillLoad() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    console.log('syntax changed', e.detail)
    this.currentValue = e.detail;
    console.log('current Value', e.detail);
  }

  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const isReadOnly = propertyDescriptor.isReadOnly;
    const fieldId = propertyName;
    const fieldName = propertyName;
    let value = this.currentValue;

    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }

    if (isReadOnly) {
      const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
      this.propertyModel.expressions[defaultSyntax] = value;
    }

    return (
      <custom-property-editor
        activityModel={this.activityModel}
        propertyDescriptor={propertyDescriptor}
        propertyModel={propertyModel}
        onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
        editor-height="5em"
        single-line={true}>
        <input type="text" id={fieldId} name={fieldName} value={value} onChange={e => this.onChange(e)}
          class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
          disabled={isReadOnly} />
      </custom-property-editor>
    );
  }
}
