//Have to use link here, as importing directly from Node Modules is causing siginificant issues - and for some reason the global script import on a cshtml page
//is also not registering the method.
import { createAuth0Client } from 'https://unpkg.com/@auth0/auth0-spa-js@2.1.3/dist/auth0-spa-js.production.esm.js';
import { Service } from 'https://cdn.jsdelivr.net/npm/axios-middleware@0.4.0/dist/axios-middleware.esm.js';
import { EncryptedCache } from './Cache/EncryptedCache.js';

export class Auth0Plugin {
  options;  //Auth0ClientOptions
  auth0;  //Auth0Client
  token;

  constructor(options, elsaStudio) {
    let cache = new EncryptedCache();
    console.log("Cache", cache);
    let origin = window.location.origin;
    console.log("Origin", origin);
    let auth0Params = {
      redirect_uri: origin,
      audience: options.audience,
/*      scope: options.scope*/
    };
    this.options = options;
    this.options.authorizationParams = auth0Params;
    this.options.cacheLocation = 'memory';
    //this.options.cache = cache;
    const eventBus = elsaStudio.eventBus;
    eventBus.on('root.initializing', this.initialize);
    eventBus.on('http-client-created', this.configureAuthMiddleware);
  }

  initialize = async () => {
    const options = this.options;
    const { domain } = options;

    if (!domain || domain.trim().length == 0)
      return;
    this.auth0 = await createAuth0Client(options);
    const isAuthenticated = await this.auth0.isAuthenticated();
    console.log("Is Authenticated", isAuthenticated);
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

    const redirectOptions = {
      redirect_uri: origin
    };

    await this.auth0.loginWithRedirect(redirectOptions);
  };

  configureAuthMiddleware = async (e) => {

    let service = new Service();
    if (e != null && e != undefined && e.service != null && e.service != undefined) {
      service = e.service;
    }
    const auth0 = this.auth0;
    const token = await auth0.getTokenSilently();

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

