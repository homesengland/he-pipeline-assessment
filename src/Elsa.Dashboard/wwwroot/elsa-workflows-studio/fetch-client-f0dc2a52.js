import { e as eventBus } from './event-bus-5d6f3774.js';
import { E as EventTypes } from './events-d0aab14a.js';

class FetchClient {
  constructor(baseURL) {
    this.interceptors = [];
    this._baseURL = baseURL;
  }
  use(interceptor) {
    this.interceptors.push(interceptor);
  }
  baseURL() { return this._baseURL; }
  async get(url, config = {}) {
    return this.request(Object.assign(Object.assign({}, config), { method: 'GET', url }));
  }
  async post(url, body, config = {}) {
    return this.request(Object.assign(Object.assign({}, config), { method: 'POST', url, body }));
  }
  async put(url, body, config = {}) {
    return this.request(Object.assign(Object.assign({}, config), { method: 'PUT', url, body }));
  }
  async delete(url, config = {}) {
    return this.request(Object.assign(Object.assign({}, config), { method: 'DELETE', url }));
  }
  async request(initialConfig) {
    let config = Object.assign({}, initialConfig);
    // Resolve base URL.
    const fullUrl = this._baseURL ? combineUrl(this._baseURL, config.url || '') : (config.url || '');
    config.url = fullUrl;
    // Apply request interceptors sequentially.
    for (const i of this.interceptors) {
      if (i.onRequest) {
        config = await i.onRequest(config);
      }
    }
    const fetchInit = {
      method: config.method,
      headers: config.headers,
      body: needsBody(config.method) ? serializeBody(config.body, config.headers) : undefined,
    };
    let response;
    try {
      response = await fetch(fullUrl, fetchInit);
    }
    catch (err) {
      for (const i of this.interceptors) {
        if (i.onError)
          await i.onError(err, config);
      }
      throw err;
    }
    const respHeaders = {};
    response.headers.forEach((v, k) => respHeaders[k.toLowerCase()] = v);
    const contentType = respHeaders['content-type'] || '';
    let data;
    if (config.responseType === 'blob') {
      data = await response.blob();
    }
    else if (config.responseType === 'text') {
      data = await response.text();
    }
    else if (config.responseType === 'json') {
      data = await safeJson(response);
    }
    else if (contentType.includes('application/json')) {
      data = await safeJson(response);
    }
    else if (contentType.includes('text/')) {
      data = await response.text();
    }
    else if (contentType.includes('application/octet-stream') || contentType.includes('application/zip')) {
      data = await response.blob();
    }
    else {
      data = await response.text(); // fallback
    }
    let httpResponse = { status: response.status, headers: respHeaders, data };
    for (const i of this.interceptors) {
      if (i.onResponse) {
        httpResponse = await i.onResponse(httpResponse, config);
      }
    }
    if (!response.ok) {
      const error = new Error(`HTTP ${response.status} for ${config.method} ${fullUrl}`);
      for (const i of this.interceptors) {
        if (i.onError)
          await i.onError(error, config);
      }
      throw error;
    }
    return httpResponse;
  }
}
function needsBody(method) {
  return method === 'POST' || method === 'PUT' || method === 'PATCH';
}
function serializeBody(body, headers) {
  if (!body)
    return undefined;
  if (typeof FormData !== 'undefined' && body instanceof FormData)
    return body;
  const contentType = headers && Object.keys(headers).find(h => h.toLowerCase() === 'content-type');
  if (contentType && headers[contentType].includes('application/json'))
    return typeof body === 'string' ? body : JSON.stringify(body);
  return body; // send as-is
}
function combineUrl(base, path) {
  if (!base)
    return path;
  if (base.endsWith('/') && path.startsWith('/'))
    return base + path.substring(1);
  if (!base.endsWith('/') && !path.startsWith('/'))
    return base + '/' + path;
  return base + path;
}
// Factory similar to previous createHttpClient.
async function createFetchHttpClient(baseAddress) {
  const config = { baseURL: baseAddress };
  await eventBus.emit(EventTypes.HttpClientConfigCreated, this, { config });
  const client = new FetchClient(baseAddress);
  // Compatibility service wrapper emulating axios-middleware Service.register
  const service = {
    register(interceptor) {
      client.use(interceptor);
    }
  };
  await eventBus.emit(EventTypes.HttpClientCreated, this, { service, httpClient: client });
  return client;
}
async function safeJson(response) {
  try {
    return await response.json();
  }
  catch (_) {
    return null;
  }
}

export { createFetchHttpClient as c };
