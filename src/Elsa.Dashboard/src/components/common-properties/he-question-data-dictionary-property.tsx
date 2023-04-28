import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';

import { SyntaxNames } from '../../constants/constants';
import { DataDictionary, DataDictionaryGroup } from '../../models/custom-component-models';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
} from "../../models/elsa-interfaces";

//import { mapSyntaxToLanguage, parseJson, Map } from "../../utils/utils";


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
  @Prop() dataDictionaryGroup: Array<DataDictionaryGroup> = [];

  @Event() expressionChanged: EventEmitter<string>;
  @State() dataDictionary: Array<DataDictionary> = [];
  @State() selectedDataDictionaryItem: number;

  selectedGroupDictionaryId: number;
  selectedGroup: DataDictionaryGroup;
  dataDictionaryDisplayToggle: boolean = false;

  async componentWillLoad() {
    
    this.dataDictionary = [];
    this.selectedDataDictionaryItem = parseInt(this.propertyModel.expressions[SyntaxNames.Literal]);
  
    this.selectedGroup = this.dataDictionaryGroup.filter(x => x.QuestionDataDictionaryList.filter(y => y.Id == this.selectedDataDictionaryItem)[0])[0];
    if (this.selectedGroup != undefined) {
      this.dataDictionaryDisplayToggle = true;
      this.dataDictionary = [...this.selectedGroup.QuestionDataDictionaryList];

    }
  }

  updatePropertyModel(updatedValue: string) {
    this.propertyModel.expressions[SyntaxNames.Literal] = updatedValue;
    this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  onGroupDictionaryOptionChanged(e: Event) {
    const select = e.currentTarget as HTMLSelectElement;
    this.selectedGroupDictionaryId = parseInt(select.value);
    let selectedGroup = this.dataDictionaryGroup.filter(x => x.Id === this.selectedGroupDictionaryId)[0];
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
    this.updatePropertyModel('');
  }

  onDataDictionaryOptionChanged(e: Event) {

    const select = e.currentTarget as HTMLSelectElement;
    this.selectedDataDictionaryItem = parseInt(select.value);
    this.updatePropertyModel(select.value);
  }

  render() {
    const displayGroupStyle = "inline-block";
    const displayDataDictionaryItemsStyle =  "inline-block" ;
    const selectedDataDictionaryItem = this.selectedDataDictionaryItem;
    const clearSelection = selectedDataDictionaryItem == 0;
    let selectedGroup = this.selectedGroup;
    if (selectedGroup == undefined) {
      selectedGroup = this.dataDictionaryGroup.filter(x => x.QuestionDataDictionaryList.filter(y => y.Id == selectedDataDictionaryItem)[0])[0];
    }

    return (
      <div style={{ padding: "10px 0px 10px 0px" }}>
      <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Data Dictionary</label>
      <div class="elsa-justify-content">
        <span style={{ display: displayGroupStyle, padding: "0px 10px 0px 0px" }}>
        <select onChange={e => this.onGroupDictionaryOptionChanged(e)}
          class="elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
          <option value="0">Choose Group</option>
              {this.dataDictionaryGroup.map(data => {
            const selected = data == selectedGroup;
          return <option selected={selected} value={data.Id}>{data.Name}</option>;
        })}
          </select>
        </span>
        {this.dataDictionaryDisplayToggle ?
          <span style={{ display: displayDataDictionaryItemsStyle, padding: "0px 5px 0px 0px" }}>
          <select id="test_1" onChange={e => this.onDataDictionaryOptionChanged(e)}
              class="elsa-mt-1 elsa-inline-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
              <option selected={clearSelection} value="0">Choose Field</option>
            {this.dataDictionary.map(data => {
              const selected = data.Id == selectedDataDictionaryItem;
              return <option selected={selected} value={data.Id}>{data.Name}</option>;
            })}
          </select>
          </span> : ""}
      
        </div>
        <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Assigns a data dictionary field to the question for reporting purposes</p>
        </div>
    )
  }


}
