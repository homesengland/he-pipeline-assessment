import { createReducer, on, StoreModule } from '@ngrx/store';
import { AppStateActionGroup } from '../actions/app.state.actions';

export class AppState {
  serverUrl: string;
  monacoLibPath: string;
}
export const initialState: AppState = {
  serverUrl: "",
  monacoLibPath: "",
}

export const appStateReducer = createReducer(
  initialState,
  on(AppStateActionGroup.setExternalState, (_state, { serverUrl, monacoLibPath }) => ({ serverUrl: serverUrl, monacoLibPath: monacoLibPath }))
);
