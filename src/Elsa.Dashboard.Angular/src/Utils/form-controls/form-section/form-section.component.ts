import { Component, Input } from '@angular/core';

@Component({
  selector: 'form-section',
  templateUrl: './form-section.component.html',
  standalone: false,
})
export class FormSectionComponent {
  @Input() title: string;
  @Input() subTitle?: string;
}
