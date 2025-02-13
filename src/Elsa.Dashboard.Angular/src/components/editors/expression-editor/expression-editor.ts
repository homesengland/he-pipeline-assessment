import { Component, ElementRef, input, Input, model, OnChanges, OnInit, output, signal, SimpleChanges, ViewChild } from "@angular/core";
import { HTMLMonacoElement, MonacoValueChangedArgs } from "../../../models/monaco-elements";
import { IntellisenseContext } from "../../../models";
import { IntellisenseGatherer } from "../../../utils/intellisense-gatherer";
import { Uri } from '../../../constants/constants';
import { selectJavaScriptTypeDefinitions, selectWorkflowDefinitionId } from '../../state/selectors/app.state.selectors';
import { Store } from "@ngrx/store";
import { MonacoEditor } from "../../controls/monaco/monaco-editor";

@Component({
    selector: 'expression-editor',
    templateUrl: './expression-editor.html',
    standalone: false
})
export class ExpressionEditor implements OnInit {


    language = model<string>();
    expression = model<string>();
    editorHeight = input<string>('6em');
    singleLineMode = input<boolean>(false);
    padding = input<string>();
    context? = model<IntellisenseContext>();
    serverUrl = model<string>();
    workflowDefinitionId? = model<string>();
    expressionChanged = output<string>();

    intellisenseGatherer: IntellisenseGatherer;
    @ViewChild('monacoContainer') monacoEditor: ElementRef<HTMLMonacoElement>
    constructor(private store: Store) {
      if(selectWorkflowDefinitionId(this.store)  != null){
        this.workflowDefinitionId.set(selectWorkflowDefinitionId(this.store))
      }
        this.intellisenseGatherer = new IntellisenseGatherer(this.store);
    }



    async ngOnInit() {
        //let libSource: string = selectJavaScriptTypeDefinitions(this.store);
        //const libUri = Uri.LibUri;

        //await this.monacoEditor.addJavaScriptLib(libSource, libUri);
        //this.setExpression(this.expression());
    }

    async ngAfterViewInit() {
        console.log(this.monacoEditor.nativeElement);
        let libSource: string = selectJavaScriptTypeDefinitions(this.store);
        const libUri = Uri.LibUri;
        await this.monacoEditor.nativeElement.addJavaScriptLib(libSource, libUri);
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
        await this.monacoEditor.nativeElement.setValue(value);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.expression.set(e.value);
   /* await this.expressionChanged.emit(e.value);*/
  }
}
