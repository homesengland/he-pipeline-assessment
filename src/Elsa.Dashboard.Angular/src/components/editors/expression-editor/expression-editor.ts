import { toSignal } from '@angular/core/rxjs-interop';
import { IntellisenseService } from '../../../services/intellisense-service';
import { Component, ElementRef, input, Input, model, OnChanges, OnInit, output, signal, SimpleChanges, ViewChild, computed, effect, Signal } from '@angular/core';
import { HTMLMonacoElement, MonacoValueChangedArgs } from '../../../models/monaco-elements';
import { IntellisenseContext } from '../../../models';
import { Uri } from '../../../constants/constants';
import { selectJavaScriptTypeDefinitions, selectWorkflowDefinitionId, selectServerUrl } from '../../../store/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { EditorModel } from 'src/components/monaco/types';

@Component({
  selector: 'expression-editor',
  templateUrl: './expression-editor.html',
  standalone: false,
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
  intellisenseGatherer: IntellisenseService;
  options: any;
  theme: string = 'vs';
  model: Signal<EditorModel> = computed(() => { return { language: this.language(), value: this.expression() } });
  @ViewChild('monacoContainer') monacoEditor!: HTMLMonacoElement;

  private isUpdating = false;

  constructor(private store: Store) {
    this.store.select(selectWorkflowDefinitionId).subscribe(workflowDefinitionId => {
      if (workflowDefinitionId != null) {
        this.workflowDefinitionId.set(workflowDefinitionId);
      }
    });
    this.options = this.getOptions();
    console.log("Expression Editor - getting Options", this.options);
  }

  async ngOnInit() {
    this.intellisenseGatherer = new IntellisenseService(this.store);
    this.store.select(selectServerUrl).subscribe(data => {
      this.serverUrl.set(data);
    });
  }

  async ngAfterViewInit() {
    this.store.select(selectJavaScriptTypeDefinitions).subscribe(libSource => {
      this.libSource.set(libSource);
    });
  }

  getOptions(): any {
    const defaultOptions = {
      value: this.expression(),
      language: this.language(),
      fontFamily: 'Roboto Mono, monospace',
      renderLineHighlight: 'none',
      minimap: {
        enabled: false,
      },
      automaticLayout: true,
      lineNumbers: 'on',
      theme: 'vs',
      roundedSelection: true,
      scrollBeyondLastLine: false,
      readOnly: false,
      overviewRulerLanes: 0,
      overviewRulerBorder: false,
      lineDecorationsWidth: 0,
      hideCursorInOverviewRuler: true,
      glyphMargin: false,
    };

    let options = defaultOptions;

    if (this.singleLineMode()) {
      options = {
        ...defaultOptions,
        ...{
          wordWrap: 'off',
          lineNumbers: 'off',
          lineNumbersMinChars: 0,
          folding: false,
          scrollBeyondLastColumn: 0,
          scrollbar: { horizontal: 'hidden', vertical: 'hidden' },
          find: { addExtraSpaceOnTop: false, autoFindInSelection: 'never', seedSearchStringFromSelection: false },
        },
      };
    }
    return options;
  }

  async setExpression(value: string) {
    await this.monacoEditor.setValue(value);
  }

  async setLanguage(language: string) {
    await this.monacoEditor.setLanguage(language);
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    console.log("Monaco Value Changed");
     this.expression.set(e.value);
     await this.expressionChanged.emit(e.value);
  }

  async onMonacoInit(e: MonacoValueChangedArgs) {
    const libUri = Uri.LibUri;
    await this.monacoEditor.addJavaScriptLib(this.libSource(), libUri);
    this.setExpression(this.expression());
  }
}
