import { Component, EventEmitter, model, OnInit } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList, SelectListItem, SyntaxNames } from 'src/models';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'multi-text-property',
  templateUrl: './multi-text-property.html',
  standalone: false,
})
export class MultiTextProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  serverUrl: string;
  currentValue?: string;
  valueChange = new EventEmitter<Array<string | number | boolean | SelectListItem>>();

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };

  constructor() {
    console.log('MultiTextProperty component initialized');
  }

  async ngOnInit(): Promise<void> {
    this.currentValue = this.propertyModel().expressions['Json'] || '[]';
  }

  onValueChanged(values: Array<string | number | boolean | SelectListItem>) {
    const newValues = values.map(item => {
      if (typeof item === 'string') return item;
      if (typeof item === 'number') return item.toString();
      if (typeof item === 'boolean') return item.toString();
      return item.value;
    });

    this.valueChange.emit(values);
    this.currentValue = JSON.stringify(newValues);
    this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue;
  }

  onDefaultSyntaxValueChanged(event: CustomEvent<string>) {
    this.currentValue = event.detail;
  }

  createKeyValueOptions(options: Array<SelectListItem>) {
    if (options === null) return options;
    return options.map(option => (typeof option === 'string' ? { text: option, value: option } : option));
  }

  getSelectItems(): any {
    return JSON.parse(this.currentValue || '[]');
  }

  get useDropdown() {
    return !!this.propertyDescriptor().options && Array.isArray(this.propertyDescriptor().options) && this.propertyDescriptor().options.length > 0;
  }

  get propertyOptions() {
    return this.createKeyValueOptions(this.selectList.items as Array<SelectListItem>);
  }
}
