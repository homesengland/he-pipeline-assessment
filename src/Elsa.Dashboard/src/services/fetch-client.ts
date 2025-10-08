// Centralized Fetch utilities for the Elsa Dashboard
// Provides a light abstraction similar to a subset of Axios features used previously.

export type BaseFetch = (input: string, init?: RequestInit) => Promise<Response>;

export class HttpError extends Error {
  status: number;
  statusText: string;
  url: string;
  body?: string;
  constructor(message: string, res: Response, body?: string) {
    super(message);
    this.status = res.status;
    this.statusText = res.statusText;
    this.url = res.url;
    this.body = body;
  }
}

export interface CreateFetchOptions {
  defaultHeaders?: Record<string, string>;
  onRequest?: (url: string, init: RequestInit) => Promise<[string, RequestInit]> | [string, RequestInit];
  onResponse?: (response: Response) => Promise<Response> | Response;
}

export function createBaseFetch(baseUrl: string, options: CreateFetchOptions = {}): BaseFetch {
  const { defaultHeaders = {}, onRequest, onResponse } = options;
  return async (input: string, init: RequestInit = {}) => {
    const url = input.startsWith('http') ? input : `${baseUrl}${input}`;
    const headers = { ...defaultHeaders, ...(init.headers || {}) } as Record<string, string>;
    let finalInit: RequestInit = { ...init, headers };
    let finalUrl = url;
    if (onRequest) {
      const result = await onRequest(finalUrl, finalInit);
      finalUrl = result[0];
      finalInit = result[1];
    }
    const response = await fetch(finalUrl, finalInit);
    if (onResponse) return await onResponse(response);
    return response;
  };
}

export async function fetchJson<T>(fetchFn: BaseFetch, url: string, init?: RequestInit): Promise<T> {
  const resp = await fetchFn(url, init);
  if (!resp.ok) {
    const body = await safeReadText(resp);
    throw new HttpError(`Request failed (${resp.status})`, resp, body);
  }
  if (resp.status === 204) return undefined as any;
  return await resp.json() as T;
}

export async function sendJson<T>(fetchFn: BaseFetch, method: string, url: string, body?: any, extraInit: RequestInit = {}): Promise<T> {
  const init: RequestInit = {
    method,
    headers: { 'Content-Type': 'application/json', ...(extraInit.headers || {}) },
    body: body != null ? JSON.stringify(body) : undefined,
    ...extraInit,
  };
  return await fetchJson<T>(fetchFn, url, init);
}

export async function fetchBlob(fetchFn: BaseFetch, method: string, url: string, body?: any, extraInit: RequestInit = {}): Promise<Blob> {
  const init: RequestInit = {
    method,
    body,
    ...extraInit,
  };
  const resp = await fetchFn(url, init);
  if (!resp.ok) {
    const text = await safeReadText(resp);
    throw new HttpError(`Blob request failed (${resp.status})`, resp, text);
  }
  return await resp.blob();
}

export async function sendFormData<T>(fetchFn: BaseFetch, url: string, form: FormData, method = 'POST'): Promise<T> {
  const resp = await fetchFn(url, { method, body: form });
  if (!resp.ok) {
    const body = await safeReadText(resp);
    throw new HttpError(`FormData request failed (${resp.status})`, resp, body);
  }
  if (resp.status === 204) return undefined as any;
  // Some endpoints may return plain text; attempt JSON parse, fallback to text.
  const text = await resp.text();
  if (!text) return undefined as any;
  try { return JSON.parse(text) as T; } catch { return text as any; }
}

export function withAuth(fetchFn: BaseFetch, tokenProvider: () => Promise<string | null | undefined>): BaseFetch {
  return async (input: string, init: RequestInit = {}) => {
    const token = await tokenProvider();
    const headers = { ...(init.headers || {}) } as Record<string, string>;
    if (token)
      headers['Authorization'] = `Bearer ${token}`;
    return fetchFn(input, { ...init, headers });
  };
}

async function safeReadText(resp: Response): Promise<string | undefined> {
  try { return await resp.text(); } catch { return undefined; }
}
