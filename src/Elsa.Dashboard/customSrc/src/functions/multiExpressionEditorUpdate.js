import { SyntaxNames } from "../constants/constants";
import { parseJson } from "../utils/utils";
//OnMultiExpressionEditorValueChanged must only be imported into classes that utilise the PropertyModel Prop, and the UpdateNestedExpressions function.
export function OnMultiExpressionEditorValueChanged(e, syntax = SyntaxNames.Json) {
    const json = e.detail;
    const parsed = parseJson(json);
    if (!parsed)
        return;
    if (!Array.isArray(parsed))
        return;
    this.propertyModel.expressions[syntax] = json;
    this.updateNestedExpressions(parsed);
}
