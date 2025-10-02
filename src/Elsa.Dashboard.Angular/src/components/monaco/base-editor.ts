import { AfterViewInit, Component, ElementRef, EventEmitter, Input, OnDestroy, Output, ViewChild, inject, input, Inject, Optional } from '@angular/core';
import { Subscription } from 'rxjs';
import { MONACO_EDITOR_CONFIG, MonacoEditorConfig } from './config';

let loadedMonaco = false;
let loadPromise: Promise<void>;

@Component({
  template: '',
  standalone: false,
})
export abstract class BaseEditor implements AfterViewInit, OnDestroy {
  constructor(@Optional() @Inject(MONACO_EDITOR_CONFIG) protected config: MonacoEditorConfig) {
    this.config = this.config || {};
  }
  @Input('insideNg')
  set insideNg(insideNg: boolean) {
    this._insideNg = insideNg;
    if (this._editor) {
      this._editor.dispose();
      this.initMonaco(this._options, this.insideNg);
    }
  }
  padding = input<string>('elsa-pt-1.5 elsa-pl-1');
  editorHeight = input<string>('5em');

  get insideNg(): boolean {
    return this._insideNg;
  }

  @ViewChild('editorContainer', { static: true }) _editorContainer: ElementRef;
  @Output() onInit = new EventEmitter<any>();
  protected _editor: any;
  protected _options: any;
  protected _windowResizeSubscription: Subscription;
  private _insideNg: boolean = false;

  ngAfterViewInit(): void {
    if (loadedMonaco) {
      // Wait until monaco editor is available
      loadPromise.then(() => {
        this.initMonaco(this._options, this.insideNg);
      });
    } else {
      loadedMonaco = true;
      loadPromise = new Promise<void>((resolve: any) => {
        let baseUrl = this.config.baseUrl;
        // ensure backward compatibility
        if (baseUrl === 'assets' || !baseUrl) {
          console.log('Setting BaseUrl to default');
          baseUrl = '/static/assets/monaco-editor/min/vs';
        }

        this.setupMonacoWorkerEnvironment('/static/assets/monaco-editor/min');

        if (typeof (<any>window).monaco === 'object') {
          this.initMonaco(this._options, this.insideNg);
          resolve();
          return;
        }
        const onGotAmdLoader: any = (require?: any) => {
          let usedRequire = require || (<any>window).require;
          let requireConfig = {
            paths: { vs: `${baseUrl}` },
          };
          Object.assign(requireConfig, this.config.requireConfig || {});

          // Load monaco
          usedRequire.config(requireConfig);
          usedRequire(
            [`vs/editor/editor.main`],
            () => {
              if (typeof this.config.onMonacoLoad === 'function') {
                this.config.onMonacoLoad();
              }
              this.initMonaco(this._options, this.insideNg);
              resolve();
            },
            error => {
              console.error('Failed to load Monaco editor script:', error);
              resolve(); // Resolve anyway to prevent hanging
            },
          );
        };

        if (this.config.monacoRequire) {
          onGotAmdLoader(this.config.monacoRequire);
          // Load AMD loader if necessary
        } else if (!(<any>window).require) {
          const loaderScript: HTMLScriptElement = document.createElement('script');
          loaderScript.type = 'text/javascript';
          loaderScript.src = `${baseUrl}/loader.js`;
          loaderScript.addEventListener('load', () => {
            onGotAmdLoader();
          });
          document.body.appendChild(loaderScript);
          // Load AMD loader without over-riding node's require
        } else if (!(<any>window).require.config) {
          var src = `${baseUrl}/loader.js`;

          var loaderRequest = new XMLHttpRequest();
          loaderRequest.addEventListener('load', () => {
            let scriptElem = document.createElement('script');
            scriptElem.type = 'text/javascript';
            scriptElem.text = [
              // Monaco uses a custom amd loader that over-rides node's require.
              // Keep a reference to node's require so we can restore it after executing the amd loader file.
              'var nodeRequire = require;',
              loaderRequest.responseText.replace('"use strict";', ''),
              // Save Monaco's amd require and restore Node's require
              'var monacoAmdRequire = require;',
              'require = nodeRequire;',
              'require.nodeRequire = require;',
            ].join('\n');
            document.body.appendChild(scriptElem);
            onGotAmdLoader((<any>window).monacoAmdRequire);
          });
          loaderRequest.open('GET', src);
          loaderRequest.send();
        } else {
          onGotAmdLoader();
        }
      });
    }
  }

  setValue(value: string) {
    if (this._editor) {
      this._editor.setValue(value);
    }
  }

  setLanguage(language: string) {
    if (this._editor) {
      this._editor.getModel().setLanguage(language);
    }
  }


  private setupMonacoWorkerEnvironment(baseUrl: string): void {
    // Ensure baseUrl is absolute
    const absoluteBaseUrl = new URL(baseUrl, window.location.origin).href;

    (window as any).MonacoEnvironment = {
      getWorkerUrl: function (moduleId: string, label: string) {
        return `data:text/javascript;charset=utf-8,${encodeURIComponent(`
          self.MonacoEnvironment = {
            baseUrl: '${absoluteBaseUrl}'
          };
          // This imports the worker bootstrapper that handles AMD modules correctly
          importScripts('${absoluteBaseUrl}/vs/base/worker/workerMain.js');
        `)}`;
      },
    };
  }

  protected abstract initMonaco(options: any, insideNg: boolean): void;

  ngOnDestroy() {
    if (this._windowResizeSubscription) {
      this._windowResizeSubscription.unsubscribe();
    }
    if (this._editor) {
      this._editor.dispose();
      this._editor = undefined;
    }
  }
}
