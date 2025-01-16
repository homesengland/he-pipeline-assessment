import {createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from '../reducers/app.state.reducers';

//export const selectServerUrl = createFeatureSelector<string>('serverUrl');

/*export const selectmonacoLibPath = createFeatureSelector<string>('monacoLibPath');*/

const getAppState = createFeatureSelector<AppState>('appState');

export const selectServerUrl = createSelector(getAppState, (state) => state.serverUrl);

export const selectMonacoLibPath = createSelector(getAppState, (state) => state.monacoLibPath);;
