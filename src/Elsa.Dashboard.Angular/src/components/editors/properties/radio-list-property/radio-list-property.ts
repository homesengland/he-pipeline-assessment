import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, SelectList } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { getSelectListItems } from 'src/utils/selected-list-items';

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

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  getSelectItems(): any {
    console.log('Select list items:', this.selectList.items);
    return this.selectList.items.map(item => {
      return {
        text: item.text,
        value: item.value,
        isSelected: true,
        inputId: '${this.fieldId}_${index}',
      };
    });
  }

  ngOnInit(): void {}

  onCheckChanged(e: Event) {}
}
