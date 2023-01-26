import { Component, h, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaExpressionEditorElement,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext,
  SyntaxNames
} from "../../models/elsa-interfaces";
import { parseJson } from "../../models/utils";
import { IOutcomeProperty } from "../../models/custom-component-models";
import { IconProvider } from "../icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';

import { mapSyntaxToLanguage } from '../../models/utils';


@Component({
  tag: 'custom-outcome-list-property',
  shadow: false,
})
export class CustomOutcomeListProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() outcomes: Array<IOutcomeProperty> = [];
  @State() iconProvider = new IconProvider();

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel.expressions['Switch']
    this.outcomes = parseJson(casesJson) || [];
    console.log("Outcomes", this.outcomes);
  }

  updatePropertyModel() {
    this.propertyModel.expressions['Switch'] = JSON.stringify(this.outcomes);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.outcomes, null, 2);
  }
  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions['Switch'] = json;
    this.outcomes = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    console.log(e);
    this.syntaxSwitchCount++;
  }

  onAddOutcomeClick() {
    const newCase = { text: { syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: '' } }, condition: { syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: '' } }  };
    this.outcomes = [...this.outcomes, newCase];
    this.updatePropertyModel();
  }

  onHandleDelete(outcome: IOutcomeProperty) {
    this.outcomes = this.outcomes.filter(x => x != outcome);
    this.updatePropertyModel();
  }

  onTextChanged(e: CustomEvent<string>, outcome: IOutcomeProperty) {
    this.outcomes = this.outcomes.filter(x => x != outcome);
    outcome.text.expressions[outcome.text.syntax] = e.detail;
    this.outcomes = [...this.outcomes, outcome];
    this.updatePropertyModel();

  }

  onConditionChanged(e: CustomEvent<string>, outcome: IOutcomeProperty) {
    this.outcomes = this.outcomes.filter(x => x != outcome);
    outcome.condition.expressions[outcome.condition.syntax] = e.detail;
    this.outcomes = [...this.outcomes, outcome];
    this.updatePropertyModel();
  }

  onTextSyntaxChanged(e: Event, outcome: IOutcomeProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    outcome.text.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(outcome.text.syntax);
    this.updatePropertyModel();
  }

  onConditionSyntaxChanged(e: Event, outcome: IOutcomeProperty, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    outcome.text.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(outcome.text.syntax);
    this.updatePropertyModel();
  }

  getEditorHeight(options: any) {
    const editorHeightName = options.editorHeight || 'Default';

    switch (editorHeightName) {
      case 'Large':
        return { propertyEditor: '30em', textArea: 6 }
      case 'Default':
      default:
        return { propertyEditor: '25em', textArea: 3 }
    }
  }

  render() {
    const outcomes = this.outcomes;
/*    const supportedSyntaxes = this.supportedSyntaxes;*/
    const json = JSON.stringify(outcomes, null, 2);

    const renderCaseEditor = (outcome: IOutcomeProperty, index: number) => {  

      const textSyntax = outcome.text.syntax;
      const conditionSyntax = outcome.condition.syntax;
      const textExpression = outcome.text.expressions[textSyntax];
      const conditionExpression = outcome.condition.expressions[conditionSyntax];

      const textLanguage = mapSyntaxToLanguage(textSyntax);
      const conditionLanguage = mapSyntaxToLanguage(conditionSyntax);

      let textExpressionEditor = null;
      let conditionExpressionEditor = null;


      return (
        <tbody>
          
          <tr>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Text
            </th>
            <td class="elsa-py-2 elsa-pl-5">
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <elsa-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                ref={el => textExpressionEditor = el}
                expression={textExpression}
                language={textLanguage}
                single-line={false}
                editorHeight="6em"
                padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                onExpressionChanged={e => this.onTextChanged(e, outcome)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onTextSyntaxChanged(e, outcome, textExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {this.supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == textSyntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
          </tr>
          <tr>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Condition
            </th>
            <td class="elsa-py-2 pl-5">  
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => conditionExpressionEditor = el}
                  expression={conditionExpression}
                  language={conditionLanguage}
                  single-line={false}
                  editorHeight="6em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this.onConditionChanged(e, outcome)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onTextSyntaxChanged(e, outcome, conditionExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {this.supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == conditionSyntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
          </tr>
          <tr>

            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-right elsa-tracking-wider elsa-w-1/12">&nbsp;
            </th>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onHandleDelete(outcome)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>
          </tbody>
      );
    };

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };

    return (
      <div>

        <elsa-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={this.propertyDescriptor.label}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >

          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200 elsa-table-striped">
             {outcomes.map(renderCaseEditor) }

          </table>
          <button type="button" onClick={() => this.onAddOutcomeClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Paragraph
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
