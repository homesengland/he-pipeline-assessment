//Have to use link here, as importing directly from Node Modules is causing siginificant issues - and for some reason the global script import on a cshtml page
//is also not registering the method.
import { createAuth0Client } from 'https://unpkg.com/@auth0/auth0-spa-js@2.1.3/dist/auth0-spa-js.production.esm.js';
import { Service } from 'https://cdn.jsdelivr.net/npm/axios-middleware@0.4.0/dist/axios-middleware.esm.js';

export class Auth0Plugin {
  options;  //Auth0ClientOptions
  auth0;  //Auth0Client
  token;
  tokenRefreshInterval = null;
  tokenExpirationWarningTimeout = null;

  constructor(options, elsaStudio) {
    let origin = window.location.origin;
    let auth0Params = {
      redirect_uri: origin,
      audience: options.audience,
      scope: 'openid profile email offline_access' // offline_access enables refresh tokens
    };
    this.options = options;
    this.options.authorizationParams = auth0Params;
    this.options.cacheLocation = 'memory'; // or 'localstorage' for cross-tab persistence
    this.options.useRefreshTokens = true; // Enable refresh token rotation
    this.options.useRefreshTokensFallback = true; // Fallback to refresh tokens if iframe fails
    
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
    
    // Start background token refresh if authenticated
    if (isAuthenticated) {
      this.startBackgroundTokenRefresh();
      return;
    }

    // Are we in a redirect back from Auth0 receiving a code?
    const query = window.location.search;
    const hasCode = query.includes("code=");

    if (hasCode) {
      try {
        // Let auth0 SDK handle the code parsing.
        await this.auth0.handleRedirectCallback();

        // Update address to remove code query string.
        window.history.replaceState({}, document.title, "/");
        
        // Start background token refresh after successful login
        this.startBackgroundTokenRefresh();
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

  startBackgroundTokenRefresh = () => {
    // Clear any existing interval
    if (this.tokenRefreshInterval) {
      clearInterval(this.tokenRefreshInterval);
    }

    // Refresh token every 50 minutes (tokens typically expire in 1 hour)
    // This runs regardless of user activity or server communication
    this.tokenRefreshInterval = setInterval(async () => {
      await this.refreshToken();
    }, 50 * 60 * 1000); // 50 minutes

    // Also refresh immediately to get initial token
    this.refreshToken();

    console.log('Background token refresh started - will refresh every 50 minutes');
  };

  refreshToken = async () => {
    if (!this.auth0) return;

    try {
      const isAuthenticated = await this.auth0.isAuthenticated();
      if (!isAuthenticated) {
        console.log('User is not authenticated, stopping token refresh');
        this.stopBackgroundTokenRefresh();
        await this.auth0.loginWithRedirect();
        return;
      }

      // This method uses hidden iframe to communicate with Auth0
      // No communication with your server required!
      this.token = await this.auth0.getTokenSilently({
        cacheMode: 'off' // Force a fresh token from Auth0
      });
      
      console.log('Token refreshed successfully at:', new Date().toLocaleTimeString());
      
    } catch (error) {
      console.error('Token refresh failed:', error);
      
      // Handle specific error cases
      if (error.error === 'login_required' || error.error === 'consent_required') {
        console.log('Re-authentication required');
        this.stopBackgroundTokenRefresh();
        await this.auth0.loginWithRedirect();
      } else if (error.error === 'timeout') {
        console.log('Token refresh timeout, will retry on next interval');
      }
    }
  };

  stopBackgroundTokenRefresh = () => {
    if (this.tokenRefreshInterval) {
      clearInterval(this.tokenRefreshInterval);
      this.tokenRefreshInterval = null;
      console.log('Background token refresh stopped');
    }
  };

  configureAuthMiddleware = async (e) => {
    if (!this.auth0) return;
    
    // Get current token (from cache if available)
    const token = await this.auth0.getTokenSilently().catch(() => null);
    this.token = token;

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

  // Call this when the plugin is destroyed (if needed)
  destroy = () => {
    this.stopBackgroundTokenRefresh();
  };
}

