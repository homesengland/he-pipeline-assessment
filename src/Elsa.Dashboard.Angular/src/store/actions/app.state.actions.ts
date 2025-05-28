import { createActionGroup, props } from '@ngrx/store';
import { StoreConfig } from '../../models/store-config';
import { DataDictionaryGroup } from '../../models/custom-component-models';
import { ActivityDefinition, ActivityDescriptor, WorkflowStorageDescriptor } from '../../models/domain';

export const AppStateActionGroup = createActionGroup({
  events: {
    'Set External State': props<{ storeConfig: StoreConfig; dataDictionary: Array<DataDictionaryGroup> }>(),
    'Set Store Config': props<{ storeConfig: StoreConfig }>(),
    'Set Workflow Definition Id': props<{ workflowDefinitionId: string }>(),
    'Set Javascript Type Definitions Fetch Status': props<{ javaScriptTypeDefinitionsFetchStatus: string }>(),
    'Set JavaScript Type Definitions': props<{ javaScriptTypeDefinitions: string }>(),
    'Set Data Dictionary Intellisense': props<{ dataDictionaryIntellisense: string }>(),
    'Fetch JavaScript Type Definitions': null,
    'Set Activity Descriptors': props<{ activityDescriptors: Array<ActivityDescriptor> }>(),
    'Set Workflow Storage Descriptors': props<{ workflowStorageDescriptors: Array<WorkflowStorageDescriptor> }>(),
  },
  source: 'external sources in workflow-root',
});
