import { Component, Input, model } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-text-area',
  templateUrl: './form-text-area.component.html',
  standalone: false,
})
export class FormTextAreaComponent {
  @Input() fieldName: string;
  @Input() label: string;
  @Input() hint?: string;
  @Input() fieldId?: string;

  value = model<string>('');

  onInput(event: Event) {
    const element = event.target as HTMLTextAreaElement;
    this.value.update(x => x = element.value);
    //this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
