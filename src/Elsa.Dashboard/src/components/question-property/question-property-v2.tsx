import { Component, Prop, State } from '@stencil/core';

import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
} from '../../models/elsa-interfaces';

import {
  NestedProperty,
  QuestionModel
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { QuestionEventHandler } from '../../events/component-events';
import { HePropertyDisplayManager } from '../../models/display-manager';
import { parseJson } from '../../models/utils';
import { SyntaxNames } from '../../constants/Constants';

@Component({
  tag: 'question-property-v2',
  shadow: false,
})

export class HEQuestionComponent {

  @Prop() activityModel: ActivityModel;
  @Prop() questionModel: QuestionModel;
  @State() iconProvider = new IconProvider();
  @State() currentValue: string;
  @State() questionProperties: Array<NestedProperty>;

  handler: QuestionEventHandler;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;
  displayManager: HePropertyDisplayManager = new HePropertyDisplayManager();

  async componentWillLoad() {
    const model = this.questionModel;
    const propertyJson = model.value.expressions[SyntaxNames.Json]
    this.questionProperties = parseJson(propertyJson) || []
  }

  onPropertyExpressionChange(event: Event, property: NestedProperty) {
    event = event;
    property = property;
  }

  render() {
    const displayManager = this.displayManager

    const renderPropertyEditor = (property: NestedProperty) => {
      return displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange)
    }


    return (
      this.questionProperties.map(renderPropertyEditor)
    )
    
  }
}
