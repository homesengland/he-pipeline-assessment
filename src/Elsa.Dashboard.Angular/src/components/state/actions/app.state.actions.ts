import { createActionGroup, props } from '@ngrx/store';

export const AppStateActionGroup = createActionGroup({
  events: {
    'Set External State': props<{ serverUrl: string; monacoLibPath: string; auth0Domain: string; auth0ClientId: string; auth0Audience: string; basePath: string }>(),
    },
    source: 'external server url'
});
