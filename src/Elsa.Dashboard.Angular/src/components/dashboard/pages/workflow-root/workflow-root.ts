import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Input, OnInit } from '@angular/core';
import { WorkflowDashboard} from '../workflow-dashboard/workflow-dashboard'
import { Store } from '@ngrx/store';
import { StoreConfig } from '../../../../Models/storeConfig';
import { AppStateActionGroup } from '../../../state/actions/app.state.actions';
import { DataDictionaryGroup } from '../../../../Models/custom-component-models';

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


  constructor(private http: HttpClient, el: ElementRef, private store: Store) {
    this.dataDictionaryJson = el.nativeElement.getAttribute('dataDictionaryJson');
    this.storeConfigJson = el.nativeElement.getAttribute('storeConfigJson');
    this.setEternalState()
  }

  setEternalState() {
    this.setStoreConfig()
    this.setDataDictionary()
    this.store.dispatch(AppStateActionGroup.setExternalState({ storeConfig: this.storeConfig, dataDictionary: this.dataDictionary }));
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

  ngOnInit(): void {

  }

}

