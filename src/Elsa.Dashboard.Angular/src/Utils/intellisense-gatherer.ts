import { Auth0Client, Auth0ClientOptions, createAuth0Client } from "@auth0/auth0-spa-js";
import axios, { AxiosInstance, AxiosRequestConfig } from "axios";
import { IntellisenseContext } from "../models";

export class IntellisenseGatherer {
  _httpClient: AxiosInstance;
  _baseUrl: string;
  auth0: Auth0Client;
  options: Auth0ClientOptions;
  context: IntellisenseContext;

  async initialize() {

  }


  async getIntellisense(): Promise<string> {
    return null;
  }

  async tryFetchDefinitions() {

  }

  hasDefinitions() {

  }

  appendGatheredValues() {

  }


  async getJavaScriptTypeDefinitions(workflowDefinitionId: string, context?: IntellisenseContext): Promise<string> {
    return null;

  }

  async createHttpClient(): Promise<AxiosInstance> {
    return null;
  }
}
