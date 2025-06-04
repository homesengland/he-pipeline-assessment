import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'single-line-property',
  templateUrl: './single-line-property.html',
  standalone: false,
})
export class SingleLineProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  isEncypted = model<boolean>(false);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');

  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  isReadOnly = computed(() => this.propertyDescriptor()?.isReadOnly ?? false);

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  onChange(e: Event) {
    const input = e.currentTarget as HTMLInputElement;
    let expressions = this.propertyModel().expressions;
    expressions[this.defaultSyntax()] = input.value;
    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
  }

  onFocus(e: Event) {
    if (this.isEncypted) {
      const input = e.currentTarget as HTMLInputElement;
      let expressions = this.propertyModel().expressions;
      expressions[this.defaultSyntax()] = input.value;
      this.propertyModel.update(x => ({ ...x, expressions: expressions }));
    }
  }
}
