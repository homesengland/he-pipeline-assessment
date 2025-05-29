import { Component, Input } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-check-box',
  templateUrl: './form-check-box.component.html',
  standalone: false,
})
export class FormCheckBoxComponent {
  @Input() context: FormContext;
  @Input() fieldName: string;
  @Input() label: string;
  @Input() checked: boolean;
  @Input() hint?: string;
  @Input() fieldId?: string;

  onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.checked });
  }
}
