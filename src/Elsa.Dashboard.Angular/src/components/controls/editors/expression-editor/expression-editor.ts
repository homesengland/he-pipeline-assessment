import { Component, Input, output, SimpleChanges } from "@angular/core";
import { HTMLMonacoElement, MonacoValueChangedArgs } from "../../../../models/monaco-elements";
import { IntellisenseContext } from "../../../../models";
import { IntellisenseGatherer } from "../../../../utils/intellisense-gatherer";
import { Uri } from '../../../../constants/constants';
import { javascriptTypeDefinitions, workflowDefinitionId } from '../../../state/selectors/app.state.selectors';
import { Store } from "@ngrx/store";

@Component({
  selector: 'expression-editor',
  template: './expression-editor.html'
})
export class ExpressionEditor {


  @Input() language: string;
  @Input() expression: string;
  @Input() editorHeight: string = '6em';
  @Input() singleLineMode: boolean = false;
  @Input() padding: string;
  @Input() context?: IntellisenseContext;
  @Input() serverUrl: string;
  @Input() workflowDefinitionId: string;
  currentExpression?: string
  expressionChanged = output<string>();

  intellisenseGatherer: IntellisenseGatherer;
  monacoEditor: HTMLMonacoElement;
  constructor(private store: Store) {
    this.currentExpression = this.expression;
  }

  async ngInit() {
    this.workflowDefinitionId = javascriptTypeDefinitions(this.store);
    this.currentExpression = this.expression;
    this.intellisenseGatherer = new IntellisenseGatherer();

    let libSource: string = workflowDefinitionId(this.store);
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
