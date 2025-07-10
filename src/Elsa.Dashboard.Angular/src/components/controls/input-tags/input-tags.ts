import { Component, EventEmitter, Input, Output, signal, Signal } from '@angular/core';

@Component({
  selector: 'input-tags',
  templateUrl: 'input-tags.css',
  standalone: false,
})
export class InputTags {
  @Input() fieldName?: string;
  @Input() fieldId?: string;
  @Input() placeHolder: string = 'Add tag';
  @Input() set values(val: string[] | undefined) {
    this.currentValues = val ?? [];
  }
  @Output() valueChanged = new EventEmitter<string[]>();

  currentValues: string[] = [];

  addItem(item: string) {
    const values = [...this.currentValues, item].filter((v, i, arr) => arr.indexOf(v) === i);
    this.currentValues = values;
    this.valueChanged.emit(values);
  }

  onInputKeyDown(event: KeyboardEvent) {
    if (event.key !== 'Enter') return;
    event.preventDefault();
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();
    if (!value) return;
    this.addItem(value);
    input.value = '';
  }

  onInputBlur(event: FocusEvent) {
    const input = event.target as HTMLInputElement;
    const value = input.value.trim();
    if (!value) return;
    this.addItem(value);
    input.value = '';
  }

  onDeleteTagClick(event: MouseEvent, tag: string) {
    event.preventDefault();
    this.currentValues = this.currentValues.filter(x => x !== tag);
    this.valueChanged.emit(this.currentValues);
  }
}
