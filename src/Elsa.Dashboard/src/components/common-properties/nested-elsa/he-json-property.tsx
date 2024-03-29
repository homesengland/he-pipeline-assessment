import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  MonacoValueChangedArgs,
} from "../../../models/elsa-interfaces";

@Component({
  tag: 'he-json-property',
  shadow: false,
})
  //Copy of Elsa Switch Case
  //Copied to allow us control over how the expression editor is displayed.
export class HEJsonProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() keyId: string;
  @Event() expressionChanged: EventEmitter<string>;

  @State() currentValue: string;

  componentWillLoad() {
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Json;
    this.currentValue = this.propertyModel.expressions[defaultSyntax] || undefined;
  }

  getEditorHeight(options: any) {
    const editorHeightName = options.editorHeight || 'Large';

    switch (editorHeightName) {
      case 'Large':
        return '20em';
      case 'Default':
      default:
        return '15em';
    }
  }

  onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue = e.value;
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.currentValue = e.detail;
  }

  componentWillRender() {
    this.keyId = getUniversalUniqueId();
  }

  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const options = propertyDescriptor.options || {};
    const editorHeight = this.getEditorHeight(options);
    const context: string = options.context;
    let value = this.currentValue;

    if (value == undefined) {
      const defaultValue = this.propertyDescriptor.defaultValue;
      value = defaultValue ? defaultValue.toString() : undefined;
    }

    return (
      <he-property-editor
        key={`property-editor-${this.keyId}`}
        activityModel={this.activityModel}
        propertyDescriptor={propertyDescriptor}
        propertyModel={propertyModel}
        onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
        editor-height={editorHeight}
        context={context}>
        <elsa-monaco value={value}
          language="json"
          editor-height={editorHeight}
          onValueChanged={e => this.onMonacoValueChanged(e.detail)}
        />
      </he-property-editor>
    );
  }
}
