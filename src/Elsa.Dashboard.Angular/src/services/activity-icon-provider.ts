import { Injectable, Type, ViewContainerRef } from '@angular/core';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Injectable({ providedIn: 'root' })
export class ActivityIconProvider {
  private iconMap: { [key: string]: (colour: string) => SafeHtml } = {};
  constructor(private sanitizer: DomSanitizer) {
    this.registerDefaultIcons();
    this.registerBreakIcons();
    this.registerCogIcons();
    this.registerCorrelateIcons();
    this.registerDeleteIcons();
    this.registerEraseIcons();
    this.registerFinishIcons();
    this.registerForkIcons();
    this.registerHttpEndpointIcons();
    this.registerIfIcons();
    this.registerInterruptTriggerIcons();
    this.registerJoinIcons();
    this.registerLoopIcons();
    this.registerReadLineIcons();
    this.registerRedirectIcons();
    this.registerRunWorkflowIcons();
    this.registerScriptIcons();
    this.registerSendEmailIcons();
    this.registerSendHttpRequestIcons();
    this.registerSendSignalIcons();
    this.registerSignalReceivedIcons();
    this.registerStateIcons();
    this.registerSwitchIcons();
    this.registerTimerIcons();
    this.registerWebhookIcons();
    this.registerWriteHttpResponseIcons();
    this.registerWriteLineIcons();
  }


  //#region Register Icon Methods

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

