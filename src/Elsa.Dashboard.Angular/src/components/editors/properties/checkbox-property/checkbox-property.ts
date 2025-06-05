import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'checkbox-property',
  templateUrl: './checkbox-property.html',
  standalone: false,
})
export class CheckboxProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  isChecked = model<boolean>(false);

  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');

  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  isReadOnly = computed(() => this.propertyDescriptor()?.isReadOnly ?? false);

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  onCheckChanged(e: Event) {
    const checkbox = (e.target as HTMLInputElement);
    isChecked = checkbox.checked;

    let expressions = this.propertyModel().expressions;
    expressions[this.defaultSyntax()] = checkbox.checked.toString();
    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
  }

  // from checkbox property Elsa
  onCheckChanged(e: Event) {
    const checkbox = (e.target as HTMLInputElement);
    this.isChecked = checkbox.checked;
    const defaultSyntax = this.propertyDescriptor.defaultSyntax || SyntaxNames.Literal;
    this.propertyModel.expressions[defaultSyntax] = this.isChecked.toString();
  }
  //

}
