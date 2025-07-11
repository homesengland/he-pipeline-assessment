import { Component, EventEmitter, Event, h, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { HeActivityPropertyDescriptor, NestedActivityDefinitionProperty, NestedActivityDefinitionPropertyAlt, NestedProperty } from '../../../models/custom-component-models';
import { ActivityModel } from '../../../models/elsa-interfaces';
import { HePropertyDisplayManager } from '../../../nested-drivers/display-managers/display-manager';

@Component({
  tag: 'nested-property-list',
  shadow: false,
})
export class NestedPropertyList {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyModel: NestedActivityDefinitionProperty;
  @State() properties: Array<NestedProperty>;
  @Prop() nestedDescriptors: Array<HeActivityPropertyDescriptor>;
  @Event() expressionChanged: EventEmitter<string>;
  @State() displayManager: HePropertyDisplayManager = new HePropertyDisplayManager();
  @State() modelSyntax: string = SyntaxNames.Json;


  constructor() {
    this.modelSyntax = this.propertyModel.syntax ?? SyntaxNames.Json;
  }

  async componentWillLoad() {
    this.nestedDescriptors.length > 0;
    let propertyJson = this.propertyModel.expressions[this.modelSyntax];
    if (propertyJson != null && propertyJson != '') {
      this.properties = JSON.parse(propertyJson);
    }
    else {
      this.initPropertyModel();
    }
  }

  initPropertyModel() {
    var properties = this.nestedDescriptors.map(x => this.getOrCreateNestedProperty(x));
    this.properties = properties;
  }

  getOrCreateNestedProperty(descriptor: HeActivityPropertyDescriptor): NestedProperty {
    var nestedProperty: NestedProperty = {
      descriptor: descriptor,
      value: this.getOrCreateActivityDefinitionProperty(descriptor)
    }
    return nestedProperty;
  }

  getOrCreateActivityDefinitionProperty(descriptor: HeActivityPropertyDescriptor): NestedActivityDefinitionPropertyAlt {
    return {
      name: descriptor.name,
      syntax: descriptor.defaultSyntax,
      expressions: {
        [descriptor.defaultSyntax]: '',
      },
      type: descriptor.expectedOutputType
    } as NestedActivityDefinitionPropertyAlt;
  }

  updateQuestionModel() {
    this.propertyModel.expressions[this.modelSyntax] = JSON.stringify(this.properties);
/*    this.expressionChanged.emit(this.propertyModel.expressions[this.propertyModel.syntax])*/
  }

  onPropertyChange(event: CustomEvent<string>, property: NestedProperty) {
    event = event;
    property = property;
    let eventProperty = JSON.parse(event.detail);
    let filteredProperties = this.properties.filter(x => x.value.name != eventProperty.name);
    let propertyToUpdate = this.properties.filter(x => x.value.name == eventProperty.name)[0];
    propertyToUpdate.value = eventProperty;
    this.properties = [...filteredProperties, propertyToUpdate];
    this.updateQuestionModel();
  }


  render() {

    const displayManager = this.displayManager;

    const renderPropertyEditor = (property: NestedProperty) => {

      let content = displayManager.displayNested(this.activityModel, property, this.onPropertyChange.bind(this));
      let id = this.propertyModel.name+'_'+property.descriptor.name + "Category";
      return (
        <elsa-control id={id} class="sm:elsa-col-span-6 hydrated">
          {content}
          <br/>
        </elsa-control>
      );
    }

    return (
      <div class="elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6">

        {this.properties.map(renderPropertyEditor)}

      </div>
    )
  };
}
