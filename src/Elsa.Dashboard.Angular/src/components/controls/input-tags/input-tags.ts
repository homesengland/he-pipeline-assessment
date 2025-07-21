import { Component, computed, model, EventEmitter, Output, signal, Signal, input, output, OnChanges, SimpleChanges } from '@angular/core';
import { ActivityPropertyDescriptor } from '../../../models/domain';

@Component({
  selector: 'input-tags',
  templateUrl: './input-tags.html',
  styleUrl: './input-tags.css',
  standalone: false,
})
export class InputTags {
  propertyDescriptor = model<ActivityPropertyDescriptor>();
  fieldName = computed(() => this.propertyDescriptor()?.name || 'default');
  fieldId = computed(() => this.propertyDescriptor()?.name || 'default');
  placeHolder: string = 'Add tag';
  values: Array<string> = [];
  valueChanged = output<string[]>();
  currentValues?: Array<string> = [];
  // values = this.currentValues || [];

  ngOnChanges(changes: SimpleChanges) {
    if (changes['values']) {
      this.currentValues = changes['values'].currentValue || [];
    }
  }

  constructor() {
    console.log('InputTags component initialized');
  }

  async ngOnInit(): Promise<void> {
    this.currentValues = this.values;
  }

  addItem(item: string): void {
    const values = [...this.currentValues, item];
    this.currentValues = values.distinct();
    this.valueChanged.emit(this.currentValues);
  }

  onInputKeyDown(event: KeyboardEvent) {
    if (event.key !== 'Enter')
      return;

    event.preventDefault();

    const input = event.target as HTMLInputElement;
    const value = input.value.trim();

    if (value.length == 0)
      return;

    this.addItem(value);
    input.value = '';
  }

  onInputBlur(e: Event) {
    const input = e.target as HTMLInputElement;
    const value = input.value.trim();

    if (value.length == 0)
      return;

    this.addItem(value);
    input.value = '';
  }

  onDeleteTagClick(e: Event, tag: string) {
    e.preventDefault();

    this.currentValues = this.currentValues.filter(x => x !== tag);
    this.valueChanged.emit(this.currentValues);
  }
}
