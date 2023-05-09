import { Component, EventEmitter, Event, h, Prop, State } from '@stencil/core';
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

  async componentWillLoad() {
    console.log("Loading nested property list");
    this.nestedDescriptors.length > 0;
    let propertyJson = this.propertyModel.expressions[this.propertyModel.syntax];
    if (propertyJson != null && propertyJson != '') {
      this.properties = JSON.parse(propertyJson);
      console.log("Properties after parse:", this.properties);
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
    console.log("Updating question Model:", this.propertyModel);
    this.propertyModel.expressions[this.propertyModel.syntax] = JSON.stringify(this.properties);
  }

  onPropertyChange(event: CustomEvent<string>, property: NestedProperty) {
    event = event;
    property = property;
    console.log("on property change", property)
    //property.expressions[property.descriptor.defaultSyntax] = event.detail;
    this.updateQuestionModel();
  }


  render() {

    const displayManager = this.displayManager;

    const renderPropertyEditor = (property: NestedProperty) => {
      console.log("rendering nested properties...");
      console.log("rendering " + property.descriptor.label, property.descriptor.label);
      console.log("rendering " + property.descriptor.name, property.descriptor.name)
      console.log("supported Syntaxes: ", property.descriptor.supportedSyntaxes ?? "none");

      let content = displayManager.displayNested(this.activityModel, property, this.onPropertyChange.bind(this));
      let id = this.propertyModel.name+'_'+property.descriptor.name + "Category";
      return (
        <elsa-control id={id} class="sm:elsa-col-span-6 hydrated">
          {content}
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
