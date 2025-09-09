import { Component, EventEmitter, Output, model, ViewChild } from '@angular/core';
import { ActivityModel } from '../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor } from '../../../models/domain';
import { HTMLElsaMultiExpressionEditorElement, HTMLElsaExpressionEditorElement, IntellisenseContext } from '../../../models/elsa-interfaces';
import { NestedActivityDefinitionProperty } from '../../../models/custom-component-models';
import { SyntaxNames } from '../../../constants/constants';
import { SortableComponent } from 'src/components/sortable-component';
import { DisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson } from '../../../utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from '../../../Models/constants';
import { MultiExpressionEditor } from '../../editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from '../../editors/expression-editor/expression-editor';

@Component({
  selector: 'he-radio-option-property',
  templateUrl: './he-radio-option-property.html',
  standalone: false,
})
export class HeRadioOptionProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  modelSyntax: string = SyntaxNames.Json;
  keyId: string = '1234'; // Setting a default keyId number since the original code doesn't seem to specify a keyId at all hence default is probably null
  properties: NestedActivityDefinitionProperty[] = ;
  activityIconProvider: any;

  _base: SortableComponent;
  _toggle: DisplayToggle;

  @Output() expressionChanged = new EventEmitter<string>();

  dictionary: { [key: string]: any } = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  supportedSyntaxForMultiExpressionEditor = [SyntaxNames.Json];
  syntaxSwitchCount: number = 0;
  container: HTMLElement;
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';
  defaultSyntax = SyntaxNames.Json;
  
  context: IntellisenseContext = {
    activityTypeName: this.activityModel().type,
    propertyName: this.propertyDescriptor().name
  };

  getExpressions() {
    return { 'Json': JSON.stringify(this.properties ?? [], null, 2) };
  }

  @ViewChild('multiExpressionEditor') multiExpressionEditor: MultiExpressionEditor;
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

    this._base.componentWillLoad();
    // this._base.componentDidLoad(); **** // TODO: Presumed this wasn't needed as it's a stencil lifecycle hook and ngOnInit should handle initialisation logic.
    this._base.componentWillRender();
  }

  // THIS COPIED FUNCTION DOES NOT APPEAR TO BE USED IN THE ORIGINAL
  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.properties = e.detail;
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

    if (!parsed)
      return;

    if (!Array.isArray(parsed))
      return;

    this.propertyModel().expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index, this);
  }

  //updateName(event: Event, option: NestedActivityDefinitionProperty) {
  //  const input = event.target as HTMLInputElement;
  //  option.name = input.value;
  //  this._base.updatePropertyModel();
  //}

  //updateSyntax(event: Event, option: NestedActivityDefinitionProperty, expressionEditor: ExpressionEditor) {
  //  const select = event.target as HTMLSelectElement;
  //  option.syntax = select.value;
  //  this._base.updatePropertyModel();
  //}

  //customUpdateExpression(value: string, option: NestedActivityDefinitionProperty, syntax: string) {
  //  option.expressions[syntax] = value;
  //  this._base.updatePropertyModel();
  //}

  getRenderCaseEditor(): any {

    const cases = this.properties;

    cases.map((radioOption: NestedActivityDefinitionProperty, index: number) => {

      const expression = radioOption.expressions[radioOption.syntax];
      const syntax = radioOption.syntax;
      const monacoLanguage = mapSyntaxToLanguage(syntax);
      const prePopulatedSyntax = SyntaxNames.JavaScript;
      const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];

      const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);

      let expressionEditor = null;
      let prePopulatedExpressionEditor = null;
      let colWidth = "100%";
      const optionsDisplay = this._toggle.component.dictionary[index] ?? "none";

      return {
        radioOption: radioOption,
        syntaxSwitchCount: this.syntaxSwitchCount,
        keyId: this.keyId,
        index: index,
        key: `expression-editor-${index}-${this.syntaxSwitchCount}_${this.keyId}`,
        monacoLanguage: monacoLanguage,
        prePopulatedExpression: prePopulatedExpression,
        prePopulatedLanguage: prePopulatedLanguage
      };
    });
  }
}
