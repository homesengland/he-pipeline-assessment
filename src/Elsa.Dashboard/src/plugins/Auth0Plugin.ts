import { Service } from 'axios-middleware';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
//import state from '../stores/store';


export class Auth0Plugin {
  private auth0: Auth0Client;
  //having to set Eventbus to 'Any' type as we can't afford to import from Elsa.  Slows build time down exponentially.
  private eventBus: any;
  private token: string;
  private options: Auth0ClientOptions;

  constructor(eventBus: any, auth0Options: Auth0ClientOptions) {
    this.eventBus = eventBus;
    this.options = auth0Options;
    this.eventBus.on('http-client-created', e => this.configureAuthMiddleware(e));
  }

  initialize = async () => {
    const options = this.options;
    const { domain } = options;

    if (!domain || domain.trim().length == 0)
      return;
    this.auth0 = await createAuth0Client(options);
    const isAuthenticated = await this.auth0.isAuthenticated();

    // Nothing to do if authenticated.
    if (isAuthenticated)
      return;

    // Are we in a redirect back from Auth0 receiving a code?
    const query = window.location.search;
    const hasCode = query.includes("code=");

    if (hasCode) {
      try {
        // Let auth0 SDK handle the code parsing.
        await this.auth0.handleRedirectCallback();

        // Update address to remove code query string.
        window.history.replaceState({}, document.title, "/");
        return;
      } catch (err) {
        console.log("Error parsing redirect:", err);
        return;
      }
    }

    // Redirect to Auth0 for the user to authenticate themselves.
    const origin = window.location.origin;

    this.options.authorizationParams.redirect_uri = origin;

    //let redirectOptions: RedirectLoginOptions = {
    //}
    //  redirectOptions.authorizationParams = this.options.authorizationParams;
    

    await this.auth0.loginWithRedirect(this.options);
  };

  getToken = async () => {
    if (this.token == null) {
      this.token = await this.auth0.getTokenSilently();
    }
    return this.token;
  }

  configureAuthMiddleware = async (e) => {

    let service = new Service();
    if (e != null && e != undefined && e.service != null && e.service != undefined) {
      service = e.service;
    }
    const token = await this.getToken();

    service.register({
      async onRequest(request) {
        // Get a (cached) access token.
        if (request.data == null) {
          request.data = "{}";
        }
        if (!!token) {
          request.headers = { ...request.headers, 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` };
        }
        return request;
      }
    });
  };

}

//  async isAlreadyInitialized(): Promise<boolean> {
//    if (state.auth0Client != null) {
//      if (state.auth0Client = typeof (Auth0Client)) {
//        return (state.auth0Client as Auth0Client) = await state.auth0Client.isAuthenticated(); 
//      }
//    }
//    return false;
//  }

//}
