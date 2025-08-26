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
  @ViewChild('expressionEditor') expressionEditor: ExpressionEditor;

  cases: Array<SwitchCase> = [];
  case: SwitchCase;
  valueChange: EventEmitter<Array<any>>;
  supportedSyntaxes: Array<string> = [SyntaxNames.Liquid, SyntaxNames.JavaScript];
  syntaxSwitchCount: number = 0;
  
  defaultSyntax = SyntaxNames.Json
  isEncypted = model<boolean>(false);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax] || '');
  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  isReadOnly = computed(() => this.propertyDescriptor()?.isReadOnly ?? false);
  monacoLanguage = computed(() => mapSyntaxToLanguage(this.defaultSyntax));
  multiExpressionEditorExpressions = {Json: SyntaxNames.Json};

  context: IntellisenseContext = {
      activityTypeName: null,
      propertyName: null
    };

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
    const propertyModel = this.propertyModel();
    const casesJson = propertyModel?.expressions['Switch'] || '[]';
    this.cases = parseJson(casesJson) || [];
    this.context.activityTypeName = this.activityModel()?.type;
    this.context.propertyName = this.propertyDescriptor()?.name;

    if (this.multiExpressionEditor?.nativeElement) {
      this.multiExpressionEditor.nativeElement.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
    }
  }

  updatePropertyModel() {
    const updatedExpressions = { ...this.propertyModel().expressions };

    updatedExpressions['Switch'] = JSON.stringify(this.cases);

    // Update the property model
    this.propertyModel.set({
      ...this.propertyModel(),
      expressions: updatedExpressions
    });

    // Update the activity model
    this.activityModel.set({
      ...this.activityModel(),
      properties: [
        {
          ...this.activityModel().properties[0],
          expressions: updatedExpressions
        },
        ...this.activityModel().properties.slice(1)
      ]
    });

    if (this.multiExpressionEditor?.nativeElement) {
      this.multiExpressionEditor.nativeElement.expressions[SyntaxNames.Json] = JSON.stringify(this.cases, null, 2);
    }
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.cases = e.detail;
  }
  
  onAddCaseClick() {
    const caseName = `Case ${this.cases.length + 1}`;
    this.case = { name: caseName, expressions: { [SyntaxNames.JavaScript]: '' }, syntax: SyntaxNames.JavaScript };
    this.cases = [...this.cases, this.case];
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
     switchCase.expressions[switchCase.syntax] = newValue;
     this.updatePropertyModel();
  }

  onCaseSyntaxChanged(e: Event, switchCase: SwitchCase, expressionEditor: ExpressionEditor) {
    const select = e.currentTarget as HTMLSelectElement;
    switchCase.syntax = select.value;
    expressionEditor.language = mapSyntaxToLanguage(switchCase.syntax);
    this.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: Event) {
    const detail = (e as CustomEvent<string>).detail;
    const json = detail;
    const parsed = parseJson(json);

    if (!parsed || !Array.isArray(parsed))
      return;

    const updatedExpressions = { ...this.propertyModel().expressions };
    updatedExpressions['Switch'] = json;

    this.propertyModel.set({
      ...this.propertyModel(),
      expressions: updatedExpressions
    });

    this.cases = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: Event) {
    this.syntaxSwitchCount++;
  }

  isObject(item: any) {
    typeof item == 'object';
  }
}
