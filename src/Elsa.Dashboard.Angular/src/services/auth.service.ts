import { Injectable } from '@angular/core';
import { createAuth0Client, Auth0Client, Auth0ClientOptions } from '@auth0/auth0-spa-js';
import { Store } from '@ngrx/store';
import { StoreConfig } from 'src/models/store-config';
import { selectStoreConfig } from 'src/store/selectors/app.state.selectors';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private config: StoreConfig;
  private auth0Client: Auth0Client;
  private initialized: boolean = false;

  constructor(store: Store) {
    store.select(selectStoreConfig).subscribe(config => {
      this.config = config;
    });
    this.initializeAuth0Client().then((initiaised) => {
      this.initialized = initiaised;
    });
  }

  private async initializeAuth0Client() {
    this.auth0Client = await createAuth0Client(this.getAuth0ClientOptions());
    if(this.auth0Client.isAuthenticated) {
      return true;
    }
    return false;
  }

  async getAuth0Client(): Promise<Auth0Client> {
    if(!this.initialized) {
      await this.initializeAuth0Client().then((initiaised) => {
        this.initialized = initiaised;
      });
      return this.auth0Client;
    }
    return this.auth0Client;
  }

  getAuth0ClientOptions() : Auth0ClientOptions {
    let auth0Params = {
      audience: this.config.audience,
    };

    let auth0Options: Auth0ClientOptions = {
      authorizationParams: auth0Params,
      clientId: this.config.clientId,
      domain: this.config.domain,
      useRefreshTokens: this.config.useRefreshTokens,
      useRefreshTokensFallback: this.config.useRefreshTokensFallback,
      cacheLocation: "memory"
    };
      return auth0Options;
    }
  }
