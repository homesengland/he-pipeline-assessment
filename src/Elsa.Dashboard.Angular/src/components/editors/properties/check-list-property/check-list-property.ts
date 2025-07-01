import { Component, computed, model, signal, Input, OnInit } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, SelectList } from '../../../../models';
import { parseJson } from '../../../../utils/utils';
import { ElsaClientService } from 'src/services/elsa-client';
import { selectServerUrl } from 'src/store/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { getSelectListItems } from 'src/utils/selected-list-items';
import { HTMLElsaMonacoElement } from '../../../../models/elsa-interfaces';

@Component({
  selector: 'check-list-property',
  templateUrl: './check-list-property.html',
  standalone: false,
})
export class CheckListProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  serverUrl: string;
  currentValue: string;
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };

  monacoEditor: HTMLElsaMonacoElement;

  constructor(private elsaClientService: ElsaClientService, private store: Store) {
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    if (this.propertyModel().expressions[SyntaxNames.Json] === undefined)
      this.propertyModel().expressions[SyntaxNames.Json] = JSON.stringify(this.propertyDescriptor().defaultValue);
    this.currentValue = this.propertyModel()?.expressions[SyntaxNames.Json] || '[]';
    this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, this.propertyDescriptor());
  }

  onCheckChanged(e: Event) {
    const checkbox = e.currentTarget as HTMLInputElement;
    const checked = checkbox.checked;
    const value = checkbox.value;
    const isFlags = this.selectList.isFlagsEnum;

    if (isFlags) {
      let newValue = parseInt(this.currentValue as string);

      if (checked)
        newValue = newValue | parseInt(value);
      else
        newValue = newValue & ~parseInt(value);

      this.currentValue = newValue.toString();
    } else {
      let newValue = parseJson(this.currentValue as string);

      if (checked)
        newValue = [...newValue, value].distinct();
      else
        newValue = newValue.filter(x => x !== value);

      this.currentValue = JSON.stringify(newValue);
    }

    this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue.toString();
  }

  getSelectItems(): any {
    console.log('Select list items:', this.selectList.items);
    return this.selectList.items.map((item, index) => {
      let isSelected = this.selectList.isFlagsEnum;

      if (isSelected) {
        isSelected = (parseInt(this.currentValue) & parseInt(item.value)) === parseInt(item.value);
      } else {
        const selectedValues = parseJson(this.currentValue as string) || [];
        isSelected = selectedValues.includes(item.value);
      }

      return {
        text: item.text,
        value: item.value,
        inputId: `${this.fieldId}_${index}`,
        isSelected: isSelected
      };
    });
  }
}
