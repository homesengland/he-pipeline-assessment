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
import { PropertyOutputTypes, SyntaxNames, TextActivityOptionsSyntax } from '../../constants/constants';
import { NestedActivityDefinitionProperty } from '../../models/custom-component-models';
import { SortableComponent, ISortableSharedComponent } from '../base-component';
import SortIcon from '../../icons/sort_icon';
import { DisplayToggle, IDisplayToggle } from '../display-toggle-component';
import MaximiseIcon from '../../icons/maximise_icon';
import MinimiseIcon from '../../icons/minimise_icon';

@Component({
  tag: 'he-text-group-property',
  shadow: false,
})

export class HeTextGroupProperty implements ISortableSharedComponent, IDisplayToggle {

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
  groupTextButton: string = "Add Text Group";

  private _base: SortableComponent;
  private _toggle: DisplayToggle;

  constructor() {
    this._base = new SortableComponent(this);
    this._toggle = new DisplayToggle(this);
  }

  async componentWillLoad() {
    //TODO - a little messy and we probably want to look to inject options into this
    //but due to time constraints this should work as an initial check.
    if (this.propertyDescriptor != null && this.propertyDescriptor.name != null) {
      this.groupTextButton = this.propertyDescriptor.name.toLowerCase().includes('guidance')
        ? "Add Guidance" : this.groupTextButton;
    }
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
    console.log("Property Model on save:", JSON.stringify(this.propertyModel));
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
        [SyntaxNames.GroupedInformationText]: ''
      }, type: PropertyOutputTypes.InformationGroup
    };
    this.properties = [... this.properties, newGroup];
    this.updatePropertyModel();
  }

  onDeleteGroupClick(informationGroup: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != informationGroup);
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

  informationTextDescriptor(): ActivityPropertyDescriptor {
    return {
      uiHint: 'he-text-activity-property',
      isReadOnly: false,
      name: "Information text",
      hint: "",
      label: "Information Text",
      supportedSyntaxes: [SyntaxNames.GroupedInformationText, SyntaxNames.Json],
      defaultSyntax: SyntaxNames.GroupedInformationText,
      considerValuesAsOutcomes: false,
      disableWorkflowProviderSelection: true

    } as ActivityPropertyDescriptor;
  }

  render() {
    const textGroups = this.properties;
    const json = JSON.stringify(textGroups, null, 2);

    const renderCheckboxGroups = (textGroup: NestedActivityDefinitionProperty) => {

      const descriptor = this.informationTextDescriptor();

      const title = textGroup.expressions[TextActivityOptionsSyntax.Title];
      const isCollapsedChecked = textGroup.expressions[TextActivityOptionsSyntax.Collapsed] == 'true';
      const isGuidanceChecked = textGroup.expressions[TextActivityOptionsSyntax.Guidance] == 'true';
      const isBulletsChecked = textGroup.expressions[TextActivityOptionsSyntax.Bulletpoints] == 'true';

      const eventHandler = this.onPropertyExpressionChange.bind(this);
      const groupKey = "group_" + textGroup.name;
      const isMinimised = this.dictionary[groupKey] != null && this.dictionary[groupKey] == this.displayValue;

      let minimiseIconStyle = isMinimised ? this.hiddenValue : this.displayValue;
      let maximiseIconStyle = !isMinimised ? this.hiddenValue : this.displayValue;
      let displayGroupStyle = isMinimised ? this.hiddenValue : "";


      return (
        <div class="elsa-border-gray-300 elsa-rounded-md elsa-group-border" key={this.keyId}>
          <br />
          <div class="elsa-mb-1">
            <div class="elsa-flex">
              <div class="elsa-flex-1 sortablejs-custom-handle">
                <SortIcon options={this.iconProvider.getOptions()}></SortIcon>
              </div>
              <div class="elsa-flex-1 elsa-text-left elsa-mx-auto">
                <h2 class="inline">Group: {textGroup.name}</h2>
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
                <button type="button" onClick={() => this.onDeleteGroupClick(textGroup)}
                  class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                  <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
                </button>
              </div>
            </div>
            <br/>

            <div style={{ display: displayGroupStyle }}>

            <div class="elsa-mb-1">
              <div class="elsa-flex">
                <div class="elsa-flex-1">
                  <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Title</label>
                </div>
              </div>
            </div>

            <div>
              <div>
                <input type="text" value={title} onChange={e => this._base.StandardUpdateExpression(e, textGroup, TextActivityOptionsSyntax.Title)}
                  class="focus:elsa-ring-blue-500 focus:elsa-border-bue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300" />
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">The title of the text grouping.  This can be left empty, but if a value is given, it will display as a heading.</p>
            </div>
            <br/>
            <div class="elsa-mb-1">
              <div class="elsa-flex">
                <div class="elsa-flex-1">
                  <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Is Guidance</label>
                </div>
              </div>
            </div>

            <div>
              <div>
                <input type="checkbox" checked={isGuidanceChecked} onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Guidance)}
                  class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Should the group of text be wrapped in a guidance block.</p>
            </div>

            <br />
            <div class="elsa-mb-1">
              <div class="elsa-flex">
                <div class="elsa-flex-1">
                  <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Is Collapsed</label>
                </div>
              </div>
            </div>

            <div>
              <div>
                <input type="checkbox" checked={isCollapsedChecked} onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Collapsed)}
                  class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Should the group of text be hidden by default, behind a collapseable element.
                Users will still be able to see the text by clicking to view the hidden text.</p>
            </div>

            <br />
            <div class="elsa-mb-1">
              <div class="elsa-flex">
                <div class="elsa-flex-1">
                  <label class="elsa-block elsa-text-sm elsa-font-medium elsa-text-gray-700">Is Bulletpointed</label>
                </div>
              </div>
            </div>

            <div>
              <div>
                <input type="checkbox" checked={isBulletsChecked} onChange={e => this._base.UpdateCheckbox(e, textGroup, TextActivityOptionsSyntax.Bulletpoints)}
                  class="focus:elsa-ring-blue-500 elsa-h-8 elsa-w-8 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              </div>
              <p class="elsa-mt-2 elsa-text-sm elsa-text-gray-500">Each paragraph of text will be displayed as a bullet-point list within this group.</p>
            </div>
            </div>
          </div>


          <he-text-activity-property
            activityModel={this.activityModel}
            propertyModel={textGroup}
            propertyDescriptor={descriptor}
            onExpressionChanged={e => eventHandler(e, textGroup)}
            modelSyntax={SyntaxNames.TextActivity}
            style={{ display: displayGroupStyle }}>
          </he-text-activity-property>
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
          hint={this.propertyDescriptor.hint}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >
          <hr />
          <div class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200" ref={el => (this.container = el as HTMLElement)}>
            {textGroups.map(renderCheckboxGroups)}
          </div>

          <button type="button" onClick={() => this.onAddGroupClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Text Group
          </button>
        </he-multi-expression-editor>
      </div>
    );
  }
}
