import { Component, model, OnInit, computed } from '@angular/core';
import {ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SyntaxNames} from "../../../../models";
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyEditor } from '../../property-editor/property-editor';
import { Map } from '../../../../utils/utils';

@Component({
  selector: 'dictionary-property',
  templateUrl: './dictionary-property.html',
  standalone: false,
})
export class DictionaryProperty {

  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  currentValue: [string, string][];
  activityIconProvider: any;
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');
  items: [string, string][];

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    this.currentValue = this.jsonToDictionary(this.propertyModel().expressions[SyntaxNames.Json] || null);
    if (this.currentValue.length === 0)
      this.currentValue = [['', '']];
  }

  jsonToDictionary = (json: string): [string, string][] => {
    if (!json) return [['', '']];

    const parsedValue = JSON.parse(json);

    return Object.keys(parsedValue).map(key => [key, parsedValue[key]]);
  }

  dictionaryToJson = (dictionary: [string, string][]) => {
    const filteredDictionary = this.removeInvalidKeys(dictionary);

    if (filteredDictionary.length === 0) return null;

    return JSON.stringify(Object.fromEntries(filteredDictionary));
  }

  removeInvalidKeys = (dictionary: [string, string][]) => {
    const filteredDictionary = [];

    dictionary.forEach(x => {
      const key = x[0].trim();
      if (key !== '' && !filteredDictionary.some(y => y[0].trim() === key))
        filteredDictionary.push(x);
    });

    return filteredDictionary;
  }

  onRowAdded = () => {
    //changing contents of array won't trigger state change,
    //need to update the reference by creating new array
    this.currentValue = [...this.currentValue, ['', '']];
  }

  onRowDeleted = (index: number) => {
    const newValue = this.currentValue.filter((x, i) => i !== index);

    if (newValue.length === 0) newValue.push(['', '']);

    this.currentValue = newValue;
    this.propertyModel().expressions[SyntaxNames.Json] = this.dictionaryToJson(newValue);
  }

  onDefaultSyntaxValueChanged(e: Event) {
    this.currentValue = this.jsonToDictionary((e as CustomEvent).detail);
    
  }

  onKeyChanged(e: Event, index: number) {
    const input = e.currentTarget as HTMLInputElement;
    this.currentValue[index][0] = input.value
    this.propertyModel().expressions[SyntaxNames.Json] = this.dictionaryToJson(this.currentValue);
  }

  onValueChanged(e: Event, index: number) {
    const input = e.currentTarget as HTMLInputElement;
    this.currentValue[index][1] = input.value
    this.propertyModel().expressions[SyntaxNames.Json] = this.dictionaryToJson(this.currentValue);
  }

  getItems(): any {
    this.items = this.currentValue

    return this.items.map((item, index) => {
      let isLast = index === (this.items.length - 1);

      return {
        keyInputId: `${this.fieldId}_${index}_key`,
        valueInputId: `${this.fieldId}_${index}_value`,
        isLast: isLast,
        value: item,
        index: index,
      };
    });
  }
}
