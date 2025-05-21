import { toSignal } from '@angular/core/rxjs-interop';
import { IntellisenseService } from '../../../services/intellisense-service';
import { Component, ElementRef, input, Input, model, OnChanges, OnInit, output, signal, SimpleChanges, ViewChild } from "@angular/core";
import { HTMLMonacoElement, MonacoValueChangedArgs } from "../../../models/monaco-elements";
import { IntellisenseContext } from "../../../models";
import { Uri } from '../../../constants/constants';
import { selectJavaScriptTypeDefinitions, selectWorkflowDefinitionId } from '../../../store/selectors/app.state.selectors';
import { Store } from "@ngrx/store";

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
    libSource = signal<string>('');
    //monacoEditor: HTMLMonacoElement = null;
    intellisenseGatherer: IntellisenseService;
    options: any;
    theme: string = 'vs';
    @ViewChild('monacoContainer') monacoEditor!: HTMLMonacoElement;

    constructor(private store: Store) {
      this.store.select(selectWorkflowDefinitionId).subscribe((workflowDefinitionId) => {
        if (workflowDefinitionId != null) {
          this.workflowDefinitionId.set(workflowDefinitionId);
        }
      });
      this.options = this.getOptions();
    }



    async ngOnInit() {
      this.intellisenseGatherer = new IntellisenseService(this.store);
        //let libSource: string = selectJavaScriptTypeDefinitions(this.store);
        //const libUri = Uri.LibUri;

        //await this.monacoEditor.addJavaScriptLib(libSource, libUri);
        //this.setExpression(this.expression());
    }

    async ngAfterViewInit() {
        this.store.select(selectJavaScriptTypeDefinitions).subscribe(libSource => {
          this.libSource.set(libSource);
        });
        const libUri = Uri.LibUri;
        console.log("libsource", this.libSource);
        console.log("Element", this.monacoEditor);
        await this.monacoEditor.addJavaScriptLib(this.libSource(), libUri);
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

  getOptions() : any {
    const defaultOptions = {
      value: this.expression,
      language: this.language,
      fontFamily: "Roboto Mono, monospace",
      renderLineHighlight: 'none',
      minimap: {
        enabled: false
      },
      automaticLayout: true,
      lineNumbers: "on",
      theme: "vs",
      roundedSelection: true,
      scrollBeyondLastLine: false,
      readOnly: false,
      overviewRulerLanes: 0,
      overviewRulerBorder: false,
      lineDecorationsWidth: 0,
      hideCursorInOverviewRuler: true,
      glyphMargin: false
    };

    let options = defaultOptions;

    if (this.singleLineMode) {
      options = {
        ...defaultOptions, ...{
          wordWrap: 'off',
          lineNumbers: 'off',
          lineNumbersMinChars: 0,
          folding: false,
          scrollBeyondLastColumn: 0,
          scrollbar: { horizontal: 'hidden', vertical: 'hidden' },
          find: { addExtraSpaceOnTop: false, autoFindInSelection: 'never', seedSearchStringFromSelection: false },
        }
      }
    }
    return options;
  }

    async setExpression(value: string) {
        await this.monacoEditor.setValue(value);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.expression.set(e.value);
   /* await this.expressionChanged.emit(e.value);*/
  }
}
