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
import { AppStateModule } from '../store/app.state.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule, NgStyle } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { RouterModule, RouterOutlet } from '@angular/router';
import { WorkflowPager } from './controls/workflow-pager/workflow-pager';
import { WorkflowDropdownButton } from './controls/workflow-dropdown-button/workflow-dropdown-button';
import { WorkflowContextMenu } from './controls/workflow-context-menu/workflow-context-menu';
import { ConfirmDialog } from './shared/confirm-dialog/confirm-dialog';
import { ModalDialog } from './shared/modal-dialog/modal-dialog';
import { WorkflowInstancesView } from './dashboard/pages/workflow-instances-view/workflow-instances-view';
import { WorkflowInstanceViewerScreen } from './screens/workflow-instance-viewer/workflow-instance-viewer-screen/workflow-instance-viewer-screen';
import { WorkflowPerformanceInformation } from './shared/workflow-performance-information/workflow-performance-information';
import { WorkflowFaultInformation } from './shared/workflow-fault-information/workflow-fault-information';
import { DesignerTree } from './designers/tree/designer-tree/designer-tree';
import { ActivityIconProvider } from 'src/services/activity-icon-provider';
import { ActivityIconProviderPlugin } from 'src/plugins/activity-icon-provider-plugin';
import { WorkflowDefinitionListScreen } from './screens/workflow-definition-list/workflow-definition-list-screen/workflow-definition-list-screen';
import { WorkflowDefinitionsView } from './dashboard/pages/workflow-definitions-view/workflow-definitions-view';
import { WorkflowInstanceJournalComponent } from './screens/workflow-instance-viewer/workflow-instance-journal/workflow-instance-journal';
import { FlyoutPanelComponent } from './shared/flyout-panel/flyout-panel.component';
import { TabHeaderComponent } from './shared/tab-header/tab-header.component';
import { TabContentComponent } from './shared/tab-content/tab-content.component';
import { SingleLineDriver } from '../drivers/single-line-driver';

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
    WorkflowDefinitionsView,
    WorkflowPerformanceInformation,
    WorkflowFaultInformation,
    DesignerTree,
    WorkflowInstancesView,
    WorkflowInstanceViewerScreen,
    WorkflowInstanceJournalComponent,
    FlyoutPanelComponent,
    TabHeaderComponent,
    TabContentComponent,
  ],

  imports: [BrowserModule, HttpClientModule, routing, AppStateModule, ReactiveFormsModule, RouterModule, RouterOutlet, CommonModule, NgStyle, BrowserAnimationsModule],
  providers: [ActivityIconProvider, ActivityIconProviderPlugin, SingleLineDriver],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {
  constructor(private activityIconProviderPlugin: ActivityIconProviderPlugin, private singleLineDriver: SingleLineDriver) {}
}
