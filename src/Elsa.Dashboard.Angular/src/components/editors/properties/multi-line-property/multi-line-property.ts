import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges, input } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, IntellisenseContext, PropertySettings } from '../../../../models/domain';

@Component({
  selector: 'multi-line-property',
  templateUrl: './multi-line-property.html',
  standalone: false,
})
export class MultiLineProperty {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');

  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');

  constructor() {
    console.log('Setting property model', this.propertyModel());
  }

  onChange(e: Event) {
    const input = e.currentTarget as HTMLTextAreaElement;
    let expressions = this.propertyModel().expressions;
    expressions['Literal'] = input.value;
    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
    this.updateActivityModel('Literal', input.value);
  }

  private updateActivityModel(syntax: string, value: string) {
    const updatedProperties = this.activityModel().properties.map(property =>
      property.name === this.propertyDescriptor().name
        ? {
            ...property,
            expressions: {
              ...property.expressions,
              [syntax]: value,
            },
            syntax: this.defaultSyntax(),
          }
        : property,
    );
    this.activityModel.update(model => ({
      ...model,
      properties: updatedProperties,
    }));
  }

  getEditorHeight() {
    const options = this.propertyDescriptor() as PropertySettings;
    const editorHeightName = options?.editorHeight || 'Default';

    switch (editorHeightName) {
      case 'Large':
        return { propertyEditor: '20em', textArea: 6 };
    }
    return { propertyEditor: '15em', textArea: 3 };
  }

  getContext(): string {
    return this.propertyDescriptor().options?.context || 'default';
  }
}
