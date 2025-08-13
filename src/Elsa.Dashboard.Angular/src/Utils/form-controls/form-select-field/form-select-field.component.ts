import { Component, Input, model } from '@angular/core';
import { FormContext, SelectOption } from 'src/Utils/forms';

@Component({
  selector: 'form-select-field',
  templateUrl: './form-select-field.component.html',
  standalone: false,
})
export class FormSelectFieldComponent {
  @Input() fieldName: string;
  @Input() label: string;
  @Input() options: Array<SelectOption> = [];
  @Input() hint?: string;
  @Input() fieldId?: string;

  value = model<string>('');

  onChange(event: Event) {
    const element = event.target as HTMLSelectElement;
    this.value.update(x => x = element.value);
  }
}
