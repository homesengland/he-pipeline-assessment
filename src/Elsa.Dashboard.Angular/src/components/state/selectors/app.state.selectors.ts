import {createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from '../reducers/app.state.reducers';

const getAppState = createFeatureSelector<AppState>('appState');

export const selectServerUrl = createSelector(getAppState, (state) => state.storeConfig.serverUrl);

export const selectMonacoLibPath = createSelector(getAppState, (state) => state.storeConfig.monacoLibPath);

export const selectStoreConfig = createSelector(getAppState, (state) => state.storeConfig);
