import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { WorkflowRoot } from './dashboard/pages/workflow-root/workflow-root';
import { WorkflowDashboard } from './dashboard/pages/workflow-dashboard/workflow-dashboard';
import { WorkflowDefinitionsList } from './dashboard/pages/workflow-definitions-list/workflow-definitions-list';
import { WorkflowInstancesList } from './dashboard/pages/workflow-instances-list/workflow-instances-list';
import { WorkflowHome } from './dashboard/pages/workflow-home/workflow-home';
import { WorkflowRegistry } from './dashboard/pages/workflow-registry/workflow-registry';
import { WorkflowInstanceListScreen } from './screens/workflow-instance-list/workflow-instance-list-screen/workflow-instance-list-screen';
import { routing } from '../workflow-routes.module';
import { AppStateModule } from './state/app.state.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgStyle } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { WorkflowPager } from './controls/workflow-pager/workflow-pager';
import { WorkflowDropdownButton } from './controls/workflow-dropdown-button/workflow-dropdown-button';
import { WorkflowContextMenu } from './controls/workflow-context-menu/workflow-context-menu';
import { ConfirmDialog } from './shared/confirm-dialog/confirm-dialog';
import { ModalDialog } from './shared/modal-dialog/modal-dialog';
import { WorkflowInstancesView } from './dashboard/pages/workflow-instances-view/workflow-instances-view';
import { WorkflowInstanceViewerScreen } from './screens/workflow-instance-viewer-screen/workflow-instance-viewer-screen';
import { WorkflowPerformanceInformation } from './shared/workflow-performance-information/workflow-performance-information';
import { WorkflowFaultInformation } from './shared/workflow-fault-information/workflow-fault-information';
import { DesignerTree } from './designers/tree/designer-tree/designer-tree';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { ActivityIconProviderPlugin } from 'src/plugins/activity-icon-provider-plugin';
import { WorkflowDefinitionListScreen } from './screens/workflow-definition-list/workflow-definition-list-screen/workflow-definition-list-screen';
import { WorkflowDefinitionEdit } from './dashboard/pages/workflow-definitions-edit/workflow-definitions-edit';
import { WorkflowDefinitionEditorScreen } from './screens/workflow-definition-editor/workflow-definition-editor-screen';

@NgModule({
  declarations: [
    WorkflowRoot,
    WorkflowDashboard,
    WorkflowDefinitionsList,
    WorkflowInstancesList,
    WorkflowHome,
    WorkflowRegistry,
    WorkflowInstanceListScreen,
    WorkflowDefinitionListScreen,
    WorkflowPager,
    WorkflowDropdownButton,
    WorkflowContextMenu,
    ConfirmDialog,
    ModalDialog,
    WorkflowDefinitionsList,
    WorkflowPerformanceInformation,
    WorkflowFaultInformation,
    DesignerTree,
    WorkflowInstancesView,
    WorkflowInstanceViewerScreen,
    WorkflowDefinitionEdit,
    WorkflowDefinitionEditorScreen,
  ],

  imports: [BrowserModule, HttpClientModule, routing, AppStateModule, ReactiveFormsModule, RouterModule, RouterOutlet, CommonModule, NgStyle],
  providers: [ActivityIconProvider, ActivityIconProviderPlugin],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {
  constructor(private activityIconProviderPlugin: ActivityIconProviderPlugin) {}
}
