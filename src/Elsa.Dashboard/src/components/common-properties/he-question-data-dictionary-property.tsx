import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';

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
  //@State() 
  @State() dataDictionary: Array<DataDictionary> = [];
  @State() selectedDataDictionaryItem: number;

  groupDictionary: Array<DataDictionaryGroup> = [];//change this to an array of class
  selectedGroupDictionaryId: number;
  selectedGroup: DataDictionaryGroup;
  dataDictionaryDisplayToggle: boolean = false;

  async componentWillLoad() {
    //const propertyModel = this.propertyModel;
    this.groupDictionary = parseJson(this.propertyDescriptor.options);
    this.dataDictionary = [];
    this.selectedDataDictionaryItem = parseInt(this.propertyModel.expressions[SyntaxNames.Literal]);
    console.log('propmodel', this.propertyModel);
    console.log('propdescriptor', this.propertyDescriptor);
    console.log('groupDictionary', this.groupDictionary);

  
    if (this.selectedGroup == undefined) {
      this.selectedGroup = this.groupDictionary.filter(x => x.QuestionDataDictionaryList.filter(y => y.Id == this.selectedDataDictionaryItem)[0])[0];
    }
    if (this.selectedGroup != undefined) {
      this.dataDictionaryDisplayToggle = true;
      this.dataDictionary = [...this.selectedGroup.QuestionDataDictionaryList];

    }
  }

  updatePropertyModel(updatedValue: string) {
    console.log('updatePropertyModel:', this.propertyModel);
    this.propertyModel.expressions[SyntaxNames.Literal] = updatedValue;
/*    this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.options);*/
    //this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.options, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onGroupDictionaryOptionChanged(e: Event) {
    const select = e.currentTarget as HTMLSelectElement;
    console.log('select', select);
    this.selectedGroupDictionaryId = parseInt(select.value);
    let selectedGroup = this.groupDictionary.filter(x => x.Id === this.selectedGroupDictionaryId)[0];
    if (selectedGroup != undefined) {
      this.selectedDataDictionaryItem = 0;
      this.selectedGroup = selectedGroup;
      this.dataDictionaryDisplayToggle = true;
      this.dataDictionary = [...selectedGroup.QuestionDataDictionaryList];

    }
    else {
      this.dataDictionaryDisplayToggle = false;
      this.dataDictionary = [];
    }

    console.log('this.dataDictionary', this.dataDictionary);
    this.updatePropertyModel('');
  }

  onDataDictionaryOptionChanged(e: Event) {

    const select = e.currentTarget as HTMLSelectElement;
    console.log("onDataDictionaryOptionChanged", e);
    this.selectedDataDictionaryItem = parseInt(select.value);
    this.updatePropertyModel(select.value);
  }

  render() {
    //const dataDictionary = this.dataDictionary;
    console.log('db value', this.propertyModel.expressions[SyntaxNames.Literal]);

    const selectedDataDictionaryItem = this.selectedDataDictionaryItem;
    const clearSelection = selectedDataDictionaryItem == 0;
    let selectedGroup = this.selectedGroup;
    if (selectedGroup == undefined) {
      selectedGroup = this.groupDictionary.filter(x => x.QuestionDataDictionaryList.filter(y => y.Id == selectedDataDictionaryItem)[0])[0];
    }

    /*const selectedGroupDictionaryId = this.selectedGroupDictionaryId;*/
    console.log('this.selectedGroupDictionaryId', this.selectedGroupDictionaryId);
    console.log('selectedGroup', selectedGroup);
    console.log('selectedItem', selectedDataDictionaryItem);

    return (
      <div>
      <h1>Group dictionary dropdown</h1>
        <select onChange={e => this.onGroupDictionaryOptionChanged(e)}
          class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="0">--- Select ---</option>
          {this.groupDictionary.map(data => {
            console.log(data);
            const selected = data == selectedGroup;
          return <option selected={selected} value={data.Id}>{data.Name}</option>;
        })}
        </select>
        {this.dataDictionaryDisplayToggle ?
        <div>
          <h1>Data dictionary dropdown</h1>
          <select id="test_1" onChange={e => this.onDataDictionaryOptionChanged(e)}
              class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
              <option selected={clearSelection} value="0">--- Select ---</option>
              {console.log('render dataDictionary', this.dataDictionary)}
              {console.log('typeof dataDictionary', typeof this.dataDictionary)}
            {this.dataDictionary.map(data => {
              console.log(data);
              const selected = data.Id == selectedDataDictionaryItem;
              console.log(selected);
              return <option selected={selected} value={data.Id}>{data.Name}</option>;
            })}
          </select>
        </div> : ""}
      
      </div>
    )
  }


}
