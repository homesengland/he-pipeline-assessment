import { Component, Input, model, OnChanges, OnInit, output, signal, SimpleChanges } from "@angular/core";
import { HTMLMonacoElement, MonacoValueChangedArgs } from "../../../models/monaco-elements";
import { IntellisenseContext } from "../../../models";
import { IntellisenseGatherer } from "../../../utils/intellisense-gatherer";
import { Uri } from '../../../constants/constants';
import { selectJavaScriptTypeDefinitions, selectWorkflowDefinitionId } from '../../state/selectors/app.state.selectors';
import { Store } from "@ngrx/store";
import { MonacoEditor } from "../../controls/monaco/monaco-editor";

@Component({
    selector: 'expression-editor',
    template: './expression-editor.html',
    imports: [MonacoEditor]
})
export class ExpressionEditor implements OnInit {


    language = model<string>();
    expression = model<string>();
    editorHeight = model<string>('6em');
    singleLineMode = model<boolean>(false);
    padding = model<string>();
    context? = model<IntellisenseContext>();
    serverUrl = model<string>();
    workflowDefinitionId = model<string>();
    expressionChanged = output<string>();

  intellisenseGatherer: IntellisenseGatherer;
  monacoEditor: HTMLMonacoElement;
    constructor(private store: Store) {
        this.workflowDefinitionId.set(selectWorkflowDefinitionId(this.store))
        this.intellisenseGatherer = new IntellisenseGatherer(this.store);

  }

    async ngOnInit() {
        let libSource: string = selectJavaScriptTypeDefinitions(this.store);
        const libUri = Uri.LibUri;

        await this.monacoEditor.addJavaScriptLib(libSource, libUri);
        this.setExpression(this.expression());
  }

  //ngOnChanges(changes: SimpleChanges) {

  //  if (changes["expression"].currentValue != changes["expression"].previousValue) {
  //    this.expressionChangedHandler(changes["expression"].currentValue)
  //  }

  //}

  //expressionChangedHandler(newValue: string) {
  //  this.currentExpression.set(newValue);
  //}

  async setExpression(value: string) {
    await this.monacoEditor.setValue(value);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.expression.set(e.value);
   /* await this.expressionChanged.emit(e.value);*/
  }
}
