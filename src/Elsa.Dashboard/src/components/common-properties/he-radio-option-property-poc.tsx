import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
  ActivityModel,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../models/elsa-interfaces";
import { ToLetter,Map } from "../../utils/utils";
import { IconProvider } from "../providers/icon-provider/icon-provider";
import PlusIcon from '../../icons/plus_icon';
import { PropertyOutputTypes, RadioOptionsSyntax, SyntaxNames } from '../../constants/constants';
import { HeActivityPropertyDescriptor, NestedActivityDefinitionProperty, NestedActivityDefinitionPropertyAlt } from '../../models/custom-component-models';
import TrashCanIcon from '../../icons/trash-can';
import { BaseComponent, ISharedComponent } from '../base-component';

@Component({
  tag: 'he-radio-options-property-poc',
  shadow: false,
})

export class HeRadioOptionPropertyPOC implements ISharedComponent {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: HeActivityPropertyDescriptor;
  @Prop() propertyModel: NestedActivityDefinitionProperty;
  @Prop() modelSyntax: string = SyntaxNames.Json;
  @State() properties: Array<NestedActivityDefinitionPropertyAlt> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;
  @State() optionsDisplayToggle: Map<string> = {};
  @State() switchTextHeight: string = "";
  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  private _base: BaseComponent;

  constructor() {
    this._base = new BaseComponent(this);
  }

  async componentWillLoad() {
    this._base.componentWillLoad();
  }

  updatePropertyModel() {
    this._base.updatePropertyModel();
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
  }

  onAddOptionClick() {
    console.log("Adding Radio");
    const optionName = ToLetter(this.properties.length + 1);
    const newAnswer: NestedActivityDefinitionPropertyAlt = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: {
        [SyntaxNames.Literal]: '',
        [RadioOptionsSyntax.PrePopulated]: 'false'
      }, type: PropertyOutputTypes.Radio,
      propertyDescriptors: this.getPropertyDescriptors()
    };

    this.properties = [...this.properties, newAnswer];
    console.log("Answers", this.properties);
    this.updatePropertyModel();
  }

  getPropertyDescriptors(): Array<HeActivityPropertyDescriptor> {
    return this.propertyDescriptor.nestedProperties[0].nestedProperties;
  }

  onDeleteOptionClick(property: NestedActivityDefinitionPropertyAlt) {
    this.properties = this.properties.filter(x => x != property);
    this.updatePropertyModel();
  }

  //onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
  //  const json = e.detail;
  //  const parsed = parseJson(json);

  //  if (!parsed)
  //    return;

  //  if (!Array.isArray(parsed))
  //    return;

  //  this.propertyModel.expressions[SyntaxNames.Json] = json;
  //  this.answers = parsed;
  //}

  updateNestedExpressions(parsed: Array<NestedActivityDefinitionPropertyAlt>) {
    this.properties = parsed
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  onNestedPropertyChange(e: CustomEvent<string>, property: NestedActivityDefinitionPropertyAlt) {
    e = e;
    property = property;
    //console.log("Updating property", property);
    //console.log("Update Radio Event", e);
    //let eventProperty = JSON.parse(e.detail);
    //console.log("Event property", eventProperty);
    //let filteredProperties = this.properties.filter(x => x.value.name != property.name)
    //let propertyToUpdate = this.properties.filter(x => x.value.name == property.name)[0];
    //propertyToUpdate.value = eventProperty;
    //this.properties = [...filteredProperties, eventProperty]
    //console.log("Properties after update", this.properties);
    ////this.propertyModel.expressions[property.value.syntax] = e.detail;
    this.updatePropertyModel();
  }

  render() {
    const answers = this.properties;
    const json = JSON.stringify(answers, null, 2);



    const renderCaseEditor = (property: NestedActivityDefinitionPropertyAlt) => {
      let name = property.name;
      return (
        <div>
                    <div class="elsa-mb-1">
            <div class="elsa-flex">
              <div class="elsa-flex-1 elsa-mx-auto">
                <h3>Answer: { name }</h3>
              </div>
              <div>
                <button type="button" onClick={() => this.onDeleteOptionClick(property)}
                  class="elsa-h-5 elsa-w-5 elsa-mx-auto elsa-outline-none focus:elsa-outline-none">
                  <TrashCanIcon options={this.iconProvider.getOptions()}></TrashCanIcon>
                </button>
              </div>
            </div>
          </div>

          <nested-property-list
            activityModel={this.activityModel}
            propertyModel={property}
            nestedDescriptors={property.propertyDescriptors}
            onExpressionChanged={e => this.onNestedPropertyChange(e, property)}
          >
          </nested-property-list>
          </div>

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
          onExpressionChanged={e => this._base.OnMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >
          <div class="elsa-min-w-full elsa-divide-y elsa-divide-gray-200">
            {answers.map(renderCaseEditor)}
          </div>
        
          <button type="button" onClick={() => this.onAddOptionClick()}
            class="elsa-inline-flex elsa-items-center elsa-px-4 elsa-py-2 elsa-border elsa-border-transparent elsa-shadow-sm elsa-text-sm elsa-font-medium elsa-rounded-md elsa-text-white elsa-bg-blue-600 hover:elsa-bg-blue-700 focus:elsa-outline-none focus:elsa-ring-2 focus:elsa-ring-offset-2 focus:elsa-ring-blue-500 elsa-mt-2">
            <PlusIcon options={this.iconProvider.getOptions()}></PlusIcon>
            Add Answer
          </button>
        </elsa-multi-expression-editor>
      </div>
    );
  }
}
