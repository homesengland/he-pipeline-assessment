import { Component, computed, EventEmitter, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { Map } from 'src/utils/utils';
import { SwitchCase } from './models';

@Component({
  selector: 'switch-case-property',
  templateUrl: './switch-case-property.html',
  standalone: false,
})
export class SwitchCaseProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  
  cases: Array<SwitchCase> = [];
  valueChange: EventEmitter<Array<any>>;


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

  }



}
