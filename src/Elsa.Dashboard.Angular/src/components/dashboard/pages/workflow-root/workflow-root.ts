import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, viewChild, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { StoreConfig } from '../../../../models/store-config';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';
import { DataDictionaryGroup } from '../../../../models/custom-component-models';
import { IntellisenseGatherer } from '../../../../utils/intellisense-gatherer';
import { StoreStatus } from '../../../../models/constants';
import { eventBus } from '../../../../services/event-bus';
import { EventTypes, WorkflowStudio } from '../../../../models';
import { HTMLElsaConfirmDialogElement } from '../../../../models/elsa-interfaces';
import { pluginManager } from 'src/services/plugin-manager';
import { createHttpClient, createWorkflowClient, WorkflowClient } from 'src/services/workflow-client';
import { AxiosInstance } from 'axios';
import { confirmDialogService } from 'src/services/confirm-dialog-service';
import { getOrCreateProperty, htmlToElement } from 'src/utils/utils';
import { Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';
import { activityIconProvider } from 'src/services/activity-icon-provider';
import { propertyDisplayManager } from 'src/services/property-display-manager';
import { toastNotificationService } from 'src/services/toast-notification-service';

@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  standalone: false
})
export class WorkflowRoot implements OnInit {
  dataDictionary: Array<DataDictionaryGroup>;
  dataDictionaryJson: string;
  storeConfig: StoreConfig;
  storeConfigJson: string;
  intellisenseGatherer: IntellisenseGatherer;
  renderer2: Renderer2;
  private modalShownListener: () => void;
  private modalHiddenListener: () => void;
  readonly confirmDialog = viewChild.required<HTMLElsaConfirmDialogElement>('confirmDialog');

  constructor(private http: HttpClient, el: ElementRef, private store: Store, renderer2: Renderer2) {
    this.renderer2 = renderer2;
    this.dataDictionaryJson = el.nativeElement.getAttribute('dataDictionaryJson');
    this.storeConfigJson = el.nativeElement.getAttribute('storeConfigJson');
    this.setExternalState();
    this.intellisenseGatherer = new IntellisenseGatherer(store);
  }

  async ngOnInit(): Promise<void> {
    eventBus.on(EventTypes.ShowConfirmDialog, this.onShowConfirmDialog);
    this.modalHandlerShown();
    this.modalHandlerHidden();

    const workflowClientFactory: () => Promise<WorkflowClient> = () => createWorkflowClient(this.storeConfig.serverUrl);
    const httpClientFactory: () => Promise<AxiosInstance> = () => createHttpClient(this.storeConfig.serverUrl);

    const workflowStudio: WorkflowStudio = {
      serverUrl: this.storeConfig.serverUrl,
      basePath: this.storeConfig.basePath,
      serverFeatures: [],
      serverVersion: null,
      eventBus,
      pluginManager,
      confirmDialogService,
      workflowClientFactory,
      httpClientFactory,
      getOrCreateProperty: getOrCreateProperty,
      htmlToElement,
      features: [],
      propertyDisplayManager,
      activityIconProvider,
      toastNotificationService
    };

    let auth0Params: AuthorizationParams = {
      audience: this.storeConfig.audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: this.storeConfig.clientId,
      domain: this.storeConfig.domain,
      useRefreshTokens: true,
      useRefreshTokensFallback: true,
    };

    pluginManager.initialize(workflowStudio, auth0Options)
    await eventBus.emit(EventTypes.Root.Initializing);
  }

  onShowConfirmDialog = (e) => e.promise = this.confirmDialog().show(e.caption, e.message)
  ngOnDestroy() {
    eventBus.detach(EventTypes.ShowConfirmDialog, this.onShowConfirmDialog);
    this.modalShownListener();
    this.modalHiddenListener();
  }

  async modalHandlerShown() {

    this.modalShownListener = this.renderer2.listen("window", "shown", event => {
      event = event;
      var url_string = document.URL;
      var n = url_string.lastIndexOf('/');
      var workflowDef = url_string.substring(n + 1);
      this.store.dispatch(AppStateActionGroup.setWorkflowDefinitionId({
        workflowDefinitionId: workflowDef
      }));
    });
    await this.getIntellisense();
  }

  async modalHandlerHidden() {

    this.modalHiddenListener = this.renderer2.listen("window", "hidden", event => {
      event = event;
      this.store.dispatch(AppStateActionGroup.setJavaScriptTypeDefinitions({
        javaScriptTypeDefinitions: ""
      }));
      this.store.dispatch(AppStateActionGroup.setJavascriptTypeDefinitionsFetchStatus({
        javaScriptTypeDefinitionsFetchStatus: StoreStatus.Empty
      }));
    });
    await this.getIntellisense();
  }

  setExternalState() {
    this.setStoreConfig()
    this.setDataDictionary()
    this.store.dispatch(AppStateActionGroup.setExternalState({
      storeConfig: this.storeConfig,
      dataDictionary: this.dataDictionary
    }));
  }

  async getIntellisense() {
    await this.intellisenseGatherer.getIntellisense();
  }

  setStoreConfig() {
    if (this.storeConfigJson != null) {
      this.storeConfig = JSON.parse(this.storeConfigJson);
    }
  }

  setDataDictionary() {
    if (this.dataDictionaryJson != null) {
      this.dataDictionaryJson = JSON.stringify(this.dataDictionaryJson);
      this.dataDictionary = JSON.parse(this.dataDictionaryJson);
    }
  }
}

