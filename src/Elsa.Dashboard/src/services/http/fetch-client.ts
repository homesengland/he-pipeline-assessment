import { eventBus } from '../event-bus';
import { EventTypes } from '../../models/events';

// Minimal Axios-like response type.
export interface HttpResponse<T = any> {
  status: number;
  headers: Record<string, string>;
  data: T;
}

export interface HttpRequestConfig {
  baseURL?: string;
  method?: string;
  url?: string;
  headers?: Record<string, string>;
  body?: any; // Will be string | FormData
  responseType?: 'blob' | 'json' | 'text';
}

export interface Interceptor {
  onRequest?(config: HttpRequestConfig): Promise<HttpRequestConfig> | HttpRequestConfig;
  onResponse?<T = any>(response: HttpResponse<T>, request: HttpRequestConfig): Promise<HttpResponse<T>> | HttpResponse<T>;
  onError?(error: any, request: HttpRequestConfig): Promise<any> | any;
}

export interface HttpClient {
  get<T = any>(url: string, config?: Omit<HttpRequestConfig, 'url' | 'method'>): Promise<HttpResponse<T>>;
  post<T = any>(url: string, body?: any, config?: Omit<HttpRequestConfig, 'url' | 'method'>): Promise<HttpResponse<T>>;
  put<T = any>(url: string, body?: any, config?: Omit<HttpRequestConfig, 'url' | 'method'>): Promise<HttpResponse<T>>;
  delete<T = any>(url: string, config?: Omit<HttpRequestConfig, 'url' | 'method'>): Promise<HttpResponse<T>>;
  use(interceptor: Interceptor): void;
  baseURL(): string;
}

class FetchClient implements HttpClient {
  private readonly _baseURL: string;
  private interceptors: Interceptor[] = [];

  constructor(baseURL: string) {
    this._baseURL = baseURL;
  }

  use(interceptor: Interceptor) {
    this.interceptors.push(interceptor);
  }

  baseURL() { return this._baseURL; }

  async get<T>(url: string, config: Omit<HttpRequestConfig, 'url' | 'method'> = {}): Promise<HttpResponse<T>> {
    return this.request<T>({ ...config, method: 'GET', url });
  }
  async post<T>(url: string, body?: any, config: Omit<HttpRequestConfig, 'url' | 'method'> = {}): Promise<HttpResponse<T>> {
    return this.request<T>({ ...config, method: 'POST', url, body });
  }
  async put<T>(url: string, body?: any, config: Omit<HttpRequestConfig, 'url' | 'method'> = {}): Promise<HttpResponse<T>> {
    return this.request<T>({ ...config, method: 'PUT', url, body });
  }
  async delete<T>(url: string, config: Omit<HttpRequestConfig, 'url' | 'method'> = {}): Promise<HttpResponse<T>> {
    return this.request<T>({ ...config, method: 'DELETE', url });
  }

  private async request<T>(initialConfig: HttpRequestConfig): Promise<HttpResponse<T>> {
    let config: HttpRequestConfig = { ...initialConfig };

    // Resolve base URL.
    const fullUrl = this._baseURL ? combineUrl(this._baseURL, config.url || '') : (config.url || '');
    config.url = fullUrl;

    // Apply request interceptors sequentially.
    for (const i of this.interceptors) {
      if (i.onRequest) {
        config = await i.onRequest(config);
      }
    }

    const fetchInit: RequestInit = {
      method: config.method,
      headers: config.headers,
      body: needsBody(config.method) ? serializeBody(config.body, config.headers) : undefined,
    };

    let response: Response;
    try {
      response = await fetch(fullUrl, fetchInit);
    } catch (err) {
      for (const i of this.interceptors) {
        if (i.onError) await i.onError(err, config);
      }
      throw err;
    }

    const respHeaders: Record<string, string> = {};
    response.headers.forEach((v, k) => respHeaders[k.toLowerCase()] = v);

    const contentType = respHeaders['content-type'] || '';
    let data: any;
    if (config.responseType === 'blob') {
      data = await response.blob();
    } else if (config.responseType === 'text') {
      data = await response.text();
    } else if (config.responseType === 'json') {
      data = await safeJson(response);
    } else if (contentType.includes('application/json')) {
      data = await safeJson(response);
    } else if (contentType.includes('text/')) {
      data = await response.text();
    } else if (contentType.includes('application/octet-stream') || contentType.includes('application/zip')) {
      data = await response.blob();
    } else {
      data = await response.text(); // fallback
    }

    let httpResponse: HttpResponse<T> = { status: response.status, headers: respHeaders, data };

    for (const i of this.interceptors) {
      if (i.onResponse) {
        httpResponse = await i.onResponse(httpResponse, config);
      }
    }

    if (!response.ok) {
      const error = new Error(`HTTP ${response.status} for ${config.method} ${fullUrl}`);
      for (const i of this.interceptors) {
        if (i.onError) await i.onError(error, config);
      }
      throw error;
    }

    return httpResponse;
  }
}

function needsBody(method?: string) {
  return method === 'POST' || method === 'PUT' || method === 'PATCH';
}

function serializeBody(body: any, headers?: Record<string, string>) {
  if (!body) return undefined;
  if (typeof FormData !== 'undefined' && body instanceof FormData) return body;
  const contentType = headers && Object.keys(headers).find(h => h.toLowerCase() === 'content-type');
  if (contentType && headers![contentType].includes('application/json'))
    return typeof body === 'string' ? body : JSON.stringify(body);
  return body; // send as-is
}

function combineUrl(base: string, path: string) {
  if (!base) return path;
  if (base.endsWith('/') && path.startsWith('/')) return base + path.substring(1);
  if (!base.endsWith('/') && !path.startsWith('/')) return base + '/' + path;
  return base + path;
}

// Factory similar to previous createHttpClient.
export async function createFetchHttpClient(baseAddress: string): Promise<HttpClient> {
  const config: HttpRequestConfig = { baseURL: baseAddress };
  await eventBus.emit(EventTypes.HttpClientConfigCreated, this, { config });
  const client = new FetchClient(baseAddress);
  // Compatibility service wrapper emulating axios-middleware Service.register
  const service = {
    register(interceptor: Interceptor) {
      client.use(interceptor);
    }
  };
  await eventBus.emit(EventTypes.HttpClientCreated, this, { service, httpClient: client });
  return client;
}

async function safeJson(response: Response) {
  try {
    return await response.json();
  } catch (_) {
    return null;
  }
}

