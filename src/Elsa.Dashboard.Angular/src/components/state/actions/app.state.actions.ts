import { createActionGroup, props } from '@ngrx/store';

export const AppStateActionGroup = createActionGroup({
    events: {
    'Set External State': props<{ serverUrl: string; monacoLibPath: string }>(),
    },
    source: 'external server url'
});
