import { Component, EventEmitter, HostListener, Input, output, signal, computed, OnInit, OnChanges, input, model, ViewChild, ElementRef } from "@angular/core";
import { NgFor, CommonModule } from '@angular/common';
import { IntellisenseContext } from "../../../models";
import { SyntaxNames } from "../../../constants/constants";
import { enter, leave, toggle } from 'el-transition';
import { Map, mapSyntaxToLanguage } from '../../../utils/utils'
import { HTMLExpressionEditorElement } from "../../../models/monaco-elements";
import { ExpressionEditor } from "../expression-editor/expression-editor";


@Component({
    selector: 'multi-expression-editor',
    templateUrl: './multi-expression-editor.html',
    imports: [ExpressionEditor]
})
export class MultiExpressionEditor implements OnInit, OnChanges {

  defaultMap: Map<string> = {};
  label = input<string>();
  propertyName? = input<string>();
  syntax? = model<string>();
  defaultSyntax = input<string>(SyntaxNames.Literal);
  expressions = model<Map<string>>(this.defaultMap);
  supportedSyntaxes = input<Array<string>>([]);
  isReadOnly? = input<boolean>();
  editorHeight = input<string>('10em');
  singleLineMode = input<boolean>(false);
  context = input<IntellisenseContext>();
  hint? = input<string>();

  syntaxChanged = output<string>();
  expressionChanged = output<string>();

  selectedSyntax?;
  currentValue?;

  defaultSyntaxValue: string;
  monacoLanguage;

    @ViewChild('contextMenu') contextMenu: ElementRef<HTMLElement>;
    @ViewChild('contextRef') contextMenuWidget: ElementRef<HTMLElement>;
    @ViewChild('expressionEdtiorRef') expressionEditor: ElementRef<HTMLExpressionEditorElement>;

  //Styling computed Variables
  fieldId;
  fieldLabel;
  expressionEditorClass;
  defaultEditorClass;
  advancedButtonClass;

    constructor() {
        this.fieldId = signal(this.propertyName);
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

    if (!this.contextMenuWidget || !this.contextMenuWidget.nativeElement.contains(target))
      this.closeContextMenu();
  }

  toggleContextMenu() {
      toggle(this.contextMenu.nativeElement);
  }

  openContextMenu() {
      enter(this.contextMenu.nativeElement);
  }

  closeContextMenu() {
      if (!!this.contextMenu.nativeElement)
      leave(this.contextMenu.nativeElement);
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

    //this.currentValue = this.expressions[syntax ? syntax : this.defaultSyntax || SyntaxNames.Literal];
      if (this.currentValue) {
          await this.expressionEditor.nativeElement.setExpression(this.currentValue);
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
