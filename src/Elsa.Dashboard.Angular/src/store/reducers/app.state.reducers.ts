import { createReducer, on, StoreModule } from '@ngrx/store';
import { AppStateActionGroup } from '../actions/app.state.actions';
import { StoreConfig } from '../../models/store-config';
import { DataDictionaryGroup } from '../../models/custom-component-models';
import { StoreStatus } from '../../models/constants';

export class AppState {
    storeConfig: StoreConfig;
    dataDictionary: Array<DataDictionaryGroup>;
    workflowDefinitionId?: string;
    javaScriptTypeDefinitionsFetchStatus: StoreStatus;
    javaScriptTypeDefinitions: string;
    dataDictionaryIntellisense: string;
}
export const initialState: AppState = {
    storeConfig: null,
    dataDictionary: null,
    workflowDefinitionId: "",
    javaScriptTypeDefinitionsFetchStatus: StoreStatus.Empty,
    javaScriptTypeDefinitions: "",
    dataDictionaryIntellisense: ""
}

export const appStateReducer = createReducer(
    initialState,
    on(AppStateActionGroup.setExternalState, (_state, { storeConfig, dataDictionary }) => ({ ..._state, storeConfig: storeConfig, dataDictionary: dataDictionary })),
    on(AppStateActionGroup.setStoreConfig, (_state, { storeConfig }) => ({ ..._state, storeConfig: storeConfig })),
    on(AppStateActionGroup.setWorkflowDefinitionId, (_state, { workflowDefinitionId }) => ({ ..._state, workflowDefinitionId: workflowDefinitionId })),
    on(AppStateActionGroup.setJavaScriptTypeDefinitionsFetchStatus, (_state, { status }) => ({ ..._state, javaScriptTypeDefinitionsFetchStatus: status })),
    on(AppStateActionGroup.setJavaScriptTypeDefinitions, (_state, { javaScriptTypeDefinitions }) => ({ ..._state, javaScriptTypeDefinitions: javaScriptTypeDefinitions })),
    on(AppStateActionGroup.setDataDictionaryIntellisense, (_state, { dataDictionaryIntellisense }) => ({ ..._state, dataDictionaryIntellisense: dataDictionaryIntellisense })),
);

