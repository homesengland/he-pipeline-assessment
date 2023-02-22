import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/Constants';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
} from "../../../models/elsa-interfaces";


@Component({
  tag: 'he-checkbox-property',
  shadow: false,
})
//Copy of Elsa Checkbox Property
//Copied to allow us control over how the expression editor is displayed.
export class HECheckboxProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Event() expressionChanged: EventEmitter<string>;

  @State() isChecked: boolean

  async componentWillLoad() {
    this.isChecked = (this.propertyModel.expressions[SyntaxNames.Literal] || this.propertyDescriptor.defaultValue?.toString() || '').toLowerCase() == 'true';
  }

  onCheckChanged(e: Event) {
    const checkbox = (e.target as HTMLInputElement);
    this.isChecked = checkbox.checked;
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.propertyModel.expressions[defaultSyntax] = this.isChecked.toString();
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.isChecked = (e.detail || '').toLowerCase() == 'true';
  }

  render() {
    const propertyDescriptor = this.propertyDescriptor;
    const propertyModel = this.propertyModel;
    const propertyName = propertyDescriptor.name;
    const fieldId = propertyName;
    const fieldName = propertyName;
    const fieldLabel = propertyDescriptor.label || propertyName;
    let isChecked = this.isChecked;

    return (
      <elsa-property-editor
        activityModel={this.activityModel}
        propertyDescriptor={propertyDescriptor}
        propertyModel={propertyModel}
        onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
        single-line={true}
        showLabel={false}>
        <div class="elsa-max-w-lg">
          <div class="elsa-relative elsa-flex elsa-items-start">
            <div class="elsa-flex elsa-items-center elsa-h-5">
              <input id={fieldId} name={fieldName} type="checkbox" checked={isChecked} value={'true'}
                onChange={e => this.onCheckChanged(e)}
                class="focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </div>
            <div class="elsa-ml-3 elsa-text-sm">
              <label htmlFor={fieldId} class="elsa-font-medium elsa-text-gray-700">{fieldLabel}</label>
            </div>
          </div>
        </div>
      </elsa-property-editor>
    )
  }
}
