import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http';
import { WorkflowRoot } from './dashboard/pages/workflow-root/workflow-root';
import { WorkflowDashboard } from './dashboard/pages/workflow-dashboard/workflow-dashboard';
import { WorkflowDefinitionsList } from './dashboard/pages/workflow-definitions-list/workflow-definitions-list';
import { WorkflowInstancesList } from './dashboard/pages/workflow-instances-list/workflow-instances-list';
import { WorkflowHome } from './dashboard/pages/workflow-home/workflow-home';
import { WorkflowRegistry } from './dashboard/pages/workflow-registry/workflow-registry';
import { WorkflowInstanceListScreen } from './screens/workflow-instance-list/workflow-instance-list-screen/workflow-instance-list-screen';
import {routing} from '../workflow-routes.module'
import { AppStateModule } from './state/app.state.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { WorkflowPager } from './controls/workflow-pager/workflow-pager';


@NgModule({
  declarations: [ WorkflowRoot, WorkflowDashboard, WorkflowDefinitionsList, WorkflowInstancesList, WorkflowHome, WorkflowRegistry, WorkflowInstanceListScreen, WorkflowPager ],
  imports: [
    BrowserModule, HttpClientModule, routing, AppStateModule, ReactiveFormsModule, RouterModule, RouterOutlet, CommonModule
  ],
  providers: [],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
  
})
export class AppModule { }
