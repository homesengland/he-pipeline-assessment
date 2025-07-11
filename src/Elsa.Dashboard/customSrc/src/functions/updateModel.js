import { mapSyntaxToLanguage } from "../utils/utils";
//Update Model method referenced in the below functions MUST be included on the component in which they are consumed.
export function CustomUpdateExpression(e, property, syntax) {
    property.expressions[syntax] = e.detail;
    this.updatePropertyModel();
}
export function StandardUpdateExpression(e, property, syntax) {
    property.expressions[syntax] = e.currentTarget.value.trim();
    this.updatePropertyModel();
}
export function UpdateExpressionFromInput(e, property, syntax) {
    let elementToUpdate = e.currentTarget;
    let valueToUpdate = elementToUpdate.value.trim();
    property.expressions[syntax] = valueToUpdate;
    this.updatePropertyModel();
}
export function UpdateCheckbox(e, property, syntax) {
    const checkboxElement = e.currentTarget;
    property.expressions[syntax] = checkboxElement.checked.toString();
    this.updatePropertyModel();
    console.table(property);
}
export function UpdateName(e, property) {
    property.name = e.currentTarget.value.trim();
    this.updatePropertyModel();
}
export function UpdateDropdown(e, property, syntax) {
    const select = e.currentTarget;
    property.expressions[syntax] = select.value;
    this.updatePropertyModel();
}
export function UpdateSyntax(e, property, expressionEditor) {
    const select = e.currentTarget;
    property.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(property.syntax);
    this.updatePropertyModel();
}
