import { Component, Input, Output, EventEmitter } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-check-box',
  templateUrl: './form-check-box.component.html',
  standalone: false,
})
export class FormCheckBoxComponent {
  @Input() fieldName: string;
  @Input() label: string;
  @Input() checked: boolean;
  @Input() hint?: string;
  @Input() fieldId?: string;

  @Output() checkedChanged: EventEmitter<string> = new EventEmitter<string>();

  onChange(event: Event) {
    const element = event.target as HTMLInputElement;
    this.checkedChanged.emit(element.value);
    //this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.checked });
  }
}
