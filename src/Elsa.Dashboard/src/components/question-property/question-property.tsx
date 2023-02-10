import { Component, Prop, State } from '@stencil/core';

import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
  SyntaxNames,
} from '../../models/elsa-interfaces';

import {
    HeProperty,
  QuestionProperty
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { QuestionEventHandler } from '../../events/component-events';
import { HePropertyDisplayManager } from '../../models/display-manager';
import { parseJson } from '../../models/utils';

@Component({
  tag: 'question-property-v2',
  shadow: false,
})

export class HEQuestionComponent {

  @Prop() activityModel: ActivityModel
  @Prop() question: QuestionProperty
  @State() iconProvider = new IconProvider();
  @State() currentValue: string;
  @State() questionProperties: Array<HeProperty>

  handler: QuestionEventHandler;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;
  displayManager: HePropertyDisplayManager = new HePropertyDisplayManager();

  async componentWillLoad() {
    const model = this.question;
    const propertyJson = model.value.expressions[SyntaxNames.Json]
    this.questionProperties = parseJson(propertyJson) || []
  }

  onPropertyExpressionChange(event: Event, property: HeProperty) {
    console.log(event);
    console.log(property);
  }


  renderPropertyEditor(property: HeProperty) {
    return this.displayManager.display(this.activityModel, property.descriptor, this.onPropertyExpressionChange, property)
  }

  render() {
    return (
      this.questionProperties.map(this.renderPropertyEditor)
    )
    
  }
}
