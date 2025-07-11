import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { parseJson, newOptionLetter, Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import TrashCanIcon from '../../icons/trash-can';
import { PropertyOutputTypes, SyntaxNames, WeightedScoringSyntax } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';
import MaximiseIcon from '../../icons/maximise_icon';
import MinimiseIcon from '../../icons/minimise_icon';

@Component({
  tag: 'he-weighted-checkbox-property',
  shadow: false,
})

export class HeWeightedCheckboxProperty implements ISortableSharedComponent, IDisplayToggle {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @State() keyId: string;
  @State() properties: Array<NestedActivityDefinitionProperty> = [];
  
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() dictionary: Map<string> = {};
  @State() switchTextHeight: string = "";
  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  scoreSyntaxSwitchCount: number = 0;
  container: HTMLElement;
  displayValue: string = "table-row";
  hiddenValue: string = "none";
  private _base: SortableComponent;
  private _toggle: DisplayToggle;

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

  onAddGroupClick() {
    const groupName = newOptionLetter(this._base.IdentifierArray());
    const newGroup: NestedActivityDefinitionProperty = {
      name: groupName,
      syntax: SyntaxNames.Json,
      expressions: {
        [SyntaxNames.Json]: '',
        [WeightedScoringSyntax.GroupArrayScore]: ''
      }, type: PropertyOutputTypes.CheckboxGroup
    };
    this.properties = [... this.properties, newGroup];
    this.updatePropertyModel();
  }

  onDeleteGroupClick(checkboxGroup: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != checkboxGroup);
    this.updatePropertyModel();
  }

  onPropertyExpressionChange(event: Event, property: NestedActivityDefinitionProperty) {
    event = event;
    property = property;
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

  onToggleOptions(index: any) {
    this._toggle.onToggleDisplay(index);
  }

  render() {
    const answerGroups = this.properties;
    const json = JSON.stringify(answerGroups, null, 2);

    const renderCheckboxGroups = (checkboxGroup: NestedActivityDefinitionProperty) => {


      const eventHandler = this.onPropertyExpressionChange.bind(this);
      const groupKey = "group_" + checkboxGroup.name;
      const isMinimised = this.dictionary[groupKey] != null && this.dictionary[groupKey] == this.displayValue;

      let minimiseIconStyle = isMinimised ? this.hiddenValue : this.displayValue;
      let maximiseIconStyle = !isMinimised ? this.hiddenValue : this.displayValue;
      let displayGroupStyle = isMinimised ? this.hiddenValue : "";


      return (
        <div key={this.keyId}>
          <br />
          <div class="elsa-mb-1">
            <div class="elsa-flex">
              <div class="elsa-flex-1 sortablejs-custom-handle">
                <SortIcon options={this.iconProvider.getOptions()}></SortIcon>
              </div>
              <div class="elsa-flex-1 elsa-text-left elsa-mx-auto">
                <h2 class="inline">Group: {checkboxGroup.name}</h2>
                <button type="button" onClick={() => this.onToggleOptions(groupKey)}
                  class="elsa-h-5 inline float-right elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none"
                  style={{ display: minimiseIconStyle }}>
                  <MinimiseIcon options={this.iconProvider.getOptions()}></MinimiseIcon>
              </button>
                <button type="button" onClick={() => this.onToggleOptions(groupKey)}
                  class="elsa-h-5 float-right inline elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none"
                  style={{ display: maximiseIconStyle }}                >
                  <MaximiseIcon options={this.iconProvider.getOptions()}></MaximiseIcon>
                </button>
              </div>
              <div class="px-3 inline"></div>
              <div>
                <button type="button" onClick={() => this.onDeleteGroupClick(checkboxGroup)}
                  class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                  <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
                </button>
              </div>
            </div>
          </div>


          <he-weighted-checkbox-option-group-property
            activityModel={this.activityModel}
            propertyModel={checkboxGroup}
            onExpressionChanged={e => eventHandler(e, checkboxGroup)}
            style={{ display: displayGroupStyle }}>
          </he-weighted-checkbox-option-group-property>
          <br />
          <hr />
        </div>
      )
    }

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };

    return (
      <div>

        <he-multi-expression-editor
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
          <hr />
          <div class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200" ref={el =>(this.container = el as HTMLElement)}>
            {answerGroups.map(renderCheckboxGroups)}
          </div>

          <button type="button" onClick={() => this.onAddGroupClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer Group
          </button>
        </he-multi-expression-editor>
      </div>
    );
  }
}
