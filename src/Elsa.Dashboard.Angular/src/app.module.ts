import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { WorkflowRoot } from './components/dashboard/pages/workflow-root/workflow-root';
import { WorkflowDashboard } from './components/dashboard/pages/workflow-dashboard/workflow-dashboard';
import { WorkflowDefinitionsList } from './components/dashboard/pages/workflow-definitions-list/workflow-definitions-list';
import { WorkflowInstancesList } from './components/dashboard/pages/workflow-instances-list/workflow-instances-list';
import { WorkflowHome } from './components/dashboard/pages/workflow-home/workflow-home';
import { WorkflowRegistry } from './components/dashboard/pages/workflow-registry/workflow-registry';
import { WorkflowInstanceListScreen } from './components/screens/workflow-instance-list/workflow-instance-list-screen/workflow-instance-list-screen';
import { routing } from './workflow-routes.module'
import { AppStateModule } from './store/app.state.module';
import { ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule, RouterOutlet } from '@angular/router';
import { WorkflowPager } from './components/controls/workflow-pager/workflow-pager';
import { WorkflowDropdownButton } from './components/controls/workflow-dropdown-button/workflow-dropdown-button';
import { WorkflowContextMenu } from './components/controls/workflow-context-menu/workflow-context-menu';
import { ConfirmDialog } from './components/shared/confirm-dialog/confirm-dialog';
import { ModalDialog } from './components/shared/modal-dialog/modal-dialog';
import { MultiExpressionEditor } from './components/editors/multi-expression-editor/multi-expression-editor';
import { ExpressionEditor } from './components/editors/expression-editor/expression-editor';
import { PropertyEditor } from './components/editors/property-editor/property-editor';
import { SingleLineProperty } from './components/editors/properties/single-line-property/single-line-property';
import { WorkflowPlaceholder } from './components/dashboard/pages/workflow-placeholder/workflow-placeholder';
import { StoreModule } from '@ngrx/store';
import { appStateReducer } from './store/reducers/app.state.reducers';
import { IntellisenseService } from './services/intellisense-service';
import { createWorkflowClient, WorkflowClient } from './services/workflow-client';
import { MonacoEditorModule, provideMonacoEditor } from './components/monaco/editor-module';
import { ToastNotification } from './components/shared/toast-notification/toast-notification';
import { monacoConfig } from './components/monaco/config';
import { WorkflowInstanceViewerScreen } from './components/screens/workflow-instance-viewer/workflow-instance-viewer-screen/workflow-instance-viewer-screen';
import { WorkflowInstancesView } from './components/dashboard/pages/workflow-instances-view/workflow-instances-view';
import { WorkflowDefinitionListScreen } from './components/screens/workflow-definition-list/workflow-definition-list-screen/workflow-definition-list-screen';
import { EditorComponent } from './components/monaco/editor.component';
import { DiffEditorComponent } from './components/monaco/diff-editor.component';

@NgModule({
  declarations: [
    WorkflowRoot,
    WorkflowDashboard,
    WorkflowDefinitionsList,
    WorkflowDefinitionListScreen,
    WorkflowInstancesList,
    WorkflowInstanceViewerScreen,
    WorkflowInstancesView,
    WorkflowContextMenu,
    WorkflowHome,
    WorkflowRegistry,
    WorkflowInstanceListScreen,
    WorkflowPager,
    WorkflowDropdownButton,
    WorkflowContextMenu,
    WorkflowPlaceholder,
    ConfirmDialog,
    ModalDialog,
    MultiExpressionEditor,
    ExpressionEditor,
    PropertyEditor,
    SingleLineProperty,
    ToastNotification,
    DiffEditorComponent,
    EditorComponent,
  ],
  imports: [
    BrowserModule,
    routing,
    AppStateModule,
    ReactiveFormsModule,
    RouterModule,
    RouterOutlet,
    CommonModule,
    StoreModule.forRoot({ appState: appStateReducer }),
  ],
  providers:[
    IntellisenseService,
    provideHttpClient(),
    provideMonacoEditor(monacoConfig)
  ],
  bootstrap: [WorkflowRoot],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule {}
