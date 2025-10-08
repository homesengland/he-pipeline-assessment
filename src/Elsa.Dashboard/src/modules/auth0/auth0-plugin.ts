import { createAuth0Client, Auth0Client, Auth0ClientOptions, RedirectLoginOptions} from '@auth0/auth0-spa-js';
import {eventBus, ElsaPlugin} from "../../services";
import { EventTypes } from "../../models";
import { Browser } from '@capacitor/browser';

export class Auth0Plugin implements ElsaPlugin {
  private readonly options: Auth0ClientOptions
  private auth0: Auth0Client;

  constructor(options: Auth0ClientOptions) {
    this.options = options;

    eventBus.on(EventTypes.Root.Initializing, this.initialize)
  eventBus.on(EventTypes.HttpClientCreated, this.configureAuthMiddleware);
  }

  private initialize = async () => {
    const options = this.options;
    const {domain} = options;

    if(!domain || domain.trim().length == 0)
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

    const redirectOptions: RedirectLoginOptions = {
      openUrl(origin) { async () => await Browser.open({ url: origin }) }
    };

    await this.auth0.loginWithRedirect(redirectOptions);
  };

  private configureAuthMiddleware = async (e: any) => {
    const register = e.registerRequestInterceptor;
    if(!register)
      return; // Older event payload; no interceptor mechanism
    const auth0 = this.auth0;
    register(async (url, init) => {
      const token = await auth0.getTokenSilently().catch(() => null);
      if(token) {
        const headers = { ...(init.headers || {}) as Record<string,string>, 'Authorization': `Bearer ${token}` };
        return [url, { ...init, headers }];
      }
      return [url, init];
    });
  };
}
