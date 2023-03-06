import { Component, Event, EventEmitter, h, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaExpressionEditorElement,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext,
} from "../../models/elsa-interfaces";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import { INestedTextActivity } from "../../models/custom-component-models";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import ExpandIcon from '../../icons/expand_icon';
import { mapSyntaxToLanguage, parseJson, ToLetter, Map } from '../../utils/utils';
import { PropertyOutputTypes, SyntaxNames } from '../../constants/constants';

@Component({
  tag: 'he-text-activity-property',
  shadow: false,
})
export class TextActivityProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @State() text: Array<INestedTextActivity> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;

  supportedSyntaxes: Array<string> = [SyntaxNames.Literal, SyntaxNames.JavaScript];
  @State() conditionDisplayHeightMap: Map<string> = {};

  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel.expressions[SyntaxNames.ConditionalTextList];
    this.text = parseJson(casesJson) || [];
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.ConditionalTextList] = JSON.stringify(this.text);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.text, null, 2);
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }
  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.text = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    console.log(e);
    this.syntaxSwitchCount++;
  }

  onAddElementClick() {
    const textName = ToLetter(this.text.length + 1);
    const newTextElement: INestedTextActivity = {
      syntax: SyntaxNames.Literal,
      isParagraph: false,
      expressions: { [SyntaxNames.Literal]: '' },
      condition: { syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: 'true' } },
      type: PropertyOutputTypes.TextActivity,
      name: textName
    };
    this.text = [...this.text, newTextElement];
    this.updatePropertyModel();
  }

  onHandleDelete(textActivity: INestedTextActivity) {
    this.text = this.text.filter(x => x != textActivity);
    this.updatePropertyModel();
  }

  onTextChanged(e: CustomEvent<string>, textActivity: INestedTextActivity) {
    textActivity.expressions[textActivity.syntax] = e.detail;
    this.updatePropertyModel();

  }

  onConditionChanged(e: CustomEvent<string>, textActivity: INestedTextActivity) {
    textActivity.condition.expressions[textActivity.condition.syntax] = e.detail;
    this.updatePropertyModel();
  }

  onTextSyntaxChanged(e: Event, textActivity: INestedTextActivity, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    textActivity.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(textActivity.syntax);
    this.updatePropertyModel();
  }

  onConditionSyntaxChanged(e: Event, textActivity: INestedTextActivity, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    textActivity.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(textActivity.syntax);
    this.updatePropertyModel();
  }

  onExpandConditionArea(index: number) {
    let tempValue = Object.assign(this.conditionDisplayHeightMap);
    let height = this.conditionDisplayHeightMap[index];
    if (height == null) {
      tempValue[index] = "6em";
    } else {
      this.conditionDisplayHeightMap[index] == "2.75em" ? tempValue[index] = "6em" : tempValue[index] = "2.75em";
    }
    this.conditionDisplayHeightMap = { ... this.conditionDisplayHeightMap, tempValue }
  }

  //getEditorHeight(options: any) {
  //  const editorHeightName = options.editorHeight || 'Default';

  //  switch (editorHeightName) {
  //    case 'Large':
  //      return { propertyEditor: '30em', textArea: 6 }
  //    case 'Default':
  //    default:
  //      return { propertyEditor: '25em', textArea: 3 }
  //  }
  //}

  render() {
    const textElements = this.text;
    const json = JSON.stringify(textElements, null, 2);
    const renderCaseEditor = (nestedTextActivity: INestedTextActivity, index: number) => {  

      const textSyntax = nestedTextActivity.syntax;
      const conditionSyntax = nestedTextActivity.condition.syntax;
      const textExpression = nestedTextActivity.expressions[textSyntax];
      const conditionExpression = nestedTextActivity.condition.expressions[conditionSyntax];

      const textLanguage = mapSyntaxToLanguage(textSyntax);
      const conditionLanguage = mapSyntaxToLanguage(conditionSyntax);

      const conditionEditorHeight = this.conditionDisplayHeightMap[index] ?? "2.75em";

      let textExpressionEditor = null;
      let conditionExpressionEditor = null;


      return (
        <tbody>
          
          <tr>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Text
            </th>
            <td class="elsa-py-2 elsa-pl-5" colSpan={2}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <elsa-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                ref={el => textExpressionEditor = el}
                expression={textExpression}
                language={textLanguage}
                single-line={false}
                editorHeight="2.75em"
                padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                onExpressionChanged={e => this.onTextChanged(e, nestedTextActivity)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onTextSyntaxChanged(e, nestedTextActivity, textExpressionEditor)}
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
            <td class="elsa-py-2 pl-5" colSpan={2}>  
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => conditionExpressionEditor = el}
                  expression={conditionExpression}
                  language={conditionLanguage}
                  single-line={false}
                  editorHeight={conditionEditorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this.onConditionChanged(e, nestedTextActivity)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this.onTextSyntaxChanged(e, nestedTextActivity, conditionExpressionEditor)}
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
              <button type="button" onClick={() => this.onExpandConditionArea(index)}
                      class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
              </button>
            </td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onHandleDelete(nestedTextActivity)}
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
             {textElements.map(renderCaseEditor) }

          </table>
          <button type="button" onClick={() => this.onAddElementClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Paragraph
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
