import { AfterViewInit, Component, ElementRef, Input, model, output, Renderer2, SimpleChanges, ViewChild } from "@angular/core";
import { initializeMonacoWorker, Monaco, EditorVariables } from "./monaco-utils";
import { selectMonacoLibPath } from '../../state/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { AppState } from '../../state/reducers/app.state.reducers'
import { MonacoValueChangedArgs } from "../../../models/monaco-elements";

@Component({
    selector: 'monaco',
    templateUrl: './monaco.html',
    styleUrls: ['./monaco.css'],
    host: {
        'class': 'elsa-monaco-editor-host elsa-border focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300 elsa-p-4',
        '[style]': 'hostStyle',
    }
})

export class MonacoEditor implements AfterViewInit { 
  private monaco: Monaco;
  monacoLibPath: string;
  valueChanged = output<MonacoValueChangedArgs>();

  editorHeight = model<string>('5em');
  value = model<string>();
  language = model<string>();
  singleLineMode = model<boolean>(false);
  padding = model<string>();

    monacoClass = 'elsa-monaco-editor-container ' + this.padding();
    hostStyle = 'min-height: ' + this.editorHeight();

    constructor(private store: Store, private renderer: Renderer2) {
    this.store.select(selectMonacoLibPath).subscribe(data => {
      this.monacoLibPath = data
    });

    }

    @ViewChild('container') container!: ElementRef<HTMLElement>

    editor: any;



  ngOnChanges(changes: SimpleChanges) {
    this.languageChangeHandler(changes);

  };

  async ngOnInit(): Promise<void> {
    this.monaco = await initializeMonacoWorker(this.monacoLibPath);
      const monaco = this.monaco;
      const language = this.language;

      // Validation settings.
      monaco.languages.typescript.javascriptDefaults.setDiagnosticsOptions({
        noSemanticValidation: true,
        noSyntaxValidation: false,
      });

      // Compiler options.
      monaco.languages.typescript.javascriptDefaults.setCompilerOptions({
        target: monaco.languages.typescript.ScriptTarget.ES2020,
        lib: [],
        allowNonTsExtensions: true
      });

      monaco.languages.typescript.javascriptDefaults.setEagerModelSync(true);

      const defaultOptions = {
        value: this.value,
        language: language,
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

      this.editor = monaco.editor.create(this.container, options);

      this.editor.onDidChangeModelContent(e => {
        e = e;
        const value = this.editor.getValue();
        const markers = monaco.editor.getModelMarkers({ owner: language });
        var event: MonacoValueChangedArgs = { value: value, markers: markers };
        this.valueChanged.emit(event);
      });

      if (this.singleLineMode) {
        this.editor.onKeyDown(e => {
          if (e.keyCode == monaco.KeyCode.Enter) {
            // We only prevent enter when the suggest model is not active
            if (this.editor.getContribution('editor.contrib.suggestController').model.state == 0) {
              e.preventDefault();
            }
          }
        });

        this.editor.onDidPaste(e => {
          if (e.range.endLineNumber > 1) {
            let newContent = "";
            const model = this.editor.getModel();
            let lineCount = model.getLineCount();
            for (let i = 0; i < lineCount; i++) {
              newContent += model.getLineContent(i + 1);
            }
            model.setValue(newContent);
          }
        });
      }
    }

    ngAfterViewInit() {
        console.log(this.container.nativeElement);
    }


  languageChangeHandler(changes: SimpleChanges) {
    if (changes["language"].currentValue != changes["language"].previousValue) {
      var newValue = changes["language"].currentValue;
      if (!this.editor)
        return;

      const model = this.editor.getModel();
      this.monaco.editor.setModelLanguage(model, this.language);
    }
  }

  async setValue(value: string) {
    if (!this.editor)
      return;

    const model = this.editor.getModel();
    model.setValue(value || '');
  }

  async addJavaScriptLib(libSource: string, libUri: string) {
    const monaco = this.monaco;
    monaco.languages.typescript.javascriptDefaults.setExtraLibs([{
      filePath: "lib.es5.d.ts"
    }, {
      content: libSource,
      filePath: libUri
    }]);

    const oldModel = monaco.editor.getModel(libUri);

    if (oldModel)
      oldModel.dispose();


    const matches = libSource.matchAll(/declare const (\w+): (string|number)/g);

    EditorVariables.splice(0, EditorVariables.length);

    for (const match of matches) {
      EditorVariables.push({ variableName: match[1], type: match[2] });
    }

  }

  disconnectedCallback() {
    const editor = this.editor;

    if (!!editor)
      editor.dispose();
  }
}
