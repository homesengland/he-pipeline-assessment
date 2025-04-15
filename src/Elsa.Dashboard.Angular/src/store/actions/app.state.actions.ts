import { createActionGroup, props } from '@ngrx/store';
import { StoreConfig } from '../../models/store-config';
import { DataDictionaryGroup } from '../../models/custom-component-models';
import { StoreStatus } from 'src/models/constants';
import { IntellisenseContext } from 'src/models';

export const AppStateActionGroup = createActionGroup({
    events: {
    'Set External State': props<{ storeConfig: StoreConfig; dataDictionary: Array<DataDictionaryGroup>; }>(),
    'Set Store Config': props < { storeConfig: StoreConfig; }>(),
    'Set Workflow Definition Id': props<{ workflowDefinitionId: string; }>(),
    'Set Javascript Type Definitions Fetch Status': props<{ javaScriptTypeDefinitionsFetchStatus: string; }>(),
    //'Set JavaScript Type Definitions': props<{ javaScriptTypeDefinitions: string; }>(),
    'Set Data Dictionary Intellisense': props<{ dataDictionaryIntellisense: string; }>(),
    'Set JavaScript Type Definitions Fetch Status': props<{ status: StoreStatus }>(),
    'Fetch JavaScript Type Definitions': null,
    'Set JavaScript Type Definitions': props<{ javaScriptTypeDefinitions: string }>(),
    },
    source: 'external sources in workflow-root'
});
