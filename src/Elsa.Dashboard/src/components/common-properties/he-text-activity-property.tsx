import { Component, Event, EventEmitter, h, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext,
} from "../../models/elsa-interfaces";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import { NestedActivityDefinitionProperty } from "../../models/custom-component-models";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import ExpandIcon from '../../icons/expand_icon';
import { mapSyntaxToLanguage, parseJson, NewOptionLetter, Map } from '../../utils/utils';
import { PropertyOutputTypes, SyntaxNames, TextActivityOptionsSyntax } from '../../constants/constants';
import { ToggleDictionaryDisplay } from '../../functions/display-toggle';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';

@Component({
  tag: 'he-text-activity-property',
  shadow: false,
})
export class TextActivityProperty implements ISortableSharedComponent {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;

  supportedSyntaxes: Array<string> = [SyntaxNames.Literal, SyntaxNames.JavaScript];

  @State() conditionDisplayHeightMap: Map<string> = {};
  @State() optionsDisplayToggle: Map<string> = {};
  @State() urlDisplayToggle: Map<string> = {};
  private _base: SortableComponent;
  container: HTMLElement;

  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  constructor() {
    this._base = new SortableComponent(this);
  }

  async componentWillLoad() {
    this._base.componentWillLoad();
  }

  async componentDidLoad() {
    this._base.componentDidLoad();
  }

