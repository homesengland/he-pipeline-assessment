//Have to use link here, as importing directly from Node Modules is causing siginificant issues - and for some reason the global script import on a cshtml page
//is also not registering the method.
import { createAuth0Client } from 'https://unpkg.com/@auth0/auth0-spa-js@2.1.3/dist/auth0-spa-js.production.esm.js';
// Removed axios-middleware dependency; we now wrap the emitted fetch client instead.

export class Auth0Plugin {
  options;  //Auth0ClientOptions
  auth0;  //Auth0Client
  token;

  constructor(options, elsaStudio) {
    let origin = window.location.origin;
    let auth0Params = {
      redirect_uri: origin,
      audience: options.audience,
/*      scope: options.scope*/
    };
    this.options = options;
    this.options.authorizationParams = auth0Params;
    this.options.cacheLocation = 'memory';
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
    if (!e)
      return;

    const auth0 = this.auth0;
    // Prefer new interceptor registration if provided.
    if (e.registerRequestInterceptor) {
      e.registerRequestInterceptor(async (url, init) => {
        let token = null;
        try { token = await auth0.getTokenSilently(); } catch { /* ignore */ }
        if (token) {
          const headers = { ...(init.headers || {}), 'Authorization': `Bearer ${token}` };
          return [url, { ...init, headers }];
        }
        return [url, init];
      });
      return;
    }
    // Legacy: wrap fetch if directly provided (older payload shape)
    if (e.fetch) {
      const originalFetch = e.fetch;
      e.fetch = async (input, init = {}) => {
        let token = null;
        try { token = await auth0.getTokenSilently(); } catch { }
        const headers = { ...(init.headers || {}) };
        if (token) headers['Authorization'] = `Bearer ${token}`;
        return originalFetch(input, { ...init, headers });
      };
      return;
    }

    // Backwards compatibility: if an axios-middleware service is still supplied, attempt to register.
    if (e.service && typeof e.service.register === 'function') {
      const token = await auth0.getTokenSilently().catch(() => null);
      e.service.register({
        async onRequest(request) {
          if (request.data == null)
            request.data = "{}";
            if (token) {
              request.headers = { ...request.headers, 'Authorization': `Bearer ${token}`, 'Content-Type': `application/json; charset=UTF-8` };
            }
          return request;
        }
      });
    }
  };

}

