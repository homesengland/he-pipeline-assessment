import { createStore } from "@stencil/store";
import { StoreStatus } from '../constants/constants';

const { state, onChange } = createStore({
  dictionaryGroups: [],
  serverUrl: '',
  audience: '',
  domain: '',
  clientId: '',
  useRefreshToken: true,
  workflowDefinitionId: '',
  workflowDefinitionFetchStatus: StoreStatus.Empty,
  javaScriptTypeDefinitions:'',
  javaScriptTypeDefinitionsFetchStatus: StoreStatus.Empty
});

onChange("dictionaryGroups", value => { state.dictionaryGroups = value });

onChange("serverUrl", value => { state.serverUrl = value });

onChange("workflowDefinitionId", value => { state.workflowDefinitionId = value });

onChange("serverUrl", value => { state.serverUrl = value });

onChange("audience", value => { state.audience = value });

onChange("domain", value => { state.domain = value });

onChange("clientId", value => { state.clientId = value });

onChange("javaScriptTypeDefinitions", value => { state.javaScriptTypeDefinitions = value });

onChange("useRefreshToken", value => { state.useRefreshToken = value });

export default state;


