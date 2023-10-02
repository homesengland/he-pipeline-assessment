import { Component, h, Event, EventEmitter, Prop, State } from '@stencil/core';
import {
  ActivityDefinitionProperty,
  ActivityModel,
  ActivityPropertyDescriptor,
  HTMLElsaExpressionEditorElement,
  HTMLElsaMultiExpressionEditorElement,
  IntellisenseContext
} from "../../../models/elsa-interfaces";
import { mapSyntaxToLanguage, parseJson, newOptionNumber } from "../../../utils/utils";
import { SwitchCase } from "../../../models/elsa-interfaces";
import { IconProvider } from "../../providers/icon-provider/icon-provider";
import PlusIcon from '../../../icons/plus_icon';
import TrashCanIcon from '../../../icons/trash-can';
import { SyntaxNames, ValidationSyntax } from '../../../constants/constants';
import { getUniversalUniqueId } from '../../../utils/utils';

@Component({
  tag: 'he-switch-answers-property',
  shadow: false,
})
//Copy of Elsa Switch Case
//Copied to allow us control over how the expression editor is displayed.
export class HeValidationProperty {

  @Prop() activityModel: ActivityModel;
  @Prop() propertyDescriptor: ActivityPropertyDescriptor;
  @Prop() propertyModel: ActivityDefinitionProperty;
  @Prop() keyId: string;
  @State() cases: Array<SwitchCase> = [];
  @State() iconProvider = new IconProvider();
  @Event() expressionChanged: EventEmitter<string>;


  @State() switchTextHeight: string = "";

  @State() editorHeight: string = "2.75em"

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;

  async componentWillLoad() {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel.expressions[SyntaxNames.Switch]
    this.cases = parseJson(casesJson) || [];
  }

  async componentWillRender() {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel.expressions[SyntaxNames.Switch]
    this.cases = parseJson(casesJson) || [];
    this.keyId = getUniversalUniqueId();
  }

  updatePropertyModel() {
    this.propertyModel.expressions[SyntaxNames.Switch] = JSON.stringify(this.cases);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.cases = e.detail;
  }

  onAddCaseClick() {
    const caseIds = this.caseIds();
    const id = newOptionNumber(caseIds);
    const caseName = `Case ${id}`;
    const newCase = { name: caseName, syntax: SyntaxNames.JavaScript, expressions: { [SyntaxNames.JavaScript]: '' } };
    this.cases = [...this.cases, newCase];
    this.updatePropertyModel();
  }

  caseIds(): Array<number> {
    let activityIds: Array<number> = [];
    if (this.cases.length > 0) {
      activityIds = this.cases.map(function (v) {
        const caseNumberMatch = v.name.match(/^Case\s(\d+$)/);
        if (caseNumberMatch != null) {
           return parseInt(caseNumberMatch[1]);
        }
        return 0;
      });
    }
    return activityIds;
  }

  ruleForDescriptor(): ActivityPropertyDescriptor {
    let desc = {

    };
    return desc as ActivityPropertyDescriptor;
  }

  errorMessageDescriptor(): ActivityPropertyDescriptor {
    let desc = {

    };
    return desc as ActivityPropertyDescriptor;
  }


  onCaseNameChanged(e: Event, switchCase: SwitchCase) {
    switchCase.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }


  onCaseExpressionChanged(e: CustomEvent<string>, switchCase: SwitchCase) {
    switchCase.expressions[switchCase.syntax] = e.detail;
    this.updatePropertyModel();
  }

  onCheckChanged(e: Event) {
    const checkbox = (e.target as HTMLInputElement);
    const isChecked = checkbox.checked;
    this.propertyModel.expressions[ValidationSyntax.UseValidation] = isChecked.toString();
    this.expressionChanged.emit(JSON.stringify(this.propertyModel));
  }

  onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: CustomEvent<string>) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel.expressions[SyntaxNames.Switch] = json;
    this.cases = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: CustomEvent<string>) {
    e = e;
    this.syntaxSwitchCount++;
  }

  render() {
    const json = JSON.stringify(this.propertyModel.expressions);
    const supportedSyntaxes = this.supportedSyntaxes;
    const supportedRuleSyntax = [SyntaxNames.JavaScript];
    const useValidation = this.propertyModel.expressions[ValidationSyntax.UseValidation];
    const validationTitle = this.propertyModel.expressions[this.propertyModel.syntax];
    const validationRule = this.propertyModel.expressions[ValidationSyntax.ValidationRule];
    const ruleDescriptor = this.ruleForDescriptor();
    const errorDescriptor = this.errorMessageDescriptor();
    const isReadOnly: boolean = false;

    const context: IntellisenseContext = {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name
    };

    return (
      <div>
        <he-multi-expression-editor
          ref={el => this.multiExpressionEditor = el}
          label={this.propertyDescriptor.label}
          defaultSyntax={SyntaxNames.Json}
          supportedSyntaxes={[SyntaxNames.Json]}
          context={context}
          expressions={{ 'Json': json }}
          editor-height="20rem"
          onExpressionChanged={e => this.onMultiExpressionEditorValueChanged(e)}
          onSyntaxChanged={e => this.onMultiExpressionEditorSyntaxChanged(e)}
        >

        <he-property-editor
          key={`property-editor-${fieldId}-${this.keyId}`}
          activityModel={this.activityModel}
          propertyDescriptor={propertyDescriptor}
          propertyModel={propertyModel}
          onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
          single-line={true}
          showLabel={false}>
          <div class="elsa-max-w-lg">
            <div class="elsa-relative elsa-flex elsa-items-start">
              <div class="elsa-flex elsa-items-center elsa-h-5">
                <input id={fieldId} name={fieldName} type="checkbox" checked={isChecked} value={'true'}
                  onChange={e => this.onCheckChanged(e)}
                  class="focus:elsa-ring-blue-500 elsa-h-4 elsa-w-4 elsa-text-blue-600 elsa-border-gray-300 elsa-rounded" />
              </div>
              <div class="elsa-ml-3 elsa-text-sm">
                <label htmlFor={fieldId} class="elsa-font-medium elsa-text-gray-700">{fieldLabel}</label>
              </div>
            </div>
          </div>
        </he-property-editor>

        <br/>
          <he-property-editor
            key={`property-editor-${fieldId}-${this.keyId}`}
            activityModel={this.activityModel}
            propertyDescriptor={errorDescriptor}
            propertyModel={this.propertyModel}
            onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
            editor-height="100%"
            single-line={true}>
            <input type="text" id={fieldId} name={fieldName} value={value} onChange={e => this.onChange(e)}
              class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
              disabled={isReadOnly} />
        </he-property-editor>
        <br/>
        <he-property-editor
          key={`property-editor-${fieldId}-${this.keyId}`}
          activityModel={this.activityModel}
          propertyDescriptor={ruleDescriptor}
          propertyModel={this.propertyModel}
          onDefaultSyntaxValueChanged={e => this.onDefaultSyntaxValueChanged(e)}
          editor-height="100%"
          single-line={true}>
          <input type="text" id={fieldId} name={fieldName} value={value} onChange={e => this.onChange(e)}
            class="disabled:elsa-opacity-50 disabled:elsa-cursor-not-allowed focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300"
            disabled={isReadOnly} />
          </he-property-editor>

         </he-multi-expression-editor>
      </div>
    );
  }
}
