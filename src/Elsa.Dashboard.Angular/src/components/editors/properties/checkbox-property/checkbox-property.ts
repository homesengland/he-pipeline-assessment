import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';

@Component({
  selector: 'checkbox-property',
  templateUrl: './checkbox-property.html',
  standalone: false,
})
export class CheckboxProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  isChecked = signal(false);

  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');

  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldLabel = computed(() => this.propertyDescriptor()?.label ?? 'default');

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  ngOnInit(): void {
    const defaultValue = this.propertyDescriptor().defaultValue?.toString() || '';
    this.isChecked.set((this.propertyModel().expressions[SyntaxNames.Literal] || defaultValue).toLowerCase() == 'true');
    if (this.propertyModel().expressions[SyntaxNames.Literal] === undefined) {
      this.updateExpression(defaultValue);
    }
  }

  onCheckChanged(e: Event) {
    const checkbox = e.target as HTMLInputElement;
    this.isChecked.set(checkbox.checked);

    this.updateExpression(checkbox.checked.toString());
  }

  private updateExpression(value: string) {
    let expressions = this.propertyModel().expressions;
    expressions[this.defaultSyntax()] = value;
    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
  }
}
