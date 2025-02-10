import { Component, input, Input, model, OnChanges, OnInit, output, SimpleChanges } from '@angular/core';
import { SyntaxNames } from '../../../constants/constants';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from '../../../models';
import { MultiExpressionEditor } from '../multi-expression-editor/multi-expression-editor';


@Component({
  selector: 'property-editor',
    template: './property-editor.html',
    imports: [MultiExpressionEditor]
})
export class PropertyEditor implements OnInit, OnChanges {

  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  editorHeight = input('10em');
  singleLineMode = input(false);
  context? = input<string>();
  showLabel = input(true);

  defaultSyntaxValueChanged = output<string>();
  editorContext: IntellisenseContext;
  constructor() {

  }
    ngOnChanges(changes: SimpleChanges): void {

    }

  ngOnInit() {
    this.editorContext = {
      propertyName: this.propertyDescriptor().name,
      activityTypeName: this.activityModel().type
    }
  }


  onSyntaxChanged(e: CustomEvent<string>) {
    this.propertyModel.update(x => ({ ...x, syntax: e.detail }));
  }

  onExpressionChanged(e: CustomEvent<string>) {
    let defaultSyntax = this.propertyDescriptor().defaultSyntax || SyntaxNames.Literal;
    let syntax = this.propertyModel().syntax || defaultSyntax;
    let expressions = this.propertyModel().expressions;
    expressions[syntax] = e.detail;
    this.propertyModel.update(x => ({ ...x, expressions: expressions }))

    if (syntax != defaultSyntax)
      return;

    this.defaultSyntaxValueChanged.emit(e.detail);
  }
}
