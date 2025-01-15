import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { WorkflowDefinitionsList } from './components/dashboard/pages/workflow-definitions-list/workflow-definitions-list';
import { WorkflowDashboard} from './components/dashboard/pages/workflow-dashboard/workflow-dashboard'
import { WorkflowInstancesList } from './components/dashboard/pages/workflow-instances-list/workflow-instances-list';
import { WorkflowRegistry } from './components/dashboard/pages/workflow-registry/workflow-registry';
import { WorkflowHome } from './components/dashboard/pages/workflow-home/workflow-home';

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
  }
];

export const routing = RouterModule.forRoot(routes);
