import { Component, Prop, State } from '@stencil/core';

import {
    ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
  ActivityPropertyDescriptor,
  SyntaxNames,
  ActivityDefinitionProperty
} from '../../models/elsa-interfaces';

import {
  IQuestionComponent
} from '../../models/custom-component-models';

import {
  IconProvider,
} from '../icon-provider/icon-provider'
import { QuestionEventHandler } from '../../events/component-events';

@Component({
  tag: 'question-property-v2',
  shadow: false,
})

export class ElsaQuestionComponent {

  @Prop() question: IQuestionComponent
  @Prop() Type: string
  @Prop() Properties: Array<ActivityPropertyDescriptor>
  @Prop() ActivityModel: ActivityModel
  @State() iconProvider = new IconProvider();
  @State() currentValue: string;

  handler: QuestionEventHandler;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxMultiChoiceCount: number = 0;

  sampleDescriptor() {
    const test: ActivityPropertyDescriptor = {
        name: "test",
        uiHint: "",
        label: "a test label",
        hint: "hint about a test label",
        options: "",
      defaultValue: "",
      defaultSyntax: SyntaxNames.Literal,
      supportedSyntaxes: [SyntaxNames.JavaScript],
        isReadOnly: false,
        considerValuesAsOutcomes: false,
        disableWorkflowProviderSelection: false
    };
        return test
  }

  sampleModel(name: string, syntax: string, modelValue: string) {
    let value = {};
    if (syntax == 'Literal') {
      value = { syntax, modelValue }
    }
    else {
      value = { "Literal": '', syntax: modelValue }
    }

    const test: ActivityDefinitionProperty = {
      name: name,
      expressions: value
    }
    return test;
  }

  //renderQuestionField(fieldId, fieldName, fieldValue, onChangedFunction, isDisabled = false) {
  //  return <div>
  //    <div class="elsa-mb-1">
  //      <div class="elsa-flex">
  //        <div class="elsa-flex-1">
  //          <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">
  //            {fieldName}
  //          </label>
  //        </div>
  //      </div>
  //    </div>
  //    <input type="text" id={fieldId} name={fieldId} disabled={isDisabled} value={fieldValue} onChange={e => {
  //      onChangedFunction.bind(this)(e);
  //    }
  //    }
  //      class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
  //  </div>;
  //}

  //renderCheckboxField(fieldId, fieldName, isChecked, onChangedFunction) {
  //  return <div>
  //    <div class="elsa-mb-1 elsa-mt-2">
  //      <div class="elsa-flex">
  //        <div>
  //          <label htmlFor={fieldId} class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700 elsa-p-1">
  //            {fieldName}
  //          </label>
  //        </div>
  //        <div>
  //          <input id={fieldId} name={fieldId} type="checkbox" checked={isChecked} value={'true'} onChange={e =>
  //            onChangedFunction.bind(this)(e)}
  //            class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
  //        </div>
  //      </div>
  //    </div>
  //  </div>;
  //}



  renderPropertyEditor(property: ActivityPropertyDescriptor) {
    console.log(property)
  }

  render() {
    return (
      this.Properties.map(this.renderPropertyEditor)
    )
    
  }
}
