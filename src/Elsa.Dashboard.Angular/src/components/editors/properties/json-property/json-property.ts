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
  }

  ngOnInit(): void {
    const defaultSyntax = this.propertyDescriptor().defaultSyntax || SyntaxNames.Json;
    this.currentValue = this.propertyModel().expressions[defaultSyntax] || undefined;
    this.options = this.propertyDescriptor().options || {};
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

  async onMonacoValueChanged(e: MonacoValueChangedArgs) {
    const expressions = {
      ...this.propertyModel().expressions,
      [SyntaxNames.Json]: e.value,
    };
    this.propertyModel.update(model => ({
      ...model,
      expressions,
    }));
    this.updateActivityModel(expressions);
    console.log('Activity Model Property', this.activityModel().properties);
  }

  onDefaultSyntaxValueChanged(e: string) {
    this.currentValue = e;
    const expressions = {
      ...this.propertyModel().expressions,
      [SyntaxNames.Json]: e,
    };
    this.propertyModel.update(model => ({
      ...model,
      expressions,
    }));
    this.model = {
      language: SyntaxNames.Json,
      value: this.currentValue,
    };
    this.updateActivityModel(expressions);
    console.log('Activity Model Property', this.activityModel().properties);
  }

  private updateActivityModel(expressions: { Json: string }) {
    const updatedProperties = this.activityModel().properties.map(property =>
      property.name === this.propertyDescriptor().name ? { ...property, expressions: expressions, syntax: this.defaultSyntax() } : property,
    );
    this.activityModel.update(model => ({
      ...model,
      properties: updatedProperties,
    }));
  }
}
