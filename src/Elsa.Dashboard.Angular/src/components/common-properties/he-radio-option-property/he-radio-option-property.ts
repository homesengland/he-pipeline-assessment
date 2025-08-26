import { Component } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor } from '../../../models/elsa-interfaces';
import { NestedActivityDefinitionProperty } from '../../../models/custom-component-models';
import { SyntaxNames } from '../../../constants/constants';
import { SortableComponent } from 'src/components/sortable-component';

@Component({
  selector: 'he-radio-option-property',
  templateUrl: './he-radio-option-property.html',
  standalone: false,
})
export class HeRadioOptionProperty {
  activityModel: ActivityModel;
  propertyDescriptor: ActivityPropertyDescriptor;
  propertyModel: ActivityDefinitionProperty;
  modelSyntax: string = SyntaxNames.Json;
  keyId: string;
  properties: NestedActivityDefinitionProperty[];

  private _base: SortableComponent;
  // private _toggle: DisplayToggle;
}
