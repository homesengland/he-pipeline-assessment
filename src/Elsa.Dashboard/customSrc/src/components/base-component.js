import Sortable from "sortablejs";
import { SyntaxNames } from "../constants/constants";
import { getUniversalUniqueId, mapSyntaxToLanguage, parseJson } from "../utils/utils";
export class BaseComponent {
    constructor(component) {
        this.component = component;
    }
    componentWillLoad() {
        if (this.component.propertyDescriptor != null) {
            if (this.component.propertyDescriptor.defaultSyntax != null
                && this.component.propertyDescriptor.defaultSyntax != undefined
                && this.component.propertyDescriptor.defaultSyntax != "") {
                this.component.modelSyntax = this.component.propertyDescriptor.defaultSyntax;
            }
        }
        const propertyModel = this.component.propertyModel;
        const modelJson = propertyModel.expressions[this.component.modelSyntax];
        this.component.properties = parseJson(modelJson) || [];
    }
    componentDidLoad() { }
    componentWillRender() {
        this.component.keyId = getUniversalUniqueId();
    }
    render() { }
    updatePropertyModel() {
        this.component.propertyModel.expressions[this.component.modelSyntax] = JSON.stringify(this.component.properties);
        this.component.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.component.properties, null, 2);
        this.component.expressionChanged.emit(JSON.stringify(this.component.propertyModel));
    }
    CustomUpdateExpression(e, property, syntax) {
        property.expressions[syntax] = e.detail;
        this.updatePropertyModel();
    }
    StandardUpdateExpression(e, property, syntax) {
        property.expressions[syntax] = e.currentTarget.value.trim();
        this.updatePropertyModel();
    }
    UpdateExpressionFromInput(e, property, syntax) {
        let elementToUpdate = e.currentTarget;
        let valueToUpdate = elementToUpdate.value.trim();
        property.expressions[syntax] = valueToUpdate;
        this.updatePropertyModel();
    }
    UpdateCheckbox(e, property, syntax) {
        const checkboxElement = e.currentTarget;
        property.expressions[syntax] = checkboxElement.checked.toString();
        this.updatePropertyModel();
        console.table(property);
    }
    UpdateName(e, property) {
        property.name = e.currentTarget.value.trim();
        this.updatePropertyModel();
    }
    UpdateDropdown(e, property, syntax) {
        const select = e.currentTarget;
        property.expressions[syntax] = select.value;
        this.updatePropertyModel();
    }
    UpdateSyntax(e, property, expressionEditor) {
        const select = e.currentTarget;
        property.syntax = select.value;
        expressionEditor.language = mapSyntaxToLanguage(property.syntax);
        this.updatePropertyModel();
    }
    IdentifierArray() {
        let propertyIdentifiers = [];
        if (this.component.properties.length > 0) {
            propertyIdentifiers = this.component.properties.map(function (v) {
                return v.name;
            });
        }
        return propertyIdentifiers;
    }
    UpdateProperties(parsed) {
        this.component.properties = parsed;
    }
    OnMultiExpressionEditorValueChanged(e, syntax = SyntaxNames.Json) {
        const json = e.detail;
        const parsed = parseJson(json);
        if (!parsed)
            return;
        if (!Array.isArray(parsed))
            return;
        this.component.propertyModel.expressions[syntax] = json;
        this.UpdateProperties(parsed);
    }
}
export class SortableComponent extends BaseComponent {
    constructor(component) {
        super(component);
        this.component = component;
    }
    componentWillLoad() {
        super.componentWillLoad();
    }
    componentDidLoad() {
        super.componentDidLoad();
        const dragEventHandler = this.onDragActivity.bind(this);
        //creates draggable area
        Sortable.create(this.component.container, {
            animation: 150,
            handle: ".sortablejs-custom-handle",
            ghostClass: 'dragTarget',
            onEnd(evt) {
                dragEventHandler(evt.oldIndex, evt.newIndex);
            }
        });
    }
    onDragActivity(oldIndex, newIndex) {
        const propertiesJson = JSON.stringify(this.component.properties);
        let propertiesClone = JSON.parse(propertiesJson);
        const activity = propertiesClone.splice(oldIndex, 1)[0];
        propertiesClone.splice(newIndex, 0, activity);
        this.component.properties = propertiesClone;
        this.updatePropertyModel();
    }
}
