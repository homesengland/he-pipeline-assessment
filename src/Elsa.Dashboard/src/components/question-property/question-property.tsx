import { Component, Prop, State } from '@stencil/core';

import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
  ActivityDefinitionProperty,
  ActivityPropertyDescriptor,
  SyntaxNames,
} from '../../models/elsa-interfaces';

import {
  HeProperty,
  QuestionProperty2,
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { QuestionEventHandler } from '../../events/component-events';
import { HePropertyDisplayManager } from '../../models/display-manager';
import { parseJson, Map } from '../../models/utils';

@Component({
  tag: 'question-property-v3',
  shadow: false,
})

export class HEQuestionComponent {

  @Prop() activityModel: ActivityModel;
  @Prop() questionModel: QuestionProperty2;
  @State() iconProvider = new IconProvider();
  @State() currentValue: string;
  @State() questionProperties: Array<HeProperty>;
  @State() displayManager: HePropertyDisplayManager = new HePropertyDisplayManager();

  handler: QuestionEventHandler;

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
    console.log("Question Properties", this.questionProperties)
  }

  getOrCreateQuestionProperties() {
    const model = this.questionModel;
    const propertyJson = model.value.expressions[SyntaxNames.Json]
    if (propertyJson != null && propertyJson != undefined && parseJson(propertyJson).length > 0) {
      
      this.questionProperties = parseJson(propertyJson);
    }
    else {
      this.questionProperties = this.createQuestionProperties();
    }
  }

  createQuestionProperties(): Array<HeProperty> {
    let propertyArray: Array<HeProperty> = [];
    const descriptor = this.questionModel.descriptor;
    descriptor.forEach(d => {
      var prop = this.createQuestionProperty(d);
      propertyArray.push(prop);
    });
    return propertyArray;
  }

  createQuestionProperty(descriptor: ActivityPropertyDescriptor): HeProperty {
    let propertyValue: ActivityDefinitionProperty = {
      syntax: descriptor.defaultSyntax,
      value: null,
      name: descriptor.name,
      expressions: this.getExpressionMap(descriptor.supportedSyntaxes)
    }
    let property: HeProperty = { value: propertyValue, descriptor: descriptor }
    return property;
  }

  onPropertyExpressionChange(event: Event, property: HeProperty) {
    console.log("Expression Changed Event", event);
    console.log("Expression Changed Property", property);
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

    const renderPropertyEditor = (property: HeProperty) => {
      console.log("Display Manager", this.displayManager);
      var content = displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange);
      return content;
    }

    return (
      this.questionProperties.map(renderPropertyEditor)
    )
    
  }
}
