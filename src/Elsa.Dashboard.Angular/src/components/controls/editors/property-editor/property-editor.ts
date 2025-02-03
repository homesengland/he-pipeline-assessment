import { Component, input, Input, model, output } from '@angular/core';
import { SyntaxNames } from '../../../../constants/constants';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, IntellisenseContext } from '../../../../models';


@Component({
  selector: 'property-editor',
  template: './property-editor.html',
})
export class PropertyEditor {

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
