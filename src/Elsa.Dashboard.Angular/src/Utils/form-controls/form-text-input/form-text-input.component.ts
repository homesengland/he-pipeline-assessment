import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-text-input',
  templateUrl: './form-text-input.component.html',
  standalone: false,
})
export class FormTextInputComponent {
  @Input() context: FormContext;
  @Input() fieldName: string;
  @Input() label: string;
  @Input() value: string;
  @Input() hint?: string;
  @Input() fieldId?: string;
  @Input() readonlyField?: boolean;

  @Output() valueChange = new EventEmitter<string>();

  onInput(event: Event) {
    const element = event.target as HTMLInputElement;
    this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
