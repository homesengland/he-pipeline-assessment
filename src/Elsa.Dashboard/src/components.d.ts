/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from "./models/elsa-interfaces";
import { HeActivityPropertyDescriptor, ITextProperty, NestedPropertyModel } from "./models/custom-component-models";
export namespace Components {
    interface ConditionalTextListProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface CustomInputProperty {
        "context"?: string;
        "customProperty": ITextProperty;
        "editorHeight": string;
        "index": number;
        "intellisenseContext": IntellisenseContext;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "showLabel": boolean;
        "singleLineMode": boolean;
        "supportedSyntaxes": Array<string>;
    }
    interface CustomTextProperty {
        "activityModel": ActivityModel;
        "editorHeight": string;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "singleLine": boolean;
    }
    interface HeCheckListProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeCheckboxOptionsProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeCheckboxProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeJsonProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeMultiLineProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HePotscoreRadioOptionsProperty {
        "activityModel": ActivityModel;
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
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeSingleLineProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeSwitchOptionsProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface HeTextActivityProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface QuestionProperty {
        "activityModel": ActivityModel;
        "questionModel": NestedPropertyModel;
    }
    interface QuestionScreenProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "questionProperties": Array<HeActivityPropertyDescriptor>;
    }
    interface SwitchOptionsProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
}
export interface CustomInputPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLCustomInputPropertyElement;
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
export interface HeJsonPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeJsonPropertyElement;
}
export interface HeMultiLinePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeMultiLinePropertyElement;
}
export interface HePotscoreRadioOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHePotscoreRadioOptionsPropertyElement;
}
export interface HeRadioOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeRadioOptionsPropertyElement;
}
export interface HeSingleLinePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeSingleLinePropertyElement;
}
export interface HeSwitchOptionsPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeSwitchOptionsPropertyElement;
}
export interface HeTextActivityPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLHeTextActivityPropertyElement;
}
export interface QuestionPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionPropertyElement;
}
declare global {
    interface HTMLConditionalTextListPropertyElement extends Components.ConditionalTextListProperty, HTMLStencilElement {
    }
    var HTMLConditionalTextListPropertyElement: {
        prototype: HTMLConditionalTextListPropertyElement;
        new (): HTMLConditionalTextListPropertyElement;
    };
    interface HTMLCustomInputPropertyElement extends Components.CustomInputProperty, HTMLStencilElement {
    }
    var HTMLCustomInputPropertyElement: {
        prototype: HTMLCustomInputPropertyElement;
        new (): HTMLCustomInputPropertyElement;
    };
    interface HTMLCustomTextPropertyElement extends Components.CustomTextProperty, HTMLStencilElement {
    }
    var HTMLCustomTextPropertyElement: {
        prototype: HTMLCustomTextPropertyElement;
        new (): HTMLCustomTextPropertyElement;
    };
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
    interface HTMLHeSwitchOptionsPropertyElement extends Components.HeSwitchOptionsProperty, HTMLStencilElement {
    }
    var HTMLHeSwitchOptionsPropertyElement: {
        prototype: HTMLHeSwitchOptionsPropertyElement;
        new (): HTMLHeSwitchOptionsPropertyElement;
    };
    interface HTMLHeTextActivityPropertyElement extends Components.HeTextActivityProperty, HTMLStencilElement {
    }
    var HTMLHeTextActivityPropertyElement: {
        prototype: HTMLHeTextActivityPropertyElement;
        new (): HTMLHeTextActivityPropertyElement;
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
    interface HTMLSwitchOptionsPropertyElement extends Components.SwitchOptionsProperty, HTMLStencilElement {
    }
    var HTMLSwitchOptionsPropertyElement: {
        prototype: HTMLSwitchOptionsPropertyElement;
        new (): HTMLSwitchOptionsPropertyElement;
    };
    interface HTMLElementTagNameMap {
        "conditional-text-list-property": HTMLConditionalTextListPropertyElement;
        "custom-input-property": HTMLCustomInputPropertyElement;
        "custom-text-property": HTMLCustomTextPropertyElement;
        "he-check-list-property": HTMLHeCheckListPropertyElement;
        "he-checkbox-options-property": HTMLHeCheckboxOptionsPropertyElement;
        "he-checkbox-property": HTMLHeCheckboxPropertyElement;
        "he-json-property": HTMLHeJsonPropertyElement;
        "he-multi-line-property": HTMLHeMultiLinePropertyElement;
        "he-potscore-radio-options-property": HTMLHePotscoreRadioOptionsPropertyElement;
        "he-question-data-dictionary-property": HTMLHeQuestionDataDictionaryPropertyElement;
        "he-radio-options-property": HTMLHeRadioOptionsPropertyElement;
        "he-single-line-property": HTMLHeSingleLinePropertyElement;
        "he-switch-options-property": HTMLHeSwitchOptionsPropertyElement;
        "he-text-activity-property": HTMLHeTextActivityPropertyElement;
        "question-property": HTMLQuestionPropertyElement;
        "question-screen-property": HTMLQuestionScreenPropertyElement;
        "switch-options-property": HTMLSwitchOptionsPropertyElement;
    }
}
declare namespace LocalJSX {
    interface ConditionalTextListProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface CustomInputProperty {
        "context"?: string;
        "customProperty"?: ITextProperty;
        "editorHeight"?: string;
        "index"?: number;
        "intellisenseContext"?: IntellisenseContext;
        "onValueChanged"?: (event: CustomInputPropertyCustomEvent<ITextProperty>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "showLabel"?: boolean;
        "singleLineMode"?: boolean;
        "supportedSyntaxes"?: Array<string>;
    }
    interface CustomTextProperty {
        "activityModel"?: ActivityModel;
        "editorHeight"?: string;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "singleLine"?: boolean;
    }
    interface HeCheckListProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeCheckListPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeCheckboxOptionsProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeCheckboxOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeCheckboxProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeCheckboxPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeJsonProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeJsonPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeMultiLineProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeMultiLinePropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HePotscoreRadioOptionsProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HePotscoreRadioOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeQuestionDataDictionaryProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeRadioOptionsProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeRadioOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeSingleLineProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeSingleLinePropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeSwitchOptionsProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeSwitchOptionsPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface HeTextActivityProperty {
        "activityModel"?: ActivityModel;
        "onExpressionChanged"?: (event: HeTextActivityPropertyCustomEvent<string>) => void;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface QuestionProperty {
        "activityModel"?: ActivityModel;
        "onUpdateQuestionScreen"?: (event: QuestionPropertyCustomEvent<string>) => void;
        "questionModel"?: NestedPropertyModel;
    }
    interface QuestionScreenProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "questionProperties"?: Array<HeActivityPropertyDescriptor>;
    }
    interface SwitchOptionsProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface IntrinsicElements {
        "conditional-text-list-property": ConditionalTextListProperty;
        "custom-input-property": CustomInputProperty;
        "custom-text-property": CustomTextProperty;
        "he-check-list-property": HeCheckListProperty;
        "he-checkbox-options-property": HeCheckboxOptionsProperty;
        "he-checkbox-property": HeCheckboxProperty;
        "he-json-property": HeJsonProperty;
        "he-multi-line-property": HeMultiLineProperty;
        "he-potscore-radio-options-property": HePotscoreRadioOptionsProperty;
        "he-question-data-dictionary-property": HeQuestionDataDictionaryProperty;
        "he-radio-options-property": HeRadioOptionsProperty;
        "he-single-line-property": HeSingleLineProperty;
        "he-switch-options-property": HeSwitchOptionsProperty;
        "he-text-activity-property": HeTextActivityProperty;
        "question-property": QuestionProperty;
        "question-screen-property": QuestionScreenProperty;
        "switch-options-property": SwitchOptionsProperty;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "conditional-text-list-property": LocalJSX.ConditionalTextListProperty & JSXBase.HTMLAttributes<HTMLConditionalTextListPropertyElement>;
            "custom-input-property": LocalJSX.CustomInputProperty & JSXBase.HTMLAttributes<HTMLCustomInputPropertyElement>;
            "custom-text-property": LocalJSX.CustomTextProperty & JSXBase.HTMLAttributes<HTMLCustomTextPropertyElement>;
            "he-check-list-property": LocalJSX.HeCheckListProperty & JSXBase.HTMLAttributes<HTMLHeCheckListPropertyElement>;
            "he-checkbox-options-property": LocalJSX.HeCheckboxOptionsProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxOptionsPropertyElement>;
            "he-checkbox-property": LocalJSX.HeCheckboxProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxPropertyElement>;
            "he-json-property": LocalJSX.HeJsonProperty & JSXBase.HTMLAttributes<HTMLHeJsonPropertyElement>;
            "he-multi-line-property": LocalJSX.HeMultiLineProperty & JSXBase.HTMLAttributes<HTMLHeMultiLinePropertyElement>;
            "he-potscore-radio-options-property": LocalJSX.HePotscoreRadioOptionsProperty & JSXBase.HTMLAttributes<HTMLHePotscoreRadioOptionsPropertyElement>;
            "he-question-data-dictionary-property": LocalJSX.HeQuestionDataDictionaryProperty & JSXBase.HTMLAttributes<HTMLHeQuestionDataDictionaryPropertyElement>;
            "he-radio-options-property": LocalJSX.HeRadioOptionsProperty & JSXBase.HTMLAttributes<HTMLHeRadioOptionsPropertyElement>;
            "he-single-line-property": LocalJSX.HeSingleLineProperty & JSXBase.HTMLAttributes<HTMLHeSingleLinePropertyElement>;
            "he-switch-options-property": LocalJSX.HeSwitchOptionsProperty & JSXBase.HTMLAttributes<HTMLHeSwitchOptionsPropertyElement>;
            "he-text-activity-property": LocalJSX.HeTextActivityProperty & JSXBase.HTMLAttributes<HTMLHeTextActivityPropertyElement>;
            "question-property": LocalJSX.QuestionProperty & JSXBase.HTMLAttributes<HTMLQuestionPropertyElement>;
            "question-screen-property": LocalJSX.QuestionScreenProperty & JSXBase.HTMLAttributes<HTMLQuestionScreenPropertyElement>;
            "switch-options-property": LocalJSX.SwitchOptionsProperty & JSXBase.HTMLAttributes<HTMLSwitchOptionsPropertyElement>;
        }
    }
}
