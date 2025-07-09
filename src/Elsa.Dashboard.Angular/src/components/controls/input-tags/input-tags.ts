import { Component, EventEmitter, Output } from '@angular/core';

@Component({
  selector: 'input-tags',
  templateUrl: 'input-tags.css',
  standalone: false,
})
export class InputTags {
  fieldName?: string;
  fieldId?: string;
  placeholder?: string = 'Add a tag';
  values?: Array<string> = [];
  @Output() valueChanged: EventEmitter<Array<string>>;
}
