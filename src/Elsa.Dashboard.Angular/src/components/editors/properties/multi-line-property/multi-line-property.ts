import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges, input } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, IntellisenseContext } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';

@Component({
  selector: 'multi-line-property',
  templateUrl: './multi-line-property.html',
  standalone: false,
})
export class MultiLineProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);
  currentValue = computed(() => this.propertyModel()?.expressions[this.defaultSyntax()] || '');

  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');
  fieldName = computed(() => this.propertyDescriptor()?.name ?? 'default');
  editorHeightTextArea = 6;

  context? = input<string>();
  editorContext: IntellisenseContext;

  constructor() {
      console.log("Setting property model", this.propertyModel());
  }

  ngOnInit() {
    this.editorContext = {
      propertyName: this.propertyDescriptor().name,
      activityTypeName: this.activityModel().type,
    };
  }

  onChange(e: Event) {
    const input = e.currentTarget as HTMLTextAreaElement;
    let expressions = this.propertyModel().expressions;
    expressions['Literal'] = input.value;
    this.propertyModel.update(x => ({ ...x, expressions: expressions }));
  }

  onDefaultSyntaxValueChanged(e: string) {
    //dont think we need this...
    //this.currentValue = e.detail;
  }
}