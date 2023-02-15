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
import { HePropertyDisplayManager } from '../../models/display-manager';
import { parseJson, Map } from '../../models/utils';
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

  //supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;
  

  async componentWillLoad() {
    console.log("Loading Display Manager", this.displayManager)
    console.log("Component loading", this.questionModel)
    const model = this.questionModel;
    const propertyJson = model.value.expressions[SyntaxNames.Json]
    console.log("Property Json", propertyJson);
    this.getOrCreateQuestionProperties();
    console.log("Question Properties", this.nestedQuestionProperties)
  }

  getOrCreateQuestionProperties() {
    const model = this.questionModel;
    const propertyJson = model.value.expressions[SyntaxNames.Json]
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
      value: null,
      name: descriptor.name,
      expressions: this.getExpressionMap(descriptor.supportedSyntaxes)
    }
    let property: NestedProperty = { value: propertyValue, descriptor: descriptor }
    return property;
  }

  onPropertyExpressionChange(event: Event, property: NestedProperty) {
    console.log("Expression Changed Event", event);
    console.log("Expression Changed Property", property);
    this.updateQuestionModel();
  }

  updateQuestionModel() {
    let nestedQuestionsJson = JSON.stringify(this.nestedQuestionProperties);
    this.questionModel.value.expressions[SyntaxNames.Json] = nestedQuestionsJson;
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
