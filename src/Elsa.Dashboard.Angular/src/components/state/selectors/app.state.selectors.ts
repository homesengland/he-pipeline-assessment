import {createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from '../reducers/app.state.reducers';

//export const selectServerUrl = createFeatureSelector<string>('serverUrl');

/*export const selectmonacoLibPath = createFeatureSelector<string>('monacoLibPath');*/

const getAppState = createFeatureSelector<AppState>('appState');

export const selectServerUrl = createSelector(getAppState, (state) => state.serverUrl);

export const selectMonacoLibPath = createSelector(getAppState, (state) => state.monacoLibPath);

export const selectAuth0Domain = createSelector(getAppState, (state) => state.auth0Domain);

export const selectAuth0ClientId = createSelector(getAppState, (state) => state.auth0ClientId);

export const selectAuth0Audience = createSelector(getAppState, (state) => state.auth0Audience);
export const selectBasePath = createSelector(getAppState, (state) => state.basePath);
