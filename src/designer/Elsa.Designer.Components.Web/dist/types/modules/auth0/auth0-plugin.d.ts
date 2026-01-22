import { Auth0ClientOptions } from '@auth0/auth0-spa-js';
import { ElsaPlugin } from "../../services";
export declare class Auth0Plugin implements ElsaPlugin {
  private readonly options;
  private auth0;
  constructor(options: Auth0ClientOptions);
  private initialize;
  private configureAuthMiddleware;
}
