// import { selectJavaScriptTypeDefinitionsFetchStatus } from './../components/state/selectors/app.state.selectors';
import { Store } from '@ngrx/store';
import { StoreConfig } from '../models/store-config';
import { selectStoreConfig, selectWorkflowDefinitionId } from '../store/selectors/app.state.selectors';
import { toSignal } from '@angular/core/rxjs-interop';
import { Signal } from "@angular/core";

import { Injectable } from '@angular/core';
import { createWorkflowClient, WorkflowClient } from "./workflow-client";

@Injectable({
  providedIn: 'root',
})
export class IntellisenseService {
  private _baseUrl: string = "";
  private _workflowDefinitionId: string = "";
  private _storeConfig: StoreConfig | null = null;

  constructor(private store: Store) {
    this.store.select(selectStoreConfig).subscribe(config => {
      this._storeConfig = config;
      this._baseUrl = config.serverUrl;
    });
    this.store.select(selectWorkflowDefinitionId).subscribe(id => {
      this._workflowDefinitionId = id;
    });
  }

  async getJavaScriptTypeDefinitions(): Promise<string> {
    let httpClient = await this.createHttpClient();
    const response = await httpClient.scriptingApi.getJavaScriptTypeDefinitions(this._workflowDefinitionId);
    return response;
  }

  private async createHttpClient(): Promise<WorkflowClient> {
    return await createWorkflowClient(this._baseUrl);
  }
}
