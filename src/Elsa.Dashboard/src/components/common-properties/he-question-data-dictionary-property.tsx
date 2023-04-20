import { Component, h, Prop, /*State*/ } from '@stencil/core';

import { SyntaxNames } from '../../constants/constants';
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

  dataDictionary: Array<string> = [];//change this to an array of class

  async componentWillLoad() {
    //const propertyModel = this.propertyModel;
    this.dataDictionary = parseJson(this.propertyDescriptor.options);
    console.log(this.propertyDescriptor);
    console.log(this.dataDictionary);
  }

  updatePropertyModel() {
    //this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.options);
    //this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.options, null, 2);
    //this.expressionChanged.emit(JSON.stringify(this.propertyModel))
  }

  //onDataDictionaryOptionChanged(e: Event, property: NestedActivityDefinitionProperty) {
  //  const select = e.currentTarget as HTMLSelectElement;
  //  property.expressions[RadioOptionsSyntax.PotScore] = select.value;
  //  this.updatePropertyModel();
  //}

  render() {
    const selectedDataDictionaryItem = this.propertyModel.expressions[SyntaxNames.DataDictionary];
    console.log(selectedDataDictionaryItem);
    //<select onChange={e => this.onDataDictionaryOptionChanged(e)}
    return (
      <div>
      <h1>Data dictionary dropdown</h1>
      <select
        class="elsa-mt-1 elsa-block focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-w-full elsa-shadow-sm sm:elsa-max-w-xs sm:elsa-text-sm elsa-border-gray-300 elsa-rounded-md">
        {this.dataDictionary.map(data => {
          const selected = data.trim() === selectedDataDictionaryItem;
          return <option selected={selected} value={data}>{data}</option>;
        })}
        </select>
      </div>
    )
  }


}
