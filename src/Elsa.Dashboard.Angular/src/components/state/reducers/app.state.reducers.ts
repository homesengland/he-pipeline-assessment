import { createReducer, on, StoreModule } from '@ngrx/store';
import { AppStateActionGroup } from '../actions/app.state.actions';

export class AppState {
  serverUrl: string;
  monacoLibPath: string;
  auth0Domain: string;
  auth0ClientId: string;
  auth0Audience: string;
  basePath: string;
}
export const initialState: AppState = {
  serverUrl: "",
  monacoLibPath: "",
  auth0Audience: "",
  auth0ClientId: "",
  auth0Domain: "",
  basePath: ""
}

export const appStateReducer = createReducer(
  initialState,
  on(AppStateActionGroup.setExternalState, (_state, { serverUrl, monacoLibPath, auth0Domain, auth0ClientId, auth0Audience, basePath }) => ({ serverUrl: serverUrl, monacoLibPath: monacoLibPath, auth0Domain: auth0Domain, auth0Audience: auth0ClientId, auth0ClientId: auth0Audience, basePath: basePath }))
);
