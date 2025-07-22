import { Component, EventEmitter, model, computed, OnInit, Output } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList, SelectListItem, SyntaxNames } from 'src/models';
import { PropertyEditor } from '../../property-editor/property-editor';
import { ElsaClientService } from 'src/services/elsa-client';
import { Store } from '@ngrx/store';
import { getSelectListItems } from 'src/utils/selected-list-items';

@Component({
  selector: 'multi-text-property',
  templateUrl: './multi-text-property.html',
  standalone: false,
})
export class MultiTextProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name || 'default');
  serverUrl: string;
  currentValue?: string;
  // valueChange = new EventEmitter<Array<string | number | boolean | SelectListItem>>();

  @Output() valueChange = new EventEmitter<Array<string | number | boolean | SelectListItem>>();

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false
  };

  constructor(private elsaClientService: ElsaClientService, private store: Store) {
    console.log('MultiTextProperty component initialized');
  }

  async ngOnInit(): Promise<void> {
    this.currentValue = this.propertyModel().expressions[SyntaxNames.Json] || '[]';

    this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, this.propertyDescriptor());
  }

  //// original
  //onValueChanged(values: Array<string | number | boolean | SelectListItem>) {
  //  const newValues = values.map(item => {
  //    if (typeof item === 'string') return item;
  //    if (typeof item === 'number') return item.toString();
  //    if (typeof item === 'boolean') return item.toString();
  //    return item.value;
  //  });

  //  this.valueChange.emit(values);
  //  this.currentValue = JSON.stringify(newValues);
  //  this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue;
  //}

  onValueChanged(event: any) {

    let values: Array<string | number | boolean | SelectListItem> = [];

    if (event instanceof CustomEvent) {
      values = event.detail;
    } else if (Array.isArray(event)) {
      values = event;
    } else if (event && event.target && event.target.value) {
      values = [event.target.value];
    }

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

  //// original
  //onDefaultSyntaxValueChanged(event: CustomEvent<string>) {
  //  this.currentValue = event.detail;
  //}

  onDefaultSyntaxValueChanged(e: Event) {
    this.currentValue = (e as CustomEvent<string>).detail
  }

  createKeyValueOptions(options: Array<SelectListItem>) {
    if (options === null)
      return options;

    return options.map(option => typeof option === 'string' ? { text: option, value: option } : option);
  }

  getSelectItems(): any {
    return JSON.parse(this.currentValue || '[]');
  }

  useDropdown() {
    return !!this.propertyDescriptor().options && Array.isArray(this.propertyDescriptor().options) && this.propertyDescriptor().options.length > 0;
  }

  getPropertyOptions() {
    return this.createKeyValueOptions(this.selectList.items as Array<SelectListItem>);
  }
}
