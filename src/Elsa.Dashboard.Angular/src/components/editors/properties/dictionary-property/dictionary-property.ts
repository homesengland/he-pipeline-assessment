import { Component, model, OnInit } from '@angular/core';
import {ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SyntaxNames} from "../../../../models";
import { ElsaClientService } from 'src/services/elsa-client';
import { Store } from '@ngrx/store';
// import Tunnel from "../../../../data/workflow-editor";
import { IconColor, IconName, iconProvider } from '../../../../services/icon-provider';

@Component({
  selector: 'dictionary-property',
  templateUrl: './dictionary-property.html',
  standalone: false,
})
export class ElsaDictionaryProperty {

  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  serverUrl: string;
  currentValue: [string, string][];

  constructor(private elsaClientService: ElsaClientService, private store: Store) {
    console.log('Setting property model', this.propertyModel());
  }

  async componentWillLoad() {
    this.currentValue = this.jsonToDictionary(this.propertyModel().expressions[SyntaxNames.Json] || null);
    if (this.currentValue.length === 0) this.currentValue = [['', '']];
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

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.currentValue = this.jsonToDictionary(e.detail);
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
}
