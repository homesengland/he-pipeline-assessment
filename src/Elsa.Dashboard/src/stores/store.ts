import { createStore } from "@stencil/store";
import { StoreStatus } from '../constants/constants';

const { state, onChange } = createStore({
  dictionaryGroups: [],
  serverUrl: '',
  audience: '',
  domain: '',
  clientId: '',
  useRefreshToken: true,
  useRefreshTokenFallback: true,
  workflowDefinitionId: '',
  javaScriptTypeDefinitions:'',
  javaScriptTypeDefinitionsFetchStatus: StoreStatus.Empty,
  monacoLibPath: '',
  basePath: '',
  dataDictionaryIntellisense: '',
});

onChange("dictionaryGroups", value => { state.dictionaryGroups = value });

onChange("serverUrl", value => { state.serverUrl = value });

onChange("basePath", value => { state.basePath = value });

onChange("workflowDefinitionId", value => { state.workflowDefinitionId = value });

onChange("serverUrl", value => { state.serverUrl = value });

onChange("audience", value => { state.audience = value });

onChange("domain", value => { state.domain = value });

onChange("clientId", value => { state.clientId = value });

onChange("javaScriptTypeDefinitions", value => { state.javaScriptTypeDefinitions = value });

onChange("useRefreshToken", value => { state.useRefreshToken = value });

onChange("useRefreshTokenFallback", value => { state.useRefreshTokenFallback = value });

onChange("monacoLibPath", value => { state.monacoLibPath = value });

onChange("dataDictionaryIntellisense", value => { state.dataDictionaryIntellisense = value });

export default state;


