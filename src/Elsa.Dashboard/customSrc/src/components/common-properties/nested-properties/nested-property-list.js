var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
import { Component, Event, h, Prop, State } from '@stencil/core';
import { SyntaxNames } from '../../../constants/constants';
import { HePropertyDisplayManager } from '../../../nested-drivers/display-managers/display-manager';
let NestedPropertyList = class NestedPropertyList {
    constructor() {
        var _a;
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
            return (h("elsa-control", { id: id, class: "sm:elsa-col-span-6 hydrated" },
                content,
                h("br", null)));
        };
        return (h("div", { class: "elsa-grid elsa-grid-cols-1 elsa-gap-y-6 elsa-gap-x-4 sm:elsa-grid-cols-6" }, this.properties.map(renderPropertyEditor)));
    }
    ;
};
__decorate([
    Prop()
], NestedPropertyList.prototype, "activityModel", void 0);
__decorate([
    Prop()
], NestedPropertyList.prototype, "propertyModel", void 0);
__decorate([
    State()
], NestedPropertyList.prototype, "properties", void 0);
__decorate([
    Prop()
], NestedPropertyList.prototype, "nestedDescriptors", void 0);
__decorate([
    Event()
], NestedPropertyList.prototype, "expressionChanged", void 0);
__decorate([
    State()
], NestedPropertyList.prototype, "displayManager", void 0);
__decorate([
    State()
], NestedPropertyList.prototype, "modelSyntax", void 0);
NestedPropertyList = __decorate([
    Component({
        tag: 'nested-property-list',
        shadow: false,
    })
], NestedPropertyList);
export { NestedPropertyList };
