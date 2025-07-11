import { Component, computed, EventEmitter, model, OnChanges, OnInit, signal, SimpleChanges, ViewChild } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { Map, mapSyntaxToLanguage, parseJson } from 'src/utils/utils';
import { SwitchCase } from './models';
import { HTMLElsaExpressionEditorElement, HTMLElsaMultiExpressionEditorElement, IntellisenseContext } from 'src/models/elsa-interfaces';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { EditorModel } from '../../../../components/monaco/types';
import { ExpressionEditor } from '../../expression-editor/expression-editor';

@Component({
  selector: 'switch-case-property',
  templateUrl: './switch-case-property.html',
  standalone: false,
})
export class SwitchCaseProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  @ViewChild('multiExpressionEditor') multiExpressionEditor;

  cases: Array<SwitchCase> = [];
  valueChange: EventEmitter<Array<any>>;
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid];
  //multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;



  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  isEncypted = model<boolean>(false);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');
  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  isReadOnly = computed(() => this.propertyDescriptor()?.isReadOnly ?? false);
  monacoLanguage = computed(() => mapSyntaxToLanguage(this.defaultSyntax()));
  // context: IntellisenseContext = {
  //   activityTypeName: this.activityModel().type,
  //   propertyName: this.propertyDescriptor().name
  // };
   expressions = computed(() => {
     const model = this.propertyModel();
     return model?.expressions ?? {};
   });
  //expressions: any;
  Json: any;
  activityIconProvider: any;

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    const propertyModel = this.propertyModel;
    const casesJson = propertyModel().expressions['Switch']
    this.cases = parseJson(casesJson) || [];
    //this.expressions = propertyModel().expressions;
    
  }

  updatePropertyModel() {
    this.propertyModel().expressions['Switch'] = JSON.stringify(this.cases);
    this.multiExpressionEditor.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.cases = e.detail;
  }

  onAddCaseClick() {
    const caseName = `Case ${this.cases.length + 1}`;
    const newCase = {name: caseName, syntax: SyntaxNames.JavaScript, expressions: {[SyntaxNames.JavaScript]: 'test'}};
    this.cases = [...this.cases, newCase];
    this.updatePropertyModel();
  }

  onDeleteCaseClick(switchCase: SwitchCase) {
    this.cases = this.cases.filter(x => x != switchCase);
    this.updatePropertyModel();
  }

  onCaseNameChanged(e: Event, switchCase: SwitchCase) {
    switchCase.name = (e.currentTarget as HTMLInputElement).value.trim();
    this.updatePropertyModel();
  }

  onCaseExpressionChanged(newValue: string, switchCase: SwitchCase) {
    // switchCase.expressions[switchCase.syntax] = newValue;
    // this.updatePropertyModel();
  }

  onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: HTMLElsaExpressionEditorElement) {
    const select = e.currentTarget as HTMLSelectElement;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: Event) {
    const detail = (e as CustomEvent<string>).detail;
    const json = detail;
    const parsed = parseJson(json);

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel().expressions['Switch'] = json;
    this.cases = parsed;

  }

  onMultiExpressionEditorSyntaxChanged(e: Event) {
    this.syntaxSwitchCount++;
  }

  getCases():any {
    return this.cases.map((switchCase, index) => {
      console.log("Index:", index);
      console.log("Name:", switchCase.name);
      console.log("Expressions:", switchCase.expressions);
      console.log("Expression:", switchCase.expressions[switchCase.syntax]);
      console.log("syntax:", switchCase.syntax);
      console.log("monacoLanguage:", mapSyntaxToLanguage(switchCase.syntax));
      return {
        index: index,
        expression: "test",
        syntax: "JavaScript",
        monacoLanguage: "JavaScript",
        name: switchCase.name
      };
    });
  }
}
