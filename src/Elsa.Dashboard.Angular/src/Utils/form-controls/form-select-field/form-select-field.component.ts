import { Component, Input } from '@angular/core';
import { FormContext, SelectOption } from 'src/Utils/forms';

@Component({
  selector: 'form-select-field',
  templateUrl: './form-select-field.component.html',
  standalone: false,
})
export class FormSelectFieldComponent {
  @Input() context: FormContext;
  @Input() fieldName: string;
  @Input() label: string;
  @Input() value: string;
  @Input() options: Array<SelectOption> = [];
  @Input() hint?: string;
  @Input() fieldId?: string;

  onChange(event: Event) {
    const element = event.target as HTMLSelectElement;
    this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
