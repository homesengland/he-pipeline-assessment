import { Component, EventEmitter, HostListener, Input, output, signal, computed, OnInit, OnChanges, input, model, ViewChild, ElementRef, Signal } from '@angular/core';
import { NgFor, CommonModule } from '@angular/common';
import { IntellisenseContext } from '../../../models';
import { SyntaxNames } from '../../../constants/constants';
import { enter, leave, toggle } from 'el-transition';
import { Map, mapSyntaxToLanguage } from '../../../utils/utils';
import { HTMLExpressionEditorElement } from '../../../models/monaco-elements';
import { ExpressionEditor } from '../expression-editor/expression-editor';

@Component({
  selector: 'multi-expression-editor',
  templateUrl: './multi-expression-editor.html',
  standalone: false,
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

  selectedSyntax = signal<string | undefined>(undefined);
  currentValue?: string;

  defaultSyntaxValue: string;
  monacoLanguage;

  @ViewChild('contextMenu') contextMenu: ElementRef<HTMLElement>;
  @ViewChild('contextRef') contextMenuWidget: ElementRef<HTMLElement>;
  @ViewChild('expressionEditorRef') expressionEditor: HTMLExpressionEditorElement;

  //Styling computed Variables
  fieldId;
  fieldLabel;
  expressionEditorClass: Signal<string>;
  defaultEditorClass: Signal<string>;
  advancedButtonClass: Signal<string>;

  constructor() {}

  ngOnInit() {
    this.selectedSyntax.set(this.syntax());
    this.fieldId = computed(() => this.propertyName() || '');
    this.fieldLabel = computed(() => this.label() || this.fieldId());
    this.currentValue = this.getCurrentValue();
    this.expressionEditorClass = computed(() => (this.selectedSyntax() ? 'block' : 'hidden'));
    this.defaultEditorClass = computed<string>(() => (this.selectedSyntax() ? 'hidden' : 'block'));
    this.advancedButtonClass = computed(() => (this.selectedSyntax() ? 'elsa-text-blue-500' : 'elsa-text-gray-300'));
    this.monacoLanguage = computed(() => mapSyntaxToLanguage(this.selectedSyntax()));
  }

  private getCurrentValue() {
    const expressionsMap = this.expressions();
    const syntaxKey = this.selectedSyntax() || this.defaultSyntax();
    return expressionsMap[syntaxKey] || '';
  }

  ngOnChanges() {}

  @HostListener('document:click', ['$event'])
  onWindowClicked(event: Event) {
    const target = event.target as HTMLElement;

    if (!this.contextMenuWidget || !this.contextMenuWidget.nativeElement.contains(target)) {
      this.closeContextMenu();
    }
  }

  toggleContextMenu() {
    toggle(this.contextMenu.nativeElement);
  }

  openContextMenu() {
    enter(this.contextMenu.nativeElement);
  }

  closeContextMenu() {
    if (!!this.contextMenu.nativeElement) leave(this.contextMenu.nativeElement);
  }

  selectDefaultEditor(e: Event) {
    e.preventDefault();
    this.selectedSyntax.set(undefined);
    this.closeContextMenu();
  }

  selectSyntax = async (e: Event, syntax: string) => {
    e.preventDefault();

    this.selectedSyntax.set(syntax);
    this.syntaxChanged.emit(syntax);

    this.currentValue = this.getCurrentValue();
    if (this.currentValue !== null && this.currentValue !== undefined) {
      await this.expressionEditor.setExpression(this.currentValue);
    }
    this.closeContextMenu();
  };

  onSettingsClick(e: Event) {
    e = e;
    this.toggleContextMenu();
  }

  onExpressionChanged(expression: string) {
    if (expression) {
      const syntaxKey = this.selectedSyntax() || this.defaultSyntax();

      this.expressions.update(currentExpressions => {
        return {
          ...currentExpressions,
          [syntaxKey]: expression,
        };
      });

      this.expressionChanged.emit(expression);
    }
  }

  selectedSyntaxClass(syntax: string) {
    return syntax == this.selectedSyntax() ? 'elsa-text-blue-700' : 'elsa-text-gray-700';
  }
}
