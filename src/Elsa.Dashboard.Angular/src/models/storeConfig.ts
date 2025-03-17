import { Auth0Client } from "@auth0/auth0-spa-js";

export class StoreConfig {
  serverUrl: string = "";
  audience: string = "";
  clientId: string = "";
  domain: string = "";
  useRefreshTokens: boolean = true;
  useRefreshTokensFallback: boolean = true;
  monacoLibPath: string = "";
  dataDictionaryIntellisense: string = "";
  auth0Client: Auth0Client = null;
  basePath: string = "";
}
