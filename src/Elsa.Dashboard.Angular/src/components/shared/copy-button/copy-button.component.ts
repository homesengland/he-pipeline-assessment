import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { clip } from '../../../utils/utils';

@Component({
  selector: 'elsa-copy-button',
  standalone: true,
  imports: [CommonModule],
  template: `
    <button type="button" (click)="copyText()" class="elsa-ml-2 elsa-text-sm elsa-text-blue-500 hover:elsa-text-blue-700">
      <svg class="elsa-h-4 elsa-w-4" fill="none" stroke="currentColor" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
        <path
          stroke-linecap="round"
          stroke-linejoin="round"
          stroke-width="2"
          d="M8 16H6a2 2 0 01-2-2V6a2 2 0 012-2h8a2 2 0 012 2v2m-6 12h8a2 2 0 002-2v-8a2 2 0 00-2-2h-8a2 2 0 00-2 2v8a2 2 0 002 2z"
        ></path>
      </svg>
    </button>
  `,
})
export class CopyButtonComponent {
  @Input() value: string;

  copyText() {
    navigator.clipboard.writeText(this.value).then(() => {
      // Optional: Show a tooltip or notification that text was copied
    });
  }
}
