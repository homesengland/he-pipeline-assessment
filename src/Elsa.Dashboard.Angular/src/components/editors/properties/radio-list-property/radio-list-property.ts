import { Component, computed, model, OnChanges, OnInit, signal, SimpleChanges } from '@angular/core';
import { ActivityModel, SyntaxNames } from '../../../../models';
import { ActivityDefinitionProperty, ActivityPropertyDescriptor, SelectList } from '../../../../models/domain';
import { PropertyEditor } from '../../property-editor/property-editor';
import { getSelectListItems } from 'src/utils/selected-list-items';
import { ElsaClientService } from 'src/services/elsa-client';
import { parseJson } from 'src/utils/utils';

@Component({
  selector: 'radio-list-property',
  templateUrl: './radio-list-property.html',
  standalone: false,
})
export class RadioListProperty implements OnInit {
  activityModel = model<ActivityModel>();
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  propertyModel = model<ActivityDefinitionProperty>();
  serverUrl: string
  currentValue?: string

  fieldLabel = computed(() => this.propertyDescriptor()?.label ?? 'default');
  fieldId = computed(() => this.propertyDescriptor()?.name ?? 'default');

  selectList: SelectList = {
    items: [],
    isFlagsEnum: false,
  };
$event: CustomEvent<any>;
$defaultSyntaxEvent: CustomEvent;

  constructor(private elsaClientService: ElsaClientService,) {
    console.log('Setting property model', this.propertyModel());
  }

  async ngOnInit(): Promise<void> {
    const defaultValue = this.propertyDescriptor().defaultValue?.toString() || '';
    this.currentValue = defaultValue;
    this.selectList = await getSelectListItems(this.elsaClientService, this.serverUrl, this.propertyDescriptor());
  }

  onDefaultSyntaxValueChanged(e: CustomEvent) {
    this.currentValue = e.detail;
  }

  onCheckChanged(e: Event) {
    const radio = (e.target as HTMLInputElement);
    const checked = radio.checked;

    if (checked)
      this.currentValue = radio.value;

    const defaultSyntax = this.propertyDescriptor().defaultSyntax || SyntaxNames.Literal;
    this.propertyModel().expressions[defaultSyntax] = this.currentValue;
  }

  getSelectItems(): any {
    return this.selectList.items.map((item, index) => {
      return {
        text: item.text,
        value: item.value,
        isSelected: this.currentValue == item.value,
        inputId: `${this.fieldId}_${index}`,
      };
    });
  }
}


