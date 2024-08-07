import * as crypto from 'https://cdnjs.cloudflare.com/ajax/libs/crypto-js/4.2.0/crypto-js.min.js';
export class LocalStorageCache  {

   set(key, entry) {
     localStorage.setItem(key, JSON.stringify(entry));
     console.log("Setting Key");
  }

  get(key) {
    console.log("getting key", key);
    const json = window.localStorage.getItem(key);

    if (!json) return;

    try {
      const payload = JSON.parse(json);
      return payload;
      /* c8 ignore next 3 */
    } catch (e) {
      return;
    }
  }

  remove(key) {
    localStorage.removeItem(key);
  }

  allKeys() {
    return Object.keys(window.localStorage).filter(key =>
      key.startsWith(CACHE_KEY_PREFIX)
    );
  }
}

export const CACHE_KEY_PREFIX = '@@auth0spajs@@';
export const CACHE_KEY_ID_TOKEN_SUFFIX = '@@user@@';
