import { Component, Input, Output, EventEmitter, model } from '@angular/core';
import { FormContext } from 'src/Utils/forms';

@Component({
  selector: 'form-text-input',
  templateUrl: './form-text-input.component.html',
  standalone: false,
})
export class FormTextInputComponent {
  @Input() fieldName: string;
  @Input() label: string;
  //@Input() value: string;
  @Input() hint?: string;
  @Input() fieldId?: string;
  @Input() readonlyField?: boolean;

  value = model<string>('');

  @Output() valueChanged: EventEmitter<string> = new EventEmitter<string>();

  onInput(event: Event) {
    console.log("Input Event");
    const element = event.target as HTMLSelectElement;
    this.value.update(x => x = element.value);
      //this.valueChanged.emit(element.value);
    //this.context && this.context.updater({ ...this.context.model, [this.fieldName]: element.value });
  }
}
