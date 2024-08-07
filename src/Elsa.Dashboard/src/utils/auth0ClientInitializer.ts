import state from '../stores/store';
import { createAuth0Client, Auth0Client, Auth0ClientOptions, AuthorizationParams } from '@auth0/auth0-spa-js';


export class Auth0ClientInitializer {
  private auth0: Auth0Client;
  private options: Auth0ClientOptions;

  constructor() {

    let auth0Params: AuthorizationParams = {
      audience: state.audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: state.clientId,
      domain: state.domain,
      cacheLocation: 'memory',
      useRefreshTokens: true

    };

    this.options = auth0Options;
  }

  async getClient(): Promise<Auth0Client> {
    if (state.auth0Client == null) {
      if (state.auth0Client != typeof (Auth0Client)) {
        await this.initialize();
        state.auth0Client = this.auth0;
      }

    }
    return state.auth0Client;
  }


  initialize = async () => {
    const options = this.options;
    const { domain } = options;

    if (!domain || domain.trim().length == 0)
      return;

    this.auth0 = await createAuth0Client(this.options);
    const isAuthenticated = await this.auth0.isAuthenticated();
    state.auth0Client = this.auth0;

    // Nothing to do if authenticated.
    if (isAuthenticated)
      return;
  }
}

