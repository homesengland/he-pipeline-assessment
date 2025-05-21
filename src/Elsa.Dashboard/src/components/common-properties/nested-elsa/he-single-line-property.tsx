import { Component, h, EventEmitter, Event, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
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
  @Prop() keyId: string;
  @State() currentValue: string;
  @Event() expressionChanged: EventEmitter<string>;


  onChange(e: Event) {
    const input = e.currentTarget as HTMLInputElement;
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.propertyModel.expressions[defaultSyntax] = this.currentValue = input.value;
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }

  componentWillLoad() {

    console.log("sample activity model", JSON.stringify(this.activityModel))
    console.log("sample property descriptor", JSON.stringify(this.propertyDescriptor))
    console.log("sample property model", JSON.stringify(this.propertyModel))
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }

  componentWillRender() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
    this.keyId = getUniversalUniqueId();
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
    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }

    if (isReadOnly) {
      const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
      this.propertyModel.expressions[defaultSyntax] = value;
    }

    return (
      <he-property-editor
        key={`property-editor-${fieldId}-${this.keyId}`}
        activityModel={this.activityModel}
        propertyDescriptor={propertyDescriptor}
        propertyModel={propertyModel}
        onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
        editor-height="100%"
        single-line={true}>
        <input type="text" id={fieldId} name={fieldName} value={value} onChange={e => this.onChange(e)}
          class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
          disabled={isReadOnly} />
      </he-property-editor>
    );
  }
}
