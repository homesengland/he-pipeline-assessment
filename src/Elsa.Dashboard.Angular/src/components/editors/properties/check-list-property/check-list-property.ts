import { Component, computed, model, Input, OnInit } from '@angular/core';
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
  currentValue: string;
  defaultSyntax = computed(() => this.propertyDescriptor()?.defaultSyntax || SyntaxNames.Literal);

  fieldName = computed(() => this.propertyDescriptor()?.name || 'default');
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');
  monacoEditor: HTMLElsaMonacoElement;
  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };
  serverUrl: string;

  constructor(private elsaClientService: ElsaClientService, private store: Store) {
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit() : Promise<void>{
    if (this.propertyModel().expressions[SyntaxNames.Json] === undefined)
      this.propertyModel().expressions[SyntaxNames.Json] = JSON.stringify(this.propertyDescriptor().defaultValue);
    this.currentValue = this.propertyModel()?.expressions[SyntaxNames.Json] || '[]';
    this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, this.propertyDescriptor());
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

    this.propertyModel().expressions[SyntaxNames.Json] = this.currentValue.toString();
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
