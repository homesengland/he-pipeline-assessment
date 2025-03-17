import { createActionGroup, props } from '@ngrx/store';
import { StoreConfig } from '../../../models/storeConfig';
import { DataDictionaryGroup } from '../../../models/custom-component-models';

export const AppStateActionGroup = createActionGroup({
  events: {
    'Set External State': props<{ storeConfig: StoreConfig; dataDictionary: Array<DataDictionaryGroup> }>(),
    'Set Store Config': props<{ storeConfig: StoreConfig }>(),
    'Set Workflow Definition Id': props<{ workflowDefinitionId: string }>(),
    'Set Javascript Type Definitions Fetch Status': props<{ javaScriptTypeDefinitionsFetchStatus: string }>(),
    'Set JavaScript Type Definitions': props<{ javaScriptTypeDefinitions: string }>(),
    'Set Data Dictionary Intellisense': props<{ dataDictionaryIntellisense: string }>(),
  },
  source: 'external sources in workflow-root',
});
