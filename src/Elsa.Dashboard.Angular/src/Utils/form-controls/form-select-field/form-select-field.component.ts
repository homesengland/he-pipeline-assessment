import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormContext, SelectOption } from 'src/Utils/forms';

@Component({
  selector: 'form-select-field',
  templateUrl: './form-select-field.component.html',
  standalone: false,
})
export class FormSelectFieldComponent {
  @Input() fieldName: string;
  @Input() label: string;
  @Input() value: string;
  @Input() options: Array<SelectOption> = [];
  @Input() hint?: string;
  @Input() fieldId?: string;

  @Output() valueChanged: EventEmitter<string> = new EventEmitter<string>();

  onChange(event: Event) {
    const element = event.target as HTMLSelectElement;
    this.valueChanged.emit(element.value);
    //this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
