import { r as registerInstance, e as createEvent, h } from './index-1542df5c.js';
import { S as SyntaxNames } from './constants-6ea82f24.js';
import { H as HePropertyDisplayManager } from './display-manager-6d41e6e8.js';
import './utils-89b7e981.js';
import './index-912d1a21.js';

const NestedPropertyList = class {
  constructor(hostRef) {
    registerInstance(this, hostRef);
    this.expressionChanged = createEvent(this, "expressionChanged", 7);
    var _a;
    this.activityModel = undefined;
    this.propertyModel = undefined;
    this.properties = undefined;
    this.nestedDescriptors = undefined;
    this.displayManager = new HePropertyDisplayManager();
    this.modelSyntax = SyntaxNames.Json;
    this.modelSyntax = (_a = this.propertyModel.syntax) !== null && _a !== void 0 ? _a : SyntaxNames.Json;
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
  getOrCreateNestedProperty(descriptor) {
    var nestedProperty = {
      descriptor: descriptor,
      value: this.getOrCreateActivityDefinitionProperty(descriptor)
    };
    return nestedProperty;
  }
  getOrCreateActivityDefinitionProperty(descriptor) {
    return {
      name: descriptor.name,
      syntax: descriptor.defaultSyntax,
      expressions: {
        [descriptor.defaultSyntax]: '',
      },
      type: descriptor.expectedOutputType
    };
  }
  updateQuestionModel() {
    this.propertyModel.expressions[this.modelSyntax] = JSON.stringify(this.properties);
    /*    this.expressionChanged.emit(this.propertyModel.expressions[this.propertyModel.syntax])*/
  }
  onPropertyChange(event, property) {
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
    const renderPropertyEditor = (property) => {
      let content = displayManager.displayNested(this.activityModel, property, this.onPropertyChange.bind(this));
      let id = this.propertyModel.name + '_' + property.descriptor.name + "Category";
      return (h("elsa-control", { id: id, class: "sm:elsa-col-span-6 hydrated" }, content, h("br", null)));
    };
    return (h("div", { class: "elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6" }, this.properties.map(renderPropertyEditor)));
  }
  ;
};

export { NestedPropertyList as nested_property_list };
