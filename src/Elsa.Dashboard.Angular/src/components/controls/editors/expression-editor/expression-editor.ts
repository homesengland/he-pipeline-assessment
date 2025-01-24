import { Component, Input, output, SimpleChanges } from "@angular/core";
import { HTMLMonacoElement, MonacoValueChangedArgs } from "../../../../models/monaco-elements";
import { IntellisenseContext } from "../../../../models";
import { IntellisenseGatherer } from "../../../../utils/intellisense-gatherer";

@Component({
  tag: 'expression-editor',
  template: './expression-editor.html'
})
export class ExpressionEditor {


  @Input() language: string;
  @Input() expression: string;
  @Input({ attribute: 'editor-height', reflect: true }) editorHeight: string = '6em';
  @Input({ attribute: 'single-line', reflect: true }) singleLineMode: boolean = false;
  @Input() padding: string;
  @Input() context?: IntellisenseContext;
  @Input({ mutable: true }) serverUrl: string;
  @Input({ mutable: true }) workflowDefinitionId: string;
  currentExpression?: string
  expressionChanged = output<string>();

  intellisenseGatherer: IntellisenseGatherer;
  monacoEditor: HTMLMonacoElement;
  constructor() {
    this.currentExpression = this.expression;
  }

  async ngInit() {
    this.workflowDefinitionId = state.workflowDefinitionId;
    this.currentExpression = this.expression;
    this.intellisenseGatherer = new IntellisenseGatherer();

    let libSource: string = state.javaScriptTypeDefinitions;
    const libUri = Uri.LibUri;

    await this.monacoEditor.addJavaScriptLib(libSource, libUri);
  }

  ngOnChange(changes: SimpleChanges) {

    if (changes["expression"].currentValue != changes["expression"].previousValue) {
      this.expressionChangedHandler(changes["expression"].currentValue)
    }

  }

  expressionChangedHandler(newValue: string) {
    this.currentExpression = newValue;
  }

  async setExpression(value: string) {
    await this.monacoEditor.setValue(value);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.currentExpression = e.value;
    await this.expressionChanged.emit(e.value);
  }
}
