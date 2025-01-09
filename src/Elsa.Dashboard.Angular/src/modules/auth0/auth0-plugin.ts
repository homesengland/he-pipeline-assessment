import { createAuth0Client, Auth0Client, Auth0ClientOptions, RedirectLoginOptions } from '@auth0/auth0-spa-js';
import { Service } from 'axios-middleware';
import { WorkflowPlugin } from '../../services/workflow-plugin';
import { eventBus } from '../../services/event-bus';
import { EventTypes } from "../../models";
import { Browser } from '@capacitor/browser';

export class Auth0Plugin implements WorkflowPlugin {
  private readonly options: Auth0ClientOptions
  private auth0: Auth0Client;

  constructor(options: Auth0ClientOptions) {
    this.options = options;

    eventBus.on(EventTypes.Root.Initializing, this.initialize)
    eventBus.on(EventTypes.HttpClientCreated, this.configureAuthMiddleware);
  }

  private initialize = async () => {
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
    let origin = window.location.origin;

    const redirectOptions: RedirectLoginOptions = {
      openUrl(origin) {async () => await Browser.open({ url: origin }) }
      };

    await this.auth0.loginWithRedirect(redirectOptions);
};

  private configureAuthMiddleware = async (e: any) => {

    const service: Service = e.service;
    const auth0 = this.auth0;

    service.register({
      async onRequest(request) {

        // Get a (cached) access token.
        const token = await auth0.getTokenSilently();

        if (!!token)
          request.headers = { ...request.headers, 'Authorization': `Bearer ${token}` };
        return request;
      }
    });
  };
}
