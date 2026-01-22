export interface SelectOption {
  value: string;
  text: string;
}
export declare class FormContext {
  constructor(model: any, updater: (model: any) => any);
  model: any;
  updater: (model: any) => any;
}
export declare function textInput(context: FormContext, fieldName: string, label: string, value: string, hint?: string, fieldId?: string, readonlyField?: boolean): any;
export declare function checkBox(context: FormContext, fieldName: string, label: string, checked: boolean, hint?: string, fieldId?: string): any;
export declare function textArea(context: FormContext, fieldName: string, label: string, value: string, hint?: string, fieldId?: string): any;
export declare function selectField(context: FormContext, fieldName: string, label: string, value: string, options: Array<SelectOption>, hint?: string, fieldId?: string): any;
export declare function section(title: string, subTitle?: string): any;
export declare function updateField<T>(context: FormContext, fieldName: string, value: T): void;
export declare function onTextInputChange(e: Event, context: FormContext): void;
export declare function onTextAreaChange(e: Event, context: FormContext): void;
export declare function onCheckBoxChange(e: Event, context: FormContext): void;
export declare function onSelectChange(e: Event, context: FormContext): void;
