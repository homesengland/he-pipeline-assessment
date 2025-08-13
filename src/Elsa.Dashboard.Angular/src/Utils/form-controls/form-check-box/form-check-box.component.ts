import { Component, Input, model } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-check-box',
  templateUrl: './form-check-box.component.html',
  standalone: false,
})
export class FormCheckBoxComponent {
  @Input() fieldName: string;
  @Input() label: string;
  @Input() hint?: string;
  @Input() fieldId?: string;

  checked = model<boolean>(false);

  onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    this.checked.update(x => x = element.checked);
  }
}
