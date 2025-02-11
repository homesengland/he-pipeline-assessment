import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2, viewChild, ViewChild } from '@angular/core';
import { Store } from '@ngrx/store';
import { StoreConfig } from '../../../../Models/storeConfig';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';
import { DataDictionaryGroup } from '../../../../Models/custom-component-models';
import { IntellisenseGatherer } from '../../../../Utils/intellisenseGatherer';
import { StoreStatus } from '../../../../Models/constants';
import { eventBus } from '../../../../services/event-bus';
import { EventTypes } from '../../../../models';
import { HTMLElsaConfirmDialogElement } from '../../../../Models/elsa-interfaces';
import { PluginManager } from 'src/services/plugin-manager';
import { Auth0Plugin } from 'src/plugins/auth0-plugin';


@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  standalone: false
})
export class WorkflowRoot implements OnInit {
  pluginManager: PluginManager
  dataDictionary: Array<DataDictionaryGroup>;
  dataDictionaryJson: string;
  storeConfig: StoreConfig;
  storeConfigJson: string;
  intellisenseGatherer: IntellisenseGatherer;
  renderer2: Renderer2;
  private modalShownListener: () => void;
  private modalHiddenListener: () => void;
  readonly confirmDialog = viewChild.required<HTMLElsaConfirmDialogElement>('confirmDialog');

  constructor(private http: HttpClient, el: ElementRef, private store: Store, renderer2: Renderer2, pluginManager: PluginManager) {
    this.renderer2 = renderer2;
    this.dataDictionaryJson = el.nativeElement.getAttribute('dataDictionaryJson');
    this.storeConfigJson = el.nativeElement.getAttribute('storeConfigJson');
    this.setExternalState();
    let auth0Config = {
      domain: this.storeConfig.domain,
      clientId: this.storeConfig.clientId,
    }
    this.pluginManager = pluginManager;
    this.pluginManager.registerPlugin(() => new Auth0Plugin(auth0Config))
    this.intellisenseGatherer = new IntellisenseGatherer(store);
  }

  ngOnInit(): void {
    eventBus.on(EventTypes.ShowConfirmDialog, this.onShowConfirmDialog);
    this.modalHandlerShown();
    this.modalHandlerHidden();
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

