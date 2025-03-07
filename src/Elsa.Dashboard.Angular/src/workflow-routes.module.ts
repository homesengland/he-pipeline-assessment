import { NgModule } from '@angular/core';
import { Routes, RouterModule, provideRouter } from '@angular/router';
import { WorkflowDefinitionsList } from './components/dashboard/pages/workflow-definitions-list/workflow-definitions-list';
import { WorkflowDashboard} from './components/dashboard/pages/workflow-dashboard/workflow-dashboard'
import { WorkflowInstancesList } from './components/dashboard/pages/workflow-instances-list/workflow-instances-list';
import { WorkflowRegistry } from './components/dashboard/pages/workflow-registry/workflow-registry';
import { WorkflowHome } from './components/dashboard/pages/workflow-home/workflow-home';
import { WorkflowInstancesView } from './components/dashboard/pages/workflow-instances-view/workflow-instances-view';
import { WorkflowBlueprintView } from './components/dashboard/pages/workflow-blueprint-view/workflow-blueprint-view';
import {WorkflowPlaceholder} from './components/dashboard/pages/workflow-placeholder/workflow-placeholder';

const routes: Routes = [
  {
    path: '',
    component: WorkflowHome,
  },
  {
    path: 'workflow-definitions',
    component: WorkflowDefinitionsList,
  },
  {
    path: 'workflow-instances',
    component: WorkflowInstancesList,
  },
  {
    path: 'workflow-registry',
      component: WorkflowRegistry,
  },
  {
    path: 'workflow-instances/:id',
    component: WorkflowInstancesView
  },
  {
    path: 'workflow-registry/:id',
    component: WorkflowBlueprintView
  },
  {
    path: 'workflow-placeholder/:id',
    component: WorkflowPlaceholder
  }

];

export const routing = RouterModule.forRoot(routes, {bindToComponentInputs: true});
