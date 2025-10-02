import { MonacoValueChangedArgs } from '../../models/monaco-elements';
import { ChangeDetectionStrategy, Component, forwardRef, inject, Input, output, NgZone } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { fromEvent } from 'rxjs';

import { BaseEditor } from './base-editor';
import { EditorModel } from './types';
import { EditorVariables } from './monaco-utils';
import { has } from 'lodash';

declare var monaco: any;

@Component({
  standalone: false,
  selector: 'monaco-editor',
  templateUrl: 'editor.html',
  styleUrl: 'monaco.css',
  host: {
    '[style.height]': 'editorHeight()',
    '[style.min-height]': 'editorHeight()',
    'class':
      'elsa-monaco-editor-host elsa-border focus:elsa-ring-blue-500 focus:elsa-border-blue-500 elsa-block elsa-w-full elsa-min-w-0 elsa-rounded-md sm:elsa-text-sm elsa-border-gray-300 elsa-p-4',
  },
  changeDetection: ChangeDetectionStrategy.OnPush,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => EditorComponent),
      multi: true,
    },
  ],
})
export class EditorComponent extends BaseEditor implements ControlValueAccessor {
  private zone = inject(NgZone);
  private _value: string = '';

  propagateChange = output<MonacoValueChangedArgs>();
  onTouched = () => {};

  @Input('options')
  set options(options: any) {
    this._options = Object.assign({}, this.config.defaultOptions, options);
    if (this._editor) {
      this._editor.dispose();
      this.initMonaco(options, this.insideNg);
    }
  }

  get options(): any {
    return this._options;
  }

  @Input('model')
  set model(model: EditorModel) {
    if (model.value == null || model.value == undefined) {
      model.value = '';
    }
    this.options.model = model;
    if (this._editor) {
      this._editor.dispose();
      this.initMonaco(this.options, this.insideNg);
    }
  }

  writeValue(value: any): void {
    this._value = value || '';
    // Fix for value change while dispose in process.
    setTimeout(() => {
      if (this._editor && !this.options.model) {
        this._editor.setValue(this._value);
      }
    });
  }

  registerOnChange(fn: any): void {
    let args: MonacoValueChangedArgs = {
      value: this._value,
      markers: this._editor.getModel().getAllMarkers(),
    };
    this.propagateChange.emit(args);
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  protected initMonaco(options: any, insideNg: boolean): void {
    var hasModel: boolean = false;
    if (options && options.model) {
      hasModel = true;
    }

    if (hasModel) {
      const model = monaco.editor.getModel(options.model.uri || '');
      if (model) {
        options.model = model;
        options.model.setValue(this._value);
      } else {
        options.model = monaco.editor.createModel(options.model.value, options.model.language, options.model.uri);
      }
    }

    if (insideNg) {
      this._editor = monaco.editor.create(this._editorContainer.nativeElement, options);
    } else {
      this.zone.runOutsideAngular(() => {
        this._editor = monaco.editor.create(this._editorContainer.nativeElement, options);
      });
    }

    if (!hasModel) {
      this._editor.setValue(this._value);
    }

    this._editor.onDidChangeModelContent((e: any) => {
      const value = this._editor.getValue();

      // value is not propagated to parent when executing outside zone.
      this.zone.run(() => {
        let args: MonacoValueChangedArgs = {
          value: value,
          markers: monaco.editor.getModelMarkers({ resource: this._editor.getModel().uri }),
        };
        this.propagateChange.emit(args);
        this._value = value;
      });
    });

    this._editor.onDidBlurEditorWidget(() => {
      this.onTouched();
    });

    // refresh layout on resize event.
    if (this._windowResizeSubscription) {
      this._windowResizeSubscription.unsubscribe();
    }
    this._windowResizeSubscription = fromEvent(window, 'resize').subscribe(() => this._editor.layout());
    this.onInit.emit(this._editor);
  }

  async addJavaScriptLib(libSource, libUri) {
    monaco.languages.typescript.javascriptDefaults.setExtraLibs([
      {
        filePath: 'lib.es5.d.ts',
      },
      {
        content: libSource,
        filePath: libUri,
      },
    ]);
    //monaco.languages.typescript.javascriptDefaults.setWorkerOptions({
    //  // Set a custom TypeScript worker
    //  customWorkerPath: '/static/assets/monaco-editor/min/vs/language/typescript/tsWorker.js',
    //});
    const oldModel = monaco.editor.getModel(libUri);
    if (oldModel) oldModel.dispose();
    const matches = libSource.matchAll(/declare const (\w+): (string|number)/g);
    EditorVariables.splice(0, EditorVariables.length);
    for (const match of matches) {
      EditorVariables.push({ variableName: match[1], type: match[2] });
    }
  }

  async addJsonLib() {
    monaco.languages.register({ id: 'json' });
  }
}
