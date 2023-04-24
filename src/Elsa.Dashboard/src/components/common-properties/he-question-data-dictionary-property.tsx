import { Component, h, Event, EventEmitter, Prop } from '@stencil/core';

import { SyntaxNames } from '../../constants/constants';
import { DataDictionary, DataDictionaryGroup } from '../../models/custom-component-models';
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
  @Event() expressionChanged: EventEmitter<string>;

  dataDictionary: Array<DataDictionary> = [];//change this to an array of class
  groupDictionary: Array<DataDictionaryGroup> = [];//change this to an array of class
  selectedGroupDictionaryId: number;

  async componentWillLoad() {
    //const propertyModel = this.propertyModel;
    this.groupDictionary = parseJson(this.propertyDescriptor.options);
    console.log('propmodel', this.propertyModel);
    console.log('propdescriptor', this.propertyDescriptor);
    console.log('groupDictionary', this.groupDictionary);
  }

  updatePropertyModel() {
    console.log('updatePropertyModel:', this.propertyModel);
/*    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.options);*/
    //this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.options, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onGroupDictionaryOptionChanged(e: Event) {
    const select = e.currentTarget as HTMLSelectElement;
    console.log('select', select);
    this.selectedGroupDictionaryId = parseInt(select.value);
    let selectedGroup = this.groupDictionary.filter(x => x.Id === this.selectedGroupDictionaryId)[0];
    this.dataDictionary = selectedGroup.QuestionDataDictionaryList;
    console.log('this.dataDictionary', this.dataDictionary);
    //this.propertyModel.expressions[SyntaxNames.Literal] = select.value;
    //this.propertyModel.syntax = SyntaxNames.Literal;
    this.updatePropertyModel();
  }

  onDataDictionaryOptionChanged(e: Event) {
    console.log("onDataDictionaryOptionChanged", e);
  }

  render() {
    const selectedDataDictionaryItem = this.propertyModel.expressions[SyntaxNames.Literal]; // needs to change to get the correct group from data Id
    const selectedGroupDictionaryId = this.selectedGroupDictionaryId;
    console.log('this.selectedGroupDictionaryId', this.selectedGroupDictionaryId);
    let selectedId = parseInt(selectedDataDictionaryItem);
    console.log('selectedItem', selectedDataDictionaryItem);

    return (
      <div>
      <h1>Group dictionary dropdown</h1>
        <select onChange={e => this.onGroupDictionaryOptionChanged(e)}
          class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="0">--- Select ---</option>
          {this.groupDictionary.map(data => {
            console.log(data);
            const selected = data.Id == selectedId;
          return <option selected={selected} value={data.Id}>{data.Name}</option>;
        })}
        </select>
        {selectedGroupDictionaryId != 0 && selectedGroupDictionaryId != undefined ?
        <div>
          <h1>Data dictionary dropdown</h1>
          <select onChange={e => this.onDataDictionaryOptionChanged(e)}
            class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
            <option value="0">--- Select ---</option>
            {this.dataDictionary.map(data => {
              console.log(data);
              const selected = data.Id == selectedId;
              return <option selected={selected} value={data.Id}>{data.Name}</option>;
            })}
          </select>
        </div> : ""}
      
      </div>
    )
  }


}
