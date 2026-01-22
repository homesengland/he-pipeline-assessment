export declare const createElsaOauth2Client: (serverUrl: string) => Promise<ElsaOauth2Client>;
export interface ElsaOauth2Client {
  oauth2Api: Oauth2Api;
}
export interface Oauth2Api {
  getUrl(secretId: string): Promise<string>;
}
