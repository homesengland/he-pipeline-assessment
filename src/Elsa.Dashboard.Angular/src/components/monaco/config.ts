import { ApplicationConfig, InjectionToken } from '@angular/core';
import { provideMonacoEditor } from './editor-module';
import { min } from 'lodash';

export const MONACO_EDITOR_CONFIG = new InjectionToken('MONACO_EDITOR_CONFIG');

export interface MonacoEditorConfig {
  baseUrl?: string;
  requireConfig?: { [key: string]: any };
  defaultOptions?: { [key: string]: any };
  monacoRequire?: Function;
  onMonacoLoad?: Function;
}

declare var monaco: any;

export function onMonacoLoad() {
  console.log((window as any).monaco);

  const uri = monaco.Uri.parse('a://b/foo.json');
  monaco.languages.json.jsonDefaults.setDiagnosticsOptions({
    validate: true,
    schemas: [
      {
        uri: 'http://myserver/foo-schema.json',
        fileMatch: [uri.toString()],
        schema: {
          type: 'object',
          properties: {
            p1: {
              enum: ['v1', 'v2'],
            },
            p2: {
              $ref: 'http://myserver/bar-schema.json',
            },
          },
        },
      },
      {
        uri: 'http://myserver/bar-schema.json',
        fileMatch: [uri.toString()],
        schema: {
          type: 'object',
          properties: {
            q1: {
              enum: ['x1', 'x2'],
            },
          },
        },
      },
    ],
  });
}

export const monacoConfig: MonacoEditorConfig = {
  // You can pass cdn url here instead
  baseUrl: 'assets',
  defaultOptions: { scrollBeyondLastLine: false, automaticLayout: true, minimap: { enabled: false } },
  onMonacoLoad,
};

export const appConfig: ApplicationConfig = {
  providers: [provideMonacoEditor(monacoConfig)],
};
