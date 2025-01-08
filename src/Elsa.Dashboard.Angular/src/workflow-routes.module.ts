import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkflowDefinitionsList } from './components/dashboard/pages/workflow-definitions-list/workflow-definitions-list';

const routes: Routes = [
  {
    path: 'workflow-definitions',
    component: WorkflowDefinitionsList,
  }
];

export const routing = RouterModule.forRoot(routes);
