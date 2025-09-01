import { Component, EventEmitter, Output } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, HTMLElsaMultiExpressionEditorElement, IntellisenseContext } from '../../../models/elsa-interfaces';
import { NestedActivityDefinitionProperty } from '../../../models/custom-component-models';
import { SyntaxNames } from '../../../constants/constants';
import { SortableComponent } from 'src/components/sortable-component';
import { DisplayToggle } from 'src/components/display-toggle.component';
import { mapSyntaxToLanguage, newOptionLetter, parseJson } from 'src/utils/utils';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { PropertyOutputTypes, RadioOptionsSyntax } from 'src/models/constants';

@Component({
  selector: 'he-radio-option-property',
  templateUrl: './he-radio-option-property.html',
  standalone: false,
})
export class HeRadioOptionProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  modelSyntax: string = SyntaxNames.Json;
  keyId: string;
  properties: NestedActivityDefinitionProperty[];
  activityIconProvider: any; // Copied from workflow-instance-journal.ts, need to confirm correct implemenation.

  private _base: SortableComponent;
  private _toggle: DisplayToggle;

  @Output() expressionChanged = new EventEmitter<string>();

  dictionary: { [key: string]: any } = {};

  switchTextHeight: string = '';
  editorHeight: string = '2.75em';

  supportedSyntaxes: Array<string> = [SyntaxNames.JavaScript, SyntaxNames.Liquid, SyntaxNames.Literal];
  multiExpressionEditor: HTMLElsaMultiExpressionEditorElement;
  syntaxSwitchCount: number = 0;
  container: HTMLElement;
  displayValue: string = 'table-row';
  hiddenValue: string = 'none';

  constructor(activityIconProvider: ActivityIconProvider) {
    // Copied from workflow-instance-journal.ts, need to confirm correct implemenation.
    this.activityIconProvider = activityIconProvider; // Copied from workflow-instance-journal.ts, need to confirm correct implemenation.
    this._base = new SortableComponent();
    this._toggle = new DisplayToggle();
  }

  ngOnInit() {
    this._base.componentWillLoad();
    // this._base.componentDidLoad(); **** // TODO: Presumed this wasn't needed as it's a stencil lifecycle hook and ngOnInit should handle initialisation logic.
    this._base.componentWillRender();
  }

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

    if (!parsed) return;

    if (!Array.isArray(parsed)) return;

    this.propertyModel.expressions[SyntaxNames.Json] = json;
    this.properties = parsed;
  }

  onMultiExpressionEditorSyntaxChanged(e: any) {
    this.syntaxSwitchCount++;
  }

  onToggleOptions(index: number) {
    this._toggle.onToggleDisplay(index, this);
  }

  renderCaseEditor(radioOption: NestedActivityDefinitionProperty, index: number) {
    const expression = radioOption.expressions[radioOption.syntax];
    const syntax = radioOption.syntax;
    const monacoLanguage = mapSyntaxToLanguage(syntax);
    const prePopulatedSyntax = SyntaxNames.JavaScript;
    const prePopulatedExpression = radioOption.expressions[RadioOptionsSyntax.PrePopulated];
    const prePopulatedLanguage = mapSyntaxToLanguage(prePopulatedSyntax);
    let colWidth = '100%';
    const optionsDisplay = this._toggle.component.dictionary[index] ?? 'none';

    return {
      radioOption,
      index,
      expression,
      syntax,
      monacoLanguage,
      prePopulatedSyntax,
      prePopulatedExpression,
      prePopulatedLanguage,
      colWidth,
      optionsDisplay,
    };
  }

  get context(): IntellisenseContext {
    return {
      activityTypeName: this.activityModel.type,
      propertyName: this.propertyDescriptor.name,
    };
  }
}
