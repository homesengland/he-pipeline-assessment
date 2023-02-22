import { Component, EventEmitter, Event, Prop, State } from '@stencil/core';

import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
  ActivityDefinitionProperty
} from '../../models/elsa-interfaces';

import {
    HeActivityPropertyDescriptor,
  NestedProperty,
  QuestionModel,
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { HePropertyDisplayManager } from '../../nested-drivers/display-managers/display-manager';
import { parseJson, Map } from '../../utils/utils';
import { SyntaxNames } from '../../constants/Constants';

@Component({
  tag: 'question-property',
  shadow: false,
})

export class QuestionProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() questionModel: QuestionModel;
  @State() iconProvider = new IconProvider();
  @State() currentValue: string;
  @State() nestedQuestionProperties: Array<NestedProperty>;
  @State() displayManager: HePropertyDisplayManager = new HePropertyDisplayManager();
  @Event() updateQuestionScreen: EventEmitter<string>;

  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;
  

  async componentWillLoad() {
    this.getOrCreateQuestionProperties();
  }

  getOrCreateQuestionProperties() {
    const model = this.questionModel;
    const propertyJson = model.value.expressions[SyntaxNames.QuestionList]
    if (propertyJson != null && propertyJson != undefined && parseJson(propertyJson).length > 0) {
      
      this.nestedQuestionProperties = parseJson(propertyJson);
    }
    else {
      this.nestedQuestionProperties = this.createQuestionProperties();
    }
  }

  createQuestionProperties(): Array<NestedProperty> {
    let propertyArray: Array<NestedProperty> = [];
    const descriptor = this.questionModel.descriptor;
    descriptor.forEach(d => {
      var prop = this.createQuestionProperty(d);
      propertyArray.push(prop);
    });
    return propertyArray;
  }

  createQuestionProperty(descriptor: HeActivityPropertyDescriptor): NestedProperty {
    let propertyValue: ActivityDefinitionProperty = {
      syntax: descriptor.defaultSyntax,
      value: descriptor.expectedOutputType,
      name: descriptor.name,
      expressions: this.getExpressionMap(descriptor.supportedSyntaxes)
    }
    console.log("Creating Question Property", propertyValue);
    let property: NestedProperty = { value: propertyValue, descriptor: descriptor }
    return property;
  }

  onPropertyExpressionChange(event: Event, property: NestedProperty) {
    event = event;
    property = property;
    this.updateQuestionModel();
  }

  updateQuestionModel() {
    console.log("Nested Properties", this.nestedQuestionProperties);
    let nestedQuestionsJson = JSON.stringify(this.nestedQuestionProperties);
    this.questionModel.value.expressions[SyntaxNames.QuestionList] = nestedQuestionsJson;
    this.updateQuestionScreen.emit(JSON.stringify(this.questionModel));
  }

  getExpressionMap(syntaxes: Array<string>): Map<string> {
    if (syntaxes.length > 0) {
      var value: Map<string> = { };
      syntaxes.forEach(s => {
        console.log("Syntax", s);
        value[s] = "";
      })
      return value;
    }
    else {
      let value: Map<string> = {};
      //value[SyntaxNames.Literal] = '';
      return value;
    }
  }




  render() {

    const displayManager = this.displayManager;

    const renderPropertyEditor = (property: NestedProperty) => {
      console.log("Display Manager", this.displayManager);
      var content = displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange.bind(this));
      return content;
    }

    return (
      this.nestedQuestionProperties.map(renderPropertyEditor)
    )
    
  }
}
