/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from "./models/elsa-interfaces";
import { CheckboxQuestion, HeActivityPropertyDescriptor, IOutcomeProperty, IQuestionComponent, ITextProperty, QuestionModel, RadioQuestion, TextAreaQuestion } from "./models/custom-component-models";
import { IconProvider } from "./components/icon-provider/icon-provider";
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
    interface CustomOutcomeProperty {
        "context"?: string;
        "editorHeight": string;
        "iconProvider": IconProvider;
        "index": number;
        "intellisenseContext": IntellisenseContext;
        "outcome": IOutcomeProperty;
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
    interface ElsaTextareaQuestion {
        "question": TextAreaQuestion;
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
    interface QuestionCheckboxProperty {
        "question": CheckboxQuestion;
    }
    interface QuestionProperty {
        "activityModel": ActivityModel;
        "questionModel": QuestionModel;
    }
    interface QuestionPropertyOld {
        "ActivityModel": ActivityModel;
        "question": IQuestionComponent;
    }
    interface QuestionPropertyV2 {
        "activityModel": ActivityModel;
        "questionModel": QuestionModel;
    }
    interface QuestionRadioProperty {
        "question": RadioQuestion;
    }
    interface QuestionScreenProperty {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "questionProperties": Array<HeActivityPropertyDescriptor>;
    }
    interface QuestionScreenPropertyOld {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
        "questionProperties": Array<ActivityPropertyDescriptor>;
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
export interface CustomOutcomePropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLCustomOutcomePropertyElement;
}
export interface ElsaTextareaQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaTextareaQuestionElement;
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
export interface QuestionCheckboxPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionCheckboxPropertyElement;
}
export interface QuestionPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionPropertyElement;
}
export interface QuestionPropertyOldCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionPropertyOldElement;
}
export interface QuestionRadioPropertyCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLQuestionRadioPropertyElement;
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
    interface HTMLCustomOutcomePropertyElement extends Components.CustomOutcomeProperty, HTMLStencilElement {
    }
    var HTMLCustomOutcomePropertyElement: {
        prototype: HTMLCustomOutcomePropertyElement;
        new (): HTMLCustomOutcomePropertyElement;
    };
    interface HTMLCustomTextPropertyElement extends Components.CustomTextProperty, HTMLStencilElement {
    }
    var HTMLCustomTextPropertyElement: {
        prototype: HTMLCustomTextPropertyElement;
        new (): HTMLCustomTextPropertyElement;
    };
    interface HTMLElsaTextareaQuestionElement extends Components.ElsaTextareaQuestion, HTMLStencilElement {
    }
    var HTMLElsaTextareaQuestionElement: {
        prototype: HTMLElsaTextareaQuestionElement;
        new (): HTMLElsaTextareaQuestionElement;
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
    interface HTMLQuestionCheckboxPropertyElement extends Components.QuestionCheckboxProperty, HTMLStencilElement {
    }
    var HTMLQuestionCheckboxPropertyElement: {
        prototype: HTMLQuestionCheckboxPropertyElement;
        new (): HTMLQuestionCheckboxPropertyElement;
    };
    interface HTMLQuestionPropertyElement extends Components.QuestionProperty, HTMLStencilElement {
    }
    var HTMLQuestionPropertyElement: {
        prototype: HTMLQuestionPropertyElement;
        new (): HTMLQuestionPropertyElement;
    };
    interface HTMLQuestionPropertyOldElement extends Components.QuestionPropertyOld, HTMLStencilElement {
    }
    var HTMLQuestionPropertyOldElement: {
        prototype: HTMLQuestionPropertyOldElement;
        new (): HTMLQuestionPropertyOldElement;
    };
    interface HTMLQuestionPropertyV2Element extends Components.QuestionPropertyV2, HTMLStencilElement {
    }
    var HTMLQuestionPropertyV2Element: {
        prototype: HTMLQuestionPropertyV2Element;
        new (): HTMLQuestionPropertyV2Element;
    };
    interface HTMLQuestionRadioPropertyElement extends Components.QuestionRadioProperty, HTMLStencilElement {
    }
    var HTMLQuestionRadioPropertyElement: {
        prototype: HTMLQuestionRadioPropertyElement;
        new (): HTMLQuestionRadioPropertyElement;
    };
    interface HTMLQuestionScreenPropertyElement extends Components.QuestionScreenProperty, HTMLStencilElement {
    }
    var HTMLQuestionScreenPropertyElement: {
        prototype: HTMLQuestionScreenPropertyElement;
        new (): HTMLQuestionScreenPropertyElement;
    };
    interface HTMLQuestionScreenPropertyOldElement extends Components.QuestionScreenPropertyOld, HTMLStencilElement {
    }
    var HTMLQuestionScreenPropertyOldElement: {
        prototype: HTMLQuestionScreenPropertyOldElement;
        new (): HTMLQuestionScreenPropertyOldElement;
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
        "custom-outcome-property": HTMLCustomOutcomePropertyElement;
        "custom-text-property": HTMLCustomTextPropertyElement;
        "elsa-textarea-question": HTMLElsaTextareaQuestionElement;
        "he-check-list-property": HTMLHeCheckListPropertyElement;
        "he-checkbox-options-property": HTMLHeCheckboxOptionsPropertyElement;
        "he-checkbox-property": HTMLHeCheckboxPropertyElement;
        "he-json-property": HTMLHeJsonPropertyElement;
        "he-multi-line-property": HTMLHeMultiLinePropertyElement;
        "he-radio-options-property": HTMLHeRadioOptionsPropertyElement;
        "he-single-line-property": HTMLHeSingleLinePropertyElement;
        "he-switch-options-property": HTMLHeSwitchOptionsPropertyElement;
        "question-checkbox-property": HTMLQuestionCheckboxPropertyElement;
        "question-property": HTMLQuestionPropertyElement;
        "question-property-old": HTMLQuestionPropertyOldElement;
        "question-property-v2": HTMLQuestionPropertyV2Element;
        "question-radio-property": HTMLQuestionRadioPropertyElement;
        "question-screen-property": HTMLQuestionScreenPropertyElement;
        "question-screen-property-old": HTMLQuestionScreenPropertyOldElement;
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
    interface CustomOutcomeProperty {
        "context"?: string;
        "editorHeight"?: string;
        "iconProvider"?: IconProvider;
        "index"?: number;
        "intellisenseContext"?: IntellisenseContext;
        "onDelete"?: (event: CustomOutcomePropertyCustomEvent<IOutcomeProperty>) => void;
        "onValueChanged"?: (event: CustomOutcomePropertyCustomEvent<IOutcomeProperty>) => void;
        "outcome"?: IOutcomeProperty;
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
    interface ElsaTextareaQuestion {
        "onUpdateQuestion"?: (event: ElsaTextareaQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: TextAreaQuestion;
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
    interface QuestionCheckboxProperty {
        "onUpdateQuestion"?: (event: QuestionCheckboxPropertyCustomEvent<IQuestionComponent>) => void;
        "question"?: CheckboxQuestion;
    }
    interface QuestionProperty {
        "activityModel"?: ActivityModel;
        "onUpdateQuestionScreen"?: (event: QuestionPropertyCustomEvent<string>) => void;
        "questionModel"?: QuestionModel;
    }
    interface QuestionPropertyOld {
        "ActivityModel"?: ActivityModel;
        "onUpdateQuestion"?: (event: QuestionPropertyOldCustomEvent<IQuestionComponent>) => void;
        "question"?: IQuestionComponent;
    }
    interface QuestionPropertyV2 {
        "activityModel"?: ActivityModel;
        "questionModel"?: QuestionModel;
    }
    interface QuestionRadioProperty {
        "onUpdateQuestion"?: (event: QuestionRadioPropertyCustomEvent<IQuestionComponent>) => void;
        "question"?: RadioQuestion;
    }
    interface QuestionScreenProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "questionProperties"?: Array<HeActivityPropertyDescriptor>;
    }
    interface QuestionScreenPropertyOld {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
        "questionProperties"?: Array<ActivityPropertyDescriptor>;
    }
    interface SwitchOptionsProperty {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface IntrinsicElements {
        "conditional-text-list-property": ConditionalTextListProperty;
        "custom-input-property": CustomInputProperty;
        "custom-outcome-property": CustomOutcomeProperty;
        "custom-text-property": CustomTextProperty;
        "elsa-textarea-question": ElsaTextareaQuestion;
        "he-check-list-property": HeCheckListProperty;
        "he-checkbox-options-property": HeCheckboxOptionsProperty;
        "he-checkbox-property": HeCheckboxProperty;
        "he-json-property": HeJsonProperty;
        "he-multi-line-property": HeMultiLineProperty;
        "he-radio-options-property": HeRadioOptionsProperty;
        "he-single-line-property": HeSingleLineProperty;
        "he-switch-options-property": HeSwitchOptionsProperty;
        "question-checkbox-property": QuestionCheckboxProperty;
        "question-property": QuestionProperty;
        "question-property-old": QuestionPropertyOld;
        "question-property-v2": QuestionPropertyV2;
        "question-radio-property": QuestionRadioProperty;
        "question-screen-property": QuestionScreenProperty;
        "question-screen-property-old": QuestionScreenPropertyOld;
        "switch-options-property": SwitchOptionsProperty;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "conditional-text-list-property": LocalJSX.ConditionalTextListProperty & JSXBase.HTMLAttributes<HTMLConditionalTextListPropertyElement>;
            "custom-input-property": LocalJSX.CustomInputProperty & JSXBase.HTMLAttributes<HTMLCustomInputPropertyElement>;
            "custom-outcome-property": LocalJSX.CustomOutcomeProperty & JSXBase.HTMLAttributes<HTMLCustomOutcomePropertyElement>;
            "custom-text-property": LocalJSX.CustomTextProperty & JSXBase.HTMLAttributes<HTMLCustomTextPropertyElement>;
            "elsa-textarea-question": LocalJSX.ElsaTextareaQuestion & JSXBase.HTMLAttributes<HTMLElsaTextareaQuestionElement>;
            "he-check-list-property": LocalJSX.HeCheckListProperty & JSXBase.HTMLAttributes<HTMLHeCheckListPropertyElement>;
            "he-checkbox-options-property": LocalJSX.HeCheckboxOptionsProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxOptionsPropertyElement>;
            "he-checkbox-property": LocalJSX.HeCheckboxProperty & JSXBase.HTMLAttributes<HTMLHeCheckboxPropertyElement>;
            "he-json-property": LocalJSX.HeJsonProperty & JSXBase.HTMLAttributes<HTMLHeJsonPropertyElement>;
            "he-multi-line-property": LocalJSX.HeMultiLineProperty & JSXBase.HTMLAttributes<HTMLHeMultiLinePropertyElement>;
            "he-radio-options-property": LocalJSX.HeRadioOptionsProperty & JSXBase.HTMLAttributes<HTMLHeRadioOptionsPropertyElement>;
            "he-single-line-property": LocalJSX.HeSingleLineProperty & JSXBase.HTMLAttributes<HTMLHeSingleLinePropertyElement>;
            "he-switch-options-property": LocalJSX.HeSwitchOptionsProperty & JSXBase.HTMLAttributes<HTMLHeSwitchOptionsPropertyElement>;
            "question-checkbox-property": LocalJSX.QuestionCheckboxProperty & JSXBase.HTMLAttributes<HTMLQuestionCheckboxPropertyElement>;
            "question-property": LocalJSX.QuestionProperty & JSXBase.HTMLAttributes<HTMLQuestionPropertyElement>;
            "question-property-old": LocalJSX.QuestionPropertyOld & JSXBase.HTMLAttributes<HTMLQuestionPropertyOldElement>;
            "question-property-v2": LocalJSX.QuestionPropertyV2 & JSXBase.HTMLAttributes<HTMLQuestionPropertyV2Element>;
            "question-radio-property": LocalJSX.QuestionRadioProperty & JSXBase.HTMLAttributes<HTMLQuestionRadioPropertyElement>;
            "question-screen-property": LocalJSX.QuestionScreenProperty & JSXBase.HTMLAttributes<HTMLQuestionScreenPropertyElement>;
            "question-screen-property-old": LocalJSX.QuestionScreenPropertyOld & JSXBase.HTMLAttributes<HTMLQuestionScreenPropertyOldElement>;
            "switch-options-property": LocalJSX.SwitchOptionsProperty & JSXBase.HTMLAttributes<HTMLSwitchOptionsPropertyElement>;
        }
    }
}
