import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, PropertySettings } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { MonacoValueChangedArgs } from 'src/models/monaco-elements';
import { EditorModel } from 'src/components/monaco/types';
import { validateHeaderName } from 'http';
import { single } from 'rxjs';

@Component({
  selector: 'json-property',
  templateUrl: './json-property.html',
  standalone: false,
})
export class JsonProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  currentValue: string;
  model: EditorModel;
  options: PropertySettings = {};

  constructor() {
    console.log('Setting property model', this.propertyModel());
    this.model = {
      value: this.currentValue,
      language: 'json',
    };
    this.options = this.getOptions();
  }

  ngOnInit(): void {
    const defaultSyntax = this.propertyDescriptor().defaultSyntax || SyntaxNames.Json;
    this.currentValue = this.propertyModel().expressions[defaultSyntax] || undefined;
  }

  getOptions(): any {
    return {
      value: this.currentValue,
      language: 'json',
      fontFamily: 'Roboto Mono, monospace',
      renderLineHighlight: 'none',
      minimap: {
        enabled: false,
      },
      automaticLayout: true,
      lineNumbers: 'off',
      theme: 'vs',
      roundedSelection: true,
      scrollBeyondLastLine: false,
      readOnly: false,
      overviewRulerLanes: 0,
      overviewRulerBorder: false,
      lineDecorationsWidth: 0,
      hideCursorInOverviewRuler: true,
      glyphMargin: false,
      singleLineMode: false,
    };
  }

  getEditorHeight() {
    const options = this.propertyDescriptor().options;
    const editorHeightName = options?.editorHeight || 'Large';

    switch (editorHeightName) {
      case 'Large':
        return '20em';
    }
    return '15em';
  }

  getContext(): string {
    return this.propertyDescriptor().options?.context;
  }

  onDefaultSyntaxValueChanged(e: string) {
    this.currentValue = e;
  }

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue = e.value;
  }
}
