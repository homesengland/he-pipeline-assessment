import { createStore } from "@stencil/store";
import StoreStatus from '../constants';

const { state, onChange } = createStore({
  dictionaryGroups: [],
  serverUrl: '',
  workflowDefinitionId: '',
  workflowDefinitionFetchStatus: StoreStatus.Empty
});

onChange("dictionaryGroups", value => { state.dictionaryGroups = value });

onChange("serverUrl", value => { state.serverUrl = value });

onChange("workflowDefinitionId", value => { state.workflowDefinitionId = value });

export default state;


