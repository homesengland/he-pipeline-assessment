import { Component, Input, OnInit } from '@angular/core';
import { ActivityDefinitionProperty, ActivityModel, ActivityPropertyDescriptor, SelectList, SyntaxNames } from '../../../../models';
import { SelectListService } from '../../../../services/select-list.service'; // You need to implement this service
import { parseJson } from '../../../../utils/utils'; // You need to implement or import this utility

@Component({
  selector: 'elsa-check-list-property',
  templateUrl: './elsa-check-list-property.component.html',
  styleUrls: ['./elsa-check-list-property.component.scss']
})
export class ElsaCheckListPropertyComponent implements OnInit {
  @Input() activityModel: ActivityModel;
  @Input() propertyDescriptor: ActivityPropertyDescriptor;
  @Input() propertyModel: ActivityDefinitionProperty;
  @Input() serverUrl: string;

  currentValue: string;
  selectList: SelectList = { items: [], isFlagsEnum: false };

  constructor(private selectListService: SelectListService) { }

  async ngOnInit() {
    if (this.propertyModel.expressions[SyntaxNames.Json] === undefined)
      this.propertyModel.expressions[SyntaxNames.Json] = JSON.stringify(this.propertyDescriptor.defaultValue);
    this.currentValue = this.propertyModel.expressions[SyntaxNames.Json] || '[]';
    this.selectList = await this.selectListService.getSelectListItems(this.serverUrl, this.propertyDescriptor);
  }

  onCheckChanged(event: Event, value: string) {
    const checked = (event.target as HTMLInputElement).checked;
    const isFlags = this.selectList.isFlagsEnum;

    if (isFlags) {
      let newValue = parseInt(this.currentValue as string, 10);

      if (checked)
        newValue = newValue | parseInt(value, 10);
      else
        newValue = newValue & ~parseInt(value, 10);

      this.currentValue = newValue.toString();
    } else {
      let newValue = parseJson(this.currentValue as string);

      if (checked)
        newValue = Array.from(new Set([...newValue, value]));
      else
        newValue = newValue.filter(x => x !== value);

      this.currentValue = JSON.stringify(newValue);
    }

    this.propertyModel.expressions[SyntaxNames.Json] = this.currentValue.toString();
  }

  isChecked(value: string): boolean {
    if (this.selectList.isFlagsEnum) {
      return (parseInt(this.currentValue, 10) & parseInt(value, 10)) === parseInt(value, 10);
    } else {
      const selectedValues = parseJson(this.currentValue as string) || [];
      return selectedValues.includes(value);
    }
  }
}
