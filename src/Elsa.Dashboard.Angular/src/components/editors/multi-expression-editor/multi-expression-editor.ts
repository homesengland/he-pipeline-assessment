import { Component, EventEmitter, HostListener, Input, output, signal, computed, OnInit, OnChanges } from "@angular/core";
import { IntellisenseContext } from "../../../models";
import { SyntaxNames } from "../../../constants/constants";
import { enter, leave, toggle } from 'el-transition';
import { Map, mapSyntaxToLanguage } from '../../../utils/utils'
import { HTMLExpressionEditorElement } from "../../../models/monaco-elements";
import { ExpressionEditor } from "../expression-editor/expression-editor";


@Component({
  selector: 'multi-expression-editor',
    template: './multi-expression-editor.html',
    imports: [ExpressionEditor]
})
export class MultiExpressionEditor implements OnInit, OnChanges {

  @Input() label: string;
  @Input() fieldName?: string;
  @Input() syntax?: string;
  @Input() defaultSyntax: string = SyntaxNames.Literal;
  @Input() expressions: Map<string> = {};
  @Input() supportedSyntaxes: Array<string> = [];
  @Input() isReadOnly?: boolean;
  @Input() editorHeight: string = '10em';
  @Input() singleLineMode: boolean = false;
  @Input() context?: IntellisenseContext;
  @Input() hint?: string;

  syntaxChanged = output<string>();
  expressionChanged = output<string>();

  selectedSyntax?;
  currentValue?;

  contextMenu: HTMLElement;
  expressionEditor: HTMLExpressionEditorElement;
  defaultSyntaxValue: string;
  contextMenuWidget: HTMLElement;
  monacoLanguage;
  

  //Styling computed Variables
  fieldId;
  fieldLabel;
  expressionEditorClass;
  defaultEditorClass;
  advancedButtonClass;

  constructor() {
    this.fieldId = signal(this.fieldName);
    this.fieldLabel = signal( this.label || this.fieldId);
    this.selectedSyntax = signal(this.syntax);
    this.currentValue = computed(() => this.expressions[this.selectedSyntax ? this.selectedSyntax : this.defaultSyntax]);
    this.expressionEditorClass = computed(() => this.selectedSyntax ? 'block' : 'hidden');
    this.defaultEditorClass = computed(() => this.selectedSyntax ? 'hidden' : 'block');
    this.advancedButtonClass = computed(() => this.selectedSyntax ? 'elsa-text-blue-500' : 'elsa-text-gray-300');
    this.monacoLanguage = computed(() => mapSyntaxToLanguage(this.selectedSyntax));

  }

  ngOnInit() {

  }

  ngOnChanges() {

  }

  @HostListener(
    'click', ['$event.target'])
  onWindowClicked(event: Event) {
    const target = event.target as HTMLElement;

    if (!this.contextMenuWidget || !this.contextMenuWidget.contains(target))
      this.closeContextMenu();
  }

  toggleContextMenu() {
    toggle(this.contextMenu);
  }

  openContextMenu() {
    enter(this.contextMenu);
  }

  closeContextMenu() {
    if (!!this.contextMenu)
      leave(this.contextMenu);
  }

  selectDefaultEditor(e: Event) {
    e.preventDefault();
    this.selectedSyntax = undefined;
    this.closeContextMenu();
  }

  async selectSyntax(e: Event, syntax: string) {
    e.preventDefault();

    this.selectedSyntax = syntax;
    this.syntaxChanged.emit(syntax);
    this.currentValue = this.expressions[syntax ? syntax : this.defaultSyntax || SyntaxNames.Literal];
    if (this.currentValue) {
      await this.expressionEditor.setExpression(this.currentValue);
    }
    this.closeContextMenu();
  }

  onSettingsClick(e: Event) {
    e = e;
    this.toggleContextMenu();
  }

  onExpressionChanged(e: CustomEvent<string>) {
    const expression = e.detail;
    if (expression) {
      this.expressions[this.selectedSyntax || this.defaultSyntax] = expression;
      this.expressionChanged.emit(expression);
    }
  }

  selectedSyntaxClass(syntax: string) {
    return syntax == this.selectedSyntax ? 'elsa-text-blue-700' : 'elsa-text-gray-700';
  }


}
