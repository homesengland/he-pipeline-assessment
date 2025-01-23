import { createActionGroup, props } from '@ngrx/store';
import { StoreConfig } from '../../../Models/storeConfig';
import { DataDictionaryGroup } from '../../../Models/custom-component-models';

export const AppStateActionGroup = createActionGroup({
    events: {
    'Set External State': props<{ storeConfig: StoreConfig; dataDictionary: Array<DataDictionaryGroup>; }>(),
    },
    source: 'external server url'
});
