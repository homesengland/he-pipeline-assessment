import { Injectable, Type, ViewContainerRef } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Injectable({ providedIn: 'root' })
export class ActivityIconProvider {
  private iconMap: { [key: string]: (colour: string) => SafeHtml } = {};
  constructor(private sanitizer: DomSanitizer) {
    this.registerDefaultIcons();
  }

  private registerDefaultIcons(): void {
    this.register('QuestionScreen', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createDefaultIconHtml(colour));
    });
    this.register('CheckYourAnswersScreen', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createDefaultIconHtml(colour));
    });
    this.register('FinishWorkflow', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createDefaultIconHtml(colour));
    });
    this.register('ConfirmationScreen', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createDefaultIconHtml(colour));
    });
  }

  private createDefaultIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
      <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
        <path stroke="none" d="M0 0h24v24H0z"/>  <polyline points="21 12 17 12 14 20 10 4 7 12 3 12"/>
      </svg>
    </span>`;
  }

  register(activityType: string, iconProvider: (colour: string) => SafeHtml): void {
    this.iconMap[activityType] = (colour: string) => iconProvider(colour);
  }

  getIcon(activityType: string, colour: string): SafeHtml | null {
    const provider = this.iconMap[activityType];

    if (!provider) {
      return null;
    }

    return provider.call(this, colour);
  }
}
