import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
    ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, newOptionLetter, Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { CheckboxOptionsSyntax, PropertyOutputTypes, SyntaxNames, TextActivityOptionsSyntax, WeightedScoringSyntax } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';

@Component({
  tag: 'he-text-group-property-old',
  shadow: false,
})

export class HeTextGroupPropertyOld implements ISortableSharedComponent, IDisplayToggle {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyModel: NestedActivityDefinitionProperty;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @Prop() keyId: string;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() dictionary: Map<string> = {};
  @State() switchTextHeight: string = "";
  @State() editorHeight: string = "2.75em"
  private _base: SortableComponent;
  private _toggle: DisplayToggle;
  container: HTMLElement;

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  scoreSyntaxSwitchCount: number = 0;
  displayValue: string = "table-row";
  hiddenValue: string = "none";

  constructor() {
    this._base = new SortableComponent(this);
    this._toggle = new DisplayToggle(this);
  }
    

  async componentWillLoad() {
    this._base.componentWillLoad();
  }

  async componentDidLoad() {
    this._base.componentDidLoad();
  }

  async componentWillRender() {
    this._base.componentWillRender();
  }


  updatePropertyModel() {
    this._base.updatePropertyModel();
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
  }

  onAddAnswerClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newAnswer: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [CheckboxOptionsSyntax.PrePopulated]: 'false',
      }, type: PropertyOutputTypes.Checkbox
    };
    this.properties = [...this.properties, newAnswer];
    this.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
    this.updatePropertyModel();
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

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index);
  }

  groupedTextPropertyDescriptor(): ActivityPropertyDescriptor {
    return {
      uiHint: 'multi-text',
      isReadOnly: false,
      name: "Information Text Activity",
      hint: "A group of Text elements that will be displayed together within an assessment screen.",
      label: "Grouped Text",
      supportedSyntaxes: [SyntaxNames.Literal],
      defaultSyntax: SyntaxNames.Literal,
      considerValuesAsOutcomes: false,
      disableWorkflowProviderSelection: true

    } as ActivityPropertyDescriptor;
  }

  multiTextModel() {
    let jsonModel = this.propertyModel.expressions[SyntaxNames.GroupedInformationText];
    let model = jsonModel != null && jsonModel != '' ?
      JSON.parse(jsonModel) as NestedActivityDefinitionProperty :
      null;
    if (model != null && model.expressions != null) {
      return model;
    }
    else {
      const newModel: ActivityDefinitionProperty = {
        name: "MultiTextModel",
        expressions: {
          
        }
      };
      return newModel;
    }
  }

  render() {
    const answers = this.properties;
    const json = JSON.stringify(answers, null, 2);

    const renderCaseEditor = (textGroup: NestedActivityDefinitionProperty, index: number) => {

      const title = textGroup.expressions[TextActivityOptionsSyntax.Title];
      const isCollapsedChecked = textGroup.expressions[TextActivityOptionsSyntax.Collapsed] == 'true';
      const isGuidanceChecked = textGroup.expressions[TextActivityOptionsSyntax.Guidance] == 'true';
      const isBulletsChecked = textGroup.expressions[TextActivityOptionsSyntax.Bulletpoints] == 'true';

      let colWidth = "100%";
      const optionsDisplay = this.dictionary[index] ?? "none";

      return (
        <tbody key={this.keyId}>
          <tr>
            <th class="sortablejs-custom-handle"><SortIcon options={this.iconProvider.getOptions() }></SortIcon></th>
            <td></td>
            <td class="elsa-pt-1 elsa-pr-2 elsa-text-right">
              <button type="button" onClick={() => this.onDeleteOptionClick(textGroup)}
                class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
              </button>
            </td>
          </tr>
          <tr key={`case-${index}`}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Identifier
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <input type="text" value={textGroup.name} onChange={e => this._base.UpdateName(e, textGroup)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
          </tr>

          <tr>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Title
            </th>
            <td class="elsa-py-2" colSpan={2} style={{ width: colWidth }}>
              <input type="text" value={title} onChange={e => this._base.StandardUpdateExpression(e, textGroup, TextActivityOptionsSyntax.Title)}
                class="focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
            </td>
          </tr>

          <tr onClick={() => this.onToggleOptions(index)}>
            <th
              class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-text-left elsa-tracking-wider elsa-w-2/12" colSpan={3} style={{ cursor: "zoom-in" }}> Options
            </th>
            <td></td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is Collapsed</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={isCollapsedChecked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Collapsed)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is Bullet Point List</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={isBulletsChecked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Bulletpoints)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>

          <tr style={{ display: optionsDisplay }} >
            <th class="elsa-py-3 elsa-text-left elsa-text-xs elsa-font-medium elsa-text-gray-500 elsa-tracking-wider elsa-w-2/12">Is Guidance</th>
            <td class="elsa-py-0">
              <input name="choice_input" type="checkbox" checked={isGuidanceChecked} value={'true'}
                onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Guidance)}
                class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
            </td>
            <td></td>
          </tr>
        </tbody>
      );
    };

    const groupName = "Text Group " + this.propertyModel.name;

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: groupName
    };

    let multiTextDescriptor = this.groupedTextPropertyDescriptor();
    let multiTextModel = this.multiTextModel();


    return (
      <div>
        <br />
        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Group Name</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.name} onChange={e => this._base.UpdateName(e, this.propertyModel)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The name of the text group.  Each group name must be unique.</p>
        </div>

        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Is Guidance Text?</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore]} onChange={e => this._base.StandardUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.MaxGroupScore)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Indicates whether the entire group of text should be displayed as guidance block.</p>
        </div>

        <div class="elsa-mb-1">
          <div class="elsa-flex">
            <div class="elsa-flex-1">
              <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Is Guidance Text?</label>
            </div>
          </div>
        </div>

        <div>
          <div>
            <input type="text" value={this.propertyModel.expressions[WeightedScoringSyntax.MaxGroupScore]} onChange={e => this._base.StandardUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.MaxGroupScore)}
              class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
          </div>
          <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Indicates whether the entire group of text should be displayed as guidance block.</p>
        </div>

        <div>
            <he-multi-text-property
            activityModel={this.activityModel}
            propertyModel={multiTextModel}
            onExpressionChanged={e => this._base.CustomUpdateExpression(e, this.propertyModel, WeightedScoringSyntax.GroupArrayScore)}
            propertyDescriptor={ multiTextDescriptor }
            >
            </he-multi-text-property>
        </div>

        <br />

        <he-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={groupName}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >
          <table class="elsa-min-w-full elsa-divide-y elsa-divide-gray-600" ref={el => (this.container = el as HTMLElement)}>
            {answers.map(renderCaseEditor)}
          </table>

          <button type="button" onClick={() => this.onAddAnswerClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </he-multi-expression-editor>
      </div>
    );
  }
}
