import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { WorkflowRoot } from './dashboard/pages/workflow-root/workflow-root';
import {routing} from '../workflow-routes.module'
import { AppStateModule } from './state/app.state.module';


@NgModule({
  declarations: [
  ],
  imports: [
    BrowserModule, HttpClientModule, routing, AppStateModule
  ],
  providers: [],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class AppModule { }
