import { c as createStore } from './index-0d4e8807.js';
import { a as StoreStatus } from './constants-6ea82f24.js';

const { state, onChange } = createStore({
  dictionaryGroups: [],
  serverUrl: '',
  audience: '',
  domain: '',
  clientId: '',
  useRefreshToken: true,
  useRefreshTokenFallback: true,
  workflowDefinitionId: '',
  javaScriptTypeDefinitions: '',
  javaScriptTypeDefinitionsFetchStatus: StoreStatus.Empty,
  monacoLibPath: '',
  basePath: '',
  dataDictionaryIntellisense: '',
  auth0Client: null
});
onChange("dictionaryGroups", value => { state.dictionaryGroups = value; });
onChange("serverUrl", value => { state.serverUrl = value; });
onChange("basePath", value => { state.basePath = value; });
onChange("workflowDefinitionId", value => { state.workflowDefinitionId = value; });
onChange("serverUrl", value => { state.serverUrl = value; });
onChange("audience", value => { state.audience = value; });
onChange("domain", value => { state.domain = value; });
onChange("clientId", value => { state.clientId = value; });
onChange("javaScriptTypeDefinitions", value => { state.javaScriptTypeDefinitions = value; });
onChange("useRefreshToken", value => { state.useRefreshToken = value; });
onChange("useRefreshTokenFallback", value => { state.useRefreshTokenFallback = value; });
onChange("monacoLibPath", value => { state.monacoLibPath = value; });
onChange("dataDictionaryIntellisense", value => { state.dataDictionaryIntellisense = value; });
onChange("auth0Client", value => { state.auth0Client = value; });

export { state as s };
