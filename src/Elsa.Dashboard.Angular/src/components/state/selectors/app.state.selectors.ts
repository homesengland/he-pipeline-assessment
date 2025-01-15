import {createFeatureSelector, createSelector } from '@ngrx/store';
import { AppState } from '../reducers/app.state.reducers';

//export const selectServerUrl = createFeatureSelector<string>('serverUrl');

export const selectmonacoLibPath = createFeatureSelector<string>('monacoLibPath');

export const selectState = (state: AppState) => state;

export const selectServerUrl = createSelector(
  selectState,
  (state: AppState) => state.serverUrl
);
