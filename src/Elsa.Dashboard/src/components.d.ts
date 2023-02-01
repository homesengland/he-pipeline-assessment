/* eslint-disable */
/* tslint:disable */
/**
 * This is an autogenerated file created by the Stencil compiler.
 * It contains typing information for all components that exist in this project.
 */
import { HTMLStencilElement, JSXBase } from "@stencil/core/internal";
import { CheckboxQuestion, IQuestionComponent, RadioQuestion, TextAreaQuestion } from "./models/custom-component-models";
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from "./models/elsa-interfaces";
export namespace Components {
    interface ElsaCheckboxQuestion {
        "question": CheckboxQuestion;
    }
    interface ElsaQuestion {
        "question": IQuestionComponent;
    }
    interface ElsaQuestionScreen {
        "activityModel": ActivityModel;
        "propertyDescriptor": ActivityPropertyDescriptor;
        "propertyModel": ActivityDefinitionProperty;
    }
    interface ElsaRadioQuestion {
        "question": RadioQuestion;
    }
    interface ElsaTextareaQuestion {
        "question": TextAreaQuestion;
    }
}
export interface ElsaCheckboxQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaCheckboxQuestionElement;
}
export interface ElsaQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaQuestionElement;
}
export interface ElsaRadioQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaRadioQuestionElement;
}
export interface ElsaTextareaQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaTextareaQuestionElement;
}
export interface ElsaCheckboxQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaCheckboxQuestionElement;
}
export interface ElsaQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaQuestionElement;
}
export interface ElsaRadioQuestionCustomEvent<T> extends CustomEvent<T> {
    detail: T;
    target: HTMLElsaRadioQuestionElement;
}
declare global {
    interface HTMLElsaCheckboxQuestionElement extends Components.ElsaCheckboxQuestion, HTMLStencilElement {
    }
    var HTMLElsaCheckboxQuestionElement: {
        prototype: HTMLElsaCheckboxQuestionElement;
        new (): HTMLElsaCheckboxQuestionElement;
    };
    interface HTMLElsaQuestionElement extends Components.ElsaQuestion, HTMLStencilElement {
    }
    var HTMLElsaQuestionElement: {
        prototype: HTMLElsaQuestionElement;
        new (): HTMLElsaQuestionElement;
    };
    interface HTMLElsaQuestionScreenElement extends Components.ElsaQuestionScreen, HTMLStencilElement {
    }
    var HTMLElsaQuestionScreenElement: {
        prototype: HTMLElsaQuestionScreenElement;
        new (): HTMLElsaQuestionScreenElement;
    };
    interface HTMLElsaRadioQuestionElement extends Components.ElsaRadioQuestion, HTMLStencilElement {
    }
    var HTMLElsaRadioQuestionElement: {
        prototype: HTMLElsaRadioQuestionElement;
        new (): HTMLElsaRadioQuestionElement;
    };
    interface HTMLElsaTextareaQuestionElement extends Components.ElsaTextareaQuestion, HTMLStencilElement {
    }
    var HTMLElsaTextareaQuestionElement: {
        prototype: HTMLElsaTextareaQuestionElement;
        new (): HTMLElsaTextareaQuestionElement;
    };
    interface HTMLElementTagNameMap {
        "elsa-checkbox-question": HTMLElsaCheckboxQuestionElement;
        "elsa-question": HTMLElsaQuestionElement;
        "elsa-question-screen": HTMLElsaQuestionScreenElement;
        "elsa-radio-question": HTMLElsaRadioQuestionElement;
        "elsa-textarea-question": HTMLElsaTextareaQuestionElement;
    }
}
declare namespace LocalJSX {
    interface ElsaCheckboxQuestion {
        "onUpdateQuestion"?: (event: ElsaCheckboxQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: CheckboxQuestion;
    }
    interface ElsaQuestion {
        "onUpdateQuestion"?: (event: ElsaQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: IQuestionComponent;
    }
    interface ElsaQuestionScreen {
        "activityModel"?: ActivityModel;
        "propertyDescriptor"?: ActivityPropertyDescriptor;
        "propertyModel"?: ActivityDefinitionProperty;
    }
    interface ElsaRadioQuestion {
        "onUpdateQuestion"?: (event: ElsaRadioQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: RadioQuestion;
    }
    interface ElsaTextareaQuestion {
        "onUpdateQuestion"?: (event: ElsaTextareaQuestionCustomEvent<IQuestionComponent>) => void;
        "question"?: TextAreaQuestion;
    }
    interface IntrinsicElements {
        "elsa-checkbox-question": ElsaCheckboxQuestion;
        "elsa-question": ElsaQuestion;
        "elsa-question-screen": ElsaQuestionScreen;
        "elsa-radio-question": ElsaRadioQuestion;
        "elsa-textarea-question": ElsaTextareaQuestion;
    }
}
export { LocalJSX as JSX };
declare module "@stencil/core" {
    export namespace JSX {
        interface IntrinsicElements {
            "elsa-checkbox-question": LocalJSX.ElsaCheckboxQuestion & JSXBase.HTMLAttributes<HTMLElsaCheckboxQuestionElement>;
            "elsa-question": LocalJSX.ElsaQuestion & JSXBase.HTMLAttributes<HTMLElsaQuestionElement>;
            "elsa-question-screen": LocalJSX.ElsaQuestionScreen & JSXBase.HTMLAttributes<HTMLElsaQuestionScreenElement>;
            "elsa-radio-question": LocalJSX.ElsaRadioQuestion & JSXBase.HTMLAttributes<HTMLElsaRadioQuestionElement>;
            "elsa-textarea-question": LocalJSX.ElsaTextareaQuestion & JSXBase.HTMLAttributes<HTMLElsaTextareaQuestionElement>;
        }
    }
}
