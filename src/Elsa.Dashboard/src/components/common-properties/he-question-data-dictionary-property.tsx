import { Component, h, Event, EventEmitter, Prop } from '@stencil/core';

import { SyntaxNames } from '../../constants/constants';
import { DataDictionary, DataDictionaryGroup, NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
} from "../../models/elsa-interfaces";

//import { mapSyntaxToLanguage, parseJson, Map } from "../../utils/utils";
import { parseJson } from "../../utils/utils";

@Component({
  tag: 'he-question-data-dictionary-property',
  shadow: false,
})
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
export class HEQuestionDataDictionaryProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() dictionary: NestedActivityDefinitionProperty;
  @Event() expressionChanged: EventEmitter<string>;

  dataDictionary: Array<DataDictionary> = [];//change this to an array of class
  groupDictionary: Array<DataDictionaryGroup> = [];//change this to an array of class

  async componentWillLoad() {
    //const propertyModel = this.propertyModel;
    this.groupDictionary = parseJson(this.propertyDescriptor.options);
    console.log('propmodel', this.propertyModel);
    console.log('propdescriptor', this.propertyDescriptor);
    console.log('datadictionary', this.dataDictionary);
  }

  updatePropertyModel() {
/*    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.options);*/
    //this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.options, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onDataDictionaryOptionChanged(e: Event, dictionary: NestedActivityDefinitionProperty) {
    const select = e.currentTarget as HTMLSelectElement;
    console.log('select', select);
    console.log('dictionary', dictionary);
    dictionary.expressions.expressions[SyntaxNames.Json] = JSON.stringify(select.value);
    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(select.value);
    this.updatePropertyModel();
  }

  render() {
    const selectedDataDictionaryItem = this.propertyModel.expressions[SyntaxNames.Json];
    console.log('selectedItem', selectedDataDictionaryItem);

    return (
      <div>
      <h1>Data dictionary dropdown</h1>
        <select onChange={e => this.onDataDictionaryOptionChanged(e, this.dictionary)}
        class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          {this.groupDictionary.map(data => {
            console.log(data);
/*          const selected = data.trim() === selectedDataDictionaryItem;*/
          //return <option selected={selected} value={data}>{data}</option>;
          return <option value={data.Id}>{data.Name}</option>;
        })}
        </select>
      </div>
    )
  }


}
