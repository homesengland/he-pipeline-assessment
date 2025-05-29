import { Component, Input } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-text-area',
  templateUrl: './form-text-area.component.html',
  standalone: false,
})
export class FormTextAreaComponent {
  @Input() context: FormContext;
  @Input() fieldName: string;
  @Input() label: string;
  @Input() value: string;
  @Input() hint?: string;
  @Input() fieldId?: string;

  onInput(event: Event) {
    const element = event.target as HTMLTextAreaElement;
    this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
