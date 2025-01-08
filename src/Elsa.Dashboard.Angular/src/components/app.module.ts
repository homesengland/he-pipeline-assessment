import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { WorkflowRoot } from './dashboard/pages/workflow-root/workflow-root';

@NgModule({
  declarations: [
  ],
  imports: [
    BrowserModule
  ],
  providers: [],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class AppModule { }