  updatePropertyModel() {
    this._base.updatePropertyModel();
  }
  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onAddElementClick() {
    const textName = NewOptionLetter(this._base.IdentifierArray());
    const newTextElement: NestedActivityDefinitionProperty = {
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [TextActivityOptionsSyntax.Paragraph]: 'true', [TextActivityOptionsSyntax.Condition]: 'true' },
      type: PropertyOutputTypes.Information,
      name: textName
    };
    this.properties = [...this.properties, newTextElement];
    this.updatePropertyModel();
  }

  onHandleDelete(textActivity: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != textActivity);
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

  onToggleOptions(index: number) {
    let tempValue = ToggleDictionaryDisplay(index, this.optionsDisplayToggle)
    this.optionsDisplayToggle = { ... this.optionsDisplayToggle, tempValue }
  }

  onDisplayUrl(index: number) {
    let tempValue = ToggleDictionaryDisplay(index, this.urlDisplayToggle)
    this.urlDisplayToggle = { ... this.urlDisplayToggle, tempValue }
  }

  render() {
    const textElements = this.properties;
    const json = JSON.stringify(textElements, null, 2);
    const renderCaseEditor = (nestedTextActivity: NestedActivityDefinitionProperty, index: number) => {  

      const textSyntax = nestedTextActivity.syntax;
      const conditionSyntax = SyntaxNames.JavaScript;
      const textExpression = nestedTextActivity.expressions[textSyntax];
      const conditionExpression = nestedTextActivity.expressions[TextActivityOptionsSyntax.Condition];
      const urlExpression = nestedTextActivity.expressions[TextActivityOptionsSyntax.Url] ?? "https://www";
      const paragraphChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Paragraph] == 'true';
      const guidanceChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Guidance] == 'true';
      const hyperlinkChecked = nestedTextActivity.expressions[TextActivityOptionsSyntax.Hyperlink] == 'true';

      const textLanguage = mapSyntaxToLanguage(textSyntax);
      const conditionLanguage = mapSyntaxToLanguage(conditionSyntax);
      const urlLanguage = mapSyntaxToLanguage(SyntaxNames.Literal)

      const conditionEditorHeight = this.conditionDisplayHeightMap[index] ?? "2.75em";
      if (this.optionsDisplayToggle[index] == null && (guidanceChecked || hyperlinkChecked)) {
        this.optionsDisplayToggle[index] = "table-row";
        if (this.urlDisplayToggle[index] == null && hyperlinkChecked) {
          this.urlDisplayToggle[index] = "table-row";
        }
      }

      const optionsDisplay = this.optionsDisplayToggle[index] ?? "none";
      const urlDisplay =
        this.urlDisplayToggle[index] != null && this.optionsDisplayToggle[index] != null && this.optionsDisplayToggle[index] != "none"
          ? this.urlDisplayToggle[index]
          : "none";

      let textExpressionEditor = null;
      let conditionExpressionEditor = null;

      let colWidth = "100%";


      return (
        <tbody>
          <tr>
            <th class="sortablejs-custom-handle"><SortIcon options={this.iconProvider.getOptions()}></SortIcon>
            </th>
            <td></td>
            <td></td>
          </tr>
          <tr>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Text
            </th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
              <elsa-expression-editor
                key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                ref={el => textExpressionEditor = el}
                expression={textExpression}
                language={textLanguage}
                single-line={false}
                editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, nestedTextActivity, nestedTextActivity.syntax)}
                />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateSyntax(e, nestedTextActivity, textExpressionEditor)}
                    class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                    {this.supportedSyntaxes.map(supportedSyntax => {
                      const selected = supportedSyntax == textSyntax;
                      return <option selected={selected}>{supportedSyntax}</option>;
                    })}
                  </select>
                </div>
              </div>
            </td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onHandleDelete(nestedTextActivity)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>

          <tr onClick={() => this.onToggleOptions(index)}>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-1/12" colSpan={3} style={{ cursor: "zoom-in" }}> Options
            </th>
          </tr>
          <tr style={{ display: optionsDisplay }} >
              <th
                class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Display on Page
              </th>
              <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>
                <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                  <elsa-expression-editor
                    key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                    ref={el => conditionExpressionEditor = el}
                    expression={conditionExpression}
                    language={conditionLanguage}
                    single-line={false}
                    editorHeight={conditionEditorHeight}
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, nestedTextActivity, TextActivityOptionsSyntax.Condition)}
                  />
                <div class="elsa-absolute elsa-inset-y-0 elsa-right-0 elsa-flex elsa-items-center">
                  <select onChange={e => this._base.UpdateSyntax(e, nestedTextActivity, conditionExpressionEditor)}
                      class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-h-full elsa-py-0 elsa-pl-2 elsa-pr-7 elsa-border-transparent elsa-bg-transparent elsa-text-gray-500 sm:elsa-text-sm elsa-rounded-md">
                      {this.supportedSyntaxes.filter(x => x == SyntaxNames.JavaScript).map(supportedSyntax => {
                        const selected = supportedSyntax == SyntaxNames.JavaScript;
                        return <option selected={selected}>{supportedSyntax}</option>;
                      })}
                    </select>
                  </div>
                </div>
              </td>
              <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
                <button type="button" onClick={() => this.onExpandConditionArea(index)}
                  class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                  <ExpandIcon options={this.iconProvider.getOptions()}></ExpandIcon>
                </button>
              </td>
            </tr>
          <tr style={{ display: optionsDisplay }}>
              <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is <br />Paragraph
              </th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={paragraphChecked} value={nestedTextActivity.expressions[TextActivityOptionsSyntax.Paragraph]}
                onChange={e => this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Paragraph)}
                    class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              </td>
              <td>
              </td>
          </tr>
          <tr style={{ display: optionsDisplay }}>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is <br/>Guidance
            </th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={guidanceChecked} value={nestedTextActivity.expressions[TextActivityOptionsSyntax.Guidance]}
                onChange={e => this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Guidance)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td>
            </td>
          </tr>
          <tr style={{ display: optionsDisplay }}>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is <br/>Hyperlink
            </th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={hyperlinkChecked} value={nestedTextActivity.expressions[TextActivityOptionsSyntax.Hyperlink]}
                onChange={e => [this._base.UpdateCheckbox(e, nestedTextActivity, TextActivityOptionsSyntax.Hyperlink), this.onDisplayUrl(index)]}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td>
            </td>
          </tr>
          <tr style={{ display: urlDisplay }}>
            <th
              class="elsa-px-6 elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Url
            </th>
            <td class="elsa-py-2 pl-5" style={{ width: colWidth }}>

              <div class="elsa-mt-1 elsa-relative elsa-rounded-md elsa-shadow-sm">
                <elsa-expression-editor
                  key={`expression-editor-${index}-${this.syntaxSwitchCount}`}
                  ref={el => conditionExpressionEditor = el}
                  expression={urlExpression}
                  language={urlLanguage}
                  single-line={true}
                  editorHeight="2.75em"
                  padding="elsa-pt-1.5 elsa-pl-1 elsa-pr-28"
                  onExpressionChanged={e => this._base.CustomUpdateExpression(e, nestedTextActivity, TextActivityOptionsSyntax.Url)}
                />
              </div>
            </td>
            <td>
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

          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200 elsa-table-striped" ref={el => (this.container = el as HTMLElement)}>
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
