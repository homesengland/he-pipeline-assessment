import { createReducer, on, StoreModule } from '@ngrx/store';
import { AppStateActionGroup } from '../actions/app.state.actions';
import { StoreConfig } from '../../../models/storeConfig';
import { DataDictionaryGroup } from '../../../models/custom-component-models';

export class AppState {
  storeConfig: StoreConfig;
  dataDictionary: Array<DataDictionaryGroup>;
  workflowDefinitionId: string;
  javaScriptTypeDefinitionsFetchStatus: string;
  javaScriptTypeDefinitions: string;
  dataDictionaryIntellisense: string;
}
export const initialState: AppState = {
  storeConfig: null,
  dataDictionary: null,
  workflowDefinitionId: '',
  javaScriptTypeDefinitionsFetchStatus: '',
  javaScriptTypeDefinitions: '',
  dataDictionaryIntellisense: '',
};

export const appStateReducer = createReducer(
  initialState,
  on(AppStateActionGroup.setExternalState, (_state, { storeConfig, dataDictionary }) => ({ ..._state, storeConfig: storeConfig, dataDictionary: dataDictionary })),
  on(AppStateActionGroup.setStoreConfig, (_state, { storeConfig }) => ({ ..._state, storeConfig: storeConfig })),
  on(AppStateActionGroup.setWorkflowDefinitionId, (_state, { workflowDefinitionId }) => ({ ..._state, workflowDefinitionId: workflowDefinitionId })),
  on(AppStateActionGroup.setJavascriptTypeDefinitionsFetchStatus, (_state, { javaScriptTypeDefinitionsFetchStatus }) => ({
    ..._state,
    javaScriptTypeDefinitionsFetchStatus: javaScriptTypeDefinitionsFetchStatus,
  })),
  on(AppStateActionGroup.setJavaScriptTypeDefinitions, (_state, { javaScriptTypeDefinitions }) => ({ ..._state, javaScriptTypeDefinitions: javaScriptTypeDefinitions })),
  on(AppStateActionGroup.setDataDictionaryIntellisense, (_state, { dataDictionaryIntellisense }) => ({ ..._state, dataDictionaryIntellisense: dataDictionaryIntellisense })),
);