  private registerBreakIcons(): void {
    this.register('Break', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createBreakIconHtml(colour));
    });
  }

  private registerCogIcons(): void {
    this.register('SetVariable', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createCogIconHtml(colour));
    });
    this.register('SetTransientVariable', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createCogIconHtml(colour));
    });
    this.register('SetContextId', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createCogIconHtml(colour));
    });
    this.register('SetName', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createCogIconHtml(colour));
    });
  }

  private registerCorrelateIcons(): void {
    this.register('Correlate', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createCorrelateIconHtml(colour));
    });
  }

  private registerDeleteIcons(): void {
    //No Activities To Register
  }

  private registerEraseIcons(): void {
    this.register('ClearTimer', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createEraseIconHtml(colour));
    });
  }

  private registerFinishIcons(): void {
    this.register('Finish', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createFinishIconHtml(colour));
    });
  }

  private registerForkIcons(): void {
    this.register('Fork', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createForkIconHtml(colour));
    });
  }

  private registerHttpEndpointIcons(): void {
    this.register('HttpEndpoint', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createHttpEndpointIconHtml(colour));
    });
  }

  private registerIfIcons(): void {
    this.register('If', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createIfIconHtml(colour));
    });
  }

  private registerInterruptTriggerIcons(): void {
    this.register('InterruptTrigger', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createInterruptTriggerIconHtml(colour));
    });
  }

  private registerJoinIcons(): void {
    this.register('Join', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createJoinIconHtml(colour));
    });
  }

  private registerLoopIcons(): void {
    this.register('For', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createLoopIconHtml(colour));
    });
    this.register('ForEach', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createLoopIconHtml(colour));
    });
    this.register('While', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createLoopIconHtml(colour));
    });
    this.register('ParallelForEach', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createLoopIconHtml(colour));
    });
  }

  private registerReadLineIcons(): void {
    this.register('ReadLine', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createReadLineIconHtml(colour));
    });
  }

  private registerRedirectIcons(): void {
    this.register('Redirect', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createRedirectIconHtml(colour));
    });
  }

  private registerRunWorkflowIcons(): void {
    this.register('RunWorkflow', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createRunWorkflowIconHtml(colour));
    });
  }

  private registerScriptIcons(): void {
    this.register('RunJavaScript', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createScriptIconHtml(colour));
    });
  }

  private registerSendEmailIcons(): void {
    this.register('SendEmail', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSendEmailIconHtml(colour));
    });
  }

  private registerSendHttpRequestIcons(): void {
    this.register('SendHttpRequest', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSendHttpRequestIconHtml(colour));
    });
  }

  private registerSendSignalIcons(): void {
    this.register('SendSignal', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSendSignalIconHtml(colour));
    });
    this.register('SendRabbitMqMessage', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSendSignalIconHtml(colour));
    });
  }

  private registerSignalReceivedIcons(): void {
    this.register('SignalReceived', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSignalReceivedIconHtml(colour));
    });
    this.register('RabbitMqMessageReceived', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSignalReceivedIconHtml(colour));
    });
  }

  private registerStateIcons(): void {
    this.register('State', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createStateIconHtml(colour));
    });
  }

  private registerSwitchIcons(): void {
    this.register('Switch', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createSwitchIconHtml(colour));
    });
  }

  private registerTimerIcons(): void {
    this.register('Timer', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createTimerIconHtml(colour));
    });
    this.register('StartAt', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createTimerIconHtml(colour));
    });
    this.register('Cron', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createTimerIconHtml(colour));
    });
  }

  private registerWebhookIcons(): void {
    this.register('Webhook', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createWebhookIconHtml(colour));
    });
  }

  private registerWriteHttpResponseIcons(): void {
    this.register('WriteHttpResponse', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createWriteHttpResponseIconHtml(colour));
    });
  }

  private registerWriteLineIcons(): void {
    this.register('WriteLine', (colour: string) => {
      return this.sanitizer.bypassSecurityTrustHtml(this.createWriteLineIconHtml(colour));
    });
  }

  //#endregion

  //#region Create Icon Html Methods

  private createDefaultIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
      <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
        <path stroke="none" d="M0 0h24v24H0z"/>  <polyline points="21 12 17 12 14 20 10 4 7 12 3 12"/>
      </svg>
    </span>`;
  }

  private createBreakIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <path d="M14 8v-2a2 2 0 0 0 -2 -2h-7a2 2 0 0 0 -2 2v12a2 2 0 0 0 2 2h7a2 2 0 0 0 2 -2v-2"/>
      <path d="M7 12h14l-3 -3m0 6l3 -3"/>
    </svg>
  </span>`
  }

  private createCogIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M10.325 4.317c.426-1.756 2.924-1.756 3.35 0a1.724 1.724 0 002.573 1.066c1.543-.94 3.31.826 2.37 2.37a1.724 1.724 0 001.065 2.572c1.756.426 1.756 2.924 0 3.35a1.724 1.724 0 00-1.066 2.573c.94 1.543-.826 3.31-2.37 2.37a1.724 1.724 0 00-2.572 1.065c-.426 1.756-2.924 1.756-3.35 0a1.724 1.724 0 00-2.573-1.066c-1.543.94-3.31-.826-2.37-2.37a1.724 1.724 0 00-1.065-2.572c-1.756-.426-1.756-2.924 0-3.35a1.724 1.724 0 001.066-2.573c-.94-1.543.826-3.31 2.37-2.37.996.608 2.296.07 2.572-1.065z"/>
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M15 12a3 3 0 11-6 0 3 3 0 016 0z"/>
    </svg>
  </span>`
  }

  private createCorrelateIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
      <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
        <path stroke="none" d="M0 0h24v24H0z"/>
        <path d="M10 14a3.5 3.5 0 0 0 5 0l4 -4a3.5 3.5 0 0 0 -5 -5l-.5 .5"/>
        <path d="M14 10a3.5 3.5 0 0 0 -5 0l-4 4a3.5 3.5 0 0 0 5 5l.5 -.5"/>
      </svg>
    </span>`
  }

  private createDeleteIconHtml(colour: string): string {
    return `<svg class="elsa-h-5 elsa-w-5 elsa-text-${colour}-500" width="24" height="24" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <line x1="4" y1="7" x2="20" y2="7"/>
      <line x1="10" y1="11" x2="10" y2="17"/>
      <line x1="14" y1="11" x2="14" y2="17"/>
      <path d="M5 7l1 12a2 2 0 0 0 2 2h8a2 2 0 0 0 2 -2l1 -12"/>
      <path d="M9 7v-3a1 1 0 0 1 1 -1h4a1 1 0 0 1 1 1v3"/>
    </svg>`
  }

  private createEraseIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <path d="M19 19h-11l-4 -4a1 1 0 0 1 0 -1.41l10 -10a1 1 0 0 1 1.41 0l5 5a1 1 0 0 1 0 1.41l-9 9"/>
      <path d="M18 12.3l-6.3 -6.3"/>
    </svg>
  </span>`
  }
  
  private createFinishIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
      <path d="M4 15s1-1 4-1 5 2 8 2 4-1 4-1V3s-1 1-4 1-5-2-8-2-4 1-4 1z"/>
      <line x1="4" y1="22" x2="4" y2="15"/>
    </svg>
  </span>`
  }

  private createForkIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6 elsa-transform elsa-rotate-180" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <circle cx="12" cy="18" r="2"/>
      <circle cx="7" cy="6" r="2"/>
      <circle cx="17" cy="6" r="2"/>
      <path d="M7 8v2a2 2 0 0 0 2 2h6a2 2 0 0 0 2 -2v-2"/>
      <line x1="12" y1="12" x2="12" y2="16"/>
    </svg>
  </span>`
  }

  private createHttpEndpointIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M3 15a4 4 0 004 4h9a5 5 0 10-.1-9.999 5.002 5.002 0 10-9.78 2.096A4.001 4.001 0 003 15z"/>
    </svg>
  </span>`
  }

  private createIfIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"/>
    </svg>
  </span>`
  }

  private createInterruptTriggerIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <path d="M8 16v-4a4 4 0 0 1 8 0v4"/>
      <path d="M3 12h1M12 3v1M20 12h1M5.6 5.6l.7 .7M18.4 5.6l-.7 .7"/>
      <rect x="6" y="16" width="12" height="4" rx="1"/>
    </svg>
  </span>`
  }

  private createJoinIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <circle cx="12" cy="18" r="2"/>
      <circle cx="7" cy="6" r="2"/>
      <circle cx="17" cy="6" r="2"/>
      <path d="M7 8v2a2 2 0 0 0 2 2h6a2 2 0 0 0 2 -2v-2"/>
      <line x1="12" y1="12" x2="12" y2="16"/>
    </svg>
  </span>`
  }

  private createLoopIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <path d="M4 12v-3a3 3 0 0 1 3 -3h13m-3 -3l3 3l-3 3"/>
      <path d="M20 12v3a3 3 0 0 1 -3 3h-13m3 3l-3-3l3-3"/>
      <path d="M11 11l1 -1v4"/>
    </svg>
  </span>`
  }
  
  private createReadLineIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <polyline points="5 7 10 12 5 17"/>
      <line x1="13" y1="17" x2="19" y2="17"/>
    </svg>
  </span>`
  }

  private createRedirectIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <circle cx="6" cy="19" r="2"/>
      <circle cx="18" cy="5" r="2"/>
      <path d="M12 19h4.5a3.5 3.5 0 0 0 0 -7h-8a3.5 3.5 0 0 1 0 -7h3.5"/>
    </svg>
  </span>`
  }

  private createRunWorkflowIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <rect x="3" y="3" width="6" height="6" rx="1"/>
      <rect x="15" y="15" width="6" height="6" rx="1"/>
      <path d="M21 11v-3a2 2 0 0 0 -2 -2h-6l3 3m0 -6l-3 3"/>
      <path d="M3 13v3a2 2 0 0 0 2 2h6l-3 -3m0 6l3 -3"/>
    </svg>
  </span>`
  }

  private createScriptIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <path d="M15 21h-9a3 3 0 0 1 -3 -3v-1h10v2a2 2 0 0 0 4 0v-14a2 2 0 1 1 2 2h-2m2 -4h-11a3 3 0 0 0 -3 3v11"/>
      <line x1="9" y1="7" x2="13" y2="7"/>
      <line x1="9" y1="11" x2="13" y2="11"/>
    </svg>
  </span>`
  }

  private createSendEmailIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M16 12a4 4 0 10-8 0 4 4 0 008 0zm0 0v1.5a2.5 2.5 0 005 0V12a9 9 0 10-9 9m4.5-1.206a8.959 8.959 0 01-4.5 1.207" />
    </svg>
  </span>`
  }

  private createSendHttpRequestIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M3 15a4 4 0 004 4h9a5 5 0 10-.1-9.999 5.002 5.002 0 10-9.78 2.096A4.001 4.001 0 003 15z"/>
    </svg>
  </span>`
  }

  private createSendSignalIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round">
      <circle cx="12" cy="12" r="2"/>
      <path d="M16.24 7.76a6 6 0 0 1 0 8.49m-8.48-.01a6 6 0 0 1 0-8.49m11.31-2.82a10 10 0 0 1 0 14.14m-14.14 0a10 10 0 0 1 0-14.14"/>
    </svg>
  </span>`
  }

  private createSignalReceivedIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke="currentColor" aria-hidden="true">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M13 10V3L4 14h7v7l9-11h-7z"/>
    </svg>
  </span>`
  }

  private createStateIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <rect x="3" y="3" width="18" height="18" rx="2" ry="2" />
    </svg>
  </span>`
  }

  private createSwitchIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" stroke-width="2" stroke="currentColor" fill="none" stroke-linecap="round" stroke-linejoin="round">
      <path stroke="none" d="M0 0h24v24H0z"/>
      <polyline points="15 4 19 4 19 8"/>
      <line x1="14.75" y1="9.25" x2="19" y2="4"/>
      <line x1="5" y1="19" x2="9" y2="15"/>
      <polyline points="15 19 19 19 19 15"/>
      <line x1="5" y1="5" x2="19" y2="19"/>
    </svg>
  </span>`
  }

  private createTimerIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"/>
    </svg>
  </span>`
  }

  private createWebhookIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M12 8v4l3 3m6-3a9 9 0 11-18 0 9 9 0 0118 0z"/>
    </svg>
  </span>`
  }

  private createWriteHttpResponseIconHtml(colour: string): string {
    return `<span class="${`elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white`}">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M11 5H6a2 2 0 00-2 2v11a2 2 0 002 2h11a2 2 0 002-2v-5m-1.414-9.414a2 2 0 112.828 2.828L11.828 15H9v-2.828l8.586-8.586z"/>
    </svg>
  </span>`
  }

  private createWriteLineIconHtml(colour: string): string {
    return `<span class="elsa-rounded-lg elsa-inline-flex elsa-p-3 elsa-bg-${colour}-50 elsa-text-${colour}-700 elsa-ring-4 elsa-ring-white">
    <svg class="elsa-h-6 elsa-w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor">
      <path stroke-linecap="round" stroke-linejoin="round" stroke-width={2} d="M15.232 5.232l3.536 3.536m-2.036-5.036a2.5 2.5 0 113.536 3.536L6.5 21.036H3v-3.572L16.732 3.732z"/>
    </svg>
  </span>`
  }

  //#endregion

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
