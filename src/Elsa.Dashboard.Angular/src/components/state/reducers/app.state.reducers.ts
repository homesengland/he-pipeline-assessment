import { createReducer, on, StoreModule } from '@ngrx/store';
import { AppStateActionGroup } from '../actions/app.state.actions';
import { StoreConfig } from '../../../Models/storeConfig';
import { DataDictionaryGroup } from '../../../Models/custom-component-models';

export class AppState {
  storeConfig: StoreConfig;
  dataDictionary: Array<DataDictionaryGroup>;
  storeDefinitions: object;
}
export const initialState: AppState = {
  storeConfig: null,
  dataDictionary: null,
  storeDefinitions: null
}

export const appStateReducer = createReducer(
  initialState,
  on(AppStateActionGroup.setExternalState, (_state, { storeConfig, dataDictionary }) => ({ storeConfig: storeConfig, dataDictionary: dataDictionary }))
);
