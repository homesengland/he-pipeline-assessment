import { Component, EventEmitter, Output, model, ViewChild, Input, output, input, SimpleChanges, OnInit } from '@angular/core';

import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../../models/domain';
import { HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement, IntellisenseContext } from '../../../../models/elsa-interfaces';
import { ActivityModel } from '../../../../models/view';
import { NestedActivityDefinitionProperty } from '../../../../models/custom-component-models';
import { SyntaxNames } from '../../../../constants/constants';
import { ISortableSharedComponent, SortableComponent } from 'src/components/sortable-component';
import { DisplayToggle, IDisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson } from '../../../../utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from '../../../../models/constants';
import { MultiExpressionEditor } from '../../../editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../../../editors/expression-editor/expression-editor';

@Component({
  selector: 'radio-option-property',
  templateUrl: './radio-option-property.html',
  standalone: false,
})
export class RadioOptionProperty implements ISortableSharedComponent, IDisplayToggle, OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  modelSyntax: string = SyntaxNames.Json;
  keyId: string;
  properties: NestedActivityDefinitionProperty[] = [];
  json: string = '';

  _base: SortableComponent;
  _toggle: DisplayToggle;

  activityIconProvider: ActivityIconProvider;
  expressionChanged = output<string>();
  dictionary: { [key: string]: any } = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';
  container: HTMLElement;

  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;

  // defaultSyntax: string = SyntaxNames.Json;
  defaultSyntax = SyntaxNames.Json;
  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];

  supportedSyntaxForMultiExpressionEditor = [SyntaxNames.Json];

  stringifiedHardCodedJsonDataForExpressionPropertyInMultiExpressionEditor = {
    Json: "[{\"name\":\"A\",\"syntax\":\"JavaScript\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"true\"},\"type\":\"radio\"},{\"name\":\"B\",\"syntax\":\"Literal\",\"expressions\":{\"Literal\":\"\",\"PrePopulated\":\"false\"},\"type\":\"radio\"}]"
  };

  syntaxSwitchCount: number = 0;

  //context: IntellisenseContext = {
  //  activityTypeName: this.activityModel().type,
  //  propertyName: this.propertyDescriptor().name,
  //};
  context: IntellisenseContext;

  getExpressions() {
    return { Json: JSON.stringify(this.properties ?? [], null, 2) };
  }

  @ViewChild('expressionEditor') expressionEditor: ExpressionEditor;
  @ViewChild('prePopulatedExpressionEditor') prePopulatedExpressionEditor: ExpressionEditor;

  radioOptionsSyntaxPrePopulated = RadioOptionsSyntax.PrePopulated;
  onlyJavaScriptSyntaxes: string[] = this.supportedSyntaxes.filter(x => x === SyntaxNames.JavaScript);

  constructor(activityIconProvider: ActivityIconProvider) {
    this.activityIconProvider = activityIconProvider;
    this._base = new SortableComponent();
    this._toggle = new DisplayToggle();
  }

  ngOnInit() {
    this._base.onComponentInitialised();

    // Safely initialize context after models are available
    this.context = {
      activityTypeName: this.activityModel()?.type ?? '',
      propertyName: this.propertyDescriptor()?.name ?? '',
    };
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes['properties']) {
      this.updateJsonExpressionsVariable();
    }
  }

  onAddOptionClick() {
    const optionName = newOptionLetter(this._base.IdentifierArray());
    const newOption: NestedActivityDefinitionProperty = {
      name: optionName,
      syntax: SyntaxNames.Literal,
      expressions: { [SyntaxNames.Literal]: '', [RadioOptionsSyntax.PrePopulated]: 'false' },
      type: PropertyOutputTypes.Radio,
    };
    this.properties = [...this.properties, newOption];
    this._base.updatePropertyModel();
  }

  onDeleteOptionClick(switchCase: NestedActivityDefinitionProperty) {
    this.properties = this.properties.filter(x => x != switchCase);
    this._base.updatePropertyModel();
  }

  onMultiExpressionEditorValueChanged(e: any) {
    const json = e.detail;
    const parsed = parseJson(json);

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel().expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index);
  }

  updateJsonExpressionsVariable() {
    this.json = JSON.stringify(this.properties, null, 2);
  }

  getRenderCaseEditor(): any {
    const cases = this.properties;

    return cases.map((radioOption: NestedActivityDefinitionProperty, index: number) => {
      const expression = radioOption.expressions[radioOption.syntax];
      const syntax = radioOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);

      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let colWidth = '100%';
      const optionsDisplay = this._toggle.component.dictionary[index] ?? 'none';

      return {
        radioOption: radioOption,
        syntaxSwitchCount: this.syntaxSwitchCount,
        keyId: this.keyId,
        index: index,
        key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.keyId}`,
        monacoLanguage: monacoLanguage,
        prePopulatedExpression: prePopulatedExpression,
        prePopulatedLanguage: prePopulatedLanguage,
        optionsDisplay: optionsDisplay,
        expression: expression,
      };
    });
  }
}
