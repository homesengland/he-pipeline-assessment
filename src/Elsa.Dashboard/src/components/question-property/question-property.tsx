import { Component, EventEmitter, Event, Prop, State, h } from '@stencil/core';
import { DataDictionaryGroup } from '../../models/custom-component-models';
import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
} from '../../models/elsa-interfaces';

import {
  NestedProperty,
  NestedPropertyModel,
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../providers/icon-provider/icon-provider'
import { HePropertyDisplayManager } from '../../nested-drivers/display-managers/display-manager';
import { parseJson, updateNestedActivitiesByDescriptors, filterPropertiesByType, createQuestionProperty } from '../../utils/utils';
import { SyntaxNames } from '../../constants/constants';

@Component({
  tag: 'question-property',
  shadow: false,
})

export class QuestionProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() questionModel: NestedPropertyModel;
  @Prop() dataDictionaryGroup: Array<DataDictionaryGroup> = [];
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

  async componentWillRender() {
    this.getOrCreateQuestionProperties();
  }

  getOrCreateQuestionProperties() {
    const model = this.questionModel;
    this.questionModel.descriptor = filterPropertiesByType(this.questionModel.descriptor, this.questionModel.ActivityType.nameConstant)
    const propertyJson = model.value.expressions[SyntaxNames.QuestionList];
    if (propertyJson != null && propertyJson != undefined && parseJson(propertyJson).length > 0) {
      const nestedProperties = parseJson(propertyJson)
      this.nestedQuestionProperties = updateNestedActivitiesByDescriptors(this.questionModel.descriptor, nestedProperties, this.questionModel);
    }
    else {
      this.nestedQuestionProperties = this.createQuestionProperties();
      console.log(this.nestedQuestionProperties)
    }
  }

  createQuestionProperties(): Array<NestedProperty> {
    let propertyArray: Array<NestedProperty> = [];
    const descriptor = this.questionModel.descriptor;
    descriptor.forEach(d => {
      var prop = createQuestionProperty(d, this.questionModel);
      propertyArray.push(prop);
    });
    return propertyArray;
  }

  onPropertyExpressionChange(event: Event, property: NestedProperty) {
    event = event;
    property = property;
    this.updateQuestionModel();
  }

  updateQuestionModel() {
    let nestedQuestionsJson = JSON.stringify(this.nestedQuestionProperties);
    this.questionModel.value.expressions[SyntaxNames.QuestionList] = nestedQuestionsJson;
    this.updateQuestionScreen.emit(JSON.stringify(this.questionModel));
  }


  render() {
    console.log("Re-rendering question", this.questionModel);
    const displayManager = this.displayManager;

    const renderPropertyEditor = (property: NestedProperty) => {
      console.log(property.value)
      var content = displayManager.displayNested(this.activityModel, property, this.onPropertyExpressionChange.bind(this));
      let id = property.descriptor.name + "Category";
      return (
        <he-elsa-control id={id} content={content} class="sm:elsa-col-span-6 hydrated"/>
        );
    }

    return (
      <div class="elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6">

        {this.nestedQuestionProperties.map(renderPropertyEditor)}

      </div>
      
      
    )
    
  }
}
