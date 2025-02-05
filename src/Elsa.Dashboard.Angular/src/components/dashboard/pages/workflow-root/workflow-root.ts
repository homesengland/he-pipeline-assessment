import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { WorkflowDashboard} from '../workflow-dashboard/workflow-dashboard'
import { Store } from '@ngrx/store';
import { StoreConfig } from '../../../../models/store-config';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';
import { DataDictionaryGroup } from '../../../../Models/custom-component-models';
import { IntellisenseGatherer } from '../../../../utils/intellisense-gatherer';
import { StoreStatus } from '../../../../models/constants';

@Component({
  selector: 'workflow-root',
  templateUrl: './workflow-root.html',
  styleUrls: ['./workflow-root.css'],
  imports: [WorkflowDashboard]
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


  constructor(private http: HttpClient, el: ElementRef, private store: Store, renderer2: Renderer2) {
    this.renderer2 = renderer2;
    this.dataDictionaryJson = el.nativeElement.getAttribute('dataDictionaryJson');
    this.storeConfigJson = el.nativeElement.getAttribute('storeConfigJson');
    this.setExternalState()
    this.intellisenseGatherer = new IntellisenseGatherer(store);
  }

  ngOnInit(): void {
    this.modalHandlerShown();
    this.modalHandlerHidden();
  }

  ngOnDestroy() {
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

