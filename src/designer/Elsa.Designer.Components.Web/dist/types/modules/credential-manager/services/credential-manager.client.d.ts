import { SecretModel } from "../models/secret.model";
export declare const createElsaSecretsClient: (serverUrl: string) => Promise<ElsaSecretsClient>;
export interface ElsaSecretsClient {
  secretsApi: SecretsApi;
}
export interface SecretsApi {
  list(): Promise<Array<SecretModel>>;
  save(request: SecretModel): Promise<SecretModel>;
  delete(secretId: string): Promise<void>;
}
