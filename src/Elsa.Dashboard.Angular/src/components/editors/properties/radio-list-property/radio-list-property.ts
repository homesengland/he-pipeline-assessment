import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'radio-list-property',
  templateUrl: './radio-list-property.html',
  standalone: false,
})
export class RadioListProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();

  fieldLabel = computed(() => this.propertyDescriptor()?.label ?? 'default');

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  ngOnInit(): void {

  }

  onCheckChanged(e: Event) {

  }


}
