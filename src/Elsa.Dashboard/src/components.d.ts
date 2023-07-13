/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "./models/elsa-interfaces";
import { VNode } from "@stencil/core";
import { DataDictionaryGroup, HeActivityPropertyDescriptor, NestedActivityDefinitionProperty, NestedPropertyModel } from "./models/custom-component-models";
export namespace Components {
    interface HeCheckListProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeCheckboxOptionsProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeCheckboxProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeDataTableProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeElsaControl {
        "content": VNode | string | Element;
    }
    interface HeJsonProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeMultiLineProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeMultiTextProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HePotscoreRadioOptionsProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeQuestionDataDictionaryProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeRadioOptionsProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeSingleLineProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeSwitchAnswersProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeTextActivityProperty {
        "activityModel": ActivityModel;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeWeightedCheckboxOptionGroupProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": NestedActivityDefinitionProperty;
    }
    interface HeWeightedCheckboxProperty {
        "activityModel": ActivityModel;
        "keyId": string;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeWeightedRadioOptionGroupProperty {
        "activityModel": ActivityModel;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": NestedActivityDefinitionProperty;
    }
    interface HeWeightedRadioProperty {
        "activityModel": ActivityModel;
        "modelSyntax": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface NestedPropertyList {
        "activityModel": ActivityModel;
        "nestedDescriptors": Array<HeActivityPropertyDescriptor>;
        "propertyModel": NestedActivityDefinitionProperty;
    }
    interface QuestionProperty {
        "activityModel": ActivityModel;
        "dataDictionaryGroup": Array<DataDictionaryGroup>;
        "questionModel": NestedPropertyModel;
    }
    interface QuestionScreenProperty {
        "activityModel": ActivityModel;
        "dataDictionaryGroup": Array<DataDictionaryGroup>;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "questionProperties": Array<HeActivityPropertyDescriptor>;
    }
    interface SwitchAnswersProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
}
export interface HeCheckListPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeCheckListPropertyElement;
}
export interface HeCheckboxOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeCheckboxOptionsPropertyElement;
}
export interface HeCheckboxPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeCheckboxPropertyElement;
}
export interface HeDataTablePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeDataTablePropertyElement;
}
export interface HeJsonPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeJsonPropertyElement;
}
export interface HeMultiLinePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeMultiLinePropertyElement;
}
export interface HeMultiTextPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeMultiTextPropertyElement;
}
export interface HePotscoreRadioOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHePotscoreRadioOptionsPropertyElement;
}
export interface HeQuestionDataDictionaryPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeQuestionDataDictionaryPropertyElement;
}
export interface HeRadioOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeRadioOptionsPropertyElement;
}
export interface HeSingleLinePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeSingleLinePropertyElement;
}
export interface HeSwitchAnswersPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeSwitchAnswersPropertyElement;
}
export interface HeTextActivityPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeTextActivityPropertyElement;
}
export interface HeWeightedCheckboxOptionGroupPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeWeightedCheckboxOptionGroupPropertyElement;
}
export interface HeWeightedCheckboxPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeWeightedCheckboxPropertyElement;
}
export interface HeWeightedRadioOptionGroupPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeWeightedRadioOptionGroupPropertyElement;
}
export interface HeWeightedRadioPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeWeightedRadioPropertyElement;
}
export interface NestedPropertyListCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLNestedPropertyListElement;
}
export interface QuestionPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionPropertyElement;
}
declare global {
    interface HTMLHeCheckListPropertyElement extends Components.HeCheckListProperty, HTMLStencilElement {
    }
    var HTMLHeCheckListPropertyElement: {
        prototype: HTMLHeCheckListPropertyElement;
        new (): HTMLHeCheckListPropertyElement;
    };
    interface HTMLHeCheckboxOptionsPropertyElement extends Components.HeCheckboxOptionsProperty, HTMLStencilElement {
    }
    var HTMLHeCheckboxOptionsPropertyElement: {
        prototype: HTMLHeCheckboxOptionsPropertyElement;
        new (): HTMLHeCheckboxOptionsPropertyElement;
    };
    interface HTMLHeCheckboxPropertyElement extends Components.HeCheckboxProperty, HTMLStencilElement {
    }
    var HTMLHeCheckboxPropertyElement: {
        prototype: HTMLHeCheckboxPropertyElement;
        new (): HTMLHeCheckboxPropertyElement;
    };
    interface HTMLHeDataTablePropertyElement extends Components.HeDataTableProperty, HTMLStencilElement {
    }
    var HTMLHeDataTablePropertyElement: {
        prototype: HTMLHeDataTablePropertyElement;
        new (): HTMLHeDataTablePropertyElement;
    };
    interface HTMLHeElsaControlElement extends Components.HeElsaControl, HTMLStencilElement {
    }
    var HTMLHeElsaControlElement: {
        prototype: HTMLHeElsaControlElement;
        new (): HTMLHeElsaControlElement;
    };
    interface HTMLHeJsonPropertyElement extends Components.HeJsonProperty, HTMLStencilElement {
    }
    var HTMLHeJsonPropertyElement: {
        prototype: HTMLHeJsonPropertyElement;
        new (): HTMLHeJsonPropertyElement;
    };
    interface HTMLHeMultiLinePropertyElement extends Components.HeMultiLineProperty, HTMLStencilElement {
    }
    var HTMLHeMultiLinePropertyElement: {
        prototype: HTMLHeMultiLinePropertyElement;
        new (): HTMLHeMultiLinePropertyElement;
    };
    interface HTMLHeMultiTextPropertyElement extends Components.HeMultiTextProperty, HTMLStencilElement {
    }
    var HTMLHeMultiTextPropertyElement: {
        prototype: HTMLHeMultiTextPropertyElement;
        new (): HTMLHeMultiTextPropertyElement;
    };
    interface HTMLHePotscoreRadioOptionsPropertyElement extends Components.HePotscoreRadioOptionsProperty, HTMLStencilElement {
    }
    var HTMLHePotscoreRadioOptionsPropertyElement: {
        prototype: HTMLHePotscoreRadioOptionsPropertyElement;
        new (): HTMLHePotscoreRadioOptionsPropertyElement;
    };
    interface HTMLHeQuestionDataDictionaryPropertyElement extends Components.HeQuestionDataDictionaryProperty, HTMLStencilElement {
    }
    var HTMLHeQuestionDataDictionaryPropertyElement: {
        prototype: HTMLHeQuestionDataDictionaryPropertyElement;
        new (): HTMLHeQuestionDataDictionaryPropertyElement;
    };
    interface HTMLHeRadioOptionsPropertyElement extends Components.HeRadioOptionsProperty, HTMLStencilElement {
    }
    var HTMLHeRadioOptionsPropertyElement: {
        prototype: HTMLHeRadioOptionsPropertyElement;
        new (): HTMLHeRadioOptionsPropertyElement;
    };
    interface HTMLHeSingleLinePropertyElement extends Components.HeSingleLineProperty, HTMLStencilElement {
    }
    var HTMLHeSingleLinePropertyElement: {
        prototype: HTMLHeSingleLinePropertyElement;
        new (): HTMLHeSingleLinePropertyElement;
    };
    interface HTMLHeSwitchAnswersPropertyElement extends Components.HeSwitchAnswersProperty, HTMLStencilElement {
    }
    var HTMLHeSwitchAnswersPropertyElement: {
        prototype: HTMLHeSwitchAnswersPropertyElement;
        new (): HTMLHeSwitchAnswersPropertyElement;
    };
    interface HTMLHeTextActivityPropertyElement extends Components.HeTextActivityProperty, HTMLStencilElement {
    }
    var HTMLHeTextActivityPropertyElement: {
        prototype: HTMLHeTextActivityPropertyElement;
        new (): HTMLHeTextActivityPropertyElement;
    };
    interface HTMLHeWeightedCheckboxOptionGroupPropertyElement extends Components.HeWeightedCheckboxOptionGroupProperty, HTMLStencilElement {
    }
    var HTMLHeWeightedCheckboxOptionGroupPropertyElement: {
        prototype: HTMLHeWeightedCheckboxOptionGroupPropertyElement;
        new (): HTMLHeWeightedCheckboxOptionGroupPropertyElement;
    };
    interface HTMLHeWeightedCheckboxPropertyElement extends Components.HeWeightedCheckboxProperty, HTMLStencilElement {
    }
    var HTMLHeWeightedCheckboxPropertyElement: {
        prototype: HTMLHeWeightedCheckboxPropertyElement;
        new (): HTMLHeWeightedCheckboxPropertyElement;
    };
    interface HTMLHeWeightedRadioOptionGroupPropertyElement extends Components.HeWeightedRadioOptionGroupProperty, HTMLStencilElement {
    }
    var HTMLHeWeightedRadioOptionGroupPropertyElement: {
        prototype: HTMLHeWeightedRadioOptionGroupPropertyElement;
        new (): HTMLHeWeightedRadioOptionGroupPropertyElement;
    };
    interface HTMLHeWeightedRadioPropertyElement extends Components.HeWeightedRadioProperty, HTMLStencilElement {
    }
    var HTMLHeWeightedRadioPropertyElement: {
        prototype: HTMLHeWeightedRadioPropertyElement;
        new (): HTMLHeWeightedRadioPropertyElement;
    };
    interface HTMLNestedPropertyListElement extends Components.NestedPropertyList, HTMLStencilElement {
    }
    var HTMLNestedPropertyListElement: {
        prototype: HTMLNestedPropertyListElement;
        new (): HTMLNestedPropertyListElement;
    };
    interface HTMLQuestionPropertyElement extends Components.QuestionProperty, HTMLStencilElement {
    }
    var HTMLQuestionPropertyElement: {
        prototype: HTMLQuestionPropertyElement;
        new (): HTMLQuestionPropertyElement;
    };
    interface HTMLQuestionScreenPropertyElement extends Components.QuestionScreenProperty, HTMLStencilElement {
    }
    var HTMLQuestionScreenPropertyElement: {
        prototype: HTMLQuestionScreenPropertyElement;
        new (): HTMLQuestionScreenPropertyElement;
    };
    interface HTMLSwitchAnswersPropertyElement extends Components.SwitchAnswersProperty, HTMLStencilElement {
    }
    var HTMLSwitchAnswersPropertyElement: {
        prototype: HTMLSwitchAnswersPropertyElement;
        new (): HTMLSwitchAnswersPropertyElement;
    };
    interface HTMLElementTagNameMap {
        "he-check-list-property": HTMLHeCheckListPropertyElement;
        "he-checkbox-options-property": HTMLHeCheckboxOptionsPropertyElement;
        "he-checkbox-property": HTMLHeCheckboxPropertyElement;
        "he-data-table-property": HTMLHeDataTablePropertyElement;
        "he-elsa-control": HTMLHeElsaControlElement;
        "he-json-property": HTMLHeJsonPropertyElement;
        "he-multi-line-property": HTMLHeMultiLinePropertyElement;
        "he-multi-text-property": HTMLHeMultiTextPropertyElement;
        "he-potscore-radio-options-property": HTMLHePotscoreRadioOptionsPropertyElement;
        "he-question-data-dictionary-property": HTMLHeQuestionDataDictionaryPropertyElement;
        "he-radio-options-property": HTMLHeRadioOptionsPropertyElement;
        "he-single-line-property": HTMLHeSingleLinePropertyElement;
        "he-switch-answers-property": HTMLHeSwitchAnswersPropertyElement;
        "he-text-activity-property": HTMLHeTextActivityPropertyElement;
        "he-weighted-checkbox-option-group-property": HTMLHeWeightedCheckboxOptionGroupPropertyElement;
        "he-weighted-checkbox-property": HTMLHeWeightedCheckboxPropertyElement;
        "he-weighted-radio-option-group-property": HTMLHeWeightedRadioOptionGroupPropertyElement;
        "he-weighted-radio-property": HTMLHeWeightedRadioPropertyElement;
        "nested-property-list": HTMLNestedPropertyListElement;
        "question-property": HTMLQuestionPropertyElement;
        "question-screen-property": HTMLQuestionScreenPropertyElement;
        "switch-answers-property": HTMLSwitchAnswersPropertyElement;
    }
}
declare namespace LocalJSX {
    interface HeCheckListProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeCheckListPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeCheckboxOptionsProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeCheckboxOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeCheckboxProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeCheckboxPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeDataTableProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeDataTablePropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeElsaControl {
        "content"?: VNode | string | Element;
    }
    interface HeJsonProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeJsonPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeMultiLineProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeMultiLinePropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeMultiTextProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeMultiTextPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HePotscoreRadioOptionsProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HePotscoreRadioOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeQuestionDataDictionaryProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeQuestionDataDictionaryPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeRadioOptionsProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeRadioOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeSingleLineProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeSingleLinePropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeSwitchAnswersProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "onExpressionChanged"?: (event: HeSwitchAnswersPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeTextActivityProperty {
        "activityModel"?: ActivityModel;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeTextActivityPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeWeightedCheckboxOptionGroupProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeWeightedCheckboxOptionGroupPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: NestedActivityDefinitionProperty;
    }
    interface HeWeightedCheckboxProperty {
        "activityModel"?: ActivityModel;
        "keyId"?: string;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeWeightedCheckboxPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeWeightedRadioOptionGroupProperty {
        "activityModel"?: ActivityModel;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeWeightedRadioOptionGroupPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: NestedActivityDefinitionProperty;
    }
    interface HeWeightedRadioProperty {
        "activityModel"?: ActivityModel;
        "modelSyntax"?: string;
        "onExpressionChanged"?: (event: HeWeightedRadioPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface NestedPropertyList {
        "activityModel"?: ActivityModel;
        "nestedDescriptors"?: Array<HeActivityPropertyDescriptor>;
        "onExpressionChanged"?: (event: NestedPropertyListCustomEvent<string>) => void;
        "propertyModel"?: NestedActivityDefinitionProperty;
    }
    interface QuestionProperty {
        "activityModel"?: ActivityModel;
        "dataDictionaryGroup"?: Array<DataDictionaryGroup>;
        "onUpdateQuestionScreen"?: (event: QuestionPropertyCustomEvent<string>) => void;
        "questionModel"?: NestedPropertyModel;
    }
    interface QuestionScreenProperty {
        "activityModel"?: ActivityModel;
        "dataDictionaryGroup"?: Array<DataDictionaryGroup>;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "questionProperties"?: Array<HeActivityPropertyDescriptor>;
    }
    interface SwitchAnswersProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface IntrinsicElements {
        "he-check-list-property": HeCheckListProperty;
        "he-checkbox-options-property": HeCheckboxOptionsProperty;
        "he-checkbox-property": HeCheckboxProperty;
        "he-data-table-property": HeDataTableProperty;
        "he-elsa-control": HeElsaControl;
        "he-json-property": HeJsonProperty;
        "he-multi-line-property": HeMultiLineProperty;
        "he-multi-text-property": HeMultiTextProperty;
        "he-potscore-radio-options-property": HePotscoreRadioOptionsProperty;
        "he-question-data-dictionary-property": HeQuestionDataDictionaryProperty;
        "he-radio-options-property": HeRadioOptionsProperty;
        "he-single-line-property": HeSingleLineProperty;
        "he-switch-answers-property": HeSwitchAnswersProperty;
        "he-text-activity-property": HeTextActivityProperty;
        "he-weighted-checkbox-option-group-property": HeWeightedCheckboxOptionGroupProperty;
        "he-weighted-checkbox-property": HeWeightedCheckboxProperty;
        "he-weighted-radio-option-group-property": HeWeightedRadioOptionGroupProperty;
        "he-weighted-radio-property": HeWeightedRadioProperty;
        "nested-property-list": NestedPropertyList;
        "question-property": QuestionProperty;
        "question-screen-property": QuestionScreenProperty;
        "switch-answers-property": SwitchAnswersProperty;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "he-check-list-property": LocalJSX.HeCheckListProperty & JSXBase.HTMLAttributes<HTMLHeCheckListPropertyElement>;
            "he-checkbox-options-property": LocalJSX.HeCheckboxOptionsProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxOptionsPropertyElement>;
            "he-checkbox-property": LocalJSX.HeCheckboxProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxPropertyElement>;
            "he-data-table-property": LocalJSX.HeDataTableProperty & JSXBase.HTMLAttributes<HTMLHeDataTablePropertyElement>;
            "he-elsa-control": LocalJSX.HeElsaControl & JSXBase.HTMLAttributes<HTMLHeElsaControlElement>;
            "he-json-property": LocalJSX.HeJsonProperty & JSXBase.HTMLAttributes<HTMLHeJsonPropertyElement>;
            "he-multi-line-property": LocalJSX.HeMultiLineProperty & JSXBase.HTMLAttributes<HTMLHeMultiLinePropertyElement>;
            "he-multi-text-property": LocalJSX.HeMultiTextProperty & JSXBase.HTMLAttributes<HTMLHeMultiTextPropertyElement>;
            "he-potscore-radio-options-property": LocalJSX.HePotscoreRadioOptionsProperty & JSXBase.HTMLAttributes<HTMLHePotscoreRadioOptionsPropertyElement>;
            "he-question-data-dictionary-property": LocalJSX.HeQuestionDataDictionaryProperty & JSXBase.HTMLAttributes<HTMLHeQuestionDataDictionaryPropertyElement>;
            "he-radio-options-property": LocalJSX.HeRadioOptionsProperty & JSXBase.HTMLAttributes<HTMLHeRadioOptionsPropertyElement>;
            "he-single-line-property": LocalJSX.HeSingleLineProperty & JSXBase.HTMLAttributes<HTMLHeSingleLinePropertyElement>;
            "he-switch-answers-property": LocalJSX.HeSwitchAnswersProperty & JSXBase.HTMLAttributes<HTMLHeSwitchAnswersPropertyElement>;
            "he-text-activity-property": LocalJSX.HeTextActivityProperty & JSXBase.HTMLAttributes<HTMLHeTextActivityPropertyElement>;
            "he-weighted-checkbox-option-group-property": LocalJSX.HeWeightedCheckboxOptionGroupProperty & JSXBase.HTMLAttributes<HTMLHeWeightedCheckboxOptionGroupPropertyElement>;
            "he-weighted-checkbox-property": LocalJSX.HeWeightedCheckboxProperty & JSXBase.HTMLAttributes<HTMLHeWeightedCheckboxPropertyElement>;
            "he-weighted-radio-option-group-property": LocalJSX.HeWeightedRadioOptionGroupProperty & JSXBase.HTMLAttributes<HTMLHeWeightedRadioOptionGroupPropertyElement>;
            "he-weighted-radio-property": LocalJSX.HeWeightedRadioProperty & JSXBase.HTMLAttributes<HTMLHeWeightedRadioPropertyElement>;
            "nested-property-list": LocalJSX.NestedPropertyList & JSXBase.HTMLAttributes<HTMLNestedPropertyListElement>;
            "question-property": LocalJSX.QuestionProperty & JSXBase.HTMLAttributes<HTMLQuestionPropertyElement>;
            "question-screen-property": LocalJSX.QuestionScreenProperty & JSXBase.HTMLAttributes<HTMLQuestionScreenPropertyElement>;
            "switch-answers-property": LocalJSX.SwitchAnswersProperty & JSXBase.HTMLAttributes<HTMLSwitchAnswersPropertyElement>;
        }
    }
}
