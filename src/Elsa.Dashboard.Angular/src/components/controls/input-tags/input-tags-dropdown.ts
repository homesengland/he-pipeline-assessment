import { Component, EventEmitter, Input, Output, SimpleChanges } from '@angular/core';
import { SelectListItem } from 'src/models';

@Component({
  selector: 'input-tags-dropdown',
  templateUrl: './input-tags-dropdown.html',
  standalone: false,
})
export class InputTagsDropdown {
  fieldName?: string;
  fieldId?: string;
  placeHolder: string = 'Add tag';
  values: Array<string | SelectListItem> = [];
  dropdownValues: Array<SelectListItem> = [];
  valueChanged = new EventEmitter<Array<SelectListItem>>();

  currentValues: Array<SelectListItem> = [];

  ngOnChanges(changes: SimpleChanges) {
    if (changes['values'] || changes['dropdownValues']) {
      this.syncCurrentValues();
    }
  }

  private syncCurrentValues() {
    const dropdownValues = this.dropdownValues || [];
    const values = this.values || [];
    this.currentValues = values.map(value => dropdownValues.find(tag => value === tag.value) || (typeof value === 'object' ? value : null)).filter((v): v is SelectListItem => !!v);
  }

  get dropdownItems(): Array<SelectListItem> {
    return (this.dropdownValues || []).filter(x => this.currentValues.findIndex(y => y.value === x.value) < 0);
  }

  get valuesJson(): string {
    return JSON.stringify(this.currentValues.map(tag => tag.value));
  }

  onTagSelected(event: Event) {
    event.preventDefault();
    const input = event.target as HTMLSelectElement;
    const selectedValue = input.value;
    if (!selectedValue || selectedValue === 'Add') return;

    const selectedTag = this.dropdownValues.find(tag => tag.value === selectedValue);
    if (!selectedTag) return;

    const values = [...this.currentValues, selectedTag].filter((tag, i, arr) => arr.findIndex(t => t.value === tag.value) === i);
    this.currentValues = values;
    input.value = 'Add';
    this.valueChanged.emit(values);
  }

  onDeleteTagClick(event: MouseEvent, tag: SelectListItem) {
    event.preventDefault();
    this.currentValues = this.currentValues.filter(t => t.value !== tag.value);
    this.valueChanged.emit(this.currentValues);
  }
}
