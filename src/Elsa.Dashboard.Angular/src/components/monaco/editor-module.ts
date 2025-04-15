import { CommonModule } from '@angular/common';
import { ModuleWithProviders, NgModule, makeEnvironmentProviders } from '@angular/core';

import { MONACO_EDITOR_CONFIG, MonacoEditorConfig } from './config';
import { DiffEditorComponent } from './diff-editor.component';
import { EditorComponent } from './editor.component';

@NgModule({
  imports: [
    CommonModule,
    EditorComponent,
    DiffEditorComponent
  ],
  exports: [
    EditorComponent,
    DiffEditorComponent
  ]
})
export class MonacoEditorModule {
  public static forRoot(config: MonacoEditorConfig = {}): ModuleWithProviders<MonacoEditorModule> {
    return {
      ngModule: MonacoEditorModule,
      providers: [
        { provide: MONACO_EDITOR_CONFIG, useValue: config }
      ]
    };
  }
}

export function provideMonacoEditor(config: MonacoEditorConfig = {}) {
  return makeEnvironmentProviders([
    { provide: MONACO_EDITOR_CONFIG, useValue: config }
  ]);
}
