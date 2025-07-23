import { Component, input, output, OnChanges, SimpleChanges } from '@angular/core';

@Component({
  selector: 'input-tags',
  templateUrl: './input-tags.html',
  styleUrl: './input-tags.css',
  standalone: false,
})
export class InputTags {
  fieldName = input<string>(null);
  fieldId = input<string>(null);
  placeHolder: string = 'Add tag';
  values: Array<string> = [];
  valueChanged = output<string[]>();
  currentValues?: Array<string> = [];
  valuesJson: string = null;

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

    this.values = this.currentValues || [];

    if (!Array.isArray(this.values))
      this.values = [];

    this.valuesJson = JSON.stringify(this.values);
  }

  addItem(item: string): void {
    const values = [...this.currentValues, item];
    this.currentValues = values.distinct();
    this.valueChanged.emit(this.currentValues);
  }

  onInputKeyDown(e: KeyboardEvent) {
    if (e.key !== 'Enter')
      return;

    e.preventDefault();

    const input = e.target as HTMLInputElement;
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
