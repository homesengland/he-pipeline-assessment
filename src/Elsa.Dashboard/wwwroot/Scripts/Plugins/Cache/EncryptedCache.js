//import { CryptoJs } from 'https://cdnjs.cloudflare.com/ajax/libs/crypto-js/4.2.0/crypto-js.min.js';
export class EncryptedCache {

  constructor() {
    this.encryptStorage = new EncryptStorage('happypotato');
  }

  set(key, entry) {
    this.encryptStorage.setItem(key, JSON.stringify(entry));
  }

  get(key) {
    const json = this.encryptStorage.getItem(key);

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
    this.encryptStorage.removeItem(key);
  }

  allKeys() {
    let keys = Object.keys(this.encryptStorage.storage).filter(key =>
      key.startsWith(CACHE_KEY_PREFIX));
    return keys;
  }
}

export const CACHE_KEY_PREFIX = '@@auth0spajs@@';
export const CACHE_KEY_ID_TOKEN_SUFFIX = '@@user@@';

