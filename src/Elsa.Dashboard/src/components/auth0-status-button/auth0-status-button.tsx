import { Component, h, State, Method } from '@stencil/core';
import { Auth0Client } from '@auth0/auth0-spa-js';
import { GetAuth0Options } from '../../http-clients/http-client-services';

@Component({
  tag: 'auth0-status-button',
  shadow: false,
})
export class Auth0StatusButton {
  
  @State() isTokenValid: boolean = false;
  @State() isChecking: boolean = true;
  
  private auth0Client: Auth0Client;
  private checkInterval: number;

  async componentWillLoad() {
    await this.initializeAuth0();
    await this.checkTokenStatus();
    // Check token status every 60 seconds
    this.checkInterval = window.setInterval(() => this.checkTokenStatus(), 60000);
  }

  disconnectedCallback() {
    if (this.checkInterval) {
      clearInterval(this.checkInterval);
    }
  }

  private async initializeAuth0() {
    try {
      const auth0Options = GetAuth0Options();
      this.auth0Client = new Auth0Client(auth0Options);
    } catch (error) {
      console.error('Failed to initialize Auth0 client:', error);
      this.isTokenValid = false;
      this.isChecking = false;
    }
  }

  @Method()
  async checkTokenStatus(): Promise<boolean> {
    if (!this.auth0Client) {
      this.isTokenValid = false;
      this.isChecking = false;
      return false;
    }

    try {
      this.isChecking = true;
      const isAuthenticated = await this.auth0Client.isAuthenticated();
      
      if (isAuthenticated) {
        // Try to get token silently - this will fail if token is expired
        await this.auth0Client.getTokenSilently();
        this.isTokenValid = true;
      } else {
        this.isTokenValid = false;
      }
    } catch (error) {
      console.warn('Token validation failed:', error);
      this.isTokenValid = false;
    } finally {
      this.isChecking = false;
    }

    return this.isTokenValid;
  }

  private async handleButtonClick() {
    if (this.isTokenValid) {
      // Token is valid, optionally show details or do nothing
      return;
    }

    // Token is invalid, attempt to get a new one
    try {
      this.isChecking = true;
      await this.auth0Client.loginWithPopup();
      await this.checkTokenStatus();
    } catch (error) {
      console.error('Failed to authenticate:', error);
      // Fallback to redirect if popup fails
      await this.auth0Client.loginWithRedirect();
    }
  }

  render() {
    const buttonClass = this.isChecking 
      ? 'auth0-status-button checking'
      : this.isTokenValid 
        ? 'auth0-status-button valid' 
        : 'auth0-status-button invalid';

    const buttonTitle = this.isChecking
      ? 'Checking authentication...'
      : this.isTokenValid
        ? 'Authentication valid'
        : 'Authentication expired - Click to re-authenticate';

    const statusText = this.isChecking
      ? '⟳'
      : this.isTokenValid
        ? '✓'
        : '✗';

    return (
      <button
        class={buttonClass}
        title={buttonTitle}
        onClick={() => this.handleButtonClick()}
        disabled={this.isChecking}
      >
        <span class="status-icon">{statusText}</span>
        <span class="status-text">
          {this.isChecking ? 'Checking...' : this.isTokenValid ? 'Authenticated' : 'Re-authenticate'}
        </span>
      </button>
    );
  }
}
