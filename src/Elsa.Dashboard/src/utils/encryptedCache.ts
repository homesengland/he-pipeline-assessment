import { EncryptStorage } from 'encrypt-storage';
export class EncryptedCache {

  private encryptStorage : any
  constructor() {
    this.encryptStorage = new EncryptStorage('happypotato');
    console.log("encryptStorage", this.encryptStorage);
  }

  set(key, entry) {
    this.encryptStorage.setItem(key, JSON.stringify(entry));
    console.log("Setting Key");
  }

  get(key) {
    console.log("getting key", key);
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
    return Object.keys(this.encryptStorage).filter(key =>
      key.startsWith(CACHE_KEY_PREFIX)
    );
  }
}

export const CACHE_KEY_PREFIX = '@@auth0spajs@@';
export const CACHE_KEY_ID_TOKEN_SUFFIX = '@@user@@';

