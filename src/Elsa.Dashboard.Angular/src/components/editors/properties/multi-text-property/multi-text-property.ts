import { Component, EventEmitter, model } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList, SelectListItem, SyntaxNames } from 'src/models';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'multi-text-property',
  templateUrl: './multi-text-property.html',
  standalone: false,
})
export class MultiTextProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  serverUrl: string;
  currentValue?: string;
  valueChange: EventEmitter<Array<string | number | boolean | SelectListItem>>;

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };
  $event: CustomEvent<any>;
  $defaultSyntaxEvent: CustomEvent;

  constructor() {
    console.log('MultiTextProperty component initialized');
  }

  async ngOnInit(): Promise<void> {
    this.currentValue = this.propertyModel().expressions['Json'] || '[]';
  }

  onValueChanged(newValue: Array<string | number | boolean | SelectListItem>) {
    const newValues = newValue.map(item => {
      if (typeof item === 'string') return item;
      if (typeof item === 'number') return item.toString();
      if (typeof item === 'boolean') return item.toString();

      return item.value;
    });

    this.valueChange.emit(newValue);
    this.currentValue = JSON.stringify(newValues);
    this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue;
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.currentValue = e.detail;
  }

  createKeyValueOptions(options: Array<SelectListItem>) {
    if (options === null) return options;

    return options.map(option => (typeof option === 'string' ? { text: option, value: option } : option));
  }

  getSelectItems(): any {
    console.log('Select list items:', this.selectList.items);
    return this.selectList.items.map((item, index) => {
      return {
        text: item.text,
        value: item.value,
        isSelected: this.currentValue == item.value,
        inputId: `${this.fieldId}_${index}`,
      };
    });
  }
}
